using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Voice_Lines : MonoBehaviour
{
    public AudioSource[] audioSources; // Array to hold the 3 audio sources
    public float minInterval = 5f; // Minimum interval between audio plays
    public float maxInterval = 30f; // Maximum interval between audio plays

    private float nextPlayTime;

    // Start is called before the first frame update
    void Start()
    {
        ScheduleNextPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlayRandomAudio();
            ScheduleNextPlay();
        }
    }

    void PlayRandomAudio()
    {
        int randomIndex = Random.Range(0, audioSources.Length);
        audioSources[randomIndex].Play();
    }

    void ScheduleNextPlay()
    {
        nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}
