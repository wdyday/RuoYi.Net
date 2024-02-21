using System.Diagnostics.CodeAnalysis;

namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// 支持重复 Key 的字典比较器
/// </summary>
internal class RepeatKeyEqualityComparer : IEqualityComparer<string>
{
    /// <summary>
    /// 相等比较
    /// </summary>
    /// <param name="x"><see cref="string"/></param>
    /// <param name="y"><see cref="string"/></param>
    /// <returns><see cref="bool"/></returns>
    public bool Equals(string x, string y)
    {
        return x != y;
    }

    /// <summary>
    /// 获取哈希值
    /// </summary>
    /// <param name="obj"><see cref="string"/></param>
    /// <returns><see cref="int"/></returns>
    public int GetHashCode([DisallowNull] string obj)
    {
        return obj.GetHashCode();
    }
}