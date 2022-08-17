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


<br>
<br>

<hr>

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