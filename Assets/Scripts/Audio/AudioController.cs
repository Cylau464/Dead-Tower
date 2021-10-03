using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;
    [Header("Music Clips")]
    [SerializeField] private AudioClip[] _musicClips;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    public static AudioController Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        _musicSource.outputAudioMixerGroup = _musicMixer;
        _sfxSource.outputAudioMixerGroup = _sfxMixer;
    }

    private void Start()
    {
        _musicSource.clip = _musicClips[Random.Range(0, _musicClips.Length)];
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public bool ToggleMusic()
    {
        _musicSource.enabled = !_musicSource.enabled;
        return _musicSource.enabled;
    }

    public bool ToggleSFX()
    {
        _sfxSource.enabled = !_sfxSource.enabled;
        return _sfxSource.enabled;
    }
}