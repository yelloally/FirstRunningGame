using UnityEngine;

public class SnowFloor : MonoBehaviour
{
    //grabbing a transform of the player to know where he is
    [SerializeField] private Transform player;
    [SerializeField] private Material material;
    public float offsetSpeed = 0.25f;

    private void Update()
    {
        transform.position = Vector3.forward * player.transform.position.z;
        material.SetVector("snowOffset", new Vector2(0, -transform.position.z * offsetSpeed));
    }
}
