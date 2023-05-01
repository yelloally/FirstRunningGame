using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFish;
    public TextMeshProUGUI currentHatName;
    public HatLogic hatLogic;

    //shop item
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats; //array of hats

    protected override void Awake()
    {
        base.Awake();
        hats = Resources.LoadAll<Hat>("Hat/"); //load all hats from the Resources folder
        PopulateShop();
    }

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop); //switch to the shop camera

        totalFish.text = SaveManager.Instance.save.Fish.ToString("000");

        shopUI.SetActive(true);
    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }

    private void PopulateShop()
    {
        
        for (int i = 0; i < hats.Length; i++) //iterate over all hats
        {
            int index = i;

            GameObject go = Instantiate(hatPrefab, hatContainer); //instantiate a new hat object in the hat container
            //button
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
            //thumbnail
            go.transform.GetChild(0).GetComponent<Image>().sprite = hats[index].Thumbnail;
            //itemname
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            //price
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
        }
    }

    private void OnHatClick(int i)
    {
        SaveManager.Instance.save.CurrentHatIndex = i;
        currentHatName.text = hats[i].ItemName;
        hatLogic.SelectHat(i);
        SaveManager.Instance.Save();
    }

    public void OnHomeClick()
    {
        //switch to the initialization state
        brain.ChangeState(GetComponent<GameStateInit>());
    }

}
