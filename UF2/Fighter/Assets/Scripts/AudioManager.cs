using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Audio Variables")]
    AudioSource audio;
    [SerializeField] Scrollbar volume;
    [SerializeField] Scrollbar pitch;
    [SerializeField] AudioClip[] song;
    int index;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        index = 0;
        audio.clip = song[index];
        audio.Play();
    }

    public void Volume()
    {
        audio.volume = volume.value;
    }

    public void Pitch()
    {
        audio.pitch = (pitch.value * 4) - 1;
    }

    public void ChangeSong()
    {
        index++;
        if (index == song.Length) index = 0;
        audio.clip = song[index];
        audio.Play();
    }
}
