using UnityEngine;

public class CameraFollow : PersistentSingleton<CameraFollow>
{
    PlayerController playerController;

    [SerializeField] float _followSpeed = 5f;
    [SerializeField] Vector3 _offset;

    void Start()
    {
        playerController = PlayerController.Instance;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = playerController.transform.position + _offset;
        MoveCamera(targetPosition);
    }

    void MoveCamera(Vector3 targetPosition)
    {
        Vector3 newPosition = new(targetPosition.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, _followSpeed * Time.deltaTime);
    }
}
