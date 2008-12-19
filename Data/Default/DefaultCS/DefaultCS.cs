using System;
using NCHLOE;
using NCHLOE.Core;
using NCHLOE.Elements;

namespace CHLOE
{
    public class Compiler
    {
        public static void EntryPoint(CCHLOECore c)
        {
            CCHLOELinkRootArray root = c.get_LinkArray("inspection_objects");
        }
    }
}