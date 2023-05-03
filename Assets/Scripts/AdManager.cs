using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    //static reference
    public static AdManager Instance { get { return instance; } }
    private static AdManager instance;

    [SerializeField] private string gameID;
    [SerializeField] private string rewardeVideoPlacementId;
    [SerializeField] private bool testMode;

    private void Awake()
    {
        instance = this;
        Advertisement.Initialize(gameID, testMode, this);
    }

    public void SgowRewardedAdd()
    {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(rewardeVideoPlacementId, so);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Advertisement module initialized successfully");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Advertisement module initialization failed: {error.ToString()} - {message}");
    }
}
