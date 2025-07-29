using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 thirdPersonOffset; 
    public Vector3 firstPersonOffset; 
    public Vector3 rotationOffset;

    public float mouseSensitivity = 100f;
    private float pitch = 0f;
    private float yaw = 0f;
    private bool isFirstPerson = false; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch + rotationOffset.x, yaw + rotationOffset.y, rotationOffset.z);

        Vector3 offset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothSpeed);

        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleCameraView();
        }
    }

    private void ToggleCameraView()
    {
        isFirstPerson = !isFirstPerson;
    }
}
