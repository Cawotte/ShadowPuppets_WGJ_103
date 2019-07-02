namespace WGJ.PuppetShadow
{

    using UnityEngine.Audio;
    using UnityEngine;


    /*
     * Class used to define a sound, any playable sound clips.
     * The audio manager contains an Array of 'Sound' which will all contain a sound.
     * */

    [System.Serializable]
    public class Sound
    {

        public string name; //sound name
        public AudioClip clip; //sound asset

        [SerializeField]
        [Range(0f, 1f)]
        private float volume = 1f;
        [SerializeField]
        [Range(.1f, 3f)]
        private float pitch = 1f;
        [SerializeField]
        private bool loop = false;

        [SerializeField]
        private float minDistance = 0f;
        [SerializeField]
        private float maxDistance = 500f;

        //component which will play the sound
        [HideInInspector] public AudioSource source;
        public string ListName;

        public float Pitch { get => pitch; set => pitch = value; }
        public bool Loop { get => loop; set => loop = value; }
        public float MinDistance { get => minDistance; set => minDistance = value; }
        public float MaxDistance { get => maxDistance; set => maxDistance = value; }
        public float Volume { get => volume; set => volume = value; }
    }
}
