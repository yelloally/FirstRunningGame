using UnityEngine;
using System;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{

    //static reference
    public static AdManager Instance { get { return instance; } }
    private static AdManager instance;

    [SerializeField] private string gameID;
    [SerializeField] private string rewardeVideoPlacementId;
    [SerializeField] private bool testMode;

    private void Awake()
    {
        instance = this; //set the static reference to this instance
        Advertisement.Initialize(gameID, testMode, this); //initialize Unity Ads
    }
    
    public void SgowRewardedAdd()
    {
        ShowOptions so = new ShowOptions();
        
        Advertisement.Show(rewardeVideoPlacementId,this); //show the rewarded video ad
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Advertisement module initialized successfully");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Advertisement module initialization failed: {error.ToString()} - {message}");
    }

    void IUnityAdsLoadListener.OnUnityAdsAdLoaded(string placementId)
    {
        //throw new NotImplementedException();
    }

    void IUnityAdsLoadListener.OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //throw new NotImplementedException();
    }

    void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //throw new NotImplementedException();
    }

    void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("ad started");
    }

    void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId)
    {
        //throw new NotImplementedException();
        Debug.Log("User clicked on ad"); 
    }

    void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //throw new NotImplementedException();
    }
}
