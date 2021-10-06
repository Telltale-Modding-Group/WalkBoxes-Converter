using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using wbox_console.Telltale;
using wbox_console.Utilities;

namespace wbox_console
{
    public class WalkBoxes_File
    {
        public WalkBoxes WalkBoxes { get; set; }

        public static WalkBoxes Read_WalkBox_FromTelltale(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            Console.WriteLine("File Size: '{0}'", fileBytes.Length);

            uint bytePointerPosition = 0;

            WalkBoxes walkBoxes = new WalkBoxes();

            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| Meta Header |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            walkBoxes.mMetaStreamVersion = ByteFunctions.ReadFixedString(fileBytes, 4, ref bytePointerPosition);
            Console.WriteLine("WBOX Meta Stream Version: '{0}'", walkBoxes.mMetaStreamVersion);

            //--------------------------Default Section Chunk Size--------------------------
            walkBoxes.mDefaultSectionChunkSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX Default Section Chunk Size: '{0}'", walkBoxes.mDefaultSectionChunkSize);

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes]
            walkBoxes.mDebugSectionChunkSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX Debug Section Chunk Size = {0}", walkBoxes.mDebugSectionChunkSize);

            //--------------------------Async Section Chunk Size-------------------------- [4 bytes]
            walkBoxes.mAsyncSectionChunkSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX Async Section Chunk Size = {0}", walkBoxes.mAsyncSectionChunkSize);

            //--------------------------CALCULATING HEADER LENGTH--------------------------
            uint calculated_HeaderLength = (uint)fileBytes.Length - walkBoxes.mDefaultSectionChunkSize;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("WBOX Calculated Header Length: '{0}'", calculated_HeaderLength);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

            //--------------------------mClassNamesLength-------------------------- [4 bytes]
            walkBoxes.mClassNamesLength = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mClassNamesLength = {0}", walkBoxes.mClassNamesLength);

            //--------------------------mClassNames--------------------------
            walkBoxes.mClassNames = new ClassName[walkBoxes.mClassNamesLength];

            for (int i = 0; i < walkBoxes.mClassNames.Length; i++)
            {
                walkBoxes.mClassNames[i] = ConvertStructs.Get_ClassName(fileBytes, ref bytePointerPosition);

                Console.WriteLine("WBOX mClassName {0} = {1}", i, walkBoxes.mClassNames[i]);
            }

            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| WBOX DATA |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| WBOX Data |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);

            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            walkBoxes.mName_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mName Block Size = {0}", walkBoxes.mName_BlockSize);

            //--------------------------mName String Length-------------------------- [4 bytes]
            walkBoxes.mName_StringLength = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNameLength: '{0}'", walkBoxes.mName_StringLength);

            //--------------------------mName-------------------------- [mName_StringLength bytes]
            walkBoxes.mName = ByteFunctions.ReadFixedString(fileBytes, walkBoxes.mName_StringLength, ref bytePointerPosition);
            Console.WriteLine("WBOX mName: '{0}'", walkBoxes.mName);

            //---------------------------mTris DCArray Capacity--------------------------
            uint bytePointerPosition_beforeTris = bytePointerPosition;
            walkBoxes.mTris_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mTris Capacity: '{0}'", walkBoxes.mTris_Capacity);

            //---------------------------mTris DCArray Size--------------------------
            walkBoxes.mTris_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mTris Size: '{0}'", walkBoxes.mTris_Size);

            //---------------------------mTris--------------------------
            walkBoxes.mTris = new Tri[walkBoxes.mTris_Size];

            for (int i = 0; i < walkBoxes.mTris.Length; i++)
            {
                //---------------------------mTri Entry--------------------------  [140 BYTES] (but there are 8 extra bytes)
                walkBoxes.mTris[i] = new Tri()
                {
                    mFootstepMaterial_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 8
                    mFootstepMaterial = (EnumMaterial)ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mFlags = ConvertStructs.Get_Flags(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mNormal = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mQuadBuddy = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mMaxRadius = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition), // [4 BYTES]
                    mVerts_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVerts = new int[3] //SArray<int,3> [12 BYTES]
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)  // [4 BYTES]
                    },
                    mEdgeInfo_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 76
                    mEdgeInfo = new Edge[3] //SArray<WalkBoxes::Edge,3> [72 BYTES]
                    {
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition), //[24 BYTES]
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition), //[24 BYTES]
                        ConvertStructs.Get_Edge(fileBytes, ref bytePointerPosition)  //[24 BYTES]
                    },
                    mVertOffsets_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVertOffsets = new int[3] //SArray<int,3> [12 BYTES]
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)  // [4 BYTES]
                    },
                    mVertScales_BlockSize = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition), //[4 BYTES] ALWAYS 16
                    mVertScales = new float[3] //SArray<float,3> [12 BYTES]
                    {
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition), // [4 BYTES]
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition)  // [4 BYTES]
                    }
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeTris, walkBoxes.mTris_Capacity, ref bytePointerPosition);

            //---------------------------mVerts DCArray Capacity--------------------------
            uint bytePointerPosition_beforeVerts = bytePointerPosition;
            walkBoxes.mVerts_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mVerts Capacity: '{0}'", walkBoxes.mVerts_Capacity);

            //---------------------------mVerts DCArray Size--------------------------
            walkBoxes.mVerts_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mVerts Size: '{0}'", walkBoxes.mVerts_Size);

            //---------------------------mVerts--------------------------
            walkBoxes.mVerts = new Vert[walkBoxes.mVerts_Size];

            for (int i = 0; i < walkBoxes.mVerts.Length; i++)
            {
                //---------------------------mVert Entry--------------------------
                walkBoxes.mVerts[i] = new Vert()
                {
                    mFlags = ConvertStructs.Get_Flags(fileBytes, ref bytePointerPosition),
                    mPos = ConvertStructs.Get_Vector3(fileBytes, ref bytePointerPosition)
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeVerts, walkBoxes.mVerts_Capacity, ref bytePointerPosition);

            //---------------------------mNormals DCArray Capacity--------------------------
            uint bytePointerPosition_beforeNormals = bytePointerPosition;
            walkBoxes.mNormals_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNormals Capacity: '{0}'", walkBoxes.mNormals_Capacity);

            //---------------------------mNormals DCArray Size--------------------------
            walkBoxes.mNormals_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mNormals Size: '{0}'", walkBoxes.mNormals_Size);

            //---------------------------mNormals--------------------------
            walkBoxes.mNormals = new Vector3[walkBoxes.mNormals_Size];

            for (int i = 0; i < walkBoxes.mNormals.Length; i++)
            {
                walkBoxes.mNormals[i] = ConvertStructs.Get_Vector3(fileBytes, ref bytePointerPosition);
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeNormals, walkBoxes.mNormals_Capacity, ref bytePointerPosition);

            //---------------------------mQuads DCArray Capacity--------------------------
            uint bytePointerPosition_beforeQuads = bytePointerPosition;
            walkBoxes.mQuads_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mQuads Capacity: '{0}'", walkBoxes.mQuads_Capacity);

            //---------------------------mQuads DCArray Size--------------------------
            walkBoxes.mQuads_Size = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("WBOX mQuads Size: '{0}'", walkBoxes.mQuads_Size);

            //---------------------------mQuads--------------------------
            walkBoxes.mQuads = new Quad[walkBoxes.mQuads_Size];

            for (int i = 0; i < walkBoxes.mQuads.Length; i++)
            {
                walkBoxes.mQuads[i] = new Quad()
                {
                    mVerts = new int[4]
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)
                    }
                };
            }

            //not necessary, but to check if we are where we should be at
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPosition_beforeQuads, walkBoxes.mQuads_Capacity, ref bytePointerPosition);

            ByteFunctions.ReachedEndOfFile(bytePointerPosition, (uint)fileBytes.Length);

            return walkBoxes;
        }
    }
}
