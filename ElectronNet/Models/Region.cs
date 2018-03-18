namespace ElectronNet.Models
{
    /// <summary>
    /// 区域表数据模型
    /// </summary>
    public class Region
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属城市Id
        /// </summary>
        public long CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
    }
}
