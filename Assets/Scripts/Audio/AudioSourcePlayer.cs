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
        private class Playing
        {
            public Sound Sound;
            public AudioSource Source;
            public string Name;
            public Action OnPlayEnd;
        }

        private void Update()
        {
            currentlyPlayedCount = currentlyPlaying.Count;
            availableCount = availableAudioSources.Count;
        }

        public void PlayRandomFromList(string listName, Action onEndPlay = null)
        {
            //Does the sound exist ?
            SoundList soundList = AudioManager.Instance.FindList(listName);

            if (soundList == null) return;


            if (IsCurrentlyPlayed(listName))
            {
                return;
            }


            Sound sound = soundList.GetRandom();

            PlaySound(sound, listName, onEndPlay);
        }

        public void PlaySound(string name, Action onEndPlay = null)
        {
            
            if (IsCurrentlyPlayed(name))
            {
                return;
            }

            //Does the sound exist ?
            Sound sound = AudioManager.Instance.Find(name);

            if (sound == null) return;


            PlaySound(sound, name, onEndPlay);
            
        }

        public void InterruptSound(string name)
        {

            //Does the sound exist ?
            Playing play;

            if (!IsCurrentlyPlayed(name, out play))
            {
                return;
            }
            
            FreeAudioSource(play.Source);
        }

        public bool IsCurrentlyPlayed(string name)
        {
            Playing play = null;
            return IsCurrentlyPlayed(name, out play);
        }

        private bool IsCurrentlyPlayed(string name, out Playing play)
        {
            play = null;
            foreach (Playing playing in currentlyPlaying)
            {
                if (playing.Name.Equals(name))
                {
                    play = playing;
                    return true;
                }
            }
            return false;
        }

        private bool IsCurrentlyPlayed(Playing play)
        {
            return currentlyPlaying.Contains(play);
        }

        private void PlaySound(Sound sound, string name, Action onEndPlay = null)
        {
            AudioSource source;

            source = GetAvailableAudioSource();
            source.enabled = true;

            Playing playing = new Playing();
            playing.Name = name;
            playing.Sound = sound;
            playing.Source = source;
            playing.OnPlayEnd = onEndPlay;
            currentlyPlaying.Add(playing);

            if (!sound.Loop)
            {
                StartCoroutine(_PlaySoundOnce(playing));
            }
            else
            {
                AudioManager.LoadSound(source, sound);
                source.Play();
            }
        }

        private IEnumerator _PlaySoundOnce(Playing playing)
        {
            AudioSource source = playing.Source;
            Sound sound = playing.Sound;

            AudioManager.LoadSound(source, sound);
            source.loop = false;
            source.Play();
            

            yield return new WaitForSeconds(source.clip.length);

            if (!IsCurrentlyPlayed(playing))
            {
                yield break;
            }

            FreeAudioSource(source);
            playing.OnPlayEnd?.Invoke();
            
        }

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
