using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private PauseManager _pauseManager;
    private bool _isPaused;

    public Transform target;
    public Transform lockOnCameraSpot;
    public enum Players
    {
        NotSelected,
        P1_,
        P2_,
        P3_,
        P4_
    }
    public Players playerPrefix;

    private Rigidbody _rigidbody;
    public float lockOnSpeed = 10.0f;

    public Transform defaultPos;

    float x = 0.0f;
    float y = 0.0f;
    
    private float distance;
    private float defaultDist;
    private float xSpeed;
    private float ySpeed;
    private float yMinLimit;
    private float yMaxLimit;
    private float distanceMin;
    private float distanceMax;
    private bool _lockedOn;
    private bool firstPerson;

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
        distanceMin = cameraManager.distanceMin;
        distanceMax = cameraManager.distanceMax;

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
        if (!_isPaused)
        {
            if (playerPrefix != Players.NotSelected)
            {

                if (Input.GetButton(playerPrefix + "LockOn"))
                {
                    var targetRotationAngle = target.eulerAngles.y;
                    var currentRotationAngle = transform.eulerAngles.y;
                    x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, lockOnSpeed * Time.deltaTime);
                    _lockedOn = true;
                }
                else
                {
                    _lockedOn = false;
                }

                if (Input.GetAxis(playerPrefix + "FirstPerson") > 0.5)
                {
                    firstPerson = true;
                }
                else
                {
                    firstPerson = false;
                }

                // Scale speed with distance
                xSpeed = 100 / distance;

                if (!_lockedOn && !firstPerson)
                {
                    x += Input.GetAxis(playerPrefix + "HorizontalRightStick") * xSpeed * distance * Time.deltaTime; //0.02f;
                    y += Input.GetAxis(playerPrefix + "VerticalRightStick") * ySpeed * Time.deltaTime; // 0.02f;
                }

                y = ClampAngle(y, yMinLimit, yMaxLimit);
                distance = Mathf.Clamp(distance, distanceMin, distanceMax);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                RaycastHit hit;
                //Debug.DrawLine(target.position, defaultPos.position, Color.green, 1);
                if (Physics.Linecast(target.position, defaultPos.position, out hit))
                {
                    if (hit.transform.tag != "Player")
                    {
                        //Debug.Log(hit.transform.name);
                        if (hit.transform.tag == "Walls" || hit.transform.tag == "Exit")
                        {
                            distance = hit.distance;
                        }
                    }
                }
                else
                {
                    distance = defaultDist;
                }

                Vector3 negDistance2 = new Vector3(0.0f, 0.0f, -defaultDist);
                Vector3 position2 = rotation * negDistance2 + target.position;

                defaultPos.rotation = rotation;
                defaultPos.position = position2;

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;

                if (firstPerson)
                {
                    Vector3 pos = new Vector3(target.position.x, target.position.y + .6f, target.position.z);
                    transform.position = pos;
                    transform.rotation = target.rotation;
                }
            }
        }
    }

    public void SetStartPosition()
    {
        var targetRotationAngle = target.eulerAngles.y;
        x = targetRotationAngle;
        distance = defaultDist;
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