using UnityEngine;
using System.Collections;

//Can be exposed in editor
[System.Serializable]
//Holds source files/audio clips
public class MusicSourceContainer
{
    //Allows Unity to edit values in this array as if they were public
    [SerializeField]
    private AudioClip[] source;


    /// <summary>
    /// Gets a random song from this container
    /// </summary>
    /// <returns>The audio clip to be played</returns>
    public AudioClip GetSong()
    {
        return source[Random.Range(0, source.Length)];
    }
}



public class MusicScript : MonoBehaviour
{
    /// <summary>
    /// 5 Themes: Day, Sad, Spooky, Serene, Night
    /// Day: Upbeat, cheery, light soft melody
    /// Sad: Dreary, soft, powerful
    /// Spooky: Unsettling, off key, ambient
    /// Serene: ambient, highkey, calm, beautiful
    /// Night: stressful, soft, creepy, lonely
    /// </summary>
    public enum Themes { Day, Sad, Spooky, Serene, Night };

    //Audio sources
    public AudioSource musicSource1;
    public AudioSource musicSource2;

    //Crossfade logic variables
    public float crossfadeTime = 4.20f;
    private float crossfadeCurrent = 0;
    private float musicSourceCF1;
    private float musicSourceCF2;

    //Audio queue clip
    private AudioClip qdSong;
    
    private bool source1 = true;

    //Container array
    public MusicSourceContainer[] musicContainers;

    
    void Start()
    {

    }

    //Update
    void Update()
    {
        //Crossfading currently
        if (crossfadeCurrent > 0)
        {
            crossfadeCurrent -= Time.deltaTime;

            //Check for double fade
            if (qdSong == null)
            {
                //Source 1 out, source 2 in
                if (source1)
                {
                    musicSource1.volume = crossfadeCurrent / crossfadeTime;
                    musicSource2.volume = 1 - crossfadeCurrent / crossfadeTime;
                }

                //Source 2 out, source 1 in
                else
                {
                    musicSource2.volume = crossfadeCurrent / crossfadeTime;
                    musicSource1.volume = 1 - crossfadeCurrent / crossfadeTime;
                }
            }

            //Source 1 out, souce 2 out
            else
            {
                musicSource1.volume = Mathf.Lerp(musicSourceCF1, 0, 1 - crossfadeCurrent / crossfadeTime);
                musicSource2.volume = Mathf.Lerp(musicSourceCF2, 0, 1 - crossfadeCurrent / crossfadeTime);
            }
        }

        //Play queued song if not crossfading
        else
        {
            if (qdSong)
            {
                PlayMusic(qdSong);
            }
        }

    }

    //Allows for manual song selection(not player)
    private void PlayMusic(AudioClip song)
    {
        crossfadeCurrent = crossfadeTime;
        qdSong = null;
        if (source1)
        {
            musicSource2.clip = song;
            musicSource2.Play();
            musicSource2.volume = 0;
        }
        else
        {
            musicSource1.clip = song;
            musicSource1.Play();
            musicSource1.volume = 0;
        }
    }

    /// <summary>
    /// Plays a random song from a specific theme
    /// </summary>
    /// <param name="theme">Theme of song</param>
    public void PlayMusic(Themes theme)
    {
        AudioClip picked = musicContainers[(int)theme].GetSong();

        //No songs are playing, fade in song.
        if (!musicSource1.isPlaying & !musicSource2.isPlaying)
        {
            crossfadeCurrent = crossfadeTime;
            qdSong = null;
            if (source1)
            {
                musicSource2.clip = picked;
                musicSource2.Play();
                musicSource2.volume = 0;
            }
            else
            {
                musicSource1.clip = picked;
                musicSource1.Play();
                musicSource1.volume = 0;
            }
        }
        // One song is playing, crossfade.
        if (musicSource1.isPlaying ^ musicSource2.isPlaying)
        {
            crossfadeCurrent = crossfadeTime;
            qdSong = null;
            if (source1)
            {
                musicSource2.clip = picked;
                musicSource2.Play();
                musicSource2.volume = 0;
            }
            else
            {
                musicSource1.clip = picked;
                musicSource1.Play();
                musicSource1.volume = 0;
            }
        }
        //Two songs are playing, fade out both. 
        if (musicSource1.isPlaying && musicSource2.isPlaying)
        {
            qdSong = picked;
            musicSourceCF1 = musicSource1.volume;
            musicSourceCF2 = musicSource2.volume;

       
        }
    }
}
