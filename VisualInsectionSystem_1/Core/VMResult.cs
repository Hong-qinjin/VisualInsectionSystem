using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualInsectionSystem.Core
{
    /// <summary>
    /// Vision Master检测结果结构化对象
    /// </summary>
    public class VMResult
    {
        /// <summary>
        /// 检测结果（OK/NG）
        /// </summary>
        public string DetectResult { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// 偏转角度
        /// </summary>
        public float R { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecuteTime { get; set; }
    }
}
