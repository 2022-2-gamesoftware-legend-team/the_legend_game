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

## 멀티플레이 관련 Script

### `GameNetworkManager.cs`

멀티플레이 매니저

**`NetworkManager`** 태그 사용

**Auto Create Player 속성을 해제해야 정상적으로 동작**

PlayerCharacterType 변수를 통해 플레이할 캐릭터를 지정할 수 있음

* 변수 값 매핑은 협의후 지정

**UI버튼 등 선택시 PlayerCharacterType 변수 변경 방법**
```C#
GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>().PlayerCharacterType = <캐릭터타입>;
```

### `SyncScore.cs`

멀티플레이 서버 점수 동기화

**`ScoreManager`** 태그 사용

**Score 변경 방법**
```C#
SyncScore score = GameObject.FindGameObjectWithTag("ScoreManager");
score.ChangeScore(score.Score + <증가값>);
```

**Score 변경 감지**

`SyncScore.cs`파일

* `ScoreChanged(oldScore, newScore)`
* `MaxScoreChanged(oldMaxScore, newMaxScore)`

함수 내부에 UI 업데이트 코드 등을 삽입하면 Score가 변경되는 경우 자동으로 해당 코드를 실행

혹은 UI 스크립트의 `Update()` 함수에서
```C#
SyncScore score = GameObject.FindGameObjectWithTag("ScoreManager");
UI컴포넌트_내용_업데이트_함수("Score : " + score.Score);
```

### `SyncHealth.cs`

플레이어/적 프리팹에 해당 스크립트를 추가

**Health값 직접 변경하면 동작하지 않음**

**사용가능 함수**
* `CmdSetHealth(health)`: 체력을 health값으로 변경
* `CmdDecHealth()`: 체력 1 감소
* `CmdIncHealth()`: 체력 1 증가


### `SyncItems.cs`

플레이어 프리팹에 해당 스크립트를 추가

Item은 Set형식으로 저장되며, 중복으로 가질 수 없음

아이템과 Collision이 발생한 경우 나에게 없는 경우만 획득하도록 코드 작성 필요

**사용가능 함수**
* `AddItem(item)`: 아이템 추가
* `HasItem(item)`: 해당 아이템 소지여부 반환
* `Items`: `SortedSet<int>` 자료형
