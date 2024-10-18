using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GTA_radio
{
    internal class Song : Audio
    {
        private int beginDelaySec;

        public Song(string path, int delay)
        {
            this.path = path;
            beginDelaySec = delay;
        }        
    }
}