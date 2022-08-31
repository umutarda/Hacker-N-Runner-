# Hacker-N-Runner-
This is a prototype of game called "Hacker N Runner"

In this prototype, the player is a yellow cube which is automatically rotated by the related script using developer tuned animation data. 
When the player pushes the screen, rotating stage stops and the hook is thrown in the direction of cube.
If cube hits a hack object, then this object's hack stage begins. 
In both cases player's hack is success or not, the hook is brought back.

![image](https://user-images.githubusercontent.com/52600067/187771779-1e7174b8-c286-4206-a3c5-0f71a884f7bf.png)

![image](https://user-images.githubusercontent.com/52600067/187772011-f9b71f0f-d354-41b6-aa97-84cc992cb296.png)

Holder.cs script is attached to the yellow cube corresponding player. 
Holder.cs script is referencing to 2 instances of ObjectAnim class. ObjectAnim class provides an easy way to create animations in this prototype. 
The class takes MinToMaxDuration (int), MaxValue (int) and Curve (Animation Curve) as instance variables. 
Holder is meant to be used as player controller, it has an internal state machine to control left-right rotation of "holder" of the hook and throw to and bring back from 
the hook in desired direction.


HackObject class is an abstract class which is applicable to any hackable object. It was thought that any hackable object must undergo hack stage that is reachable
only when being catched by player's hook. 

![image](https://user-images.githubusercontent.com/52600067/187773930-2a825523-8515-4c30-b25b-3203661e25ce.png)

EnemyGun class is a representative of hack object. EnemyGun is a gun carried by cars. The EnemyGun hack game was thought to be winnable by doing prompted movements
in the right way. For example 0th movement "right" means drawing a line whose slope is 0 degree +- AngleErrorMargin,
1st movement "left" means drawing a line whose slope is 180 degrees +- AngleErrorMargin, and so on.

https://user-images.githubusercontent.com/52600067/187777695-ed7eb59d-5f1c-45ed-a4c6-4b3318e3ffca.mp4

