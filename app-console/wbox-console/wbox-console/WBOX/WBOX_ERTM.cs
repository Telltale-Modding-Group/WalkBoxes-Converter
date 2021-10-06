using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using wbox_console.Telltale;
using wbox_console.Utilities;

namespace wbox_console.WBOX
{
    public class WBOX_ERTM
    {
        //META HEADER
        public string mMetaStreamVersion;
        public uint mClassNamesLength;
        public MetaClassName[] mClassNames;

        //WBOX DATA
        public uint mName_BlockSize;
        public uint mName_StringLength;
        public string mName;
        public uint mTris_Capacity;
        public int mTris_Size;
        public Tri[] mTris; //DCArray<WalkBoxes::Tri>
        public uint mVerts_Capacity;
        public int mVerts_Size;
        public Vert[] mVerts; //DCArray<WalkBoxes::Vert>
        public uint mNormals_Capacity;
        public int mNormals_Size;
        public Vector3[] mNormals; //DCArray<Vector3>
        public uint mQuads_Capacity;
        public int mQuads_Size;
        public Quad[] mQuads; //DCArray<WalkBoxes::Quad>

        public WBOX_ERTM(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            Console.WriteLine("File Size: '{0}'", fileBytes.Length);

            uint bytePointerPosition = 0;

            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| Meta Header |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            mMetaStreamVersion = ByteFunctions.ReadFixedString(fileBytes, 4, ref bytePointerPosition);
            Console.WriteLine("WBOX Meta Stream Version: '{0}'", mMetaStreamVersion);

            //--------------------------mClassNamesLength-------------------------- [4 bytes]
            mClassNamesLength = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mClassNamesLength = {0}", mClassNamesLength);

            //--------------------------mClassNames--------------------------
            mClassNames = new MetaClassName[mClassNamesLength];

            for (int i = 0; i < mClassNames.Length; i++)
            {
                mClassNames[i] = ConvertStructs.Get_ClassName(fileBytes, ref bytePointerPosition);

                Console.WriteLine("WBOX mClassName {0} = {1}", i, mClassNames[i]);
            }

            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| WBOX Data |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            mName_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mName Block Size = {0}", mName_BlockSize);

            //--------------------------mName String Length-------------------------- [4 bytes]
            mName_StringLength = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNameLength: '{0}'", mName_StringLength);

            //--------------------------mName-------------------------- [mName_StringLength bytes]
            mName = ByteFunctions.ReadFixedString(fileBytes, mName_StringLength, ref bytePointerPosition);
            Console.WriteLine("WBOX mName: '{0}'", mName);

            //---------------------------mTris DCArray Capacity--------------------------
            uint bytePointerPosition_beforeTris = bytePointerPosition;
            mTris_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mTris Capacity: '{0}'", mTris_Capacity);

            //---------------------------mTris DCArray Size--------------------------
            mTris_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mTris Size: '{0}'", mTris_Size);

            //---------------------------mTris--------------------------
            mTris = new Tri[mTris_Size];

            for (int i = 0; i < mTris.Length; i++)
            {
                //---------------------------mTri Entry--------------------------  [140 BYTES]
                mTris[i] = new Tri()
                {
                    mFlags = ConvertStructs.Get_Flags(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mNormal = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mQuadBuddy = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mMaxRadius = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mVerts_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVerts = ConvertStructs.Get_IntArray(3, fileBytes, ref bytePointerPosition), //SArray<int,3> [12 BYTES]
                    mEdgeInfo_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 76
                    mEdgeInfo = new Edge[3] //SArray<WalkBoxes::Edge,3> [72 BYTES]
                    {
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition), //[24 BYTES]
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition), //[24 BYTES]
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition)  //[24 BYTES]
                    },
                    mVertOffsets_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVertOffsets = ConvertStructs.Get_IntArray(3, fileBytes, ref bytePointerPosition), //SArray<int,3> [12 BYTES]
                    mVertScales_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVertScales = ConvertStructs.Get_FloatArray(3, fileBytes, ref bytePointerPosition) //SArray<float,3> [12 BYTES]
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeTris, mTris_Capacity, ref bytePointerPosition);

            //---------------------------mVerts DCArray Capacity--------------------------
            uint bytePointerPosition_beforeVerts = bytePointerPosition;
            mVerts_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mVerts Capacity: '{0}'", mVerts_Capacity);

            //---------------------------mVerts DCArray Size--------------------------
            mVerts_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mVerts Size: '{0}'", mVerts_Size);

            //---------------------------mVerts--------------------------
            mVerts = new Vert[mVerts_Size];

            for (int i = 0; i < mVerts.Length; i++)
            {
                //---------------------------mVert Entry--------------------------
                mVerts[i] = new Vert()
                {
                    mFlags = ConvertStructs.Get_Flags(fileBytes, ref bytePointerPosition),
                    mPos = ConvertStructs.Get_Vector3(fileBytes, ref bytePointerPosition)
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeVerts, mVerts_Capacity, ref bytePointerPosition);

            //---------------------------mNormals DCArray Capacity--------------------------
            uint bytePointerPosition_beforeNormals = bytePointerPosition;
            mNormals_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNormals Capacity: '{0}'", mNormals_Capacity);

            //---------------------------mNormals DCArray Size--------------------------
            mNormals_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNormals Size: '{0}'", mNormals_Size);

            //---------------------------mNormals--------------------------
            mNormals = new Vector3[mNormals_Size];

            for (int i = 0; i < mNormals.Length; i++)
            {
                mNormals[i] = ConvertStructs.Get_Vector3(fileBytes, ref bytePointerPosition);
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeNormals, mNormals_Capacity, ref bytePointerPosition);

            //---------------------------mQuads DCArray Capacity--------------------------
            uint bytePointerPosition_beforeQuads = bytePointerPosition;
            mQuads_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mQuads Capacity: '{0}'", mQuads_Capacity);

            //---------------------------mQuads DCArray Size--------------------------
            mQuads_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mQuads Size: '{0}'", mQuads_Size);

            //---------------------------mQuads--------------------------
            mQuads = new Quad[mQuads_Size];

            for (int i = 0; i < mQuads.Length; i++)
            {
                mQuads[i] = new Quad()
                {
                    mVerts = ConvertStructs.Get_IntArray(4, fileBytes, ref bytePointerPosition) //SArray<int,4> [16 BYTES]
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeQuads, mQuads_Capacity, ref bytePointerPosition);

            ByteFunctions.ReachedEndOfFile(bytePointerPosition, (uint)fileBytes.Length);
        }
    }
}
