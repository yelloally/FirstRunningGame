using System;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } }
    private static GameStats instance;

    //score
    public float score;
    public float highscore;
    public float distanceModifier = 1.5f; 

    //fish
    public int totalFish;
    public int fishCollectedThisSession;
    public float pointsPerFish = 10;
    public AudioClip fishCollectSFX;

    //internal cooldown
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f; 

    //action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Awake()
    {
        instance = this; 
    }

    public void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += fishCollectedThisSession * pointsPerFish;

        //update the score if it increased
        if (s > score)
        {
            score = s;
            if(Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
        }
    }

    //add a fish to the collection 
    public void CollectFish()
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession);
        AudioManager.Instance.PlaySFX(fishCollectSFX); 
    }

    public void resetFish()
    {
        fishCollectedThisSession = 0;
        OnCollectFish?.Invoke(fishCollectedThisSession);
        AudioManager.Instance.PlaySFX(fishCollectSFX);
    }

    //reset the game 
    public void ResetSession()
    {
        score = 0;
        fishCollectedThisSession = 0;

        OnCollectFish?.Invoke(fishCollectedThisSession);
        OnScoreChange?.Invoke(score);
    }

    public string ScoreToText()
    {
        return score.ToString("000000");
    }

    public string FishToText()
    {
        return fishCollectedThisSession.ToString("000");
    }
}
