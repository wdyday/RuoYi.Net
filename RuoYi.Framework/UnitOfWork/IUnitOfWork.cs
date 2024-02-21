using Microsoft.AspNetCore.Mvc.Filters;

namespace RuoYi.Framework.DatabaseAccessor;

/// <summary>
/// 工作单元依赖接口
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 开启工作单元处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="unitOfWork"></param>
    void BeginTransaction(FilterContext context, UnitOfWorkAttribute unitOfWork);

    /// <summary>
    /// 提交工作单元处理
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="unitOfWork"></param>
    void CommitTransaction(FilterContext resultContext, UnitOfWorkAttribute unitOfWork);

    /// <summary>
    /// 回滚工作单元处理
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="unitOfWork"></param>
    void RollbackTransaction(FilterContext resultContext, UnitOfWorkAttribute unitOfWork);

    /// <summary>
    /// 执行完毕（无论成功失败）
    /// </summary>
    /// <param name="context"></param>
    /// <param name="resultContext"></param>
    void OnCompleted(FilterContext context, FilterContext resultContext);
}