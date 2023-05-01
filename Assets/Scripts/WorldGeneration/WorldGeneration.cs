 using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    //GAMEPLAY

    //contain where is the last spawn chunk 
    private float chunkSpawnZ;
    //we can take out the first one spawn when we try to despawn 
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    //keep in memory, to reuse yhem in future
    private List<Chunk> chunkPool = new List<Chunk>();

    //CONFIGURABLE FIELDS
    [SerializeField] private int firstChunkSpawnPosition = -10;
    //how many chunk are we gonna have on the screen at the same time
    [SerializeField] private int chunkOnScreen = 5;
    //how far a way we have to go after a chunk before we despawned it 
    [SerializeField] private float despawnDistance = 5.0f;

    //will contain the full list of everything we can spawn 
    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        ResetWorld();
    }
    

    //beginning of the game
    private void Start() 
    {
        //check if we have an empty chunkPrefab list
        if (chunkPrefab.Count == 0)
        {
            Debug.LogError("No chunk prefab found on the world generator, please assign some chunks ");
            return;
        }

        //try to assign the cameraTransform
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("We've assigned cameraTransform automaticly to the Camera.main");
        }
    }

    

    //looking for the camera position, and its going to keep asking camera
    //are we far enough to the pont camera when we need to spawn a new chunk 
    public void ScanPosition()
    {
        //where is the last chunk has been spawned
        float cameraZ = cameraTransform.position.z;
        //goes at the very last object in the queque and simply return 
        Chunk lastChunk = activeChunks.Peek();

        //we are far enough
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
        
    }

    //we need it when the last chunk behind us and we need to spawn new one so
    //we dont see the empty in front of us
    private void SpawnNewChunk()
    {
        //get a random index for which prefab to spawn
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        //does it already exist within our pool
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == chunkPrefab[randomIndex].name + "(Clone)");

        //create a chunk, if were not able to find one to reuse
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }


        //place the object and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        //store the value, to reuse in our pool
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }

    //going to take the first chunk (the one we just passed), and make sure to
    //disable it 
    private void DeleteLastChunk()
    {
        //picks the first one we put, so that the oldest remowves from the list
        //at the same time as it removes it also gives a reference
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }

    //function for resetting the whole thing ,when we die or when we want
    //to play again 
    public void ResetWorld()
    {
        //reset the ChunkSpawnZ
        chunkSpawnZ = firstChunkSpawnPosition;

        for (int i = activeChunks.Count; i != 0; i--)
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++)
            SpawnNewChunk();
    }
}
