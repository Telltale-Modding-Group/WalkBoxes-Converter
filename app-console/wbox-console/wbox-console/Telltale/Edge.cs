using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbox_console.Telltale
{
    public struct Edge
    {
        public Flags mFlags;
        public long mV1;
        public long mV2;
        public long mEdgeDest;
        public long mEdgeDestEdge;
        public long mEdgeDir;
        public float mMaxRadius;
    }
}
