using Component.Infrastructure;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Application.Job
{
    public class SimpleJob : IJob
    {
        #region IJob 成员

        Task IJob.Execute(IJobExecutionContext context)
        {
            return Task.Factory.StartNew(() => {
                LogFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).Info("Execute SimpleJob");
            });
        }

        #endregion
    }
}
