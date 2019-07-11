namespace WGJ.PuppetShadow
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Encapsulate a list of sounds, used most often when sounds are swappable and can be used in the same scenario,
    /// so we fetch a random one from the list.
    /// </summary>
    [System.Serializable]
    public class SoundList
    {
        [SerializeField]
        private string listName;
        [SerializeField]
        private Sound[] sounds;

        /// <summary>
        /// Initialize all sounds with this list's name, to which they belong.
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].ListName = listName;
            }
        }

        /// <summary>
        /// Find a sound with the given name in the list. 
        /// Return null if not found. (This is called through the AudioManager that will output a error log)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sound Find(string name)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            return s;
        }

        /// <summary>
        /// Get a random sound from the List
        /// </summary>
        /// <returns></returns>
        public Sound GetRandom()
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        public Sound[] Sounds { get => sounds; set => sounds = value; }
        public string Name { get => listName; set => listName = value; }
    }
}