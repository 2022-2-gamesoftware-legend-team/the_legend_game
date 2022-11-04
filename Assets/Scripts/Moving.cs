using UnityEngine;

public class Moving : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private bool _isjumping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Move GameObject with given direction.
    /// </summary>
    /// <param name="direction">Vector3.left or Vector3.right</param>
    /// <param name="deltaTime">deltaTime</param>
    public void Move(Vector3 direction, float deltaTime) {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.MovePosition(rigid.position + (direction * speed * deltaTime));
    }

    /// <summary>
    /// Jump GameObject
    /// </summary>
    public void Jump() {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (!_isjumping) {
            _isjumping = true;
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision col) {
        print(col.transform.tag);
        if (col.transform.tag == "Ground") {
            _isjumping = false;
        }
    }
}
