namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// PermissionController访问Url
    /// </summary>
    public class PermissionSettings
    {
        /// <summary>
        /// 权限列表访问Url
        /// </summary>
        public string ListPermissions { get; set; }

        /// <summary>
        /// 根据数据Id获取数据Url
        /// </summary>
        public string GetPermissionById { get; set; }

        /// <summary>
        /// 添加权限Url
        /// </summary>
        public string AddPermission { get; set; }

        /// <summary>
        /// 编辑权限Url
        /// </summary>
        public string EditPermission { get; set; }

        /// <summary>
        /// 删除权限Url
        /// </summary>
        public string DeletePermission { get; set; }

        /// <summary>
        /// 批量删除权限Url
        /// </summary>
        public string BatchDeletePermission { get; set; }
    }
}
