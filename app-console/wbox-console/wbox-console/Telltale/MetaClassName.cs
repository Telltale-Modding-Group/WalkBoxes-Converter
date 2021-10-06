using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wbox_console.Telltale
{
    public struct MetaClassName
    {
        public Symbol mTypeNameCRC;
        public uint mVersionCRC;

        public override string ToString()
        {
            return string.Format("[ClassNames] mTypeNameCRC: ({0}) mVersionCRC: {1}", mTypeNameCRC, mVersionCRC);
        }
    }
}
