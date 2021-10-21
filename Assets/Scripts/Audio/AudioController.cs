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

    private const string SFXVolumeParamName = "SFXVolume";
    private const string MusicVolumeParamName = "MusicVolume";

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
        SceneLoader.OnSceneLoadStart += PauseMusic;
        SceneLoader.OnSceneLoadComplete += ResumeMusic;

        _musicSource.clip = _musicClips[Random.Range(0, _musicClips.Length)];
        _musicSource.loop = true;
        _musicSource.Play();
    }

    private void PauseMusic()
    {
        _musicSource.Pause();
    }

    private void ResumeMusic()
    {
        _musicSource.Play();
    }

    public bool ToggleMusic()
    {
        _musicMixer.audioMixer.GetFloat(MusicVolumeParamName, out float value);

        if (value < 0f)
        {
            _musicMixer.audioMixer.SetFloat(MusicVolumeParamName, 0f);
            return true;
        }
        else
        {
            _musicMixer.audioMixer.SetFloat(MusicVolumeParamName, -80f);
            return false;
        }
    }

    public void ToggleMusic(bool enabled)
    {
        float value = enabled == true ? 0f : -80f;
        _musicMixer.audioMixer.SetFloat(MusicVolumeParamName, value);
    }

    public bool ToggleSFX()
    {
        _sfxMixer.audioMixer.GetFloat(SFXVolumeParamName, out float value);

        if (value < 0f)
        {
            _sfxMixer.audioMixer.SetFloat(SFXVolumeParamName, 0f);
            return true;
        }
        else
        {
            _sfxMixer.audioMixer.SetFloat(SFXVolumeParamName, -80f);
            return false;
        }
    }

    public void ToggleSFX(bool enabled)
    {
        float value = enabled == true ? 0f : -80f;
        _sfxMixer.audioMixer.SetFloat(SFXVolumeParamName, value);
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadStart -= PauseMusic;
        SceneLoader.OnSceneLoadComplete -= ResumeMusic;
    }

    public static AudioSource PlayClipAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float minDistance = 1f, float pitch = 1f, AudioMixerGroup mixerGroup = null, Transform parent = null)
    {
        GameObject go = new GameObject("One Shot Audio");
        go.transform.position = position;
        go.transform.parent = parent;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.minDistance = minDistance;
        source.pitch = Random.Range(pitch - .2f, pitch + .2f);
        source.outputAudioMixerGroup = mixerGroup == null ? Instance._sfxMixer : mixerGroup;
        source.Play();
        Destroy(go, source.clip.length);

        return source;

    }
}