using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;

    public AudioClip bg1;
    public AudioClip bg2;
    public AudioClip bg3;
    
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = bg1;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*private AudioClip FindInClips(string clip)
    {
        return clips.Find(x => x.name.Equals(clip));
    }*/
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlayOnLoop(AudioClip clip)
    {
        
        if (!sfxSource.isPlaying)
        {
            PlaySFX(clip);
        }
    }
}
