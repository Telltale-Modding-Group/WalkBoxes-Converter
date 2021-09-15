using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbox_console.Telltale
{
    public struct WalkBoxes
    {
        public string mName;
        public Tri[] mTris; //DCArray<WalkBoxes::Tri>
        public Vert[] mVerts; //DCArray<WalkBoxes::Vert>
        public Vector3[] mNormals; //DCArray<Vector3>
        public Quad[] mQuads; //DCArray<WalkBoxes::Quad>
    }
}
