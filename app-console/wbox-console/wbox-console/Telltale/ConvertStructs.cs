using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace wbox_console.Telltale
{
    public static class ConvertStructs
    {
        public static Vector3 Get_Vector3(byte[] byteArray)
        {
            Vector3 vector3 = new Vector3();

            int size = Marshal.SizeOf(vector3);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            vector3 = (Vector3)Marshal.PtrToStructure(ptr, vector3.GetType());
            Marshal.FreeHGlobal(ptr);

            return vector3;
        }

        public static Flags Get_Flags(byte[] byteArray)
        {
            Flags flags = new Flags();

            int size = Marshal.SizeOf(flags);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            flags = (Flags)Marshal.PtrToStructure(ptr, flags.GetType());
            Marshal.FreeHGlobal(ptr);

            return flags;
        }

        public static Edge Get_Edge(byte[] byteArray)
        {
            Edge edge = new Edge();

            int size = Marshal.SizeOf(edge);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            edge = (Edge)Marshal.PtrToStructure(ptr, edge.GetType());
            Marshal.FreeHGlobal(ptr);

            return edge;
        }

        public static Tri Get_Tri(byte[] byteArray)
        {
            Tri tri = new Tri();

            int size = Marshal.SizeOf(tri);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            tri = (Tri)Marshal.PtrToStructure(ptr, tri.GetType());
            Marshal.FreeHGlobal(ptr);

            return tri;
        }

        public static Quad Get_Quad(byte[] byteArray)
        {
            Quad quad = new Quad();

            int size = Marshal.SizeOf(quad);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            quad = (Quad)Marshal.PtrToStructure(ptr, quad.GetType());
            Marshal.FreeHGlobal(ptr);

            return quad;
        }

        public static Vert Get_Vert(byte[] byteArray)
        {
            Vert vert = new Vert();

            int size = Marshal.SizeOf(vert);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            vert = (Vert)Marshal.PtrToStructure(ptr, vert.GetType());
            Marshal.FreeHGlobal(ptr);

            return vert;
        }

        public static WalkBoxes Get_WalkBoxes(byte[] byteArray)
        {
            WalkBoxes walkBoxes = new WalkBoxes();

            int size = Marshal.SizeOf(walkBoxes);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(byteArray, 0, ptr, size);

            walkBoxes = (WalkBoxes)Marshal.PtrToStructure(ptr, walkBoxes.GetType());
            Marshal.FreeHGlobal(ptr);

            return walkBoxes;
        }
    }
}
