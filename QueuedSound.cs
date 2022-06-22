using UnityEngine;

public class QueuedSound
{
	private IntVec3 location;

	private AudioSource audioSource;

	public AudioClip clip;

	private float volume;

	public QueuedSound(IntVec3 location, AudioClip clip, float volume)
	{
		this.location = location;
		this.clip = clip;
		this.volume = volume;
	}

	public QueuedSound(AudioSource audioSource, AudioClip clip, float volume)
	{
		this.audioSource = audioSource;
		this.clip = clip;
		this.volume = volume;
	}

	public void ExecuteSound()
	{
		if (this.audioSource != null)
		{
			this.audioSource.PlayOneShot(clip, volume);
			return;
		}
		GameObject gameObject = new GameObject("AudioSource");
		gameObject.transform.position = location.ToVector3ShiftedWithAltitude(0f);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.minDistance = 25f;
		audioSource.maxDistance = 70f;
		audioSource.dopplerLevel = 0f;
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.Play();
		Object.Destroy(gameObject, clip.length);
	}
}
