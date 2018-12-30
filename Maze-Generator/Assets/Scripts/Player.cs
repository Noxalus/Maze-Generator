using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;

    public void Initialize(Vector2 position)
    {
        var initialPosition = new Vector3(position.x, 0f, position.y);
        initialPosition.y = 2f;

        _rigidbody.MovePosition(initialPosition);
    }
}
