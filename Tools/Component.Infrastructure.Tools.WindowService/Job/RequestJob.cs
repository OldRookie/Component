using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.Tools.Job.WindowService
{
    public class RequestJob : IJob
    {
        private static readonly ILog logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string PropertyCommand = "command";
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    JobDataMap data = context.MergedJobDataMap;

                    string endpoint = data.GetString("endpoint");

                    string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
                    Uri url = new Uri(baseUrl + endpoint);

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(60);
                        HttpResponseMessage msg = httpClient.GetAsync(url).Result;
                        string result = string.Empty;
                        if (msg.StatusCode == HttpStatusCode.OK)
                        {
                            result = msg.Content.ReadAsStringAsync().Result;
                            logger.Info($"{context.JobDetail.Description}execute successfully!");
                        }
                        else
                        {
                            logger.Info($"{context.JobDetail.Description}:{url} execute  failure!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    throw;
                }
            });
        }
    }
}
