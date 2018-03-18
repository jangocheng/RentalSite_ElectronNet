using System;

namespace ElectronNet.Models
{
    /// <summary>
    /// 房屋表单模型
    /// </summary>
    public class HouseFormModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 社区表Id
        /// </summary>
        public long CommunityId { get; set; }

        /// <summary>
        /// 户型Id
        /// </summary>
        public long RoomTypeId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 月租金
        /// </summary>
        public int MonthRent { get; set; }

        /// <summary>
        /// 房屋状态Id
        /// </summary>
        public long StatusId { get; set; }

        /// <summary>
        /// 房屋面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 房屋装修Id
        /// </summary>
        public long DecorateStatusId { get; set; }

        /// <summary>
        /// 房屋所在楼层
        /// </summary>
        public int FloorIndex { get; set; }

        /// <summary>
        /// 房屋总楼层
        /// </summary>
        public int TotalFloorCount { get; set; }

        /// <summary>
        /// 房屋朝向
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 预约看房时间
        /// </summary>
        public DateTime LookableDateTime { get; set; }

        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime CheckInDateTime { get; set; }

        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// 业主电话
        /// </summary>
        public string OwnerPhoneNum { get; set; }

        /// <summary>
        /// 房源描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 房屋配套设施
        /// </summary>
        public long[] AttachmentIds { get; set; }

        /// <summary>
        /// 房屋类型Id
        /// </summary>
        public long TypeId { get; set; }
    }
}
