using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour
{
    //static reference
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    //fields
    public SaveState save; 
    private const string saveFileName = "/data_1.ss"; //file name to store the saved data
    //delete this file and runagaina
    private BinaryFormatter formatter; //object to serialize/deserializa data

    //actions
    public Action<SaveState> OnLoad; //to trigger when data is loaded
    public Action<SaveState> OnSave; //to trigger when data is saved

    private void Awake()
    {
        instance = this;
        formatter = new BinaryFormatter();

        //try and load the previous save state
        Load();
    }

    public void Load()
    {
        try
        {
            //open the save file in read mode and deserialize the data
            FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
            //if you want to find the file path print it like this
            Debug.Log(Application.persistentDataPath + saveFileName);
            //where is the visual studio tool bar? how to s
            save = (SaveState)formatter.Deserialize(file);
            Debug.Log(save.UnlockedHatFlag.Length);
            file.Close();
            //now in this mode how to get the menu????
            OnLoad?.Invoke(save);
        }

        catch
        {
            //if save file not found ,  create a new one
            Debug.Log("Save file not found, lets create a new one");
            Save();
        }
    }

    public void Save()
    {
        //if theres no previous state found, create a new one
        if (save == null)
            save = new SaveState();

        //set the time at which we've tried saving
        save.LastSaveTime = DateTime.Now;

        //open a file in our system, and write to it 
        FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);
    }
}
