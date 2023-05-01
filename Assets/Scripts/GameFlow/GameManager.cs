using System;
using UnityEngine;

public enum GameCamera
{
    Init = 0,
    Game = 1,
    Shop = 2,
    Respawn = 3
}

public class GameManager : MonoBehaviour
{
    //static reference
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    //references to the player motor and cameras in the gamee
    public PlayerMotor motor;
    public WorldGeneration worldGeneration;
    public SceneChunkGeneration sceneChunkGeneration;
    public GameObject[] cameras;
    

    private GameState state;

    //is called when the script instance is being loaded
    private void Start()
    {
        instance = this;
        state = GetComponent<GameStateInit>();
        state.Construct();
    }

    private void Update()
    {
        state.UpdateState();
    }

    public void ChangeState(GameState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }

    //changes the active camera
    public void ChangeCamera(GameCamera c)
    {
        //deactivate all cameras in the cameras array
        foreach (GameObject go in cameras)
            go.SetActive(false);

        //activate the camera corresponding to the specified GameCamera
        cameras[(int)c].SetActive(true);
    }
}
