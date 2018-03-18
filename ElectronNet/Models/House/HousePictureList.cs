namespace ElectronNet.Models
{
    /// <summary>
    /// 图片列表数据
    /// </summary>
    public class HousePictureList
    {
        /// <summary>
        /// 图片数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 房屋表Id
        /// </summary>
        public long HouseId { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumbUrl { get; set; }
    }
}
