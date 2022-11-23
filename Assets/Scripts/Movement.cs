using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.0f;
    private Vector3 moveDirection = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float jumpForee = 5.0f;
    private Rigidbody2D rigid2d;
    private LayerMask groundeLayer;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isGrounded;
    private Vector3 footPosition;
    public int maxJumpcount = 1;
    private int currentJumpCount = 0;

    private void Awake(){
        rigid2d = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        Bounds bounds = capsuleCollider2D.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundeLayer);


        if(isGrounded == true && rigid2d.velocity.y <=0)
        {
            currentJumpCount = maxJumpcount;
        }
        
    }

    public void Move(float x)
    {
        rigid2d.velocity = new Vector2(x*moveSpeed, rigid2d.velocity.y);
    }
    public void Jump()
    {
        if(currentJumpCount >0)
        {
            rigid2d.AddForce(Vector2.up * jumpForee, ForceMode2D.Impulse);
            currentJumpCount--;
        }
    }
}
