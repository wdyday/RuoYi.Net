using RuoYi.Common.Constants;
using RuoYi.Common.Utils;
using RuoYi.Framework.Exceptions;
using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  字典类型表 Service
///  author ruoyi
///  date   2023-09-04 17:49:59
/// </summary>
public class SysDictTypeService : BaseService<SysDictType, SysDictTypeDto>, ITransient
{
    private readonly ILogger<SysDictTypeService> _logger;
    private readonly SysDictTypeRepository _sysDictTypeRepository;
    private readonly SysDictDataRepository _sysDictDataRepository;

    public SysDictTypeService(ILogger<SysDictTypeService> logger,
        SysDictTypeRepository sysDictTypeRepository,
        SysDictDataRepository sysDictDataRepository)
    {
        BaseRepo = sysDictTypeRepository;

        _logger = logger;
        _sysDictTypeRepository = sysDictTypeRepository;
        _sysDictDataRepository = sysDictDataRepository;

        this.LoadingDictCache();
    }

    /// <summary>
    /// 所有字典类型
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysDictType>> SelectDictTypeAllAsync()
    {
        return await _sysDictTypeRepository.GetListAsync(new SysDictTypeDto());
    }

    /// <summary>
    /// 根据字典类型查询字典数据
    /// </summary>
    /// <param name="dictType">字典类型</param>
    public async Task<List<SysDictData>> SelectDictDataByTypeAsync(string dictType)
    {
        List<SysDictData> dictDatas = DictUtils.GetDictCache(dictType);
        if (dictDatas.IsNotEmpty())
        {
            return dictDatas;
        }
        dictDatas = await _sysDictDataRepository.SelectDictDataByTypeAsync(dictType);
        if (dictDatas.IsNotEmpty())
        {
            DictUtils.SetDictCache(dictType, dictDatas);
            return dictDatas;
        }
        return null!;
    }

    /// <summary>
    /// 查询 字典类型表 详情
    /// </summary>
    public async Task<SysDictType> GetAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.DictId == id);
        return entity;
    }

    /// <summary>
    /// 校验字典类型称是否唯一
    /// </summary>
    /// <param name="dict">字典类型</param>
    public async Task<bool> CheckDictTypeUniqueAsync(SysDictTypeDto dict)
    {
        long dictId = dict.DictId ?? 0;
        SysDictType dictType = await _sysDictTypeRepository.CheckDictTypeUniqueAsync(dict.DictType!);
        if (dictType != null && dictType.DictId != dictId)
        {
            return UserConstants.NOT_UNIQUE;
        }
        return UserConstants.UNIQUE;
    }

    /// <summary>
    /// 新增保存字典类型信息
    /// </summary>
    public async Task<bool> InsertDictTypeAsync(SysDictTypeDto dict)
    {
        bool success = await _sysDictTypeRepository.InsertAsync(dict);
        if (success)
        {
            DictUtils.SetDictCache(dict.DictType!, null!);
        }
        return success;
    }

    /// <summary>
    /// 修改保存字典类型信息
    /// </summary>
    public async Task<int> UpdateDictTypeAsync(SysDictTypeDto dict)
    {
        SysDictType oldDict = await this.GetAsync(dict.DictId);
        await _sysDictDataRepository.UpdateDictDataTypeAsync(oldDict.DictType!, dict.DictType!);
        int row = await _sysDictTypeRepository.UpdateAsync(dict);
        if (row > 0)
        {
            List<SysDictData> dictDatas = await _sysDictDataRepository.SelectDictDataByTypeAsync(dict.DictType!);
            DictUtils.SetDictCache(dict.DictType!, dictDatas);
        }
        return row;
    }

    /// <summary>
    /// 批量删除字典类型信息
    /// </summary>
    public async Task DeleteDictTypeByIdsAsync(long[] dictIds)
    {
        foreach (long dictId in dictIds)
        {
            SysDictType dictType = await this.GetAsync(dictId);
            if (await _sysDictDataRepository.CountDictDataByTypeAsync(dictType.DictType!) > 0)
            {
                throw new ServiceException($"{dictType.DictName}已分配,不能删除");
            }
            await _sysDictTypeRepository.DeleteAsync(dictId);
            DictUtils.RemoveDictCache(dictType.DictType!);
        }
    }

    /// <summary>
    /// 重置字典缓存数据
    /// </summary>
    public void ResetDictCache()
    {
        ClearDictCache();
        LoadingDictCache();
    }

    /// <summary>
    /// 清空字典缓存数据
    /// </summary>
    public void ClearDictCache()
    {
        DictUtils.ClearDictCache();
    }

    /// <summary>
    /// 加载字典缓存数据
    /// </summary>
    public void LoadingDictCache()
    {
        SysDictDataDto dto = new SysDictDataDto { Status = Status.Enabled };
        var dictDatas = _sysDictDataRepository.GetList(dto);
        var dictTypes = dictDatas.Select(d => d.DictType).Distinct().ToList();
        foreach (string dictType in dictTypes)
        {
            DictUtils.SetDictCache(dictType!, dictDatas.Where(d => d.DictType == dictType).ToList());
        }
    }
}