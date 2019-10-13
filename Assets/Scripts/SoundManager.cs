﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance = null;

	public AudioSource efxSource;
    public AudioSource musicSource;
	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

    void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}else if (instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip audioClip)
	{
		efxSource.clip = audioClip;
		efxSource.Play();
	}

    public void RandomizeSfx(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		efxSource.clip = clips[randomIndex];
		efxSource.pitch = randomPitch;
        efxSource.loop = false;
        efxSource.Play();
	}
}
