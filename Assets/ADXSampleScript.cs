using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AdxUnityPlugin;

public class ADXSampleScript : MonoBehaviour {

    private AdxBannerAd bannerAd;
    private AdxInterstitialAd interstitialAd;
    private AdxRewardedAd rewardedAd;

    private bool IsInterstitialAdClosed = false;
    private bool IsRewardedAdClosed = false;
    private bool IsRewardedAdEarned = false;


#if UNITY_ANDROID

    string adxAppId = "61ee18cecb8c670001000023";
    string adxBannerAdUnitId = "61ee2b7dcb8c67000100002a";
	string adxInterstitialAdUnitId = "61ee2e3fcb8c67000100002e";
    string adxRewardedAdUnitId = "61ee2e91cb8c67000100002f";

#elif UNITY_IPHONE

    string adxAppId = "6200fea42a918d0001000001";
    string adxBannerAdUnitId = "6200fee42a918d0001000003";
	string adxInterstitialAdUnitId = "6200fef52a918d0001000007";
    string adxRewardedAdUnitId = "6200ff0c2a918d000100000d";
    
#endif

    // Use this for initialization
    void Start () {
        
        Debug.Log("Start ::: ");
        
        AdxSDK.SetLogEnable(true);
        
        ADXConfiguration adxConfiguration = new ADXConfiguration.Builder()
                                .SetAppId(adxAppId)
                                .SetGdprType(GdprType.POPUP_LOCATION).Build();

        AdxSDK.Initialize(adxConfiguration, adxConsentState => {
            Debug.Log(":::AdxSample:::onADXConsentCompleted : " + adxConsentState);
        });
	}

	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (IsInterstitialAdClosed)
        {
            IsInterstitialAdClosed = false;

            // 전면광고 닫기 후 처리 수행
        }

        if (IsRewardedAdClosed)
        {
            IsRewardedAdClosed = false;

            // 리워드광고 닫기 후 처리 수행
        }

        if (IsRewardedAdEarned)
        {
            IsRewardedAdEarned = false;

            // 리워드광고 보상 처리 수행
        }
    }
    
    void OnGUI () {
        
        var fontSize = (int)(0.035f * Screen.width);
        
        GUI.skin.box.fontSize = fontSize;
        GUI.skin.button.fontSize = fontSize;
        
        var buttonWidth = 0.35f * Screen.width;
        var buttonHeight = 0.15f * Screen.height;
        var buttonRowCount = 3;
        
        var groupWidth = buttonWidth * 2 + 30;
        var groupHeight = fontSize + (buttonHeight * buttonRowCount) + (buttonRowCount * 10) + 10;

        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var groupX = ( screenWidth - groupWidth ) / 2;
        var groupY = ( screenHeight - groupHeight ) / 2;

        GUI.BeginGroup(new Rect( groupX, groupY, groupWidth, groupHeight ) );
        GUI.Box(new Rect( 0, 0, groupWidth, groupHeight ), "Select ADXLibrary function" );

        if ( GUI.Button(new Rect( 10, fontSize + 10, buttonWidth, buttonHeight ), "Load Banner") )
        {
            LoadBannerAd();
        }
        if ( GUI.Button(new Rect( 10, fontSize + 20 + buttonHeight, buttonWidth, buttonHeight ), "Load Interstitial" ) )
        {
            LoadInterstitialAd();
        }
        if ( GUI.Button(new Rect( 10, fontSize + 30 + buttonHeight * 2, buttonWidth, buttonHeight ), "Load RewardedAd" ) )
        {
            LoadRewardedAd();
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 10, buttonWidth, buttonHeight ), "Destroy Banner" ) )
        {
            DestroyBannerAd();
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 20 + buttonHeight, buttonWidth, buttonHeight ), "Show Interstitial" ) )
        {
            ShowInterstitialAd();
        }
        if ( GUI.Button(new Rect( 20 + buttonWidth, fontSize + 30 + buttonHeight * 2, buttonWidth, buttonHeight ), "Show RewardedAd" ) )
        {
            ShowRewardedAd();
        }

        GUI.EndGroup();
    }

    void LoadBannerAd()
    {
        if (bannerAd != null)
        {
            bannerAd.Destroy();
            bannerAd = null;
        }

        // -------- 배너 광고 크기 --------
        // AdxBannerAd.AD_SIZE_320x50
        // AdxBannerAd.AD_SIZE_728x90
        // AdxBannerAd.AD_SIZE_320x100
        // AdxBannerAd.AD_SIZE_300x250

        // -------- 배너 광고 위치 --------
        // AdxBannerAd.POSITION_TOP
        // AdxBannerAd.POSITION_BOTTOM
        // AdxBannerAd.POSITION_TOP_LEFT
        // AdxBannerAd.POSITION_TOP_RIGHT
        // AdxBannerAd.POSITION_BOTTOM_LEFT
        // AdxBannerAd.POSITION_BOTTOM_RIGHT
        // AdxBannerAd.POSITION_CENTER

        bannerAd = new AdxBannerAd(adxBannerAdUnitId, AdxBannerAd.AD_SIZE_320x50, AdxBannerAd.POSITION_TOP);
        bannerAd.OnAdLoaded += BannerAd_OnAdLoaded;
        bannerAd.OnAdFailedToLoad += BannerAd_OnAdFailedToLoad;
        bannerAd.OnAdClicked += BannerAd_OnAdClicked;
        bannerAd.Load();
    }

    void DestroyBannerAd()
    {
        if (bannerAd != null)
        {
            bannerAd.Destroy();
            bannerAd = null;
        }
    }

    void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        interstitialAd = new AdxInterstitialAd(adxInterstitialAdUnitId);
        interstitialAd.OnAdLoaded += InterstitialAd_OnAdLoaded;
        interstitialAd.OnAdFailedToLoad += InterstitialAd_OnAdFailedToLoad;
        interstitialAd.OnAdClicked += InterstitialAd_OnAdClicked;
        interstitialAd.OnAdShown += InterstitialAd_OnAdShown;
        interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;
        interstitialAd.OnAdFailedToShow += InterstitialAd_OnAdFailedToShow;

        interstitialAd.Load();
    }

    void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }

    void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        rewardedAd = new AdxRewardedAd(adxRewardedAdUnitId);
        rewardedAd.OnRewardedAdLoaded += RewardedAd_OnRewardedAdLoaded;
        rewardedAd.OnRewardedAdFailedToLoad += RewardedAd_OnRewardedAdFailedToLoad;
        rewardedAd.OnRewardedAdShown += RewardedAd_OnRewardedAdShown;
        rewardedAd.OnRewardedAdClicked += RewardedAd_OnRewardedAdClicked;
        rewardedAd.OnRewardedAdFailedToShow += RewardedAd_OnRewardedAdFailedToShow;
        rewardedAd.OnRewardedAdEarnedReward += RewardedAd_OnRewardedAdEarnedReward;
        rewardedAd.OnRewardedAdClosed += RewardedAd_OnRewardedAdClosed;

        rewardedAd.Load();
    }

    void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    // ---------------- BannerAd Callback ----------------

    void BannerAd_OnAdLoaded()
    {
        Debug.Log(":::AdxSample:::BannerAd_OnAdLoaded");
    }

    void BannerAd_OnAdFailedToLoad(int errorCode)
    {
        Debug.Log(":::AdxSample:::BannerAd_OnAdFailedToLoad : " + errorCode);
    }

    void BannerAd_OnAdClicked()
    {
        Debug.Log(":::AdxSample:::BannerAd_OnAdClicked");
    }

    // ---------------- InterstitialAd Callback ----------------

    void InterstitialAd_OnAdLoaded()
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdLoaded");
    }

    void InterstitialAd_OnAdFailedToLoad(int errorCode)
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdFailedToLoad : " + errorCode);
    }

    void InterstitialAd_OnAdClicked()
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdClicked");
    }

    void InterstitialAd_OnAdShown()
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdShown");
    }

    void InterstitialAd_OnAdClosed()
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdClosed");

        // main thread 로 인해 Update() 에서 처리 권장
        IsInterstitialAdClosed = true;

        // 광고 재요청 수행
        LoadInterstitialAd();
    }

    void InterstitialAd_OnAdFailedToShow()
    {
        Debug.Log(":::AdxSample:::InterstitialAd_OnAdFailedToShow");
    }

    // ---------------- RewardedAd Callback ----------------

    void RewardedAd_OnRewardedAdLoaded()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdLoaded");
    }

    void RewardedAd_OnRewardedAdFailedToLoad(int errorCode)
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdFailedToLoad : " + errorCode);
    }

    void RewardedAd_OnRewardedAdShown()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdShown");
    }

    void RewardedAd_OnRewardedAdClicked()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdClicked");
    }

    void RewardedAd_OnRewardedAdFailedToShow()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdFailedToShow");
    }

    void RewardedAd_OnRewardedAdEarnedReward()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdEarnedReward");

        // main thread 로 인해 Update() 에서 처리 권장
        IsRewardedAdEarned = true;
    }

    void RewardedAd_OnRewardedAdClosed()
    {
        Debug.Log(":::AdxSample:::RewardedAd_OnRewardedAdClosed");

        // main thread 로 인해 Update() 에서 처리 권장
        IsRewardedAdClosed = true;

        // 광고 재요청 수행
        LoadRewardedAd();
    }
}
