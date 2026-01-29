using System;
using AppsInToss;
using TMPro;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }
    
    private const string adId = "ait-ad-test-rewarded-id";
    private bool isAdLoaded = false;
    
    private Action onReward;
    private Action _loadUnsubscribe;
    private Action _showUnsubscribe;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        _loadUnsubscribe?.Invoke();
        
        _loadUnsubscribe = AIT.GoogleAdMobLoadAppsInTossAdMob(
            options: new LoadAdMobOptions { AdGroupId = adId },
            onEvent: (result) =>
            {
                if (result.Type == "loaded")
                {
                    isAdLoaded = true;
                }
            },
            onError: (error) =>
            {
                isAdLoaded = false;
                Invoke(nameof(LoadAd), 5f);
            });
    }

    public void ShowAd(Action reward)
    {
        if (!isAdLoaded)
        {
            LoadAd();
            return;
        }

        onReward = null;
        
        _showUnsubscribe?.Invoke();
        
        _showUnsubscribe = AIT.GoogleAdMobShowAppsInTossAdMob(
            options: new ShowAdMobOptions { AdGroupId = adId },
            onEvent: (result) =>
            {
                if (result.Type == "dismissed")
                {
                    isAdLoaded = false;
                    onReward?.Invoke();
                    LoadAd();
                }
                
                if (result.Type == "userEarnedReward" && result.Data != null)
                {
                    onReward = reward;
                }
            },
            onError: (error) =>
            {
            });
    }

    private void OnDisable()
    {
        _loadUnsubscribe?.Invoke();
        _showUnsubscribe?.Invoke();
    }
}
