using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualInsectionSystem.Common
{
    public class VMResult
    {
        public string DetectResult { get; set; } = "error module result";
        public float X {  get; set; }
        public float Y { get; set; }
        public float R {  get; set; }
        public string ProductCode {  get; set; }
        public DateTime ExecuteTime {  get; set; }= DateTime.Now;

        public static VMResult Parse(string raw)
        {
            var result = new VMResult();
            if(string.IsNullOrEmpty(raw))
            {
                return result;
            }
            string[] parts = raw.Trim().Split(',');
            if(parts.Length >=4)
            {
                result.DetectResult = parts[0].Trim().ToUpper();
                float.TryParse(parts[1].Trim(), out float x); result.X = x;
                float.TryParse(parts[2].Trim(), out float y); result.Y = y;
                float.TryParse(parts[3].Trim(), out float r); result.R = r;
            }
            return result;
        }
    }
}
