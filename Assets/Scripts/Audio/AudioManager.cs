namespace WGJ.PuppetShadow
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Act as a Singleton Dictionary so other objects can access to any sound they want to play.
    /// It must be filled in the inspector with all music and sounds.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {

        /* 
         * 
         * All Sounds (including music) are encapsulated in the Sound class to parametrize them.
         * (See Sound and SoundList classes)
         * 
         * To be honest, it's not a very efficient solution.
         * 
         * This whole game current Audio management is a ugly workaround built around Brackey's AudioManager and Sound class.
         * (See Brackey's Youtube Channel)
         * */

        //Individual Sounds
        [SerializeField]
        private Sound[] sounds;

        //List of similar sounds that can be swapped.
        [SerializeField]
        private SoundList[] soundLists;
        
        //Globals sounds

        //Main music
        public Sound music;

        //Ambient Sounds
        public Sound ambientSound;

        public Sound[] Sounds { get => sounds; set => sounds = value; }
        public SoundList[] SoundLists { get => soundLists; set => soundLists = value; }

        void Awake()
        {

            //Initialize sounds lists
            foreach (SoundList list in soundLists)
            {
                soundLists.Initialize();
            }


            //Initialize the music
            if (music != null)
            {
                music.source = gameObject.AddComponent<AudioSource>();
                LoadMusic(music);
            }
            //Initialize the ambient sounds
            if (ambientSound != null)
            {
                ambientSound.source = gameObject.AddComponent<AudioSource>();
                LoadAmbient(ambientSound);
            }

            //Play them
            music.source.Play();
            ambientSound.source.Play();

        }

        #region Public Methods

        /// <summary>
        /// Load the given Sound in the music source slot.
        /// </summary>
        /// <param name="mus"></param>
        public void LoadMusic(Sound mus)
        {
            LoadSound(music.source, mus);

        }


        /// <summary>
        /// Load the given Sound in the ambient music source slot.
        /// </summary>
        /// <param name="mus"></param>
        public void LoadAmbient(Sound mus)
        {
            LoadSound(ambientSound.source, mus);
        }

        /// <summary>
        /// Load a Sound in the given source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sound"></param>
        public static void LoadSound(AudioSource source, Sound sound)
        {
            source.clip = sound.clip;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.loop = sound.Loop;
            source.minDistance = sound.MinDistance;
            source.maxDistance = sound.MaxDistance;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Linear;
        }

        /// <summary>
        /// Return the Sound object with the given name. 
        /// Used by other objets to access the sounds they want to play.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return the SoundList with the given name. 
        /// Used by other objets to access the sounds they want to play.        
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoundList FindList(string name)
        {
            SoundList s = Array.Find(soundLists, list => list.Name == name);
            if (s == null)
            {
                Debug.LogWarning("SoundList:" + name + " not found!");
            }
            return s;
        }

        #endregion


    }
}