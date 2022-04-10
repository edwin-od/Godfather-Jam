using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public Sound[] sounds;

    public static SoundManager Get;
    private void Awake() {
        if (Get == null)
        {
            Get = this;
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            
            if(s.name == Sound.soundNames.MusicSound) {
                s.source.loop = true;
                s.source.Play();
            }
        }
    }

    public void Play(Sound.soundNames soundName) {
        if(soundName == Sound.soundNames.None || soundName == Sound.soundNames.MusicSound) {
            Debug.LogWarning("Specified sound you tried to play is refered as \'None\' or \'MusicSound\'!");
            return;
        }

        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        s.source.Play();
    }
    
}
