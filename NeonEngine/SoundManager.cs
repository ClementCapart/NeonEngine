using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Xna.Framework;
using System.Threading;

namespace NeonEngine
{
    static public class SoundManager
    {
        #region fields
        static Dictionary<string, SoundEffect> soundsList;
        static public Dictionary<string, string> sounds;
        static Dictionary<string, Song> songsList;
        static public Dictionary<string, string> songs;

        static public string CurrentTrackName = "";
        static public SoundEffectInstance CurrentTrack;
        static private bool _currentTrackMustLoop = false;
        static public string NextTrackName = "";
        static public SoundEffectInstance NextTrack;
        static private bool _nextTrackMustLoop = false;
        static public bool MusicLock = false;
        static public float MusicTimer = -1.0f;

        static private bool _fading = false;

        static public float GlobalPitch = -0.2f;
        static public float GlobalEffectsVolume = 0.3f;
        static public float GlobalSoundtrackVolume = 0.2f;

        static public Thread StoppedEventThread;
        #endregion

        static public void StartLoopTrack(string name)
        {           
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            if (CurrentTrack != null)
                CurrentTrack.Stop();
            CurrentTrack = se.CreateInstance();
            CurrentTrackName = name;
            CurrentTrack.Volume = GlobalSoundtrackVolume;
            CurrentTrack.Play();
            _currentTrackMustLoop = true;
        }

        static public void StartTrack(string name)
        {
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            if (CurrentTrack != null)
                CurrentTrack.Stop();
            CurrentTrack = se.CreateInstance();
            CurrentTrackName = name;
            CurrentTrack.Volume = GlobalSoundtrackVolume;
            CurrentTrack.Play();
            _currentTrackMustLoop = false;
        }

        static public void PrepareLoopTrack(string name)
        {
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            NextTrack = se.CreateInstance();
            NextTrackName = name;
            NextTrack.Volume = GlobalSoundtrackVolume;
            _nextTrackMustLoop = true;
        }

        static public void PrepareTrack(string name)
        {
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            NextTrackName = name;
            NextTrack = se.CreateInstance();
            NextTrack.Volume = GlobalSoundtrackVolume;
            _nextTrackMustLoop = false;
        }

        static public void CrossFadeLoopTrack(string name)
        {
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            NextTrack = se.CreateInstance();
            NextTrackName = name;
            NextTrack.Volume = 0.0f;
            NextTrack.Play();
            _nextTrackMustLoop = true;
            _fading = true;
        }

        static public void CrossFadeTrack(string name)
        {          
            SoundEffect se = GetSound(name);
            if (se == null)
                return;
            NextTrackName = name;
            NextTrack = se.CreateInstance();
            NextTrack.Volume = 0.0f;
            NextTrack.Play();
            _nextTrackMustLoop = false;
            _fading = true;
        }

        static public void Update(GameTime gameTime)
        {
            if (_fading)
            {
                if (NextTrack != null)
                {
                    if (NextTrack.Volume < GlobalSoundtrackVolume)
                    {
                        if(CurrentTrack != null)
                            CurrentTrack.Volume = Math.Max(CurrentTrack.Volume - GlobalSoundtrackVolume * (float)gameTime.ElapsedGameTime.TotalSeconds / 2, 0.0f) ;
                        NextTrack.Volume = Math.Min(NextTrack.Volume + GlobalSoundtrackVolume * (float)gameTime.ElapsedGameTime.TotalSeconds / 2, GlobalSoundtrackVolume);
                    }
                    else
                    {
                        CurrentTrackName = NextTrackName;
                        CurrentTrack = NextTrack;
                        CurrentTrack.Volume = GlobalSoundtrackVolume;
                        _currentTrackMustLoop = _nextTrackMustLoop;
                        NextTrack = null;
                        NextTrackName = "";
                        _fading = false;
                    }
                }
            }

            if (CurrentTrack != null && CurrentTrack.State == SoundState.Stopped)
            {
                if (NextTrack == null)
                {
                    if(_currentTrackMustLoop)
                        CurrentTrack.Play();
                }
                else
                {
                    CurrentTrack = NextTrack;
                    NextTrack = null;
                    CurrentTrackName = NextTrackName;
                    NextTrackName = "";
                    _currentTrackMustLoop = _nextTrackMustLoop;
                    CurrentTrack.Play();
                }
            }
            //MusicTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        static public void InitializeSounds()
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

            if (Directory.Exists(@"Content/Soundtracks"))
            {
                foreach (string p in Directory.EnumerateFiles(@"Content/Soundtracks"))
                {
                    if(Path.GetExtension(p) == ".xnb")
                        sounds.Add(Path.GetFileNameWithoutExtension(p), @"Soundtracks/" + Path.GetFileNameWithoutExtension(p));
                }
            }
            


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
    }
}
