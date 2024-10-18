using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace GTA_radio
{
    internal class Intro : Audio
    {
        private string songName;
        public Intro(string path)
        {
            this.path = path;
            songName = GetSongName();
        }

        public string SongName { get { return songName; } }

        private string GetSongName()
        {
            int length = path.Length - path.LastIndexOf('\\') - 8;
            return path.Substring(path.LastIndexOf('\\') + 1, length);
        }
    }
}
