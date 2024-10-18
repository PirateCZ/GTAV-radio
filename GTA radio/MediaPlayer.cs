using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using WMPLib;

namespace GTA_radio
{
    internal class MediaPlayer
    {
        private string station;
        private string mainPath;
        private WindowsMediaPlayer djPlayer;
        private WindowsMediaPlayer songPlayer;
        private List<Song> songList;
        private List<Intro> songIntroList;
        Random rnd = new Random();

        public MediaPlayer()
        {
            songList = new List<Song>();
            songPlayer = new WindowsMediaPlayer();
            songPlayer.PlayStateChange += SongPlayerState;

            songIntroList = new List<Intro>();
            djPlayer = new WindowsMediaPlayer();

            station = "nonstoppop";
            //station = "westcoastclassic";
            mainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "sound", station);
        }

        public void PlaySong()
        {
            if (songList.Count <= 0) LoadSongs();
            int rndIndex = rnd.Next(0, songList.Count);

            songPlayer.controls.stop();
            songPlayer.URL = songList[rndIndex].Path;
            songPlayer.controls.play();

            try { djPlayer.controls.stop(); djPlayer.URL = GetIntros().Path; djPlayer.controls.play(); } catch { }
            songList.RemoveAt(rndIndex);
        }

        public void LoadSongs()
        {
            try
            {
                string songsPath = Path.Combine(mainPath, "songs");
                var songs = Directory.GetFiles(songsPath, "*.wav").ToList();
                foreach (var song in songs)
                {
                    songList.Add(new Song(song, 0));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoadIntros()
        {
            try
            {
                string introsPath = Path.Combine(mainPath, "speach", "b4music");
                var intros = Directory.GetFiles(introsPath, "*.wav").ToList();
                foreach (var intro in intros)
                {
                    songIntroList.Add(new Intro(intro));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Intro GetIntros()
        {
            var tempList = new List<Intro>();
            foreach (var item in songIntroList)
            {
                if (songPlayer.URL.Contains(item.SongName))
                {
                    tempList.Add(item);
                }
            }
            return tempList.Count <= 0 ? null : tempList[rnd.Next(0, tempList.Count)];
        }

        public void ChangeVolume(int volume)
        {
            songPlayer.settings.volume = volume;
            djPlayer.settings.volume = volume;
        }

        private void SongPlayerState(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsMediaEnded)
            {
                PlaySong();
            }
        }
    }
}
