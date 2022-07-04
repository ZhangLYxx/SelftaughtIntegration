using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public interface IIdentityParser
    {
        /// <summary>
        /// 认证数据转换为字符
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        string Parse(UserSession body);

        /// <summary>
        /// 字符转换为认证识别数据
        /// </summary>
        /// <param name="identityStr"></param>
        /// <returns></returns>
        UserSession Parse(string identityStr);
    }
}
