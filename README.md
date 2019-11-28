### Cloud Based Claim Management Solution


![image](https://user-images.githubusercontent.com/1701237/69831007-c2d53a80-1227-11ea-88b9-6b1576583082.png)

* asp.net core 2.2

* azure event grid

* azure functions (serverless)

* azure sql server

* azure key vault

* azure cosmos db


### Additional Key Points:

. Tried to follow SOLID and Clean Code guidelines

. Separations of concerns

. Implemented mediator pattern using MediatR library

. There are separate


<b>Commands</b> 
```diff 
- for write 
```
<b>Queries</b>
```diff 
+ for read 
```
<b>Events</b>
```diff 
! to notify other system components if something interesting related to them happens
```
