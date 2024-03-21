using RuoYi.Data;
using RuoYi.Data.Entities;
using RuoYi.Framework;
using RuoYi.Framework.Cache;
using RuoYi.Framework.Extensions;
using RuoYi.Framework.Utils;
using System.Text;

namespace RuoYi.Common.Utils
{
    public class DictUtils
    {
        // 分隔符
        public static string SEPARATOR = ",";

        /// <summary>
        /// 设置字典缓存
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="dictDatas">字典数据列表</param>
        public static void SetDictCache(string key, List<SysDictData> dictDatas)
        {
            App.GetService<ICache>().Set(GetCacheKey(key), dictDatas);
        }

        /// <summary>
        /// 获取字典缓存
        /// </summary>
        /// <param name="key">参数键</param>
        /// <returns>字典数据列表</returns>
        public static List<SysDictData> GetDictCache(string key)
        {
            return App.GetService<ICache>().Get<List<SysDictData>>(GetCacheKey(key));
        }

        /// <summary>
        /// 根据字典类型和字典值获取字典标签
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictValue">字典值</param>
        /// <returns>字典标签</returns>
        public static string GetDictLabel(string dictType, string dictValue)
        {
            return GetDictLabel(dictType, dictValue, SEPARATOR);
        }

        /// <summary>
        /// 根据字典类型和字典标签获取字典值
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictValue">字典值</param>
        /// <returns>字典值</returns>
        public static string GetDictValue(string dictType, string dictLabel)
        {
            return GetDictValue(dictType, dictLabel, SEPARATOR);
        }

        /// <summary>
        /// 根据字典类型和字典值获取字典标签
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictValue">字典值</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string GetDictLabel(string dictType, string dictValue, string separator)
        {
            StringBuilder propertyString = new StringBuilder();
            List<SysDictData> datas = GetDictCache(dictType);

            if (datas != null)
            {
                if (StringUtils.ContainsAny(separator, dictValue))
                {
                    foreach (SysDictData dict in datas)
                    {
                        foreach (string value in dictValue.Split(separator))
                        {
                            if (value.Equals(dict.DictValue))
                            {
                                propertyString.Append(dict.DictLabel).Append(separator);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (SysDictData dict in datas)
                    {
                        if (dictValue.Equals(dict.DictValue))
                        {
                            return dict.DictLabel!;
                        }
                    }
                }
            }
            return StringUtils.StripEnd(propertyString.ToString(), separator);
        }

        /// <summary>
        /// 根据字典类型和字典标签获取字典值
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictLabel">字典标签</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字典值</returns>
        public static string GetDictValue(string dictType, string dictLabel, string separator)
        {
            StringBuilder propertyString = new StringBuilder();
            List<SysDictData> datas = GetDictCache(dictType);

            if (StringUtils.ContainsAny(separator, dictLabel) && datas.IsNotEmpty())
            {
                foreach (SysDictData dict in datas)
                {
                    foreach (string label in dictLabel.Split(separator))
                    {
                        if (label.Equals(dict.DictLabel))
                        {
                            propertyString.Append(dict.DictValue).Append(separator);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (SysDictData dict in datas)
                {
                    if (dictLabel.Equals(dict.DictLabel))
                    {
                        return dict.DictValue!;
                    }
                }
            }
            return StringUtils.StripEnd(propertyString.ToString(), separator);
        }

        /// <summary>
        /// 删除指定字典缓存
        /// </summary>
        /// <param name="key">字典键</param>
        public static void RemoveDictCache(string key)
        {
            App.GetService<ICache>().Remove(GetCacheKey(key));
        }

        /// <summary>
        /// 清空字典缓存
        /// </summary>
        public static void ClearDictCache()
        {
            var redisCache = App.GetService<ICache>();
            redisCache.RemoveByPattern(CacheConstants.SYS_DICT_KEY + "*");
        }

        /// <summary>
        /// 设置cache key
        /// </summary>
        /// <param name="configKey">参数键</param>
        public static string GetCacheKey(string configKey)
        {
            return CacheConstants.SYS_DICT_KEY + configKey;
        }
    }
}
