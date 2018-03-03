namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// RoleController访问Url
    /// </summary>
    public class RoleSettings
    {
        /// <summary>
        /// 角色列表访问Url
        /// </summary>
        public string ListRoles { get; set; }

        /// <summary>
        /// 添加角色Url
        /// </summary>
        public string AddRole { get; set; }

        /// <summary>
        /// 编辑角色Url
        /// </summary>
        public string EditRole { get; set; }

        /// <summary>
        /// 根据数据Id获取角色数据Url
        /// </summary>
        public string GetRoleById { get; set; }

        /// <summary>
        /// 删除角色Url
        /// </summary>
        public string DeleteRole { get; set; }

        /// <summary>
        /// 批量删除角色Url
        /// </summary>
        public string BatchDeleteRole { get; set; }

        /// <summary>
        /// 根据后台用户Id获取角色Url
        /// </summary>
        public string GetByAdminUserId { get; set; }
    }
}
