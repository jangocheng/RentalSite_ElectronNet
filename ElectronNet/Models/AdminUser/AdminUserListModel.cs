namespace ElectronNet.Models
{
    /// <summary>
    /// 后台用户列表模型
    /// </summary>
    public class AdminUserListModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum { get; set; }
    }
}
