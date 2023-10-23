using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Furion.Logging;
using SqlSugar;

namespace RuoYi.Framework.Interceptors
{
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
            }
        }
    }
}
