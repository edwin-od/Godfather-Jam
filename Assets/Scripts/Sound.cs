using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public enum soundNames
    {
        None,
        MusicSound,
        CorrectSound,
        IncorrectSound,
        MatchConfirm,
        Emballage,
        EatBonbon,
        MultiplierMileStone,
        MenuClick,
        PimentSpam,
        PimentMange,
        PimentDebarasse,
        GoldBonbon
    }

    public AudioClip clip;

    public soundNames name;

    [Range(0f, 1f)]
    public float volume;

    [Range(-3f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

}
