# 멀티플레이 동기화 방법

* Script 제작시 MonoBehaviour가 아닌 NetworkBehaviour를 상속받아 제작
```C#
using Mirror;

public class ClassName : NetworkBehaviour {
    ...
}
```
* GameObject에 NetworkRigidbody2D, NetworkTransform, NetworkAnimator 컴포넌트 추가
    * 플레이어의 물리 처리는 클라이언트에서 이루어지므로 NetworkRigidbody2D, NetworkTransform, NetworkAnimator의 Client Authority를 체크해야 함
* Rigidbody2D및 Transform은 위에서 추가한 컴포넌트에 의해 동기화 됨
    * `void Update()` 함수 대신 `void FixedUpdate()`를 사용해야 Rigidbody 동기화가 정상적으로 동작함
    * `Time.deltaTime` 대신 `Time.fixedDeltaTime`을 사용 함
* 기타 아이템 사용 등과 같은 처리는 아래 규칙을 사용해야 서버에서 실행되어 전체에 동기화 가능
    * 함수 이름은 Cmd로 시작해야 함
    * `[Command]`를 함수 선언에 포함해야 함
```C#
[Command]
void CmdUseItem() {
    ...
}
```
* 서버에서 Cmd~함수를 통해 클라이언트 각각이 함수를 실행하게 하기 위해서 Rpc~ 함수를 사용함
    * 함수 이름은 Rpc로 시작해야 함
    * `[ClientRpc]`를 함수 선언에 포함해야 함
```C#
[ClientRpc]
void RpcOnAttack() {
    ...
}
```
* 충돌감지 등 이벤트 함수는 서버에서만 처리하도록 설정
    * `[ServerCallback]`을 함수 선언에 포함해야 함
```C#
[ServerCallback]
void OnTriggerEnter2D(Collider2D other) {
    ...
}
```
* 플레이어 키 입력 등 GameObject가 내것인지 확인하려면 `isLocalPlayer`변수를 확인함
* 서버에서 동일 스크립트를 사용하는 모든 GameObject끼리 어떤 변수를 동기화 하려면 `[SyncVar]`를 변수 선언에 포함해야 함
    * 해당 변수를 수정하려면 `[ServerCallback]`으로 선언된 함수 내부에서만 가능
```C#
[SyncVar] public int health = 5;

[ServerCallback]
void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.tag == "Enemy")
    {
        --health;
        if (health == 0)
            NetworkServer.Destroy(gameObject);
    }
}
```