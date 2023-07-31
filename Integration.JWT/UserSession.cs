using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.JWT
{
    public class UserSession
    {
        /// <summary>
        /// ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>

        public string UserName { get; set; }
        /// <summary>
        /// 权限
        /// </summary>

        public string[] Roles { get; set; }

        public string AccessToken { get; set; }
    }

    /// <summary>
    /// 用户 Session
    /// </summary>
    public class MemberSession : UserSession
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }
    }

    public class ManagerSession : UserSession
    {

    }

}
