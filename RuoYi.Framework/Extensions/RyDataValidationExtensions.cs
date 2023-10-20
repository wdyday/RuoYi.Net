using Furion.DataValidation;
using RuoYi.Framework.Exceptions;

namespace RuoYi.Framework.Extensions
{
    public static class RyDataValidationExtensions
    {
        /// <summary>
        /// 拓展方法，验证类类型对象
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="validateAllProperties">是否验证所有属性</param>
        public static void Validate(this object obj, bool validateAllProperties = true)
        {
            DataValidator.TryValidateObject(obj, validateAllProperties).ThrowValidateFailedModel();
        }

        /// <summary>
        /// 直接抛出异常信息
        /// </summary>
        /// <param name="dataValidationResult"></param>
        public static void ThrowValidateFailedModel(this DataValidationResult dataValidationResult)
        {
            if (!dataValidationResult.IsValid)
            {
                // 解析验证失败消息，输出统一格式
                //var validationFailMessage =
                //      dataValidationResult.ValidationResults
                //      .Select(u => new {
                //          MemberNames = u.MemberNames.Any() ? u.MemberNames : new[] { $"{dataValidationResult.MemberOrValue}" },
                //          u.ErrorMessage
                //      })
                //      .OrderBy(u => u.MemberNames.First())
                //      .GroupBy(u => u.MemberNames.First())
                //      .ToDictionary(x => x.Key, u => u.Select(c => c.ErrorMessage).ToArray());

                var validationFailMessage = string.Join(";", dataValidationResult.ValidationResults.Select(u => u.ErrorMessage).Distinct().ToList());

                // 抛出验证失败异常
                throw new ServiceException(validationFailMessage);
            }
        }
    }
}
