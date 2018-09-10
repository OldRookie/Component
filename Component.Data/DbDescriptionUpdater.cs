﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Component.Data
{
    public class DbDescriptionUpdater<TContext>
      where TContext : DbContext
    {
        public DbDescriptionUpdater(TContext context)
        {
            this.context = context;
        }

        Type contextType;
        TContext context;
        DbTransaction transaction;
        public void UpdateDatabaseDescriptions()
        {
            contextType = typeof(TContext);
            var types = context.Model.GetEntityTypes();
            transaction = null;
            try
            {
                context.Database.GetDbConnection().Open();
                transaction = context.Database.GetDbConnection().BeginTransaction();
                foreach (var type in types)
                {
                    SetTableDescriptions(type.ClrType);
                }
                transaction.Commit();
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();
                throw;
            }
            finally
            {
                if (context.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
                    context.Database.GetDbConnection().Close();
            }
        }

        private void SetTableDescriptions(Type tableType)
        {
            string tableName = context.GetTableName(tableType);

            var tableAttrs = tableType.GetCustomAttributes(typeof(TableAttribute), false);
            if (tableAttrs.Length > 0)
                tableName = ((TableAttribute)tableAttrs[0]).Name;

            var tableNameAtrr = tableType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (tableNameAtrr.Length > 0)
            {
                SetTableDescription(tableName, ((DescriptionAttribute)tableNameAtrr[0]).Description);
            }
            foreach (var prop in tableType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    continue;
                var attrs = prop.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attrs.Length > 0)
                    SetColumnDescription(tableName, prop.Name, ((DisplayAttribute)attrs[0]).Name);
            }
        }

        private void SetColumnDescription(string tableName, string columnName, string description)
        {
            string strGetDesc = "select [value] from fn_listextendedproperty('MS_Description','schema','dbo','table',N'" + tableName + "','column',null) where objname = N'" + columnName + "';";
            var prevDesc = RunSqlScalar(strGetDesc);
            if (prevDesc == null)
            {
                RunSql(@"EXEC sp_addextendedproperty 
                   @name = N'MS_Description', @value = @desc,
                   @level0type = N'Schema', @level0name = 'dbo',
                   @level1type = N'Table',  @level1name = @table,
                   @level2type = N'Column', @level2name = @column;",
                                                       new SqlParameter("@table", tableName),
                                                       new SqlParameter("@column", columnName),
                                                       new SqlParameter("@desc", description));
            }
            else
            {
                RunSql(@"EXEC sp_updateextendedproperty 
                   @name = N'MS_Description', @value = @desc,
                   @level0type = N'Schema', @level0name = 'dbo',
                   @level1type = N'Table',  @level1name = @table,
                   @level2type = N'Column', @level2name = @column;",
                                                       new SqlParameter("@table", tableName),
                                                       new SqlParameter("@column", columnName),
                                                       new SqlParameter("@desc", description));
            }
        }

        private void SetTableDescription(string tableName, string description)
        {
            string strGetDesc = "select [value] from fn_listextendedproperty('MS_Description','schema','dbo','table',N'" + tableName + "',null,null);";
            var prevDesc = RunSqlScalar(strGetDesc);
            if (prevDesc == null)
            {
                RunSql(@"EXEC sp_addextendedproperty 
                   @name = N'MS_Description', @value = @desc,
                   @level0type = N'Schema', @level0name = 'dbo',
                   @level1type = N'Table',  @level1name = @table;",
                                                       new SqlParameter("@table", tableName),
                                                       new SqlParameter("@desc", description));
            }
            else
            {
                RunSql(@"EXEC sp_updateextendedproperty 
                   @name = N'MS_Description', @value = @desc,
                   @level0type = N'Schema', @level0name = 'dbo',
                   @level1type = N'Table',  @level1name = @table;",
                                                       new SqlParameter("@table", tableName),
                                                       new SqlParameter("@desc", description));
            }
        }

        DbCommand CreateCommand(string cmdText, params SqlParameter[] parameters)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Transaction = transaction;
            foreach (var p in parameters)
                cmd.Parameters.Add(p);
            return cmd;
        }

        void RunSql(string cmdText, params SqlParameter[] parameters)
        {
            var cmd = CreateCommand(cmdText, parameters);
            cmd.ExecuteNonQuery();
        }

        object RunSqlScalar(string cmdText, params SqlParameter[] parameters)
        {
            var cmd = CreateCommand(cmdText, parameters);
            return cmd.ExecuteScalar();
        }
    }
    public static class ReflectionUtil
    {
        public static bool InheritsOrImplements(this Type child, Type parent)
        {
            parent = ResolveGenericTypeDefinition(parent);

            var currentChild = child.IsGenericType
                                   ? child.GetGenericTypeDefinition()
                                   : child;

            while (currentChild != typeof(object))
            {
                if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                    return true;

                currentChild = currentChild.BaseType != null
                               && currentChild.BaseType.IsGenericType
                                   ? currentChild.BaseType.GetGenericTypeDefinition()
                                   : currentChild.BaseType;

                if (currentChild == null)
                    return false;
            }
            return false;
        }

        private static bool HasAnyInterfaces(Type parent, Type child)
        {
            return child.GetInterfaces()
                .Any(childInterface =>
                {
                    var currentInterface = childInterface.IsGenericType
                        ? childInterface.GetGenericTypeDefinition()
                        : childInterface;

                    return currentInterface == parent;
                });
        }

        private static Type ResolveGenericTypeDefinition(Type parent)
        {
            var shouldUseGenericType = true;
            if (parent.IsGenericType && parent.GetGenericTypeDefinition() != parent)
                shouldUseGenericType = false;

            if (parent.IsGenericType && shouldUseGenericType)
                parent = parent.GetGenericTypeDefinition();
            return parent;
        }
    }

    public static class ContextExtensions {
        public static string GetTableName(this DbContext context, Type tableType)
        {
            var mapping = context.Model.FindEntityType(tableType).Relational();
            var schema = mapping.Schema;
            var tableName = mapping.TableName;

            return tableName;
        }
    }
}
