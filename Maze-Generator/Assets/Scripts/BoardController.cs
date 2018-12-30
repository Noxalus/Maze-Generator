using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private Transform _mazeHolder;

    public void SetOrigin(Vector2 origin)
    {
        transform.localPosition = new Vector3(origin.x, 0f, origin.y);
        _mazeHolder.transform.localPosition = new Vector3(-origin.x, 0f, -origin.y);
    }
}
