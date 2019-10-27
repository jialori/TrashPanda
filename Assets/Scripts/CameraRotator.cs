using UnityEngine;

public class CameraRotator : MonoBehaviour
{

    // Constants for clamping camera angles and establishing boundaries for rotation
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    // Initialization of variables
    public Transform camTransform;


    [SerializeField] private float distance = 8.0f;
    private float currentX = 0.0f;
    private float currentY = 10f;

    [SerializeField] private Transform target = null;

    [System.Serializable]
    public class PosSettings
    {
        public Vector3 targetLookAtOffset = new Vector3(0, 2f, 0);
        public float lookSmooth = 50f;
        public float disFromTar = -8;
        public float smooth = 0.05f;
        [HideInInspector]
        public float adjustDis = -8;
    }

    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 30;
        public float minXRotation = -55;
        public float maxYRotation = -90;
        public float minYRotation = -270;
        // How fast the rotation can take place
        public float vOrbitSmooth = 50;
        public float hOrbitSmooth = 50;
    }

    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "Cancel";
        public string ORBIT_HORIZONTAL = "Mouse X";
        public string ORBIT_VERTICAL = "Mouse Y";
        public string ZOOM = "Mouse ScrollWheel";

    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    public PosSettings position = new PosSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler coll = new CollisionHandler();

    // target.position + targetLookAtOffset
    Vector3 lookAtPtPos = Vector3.zero;
    Vector3 des = Vector3.zero;
    Vector3 adjustedDes = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    RaccoonController player;
    float vOrbitInp, hOrbitInp, hOrbitSnapInp;

    private void Start()
    {
        MoveToTar();
        coll.Initialize(Camera.main);
        coll.UpdateCamClipPts(transform.position, transform.rotation, ref coll.adjustedCamClipPts);
        coll.UpdateCamClipPts(des, transform.rotation, ref coll.desiredCamClipPts);
    }

    private void Update()
    {
        GetInput();
        MoveToTar();
        LookAtTar();
        OrbitTar();

        coll.UpdateCamClipPts(transform.position, transform.rotation, ref coll.adjustedCamClipPts);
        coll.UpdateCamClipPts(des, transform.rotation, ref coll.desiredCamClipPts);

        // draw debug lines
        for (int i = 0; i < 5; i++)
        {
            if (debug.drawDesiredCollisionLines)
                Debug.DrawLine(lookAtPtPos, coll.desiredCamClipPts[i], Color.white);
            if (debug.drawAdjustedCollisionLines)
                Debug.DrawLine(lookAtPtPos, coll.adjustedCamClipPts[i], Color.green);
        }

        coll.CheckColliding(lookAtPtPos); // using raycasts
        position.adjustDis = coll.AdjustedDisWithRaycast(lookAtPtPos);
    }
    

    void GetInput()
    {
        //vOrbitInp = Input.GetAxis(input.ORBIT_VERTICAL);
        vOrbitInp = GetYAxis();
        //hOrbitInp = Input.GetAxis(input.ORBIT_HORIZONTAL);
        hOrbitInp = GetXAxis();
        hOrbitSnapInp = Input.GetAxis(input.ORBIT_HORIZONTAL_SNAP);
    }

    private float GetXAxis()
    {
        if (GameManager.instance.UseController)
            return Input.GetAxis("RightJoystickX");
        else
            return Input.GetAxis("Mouse X");
    }

    private float GetYAxis()
    {
        if (GameManager.instance.UseController)
            return Input.GetAxis("RightJoystickY");
        else
            return Input.GetAxis("Mouse Y");
    }


    void MoveToTar()
    {
        lookAtPtPos = target.position + position.targetLookAtOffset;
        // des = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.disFromTar;
        des = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.disFromTar;
        des += lookAtPtPos;

        if (coll.isColliding)
        {
            // adjustedDes = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustDis;
            adjustedDes = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * Vector3.forward * position.adjustDis;
            adjustedDes += lookAtPtPos;

            // Smooth camera movement
            transform.position = Vector3.SmoothDamp(transform.position, adjustedDes, ref camVel, position.smooth);
            
        }
        else
            transform.position = Vector3.SmoothDamp(transform.position, des, ref camVel, position.smooth);
    }

    void LookAtTar()
    {
        Quaternion tarRotation = Quaternion.LookRotation(lookAtPtPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, tarRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTar()
    {
        if (hOrbitSnapInp > 0)
        {
            orbit.yRotation = -180;
        }
        //?
        orbit.xRotation += -vOrbitInp * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInp * orbit.hOrbitSmooth * Time.deltaTime;

        orbit.xRotation = Mathf.Clamp(orbit.xRotation, orbit.minXRotation, orbit.maxXRotation);
        orbit.yRotation = Mathf.Clamp(orbit.yRotation, orbit.minYRotation, orbit.maxYRotation);

    }


    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool isColliding = false;
        [HideInInspector]
        public Vector3[] adjustedCamClipPts;
        [HideInInspector]
        public Vector3[] desiredCamClipPts;

        private Camera cam;
        public void Initialize(Camera camera)
        {
            cam = camera;
            adjustedCamClipPts = new Vector3[5];
            desiredCamClipPts = new Vector3[5];
        }

        public void UpdateCamClipPts(Vector3 camPos, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!cam)
                return;

            // clear the contents of intoArray
            intoArray = new Vector3[5];
            float z = cam.nearClipPlane;
            float x = Mathf.Tan(cam.fieldOfView / 3.41f) * z;
            float y = x / cam.aspect;

            // top left
            // added and rotated the point relative to the cam position
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPos;
            // top right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPos;
            // bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + camPos;
            // bottom right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + camPos;
            // cam pos
            intoArray[4] = camPos - cam.transform.forward;
        }


        bool CollisionDectectedAtClipPts(Vector3[] clipPts, Vector3 tarPos)
        {
            for (int i = 0; i < clipPts.Length; i++)
            {
                Ray ray = new Ray(tarPos, clipPts[i] - tarPos);
                float distance = Vector3.Distance(clipPts[i], tarPos);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }
            return false;
        }

        public float AdjustedDisWithRaycast(Vector3 tarPos)
        {
            float dis = -1;

            for (int i = 0; i < desiredCamClipPts.Length; i++)
            {
                Ray ray = new Ray(tarPos, desiredCamClipPts[i] - tarPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (dis == -1)
                        dis = hit.distance;
                    else
                    {
                        if (hit.distance < dis)
                            dis = hit.distance;
                    }
                }
            }

            if (dis == -1)
                return 0;
            else
                return dis;
        }

        public void CheckColliding(Vector3 tarPos)
        {
            isColliding = CollisionDectectedAtClipPts(desiredCamClipPts, tarPos);
        }
    }
    
}
