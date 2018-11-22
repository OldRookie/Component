using Component.Data.Repository;
using Component.Domain.Service.Workflow;
using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Domain.Service
{
    public class WFServiceFactory
    {
        private WorkFlowRepository _workflowRepository = new WorkFlowRepository();

        public WFServiceFactory()
        {

        }
        private Dictionary<WorkFlowTypeCode, Type> wfServices = new Dictionary<WorkFlowTypeCode, Type>();
        public BaseWFService Get(WorkFlowTypeCode workFlowTypeCode)
        {
            return (BaseWFService)Activator.CreateInstance(wfServices[workFlowTypeCode], null);
        }

        public BaseWFService Get(Guid oaWfId)
        {
            var wf = _workflowRepository
                .FirstOrDefault(x => x.OAWorkFlowId == oaWfId && x.Status == WorkFlowStatusCode.InProgress,
                includeEntities: new List<string>() { "BaseWorkFlowForm.WorkFlowFormReferenceEntity" });
            if (wf != null)
            {
                return (BaseWFService)Activator
                    .CreateInstance(wfServices[wf.WorkFlowType], wf);
            }
            else
            {
                return null;
            }
        }
    }
}
