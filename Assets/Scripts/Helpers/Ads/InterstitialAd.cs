using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] private string _iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] private float _cooldown;

    private string _adUnitId;
    private bool _isLoaded;
    private float _curCooldown;

    public void Init()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;

        LoadAd();
    }

    private void Update()
    {
        _curCooldown -= Time.unscaledDeltaTime;
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitId);
        _isLoaded = false;
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit: 
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        if (_curCooldown > 0f)
            return;

        if (_isLoaded == false)
        {
            Debug.LogError("Interstitail is not loaded but trying to show!");
            return;
        }

        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods:  
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        _isLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        _isLoaded = false;
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        LoadAd();
        // Optionally execite code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execite code if the Ad Unit fails to show, such as loading another ad.
        LoadAd();
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAd();
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        _curCooldown = _cooldown;
    }

    public void OnUnityAdsShowClick(string adUnitId) { }
}