using System.Collections.Generic;

namespace ElectronNet.Models
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限项
        /// </summary>
        public List<Permission> Permissions { get; set; }

        /// <summary>
        /// 所有权限项
        /// </summary>
        public List<Permission> AllPermissions { get; set; }
    }
}
