using UnityEngine;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 5f;
    public float gravity = 20f;
    public Transform finishingGround;
    public Camera gameCamera;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // поворачиваем камеру в нужном направлении
        gameCamera.transform.LookAt(finishingGround);
    }

    void FixedUpdate()
    {
        // движение камеры вместе с пингвином
        gameCamera.transform.position = new Vector3(transform.position.x + 3f, transform.position.y + 2f, transform.position.z);

        // проверяем, достиг ли пингвин финишной зоны
        if (transform.position.z >= finishingGround.position.z)
        {
            // откатываем пингвина на 10 единиц назад
            transform.position = new Vector3(transform.position.x, transform.position.y, finishingGround.position.z - 10f);
            // поворачиваем камеру в нужном направлении
            gameCamera.transform.LookAt(finishingGround);
        }
        else
        {
            // перемещаем пингвина вперед
            moveDirection = new Vector3(0, 0, 1);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.fixedDeltaTime);
        }
    }
}
