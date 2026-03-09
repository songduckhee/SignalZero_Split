
##### + 이 레포는 원본 프로젝트에서 개인 코드만 분리한 레포입니다
---


# SignalZero
# 프로젝트 이름

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [팀소개](#팀소개)
3. [주요기능](#주요기능)
4. [개발기간](#개발기간)
5. [기술스택](#기술스택)
6. [프로젝트 파일 구조](#프로젝트-파일-구조)
7. [와이어프레임](#와이어프레임)

    
## 👨‍🏫 프로젝트 소개

### 장르
    쿼터뷰, 탑뷰 슈팅 / 로그라이크 / 액션 어드벤쳐 / 3D
### 플랫폼 :
    PC
### 로그라인 : 
    마우스로만 진행하는 속도감 있는 슈팅 로그라이크
### 게임 배경 :
    우주적 병기로 개조된 인간이었던 뇌인 시뮬러의 소원은 평범한 인간이 되는것. 하지만 그러기 위해선 섹터 3으로 가서 레테를 탈취해야만 한다.
    길목마다 발칸의 약탈자,제국의 커세어,림보의 탈영병이 당신을 노린다. 레테의 빙하에 가까워질수록 상황은 변한다. 이해를 거부하는 이상현상과 존재 자체만으로 이지를 침식하는 우주 생물.
    당신은 선택할 수 있다.
    레테가 있는 곳까지 잡다한 의뢰를 수행하며 크레딧을 모으고, 레테를 찬탈해 인간이 되는 꿈을 이루거나. 배신하지 않고 임무만 완수하거나.
    혹은, 그 어디에도 속하지 않는 길을 찾거나.
  

## 🌠팀소개
**SignalZero**
<img width="1204" height="585" alt="Image" src="https://github.com/user-attachments/assets/d0698c81-9e10-481e-8cf4-f4f399c694c8" />

## 💜주요기능

- Unity 엔진을 사용한 게임 플레이 시스템 구현
- 플레이어 컨트롤러, 전투 로직, 무기 시스템 설계
- 몬스터 패턴 및 보스 구조 모듈화
- ScriptableObject 기반 데이터 관리 (무기, 몬스터, 아이템)
- Prefab 구조를 활용한 오브젝트 재사용 및 관리
- 필드 이벤트 및 상호작용 시스템 구현
- UI 시스템 (인벤토리, 퀘스트, 미니맵 등) 구성
- 사운드 및 VFX를 활용한 게임 피드백 강화

## ⏲️개발기간
- 2025.12.05(금) ~ 2026.01.02(금)

## 📚️기술스택

### ✔️ Language
- C# / Unity
### ✔️ Version Control
- Git
- GitHub
- GitHub Desktop
- Fork
### ✔️ IDE
- Visual Studio
- Visual Studio Code
## 프로젝트 파일 구조
```
📦Assets
 ┣ 📂01.Scripts
 ┃ ┣ 📂Field
 ┃ ┃ ┣ 📂BlueSpot&Boss
 ┃ ┃ ┣ 📂Dialogue
 ┃ ┃ ┣ 📂Event
 ┃ ┃ ┃ ┣ 📂EventImplementation
 ┃ ┃ ┃ ┣ 📂EventSO
 ┃ ┃ ┣ 📂Obstacle
 ┃ ┃ ┃ ┣ 📂ObstacleShipUI
 ┃ ┃ ┃ ┃ ┣ 📂ObjectPooling
 ┃ ┃ ┃ ┃ ┣ 📂ObstacleSO
 ┃ ┃ ┃ ┣ 📂SpaceShipSO
 ┃ ┃ ┣ 📂Skybox
 ┃ ┃ ┣ 📂TestPlayer
 ┃ ┣ 📂GameOver
 ┃ ┣ 📂MiniMap
 ┃ ┃ ┣ 📂MapSpriteSO
 ┃ ┃ ┣ 📂MapMap
 ┃ ┣ 📂SpaceShip
 ┃ ┃ ┣ 📂SpaceShipScripts
 ┣ 📂02.Datas
 ┃ ┣ 📂field
 ┃ ┃ ┣ 📂EventSO
 ┃ ┃ ┣ 📂ObstacleSO
 ┃ ┃ ┣ 📂RewardSO
 ┃ ┃ ┣ 📂ShopSO
 ┃ ┣ 📂MiniMap
 ┃ ┃ ┣ 📂CellSpriteSO
 ┣ 📂03.Prefabs
 ┃ ┣ 📂Field
 ┃ ┃ ┣ 📂Manager
 ┃ ┃ ┣ 📂UI
 ┃ ┃ ┃ ┣ 📂Button
 ┃ ┃ ┣ 📂VFX
 ┃ ┣ 📂MiniMap
 ┃ ┣ 📂Npc
 ┃ ┣ 📂SpaceShip
 ┣ 📂05.Animations
 ┃ ┣ 📂BlueSpotUI
 ┃ ┣ 📂NpcAnimation
 ┃ ┣ 📂SpaceShipUI
 ┣ 📂72_Font
 ┣ 📂99.Extra
 ┣ 📂Plugins
 ┣ 📂ProtoTypeCS
 ┣ 📂Resources
 ┣ 📂TextMesh Pro
 ┣ 📂TutorialInfo
 ```
## 와이어프레임
https://www.figma.com/board/WEq6ZOYA8HWJTYJM7v6r5C/%EC%8B%9C%EA%B7%B8%EB%84%90-%EC%A0%9C%EB%A1%9C-%EC%99%80%EC%9D%B4%EC%96%B4-%ED%94%84%EB%A0%88%EC%9E%84?node-id=0-1&t=5ophUx9kGWc5PBVA-1
