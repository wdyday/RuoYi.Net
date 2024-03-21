using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using RuoYi.Framework.Logging;
using SqlSugar;

namespace RuoYi.Framework.Interceptors
{
    /// <summary>
    /// 在要被拦截的方法上标注 [Transactional], 
    /// 类需要是public类，方法如果需要拦截就是[虚方法]，
    /// 支持异步方法，因为动态代理是动态生成被代理的类的动态子类实现的。
    /// </summary>
    public class TransactionalAttribute : AbstractInterceptorAttribute
    {
        [FromServiceContext]
        public ISqlSugarClient Db { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await Db.Ado.BeginTranAsync();

                await next(context);

                await Db.Ado.CommitTranAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Transactional Error", ex);
                await Db.Ado.RollbackTranAsync();

                throw ex;
            }
        }
    }
}
