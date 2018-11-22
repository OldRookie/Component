using Component.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Domain
{
    public class BaseWorkFlowFormViewModel
    {
        public BaseWorkFlowFormViewModel()
        {
            Approvers = new List<string>();
        }
        public virtual string Reason { get; set; }

        public virtual string AttachmentIds { get; set; }

        public virtual Guid ReferenceEntityId { get; set; }

        public int FlowInstanceVersionID { get; set; }

        public List<string> Approvers { get; set; }

        public Guid? WorkFlowId { get; set; }

        public virtual string Approver { get; set; }
    }

    public class WorkflowOperation
    {
        public Guid? OAWFId { get; set; }

        public object Data { get; set; }

        public BaseWorkFlowFormViewModel WorkFlowForm { get; set; }

        public WorkFlowActionCode ActionCode { get; set; }

        public Func<WorkflowOperation, ResponseResultBase> CallBack { get; set; }
    }
    public enum WorkFlowActionCode
    {
        Start
    }
}
