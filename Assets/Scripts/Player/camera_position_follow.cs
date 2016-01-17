using UnityEngine;
using System.Collections;

public class camera_position_follow : MonoBehaviour {

    //Public Variables
    public Transform player;
    public Transform aimTarget;
    public Texture reticle;
    public Vector3 pivotOffset = new Vector3(1.3f, 0.4f, 0.0f);
    public Vector3 camOffset = new Vector3(0.0f, 0.7f, -2.4f);
    public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f);
    public float smoothingTime = 0.5f;
    public float horizontalAimingSpeed = 270f;
    public float verticalAimingSpeed = 270f;
    public float maxVerticalAngle = 80f;
    public float minVerticalAngle = -80f;
    public float mouseSensitivity = 0.1f;
    
    //Private Variables
    private Transform cam;
    private LayerMask mask;
    private Vector3 smoothPlayerPos;
    private float maxCamDist = 1;
    private float angleH = 0;
    private float angleV = 0;

    // Use this for initialization
    void Start()
    {
        // Add player's own layer to mask
        mask = 1 << player.gameObject.layer;
        // Add Igbore Raycast layer to mask
        mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
        // Invert mask
        mask = ~mask;

        cam = transform;
        smoothPlayerPos = player.position;
        //cam.position = player.forward * 10.0f;
        maxCamDist = 3;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null)
            return;

        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed * Time.deltaTime;

        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed * Time.deltaTime;
        // limit vertical angle
        angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

        // Before changing camera, store the prev aiming distance.
        // If we're aiming at nothing (the sky), we'll keep this distance.
        float prevDist = (aimTarget.position - cam.position).magnitude;

        // Set aim rotation
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
        Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
        cam.rotation = aimRotation;

        // Find far and close position for the camera
        smoothPlayerPos = Vector3.Lerp(smoothPlayerPos, player.position, smoothingTime * Time.deltaTime);
        smoothPlayerPos.x = player.position.x;
        smoothPlayerPos.z = player.position.z;
        Vector3 farCamPoint = smoothPlayerPos + camYRotation * pivotOffset + aimRotation * camOffset;
        Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
        float farDist = Vector3.Distance(farCamPoint, closeCamPoint);

        // Smoothly increase maxCamDist up to the distance of farDist
        maxCamDist = Mathf.Lerp(maxCamDist, farDist, 5 * Time.deltaTime);

        // Make sure camera doesn't intersect geometry
        // Move camera towards closeOffset if ray back towards camera position intersects something 
        RaycastHit hit;
        Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
        float padding = 0.3f;
        if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding, mask))
        {
            maxCamDist = hit.distance - padding;
        }
        cam.position = closeCamPoint + closeToFarDir * maxCamDist;

        // Do a raycast from the camera to find the distance to the point we're aiming at.
        float aimTargetDist;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 100, mask))
        {
            aimTargetDist = hit.distance + 0.05f;
        }
        else
        {
            // If we're aiming at nothing, keep prev dist but make it at least 5.
            aimTargetDist = Mathf.Max(5, prevDist);
        }

        // Set the aimTarget position according to the distance we found.
        // Make the movement slightly smooth.
        aimTarget.position = cam.position + cam.forward * aimTargetDist;
    }

    void OnGUI()
    {
        float scale = 0.1f;
        if (Time.time != 0 && Time.timeScale != 0)
            GUI.DrawTexture(new Rect(Screen.width / 2 - (reticle.width * 0.5f * scale), Screen.height / 2 - (reticle.height * 0.5f * scale), reticle.width*scale, reticle.height*scale), reticle);
    }
































    /***** Own **********
    public float fWidth = 9.0f;         // Desired width 
    public float distanceAway;          // distance from the back of the craft
    public float distanceUp;            // distance above the craft
    public float smooth;                // how smooth the camera movement is
    private Vector3 targetPosition;     // the position the camera is trying to be in

    float horizontalSpeed = 140.0f;
    float verticalSpeed = 140.0f;


    Transform the_player; 

    void Start()
    {
        /*
        float fT = fWidth / Screen.width * Screen.height;
        fT = fT / (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad));
        Vector3 v3T = Camera.main.transform.position;
        v3T.z = -fT;
        transform.position = v3T;
        

        the_player = GameObject.Find("Player").transform;

        transform.position = new Vector3(2.932574e-06f, 2.999999f, 8.999997f);
        transform.rotation = Quaternion.Euler(11.30993f, 180.0f, 0.0f);
       
    }

    void Update()
    {          
        float h = horizontalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        float v = verticalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
        //transform.Translate(v, 0, 0);
        transform.RotateAround(the_player.position, the_player.right, h);
        transform.RotateAround(the_player.position, the_player.up, v);



        // setting the target position to be the correct offset from the hovercraft
        //targetPosition = the_camera.position + Vector3.up * Input.GetAxis("Mouse Y") - the_camera.forward * distanceAway;

        // making a smooth transition between it's current position and the position it wants to be in
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);


        // make sure the camera is looking the right way!
        //transform.LookAt(the_camera.transform.position + the_camera.transform.forward * 6);

    }

    ****************/
}
