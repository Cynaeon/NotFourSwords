using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraControl : MonoBehaviour
{
    private PauseManager _pauseManager;
    private bool _isPaused;

    public Transform target;
    public Transform lockOnCameraSpot;
    public string playerPrefix;

    private Rigidbody _rigidbody;
    public float lockOnSpeed = 10.0f;

    float x = 0.0f;
    float y = 0.0f;
    
    private float distance;
    private float defaultDist;
    private float xSpeed;
    private float ySpeed;
    private float yMinLimit;
    private float yMaxLimit;
    //private float distanceMin;
    //private float distanceMax;

    void Start()
    {
        GameObject cameraManagerGO = GameObject.Find("CameraManager");
        CameraManager cameraManager = cameraManagerGO.GetComponent<CameraManager>();
        distance = cameraManager.distance;
        xSpeed = cameraManager.xSpeed;
        ySpeed = cameraManager.ySpeed;
        yMinLimit = cameraManager.yMinLimit;
        yMaxLimit = cameraManager.yMaxLimit;
        defaultDist = distance;
        //distanceMin = cameraManager.distanceMin;
        //distanceMax = cameraManager.distanceMax;

        _pauseManager = GetComponent<PauseManager>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        _rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = true;
        }
    }


    private void Update()
    {
        _isPaused = _pauseManager.isPaused;
    }

    void LateUpdate()
    {
        if (!_isPaused) {
            if (target)
            {
                if (Input.GetButton(playerPrefix + "LockOn"))
                {
                    var targetRotationAngle = target.eulerAngles.y;
                    var currentRotationAngle = transform.eulerAngles.y;
                    x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, lockOnSpeed * Time.deltaTime);
                }

                // Scale speed with distance
                xSpeed = 100 / distance;

                x += Input.GetAxis(playerPrefix + "HorizontalRightStick") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis(playerPrefix + "VerticalRightStick") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                //Debug.DrawLine(transform.position, target.position, Color.green, 2);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    if (hit.transform.tag == "Walls")
                    {
                        distance = hit.distance;
                    }
                }
                else
                {
                    if (distance <= 10.0f)
                    {
                        distance += 0.1f;
                    }
                }

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;

                if (Input.GetAxis(playerPrefix + "FirstPerson") > 0.5)
                {
                    transform.position = target.position;
                    transform.rotation = target.rotation;
                }
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}