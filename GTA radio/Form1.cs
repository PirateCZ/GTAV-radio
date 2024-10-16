﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace GTA_radio
{
    public partial class Radio : Form
    {
        private MediaPlayer media;
        public Radio()
        {
            InitializeComponent();
            media = new MediaPlayer();
            media.ChangeVolume(volumeScroll.Value);
            media.LoadSongs();
            media.LoadIntros();
            media.PlaySong();
        }

        private void volumeScroll_Scroll(object sender, EventArgs e)
        {
            media.ChangeVolume(volumeScroll.Value);
        }
    }
}
