namespace RuoYi.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TaskAttribute : Attribute
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        public TaskAttribute(string name)
        {
            this.Name = name;
        }
    }
}
