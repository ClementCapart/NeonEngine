using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace NeonEngine
{
    static public class SoundManager
    {
        #region fields
        static Dictionary<string, SoundEffect> soundsList;
        static public Dictionary<string, string> sounds;
        static Dictionary<string, Song> songsList;
        static public Dictionary<string, string> songs;
        #endregion

        static public void LoadSounds()
        {
            sounds = new Dictionary<string, string>();
            songs = new Dictionary<string, string>();
            /* use sounds.add("tag", "filePath") to load your sounds
             * the tag will be use in your entities to call your sounds
             * 
             * the filepath is the absolute path to your file from your projet content root w/o the extention
             * 
             * ex : sounds.Add("menuPlayButton", @"menu\buttons\play");
             */

            /* WARNING : if you are using mp3 files, add them to songs, if you are using wav or wma files, you can add it to sounds */

            //assets.Add("", @"");
            //assets.Add("", @"");
            //assets.Add("", @"");
            if(Directory.Exists(@"Content/SFX"))
            {
                foreach (string p in Directory.EnumerateFiles(@"Content/SFX"))
                {
                    sounds.Add(Path.GetFileNameWithoutExtension(p), @"SFX/" + Path.GetFileNameWithoutExtension(p));
                }
            }
            
            songs.Add("Demo", @"Soundtracks/demo");


        }

        static public void Load(ContentManager Content)
        {
            soundsList = new Dictionary<string, SoundEffect>();
            foreach (KeyValuePair<string, string> kvp in sounds)
                soundsList.Add(kvp.Key, Content.Load<SoundEffect>(kvp.Value));
            songsList = new Dictionary<string, Song>();
            foreach (KeyValuePair<string, string> kvp in songs)
                songsList.Add(kvp.Key, Content.Load<Song>(kvp.Value));
        }

        static public SoundEffect GetSound(string tag)
        {
            return soundsList[tag];
        }

        static public Song GetSong(string tag)
        {
            return songsList[tag];
        }
    }
}
