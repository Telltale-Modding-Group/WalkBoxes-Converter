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

    public struct WalkBoxes
    {
        //META HEADER
        public string mMetaStreamVersion; //*
        public uint mDefaultSectionChunkSize; //*
        public uint mDebugSectionChunkSize; //*
        public uint mAsyncSectionChunkSize; //*
        public uint mClassNamesLength; //*
        public ClassName[] mClassNames; //*

        //WBOX DATA
        public uint mName_BlockSize; //*
        public uint mName_StringLength; //*
        public string mName;
        public uint mTris_Capacity; //*
        public int mTris_Size; //*
        public Tri[] mTris; //DCArray<WalkBoxes::Tri>
        public uint mVerts_Capacity; //*
        public int mVerts_Size; //*
        public Vert[] mVerts; //DCArray<WalkBoxes::Vert>
        public uint mNormals_Capacity; //*
        public int mNormals_Size; //*
        public Vector3[] mNormals; //DCArray<Vector3>
        public uint mQuads_Capacity; //*
        public int mQuads_Size; //*
        public Quad[] mQuads; //DCArray<WalkBoxes::Quad>
    }
}
