using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform startingRoom;
    [SerializeField] private float speed;
    [SerializeField] private bool followPlayer = false; 
    private Transform player; 

    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        MoveToNewRoom(startingRoom);
    }

    private void Update()
    {
        float targetX = currentPosX;

        if (followPlayer && player != null)
        {
            targetX = player.position.x;
        }

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), ref velocity, speed);
        currentPosX = transform.position.x;
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
