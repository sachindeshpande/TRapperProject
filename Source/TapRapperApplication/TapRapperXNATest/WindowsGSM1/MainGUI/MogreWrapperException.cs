using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;

namespace MainGUI
{
    public class MogreWrapperException : Exception
    {
        public MogreWrapperException(System.Runtime.InteropServices.SEHException e):base(e.Message)
        {
        }
    }
}
