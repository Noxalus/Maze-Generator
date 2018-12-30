using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private Transform _mazeHolder;

    private void Update()
    {
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = DeviceRotation.Get();
        transform.localRotation = deviceRotation;

        //Quaternion eliminationOfXY = Quaternion.Inverse(
        //    Quaternion.FromToRotation(
        //        referenceRotation * Vector3.forward,
        //        deviceRotation * Vector3.forward
        //    )
        //);

        //Quaternion rotationZ = eliminationOfXY * deviceRotation;
        //float roll = rotationZ.eulerAngles.z;
    }

    public void SetOrigin(Vector2 origin)
    {
        transform.localPosition = new Vector3(origin.x, 0f, origin.y);
        _mazeHolder.transform.localPosition = new Vector3(-origin.x, 0f, -origin.y);
    }
}
