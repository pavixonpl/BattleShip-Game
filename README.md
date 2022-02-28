# BattleShip-Game
BattleShip game created using .NET 6 and React. Frontend is very simple, because main goal of application was to create working backend. 

## Rules of the game
Each of the players has one board and 5 ships with lengths: (5, 4, 3, 3, 2). Random player starts game and shoots to another players board. There are 3 possibilites (Miss/ Hit/ Hit and sunk). The game is played in rounds, even if a player hits, it is still the next player's turn. Ships can't touch each other directly, there must be minimum one block between. Ships can't be placed diagonal. As soon as all of one player's ships have been sunk, the game ends.

## How does it work?
Backend places ships randomly, including direction and position, but still respecting rules of the game.
![shpis on board](https://user-images.githubusercontent.com/56199357/156036556-607b476f-87c0-4acc-b623-0578a177c72c.png)

After that game begins shots are random until first hit (1). After first hit, next shots are fired next to hit (2), to find next part of ship (3). If next part is hit (3), then it tries to find part of ship on this axis (4). If last hit was miss (4), then it goes to opposite direction (5), until ship is sunk (6). If all ships are sunk, game is over.

![how shoots look like](https://user-images.githubusercontent.com/56199357/156036961-3068c80a-3fe1-4c39-b35c-53e4b01f9c5e.png)

Random shooting considers all rules which human would consider, including: 
- Not shooting to fields which are directly next to sunk ships
- Not shooting to fields which were alredy shoot, including Miss/ Hit/ Hit and sunk
- Not shooting to field if there is no possibility of existing ship there, example: 
- ![notavailefield](https://user-images.githubusercontent.com/56199357/156038473-ac687589-873f-4535-ba7a-3cb285723a36.png)
- It will shoot only to fields, where ships can fit, considering which ships where previously destroyed. For example, if the only ship left has length of 5, then it will not shoot if only 3 fields are free, based on all of previous misses/ hits/ hit and sunks. For exmaple it will not shoot in place with red squre:
- ![freefields](https://user-images.githubusercontent.com/56199357/156039367-157b1dc4-3e3f-4838-bb3f-fb1b8729b1a8.png)

### Frontend

![BoardWithWin](https://user-images.githubusercontent.com/56199357/156034503-7cc891fb-3688-4bf8-aafa-7675be259747.png)

### Swagger of backend
![swaggerbattleship](https://user-images.githubusercontent.com/56199357/156035866-dbf6e567-35f6-4ecd-92ba-26b5dd6b1360.png)

Address to fronted: http://localhost:3000/
Address to backend: http://localhost:5286/
