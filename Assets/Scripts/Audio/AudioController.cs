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
        _musicSource.enabled = !_musicSource.enabled;
        return _musicSource.enabled;
    }

    public bool ToggleSFX()
    {
        _sfxSource.enabled = !_sfxSource.enabled;
        return _sfxSource.enabled;
    }

    private void OnDestroy()
    {
        SceneLoader.OnSceneLoadStart -= PauseMusic;
        SceneLoader.OnSceneLoadComplete -= ResumeMusic;
    }

    public static AudioSource PlayClipAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float minDistance = 1f, float pitch = 1f, Transform parent = null)
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
        source.Play();
        Destroy(go, source.clip.length);

        return source;

    }
}