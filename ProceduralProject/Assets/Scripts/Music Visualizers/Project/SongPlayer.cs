using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class SongPlayer : MonoBehaviour
{
    /// <summary>
    /// The current track being played.
    /// </summary>
    private int currentTrack = -1;
    /// <summary>
    /// The source of the music being played.
    /// </summary>
    private AudioSource player;
    /// <summary>
    /// The list of every song that can be played using this script.
    /// </summary>
    public AudioClip[] playlist;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        PlayTrackRandom();
    }

    /// <summary>
    /// Plays the selected track.
    /// </summary>
    /// <param name="n">The index of the track being selected.</param>
    public void PlayTrack(int n)
    {
        if (n < 0 || n >= playlist.Length) return;
        player.PlayOneShot(playlist[n]);
        currentTrack = n;
    }

    public void PlayTrackRandom() => PlayTrack(Random.Range(0, playlist.Length));

    /// <summary>
    /// Plays the track that comes after the current track.
    /// </summary>
    public void PlayTrackNext()
    {
        int track = currentTrack + 1;
        if (track >= playlist.Length) track = 0;
        PlayTrack(track);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying) PlayTrackNext();
    }
}

//[CustomEditor(typeof(SongPlayer))]
//public class Mus
