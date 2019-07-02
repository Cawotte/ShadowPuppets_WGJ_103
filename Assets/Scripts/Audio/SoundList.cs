namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    public class SoundList
    {
        [SerializeField]
        private string listName;
        [SerializeField]
        private Sound[] sounds;

        public void Initialize()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].ListName = listName;
            }
        }

        public Sound Find(string name)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            return s;
        }

        public Sound GetRandom()
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        public Sound[] Sounds { get => sounds; set => sounds = value; }
        public string Name { get => listName; set => listName = value; }
    }
}