using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField]
    private Transform[] paths; //순찰 경로
    private int currentPath = 0; //현재 목표지점 인덱스
    private float moveSpeed = 2f; //이동 속도
    private SpriteRenderer spriteRenderer;
    private bool flag = true;

     private void Awake(){
        spriteRenderer = GetComponent < SpriteRenderer >();
    }

    private void Update(){
        Vector3 direction = (paths[currentPath].position - transform.position).normalized;
        // 이동 방향 설정 (목표위치 - 내위치 ) 정규화
        transform.position += direction * moveSpeed * Time.deltaTime;
        // 오브젝트 이동


        // 목표 위치에 거의 도달했을 때
        if((paths[currentPath].position - transform.position).sqrMagnitude < 2f)
        {
            // 목표 위치 변경 (순찰 경로 순환)
            if(currentPath < paths.Length-1) currentPath++;
            else {
                currentPath = 0;
            }
            if(flag == true){
                    flag = false;
                    spriteRenderer.flipX = (flag);
                }
                else{
                    flag = true;
                    spriteRenderer.flipX = (flag);
                }
            
        }
    }
}
