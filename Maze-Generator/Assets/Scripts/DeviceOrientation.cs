using UnityEngine;

public static class DeviceRotation
{
    private static bool gyroInitialized = false;
    private static Quaternion referenceOrientation = Quaternion.identity;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static void UpdateReferenceOrientation()
    {
        referenceOrientation = GetOrientation();

        Debug.Log("Has gyroscope: " + HasGyroscope);
        Debug.Log("Reference orientation: " + referenceOrientation);
        Debug.Log("Attitude: " + Input.gyro.attitude);

        //referenceOrientation = Input.gyro.attitude;
    }

    public static Quaternion GetOrientation()
    {
        if (!gyroInitialized)
            InitializeGyroscope();

        return HasGyroscope
            ? ReadGyroscopeRotation()
            : Quaternion.identity;
    }

    private static void InitializeGyroscope()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;              // enable the gyroscope
            //Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        }

        Debug.Log("Gyroscope initialized");

        gyroInitialized = true;
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        //Debug.Log("Device attitude: " + Input.gyro.attitude);
        //Debug.Log("Reference orientation: " + referenceOrientation);

        return referenceOrientation * Input.gyro.attitude;
    }
}