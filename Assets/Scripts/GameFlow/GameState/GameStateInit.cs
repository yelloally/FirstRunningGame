using UnityEngine;
using TMPro;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI fishcountText;

    //change the camera to the initialization camera
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        hiscoreText.text = "Highscore: " + SaveManager.Instance.save.Highscore.ToString();
        fishcountText.text = "Fish: " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);
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
        GetComponent<GameStateDeath>().EnableRevive(); 
    }

    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
    }
}
