# OurCraft

## How to play your game
移動: WASD<br/>
跑步: double click W<br/>
跳躍: Space<br/>
移動視角: Mouse<br/>
物品欄選擇: alpha-num<br/>
取得物品: Left Click<br/>
放置物品: Right Click<br/>
回到選單: Esc<br/>
開啟console: Grave accent(`)<br/>

## Environment
Windows

## Your game

## Game design (how to design your character, monster, etc.)
character: 就是普通的維京人，喜歡蓋房子<br/>
monster: 殭屍化的維京人，動作緩慢，很吵<br/>
遊戲中的一天(24000單位時間)為現實中的20分鐘，到晚上會生成怪物<br/>
使用console的saveMap可以儲存地圖，Ex. saveMap("myCYKA")。預設儲存位置為"我的文件/Ourcraft Maps/"<br/>
你也可以用console的loadMap來載入地圖。在使用saveMap、loadMap時若參數留白，將會使用預設檔名map1.json<br/>

## Bonus
Chen:
1. 以根據滑鼠移動多寡來移動視角，並且將其上下固定在-90度到90度之間
2. 游標固定在螢幕中間以方便操作
3. 放置方塊時排列整齊，且不會重疊
4. 放置方塊時玩家需在放置位置一定距離內
5. 在螢幕左上顯示玩家位置與滑鼠點選位置
6. 降雨系統(陰天、降雨、降雨機率)
7. 怪物雙聲道音效
8. 按兩次W能跑

Weber
1. 遊戲地圖的存檔
2. Lua Console!!! (可以使用原生lua語法)
> 遊戲內命令列表<br>
> 3. timeSet(int) [from 0 ~ 24000]<br/>
> 4. saveMap(string) [filename]<br/>
> 5. loadMap(string) [filename]<br/>
> 6. setHP(int) [from 0 ~ 20]<br>
7. player與monster被攻擊時往後退

## Feedback
