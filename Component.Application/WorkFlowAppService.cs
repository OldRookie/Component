using Component.Data;
using Component.Domain.Service;
using Component.Infrastructure;
using Component.Model.Domain;
using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Application
{
    public class WorkFlowAppService
    {
        WFServiceFactory _wfServiceFactory = new WFServiceFactory();

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="workFlowTypeCode"></param>
        /// <param name="data"></param>
        /// <param name="workFlowForm"></param>
        /// <returns></returns>
        public ResponseResultBase Start(WorkFlowTypeCode workFlowTypeCode, object data, BaseWorkFlowFormViewModel workFlowForm)
        {
            var result = new ResponseResultBase();
            var wf = _wfServiceFactory.Get(workFlowTypeCode);
            if (wf != null)
            {
                result = wf
                .Execute(new WorkflowOperation()
                {
                    Data = data,
                    WorkFlowForm = workFlowForm,
                    ActionCode = WorkFlowActionCode.Start
                });
                DBContextManager.Commit();
            }

            return result;
        }

        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        public ResponseResultBase Handle(Guid id, string data, WorkFlowActionCode? actionCode = null)
        {
            var wf = _wfServiceFactory.Get(id);
            var result = new ResponseResultBase();
            if (wf != null)
            {
                result = wf
                .Execute(new WorkflowOperation()
                {
                    Data = data,
                    ActionCode = actionCode.Value
                });
                if (result.Code == ResultCode.Success)
                {
                    DBContextManager.Commit();
                }
            }

            return result;
        }

    }
}
