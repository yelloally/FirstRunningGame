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
    private bool isInit = false;

    //shop item
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats; //array of hats

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        hats = Resources.LoadAll<Hat>("Hat");
        shopUI.SetActive(true);

        if (!isInit)
        {
            totalFish.text = SaveManager.Instance.save.Fish.ToString("000");
            currentHatName.text = hats[SaveManager.Instance.save.CurrentHatIndex].ItemName;
            PopulateShop();

            isInit = true;
        }
    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }

    private void PopulateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(hatPrefab, hatContainer);
            // Button
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
            // Thumbnail
            go.transform.GetChild(0).GetComponent<Image>().sprite = hats[index].Thumbnail;
            // ItemName
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            // Price
            //Debug.Log(SaveManager.Instance);
            //Debug.Log(SaveManager.Instance.save);
            //Debug.Log(SaveManager.Instance.save.UnlockedHatFlag);
            if (SaveManager.Instance.save.UnlockedHatFlag != null)
            {
                if (SaveManager.Instance.save.UnlockedHatFlag[i] == 0)
                    go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
                else
                {
                    go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

                }
            }
        }
    }


    private void OnHatClick(int i)
    {
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            SaveManager.Instance.Save();
        }

        //if we dont have it, can we buy it? 
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            totalFish.text = SaveManager.Instance.save.Fish.ToString("000");
            SaveManager.Instance.Save();
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

        }
        //dont have it, cant buy it
        else
        {
            Debug.Log("not enough fish to buy it.");
        }

    }

    public void OnHomeClick()
    {
        //switch to the initialization state
        brain.ChangeState(GetComponent<GameStateInit>());
    }

}
