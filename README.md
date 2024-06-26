
Shitaku - Prototype Game README

Team: Gabrielle Berman, Nicholas Cimino, Andrew Knee, Melissa Requeny, Javier Lopez, and Nishimi Yuki

##Project Description:

###Summary
The game will revolve around a player character who acts as an engineer being tasked with preparing a city to combat a terrible disaster that will come in a few days. The disaster will be chosen upon the game's start (tsunami will be the default) and the player would have to go around the map to adequately prepare for the oncoming disaster. They will do this by interacting with buildings and people in order to build, reinforce, and evacuate any assets that would ensure the safety and sustainability of the city. The player will have a set amount of money and actions per day before the disaster that the player must wisely use to maximize utility. Once the time is up, the disaster will happen and the score is determined based on how many lives and infrastructure were saved.

###Details
Genre: 2D Top-down, Pixel Art, City Building Game
Educational Category: Simulation and Decision Making.
Scenario: You’re the engineer assigned to prepare a city for a future natural disaster.
With a limited amount of time and possible options, you go around the city to make repairs, renovations, and reconstructions. You choose what infrastructure and housing you want to maintain and update so that the city is better prepared for the city. Your success is determined by whether the residents were aided by your actions when the disaster comes. 
Mechanics: Overworld map along with building interior maps, movable player character, weekly period of performing actions, limited action points requiring the player to make decisions and balance potential options, final score system that keeps track of decisions.
Risks: Ambitious requirements for assets and possible actions to perform for the city, limited time to implement all aspects of original idea, limited initial knowledge of tools.
Trivia: The name ShiTaku is a play on words using the Japanese words for city (Shi) and blessing (Taku) which, when combined, make the word Shitaku which means preparedness 市沢.
Characters: Player - Engineer, NPCs - residents throughout the city (May include workers, civilians, engineers, and politicians)
Environments: Urban city overworld (with both day and night time settings), Interior urban building maps with a menu design for interaction (Streets, alleys, interiors, transit lines, parking lots)

###Game Logic
The game is being built in Unity with assets made independently by our team alongside premade assets found within the game engine. The project leverages game objects and scenes as its main focus. We have created objects such as the player, interactables (npcs, doors, and items) that each have their own prefabs. These are pre-established assets that act as containers for information. Each game piece has a sprite render, a 2D collider, and script that give the assets some function such as a method that changes some aspect of the game object (ie direction, design, or behavior) when a function of it is called. Game controls and event listeners are handled by underlying scripts we call the managers. The managers are bound to the objects they manage (an interactivity manager handles the behavior of interactable elements). These controllers track assets of the game in runtime and call functions of independent objects when specified events happen. 

Of the major game assets, we have:
-A camera that follows a player
-A map manager that auto generates a map of a specified size in runtime with different tiles which each have a different property (road, house, door, NPC, etc)
-A player that spawns in the map and is controlled using the WASD keys, door objects that detect a nearby player and allow them to change the scene (change their location) when they press the action key ‘E’
-NPC objects that detect nearby players and allow them to spawn in a text box when interacted with consisting of pre-set dialogue that can be progressed and read, menus which spawn in when certain interactables are called
-A time system that counts down after certain interactable elements are called, and a dialogue manager that allows for text boxes to appear on screen when needed
-A menu that appears when pressing ‘Q’ that is navigable using the arrow keys.
-Day counter that indicates how much time is left in the game, when action points (an in-game counter) is used up, the day progresses. After all days end, the game ends
-Menu actions that allow for selectable player functions such as “interact”

These come together to form the key components of our game that this prototypes aims to exemplify.

##One-Line Build Command

From the project root directory type

Unity -quit -batchmode -nographics -executeMethod Builder.Build -projectPath ./

Where Unity is the path to your Unity editor executable on Windows. After this it will produce a folder called cli-build which contains all the executable information.

##Link to GitHub Repository

Link: https://github.com/ncimino1/ShiTaku

##Link to installation package executable
https://github.com/ncimino1/ShiTaku/releases/download/Main/ShiTakuInstaller.exe

##Run executable command
From the install or build location, type ShiTaku.exe in terminal.
