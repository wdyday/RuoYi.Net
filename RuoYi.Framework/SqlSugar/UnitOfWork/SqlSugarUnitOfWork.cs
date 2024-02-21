using Microsoft.AspNetCore.Mvc.Filters;
using RuoYi.Framework.DatabaseAccessor;

namespace SqlSugar
{
    /// <summary>
    /// SqlSugar 工作单元实现
    /// [UnitOfWork] 特性只能用于控制器的 Action 中，一旦贴了 [UnitOfWork] 特性后，那么该请求自动启用工作单元模式，要么成功，要么失败。
    /// </summary>
    public sealed class SqlSugarUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// SqlSugar 对象
        /// </summary>
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        public SqlSugarUnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 开启工作单元处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWork"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void BeginTransaction(FilterContext context, UnitOfWorkAttribute unitOfWork)
        {
            _sqlSugarClient.AsTenant().BeginTran();
        }

        /// <summary>
        /// 提交工作单元处理
        /// </summary>
        /// <param name="resultContext"></param>
        /// <param name="unitOfWork"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CommitTransaction(FilterContext resultContext, UnitOfWorkAttribute unitOfWork)
        {
            _sqlSugarClient.AsTenant().CommitTran();
        }

        /// <summary>
        /// 回滚工作单元处理
        /// </summary>
        /// <param name="resultContext"></param>
        /// <param name="unitOfWork"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RollbackTransaction(FilterContext resultContext, UnitOfWorkAttribute unitOfWork)
        {
            _sqlSugarClient.AsTenant().RollbackTran();
        }

        /// <summary>
        /// 执行完毕（无论成功失败）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resultContext"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnCompleted(FilterContext context, FilterContext resultContext)
        {
            _sqlSugarClient.Dispose();
        }
    }
}
