using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HaiKangDemo
{
    class HalconManage
    {
        public HObject ByteToHImageMono8(byte[] ImageGray,ushort Width, ushort Height)
        {
            HObject _Image;
            unsafe
            {
                fixed (byte* p = ImageGray)
                {
                    HOperatorSet.GenImage1Extern(out _Image, "byte", Width, Height, new IntPtr(p), 0);
                }
            }
            return _Image;
        }
        public HObject ByteToHImageBGR8(byte[]R, byte[]G, byte[]B, ushort Width, ushort Height)
        {

            HObject _Image;
            unsafe
            {
                fixed (byte* pr = R, pg = G, pb = B)
                {
                    HOperatorSet.GenImage3Extern(out _Image, "byte", Width, Height, new IntPtr(pr), new IntPtr(pg), new IntPtr(pb), 0);
                }
            }
            return _Image;
        }
    }
}
