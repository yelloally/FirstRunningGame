using UnityEngine;
using System.Collections;
using static UnityEngine.UI.Image;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return instance; } }
    private static AudioManager instance;

    //volume of the background music
    [SerializeField] private float musicVolume = 1;

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfxSource;

    //determine which music is currently active
    private bool firstMusicSourceActive;

    private void Awake()
    {
        instance = this;

        //to make sure that this audio source persists in between scenes even though we are not switching scene 
        DontDestroyOnLoad(this.gameObject);

        //add audio sources to the object
        music1 = gameObject.AddComponent<AudioSource>();
        music2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        music1.loop = true;
        music2.loop = true;
    }

    //play sound efect
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    //play sound efect with special volume
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    //play background music with crossfade transition
    public void PlayMusicWithXFade (AudioClip musicClip, float transitionTime = 1.0f)
    {
        //determine which source is active
        AudioSource activeSource = (firstMusicSourceActive) ? music1 : music2;
        AudioSource newSource = (firstMusicSourceActive) ? music1 : music2;

        //switch music
        firstMusicSourceActive = !firstMusicSourceActive;

        //set clip and play another music (background)
        newSource.clip = musicClip;
        newSource.Play();
        //update volume during the transition
        StartCoroutine(UpdateMusicWithXFade(activeSource, newSource, musicClip, transitionTime));
    }

    public void toggleMute()
    {
        AudioSource a = GetComponent<AudioSource>();
        if (a.isPlaying)
        {
            a.Pause();
        }
        else
        {
            a.Play();
        }
    }

    private IEnumerator UpdateMusicWithXFade(AudioSource original, AudioSource newSource, AudioClip music, float transitionTime)
    {
        //make sure the source is active and playing
        if (!original.isPlaying)
            original.Play();

        //stop background music and set clip
        newSource.Stop();
        newSource.clip = music;
        newSource.Play();

        //loop until the end of the transition time
        float t = 0.0f;
        for (t = 0.0f; t <=transitionTime; t += Time.deltaTime)
        {
            //upate the volume levels
            original.volume = musicVolume - ((t / transitionTime) * musicVolume);
            newSource.volume = (t / transitionTime) * musicVolume;
            //wait for the next frame
            yield return null;
        }

        //muting the music
        original.volume = 0;
        //set the volume of the new audio
        newSource.volume = musicVolume;

        //stop playback on the oroginal audio source
        original.Stop(); 
    }
} 
