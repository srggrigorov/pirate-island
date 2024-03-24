using UnityEngine;

[DisallowMultipleComponent]
public class SoundManager
{
    public SoundManager(AudioSource musicSource, AudioSource sfxSource)
    {
        _musicSource = musicSource;
        _sfxSource = sfxSource;
    }

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    public void PlaySoundOnce(AudioClip clip) => PlaySoundOnce(clip, _sfxSource.volume);

    public void PlaySoundOnce(AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void EnableSound(bool value) => _sfxSource.mute = !value;
    public void EnableMusic(bool value) => _musicSource.mute = !value;
}