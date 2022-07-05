using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.ToolKits
{
    public class BizException : Exception
    {
        public BizException(string msg, ResultCode code = ResultCode.InvalidRequest) : base(msg)
        {
            Code = code;
        }
        public ResultCode Code { get; set; }
    }
}
