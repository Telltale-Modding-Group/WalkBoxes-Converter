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

        public struct Header_ClassName
        {
            public ulong crcValue;
            public string hexValue;
        }

        public static WalkBoxes Read_WalkBox_FromTelltale(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            Console.WriteLine("File Size: '{0}'", fileBytes.Length);

            uint bytePointerPosition = 0;

            //----------------------- WBOX HEADER -----------------------
            string MagicHeader = ByteFunctions.ReadFixedString(fileBytes, 4, ref bytePointerPosition);
            Console.WriteLine("MagicHeader: '{0}'", MagicHeader);

            uint mData = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mData: '{0}'", mData);

            //CALCULATION HERE (NOT IN THE FILE)
            uint HeaderLength = (uint)fileBytes.Length - mData;
            Console.WriteLine("Header Length: '{0}'", HeaderLength);

            uint Unknown1 = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition); //check what this value is in IDA
            uint Unknown2 = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition); //check what this value is in IDA
            uint Unknown3 = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition); //check what this value is in IDA

            int classNamesInHeader = 9;

            Header_ClassName[] header_ClassNames = new Header_ClassName[classNamesInHeader];

            for (int i = 0; i < header_ClassNames.Length; i++)
            {
                Header_ClassName className = new Header_ClassName();

                className.crcValue = ByteFunctions.ReadUnsignedLong(fileBytes, ref bytePointerPosition); //this is a 8 byte ulong crc of a classname
                className.hexValue = className.crcValue.ToString("X");

                header_ClassNames[i] = className;
            }

            //----------------------- WBOX DATA -----------------------
            uint Unknown4 = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition); //check what this value is in IDA
            Console.WriteLine("Unknown4: '{0}'", Unknown1);

            uint mNameLength = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mNameLength: '{0}'", mNameLength);

            WalkBoxes walkBoxes = new WalkBoxes()
            {
                mName = ByteFunctions.ReadFixedString(fileBytes, mNameLength, ref bytePointerPosition)
            };

            Console.WriteLine("mName: '{0}'", walkBoxes.mName);

            //---------------------------mTris--------------------------
            uint bytePointerPosition_beforeTris = bytePointerPosition;
            uint mTris_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mTris Capacity: '{0}'", mTris_Capacity);

            uint mTris_Size = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mTris Size: '{0}'", mTris_Size);

            walkBoxes.mTris = new Tri[mTris_Size];

            for (int i = 0; i < walkBoxes.mTris.Length; i++)
            {
                Tri mTri = new Tri()
                {
                    mFootstepMaterial = (EnumMaterial)ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                    mFlags = new Flags()
                    {
                        mFlags = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition)
                    },
                    mNormal = ByteFunctions.ReadLong(fileBytes, ref bytePointerPosition),
                    mQuadBuddy = ByteFunctions.ReadLong(fileBytes, ref bytePointerPosition),
                    mMaxRadius = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition),
                    mVerts = new int[3] //SArray<int,3>
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)
                    },
                    mEdgeInfo = new Edge[3] //SArray<WalkBoxes::Edge,3>
                    {
                        GetEdge(fileBytes, ref bytePointerPosition),
                        GetEdge(fileBytes, ref bytePointerPosition),
                        GetEdge(fileBytes, ref bytePointerPosition)
                    },
                    mVertOffsets = new int[3] //SArray<int,3> 
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)
                    },
                    mVertScales = new float[3] //SArray<float,3>
                    {
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition)
                    }
                };

                walkBoxes.mTris[i] = mTri;
            }

            //not necessary, but to check if we are where we should be at
            ArrayCheckAdjustment(bytePointerPosition_beforeTris, mTris_Capacity, ref bytePointerPosition);

            //---------------------------mVerts--------------------------
            uint bytePointerPosition_beforeVerts = bytePointerPosition;
            uint mVerts_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mVerts Capacity: '{0}'", mVerts_Capacity);

            uint mVerts_Size = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mVerts Size: '{0}'", mVerts_Size);

            walkBoxes.mVerts = new Vert[mVerts_Size];

            for (int i = 0; i < walkBoxes.mVerts.Length; i++)
            {
                Vert vertex = new Vert()
                {
                    mFlags = new Flags()
                    {
                        mFlags = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition)
                    },
                    mPos = GetVector3(fileBytes, ref bytePointerPosition)
                };

                walkBoxes.mVerts[i] = vertex;
            }

            //not necessary, but to check if we are where we should be at
            ArrayCheckAdjustment(bytePointerPosition_beforeVerts, mVerts_Capacity, ref bytePointerPosition);

            //---------------------------mNormals--------------------------
            uint bytePointerPosition_beforeNormals = bytePointerPosition;
            uint mNormals_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mNormals Capacity: '{0}'", mNormals_Capacity);

            uint mNormals_Size = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mNormals Size: '{0}'", mNormals_Size);

            walkBoxes.mNormals = new Vector3[mNormals_Size];

            for (int i = 0; i < walkBoxes.mNormals.Length; i++)
            {
                walkBoxes.mNormals[i] = GetVector3(fileBytes, ref bytePointerPosition);
            }

            //not necessary, but to check if we are where we should be at
            ArrayCheckAdjustment(bytePointerPosition_beforeNormals, mNormals_Capacity, ref bytePointerPosition);

            //---------------------------mQuads--------------------------
            uint bytePointerPosition_beforeQuads = bytePointerPosition;
            uint mQuads_Capacity = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mQuads Capacity: '{0}'", mQuads_Capacity);

            uint mQuads_Size = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition);
            Console.WriteLine("mQuads Size: '{0}'", mQuads_Size);

            walkBoxes.mQuads = new Quad[mQuads_Size];

            for (int i = 0; i < walkBoxes.mQuads.Length; i++)
            {
                Quad quad = new Quad()
                {
                    mVerts = new int[4]
                    {
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                        ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition)
                    }
                };

                walkBoxes.mQuads[i] = quad;
            }

            //not necessary, but to check if we are where we should be at
            ArrayCheckAdjustment(bytePointerPosition_beforeQuads, mQuads_Capacity, ref bytePointerPosition);

            ReachedEndOfFile(bytePointerPosition, (uint)fileBytes.Length);

            return walkBoxes;
        }

        public static Vector3 GetVector3(byte[] fileBytes, ref uint bytePointerPosition)
        {
            return new Vector3()
            {
                x = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition),
                y = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition),
                z = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition)
            };
        }

        public static Edge GetEdge(byte[] fileBytes, ref uint bytePointerPosition)
        {
            return new Edge()
            {
                mFlags = new Flags()
                {
                    mFlags = ByteFunctions.ReadUnsignedInt(fileBytes, ref bytePointerPosition)
                },
                mV1 = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mV2 = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDest = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDestEdge = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mEdgeDir = ByteFunctions.ReadInt(fileBytes, ref bytePointerPosition),
                mMaxRadius = ByteFunctions.ReadFloat(fileBytes, ref bytePointerPosition)
            };
        }

        public static void ArrayCheckAdjustment(uint pointerPositionBeforeCapacity, uint arrayCapacity, ref uint bytePointerPosition)
        {
            uint estimatedOffPoint = pointerPositionBeforeCapacity + ((uint)arrayCapacity);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("(Array Check) Estimated to be at = {0}", estimatedOffPoint);

            if (bytePointerPosition != estimatedOffPoint)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("(Array Check) Left off at = {0}", bytePointerPosition);
                Console.WriteLine("(Array Check) Skipping by using the estimated position...", bytePointerPosition);
                bytePointerPosition = estimatedOffPoint;
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
                Console.WriteLine("(Array Check) Left off at = {0}", bytePointerPosition);
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        }

        public static void ReachedEndOfFile(uint bytePointerPosition, uint fileSize)
        {
            if (bytePointerPosition != fileSize)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("Didn't reach the end of the file!");
                Console.WriteLine("Left off at = {0}", bytePointerPosition);
                Console.WriteLine("File Size = {0}", fileSize);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
                Console.WriteLine("Reached end of file!");
                Console.WriteLine("Left off at = {0}", bytePointerPosition);
                Console.WriteLine("File Size = {0}", fileSize);
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        }
    }
}
