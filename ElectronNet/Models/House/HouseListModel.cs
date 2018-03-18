using System;

namespace ElectronNet.Models
{
    /// <summary>
    /// 房源列表数据模型
    /// </summary>
    public class HouseListModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public long CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public long RegionId { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 社区Id
        /// </summary>
        public long CommunityId { get; set; }

        /// <summary>
        /// 社区名称
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 社区位置
        /// </summary>
        public string CommunityLocation { get; set; }

        /// <summary>
        /// 社区周围交通
        /// </summary>
        public string CommunityTraffic { get; set; }

        /// <summary>
        /// 社区创建年份
        /// </summary>
        public int? CommunityBuildYear { get; set; }

        /// <summary>
        /// 房屋地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 月租金
        /// </summary>
        public int MonthRent { get; set; }

        /// <summary>
        /// 房间类别Id
        /// </summary>
        public long RoomTypeId { get; set; }

        /// <summary>
        /// 房间类别名称
        /// </summary>
        public string RoomTypeName { get; set; }

        /// <summary>
        /// 房屋状态Id
        /// </summary>
        public long StatusId { get; set; }

        /// <summary>
        /// 房屋状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 房屋类别Id
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// 房屋类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 房屋面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 装修状态Id
        /// </summary>
        public long DecorateStatusId { get; set; }

        /// <summary>
        /// 装修状态
        /// </summary>
        public string DecorateStatusName { get; set; }

        /// <summary>
        /// 总楼层数
        /// </summary>
        public int TotalFloorCount { get; set; }

        /// <summary>
        /// 所在楼层
        /// </summary>
        public int FloorIndex { get; set; }

        /// <summary>
        /// 朝向
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 可看房时间
        /// </summary>
        public DateTime LookableDateTime { get; set; }

        /// <summary>
        /// 可入住时间
        /// </summary>
        public DateTime CheckInDateTime { get; set; }

        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// 业主电话号码
        /// </summary>
        public string OwnerPhoneNum { get; set; }

        /// <summary>
        /// 房源描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 房屋配置Id数组
        /// </summary>
        public long[] AttachmentIds { get; set; }

        /// <summary>
        /// 第一张缩略图地址
        /// </summary>
        public string FirstThumbUrl { get; set; }

    }
}
