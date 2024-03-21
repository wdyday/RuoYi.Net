using RuoYi.Data.Slave.Dtos;
using RuoYi.Data.Slave.Entities;

namespace RuoYi.System.Slave.Repositories;

/// <summary>
///  用户信息表 Repository
///  author ruoyi
///  date   2023-08-21 14:40:20
/// </summary>
public class SysUserRepository : BaseRepository<SlaveSysUser, SlaveSysUserDto>
{
    public SysUserRepository(ISqlSugarRepository<SlaveSysUser> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SlaveSysUser> Queryable(SlaveSysUserDto dto)
    {
        return Repo.AsQueryable()
        //.Includes(t => t.SubTable)
        //.WhereIF(dto.Id > 0, (t) => t.Id == dto.Id)
        ;
    }

    public override ISugarQueryable<SlaveSysUserDto> DtoQueryable(SlaveSysUserDto dto)
    {
        return Repo.AsQueryable()
            //.LeftJoin<SubTable>((t, s) => t.Id == s.Id)
            //.WhereIF(dto.Id > 0, (t) => t.Id == dto.Id)
            .Select((t) => new SlaveSysUserDto
            {
                CreateBy = t.CreateBy,
                CreateTime = t.CreateTime,
                UpdateBy = t.UpdateBy,
                UpdateTime = t.UpdateTime,

            });
    }
}