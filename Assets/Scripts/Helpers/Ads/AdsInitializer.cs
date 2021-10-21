using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private InterstitialAd _interstitialAd;
    [SerializeField] private RewardedAd _rewardedAd;
    [Space]
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOsGameId;
    [SerializeField] private bool _testMode = true;
    [SerializeField] private bool _enablePerPlacementMode = true;
    public InterstitialAd InterstitialAd => _interstitialAd;
    public RewardedAd RewardedAd => _rewardedAd;

    private string _gameId;

    public static AdsInitializer Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);
    }

    public void OnInitializationComplete()
    {
        //Debug.Log("Unity Ads initialization complete.");
        _rewardedAd.Init();

        if(SLS.Data.Settings.AdsEnabled.Value == true)
            _interstitialAd.Init();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void ShowInterstitial()
    {
        if (SLS.Data.Settings.AdsEnabled.Value == true)
            _interstitialAd.ShowAd();
    }

    public void ShowRewarded()
    {
        _rewardedAd.ShowAd();
    }
}