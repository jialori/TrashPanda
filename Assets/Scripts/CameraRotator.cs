using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private bool useController = false;

    // Constants for clamping camera angles and establishing boundaries for rotation
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    // Initialization of variables
    public Transform camTransform;

    private Camera cam;

    [SerializeField] private float distance = 8.0f;
    private float currentX = 0.0f;
    private float currentY = 10f;
    private float sensitivityX = 3.0f;
    private float sensitivityY = 3.0f;

    [SerializeField] private Transform target = null;
    [SerializeField] private float damping = 2.0f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float rotationDamping = 2.0f;

    [SerializeField] private Vector3 targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights

    [SerializeField] private float bumperDistanceCheck = 2.0f; // length of bumper ray
    [SerializeField] private float bumperCameraHeight = 0.5f; // adjust camera height while bumping
    [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin

    private void Awake()
    {
        camTransform = transform;
        cam = Camera.main;
        GetComponent<Camera>().transform.parent = target;
    }

    private void Update()
    {
        // Ensure rotation values correspond with movement of mouse (will need to change to fit controller later)
        currentX += GetXAxis() * sensitivityX;
        currentY += GetYAxis() * sensitivityY;

        // Clamp camera rotation
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private float GetXAxis()
    {
        if (useController)
        {
            return Input.GetAxis("RightJoystickX");
        }
        else
        {
            return Input.GetAxis("Mouse X");
        }
    }

    private float GetYAxis()
    {
        if (useController)
        {
            return Input.GetAxis("RightJoystickY");
        }
        else
        {
            return Input.GetAxis("Mouse Y");
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        // TODO why the raccoon cannot walk straight line in small room

        Vector3 wantedPosition = target.TransformPoint(rotation * dir);
        
        // check to see if there is anything behind the target
        RaycastHit hit;
        // TODO target.forward or Vector3.forward
        Vector3 back = target.transform.TransformDirection(-1 * target.forward);

        // cast the bumper ray out from rear and check to see if there is anything behind
        if (Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck)
            && hit.transform != target) // ignore ray-casts that hit the user. DR
        {
            // clamp wanted position to hit position
            wantedPosition.x = hit.point.x;
            wantedPosition.z = hit.point.z;
            wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else
            transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
    }
}
