using UnityEngine;

public class PlayerFly : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverHeight = 5f;
    public float riseSpeed = 5f;
    public float forwardSpeed = 5f;
    public float tiltAngle = 25f;       // Tilt for side movement
    public float tiltSpeed = 5f;        // How fast the tilt interpolates
    public bool autoHover = false;

    [Header("Bobbing Settings")]
    public float bobAmplitude = 0.2f;   // How high the bob moves up and down
    public float bobFrequency = 2f;     // Speed of the bobbing

    [Header("Debug / Options")]
    public bool isFlying = false;

    private Transform rootObject;
    private MonoBehaviour legoMovement;
    private float targetY = 0f;
    private bool rising = false;
    private Quaternion originalRotation;
    private float currentTilt = 0f;
    private float bobOffset = 0f;

    void Start()
    {
        rootObject = transform;
        originalRotation = rootObject.rotation;

        // Find LEGO movement script
        var mc = GetComponentInChildren(System.Type.GetType("MinifigController"));
        if (mc != null)
            legoMovement = (MonoBehaviour)mc;

        if (autoHover)
            StartFlying();
    }

    void Update()
    {
        if (!isFlying) return;

        // Forward/backward movement
        float moveForward = Input.GetAxis("Vertical"); // W/S
        rootObject.position += rootObject.forward * moveForward * forwardSpeed * Time.deltaTime;

        // Left/right tilt
        float moveSide = Input.GetAxis("Horizontal"); // A/D
        float targetTilt = moveSide * tiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSpeed * Time.deltaTime);

        // Apply rotation: X = 90° forward tilt, Y = original, Z = side tilt
        rootObject.rotation = Quaternion.Euler(90f, originalRotation.eulerAngles.y, -currentTilt);
    }

    void LateUpdate()
    {
        if (!isFlying) return;

        // Smooth rise to target hover height
        if (rising)
        {
            float newY = Mathf.MoveTowards(rootObject.position.y, targetY, riseSpeed * Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, newY, rootObject.position.z);

            if (Mathf.Abs(newY - targetY) < 0.01f)
                rising = false;
        }

        // Apply bobbing motion
        bobOffset += Time.deltaTime * bobFrequency * Mathf.PI * 2f; // Convert frequency to radians
        float bobY = Mathf.Sin(bobOffset) * bobAmplitude;
        rootObject.position = new Vector3(rootObject.position.x, targetY + bobY, rootObject.position.z);
    }

    public void StartFlying()
    {
        if (legoMovement != null)
            legoMovement.enabled = false;

        targetY = rootObject.position.y + hoverHeight;
        rising = true;
        isFlying = true;
        currentTilt = 0f;
        bobOffset = 0f;
    }

    public void StopFlying()
    {
        isFlying = false;
        rising = false;

        if (legoMovement != null)
            legoMovement.enabled = true;

        // Reset rotation
        rootObject.rotation = originalRotation;
    }
}