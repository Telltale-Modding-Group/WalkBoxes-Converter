using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbox_console.Telltale
{
    public struct Tri
    {
        public EnumMaterial mFootstepMaterial;
        public Flags mFlags;
        public long mNormal;
        public long mQuadBuddy;
        public float mMaxRadius;
        public int[] mVerts; //SArray<int,3>
        public Edge[] mEdgeInfo; //SArray<WalkBoxes::Edge,3>
        public int[] mVertOffsets; //SArray<int,3> 
        public float[] mVertScales; //SArray<float,3>
    }
}
