using Newtonsoft.Json;
using System.Collections.Generic;

namespace ElectronNet.Models
{
    /// <summary>
    /// 房源编辑
    /// </summary>
    public class HouseEditViewModel : HouseAddViewModel
    {
        /// <summary>
        /// 房源列表
        /// </summary>
        [JsonProperty("houseDTO")]
        public HouseListModel House { get; set; }

        /// <summary>
        /// 该房屋所属配套设施
        /// </summary>
        public List<Attachment> HouseAttachments { get; set; }
    }
}
