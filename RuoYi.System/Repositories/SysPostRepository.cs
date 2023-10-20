using RuoYi.Data;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;
namespace RuoYi.System.Repositories
{
    /// <summary>
    ///  岗位信息表 Repository
    ///  author ruoyi
    ///  date   2023-08-30 13:21:36
    /// </summary>
    public class SysPostRepository : BaseRepository<SysPost, SysPostDto>
    {
        public SysPostRepository(ISqlSugarRepository<SysPost> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }

        public override ISugarQueryable<SysPost> Queryable(SysPostDto dto)
        {
            return Repo.AsQueryable()
                //.Includes(t => t.SubTable)
                .OrderBy((p) => p.PostSort)
                .WhereIF(!string.IsNullOrEmpty(dto.Status), (p) => p.Status == dto.Status)
                .WhereIF(!string.IsNullOrEmpty(dto.PostCode), (p) => p.PostCode == dto.PostCode)
                .WhereIF(!string.IsNullOrEmpty(dto.PostName), (p) => p.PostName == dto.PostName)
                .WhereIF(!string.IsNullOrEmpty(dto.PostCodeLike), (p) => p.PostCode.Contains(dto.PostCodeLike))
                .WhereIF(!string.IsNullOrEmpty(dto.PostNameLike), (p) => p.PostName.Contains(dto.PostNameLike))
            ;
        }

        public override ISugarQueryable<SysPostDto> DtoQueryable(SysPostDto dto)
        {
            return Repo.AsQueryable()
                .LeftJoin<SysUserPost>((p, up) => p.PostId == up.PostId)
                .LeftJoin<SysUser>((p, up, u) => up.UserId == u.UserId)
                .OrderBy((p, up, u) => p.PostSort)
                .WhereIF(!string.IsNullOrEmpty(dto.Status), (p, up, u) => p.Status == dto.Status)
                .WhereIF(!string.IsNullOrEmpty(dto.UserName), (p, up, u) => u.UserName == dto.UserName)
                .WhereIF(!string.IsNullOrEmpty(dto.PostCode), (p, up, u) => p.PostCode == dto.PostCode)
                .WhereIF(!string.IsNullOrEmpty(dto.PostName), (p, up, u) => p.PostName == dto.PostName)
                .WhereIF(!string.IsNullOrEmpty(dto.PostCodeLike), (p, up, u) => p.PostCode.Contains(dto.PostCodeLike))
                .WhereIF(!string.IsNullOrEmpty(dto.PostNameLike), (p, up, u) => p.PostName.Contains(dto.PostNameLike))
                .Select((p, up, u) => new SysPostDto
                {
                    CreateBy = p.CreateBy,
                    CreateTime = p.CreateTime,
                    UpdateBy = p.UpdateBy,
                    UpdateTime = p.UpdateTime,
                    Remark = p.Remark,

                    PostId = p.PostId,
                    PostCode = p.PostCode,
                    PostName = p.PostName,
                    PostSort = p.PostSort,
                    Status = p.Status
                }).Distinct()
                ;
        }

        protected override async Task FillRelatedDataAsync(IEnumerable<SysPostDto> dtos)
        {
            await base.FillRelatedDataAsync(dtos);

            foreach (var d in dtos)
            {
                d.StatusDesc = Status.ToDesc(d.Status);
            }
        }

        public async Task<SysPost> CheckPostNameUniqueAsync(string? postName)
        {
            if (string.IsNullOrEmpty(postName)) return null!;

            return await this.GetFirstAsync(new SysPostDto { PostName = postName });
        }

        public async Task<SysPost> CheckPostCodeUniqueAsync(string? postCode)
        {
            if (string.IsNullOrEmpty(postCode)) return null!;

            return await this.GetFirstAsync(new SysPostDto { PostCode = postCode });
        }
    }
}