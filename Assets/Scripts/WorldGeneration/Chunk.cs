using UnityEngine;

public class Chunk : MonoBehaviour
{
    //need to know the length of our chunk
    public float chunkLength;

    //two functions which will be show and hide our chunks (appear and disappear,
    //when our penguin will be moving, idk how to say it, u know)
    public Chunk ShowChunk()
    {
        transform.gameObject.BroadcastMessage("OnShowChunk", SendMessageOptions.DontRequireReceiver);
        //appear
        gameObject.SetActive(true);

        return this;
    }

    public Chunk HideChunk()
    {
        //disappear
        gameObject.SetActive(false);

        return this;
    }
}

 