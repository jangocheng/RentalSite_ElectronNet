namespace ElectronNet.Models
{
    /// <summary>
    /// 权限数据模型
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限说明
        /// </summary>
        public string Description { get; set; }
    }
}
