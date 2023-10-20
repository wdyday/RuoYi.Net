namespace RuoYi.Generator.Services
{
    /// <summary>
    /// 业务 服务层
    /// </summary>
    public interface IGenTableService
    {
        /**
         * 查询业务列表
         * 
         * @param genTable 业务信息
         * @return 业务集合
         */
        public List<GenTable> SelectGenTableList(GenTableDto genTable);

        /**
         * 查询据库列表
         * 
         * @param genTable 业务信息
         * @return 数据库表集合
         */
        public List<GenTable> SelectDbTableList(GenTableDto genTable);

        /// <summary>
        /// 分页查询
        /// </summary>
        public SqlSugarPagedList<GenTable> GetPagedList(GenTableDto dto);

        /**
         * 查询据库列表
         * 
         * @param tableNames 表名称组
         * @return 数据库表集合
         */
        public List<GenTable> SelectDbTableListByNames(String[] tableNames);

        /**
         * 查询所有表信息
         * 
         * @return 表信息集合
         */
        public List<GenTable> SelectGenTableAll();

        /**
         * 查询业务信息
         * 
         * @param id 业务ID
         * @return 业务信息
         */
        public GenTable SelectGenTableById(long id);

        /**
         * 修改业务
         * 
         * @param genTable 业务信息
         * @return 结果
         */
        public void UpdateGenTable(GenTableDto genTable);

        /**
         * 删除业务信息
         * 
         * @param tableIds 需要删除的表数据ID
         * @return 结果
         */
        public void DeleteGenTableByIds(long[] tableIds);

        /**
         * 导入表结构
         * 
         * @param tableList 导入表列表
         */
        public void ImportGenTable(List<GenTable> tableList);

        /**
         * 预览代码
         * 
         * @param tableId 表编号
         * @return 预览数据列表
         */
        public Dictionary<string, string> PreviewCode(long tableId);

        /**
         * 生成代码（下载方式）
         * 
         * @param tableName 表名称
         * @return 数据
         */
        public byte[] DownloadCode(string tableName);

        /**
         * 生成代码（自定义路径）
         * 
         * @param tableName 表名称
         * @return 数据
         */
        public void GeneratorCode(string tableName);

        /**
         * 同步数据库
         * 
         * @param tableName 表名称
         */
        public void SynchDb(string tableName);

        /**
         * 批量生成代码（下载方式）
         * 
         * @param tableNames 表数组
         * @return 数据
         */
        public byte[] DownloadCode(string[] tableNames);

        /**
         * 修改保存参数校验
         * 
         * @param genTable 业务信息
         */
        public void ValidateEdit(GenTable genTable);
    }
}