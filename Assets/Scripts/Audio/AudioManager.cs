namespace WGJ.PuppetShadow
{
    using UnityEngine.Audio;
    using System;
    using UnityEngine;

    public class AudioManager : Singleton<AudioManager>
    {

        /* 
         * AudioManager, contains every Sounds ans music played, in a Sound[] array, same for musics.
         * */

        //Sounds
        [SerializeField]
        private Sound[] sounds;


        [SerializeField]
        private SoundList[] soundLists;

        //Musics
        public Sound[] musics;
        int num_music = 0;
        public Sound music;

        public Sound[] Sounds { get => sounds; set => sounds = value; }
        public SoundList[] SoundLists { get => soundLists; set => soundLists = value; }

        void Awake()
        {

            foreach (SoundList list in soundLists)
            {
                soundLists.Initialize();
            }



            //Choose a random first music.
            //num_music = UnityEngine.Random.Range(0, musics.Length);

            //We initialize the music the same way than for the sound, but just once this time.
            music.source = gameObject.AddComponent<AudioSource>();
            LoadMusic(musics[num_music]);
            music.source.Play();

        }

        #region Public Methods
        public void LoadMusic(Sound mus)
        {
            LoadSound(music.source, mus);

        }
        public static void LoadSound(AudioSource source, Sound sound)
        {
            source.clip = sound.clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.Loop;
            source.minDistance = source.minDistance;
            source.maxDistance = source.maxDistance;
        }

        //Return the Sound object with the name given. Used by other objets to access the sounds they want to play.
        public Sound Find(string name)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            if (s == null)
            {
                foreach (SoundList list in soundLists)
                {
                    s = list.Find(name);
                    if (s != null) break;
                }
                if (s == null)
                {
                    Debug.LogWarning("Sound:" + name + " not found!");
                }
            }
            return s;
        }

        public SoundList FindList(string name)
        {
            SoundList s = Array.Find(soundLists, list => list.Name == name);
            if (s == null)
            {
                Debug.LogWarning("SoundList:" + name + " not found!");
            }
            return s;
        }

        //Mute all SFX sounds
        public void MuteAllSounds()
        {
            foreach (Sound s in Sounds)
                s.source.mute = true;
        }
        #endregion

        //Unmute all sounds

        //Pause all sounds.
        public void PauseAll()
        {
            foreach (Sound s in Sounds)
                s.source.Pause();
            music.source.Pause();
        }
        //Resume all sounds.
        public void ResumeAll()
        {
            foreach (Sound s in Sounds)
                s.source.UnPause();
            music.source.UnPause();
        }

        //Change the music for the next one in the array musics[]
        public void nextMusic()
        {
            if (musics.Length - 1 > num_music)
                num_music++;
            else
                num_music = 0;

            music.source.Stop(); //We stop the previous one.
                                 //We replace it with the new one.
            LoadMusic(musics[num_music]);
            music.source.Play();
        }

    }
}
