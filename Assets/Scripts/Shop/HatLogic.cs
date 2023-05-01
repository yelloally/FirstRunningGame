using System.Collections.Generic;
using UnityEngine;

public class HatLogic : MonoBehaviour
{
    [SerializeField] private Transform hatContainer; //hat container transform object
    private List<GameObject> hatModels = new List<GameObject>(); //list of instantiated hat models
    private Hat[] hats; //array of hats


    private void Start()
    {
        hats = Resources.LoadAll<Hat>("Hat/"); //load all hats from the Resources folder
        SpawnHats();
        SelectHat(SaveManager.Instance.save.CurrentHatIndex);
    }

    //instantiate all hat models and add them to the hat models list
    private void SpawnHats()
    {
        for (int i = 0; i < hats.Length; i++) //iterate over all hats
        {
            //instantiate the hat model and add it to the hat container transform object
            hatModels.Add(Instantiate(hats[i].Model, hatContainer) as GameObject);
        }
    }

    public void DisableAllHats()
    {
        for (int i = 0; i < hatModels.Count; i++)
        {
            hatModels[i].SetActive(false);
        }
    }

    public void SelectHat(int index)
    {
        DisableAllHats();
        hatModels[index].SetActive(true);
    }
}

