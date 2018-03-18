namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// HouseController访问Url
    /// </summary>
    public class HouseSettings
    {
        /// <summary>
        /// 房屋列表Url
        /// </summary>
        public string ListHouses { get; set; }

        /// <summary>
        /// 获取添加所需数据Url
        /// </summary>
        public string GetDict { get; set; }

        /// <summary>
        /// 添加房源Url
        /// </summary>
        public string AddHouse { get; set; }

        /// <summary>
        /// 获取编辑房源所需数据Url
        /// </summary>
        public string GetEditDict { get; set; }

        /// <summary>
        /// 编辑房源Url
        /// </summary>
        public string EditHouse { get; set; }

        /// <summary>
        /// 删除房源Url
        /// </summary>
        public string DeleteHouse { get; set; }

        /// <summary>
        /// 批量删除房源Url
        /// </summary>
        public string BatchDeleteHouse { get; set; }

        /// <summary>
        /// 上传房源图片Url
        /// </summary>
        public string UploadPicture { get; set; }

        /// <summary>
        /// 删除房源图片Url
        /// </summary>
        public string DeletePic { get; set; }

        /// <summary>
        /// 房源图片列表Url
        /// </summary>
        public string PictureList { get; set; }
    }
}
