using UnityEngine;

[DisallowMultipleComponent]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void PlaySoundOnce(AudioClip clip) => PlaySoundOnce(clip, _sfxSource.volume);

    public void PlaySoundOnce(AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void EnableSound(bool value) => _sfxSource.mute = !value;
    public void EnableMusic(bool value) => _musicSource.mute = !value;
}