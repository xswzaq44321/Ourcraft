# OurCraft

## How to play your game
移動: WASD<br/>
跑步: double click W<br/>
跳躍: Space<br/>
移動視角: Mouse<br/>
物品欄選擇: alpha-num<br/>
取得物品: Left Click<br/>
放置物品: Right Click<br/>
開啟遊戲資訊: F1<br/>
回到選單: Esc<br/>
開啟console: Grave accent(\`)<br/>
在飛行模式下:
> 進入/離開 飛行: 雙擊Space
> 上升: Space
> 下降: Left Shift

## Environment
Windows

## ~~Your~~ game
Our game

## Game design (how to design your character, monster, etc.)
character: 就是普通的維京人，喜歡蓋房子<br/>
monster: 殭屍化的維京人，動作緩慢，很吵<br/>
遊戲中的一天(24000單位時間)為現實中的20分鐘，到晚上會生成怪物<br/>
使用console的saveMap可以儲存地圖，Ex. saveMap("myCYKA")。預設儲存位置為"我的文件/Ourcraft Maps/"<br/>
你也可以用console的loadMap來載入地圖。在使用saveMap、loadMap時若參數留白，將會使用預設檔名map1.json<br/>

## Bonus
Chen:
1. 會根據滑鼠移動多寡來移動視角，並且將其上下固定在-90度到90度之間
2. 游標固定在螢幕中間以方便操作
3. 放置方塊時排列整齊，且不會重疊
4. 放置方塊時玩家需在放置位置一定距離內
5. 在螢幕左上顯示遊戲資訊
6. 降雨系統(陰天、降雨、放晴、降雨機率)
7. 怪物音效(雙聲道、根據距離調整音量)
8. 怪物視野，進入怪物視野會被追逐
9. 按兩次W能跑(腳步聲也會變快)
10. 鏡像世界
11. 飛行模式

Weber
1. 遊戲地圖的存檔，預設存在"myDocument/Ourcraft Maps/"
3. Lua Console!!! (可以使用原生lua語法)
> 遊戲內命令列表，以下指令都有防呆(印訊息)，以免使用者輸入錯誤<br/>
> 3. setTime(int) [from 0 ~ 24000]<br/>
> 4. saveMap(string) [filename]<br/>
> 5. loadMap(string) [filename]<br/>
> 6. setHP(int) [from 0 ~ 20]<br/>
> 7. setWalkSpeed(float) [from 0 ~ inf]<br/>
> 8. setRunSpeed(float) [from 0 ~ inf]<br/>
> 9. setTimeSpeed(float) [from 0 ~ 1000]<br/>
> 10. addItem(string, int) [item name, from 0 ~ 9999]<br/>
> 11. infinityItem(boolean)<br/>
> 12. setHealSpeed(float) [from 0 ~ inf]<br/>
> 13. rain(float, uing) [from 100 ~ 23799, from 10 ~ 40]<br/>
> 14. setRainingRate(float) [from 0 ~ 1]<br/>
> 15. getRainInfo()<br/>
> 16. setAtkRange(float) [from 0 ~ inf]<br/>
> 17. setTouchDistance(float) [from 0 ~ inf]<br/>
> 18. enableFly(boolean)<br/>
> 
> 操作console<br/>
> 19. 可以看訊息，且訊息過多可以滾動<br/>
> 20. 可以按上下按鍵(arrow key)來存取之前打的指令<br/>
21. player與monster被攻擊時往後退

## Feedback
作業跟project一樣麻煩，占分比又比project低很多，又占用做project的時間
