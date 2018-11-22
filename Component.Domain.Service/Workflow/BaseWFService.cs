using Component.Data.Repository;
using Component.Infrastructure;
using Component.Model.Domain;
using Component.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Domain.Service.Workflow
{
    public class BaseWFService
    {
        private WorkFlowRepository _workflowRepository = new WorkFlowRepository();

        /// <summary>
        /// 流程类型
        /// </summary>
        public virtual WorkFlowTypeCode WorkFlowTypeCode { get; }

        /// <summary>
        /// 流程信息
        /// </summary>
        public WorkFlow WorkFlowInfo { get; set; }

        public Guid WorkFlowId { get; set; }

        public BaseWFService(WorkFlow workFlow = null)
        {
            this.WorkFlowInfo = workFlow;
            if (workFlow != null)
            {
                WorkFlowId = workFlow.Id;
            }

        }

        /// <summary>
        /// 流程表单信息
        /// </summary>
        protected object FormInfo { get; set; }

        #region 启动流程
        /// <summary>
        /// 开启流程
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected virtual ResponseResultBase Start(WorkflowOperation workflowOperation)
        {
            ResponseResultBase result = new ResponseResultBase();
            var validtionResult = ValidateStart(workflowOperation);
            if (validtionResult.IsValid)
            {
                workflowOperation.WorkFlowForm.WorkFlowId = Guid.NewGuid();
                WorkFlowId = workflowOperation.WorkFlowForm.WorkFlowId.Value;
                if (true)
                {
                    result.Code = ResultCode.Success;
                    FormInfo = workflowOperation.Data;
                    var workFlow = BuildWorkFlow(workflowOperation);
                    _workflowRepository.Add(workFlow);
                    this.WorkFlowInfo = workFlow;
                }
                else
                {
                    result.Code = ResultCode.Failtrue;
                }
            }
            else
            {
                result.Code = ResultCode.Failtrue;
                result.ErrorMessages = validtionResult.Messages;
            }

            return result;
        }

        /// <summary>
        /// 验证流程数据
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected virtual ValidationResultBaseInfo ValidateStartData(WorkflowOperation workflowOperation)
        {
            var validationResultBaseInfo = new ValidationResultBaseInfo();
            return validationResultBaseInfo;
        }

        /// <summary>
        /// 验证开启流程
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected ValidationResultBaseInfo ValidateStart(WorkflowOperation workflowOperation)
        {
            var validationResultBaseInfo = new ValidationResultBaseInfo();
            var vm = workflowOperation.WorkFlowForm as BaseWorkFlowFormViewModel;
            if (this._workflowRepository.GetActiveWorkFlow(vm.ReferenceEntityId) != null)
            {
                validationResultBaseInfo.Messages.Add("已经在流程中");
            }
            else
            {
                validationResultBaseInfo = ValidateStartData(workflowOperation);
            }
            return validationResultBaseInfo;
        }

        #region 构建本地流程数据
        /// <summary>
        /// 构造流程实体数据
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected virtual WorkFlow BuildWorkFlow(WorkflowOperation workflowOperation)
        {
            var wf = new WorkFlow();
            wf.Id = workflowOperation.WorkFlowForm.WorkFlowId.Value;
            wf.WorkFlowType = WorkFlowTypeCode;
            wf.Status = WorkFlowStatusCode.InProgress;
            wf.CreatedUserId = IdentityInfoHelper.CurrentIdentityInfo.Id;
            wf.CreatedDate = DateTime.Now;
            wf.ModifiedUserId = IdentityInfoHelper.CurrentIdentityInfo.Id;
            wf.ModifiedDate = DateTime.Now;
            wf.BaseWorkFlowForm = new List<BaseWorkFlowForm>();
            wf.BaseWorkFlowForm.Add(BuildWorkFlowForm(wf));
            return wf;
        }

        /// <summary>
        /// 构建流程表单
        /// </summary>
        /// <param name="workFlow"></param>
        /// <returns></returns>
        protected virtual BaseWorkFlowForm BuildWorkFlowForm(WorkFlow workFlow)
        {
            BaseWorkFlowForm form = null;
            if (FormInfo != null)
            {
                var vm = FormInfo as BaseWorkFlowFormViewModel;
                form = vm.AutoMapTo<BaseWorkFlowForm>();
                form.Id = Guid.NewGuid();
                form.WorkFlowId = workFlow.Id;

                var referenceEntity = new WorkFlowFormReferenceEntity();
                referenceEntity.Id = Guid.NewGuid();
                referenceEntity.EntityId = vm.ReferenceEntityId;
                referenceEntity.EntityState = FormEntityStateCode.Create;
                referenceEntity.WorkFlowFormId = form.Id;
                form.WorkFlowFormReferenceEntity.Add(referenceEntity);
            }
            return form;
        }

        /// <summary>
        /// 构建流程实体表单数据
        /// </summary>
        /// <typeparam name="TForm"></typeparam>
        /// <param name="workFlow"></param>
        /// <returns></returns>
        protected BaseWorkFlowForm BuildWorkFlowEntityChangeForm<TForm>(WorkFlow workFlow) where
           TForm : BaseWorkFlowFormViewModel
        {
            BaseWorkFlowForm form = null;
            if (FormInfo != null)
            {
                var vm = FormInfo as TForm;
                form = vm.AutoMapTo<BaseWorkFlowForm>();
                form.Id = Guid.NewGuid();
                form.WorkFlowId = workFlow.Id;

                var referenceEntity = new WorkFlowFormReferenceEntity();
                referenceEntity.Id = Guid.NewGuid();
                referenceEntity.EntityId = vm.ReferenceEntityId;
                referenceEntity.EntityState = FormEntityStateCode.Update;
                referenceEntity.Value = vm.AutoMapTo<TForm>().ToJson();
                referenceEntity.WorkFlowFormId = form.Id;
                form.WorkFlowFormReferenceEntity.Add(referenceEntity);
            }
            return form;
        }
        #endregion
        #endregion

        #region 执行流程
        /// <summary>
        /// 执行流程
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        public ResponseResultBase Execute(WorkflowOperation workflowOperation)
        {
            //根据ActionCode反射获得需要执行的方法
            var obj = this.GetType().GetMethod(workflowOperation.ActionCode.ToString(),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod)
                .Invoke(this, new object[] { workflowOperation });
            return obj as ResponseResultBase;
        }

        /// <summary>
        /// 审核拒绝
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected virtual ResponseResultBase Reject(WorkflowOperation workflowOperation)
        {
            ResponseResultBase result = new ResponseResultBase();
            this.WorkFlowInfo.Status = WorkFlowStatusCode.Rejected;
            _workflowRepository.Update(WorkFlowInfo);
            result.Code = ResultCode.Success;
            return result;
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="workflowOperation"></param>
        /// <returns></returns>
        protected virtual ResponseResultBase Agree(WorkflowOperation workflowOperation)
        {
            ResponseResultBase result = new ResponseResultBase();
            WorkFlowInfo.Status = WorkFlowStatusCode.Completed;
            _workflowRepository.Update(WorkFlowInfo);
            result.Code = ResultCode.Success;
            return result;
        }
        #endregion

    }
}
