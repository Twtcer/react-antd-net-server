using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Reflection;
using System.Linq;

namespace ReactAntdServer.Model.Dto
{
    #region Interface
    public interface IResult
    {
        /// <summary>
        /// 结果状态码
        /// </summary>
        ResultCode Code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <example>操作成功</example>
        string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        bool Success { get; }
    }

    public interface IResult<T> : IResult
    {
        /// <summary>
        /// 返回的附带数据
        /// </summary>
        T Data { get; set; }
    }
    #endregion 

    #region Implement

    public class ResultWrapper : IResult
    {
        private string _message;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success => Code == ResultCode.Ok;

        /// <summary>
        /// 结果码
        /// </summary>
        public ResultCode Code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return _message ?? Code.DisplayName(); }
            set { _message = value; }
        }

        /// <summary>
        /// 返回结果，默认成功
        /// </summary>
        public ResultWrapper()
        {
            Code = ResultCode.Ok;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示信息</param>
        public ResultWrapper(ResultCode code, string message = null)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 返回指定code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultWrapper FromCode(ResultCode code, string message = null)
        {
            return new ResultWrapper(code, message);
        }

        /// <summary>
        ///返回错误信息 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ResultWrapper FromError(string message, ResultCode code = ResultCode.Fail)
        {
            return new ResultWrapper(code, message);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultWrapper Ok(string message = null)
        {
            return new ResultWrapper(ResultCode.Ok, message);
        }

        /// <summary>
        /// 返回指定code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultWrapper<T> FromCode<T>(ResultCode code, string message = null)
        {
            return new ResultWrapper<T>(code, message);
        }

        /// <summary>
        ///返回错误信息 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ResultWrapper<T> FromError<T>(string message, ResultCode code = ResultCode.Fail)
        {
            return new ResultWrapper<T>(code, message);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultWrapper<T> Ok<T>(string message = null)
        {
            return new ResultWrapper<T>(ResultCode.Ok, message);
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultWrapper<T> FromData<T>(T data)
        {
            return new ResultWrapper<T>(data);
        }

        /// <summary>
        /// 返回数据和提示信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultWrapper<T> FromData<T>(T data, string message)
        {
            return new ResultWrapper<T>(ResultCode.Ok, message, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResultWrapper<T> Ok<T>(T data)
        {
            return FromData(data);
        }

    }

    /// <summary>
    /// 返回数据包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultWrapper<T> : ResultWrapper, IResult<T>
    {
        public ResultWrapper()
        {
            Code = ResultCode.Ok;
        }

        public ResultWrapper(T data) : base(ResultCode.Ok)
        {
            Data = data;
        }

        public ResultWrapper(ResultCode code, string message = null) : base(code, message)
        {

        }

        public ResultWrapper(ResultCode code, string message = null, T data = default(T)) : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 返回业务数据
        /// </summary>
        public T Data { get; set; }
    }

    #endregion

    #region Model & d Enum
    /// <summary>
    /// api return code enum
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 操作成功
        ///</summary>
        [Display(Name = "操作成功")]
        Ok = 1,

        /// <summary>
        /// 操作失败
        ///</summary>
        [Display(Name = "操作失败")]
        Fail = 11,

        /// <summary>
        /// 登陆失败
        ///</summary>
        [Display(Name = "登陆失败")]
        LoginFail = 12,

        /// <summary>
        /// 没有该数据
        ///</summary>
        [Display(Name = "没有数据")]
        NoRecord = 13,

        /// <summary>
        /// 用户不存在
        ///</summary>
        [Display(Name = "用户不存在")]
        NoSuchUser = 14,

        /// <summary>
        /// 未登录
        ///</summary>
        [Display(Name = "未登录")]
        Unauthorized = 20,

        /// <summary>
        /// 未授权
        /// </summary>
        [Display(Name = "未授权")]
        Forbidden = 21,

        /// <summary>
        /// 无效Token
        /// </summary>
        [Display(Name = "无效Token")]
        InvalidToken = 22,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Display(Name = "参数验证失败")]
        InvalidData = 23,

        /// <summary>
        /// 无效用户
        /// </summary>
        [Display(Name = "无效用户")]
        InvalidUser = 24
    }

    public enum DisplayProperty
    {
        /// <summary>
        /// 名称
        /// </summary>
        Name,

        /// <summary>
        /// 短名称
        /// </summary>
        ShortName,

        /// <summary>
        /// 分组名称
        /// </summary>
        GroupName,

        /// <summary>
        /// 说明
        /// </summary>
        Description,

        /// <summary>
        /// 排序
        /// </summary>
        Order,

        /// <summary>
        /// 水印信息
        /// </summary>
        Prompt,
    }
    #endregion

    #region Utils
    public static class Extentions
    {
        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayName(this Enum val)
        {
            return val.Display(DisplayProperty.Name) as string;
        }

        /// <summary>
        /// 获取枚举短名称说明
        /// </summary>
        public static string DisplayShortName(this Enum val)
        {
            return val.Display(DisplayProperty.ShortName) as string;
        }

        /// <summary>
        /// 获取枚举水印信息
        /// </summary>
        public static string DisplayPrompt(this Enum val)
        {
            return val.Display(DisplayProperty.Prompt) as string;
        }

        /// <summary>
        /// 获取枚举备注
        /// </summary>
        public static string DisplayDescription(this Enum val)
        {
            return val.Display(DisplayProperty.Description) as string;
        }

        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static object Display(this Enum val, DisplayProperty property)
        {
            var enumType = val.GetType();

            var str = val.ToString();

            if (enumType.GetCustomAttribute<FlagsAttribute>() != null && str.Contains(","))
            {
                var array = str.Split(',').Select(o => o.Trim());

                var result = array.Aggregate("", (s, s1) =>
                {
                    var f = enumType.GetField(s1);

                    if (f != null)
                    {　　　　　　　　　　　　　　//MethodInfo的扩展，方法在下面
                        var text = f.Display(property);
                        return string.IsNullOrEmpty(s) ? text.ToString() : $"{s},{text}";
                    }

                    return s;
                });

                return string.IsNullOrEmpty(result) ? null : result;
            }

            var field = enumType.GetField(str);
            if (field != null)
            {
                return field.Display(property);
            }

            return null;
        }

        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static object Display(this MemberInfo memberInfo, DisplayProperty property)
        {
            if (memberInfo == null) return null;

            var display = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (display != null)
            {
                switch (property)
                {
                    case DisplayProperty.Name:
                        return display.GetName();
                    case DisplayProperty.ShortName:
                        return display.GetShortName();
                    case DisplayProperty.GroupName:
                        return display.GetGroupName();
                    case DisplayProperty.Description:
                        return display.GetDescription();
                    case DisplayProperty.Order:
                        return display.GetOrder();
                    case DisplayProperty.Prompt:
                        return display.GetPrompt();
                }
            }

            return null;
        }
    }
    #endregion
}
