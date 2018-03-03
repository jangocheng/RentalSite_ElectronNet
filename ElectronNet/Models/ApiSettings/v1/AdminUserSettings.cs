namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// AdminUserController访问Url
    /// </summary>
    public class AdminUserSettings
    {
        /// <summary>
        /// 后台用户列表访问Url
        /// </summary>
        public string ListAdminUsers { get; set; }

        /// <summary>
        /// 添加后台用户Url
        /// </summary>
        public string AddAdminUser { get; set; }

        /// <summary>
        /// 编辑后台用户Url
        /// </summary>
        public string EditAdminUser { get; set; }

        /// <summary>
        /// 删除后台用户Url
        /// </summary>
        public string DeleteAdminUser { get; set; }

        /// <summary>
        /// 批量删除后台用户Url
        /// </summary>
        public string BatchDeleteAdminUser { get; set; }

        /// <summary>
        /// 根据数据Id获取数据Url
        /// </summary>
        public string GetAdminUserById { get; set; }

        /// <summary>
        /// 查询Url
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// 获取用户头像地址Url
        /// </summary>
        public string GetHeadImgUrlById { get; set; }
    }
}
