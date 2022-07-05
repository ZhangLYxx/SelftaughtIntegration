namespace Integration.ToolKits
{
    public class ApiResult
    {
        /// <summary>
        /// code
        /// </summary>
        public ResultCode Code { get; set; } = ResultCode.Ok;

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        public static ApiResult Ok()
        {
            return new ApiResult { Msg = "ok" };
        }

        public static ApiResult Fail(string msg, ResultCode code = ResultCode.InvalidRequest)
        {
            return new ApiResult { Msg = msg, Code = code };
        }

        public static ApiResult<T> Fail<T>(string msg, ResultCode code = ResultCode.InvalidRequest)
        {
            return new ApiResult<T> { Msg = msg, Code = code };
        }

        public static ApiResult<T> Ok<T>(T data)
        {
            return new ApiResult<T> { Msg = "ok", Data = data };
        }

    }

    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    public enum ResultCode
    {
        /// <summary>
        /// 正常
        /// </summary>
        Ok = 200,


        /// <summary>
        /// 业务逻辑错误
        /// </summary>
        InvalidRequest = 340,


        /// <summary>
        /// 参数错误
        /// </summary>
        InvalidParameter = 400,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 禁止访问（权限不足）
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 无效的请求
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 请求过于频繁
        /// </summary>
        TooManyRequests = 429,

        /// <summary>
        /// 服务器错误
        /// </summary>
        ServerError = 500
    }
}