﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Component.Data
{
    public class DBContextManager
    {
        public static object syncObject = new object();
        public static DbContext _dbContext;

        private static ConcurrentDictionary<int, DbContext> ThreadSPDB
            = new ConcurrentDictionary<int, DbContext>();

        public static DbContext DBContext
        {
            get
            {
                DbContext dbContext = null;
                if (HttpContext.Current != null)
                {
                    dbContext = HttpContext.Current.Items["_EntityContext"] as DbContext;

                    if (dbContext == null)
                    {
                        lock (syncObject)
                        {
                            if (dbContext == null)
                            {
                                dbContext = new ComponentDBContext();
                                HttpContext.Current.Items["_EntityContext"] = dbContext;
                            }
                        }
                    }
                    return dbContext;
                }
                else
                {
                    if (!ThreadSPDB.ContainsKey(Thread.CurrentThread.ManagedThreadId)
                        || ThreadSPDB[Thread.CurrentThread.ManagedThreadId] == null)
                    {
                        lock (syncObject)
                        {
                            if (!ThreadSPDB.ContainsKey(Thread.CurrentThread.ManagedThreadId)
                                || ThreadSPDB[Thread.CurrentThread.ManagedThreadId] == null)
                            {
                                ThreadSPDB.TryAdd(Thread.CurrentThread.ManagedThreadId, new ComponentDBContext());
                            }
                        }
                    }
                    dbContext = ThreadSPDB[Thread.CurrentThread.ManagedThreadId];
                    return dbContext;
                }
            }
        }

        public static void Commit()
        {
            try
            {
                DBContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (ThreadSPDB.ContainsKey(Thread.CurrentThread.ManagedThreadId)
                    && ThreadSPDB[Thread.CurrentThread.ManagedThreadId] == DBContext)
                {
                    DbContext dbContext = null;
                    ThreadSPDB.TryRemove(Thread.CurrentThread.ManagedThreadId, out dbContext);
                    dbContext.Dispose();
                }
            }
        }

        public static void Dispose()
        {
            DBContext.Dispose();
            if (ThreadSPDB.ContainsKey(Thread.CurrentThread.ManagedThreadId)
                   && ThreadSPDB[Thread.CurrentThread.ManagedThreadId] == DBContext)
            {
                DbContext dbContext = null;
                ThreadSPDB.TryRemove(Thread.CurrentThread.ManagedThreadId, out dbContext);
                dbContext.Dispose();
            }
        }
    }

    public static class HttpContext
    {
        public static IServiceProvider ServiceProvider;

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

    }
}
