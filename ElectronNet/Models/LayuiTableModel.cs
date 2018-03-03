using System.Collections.Generic;

namespace ElectronNet.Models
{
    /// <summary>
    /// Layui表格控件数据模型
    /// </summary>
    public class LayuiTableModel<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Http状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }
    }
}
