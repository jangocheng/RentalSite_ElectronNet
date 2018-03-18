namespace ElectronNet.Models
{
    /// <summary>
    /// 房屋配置
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconName { get; set; }
    }
}
