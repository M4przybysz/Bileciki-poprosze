using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform startingRoom;
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        MoveToNewRoom(startingRoom);
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
