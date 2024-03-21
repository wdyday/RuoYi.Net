using RuoYi.Common.Utils;
using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  字典数据表 Service
///  author ruoyi
///  date   2023-09-04 17:49:59
/// </summary>
public class SysDictDataService : BaseService<SysDictData, SysDictDataDto>, ITransient
{
    private readonly ILogger<SysDictDataService> _logger;
    private readonly SysDictDataRepository _sysDictDataRepository;

    public SysDictDataService(ILogger<SysDictDataService> logger,
        SysDictDataRepository sysDictDataRepository)
    {
        BaseRepo = sysDictDataRepository;

        _logger = logger;
        _sysDictDataRepository = sysDictDataRepository;
    }

    /// <summary>
    /// 查询 字典数据表 详情
    /// </summary>
    public async Task<SysDictData> GetAsync(long dictCode)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.DictCode == dictCode);
        return entity;
    }

    /// <summary>
    /// 新增保存字典数据信息
    /// </summary>
    public async Task<bool> InsertDictDataAsync(SysDictDataDto data)
    {
        bool success = await _sysDictDataRepository.InsertAsync(data);
        if (success)
        {
            List<SysDictData> dictDatas = await _sysDictDataRepository.SelectDictDataByTypeAsync(data.DictType!);
            DictUtils.SetDictCache(data.DictType!, dictDatas);
        }
        return success;
    }

    /// <summary>
    /// 修改保存字典数据信息
    /// </summary>
    public async Task<int> UpdateDictDataAsync(SysDictDataDto data)
    {
        int row = await _sysDictDataRepository.UpdateAsync(data);
        if (row > 0)
        {
            List<SysDictData> dictDatas = await _sysDictDataRepository.SelectDictDataByTypeAsync(data.DictType!);
            DictUtils.SetDictCache(data.DictType!, dictDatas);
        }
        return row;
    }

    /// <summary>
    /// 批量删除字典数据信息
    /// </summary>
    public async Task DeleteDictDataByIdsAsync(long[] dictCodes)
    {
        foreach (long dictCode in dictCodes)
        {
            SysDictData data = await GetAsync(dictCode);
            await _sysDictDataRepository.DeleteAsync(dictCode);
            List<SysDictData> dictDatas = await _sysDictDataRepository.SelectDictDataByTypeAsync(data.DictType!);
            DictUtils.SetDictCache(data.DictType!, dictDatas);
        }
    }
}