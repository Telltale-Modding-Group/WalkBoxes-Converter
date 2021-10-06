using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using wbox_console.Utilities;

namespace wbox_console.Telltale
{
    public static class ConvertStructs
    {
        public static Edge Get_Edge(byte[] fileBytes, ref uint bytePointerPosition)
        {
            return new Edge()
            {
                mV1 = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mV2 = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDest = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDestEdge = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDir = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mMaxRadius = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition)
            };
        }

        public static Vector3 Get_Vector3(byte[] byteArray, ref uint bytePointerPostion)
        {
            return new Vector3()
            {
                x = ByteFunctions.ReadFloat(byteArray, ref bytePointerPostion),
                y = ByteFunctions.ReadFloat(byteArray, ref bytePointerPostion),
                z = ByteFunctions.ReadFloat(byteArray, ref bytePointerPostion)
            };
        }

        public static Flags Get_Flags(byte[] byteArray, ref uint bytePointerPostion)
        {
            return new Flags()
            {
                mFlags = ByteFunctions.ReadUnsignedInt(byteArray, ref bytePointerPostion)
            };
        }

        public static Symbol Get_Symbol(byte[] byteArray, ref uint bytePointerPostion)
        {
            return new Symbol()
            {
                mCrc64 = ByteFunctions.ReadUnsignedLong(byteArray, ref bytePointerPostion)
            };
        }

        public static ClassName Get_ClassName(byte[] byteArray, ref uint bytePointerPostion)
        {
            return new ClassName()
            {
                mTypeNameCRC = Get_Symbol(byteArray, ref bytePointerPostion),
                mVersionCRC = ByteFunctions.ReadUnsignedInt(byteArray, ref bytePointerPostion)
            };
        }
    }
}
