using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using wbox_console.Utilities;

namespace wbox_console.Telltale
{
    public static class ConvertStructs
    {
        public static int[] Get_IntArray(int length, BinaryReader reader)
        {
            int[] intArray = new int[length];

            for(int i = 0; i < length; i++)
            {
                intArray[i] = reader.ReadInt32(); // [4 BYTES]
            }

            return intArray;
        }

        public static float[] Get_FloatArray(int length, BinaryReader reader)
        {
            float[] floatArray = new float[length];

            for (int i = 0; i < length; i++)
            {
                floatArray[i] = reader.ReadSingle(); // [4 BYTES]
            }

            return floatArray;
        }

        public static Edge Get_Edge(BinaryReader reader)
        {
            return new Edge()
            {
                mV1 = reader.ReadInt32(),
                mV2 = reader.ReadInt32(),
                mEdgeDest = reader.ReadInt32(),
                mEdgeDestEdge = reader.ReadInt32(),
                mEdgeDir = reader.ReadInt32(),
                mMaxRadius = reader.ReadSingle()
            };
        }

        public static Vector3 Get_Vector3(BinaryReader reader)
        {
            return new Vector3()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle()
            };
        }

        public static Flags Get_Flags(BinaryReader reader)
        {
            return new Flags()
            {
                mFlags = reader.ReadUInt32()
            };
        }

        public static Symbol Get_Symbol(BinaryReader reader)
        {
            return new Symbol()
            {
                mCrc64 = reader.ReadUInt64()
            };
        }

        public static Edge GetNewEdge()
        {
            return new Edge()
            {
                mEdgeDest = 0,
                mEdgeDir = 1,
                mEdgeDestEdge = 0,
                mMaxRadius = -1.5816464E+38f,
                mV1 = 0,
                mV2 = 0
            };
        }

        public static Flags GetNewFlags()
        {
            return new Flags()
            {
                mFlags = 0
            };
        }

        public static Tri GetNewTri()
        {
            return new Tri()
            {
                mEdgeInfo_BlockSize = 76,
                mEdgeInfo = new Edge[3]
                {
                    GetNewEdge(),
                    GetNewEdge(),
                    GetNewEdge()
                },
                mMaxRadius = 100000.0f,
                mQuadBuddy = -1,
                mFlags = GetNewFlags(),
                mFootstepMaterial_BlockSize = 8,
                mFootstepMaterial = EnumMaterial.Default,
                mNormal = 0,
                mVertOffsets_BlockSize = 16,
                mVertScales_BlockSize = 16,
                mVerts_BlockSize = 16,
                mVertOffsets = new int[3],
                mVerts = new int[3],
                mVertScales = new float[3]
                {
                    1.0f,
                    1.0f,
                    1.0f
                }
            };
        }
    }
}
