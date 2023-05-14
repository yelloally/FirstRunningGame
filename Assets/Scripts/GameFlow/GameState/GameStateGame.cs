using TMPro;
using UnityEditor;
using UnityEngine;
//we have a player that is currently paused and we need to make sure this one
//is enabled, so he can start running as soon as this one is created
public class GameStateGame : GameState 
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private AudioClip gameLoopMusic;

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.motor.ResumePlayer();

          //change the camera to the initialization camera
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        //subscribe to events for updating the fish and score counts in the UI
        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChange;

        gameUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(gameLoopMusic, 0.5f);
    }

    //callback for when the player collects fish
    private void OnCollectFish(int amnCollected)
    {
        fishCount.text = GameStats.Instance.FishToText();
    }

    //callback for when the player s score changes
    private void OnScoreChange(float score)
    {
        scoreCount.text = GameStats.Instance.ScoreToText();
    }

    public override void Destruct()
    {
        gameUI.SetActive(false);

        //unsubscribe from the fish and score events
        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }
    public override void UpdateState()
    {
        //if there is a world generation script scan the player's pozition
        if (GameManager.Instance.worldGeneration != null)
            GameManager.Instance.worldGeneration.ScanPosition();
        //if there is a scene chunk generation script, scan the player' s pozition
        if (GameManager.Instance.sceneChunkGeneration != null)
            GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}

 