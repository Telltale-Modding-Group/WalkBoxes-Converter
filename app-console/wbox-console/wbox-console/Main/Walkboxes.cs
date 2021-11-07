using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wbox_console.Telltale;
using wbox_console.Utilities;
using System.IO;

namespace wbox_console.Main
{
    /// <summary>
    /// Main walkboxes object.
    /// </summary>
    public class Walkboxes
    {
        public uint mName_BlockSize;
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

        public Walkboxes (BinaryReader reader, bool showConsole = false)
        {
            mName_BlockSize = reader.ReadUInt32(); //mName Block Size [4 bytes] //mName block size (size + string len)
            mName = ByteFunctions.ReadString(reader); //mName [x bytes]
            mTris_Capacity = reader.ReadUInt32(); //mTris DCArray Capacity [4 bytes]
            mTris_Size = reader.ReadInt32(); //mTris DCArray Size [4 bytes]

            //---------------------------mTris--------------------------
            mTris = new Tri[mTris_Size];

            for (int i = 0; i < mTris.Length; i++)
            {
                //---------------------------mTri Entry--------------------------  [148 BYTES]
                mTris[i] = new Tri()
                {
                    mFootstepMaterial_BlockSize = reader.ReadUInt32(), //[4 BYTES] ALWAYS 8
                    mFootstepMaterial = (EnumMaterial)reader.ReadInt32(), // [4 BYTES]
                    mFlags = ConvertStructs.Get_Flags(reader), // [4 BYTES]
                    mNormal = reader.ReadInt32(), // [4 BYTES]
                    mQuadBuddy = reader.ReadInt32(), // [4 BYTES]
                    mMaxRadius = reader.ReadSingle(), // [4 BYTES]
                    mVerts_BlockSize = reader.ReadUInt32(), //[4 BYTES] ALWAYS 16
                    mVerts = ConvertStructs.Get_IntArray(3, reader), //SArray<int,3> [12 BYTES]
                    mEdgeInfo_BlockSize = reader.ReadUInt32(), //[4 BYTES] ALWAYS 76
                    mEdgeInfo = new Edge[3] //SArray<WalkBoxes::Edge,3> [72 BYTES]
                    {
                        ConvertStructs.Get_Edge(reader), //[24 BYTES]
                        ConvertStructs.Get_Edge(reader), //[24 BYTES]
                        ConvertStructs.Get_Edge(reader)  //[24 BYTES]
                    },
                    mVertOffsets_BlockSize = reader.ReadUInt32(), //[4 BYTES] ALWAYS 16
                    mVertOffsets = ConvertStructs.Get_IntArray(3, reader), //SArray<int,3> [12 BYTES]
                    mVertScales_BlockSize = reader.ReadUInt32(), //[4 BYTES] ALWAYS 16
                    mVertScales = ConvertStructs.Get_FloatArray(3, reader) //SArray<float,3> [12 BYTES]
                };
            }

            mVerts_Capacity = reader.ReadUInt32(); //mVerts DCArray Capacity [4 bytes]
            mVerts_Size = reader.ReadInt32(); //mVerts DCArray Size [4 bytes]

            //---------------------------mVerts--------------------------
            mVerts = new Vert[mVerts_Size];

            for (int i = 0; i < mVerts.Length; i++)
            {
                //---------------------------mVert Entry--------------------------
                mVerts[i] = new Vert()
                {
                    mFlags = ConvertStructs.Get_Flags(reader),
                    mPos = ConvertStructs.Get_Vector3(reader)
                };
            }

            mNormals_Capacity = reader.ReadUInt32(); //mNormals DCArray Capacity [4 bytes]
            mNormals_Size = reader.ReadInt32(); //mNormals DCArray Size [4 bytes]

            //---------------------------mNormals--------------------------
            mNormals = new Vector3[mNormals_Size];

            for (int i = 0; i < mNormals.Length; i++)
            {
                mNormals[i] = ConvertStructs.Get_Vector3(reader);
            }

            mQuads_Capacity = reader.ReadUInt32(); //mQuads DCArray Capacity [4 bytes]
            mQuads_Size = reader.ReadInt32(); //mQuads DCArray Size [4 bytes]

            //---------------------------mQuads--------------------------
            mQuads = new Quad[mQuads_Size];

            for (int i = 0; i < mQuads.Length; i++)
            {
                mQuads[i] = new Quad()
                {
                    mVerts = ConvertStructs.Get_IntArray(4, reader) //SArray<int,4> [16 BYTES]
                };
            }

            if (showConsole)
                PrintConsole();
        }

        public void PrintConsole()
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| WBOX Data |||||||||||");
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("WBOX mName Block Size = {0}", mName_BlockSize);
            Console.WriteLine("WBOX mName: '{0}'", mName);
            Console.WriteLine("WBOX mTris Capacity: '{0}'", mTris_Capacity);
            Console.WriteLine("WBOX mTris Size: '{0}'", mTris_Size);
            Console.WriteLine("WBOX mVerts Capacity: '{0}'", mVerts_Capacity);
            Console.WriteLine("WBOX mVerts Size: '{0}'", mVerts_Size);
            Console.WriteLine("WBOX mNormals Capacity: '{0}'", mNormals_Capacity);
            Console.WriteLine("WBOX mNormals Size: '{0}'", mNormals_Size);
            Console.WriteLine("WBOX mQuads Capacity: '{0}'", mQuads_Capacity);
            Console.WriteLine("WBOX mQuads Size: '{0}'", mQuads_Size);
        }
    }
}
