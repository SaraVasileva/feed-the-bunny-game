# FeedTheBunny
#### Апликацијата претставува едноставна 2D игра, која се игра само со еден играч.    
                                                                                                                                                                                                                             
                                                                                                                                                                                        
#### Опис:  
Feed The Bunny е возбудлива и предизвикувачка игра каде играчот контролира зајче кое тргнува во мисија да собира моркови и зелки. Притоа освен храна за зајчето паѓаат и бомби, кои доколку ги фати, губи еден живот, а исто така паѓа и RedBull кој му дава поголема брзина на движење. 

#### Цел:     
Целта на играта е да се собере што е можно повеќе храна за зајчето,и притоа да не се загубат сите максимум животи кои ги има. Играчот треба да се движи со намера да ги фати сите моркови и зелки, одбегнувајќи ги замките.На крај на играта се појавува нов прозорец со освоените поени добиени од тековната игра.

#### Упатство за играње: 
При започнување на играта имате опција да го видите упатството со кликање на R, играње на играта со помош на копчето Enter, а за пауза на истата може да кликнете P. Зајчето на почетокот на играта има 3 животи, при фаќање на бомба, губи по 1 живот, a доколку собере 5 зелки добива повторно плус 1 живот и со 5 морковчиња има право на 1 скок со кликање на копчето Space. Еден морков носи 5 поени, зелката носи 2 поени и со RedBull добива поголема брзина. На секои соберени 250 поени се менува позадината на играње односно се преминува на следниот левел, каде храната, бомбите и RedBull-от паѓаат се побрзо и побрзо.

#### Почеток на играта:
![image](https://github.com/SaraVasileva/feed-the-bunny-game/assets/127666693/75e64832-30b1-4d9c-8461-5647c2759cb8)

#### Изглед на играта:
![image](https://github.com/SaraVasileva/feed-the-bunny-game/assets/127666693/a77067a7-7bc6-4247-a381-77d3fe2ea542)

#### Изглед на играта по завршување:
![image](https://github.com/SaraVasileva/feed-the-bunny-game/assets/127666693/97ad6d40-d717-49ac-914a-d0d34bb0ab3a)

#### Логика на кодот на играта:
Логиката на играта е имплементирана во Form1 класата, која ја претставува главната форма на играта. Еве неколку клучни карактеристики и методи во кодот:                                                                                  

-> Играта користи **timer** за ажурирање на состојбата на играта.  
                                                                                                                                                                                                                        
-> Класата **FallingObject** содржи методи за изведени класи како Carrot, Cabbage, Bomb, и Redbull и притоа секоја изведена класа претставува специфичен тип на предмет што паѓа во играта, дефинирајќи ги неговите уникатни својства и 
   однесувања. 
                                                                                                                                                                                                                        
-> Методот **InitializeGame** ја поставува почетната состојба на играта, вклучувајќи вклучувајќи го зајчето и предметите кои паѓаат.  
                                                                                                                                                                                                                      
-> Методот **StartGame** ја започнува играта со вклучување на тајмерот и ажурирање на статусот на играта.       
                                                                                                                                                                                                                              
-> Методот **PauseGame** ја паузира играта со запирање на тајмерот и прикажување порака за пауза.       
                                                                                                                                                                                                                            
-> Методот **RestartGame** ја ресетира состојбата на играта на нејзините почетни вредности.                                                                                                                                                                                                                                                                                                                                                                                       
-> Методот **SpawnObject** по случаен избор создава и додава предмети што паѓаат во играта.                          
                                                                                                                                                                                          
-> Методот **CheckCollision** проверува дали зајчето успеало да ги фати предметите што паѓаат, ажурирајќи го резултатот и отстранувајќи ги фатените/изгубените предмети. 
                                                                                                                                                                                                              
-> Методот **CheckGameOver** проверува дали играчот ја изгубил играта (нема преостанати животи) и прикажува порака за завршување на играта.  
                                                                                                                                                                                                      
-> Методот **CheckLevel** го прилагодува нивото на играта врз основа на резултатот на играчот и соодветно ја ажурира бојата на позадината.   
                                                                                                                                                                                                           
-> Методот **MoveObjects** ги ажурира позициите на сите предмети што паѓаат.  
                                                                                                                                                                                                           
-> Методот **UpdateRedbull** ја менуува брзината на движење и изгледот на зајакот.
                                                                                                                                                                                                                           
-> Методот **Form1_KeyDown** се справува со внесените команди од тастатурата на корисникот, како што е контролирање на движењето на зајакот и интеракција со играта.  
                                                                                                                                                                                                                                    
-> Методот **ProcessCmdKey** овозможува ракување со специјални копчиња како Enter, P и Escape за контрола на играта.                                                                                                                           
