using UnityEngine;
using TMPro;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI fishcountText;
    [SerializeField] private AudioClip menuLoopMusic;

    //change the camera to the initialization camera
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        hiscoreText.text = " " + SaveManager.Instance.save.Highscore.ToString();
        fishcountText.text = " " + SaveManager.Instance.save.Fish.ToString();
        
        menuUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(menuLoopMusic, 0.5f);
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        //eeset the game stats for the new session
        GameStats.Instance.ResetSession();
        GameManager.Instance.motor.locationPoints.Clear();
        GameManager.Instance.motor.slideBack = false;
        GetComponent<GameStateDeath>().EnableRevive(); 
    }

    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
    }
}
