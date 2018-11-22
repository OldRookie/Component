using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Model.Entity
{
    public class WorkFlow : BaseEntity
    {
        public WorkFlowStatusCode Status { get; set; }

        public WorkFlowStatusCode? PrevStatus { get; set; }

        public Guid OAWorkFlowId { get; set; }

        /// <summary>
        /// OA流程结果
        /// </summary>
        public string OAWorkFlowResult { get; set; }

        /// <summary>
        /// 流程类型
        /// </summary>
        public WorkFlowTypeCode WorkFlowType { get; set; }

        [ForeignKey("CreatedUserId")]
        public virtual User CreatedUser { get; set; }
        public Guid CreatedUserId { get; set; }

        [ForeignKey("ModifiedUserId")]
        public virtual User ModifiedUser { get; set; }
        public Guid ModifiedUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual List<BaseWorkFlowForm> BaseWorkFlowForm { get; set; }
        public Guid Id { get; set; }
    }

    public enum WorkFlowTypeCode
    {
    }

    public class BaseWorkFlowForm : BaseEntity
    {
        public BaseWorkFlowForm()
        {
            WorkFlowFormReferenceEntity = new List<WorkFlowFormReferenceEntity>();
        }
        public string Reason { get; set; }

        public string AttachmentIds { get; set; }

        /// <summary>
        /// 流程关联实体数据
        /// </summary>
        public virtual List<WorkFlowFormReferenceEntity> WorkFlowFormReferenceEntity { get; set; }

        public Guid WorkFlowId { get; set; }

        [ForeignKey("WorkFlowId")]
        public virtual WorkFlow WorkFlow { get; set; }
        public Guid Id { get; set; }
    }


    /// <summary>
    /// 流程关联实体数据
    /// </summary>
    public class WorkFlowFormReferenceEntity : BaseEntity
    {
        public string Value { get; set; }

        public FormEntityStateCode EntityState { get; set; }

        [MaxLength(256)]
        public string EntityName { get; set; }

        public Guid EntityId { get; set; }

        public Guid WorkFlowFormId { get; set; }

        [ForeignKey("WorkFlowFormId")]
        public virtual BaseWorkFlowForm BaseWorkFlowForm { get; set; }
        public Guid Id { get; set; }
    }

    public enum FormEntityStateCode
    {
        Update,
        Create
    }
}
