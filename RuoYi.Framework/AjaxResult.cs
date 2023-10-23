using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace RuoYi.Framework
{
    public class AjaxResult : ConcurrentDictionary<string, object>
    {
        private const string Msg_Success = "操作成功.";
        private const string Msg_Error = "系统错误, 请联系管理员.";

        /** 状态码 */
        public const string CODE_TAG = "code";
        /** 返回内容 */
        public const string MSG_TAG = "msg";
        /** 数据对象 */
        public const string DATA_TAG = "data";

        /**
       * 初始化一个新创建的 AjaxResult 对象，使其表示一个空消息。
       */
        public AjaxResult()
        {
        }

        /**
         * 初始化一个新创建的 AjaxResult 对象
         * 
         * @param code 状态码
         * @param msg 返回内容
         */
        public AjaxResult(int code, string msg)
        {
            this.TryAdd(CODE_TAG, code);
            this.TryAdd(MSG_TAG, msg);
        }

        /**
         * 初始化一个新创建的 AjaxResult 对象
         * 
         * @param code 状态码
         * @param msg 返回内容
         * @param data 数据对象
         */
        public AjaxResult(int code, string msg, object? data)
        {
            this.TryAdd(CODE_TAG, code);
            this.TryAdd(MSG_TAG, msg);
            if (data != null)
            {
                this.TryAdd(DATA_TAG, data);
            }
        }

        public AjaxResult Add(string key, object value)
        {
            this.TryAdd(key, value);
            return this;
        }

        public static AjaxResult Success(string msg, object? data)
        {
            return new AjaxResult(StatusCodes.Status200OK, msg, data);
        }

        public static AjaxResult Success(object data)
        {
            return Success(Msg_Success, data);
        }

        public static AjaxResult Success(string msg)
        {
            return Success(msg, null);
        }

        public static AjaxResult Success()
        {
            return Success(Msg_Success, null);
        }


        public static AjaxResult Error(int code, string msg)
        {
            return new AjaxResult(code, msg, null);
        }

        public static AjaxResult Error(string msg, object? data)
        {
            return new AjaxResult(StatusCodes.Status500InternalServerError, msg, data);
        }

        public static AjaxResult Error(string msg)
        {
            return Error(msg, null);
        }

        public static AjaxResult Error()
        {
            return Error(Msg_Error, null);
        }
    }
}
