namespace ElectronNet.Models
{
    public class AdminUserFormModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        public string Password2 { get; set; }

        /// <summary>
        /// 城市表数据Id，为null则表示总部
        /// </summary>
        public long? CityId { get; set; }

        /// <summary>
        /// 头像图片地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public long[] RoleIds { get; set; }
    }
}
