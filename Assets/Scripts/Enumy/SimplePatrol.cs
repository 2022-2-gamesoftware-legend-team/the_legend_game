using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SimplePatrol : NetworkBehaviour
{
    [SerializeField]
    Transform enemyTransform;
    Enumy enemy;
    private int currentPath = 0; //현재 목표지점 인덱스
    private float moveSpeed = 2f; //이동 속도

    public Vector2 StartPosition;

    public int MoveRange;

    [SyncVar]
    bool isMoveBackward = false;

    [SyncVar]
    bool isFollowPlayer = false;
    private SpriteRenderer spriteRenderer;
    private bool flag = true;
     private void Awake(){
        
        spriteRenderer = GetComponent < SpriteRenderer >();
        // enemy = GetComponent<Enumy>();
        // enemyTransform = GetComponent<Transform>();
    }

    private void Update(){

        if (isServer){

            // 범위 벗어남 확인
            if (transform.position.x < (StartPosition.x - MoveRange)) {
                isMoveBackward = false;
            }
            if (transform.position.x > (StartPosition.x + MoveRange)) {
                isMoveBackward = true;
            }

            // Player 탐지
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) {
                if (Vector2.Distance(transform.position, player.transform.position) < 3.0f) {
                    // follow
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    isMoveBackward = direction.x < 0;
                    transform.position += direction * moveSpeed * Time.deltaTime;
                    isFollowPlayer = true;
                    break;
                }
            }

            if (!isFollowPlayer) {
                Vector3 direction = (isMoveBackward ? Vector3.left : Vector3.right);
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
        // Sprite Flip
        spriteRenderer.flipX = isMoveBackward;
    }
}
