using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private Transform _mazeHolder = null;

    private bool _gyroscopeInitialized = false;
    private Quaternion _referenceOrientation = Quaternion.identity;
    private Quaternion _currentRotation = Quaternion.identity;
    private float _angularSpeed = 50f;

    public void Initialize()
    {
        if (!_gyroscopeInitialized)
        {
            InitializeGyroscope();
        }

        _referenceOrientation = Input.gyro.attitude;
    }

    private void InitializeGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;              // enable the gyroscope
            //Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        }

        Debug.Log("Gyroscope initialized");

        _gyroscopeInitialized = true;
    }

    private void Update()
    {
        if (DeviceRotation.HasGyroscope)
        {
            Quaternion deviceRotation = _referenceOrientation * Input.gyro.attitude;

            //Debug.Log("Device rotation: " + deviceRotation);

            //Quaternion eliminationOfXY = Quaternion.Inverse(
            //    Quaternion.FromToRotation(
            //        referenceRotation * Vector3.forward,
            //        deviceRotation * Vector3.forward
            //    )
            //);

            //Quaternion rotationZ = eliminationOfXY * deviceRotation;
            //float roll = rotationZ.eulerAngles.z;

            var boardRotation = Quaternion.Inverse(Quaternion.Euler(
                deviceRotation.eulerAngles.x,
                0f,
                deviceRotation.eulerAngles.z
            ));

            _currentRotation = deviceRotation;
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _currentRotation = Quaternion.Euler(
                    _currentRotation.eulerAngles.x + (_angularSpeed * Time.deltaTime),
                    _currentRotation.eulerAngles.y,
                    _currentRotation.eulerAngles.z
                );
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _currentRotation = Quaternion.Euler(
                    _currentRotation.eulerAngles.x + (-_angularSpeed * Time.deltaTime),
                    _currentRotation.eulerAngles.y,
                    _currentRotation.eulerAngles.z
                );
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _currentRotation = Quaternion.Euler(
                    _currentRotation.eulerAngles.x,
                    _currentRotation.eulerAngles.y,
                    _currentRotation.eulerAngles.z + (_angularSpeed * Time.deltaTime)
                );
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _currentRotation = Quaternion.Euler(
                    _currentRotation.eulerAngles.x,
                    _currentRotation.eulerAngles.y,
                    _currentRotation.eulerAngles.z - (_angularSpeed * Time.deltaTime)
                );
            }
        }

        transform.rotation = _currentRotation;
    }

    public void SetOrigin(Vector2 origin)
    {
        transform.localPosition = new Vector3(origin.x, 0f, origin.y);
        _mazeHolder.transform.localPosition = new Vector3(-origin.x, 0f, -origin.y);
    }
}
