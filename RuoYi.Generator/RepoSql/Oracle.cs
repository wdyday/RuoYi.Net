using RuoYi.Generator.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuoYi.Generator.RepoSql
{
    public class Oracle : IDbSql, ITransient
    {
        // Table
        public SqlAndParameter GetDbTableListSqlAndParameter(GenTableDto dto)
        {
            var sql = $@"
SELECT 
    a.OBJECT_NAME AS TABLE_NAME,
    NVL(c.COMMENTS, '无描述') AS TABLE_COMMENT,
    a.CREATED AS CREATE_TIME,
    a.LAST_DDL_TIME AS UPDATE_TIME
FROM 
    ALL_OBJECTS a
JOIN 
    ALL_TAB_COMMENTS c
ON 
    a.OWNER = c.OWNER
    AND a.OBJECT_NAME = c.TABLE_NAME
WHERE 
    a.OBJECT_TYPE = 'TABLE'
    AND a.OWNER = 'C##RY_NET' 
        ";
            var parameters = new List<SugarParameter>();
            if (!string.IsNullOrEmpty(dto.TableName))
            {
                sql += " AND a.OBJECT_NAME LIKE @TABLE_NAME ";
                parameters.Add(new SugarParameter("@TABLE_NAME", "%" + dto.TableName + "%"));
            }
            if (!string.IsNullOrEmpty(dto.TableComment))
            {
                sql += " AND c.COMMENT LIKE @TABLE_COMMENT ";
                parameters.Add(new SugarParameter("@TABLE_COMMENT", "%" + dto.TableComment + "%"));
            }
            if (dto.Params.BeginTime != null)
            {
                sql += " AND a.CREATED >= @BEGIN_TIME ";
                parameters.Add(new SugarParameter("@BEGIN_TIME", "'" + dto.Params.BeginTime + "'"));
            }
            if (dto.Params.EndTime != null)
            {
                sql += " AND a.CREATED <= @END_TIME ";
                parameters.Add(new SugarParameter("@END_TIME", "'" + dto.Params.EndTime + "'"));
            }

            return new SqlAndParameter
            {
                Sql = sql,
                Parameters = parameters
            };
        }

        // Table: 按名字查询
        public string GetDbTableListByNamesSql()
        {
            return $@"
SELECT 
    a.OBJECT_NAME AS TABLE_NAME,
    NVL(c.COMMENTS, '无描述') AS TABLE_COMMENT,
    a.CREATED AS CREATE_TIME,
    a.LAST_DDL_TIME AS UPDATE_TIME
FROM 
    ALL_OBJECTS a
JOIN 
    ALL_TAB_COMMENTS c
ON 
    a.OWNER = c.OWNER
    AND a.OBJECT_NAME = c.TABLE_NAME
WHERE 
    a.OBJECT_TYPE = 'TABLE'
    AND a.OWNER = 'C##RY_NET'
    AND a.OBJECT_NAME NOT LIKE 'QRTZ_%'
    AND a.OBJECT_NAME NOT LIKE 'GEN_%'
    AND a.OBJECT_NAME IN (@tableNames) 
        ";
        }

        // 列 按table名称查询
        public string GetDbTableColumnsByName()
        {
            return $@"
SELECT 
    a.TABLE_NAME AS TableName,
    a.COLUMN_NAME AS column_name,
    c.COMMENTS AS column_comment,
    a.DATA_TYPE AS column_type,
    case when a.DATA_TYPE in ('RAW','RAW(16)','UUID') then 'guid'
      when a.DATA_TYPE='NUMBER' then 'int'
      --when a.DATA_TYPE='NUMBER' then 'long'
      when a.DATA_TYPE in ('VARCHAR2','NVARCHAR2') then 'string'
      --when a.DATA_TYPE in ('RAW','BLOB') then 'byte'
      --bool 
      when a.DATA_TYPE in ('TIMESTAMP','TIMESTAMP WITH LOCAL TIME ZONE','DATE') then 'DateTime'
      when a.DATA_TYPE in ('BINARY_FLOAT ','BINARY_DOUBLE') then 'decimal'
      else 'string'
      end as NETTYPE,
      0 AS MAXLENGTH,
      case when substr(b.CONSTRAINT_NAME,1,3)='PK_' then 1 else 0 end as is_pk,
    CASE WHEN a.NULLABLE='N' THEN 0 ELSE 1 END AS is_required,
    0 as is_increment,
    0 as sort
FROM 
    ALL_TAB_COLUMNS a
LEFT JOIN
    all_cons_columns b on a.TABLE_NAME=b.TABLE_NAME
LEFT JOIN 
    all_constraints c ON b.constraint_name = c.constraint_name
LEFT JOIN
    ALL_COL_COMMENTS c on a.TABLE_NAME=c.TABLE_NAME AND a.COLUMN_NAME=c.COLUMN_NAME
WHERE 
    a.TABLE_NAME = @tablename
    --AND b.CONSTRAINT_NAME LIKE 'PK_%'
    AND c.CONSTRAINT_TYPE='P'
    AND a.OWNER = 'C##RY_NET' 
        ";
        }
    }
}
