using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<AudioSource> _audioSources = new List<AudioSource>();
    private int counter = 0;
    private void Start()
    {
        foreach (AudioSource item in GetComponents<AudioSource>())
        {
            _audioSources.Add(item);
        }
    }
    public void ReplaySameAudio()
    {
        _audioSources[++counter % _audioSources.Count].Play();
    }
}
