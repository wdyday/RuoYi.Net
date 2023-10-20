using RuoYi.Data;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;

namespace RuoYi.System.Repositories
{
    /// <summary>
    ///  通知公告表 Repository
    ///  author ruoyi
    ///  date   2023-09-04 17:50:00
    /// </summary>
    public class SysNoticeRepository : BaseRepository<SysNotice, SysNoticeDto>
    {
        public SysNoticeRepository(ISqlSugarRepository<SysNotice> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }

        public override ISugarQueryable<SysNotice> Queryable(SysNoticeDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.NoticeId > 0, (t) => t.NoticeId == dto.NoticeId)
            ;
        }

        public override ISugarQueryable<SysNoticeDto> DtoQueryable(SysNoticeDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.NoticeId > 0, (t) => t.NoticeId == dto.NoticeId)
                .Select((t) => new SysNoticeDto
                {
                    CreateBy = t.CreateBy,
                    CreateTime = t.CreateTime,
                    UpdateBy = t.UpdateBy,
                    UpdateTime = t.UpdateTime,

                    NoticeId = t.NoticeId,
                    NoticeTitle = t.NoticeTitle,
                    NoticeContent = SqlFunc.MappingColumn(t.NoticeContent, " cast(notice_content as char) "),
                    NoticeType = t.NoticeType,
                    Status = t.Status
                });
        }
    }
}