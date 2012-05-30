using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCommon
{
    //should be moved to a different package. later
    public class VideoUserOptions
    {
        public bool SlowMo { get; set; }

        public VideoUserOptions()
        {
            SlowMo = false;
        }
    }
}
