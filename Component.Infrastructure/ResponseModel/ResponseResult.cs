using System;
using System.Collections.Generic;

namespace Component.Infrastructure
{
    public class ResponseResult<T>
    {
        public ResponseResult()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsValid { get { return ErrorMessages.Count == 0; } }
        public T Data { get; set; }
        public ResultCode Code { get; set; }
        public List<string> ErrorMessages { get; set; }
        public string MessageSummary { get; set; }
        public ValidationResultInfo ValidationResultInfo { get; set; }
    }

    public class ResponseResultBase
    {
        public ResponseResultBase()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsValid { get { return ErrorMessages.Count == 0; } }
        public Object Data { get; set; }
        public ResultCode Code { get; set; }
        public List<string> ErrorMessages { get; set; }
        public string MessageSummary { get; set; }
        public ValidationResultInfo ValidationResultInfo { get; set; }
    }

    public enum ResultCode
    {
        Success = 1,
        Failtrue = 0,
    }
}