using UnityEngine;

public interface ISoundService
{
    void PlaySoundOnce(AudioClip clip);
    void PlaySoundOnce(AudioClip clip, float volume);
    void EnableSound(bool value);
    void EnableMusic(bool value);
}
