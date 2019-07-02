namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioSourcePlayer : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private List<Playing> currentlyPlaying = new List<Playing>();

        /*
        [SerializeField]
        [ReadOnly]
        private Dictionary<Sound, AudioSource> currentlyPlayed = new Dictionary<Sound, AudioSource>(); */

        [SerializeField]
        [ReadOnly]
        private Stack<AudioSource> availableAudioSources = new Stack<AudioSource>();

        [SerializeField]
        [ReadOnly]
        private int currentlyPlayedCount;

        [SerializeField]
        [ReadOnly]
        private int availableCount;

        [Serializable]
        private struct Playing
        {
            public Sound Sound;
            public AudioSource Source;
            public string Name;
        }

        private void Update()
        {
            currentlyPlayedCount = currentlyPlaying.Count;
            availableCount = availableAudioSources.Count;
        }

        public void PlayRandomFromList(string listName)
        {
            //Does the sound exist ?
            SoundList soundList = AudioManager.Instance.FindList(listName);

            if (soundList == null) return;


            if (IsCurrentlyPlayed(listName))
            {
                return;
            }


            Sound sound = soundList.GetRandom();

            PlaySound(sound, listName);
        }

        public void PlaySound(string name)
        {
            
            if (IsCurrentlyPlayed(name))
            {
                return;
            }

            //Does the sound exist ?
            Sound sound = AudioManager.Instance.Find(name);

            if (sound == null) return;


            PlaySound(sound, name);
            
        }

        public void InterruptSound(string name)
        {

            //Does the sound exist ?
            Sound sound = null;
            AudioSource source = null;

            if (!IsCurrentlyPlayed(name, out sound, out source))
            {
                return;
            }
            
            FreeAudioSource(source);
        }

        public bool IsCurrentlyPlayed(string name)
        {
            AudioSource source = null;
            Sound sound = null;
            return IsCurrentlyPlayed(name, out sound, out source);
        }

        private bool IsCurrentlyPlayed(string name, out Sound sound, out AudioSource source)
        {
            source = null;
            sound = null;
            foreach (Playing playing in currentlyPlaying)
            {
                if (playing.Name.Equals(name))
                {
                    source = playing.Source;
                    sound = playing.Sound;
                    return true;
                }
            }
            return false;
        }

        private void PlaySound(Sound sound, string name)
        {
            AudioSource source;

            source = GetAvailableAudioSource();
            source.enabled = true;

            Playing newSound;
            newSound.Name = name;
            newSound.Sound = sound;
            newSound.Source = source;
            currentlyPlaying.Add(newSound);

            if (!sound.Loop)
            {
                StartCoroutine(_PlaySoundOnce(source, sound));
            }
            else
            {
                AudioManager.LoadSound(source, sound);
                source.Play();
            }
        }

        private IEnumerator _PlaySoundOnce(AudioSource source, Sound sound)
        {
            AudioManager.LoadSound(source, sound);
            source.loop = false;
            source.Play();
            

            yield return new WaitForSeconds(source.clip.length);

            FreeAudioSource(source);
        }

        /*
        private bool IsCurrentlyPlayed(string listName, out AudioSource source)
        {
            source = null;
            foreach (Sound s in currentlyPlayed.Keys)
            {
                if (s.ListName.Equals(listName))
                {
                    source = currentlyPlayed[s];
                    return true;
                }
            }
            return false;
        }

        private bool IsCurrentlyPlayed(Sound sound, out AudioSource source)
        {
            source = null;
            foreach (Sound s in currentlyPlayed.Keys)
            {
                if (s.name.Equals(sound.name))
                {
                    source = currentlyPlayed[s];
                    return true;
                }
            }
            return false;
        } */

        private AudioSource GetAvailableAudioSource()
        {
            if (availableAudioSources.Count > 0)
            {
                return availableAudioSources.Pop();
            }
            return gameObject.AddComponent<AudioSource>();
        }

        private void FreeAudioSource(AudioSource source)
        {
            source.Stop();
            source.enabled = false;
            RemoveFromCurrentlyPlayed(source);
            availableAudioSources.Push(source);
        }

        private bool RemoveFromCurrentlyPlayed(AudioSource source) {
            foreach (Playing entry in currentlyPlaying)
            {
                if (entry.Source == source)
                {
                    currentlyPlaying.Remove(entry);
                    return true;
                }
            }
            return false;
        }
    }
}
