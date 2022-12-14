# 2022-2 GameSoftware - Tombengers

https://github.com/2022-2-gamesoftware-legend-team/the_legend_game

## 게임소프트웨어 1분반 3조

* 박민준/소프트웨어학부/3학년
* 박종흠/소프트웨어학부/3학년
* 박준용/소프트웨어학부/4학년
* 이세희/산림환경시스템/4학년
* 최보석/소프트웨어학부/3학년

## 프로젝트 환경

* 유티니 버전: 2021.3.9f
* 2D 템플릿 기반

## 실행시 참고사항

* 16:9 화면비율에 최적화 되어있음
* 멀티플레이 기능을 사용하기 위해서는 7777번 포트가 개방되어 있어야 함

## 협업 활동

* Github
![Screenshot_20221209_104645](https://user-images.githubusercontent.com/6850405/206716507-a48d1b3a-6c3b-4a1c-b6e0-b54d1fb80d31.png)

* Trello
![Trello1](https://user-images.githubusercontent.com/6850405/206715095-5779bd14-3cb3-4ba8-833e-b7347e7e0cfb.png)
![Trello2](https://user-images.githubusercontent.com/6850405/206715120-d5da121c-40ef-4b7a-95d7-ae044c2d53c6.png)

* Google Docs
![Screenshot_20221209_104841](https://user-images.githubusercontent.com/6850405/206716713-e35aa866-2eca-435e-9ac1-ed22163c6852.png)


## 사용 Asset

* 플레이어 : 
   * https://assetstore.unity.com/packages/2d/characters/hero-nad-opponents-animation-140776 Hero and Opponents Animation
* 적 :
   * https://assetstore.unity.com/packages/2d/characters/medieval-warrior-pack-159577 Medieval Warrior Pack (Luiz Melo)
   * https://assetstore.unity.com/packages/2d/characters/bandits-pixel-art-104130 Bandits (Sven Thole)
   * https://assetstore.unity.com/packages/2d/characters/medieval-warrior-pack-2-174788 Medieval Warrior Pack 2
   * https://assetstore.unity.com/packages/2d/characters/medieval-king-pack-2-174863 Medieval King Pack 2
* 맵 : 
   * 2D Platfrom Tile Set (동굴)
   * 2D Jungle Side    (정글)
   * Pixel Lost Game Scene (마을)
   * Pixel Art Platformer (마을 -> 성)
   * Platformer Set (성)

## Coding Guidlines(권장 가이드라인)

### C# Coding Guideline

https://vmsdurano.com/c-coding-guidelines-and-practices/

### GIT Commit Guideline

<img width="535" alt="화면 캡처 2022-11-10 134122" src="https://user-images.githubusercontent.com/85275893/201002326-84ab80ac-af5f-4b58-b216-26341ddd6079.png">

* gitflow 방식 사용
* 커밋 메시지 한글로 작성

## Branches

* Feat_Map1,2,3,4,5 : 맵1~5 작성
* Feat_Map: 모든 맵 통합
* Feat_multiplayer: 멀티플레이 구현 및 최종 마무리
* Feat_player_pmj : 플레이어 캐릭터 구현
* Feat_EnumyJyp : 적 캐릭터 구현
* Feat_Player&UI : Feat_player_pmj Merge 후 화면 UI 구성

## Project Structure

* Assets: 프로젝트 에셋 폴더
    * Materials: 생성한 Material들
    * Prefabs: 생성한 Prefab들
        * PrefabName1, PrefabName2 …
    * Scenes: 생성한 Scene들
    * Scripts: 생성한 C# Script들
    * Tilemap: 생성한 Tilemap들
        * Tilemap1, Tilemap2…
    * Animator: 생성한 animation clip들
        * playerIdle, enemyIdle…


