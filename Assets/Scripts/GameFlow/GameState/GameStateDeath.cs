using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameStateDeath : GameState
{
    //UI
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;

    //completion circle fields
    [SerializeField] private Image completionCircle;
    public float timeToDecision = 2.5f;
    private float deathTime;

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.motor.PausePlayer(); //pause the player movement

        deathTime = Time.time;
        deathUI.SetActive(true); //activate the death UI panel
        completionCircle.gameObject.SetActive(true);

        //prior to saving, set the highscore if needed
        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            currentScore.color = Color.green;
        }
        else
            currentScore.color = Color.white;

        //add the fish collected in this session to the total count
        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save(); //save the game state

        highScore.text = "Highscore: " + SaveManager.Instance.save.Highscore;
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "Total fish: " + SaveManager.Instance.save.Fish;
        currentFish.text = GameStats.Instance.FishToText();
    }

    public override void Destruct()
    {
        deathUI.SetActive(false); //deactivate the death UI panel
    }

    public override void UpdateState()
    {
        //calculate the ratio of time passed for decision making
        float ratio = (Time.time - deathTime) / timeToDecision;
        //update the color of the completion circle based on the ratio
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        //update the fill amount of the completion circle based on the ratio
        completionCircle.fillAmount = 1 - ratio;

        if(ratio > 1) //if the ratio has exceeded 1 , hide the completion circle
        {
            completionCircle.gameObject.SetActive(false); 
        }

    }

    public void ResumeGame()
    {
        //transition to the GamestateGame and respawn the player
        brain.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.motor.RespawnPlayer();
    }

    public void ToMenu()
    {
        //transition to the GameStateInit , reset the player and world generation
        brain.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();

       
    }
}
