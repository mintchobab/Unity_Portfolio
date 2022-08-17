# Unity_Portfolio

Unity로 제작한 포트폴리오입니다.
<br>
PC와 모바일에서 ~~~~

<br>
<br>

## 링크
영상 (기능) : https://youtu.be/abKHZa3Ivpo
<br>
영상 (플레이 영상) : https://youtu.be/OaDpxv8fKhE
<br>
기술서 :

<br>
<br>

## 구현된 기능
<!-- - [다중언어 지원](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/language/language.md)
- [대화](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/talk/talk.md)
- [아이템 인벤토리](https://github.com/mintchobab/Unity_Portfolio/tree/main/contents/inventory_item/inventory_item.md)
- [장비 인벤토리](https://github.com/mintchobab/Unity_Portfolio/tree/main/contents/inventory_equip/inventory_equip.md)
- [채집](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/collect/collect.md)
- [퀘스트](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/quest/quest.md)
- [전투](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/combat/combat.md)
- [해상도 대응](https://github.com/mintchobab/Unity_Portfolio/blob/main/contents/resolution/resolution.md) -->

 
- [다중 언어 지원](#다중-언어-지원) <!-- omit in toc -->
  - [json](#json)
  - [언어 선택](#언어-선택)
- [대화](#대화)
  - [대화 시작](#대화-시작)
  - [대화 진행](#대화-진행)
- [아이템 인벤토리](#아이템-인벤토리)
  - [아이템 획득](#아이템-획득)
  - [아이템 사용](#아이템-사용)
  - [인벤토리 확장](#인벤토리-확장)
- [장비 인벤토리](#장비-인벤토리)
  - [정렬](#정렬)
  - [장착](#장비-장착)
  - [해제](#장비-해제)
  - [스텟 적용](#스텟-적용)

<br>
<br>

## 다중 언어 지원

<br>

### json

<br>

json 파일에 저장된 데이터에 따라 게임내 텍스트들의 언어를 변경할 수 있다.
<br>
현재 한국어와 영어 두 가지 언어만 저장되어 있지만, 추가하고자 하는 언어의 문장들만 입력하면 적용이 가능하다.

![json](./images/language_json.jpg)

<br>
<br>
<br>
<br>

### 언어 선택

<br>

언어 변경은 게임 중에는 불가능하고, 시작 화면에서만 선택할 수 있다.

![언어 선택](./images/language_select.png)

<br>
<br>
<br>
<br>

한국어와 영어 비교
<p align="left"> <img src="./images/language_1.png" width="400"> <img src="./images/language_2.png" width="400"> </p>
<p align="left"> <img src="./images/language_3.png" width="400"> <img src="./images/language_4.png" width="400"> </p>

<br>
<br>
<br>
<br>

## 대화

<br>

### 대화 시작

<br>

NPC의 근처로 이동하면 버튼의 이미지가 대화 모양으로 변경된다.
<br>
버튼을 클릭하면 자동으로 NPC 근처로 이동하고 일정거리만큼 가까워지면 대화가 시작된다.

![대화 시작](./images/talk_move.gif)

<br>
<br>
<br>
<br>

### 대화 진행

<br>

대화창은 타이핑되는 것처럼 문장이 출력되고 화면을 클릭하면 다음 대화문이 출력된다.
<br>
대화문이 출력되는 도중에 화면을 클릭하면 해당 대화문의 끝까지 한번에 출력된다.

![대화](./images/talk_talk.gif)

<br>
<br>
<br>
<br>

## 아이템 인벤토리

<br>

### 아이템 획득

<br>

아이템을 획득하면 순서대로 인벤토리에 저장된다.

![인벤토리](./images/inventory.png)

<br>
<br>
<br>
<br>

아이템마다 최대 개수가 존재하고, 개수를 초과하게 되면 새로운 슬롯에 생성된다.

![아이템 추가](./images/inventory_item_add.gif)

<br>
<br>
<br>
<br>

### 아이템 사용

<br>

아이템을 클릭하면 팝업창이 활성화 되고 해당 아이템에 대한 정보가 출력된다.

![팝업](./images/inventory_item_popup.gif)

<br>
<br>
<br>
<br>

아이템 사용 가능 여부에 따라 팝업창의 버튼이 활성화 / 비활성화 된다.

<p align="left"> <img src="./images/inventory_item_available.png" width="400"> <img src="./images/inventory_item_unavailable.png" width="400"> </p>

<br>
<br>
<br>
<br>

### 인벤토리 확장

<br>

'+' 버튼을 누르면 최대 50칸까지 인벤토리를 확장할 수 있다.

![확장](./images/inventory_item_expand.gif)

<br>
<br>
<br>
<br>

## 장비 인벤토리

<br>

### 정렬

<br>

탭 버튼을 클릭하면 해당 장비가 인벤토리에 앞쪽으로 오게 정렬된다. (같은 종류끼리는 아이템의 ID 순서에 따라 정렬)

![정렬](./images/inventory_equip_tab.gif)

<br>
<br>
<br>
<br>

### 장비 장착

<br>

장비를 클릭하면 해당 장비를 설명하는 팝업창이 나오고 장착 버튼을 누르면 부위에 맞게 자동으로 장착된다.

![장착](./images/inventory_equip_equip.gif)

<br>
<br>
<br>
<br>

### 장비 해제

<br>

장착된 장비를 클릭하면 장비를 해제할 수 있다.

![장착 해제](./images/inventory_equip_unequip.gif)

<br>
<br>
<br>
<br>

### 스텟 적용

<br>

장비가 장착되거나 해제됨에 따라 플레이어의 스텟이 변경된다.

![스텟 변경](./images/inventory_equip_stat.gif)