using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WMPLib;

namespace GTAV_radio_stations
{
    public partial class mediaPlayer : Form
    {

        string baseDirPath = AppDomain.CurrentDomain.BaseDirectory; //gets this path "\GTAV radio stations\bin\Debug\"
        string station = "nonstoppop"; //determines the station thats playing
        int volume; // value of the player volume
        bool speachHasPlayed = false; //determines if the dj already talked so it doesnt talk twice
        int counter = 0;

        //create a rnd var
        Random rnd = new Random();

        //these are for songs
        List<string> songsList;
        WindowsMediaPlayer songPlayer;

        //these are for the DJ speach
        List<string> speachList;
        WindowsMediaPlayer speachPlayer;
        List<string> djLoreList;
        WindowsMediaPlayer lorePlayer;

        //start
        public mediaPlayer()
        {
            //basic stuff
            InitializeComponent();
            
            //doesnt work really well cuz the dj is such a yapper
            //station = "westcoastclassic";

            //import sound files
            LoadSoundFiles(Path.Combine(baseDirPath, "res", "sound", station, "songs"), ref songsList);
            LoadSoundFiles(Path.Combine(baseDirPath, "res", "sound", station, "speach", "b4music"), ref speachList);
            LoadSoundFiles(Path.Combine(baseDirPath, "res", "sound", station, "speach", "djlore"), ref djLoreList);

            //declare media players
            songPlayer = new WindowsMediaPlayer();
            songPlayer.PlayStateChange += Player1_PlayStateChange;
            speachPlayer = new WindowsMediaPlayer();
            speachPlayer.PlayStateChange += Player2_PlayStateChange;
            lorePlayer = new WindowsMediaPlayer();
            lorePlayer.PlayStateChange += Player3_PlayStateChange;


            //change default volume
            ChangeVolume();

            //start playback
            PlayMusic();
        }
        private void PlayRadio()
        {
            if (rnd.NextDouble() < 0.1)
            {
                lorePlayer = new WindowsMediaPlayer();
                lorePlayer.URL = djLoreList[counter++];
                lorePlayer.PlayStateChange += Player3_PlayStateChange;
                ChangeVolume();
                speachHasPlayed = true;
            }
            else
            {
                PlayMusic();
            }
        }

        private void PlayMusic()
        {
            //initialize bs
            int rndIndex = rnd.Next(0, songsList.Count);
            List<string> tempList = new List<string>();

            //if song run out load new ones
            if (songsList.Count < 0) LoadSoundFiles(Path.Combine(baseDirPath, "res", "sound", "songs"), ref songsList);
            
            //get all intro to songs 
            //this is here cuz some song have multiple intros
            for (int i = 0; i < speachList.Count; i++)
            {
                if (CompareUntilChar(Path.GetFileName(songsList[rndIndex]), Path.GetFileName(speachList[i])))
                {
                    tempList.Add(speachList[i]);
                }
            }

            //play intro to song
            if (speachHasPlayed == false)
            {
                speachPlayer = new WindowsMediaPlayer();
                speachPlayer.URL = tempList.Count == 0 ? null : tempList[rnd.Next(0, tempList.Count)];
                speachPlayer.PlayStateChange += Player2_PlayStateChange;
            }
            speachHasPlayed = false;

            //play song and remove so no repeats
            songPlayer = new WindowsMediaPlayer();
            songPlayer.URL = songsList[rndIndex];
            songPlayer.PlayStateChange += Player1_PlayStateChange;
            songsList.RemoveAt(rndIndex);

            ChangeVolume();
        }

        //change volume to volumeSlider value
        private void volumeSlider_Scroll(object sender, EventArgs e)
        {
            ChangeVolume();
        }
        private void ChangeVolume()
        {
            volume = volumeSlider.Value;
            songPlayer.settings.volume = volume;
            speachPlayer.settings.volume = volume;
            lorePlayer.settings.volume = volume;
        }

        //load files into a list
        private void LoadSoundFiles(string folderPath, ref List<string> target)
        {
            //try to add files to the target
            try
            {
                target = Directory.GetFiles(folderPath, "*.wav").ToList();
            }
            //write error msg
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        //compare first word
        private bool CompareUntilChar(string str1, string str2)
        {
            //find position where first word ends
            int index1 = str1.IndexOf('_');
            int index2 = str2.IndexOf('_');

            // create substrings of the first word
            string substr1 = str1.Substring(0, index1);
            string substr2 = str2.Substring(0, index2);

            //if first word equal then return true
            return substr1.Equals(substr2);
        }

        //event for changing !songPlayer!
        private void Player1_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsMediaEnded)
            {
                PlayRadio();
            }
        }
        private void Player2_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsReady)
            {
            }
        }
        private void Player3_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsMediaEnded)
            {
                PlayMusic();
            }
        }

        private void radioLogo_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}