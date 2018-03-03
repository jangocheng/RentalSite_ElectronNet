namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// CityController访问Url
    /// </summary>
    public class CitySettings
    {
        /// <summary>
        /// 城市列表访问Url
        /// </summary>
        public string ListCities { get; set; }

        /// <summary>
        /// 添加城市访问Url
        /// </summary>
        public string AddCity { get; set; }

        /// <summary>
        /// 编辑城市Url
        /// </summary>
        public string EditCity { get; set; }

        /// <summary>
        /// 删除城市Url
        /// </summary>
        public string DeleteCity { get; set; }

        /// <summary>
        /// 批量删除城市Url
        /// </summary>
        public string BatchDeleteCity { get; set; }
    }
}
