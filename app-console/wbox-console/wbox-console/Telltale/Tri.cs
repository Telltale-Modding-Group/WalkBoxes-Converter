using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbox_console.Telltale
{
    /*
    NOTE:
    Any variables with a star symbol *
    Are not actually in the original data struct, but are serialized in the file.
    However, they are just as important as the data itself.
    */

    public struct Tri
    {
        public uint mFootstepMaterial_BlockSize; //* (always 8)
        public EnumMaterial mFootstepMaterial;

        public Flags mFlags;
        public int mNormal;
        public int mQuadBuddy;
        public float mMaxRadius;

        public uint mVerts_BlockSize; //* (always 16)
        public int[] mVerts; //SArray<int,3>

        public uint mEdgeInfo_BlockSize; //* (always 76)
        public Edge[] mEdgeInfo; //SArray<WalkBoxes::Edge,3>

        public uint mVertOffsets_BlockSize; //* (always 16)
        public int[] mVertOffsets; //SArray<int,3> 

        public uint mVertScales_BlockSize; //* (always 16)
        public float[] mVertScales; //SArray<float,3>
    }
}
