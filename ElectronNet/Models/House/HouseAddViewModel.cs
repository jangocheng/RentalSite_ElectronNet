using System.Collections.Generic;

namespace ElectronNet.Models
{
    /// <summary>
    /// 房源添加视图模型
    /// </summary>
    public class HouseAddViewModel
    {
        /// <summary>
        /// 区域
        /// </summary>
        public List<Region> Regions { get; set; }

        /// <summary>
        /// 户型
        /// </summary>
        public List<IdName> RoomType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public List<IdName> RoomStatus { get; set; }

        /// <summary>
        /// 装修
        /// </summary>
        public List<IdName> DecorateStatuses { get; set; }

        /// <summary>
        /// 房屋类别
        /// </summary>
        public List<IdName> Types { get; set; }

        /// <summary>
        /// 配套设施
        /// </summary>
        public List<Attachment> Attachments { get; set; }
    }
}
