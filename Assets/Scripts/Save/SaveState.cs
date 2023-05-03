using UnityEngine;
using System;

[System.Serializable]
public class SaveState
{
    //number of different hats that can be unlocked in the game
    [NonSerialized] private const int HAT_COUNT = 3;

    //properties to store the highscore and number of fish
    public int Highscore { set; get; }
    public int Fish { get; set; }

    //property to store the date and time of the last save
    public DateTime LastSaveTime { set; get; }
    //index of the hat that the player is currently wearing
    public int CurrentHatIndex { set; get; }
    //binary flag for each hat that indicates whether it has been unlocked
    public byte[] UnlockedHatFlag { set; get; }

    //constructor to initialize default values
    public SaveState()
    {
        Highscore = 0;
        Fish = 0;
        LastSaveTime = DateTime.Now;
        //index of the current hat to zero
        CurrentHatIndex = 0;
        UnlockedHatFlag = new byte[HAT_COUNT];
        // the unlocked hat flag array to have the first hat unlocked
        UnlockedHatFlag[0] = 1;
    }
}
