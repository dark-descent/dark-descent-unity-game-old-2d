using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
	[SerializeField] AudioClip music;
	[SerializeField] float musicVelocity;

	[SerializeField] AudioClip ambient;
	[SerializeField] float ambientVelocity;

	AudioSource musicAudioSource;
	AudioSource ambientAudiouSource;

	void Start()
	{
		musicAudioSource = gameObject.AddComponent<AudioSource>();
		musicAudioSource.volume = musicVelocity;
		
		ambientAudiouSource = gameObject.AddComponent<AudioSource>();
		ambientAudiouSource.volume = ambientVelocity;

		musicAudioSource.clip = music;
		musicAudioSource.Play();

		ambientAudiouSource.clip = ambient;
		ambientAudiouSource.Play();
	}
}
