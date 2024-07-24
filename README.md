
Shitaku - Prototype Game README

Team: Gabrielle Berman, Nicholas Cimino, Andrew Knee, Melissa Requeny, Javier Lopez, and Nishimi Yuki

## Project Description:

### Summary
The game will revolve around a player character who acts as an engineer being tasked with preparing a city to combat a terrible disaster that will come in a few days. The disaster will be chosen upon the game's start (tsunami will be the default) and the player would have to go around the map to adequately prepare for the oncoming disaster. They will do this by interacting with buildings and people in order to build, reinforce, and evacuate any assets that would ensure the safety and sustainability of the city. The player will have a set amount of money and actions per day before the disaster that the player must wisely use to maximize utility. Once the time is up, the disaster will happen and the score is determined based on how many lives and infrastructure were saved.

### Details
Genre: 2D Top-down, Pixel Art, City Building Game
Educational Category: Simulation and Decision Making.
Scenario: You’re the engineer assigned to prepare a city for a future natural disaster.
With a limited amount of time and possible options, you go around the city to make repairs, renovations, and reconstructions. You choose what infrastructure/buildings you want to maintain and update so that the city is better prepared for the city. Your success is determined by the score you get from rebuilding certain buildings and the residents evacuated by your actions when the disaster comes. 
Mechanics: Overworld map along with intractable building interior menus, movable player character, daily period of performing actions, limited action points requiring the player to make decisions and balance potential options, and a final score system that keeps track of decisions.
Trivia: The name ShiTaku is a play on words using the Japanese words for city (Shi) and blessing (Taku) which, when combined, make the word Shitaku which means preparedness 市沢.
Characters: Player - Engineer, NPCs - residents throughout the city (May include workers, civilians, engineers, and politicians)
Environments: Urban city overworld with unique interior building menus with custom room and npc art (fire house, hardware store, homes, skyscrapers, shrines, police station)

### Game Logic
The game is being built in Unity with assets made independently by our team alongside premade assets found within the game engine. The project leverages game objects and scenes as its main focus. We have created objects such as the player, interactables (npcs, doors, and items) that each have their own prefabs. These are pre-established assets that act as containers for information. Each game piece has a sprite render, a 2D collider, and script that give the assets some function such as a method that changes some aspect of the game object (ie direction, design, or behavior) when a function of it is called. Game controls and event listeners are handled by underlying scripts we call the managers. The managers are bound to the objects they manage (an interactivity manager handles the behavior of interactable elements). These controllers track assets of the game in runtime and call functions of independent objects when specified events happen. The game is built off a time system where the player is given a small amount of “Action Point” that the player can use for certain actions such as evacuating npc’s or rebuilding houses. The player gets 5 points per day for 4 days and can only choose one act per house and the type of building reinforced matters for points. Each act and type of building an act is done on determines the amount of action points used up and how much score is awarded. Score is determined in the background by adding every positive aspect done by the player and is tallied up when the game ends to give the player a final score. The higher the score, the better prepared your city is for the upcoming threat. Evacuations all award static scores and both cost the least and award the least. Rebuilding skyscrapers cost the most but awards the most points since their reinforcement would provide good high points for a tsunami and can house many people. Buildings such as firehouses, police stations, city hall, and hardware store care also award large points at marginal costs since they provide good city functions that are useful to have during a disaster. Alternatively, shrines award less points because what matters more for shrines is its message, not its physical structure per our KU student. 

The main scene uses a player asset with its own walk cycle, player controller, and alerts. The player is controlled using the WASD keys and has an alert that appears when near an interactable element. The ‘E’ key allows for interaction with menus and interactables. The arrow keys allow for menu movement, and the ‘Z’ key brings up the pause menu. The main scene also has a main camera that follows the player, a fader that helps aid in fading effects, a dialogue manager to show and display text, a menu manager that tracks which menu is active or not, a room canvas to display npc and unique rooms, and two managers to handle event listeners and actions.

The game interface involves several menus that aid in gameplay. The pause menu allows the user to resume the game or return to the menu. The npc menu shows up when talking in a room and allows for decision making such as rebuilding a house, evacuating npcs, talking, or exiting a menu. The end menu shows up when the days end and conclude the game and display the final score. A day menu GUI also appears throughout the game to show what day the player is on. 

The city is generated at runtime using a wave function collapse algorithm. This function makes a new map every time the game is run with different layouts, dialogue, npc’s, and names. Each type of building has a different tag that alerts to the type of building it is. It could be a firehouse, police station, city hall, hardware store, normal houses, and skyscrapers. The named buildings will appear only once per map but the building and skyscrapers can show up a myriad of times. Each building is on a tile and the remaining tiles are used as roads to traverse the map. The building tiles also have a door in their tiles that allow the user to interact with them. Once interacted with, the room is faded in and a new npc can be seen and talked to. This npc and room changes per room so each building has a different design. When a player talks to the npc, they can either rebuild a building if the building is destroyed in the main map, evacuate the npc if they are still there, or just talk with the npc.

For music, we have audio cues for the menu appearance, movement and select as well as background music for the main menu and main game. The main menu and instruction screens use ambient noise for a serene setting to indicate the calm before the storm and the main game has a faster japanese-style Lo-Fi song that plays on loop.

## Credits/References
Music
Menu Select Sound: https://freesound.org/people/broumbroum/sounds/50561/
Menu Move Sound: https://www.zapsplat.com/music/bright-button-click-3/
Menu Appear Sound: https://freesound.org/people/dland/sounds/320181/
Door Opening/Closing: https://freesound.org/people/pagancow/sounds/15419/
Ambient Main Menu Music: https://freesound.org/people/Garuda1982/sounds/737002/
Bamboo Main Menu Sounds: https://freesound.org/people/gmni/sounds/67046/
Main Game Background Music: https://freesound.org/people/Seth_Makes_Sounds/sounds/666721/
Wave Sound: https://freesound.org/people/NIkhill%20Kumar/sounds/320330/

Other:
Pixel Font: https://fonts.google.com/specimen/Pixelify+Sans?query=pixel

Wave Function Collapse: The following github repository was used for the basic version of Wave Function Collapse. https://github.com/kavinbharathii/wave-function-collapse/tree/main. Andrew converted the code from Python to C# and added additional features to make generation smoother. Additionally, he changed the algorithm to support more tile configurations than just the binary adjacency test provided by the version in the github repository.

## One-Line Build Command

From the project root directory type
Unity -quit -batchmode -nographics -executeMethod Builder.Build -projectPath ./
Where Unity is the path to your Unity editor executable on Windows. After this it will produce a folder called cli-build which contains all the executable information.

## Link to GitHub Repository

Link: https://github.com/ncimino1/ShiTaku

## Link to installation package executable
https://github.com/ncimino1/ShiTaku/releases/download/Final/ShiTakuInstaller.exe

## Run executable command
From the install or build location, type ShiTaku.exe in terminal.
