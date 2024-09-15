# SpriteX Framework

This is a 2D Game Framework. Go to [Releases](https://github.com/AAli107/SpriteX-Framework/releases) and select the latest release to download. Once the download is complete, extract the zip file then read the documentation below if you're new to SpriteX Framework.

# Documentation
## Doc Index
- [Requirements](#requirements)
	- [Recommended Hardware Requirements](#recommended-hardware-requirements)
	- [Recommended Software Requirements](#recommended-software-requirements)
	- [Recommended User Knowledge](#recommended-user-knowledge)
- [Game Code Basics](#game-code-basics)
	- [Navigating to the SampleLevel](#navigating-to-the-samplelevel)
	- [A Look Around Inside SampleLevel](#a-look-around-inside-samplelevel)
	- [How to take in Player Input](#how-to-take-in-player-input)
	- [How to Create a new Level Script and switch to it](#how-to-create-a-new-level-script-and-switch-to-it)
- [Rendering Graphics](#rendering-graphics)
- [Camera](#camera)
- [InitialGameWindowConfig](#initialgamewindowconfig)
- [Game Objects](#game-objects)
	- [Spawn a Game Object](#spawn-a-game-object)
	- [Removing a Game Object](#removing-a-game-object)
	- [Getting a Game Object / Check if Game Object Exists](#getting-a-game-object--check-if-game-object-exists)
- [Components](#components)
	- [ButtonComponent](#buttoncomponent)
	- [ColliderComponent](#collidercomponent)
	- [PhysicsComponent](#physicscomponent)
	- [RenderComponent](#rendercomponent)
	- [ScriptComponent](#scriptcomponent)
- [Character Base](#character-base)
	- [Side Scroller Character](#side-scroller-character)
	- [Top-Down Character](#top-down-character)
- [Utilities](#utilities)
	- [Numbers](#numbers)
	- [Colors](#colors)
	- [Enums](#enums)
	- [Rand](#rand)
	- [Vec2D](#vec2d)
	- [Vec3D](#vec3d)
- [MainWindow](#mainwindow)


## Requirements

### Recommended Hardware Requirements
- CPU: Processor with 2 GHz dual-core
- Memory: 2 GB
- GPU: Any modern GPU
- Storage: The zip download is less than 1 megabyte, which means you shouldn't worry about storage much.

### Recommended Software Requirements
- Windows 10 Operating System or above.
- Microsoft Visual Studio with C# [(You can download it here)](https://visualstudio.microsoft.com/vs/community/)

### Recommended User Knowledge
- Basic C# code knowledge [(You can watch Brackeys' C# Basics Tutorial Series to get you mostly caught up)](https://www.youtube.com/playlist?list=PLPV2KyIb3jR4CtEelGPsmPzlvP7ISPYzR)
- Experience with playing Games (not necessary but recommended)

**Note:** You shouldn’t worry too much about the requirements since most of the requirements like hardware and software you probably have already met.

## Game Code Basics

### Navigating to the SampleLevel
To code your first game, you need to open “SpriteX Framework.sln” using Visual Studio. From the Solution Explorer, choose “SampleLevel.cs” which is the default level you start out with.

### A Look Around Inside SampleLevel
When Running the game, it will load “SampleLevel” level as the starting level. When SampleLevel is loaded in-game, you'll see a black window displaying a "Hello World!" message.

This is how SampleLevel's code should look like:
```c#
using OpenTK.Mathematics;
using SpriteX_Framework.FrameworkContents;

namespace SpriteX_Framework
{
    public class SampleLevel : GameLevelScript
    {
        // LevelStart is called when the level begins
        public override void LevelStart(MainWindow win)
        {

        }

        // GameUpdate is called once per frame as long as the game is not paused
        public override void GameUpdate(MainWindow win)
        {

        }

        // GraphicsUpdate is called once per frame to render graphics
        public override void GraphicsUpdate(MainWindow win, gfx gfx)
        {
            gfx.DrawText(new Vector2d(100, 100), "Hello World!", Color4.White);
        }
    }
}
```
As shown above, SampleLevel has three main parts, “LevelStart”, “GameUpdate”, and “GraphicsUpdate”.

### How to take in Player Input
Games without inputs aren't games! So in order to let the game know the player is pushing a button on the keyboard, we could write either of these methods below within if statements inside the `GameUpdate` method. Each one requires a parameter of type `OpenTK.Windowing.GraphicsLibraryFramework.Keys` to specify which key is pressed.
- **win.IsKeyDown():** Returns a `bool` that is true when the specified key is being held down by the player.
- **win.IsKeyPressed():** Returns a `bool` that is true when the specified key is pressed in the current frame, but is released in the previous frame.
- **win.IsKeyReleased():** Returns a `bool` that is true when the specified key is not being held by the player in the current frame, but is pressed by the player in the previous frame.

### How to Create a new Level Script and switch to it
SpriteX Frameworks comes with a SampleLevel script. SampleLevel is a subclass of `GameLevelScript`. All levels you want to make must extend from `GameLevelScript` class in order for it be counted as a "Level". So what you have to do is create a new class, and call it whatever you want. For this example, It's called "Level2", and it must extend from `GameLevelScript`. Here is how it should look like:
```c#
public class Level2 : GameLevelScript
{
	// TODO
}
```
Once created, you can now type `public override` and the Visual Studio should display all the overridable methods.

- **Awake:** This method gets executed before the level is loaded.
- **LevelStart:** This method gets executed after the level is loaded. You can use this to instantiate all your variables and set their values accordingly, play sound, add game objects, etc.
- **GameUpdate:** This method gets executed once per frame as long as the game is not paused. This is where you put all your game code in. Like Player input, etc.
- **GraphicsUpdate:** This method is called once per frame to render graphics. All graphics rendering you intend to add must be placed within this method.
- **PrePhysicsUpdate:** This method gets executed before Physics and collision updates for fixed amount of times per second.
- **FixedUpdate:** This method gets executed for fixed amount of times per second. (default: 60 per second)
- **GameUpdateNoPause:** This method gets executed once per frame (cannot be paused; executed right after GameUpdate)
- **LevelEnd:** This method gets executed right before the currently running level gets switched to another level.
- **GameEnd:** This method gets executed when the game is about to close, while this level is running.

Now that we made a new Level for the game, we can now load it from the default SampleLevel. To do that, we first have to insert an if statement with `win.IsKeyPressed(Keys.L)` as the condition inside the SampleLevel. This means that if the player has pressed the "L" key within the sample level, the level is switched to the `Level2` we made earlier. In order to switch to the level want, we have to use `win.LoadLevel()` method within the if statement. `LoadLevel()` requires a parameter of type `GameLevelScript`, which means it requires you to insert your level. This is how the `GameUpdate` method within the Sample level should look once you finish:
```c#
public override void GameUpdate(MainWindow win)
{
    win.LoadLevel(new Level2());
}
```
## Rendering Graphics
As mentioned earlier, the `GraphicsUpdate` method will allow you to render visuals/shapes on the screen. All the built-in graphics shapes can be accessed within the `gfx` class. You can access it through `GraphicsUpdate` as a method parameter.

There are a lot of methods within `gfx` you can use to render stuff on the screen, so here are all the accessible gfx methods that is used to render shapes and images:
- `void DrawPixel(double x, double y, Color4 color)`: Draws a single pixel on the game window (exact pixel position)
- `void DrawPixel(Vector2d position, Color4 color)`: Draws a single pixel on the game window (exact pixel position)
- `void DrawPixel(float[] data, Color4 color)`: Draws a single pixel on the game window based on given data
- `void DrawPixels(Vector2d[] position, Color4 color)`: Draws many pixels on the game window (exact pixel position)
- `void DrawPixels(float[] vertexData, Color4 color)`: Draws many pixels on the game window using given vertex data
- `void DrawScaledPixel(Vector2d position, Color4 color, bool isStatic = false)`: Draws a single pixel that scales with Game Window
- `void DrawScaledPixels(Vector2d[] position, Color4 color, bool isStatic = false)`: Draws many pixels that scales with Game Window
- `void DrawScaledPixel(double x, double y, Color4 color, bool isStatic = false)`: Draws a single pixel that scales with Game Window
- `void DrawTri(Vector2d a, Vector2d b, Vector2d c, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)`: Draws a triangle on the game window
- `void DrawTris(Vector2d[] vertexPos, Color4 color, bool isStatic = false)`: Draws many triangles on the game window
- `void DrawQuad(Vector2d a, Vector2d b, Vector2d c, Vector2d d, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)`: Draws a Quad on the game window
- `void DrawTexturedQuad(Vector2d a, Vector2d b, Vector2d c, Vector2d d, Texture texture, Color4 color, bool isStatic = false)`: Draws a Quad with texture
- `void DrawTexturedQuad(Vector2d a, Vector2d b, Vector2d c, Vector2d d, Texture texture, bool isStatic = false)`: Draws a Quad with texture
- `void DrawRect(Vector2d pos, Vector2d dimension, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)`: Draws a simple rectangle on the game window
- `void DrawImage(Vector2d pos, Vector2d dimension, Texture texture, Color4 color, bool isStatic = false)`: Draws an image texture on screen
- `void DrawImage(Vector2d pos, Vector2d dimension, Texture texture, bool isStatic = false)`: Draws an image texture on screen
- `void DrawLine(Vector2d a, Vector2d b, Color4 color, double width = 1, bool isStatic = false)`: Draws a line between two points on the game window
- `void DrawSingleChar(Vector2d pos, char character, Color4 color, float size = 1, bool isStatic = true)`: Draws a Single character on screen
- `DrawText(Vector2d pos, object _object, Color4 color, float size = 1, bool isStatic = true)`: Draws Text on screen
- `DrawText(Vector2d pos, string text, Color4 color, float size = 1, bool isStatic = true)`: Draws Text on screen
- `void DrawShape(Shape shape, Vector2d[] pos, Color4 color, Texture texture, DrawType drawType = DrawType.Filled, float size = 1, bool isStatic = false)`: Allows you to draw shapes depending on the `shape` parameter you give it

The `isStatic` boolean parameter that each method has controls whether the rendered graphic is fixed to the camera view or not.

## Camera
Whenever you load a Level, a new Camera is created which is positioned at XY = (0, 0). This camera acts like a way to view the Level. To access the camera that is currently being used to view the level from `GameUpdate` for example, you either do this `win.GetWorldCamera()` or `win.world.cam`.
The Camera has many methods within it, which are:
- `void MoveCamera(Vector2d offset)`: Offsets the camera location by the offset vector given.
- `void MoveCameraBySpeed(Vector2d dir)`: Offsets the camera location based on the dir(normalized vector) by the camera's speed.
- `void SetCameraPosition(Vector2d newPos)`: Sets the camera's position to whatever newPos is at
- `bool SetCameraBound(Vector2d start, Vector2d width, bool enableBounds = false)`: Gives Camera a Rectangle Bound where it cannot leave from
- `void SetEnableCameraBound(bool isCameraBound)`: Controls whether the camera is bound or not

Setting up the bounds of the camera and inserting true to the `SetEnableCameraBound()` method will cause camera to be locked within the bounds you set up, which means that it cannot leave the bounds that you gave it.

## InitialGameWindowConfig
Within the FrameworksContent folder, there's `InitialGameWindowConfig.cs` file. It contains the initial values the game starts up with. These values within this class cannot be changed in runtime as they are only read at the start when the game is run. If you want to change some of these values in runtime, you can access them using the `MainWindow win` parameter (similar names) you get within all the overridable methods within each level as the ones in `InitialGameWindowConfig` are just for setting them up initially when the game starts.
 
Here are all the values and their descriptions:
- `string Title`: This is where you type in what the window title would be.
- `Vector2i ClientSize`: This is the window resolution when not in fullscreen.
- `WindowBorder WindowBorder`: Controls the game window's border type. [Resizable, Fixed, Hidden]
- `WindowState WindowState`: Controls the window state. [Normal, Minimized, Maximized, Fullscreen]
- `double UpdateFrequency`: Controls the maximum FPS. [Set to `0` if you want to unlock FPS]
- `double fixedFrameTime`: Controls how many times per second the `FixedUpdate` and game physics updates.
- `VSyncMode VSync`: Controls the initial window's Vertical Sync. [Off, On, Adaptive]
- `Color bgColor`: Controls the window's background color. [Defaults to black]
- `bool allowAltEnter`: Controls whether the player can toggle fullscreen by pressing Alt+Enter.
- `bool showDebugHitbox`: Will show an outline of all the colliders within the game.
- `bool showStats`: Controls whether you want to display the FPS and milliseconds per frame.
- `Font font`: Contains the game's font.
- `GameLevelScript startLevel`: Contains the level that will be loaded first when the game runs.

## Game Objects
Game Objects are objects you create and insert into the game world. By themselves, they don't do anything at all. Components can be added to Game Objects to make them do things like have physics, collision, etc.

### Spawn a Game Object
To add a Game Object into the world, you must do the following:
```c#
public override void LevelStart(MainWindow win)
{
    GameObject gameObject = win.world.SpawnEmptyGameObject();
}
```
Likewise, you can also spawn a game object from a `GameObject` that has been instantiated beforehand but not spawned yet.
```c#
GameObject gameObject = new GameObject();

public override void LevelStart(MainWindow win)
{
	// Returns the Game Object's UUID
    string objUuid = win.world.SpawnGameObject(gameObject);
}
```
Within the brackets of these methods, you can also specify the position it spawns in. (defaults to XY = (0, 0) when empty)

### Removing a Game Object
To remove a Game Object from the world, you must do either of the following:
```c#
public override void LevelStart(MainWindow win)
{
    win.world.RemoveGameObject(gameObject); // Takes in the GameObject directly
    win.world.RemoveGameObject(uuid); // Takes in the uuid of the Game Object
}
```

### Getting a Game Object / Check if Game Object Exists
If you have a game object in-game and you don't have its reference while only having its uuid string, you can do the following:
```c#
public override void LevelStart(MainWindow win)
{
	// will return null if it didn't find any game object in the world
    GameObject gameObject = win.world.GetGameObject(uuid);
}
```
Now to see whether a game object with a specific uuid string exists, you have to do the following:
```c#
public override void LevelStart(MainWindow win)
{
    bool exists = win.world.DoesGameObjectExist(uuid);
}
```
You can also check if it exists if you have the GameObject reference!
```c#
public override void LevelStart(MainWindow win)
{
    bool exists = win.world.DoesGameObjectExist(gameObject);
}
```

## Components
Components can be attached to Game Objects to add features and behaviors onto the game object.
- To add a component onto a GameObject, you have to do `gameObject.AddComponent<ComponentType>();`.
- To get a component that's already in the GameObject, you have to do `gameObject.GetComponent<ComponentType>();`. Note that if the component type specified doesn't exist, it will return null instead.
- There's also `gameObject.GetComponents<ComponentType>();` that returns an array of all the components that fall within the same specified component type.
- `gameObject.GetAllComponents()` will straight up return a copy of the component list as an array.
- To remove a component, you have to do `gameObject.AddComponent(component);`.

### ButtonComponent
Adding this Component onto your Game Object will make your game object be a button you can click with a mouse.
Button components have these attributes:
- `Transform transform`: Contains the position (offsets from parent game object) and the scale of the button.
- `Box2d buttonRect`: Returns the dimensions of the button.
- `Color4 currentColor`: Returns the current color of the button.
- `Color4 NormalColor`: Contains the color of the button when it is neither pressed or hovered on.
- `Color4 HoverColor`: Contains the color of the button when it is being hovered on by the player's mouse cursor.
- `Color4 PressColor`: Contains the color of the button when it is being pressed by the player.
- `Texture Texture`: Contains the texture of the button.
- `bool IsHovered`: Returns whether the player cursor is hovering over the button or not.
- `bool IsPressed`: Returns whether the player is currently pressing the button or not.
- `EventHandler<MouseButtonEventArgs> OnButtonPressed` Event can be used to execute code when the button is pressed.

### ColliderComponent
Adding this Component onto your Game Object will make your game object have a box collider.

Collider components have these attributes:
- `Transform transform`: Contains the position (offsets from parent game object) and the scale of the box collider.
- `bool isSolidCollision`: Controls whether it blocks any other game objects with solid colliders from going through it.
- `float friction`: It is the friction between colliders [0-1]. Lower values will cause ice-like friction.

Collider components also have these methods:
- `void SetRelativePosition(Vector2 pos)`: Sets the collider's relative position to its parent game object
- `void SetRelativeScale(Vector2 scale)`: Sets the collider's scale
- `Box2d GetHitbox()`: Returns the collider dimensions
- `bool IsIntersectingWith(ColliderComponent cc)`: Returns true when Collider intersects with another one
- `bool IsOverlapping()`: Will return true if this collider is overlapping with another collider
- `bool IsOverlapping(ColliderComponent[] ignoredColliders)`: Will return true if this collider is overlapping with another collider ignoring the array of ignored colliders
- `Vector2d GetHalfSize()`: Returns Half-size of the collider
---

### PhysicsComponent
Adding this Component onto your Game Object will apply physics behaviors to your game object.

Physics components have these attributes:
- `Vector2d velocity`: velocity of parent game object
- `float mass`: The mass of the parent game object
- `float friction`: movement friction that slows down parent game object the higher the value
- `bool isAirborne`: controls the behavior of the parent game object friction so that it behaves airborne when true
- `Constraint2D movementConstraint`: controls which axis (X,Y) cannot move
- `bool gravityEnabled`: Controls whether parent game object has gravity or not
- `Vector2d gravityVector`: controls the direction of gravity
- `float gravityMultiplier`: controls the strength of gravity affecting the parent game object

components also have these methods:
- `void OverrideVelocity(Vector2d velocity)`: Will override the current Velocity
- `void AddVelocity(Vector2d velocity)`: Adds velocity into current Velocity
---

### RenderComponent
Adding this Component onto your Game Object will allow you render game objects based on what values you give it.

Render components have these attributes:
- `bool isVisible`: Controls whether to render the graphic or not.
- `Vector2d[] vertex`: Vertex data the RenderComponent will use to render the shape/graphic.
- `gfx.Shape shape`: The shape type it would want to draw.
- `gfx.DrawType drawType`: Used by some shapes to determine if the shape is filled or just outline.
- `Texture tex`: Used by some shapes to determine the texture it will be using.
- `Color4 color`: The color of the graphic/shape to be drawn.
- `float size`: Used by the Line shape to determine its thickness.
---

### ScriptComponent
It is an abstract Component that will allow you to make your own Script class that is a sub class of the `ScriptComponent`.
To create your own ScriptComponent, you have to create a new class and call it whatever you like. (we'll call it TeleportScript for now) Then make it extend from `ScriptComponent`. 
Then you have to create this constructor:
```c#
public TeleportScript(GameObject parent) : base(parent) {}
```

After you do all these steps, it should look like this:
```c#
public class TeleportScript : ScriptComponent
{
    public TeleportScript(GameObject parent) : base(parent) {}

}
```
From here you can type `public override` and it will show you the methods you can override from.
Here are the one you should override from:
- **Awake:** Executed at script base constructor.
- **Start:** Executed after script component gets added to game object.
- **Update:** Executed every frame.
- **FixedUpdate:** Executed fixed amount of times per second.

Now if you attach this TeleportScript onto a game object, it will be executed accordingly. If you are familiar with Unity3D, this is similar to the `MonoBehavior` scripts Unity has.

```c#
public class TeleportScript : ScriptComponent
{
    public TeleportScript(GameObject parent) : base(parent) { }
    
    // Start is called when the script is added to the game object
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update(MainWindow win)
    {
		// Insert Teleportation Code
    }
}
```

## Character Base
`CharacterBase` is an abstract class that extends `GameObject` that helps in creating "Characters", which means it can be used to create NPCs and the player.

Here are all its attributes:
- `bool Invincibility`: Determines whether the character is immune to damage or not
- `bool IsDead`: Returns true when hitPoints is 0, aka when the character dies
- `bool IsInvulnerable`: Returns whether the character is currently invulnerable to attacks determined by iframes and Invincibility
- `Vector2d ForwardDirection`: Returns the Forward Direction of the Character
- `Vector2d RightDirection`: Returns the Right Direction of the Character

Here are all its methods:
- `void Respawn()`: Respawns the character by moving it to its spawnpoint and refilling the hitpoints to full
- `void Kill()`: Kills the character even when invulnerable to damage
- `void TurnLook(double turnAmount)`: Turns the character's look rotation by an amount
- `virtual void DeathSequence(DamageType damageType)`: Executed when the character dies / hitpoints reaches 0
- `void SetHP(float hp)`: Sets the current HP of the character
- `void SetMaxHP(float maxHP)`: Sets the maximum HP of the character
- `void SetSpawnpoint(Vector2 spawnpoint, bool willRespawn = false)`: Sets where the character will respawn by changing its spawnpoint
- `void SetLookRotation(float lookRotation)`: Sets the character's look rotation
- `float GetHP()`: Returns current HP of the character
- `float GetMaxHP()`: returns maximum HP of the character
- `Vector2d GetSpawnpoint()`: returns character spawnpoint
- `double GetLookRotation()`: returns character's look rotation
- `bool DealDamage(float amount, uint iframes = 60, DamageType damageType = DamageType.Generic)`: Deducts Character's hitpoints
- `bool Heal(float amount)`: Increases Character's hitpoints
---
### Side Scroller Character
`SideScrollerCharacter` is a class that extends `CharacterBase` that helps in creating Characters that are for side-scroller type games. Of course it inherits all the methods and attributes from `CharacterBase`.

Here are all its attributes:
- `bool IsGrounded`: Returns true if character is on the ground

Here are all its methods:
- `void SetGravityVector(Vector2 gravityVector)`: Sets which direction the character falls towards
- `Vector2 GetGravityVector()`: Returns the direction at which the character falls towards
- `void Jump(float jumpMultiplier = 1f, bool requireGrounded = true)`: Will cause the character to jump
- `void SetJumpStrength(float jumpStrength)`: Sets the character's jump strength
- `float GetJumpStrength()`: Returns the character's jump strength
---
### Top-Down Character
`TopDownCharacter` is a class that extends `CharacterBase` that helps in creating Characters that are for top-down type games. Of course it inherits all the methods and attributes from `CharacterBase`.

Here are all its methods:
- `void MoveForward(float speed = 1f)`: Causes the Character to move forward.
- `void StrafeRight(float speed = 1f)`: Causes the Character to strafe right. (negative speed will make it strafe left)
- `void SetSimulatePhysics(bool simulatePhysics)`: Allows you to set whether to enable or disable character physics.
- `IsSimulatingPhysics()`: Returns whether the character is simulating physics or not.

## Utilities
Utilities is a namespace that has static classes that contain useful code and functions.


### Numbers
- `double Distance1D(double num1, double num2)`: calculates the distance between two single numbers
- `double AverageNum(double[] arr)`: Will return the average of numbers inside an array
- `double DegreeToRad(double degree)`: Converts degrees to radians
- `double RadToDegree(double radian)`: Converts radian to degrees
- `float ClampN(float val, float min, float max)`: Fixes the value number between the minimum and maximum
- `double ClampND(double val, double min, double max)`: Fixes the value number between the minimum and maximum in double
- `double MaxVal(double[] arr)`: Returns the largest number in an array
- `double MinVal(double[] arr)`: Returns the smallest number in an array
- `float Lerp(float a, float b, float alpha)`: Lerps two floats based on "alpha"
- `double LerpD(double a, double b, double alpha)`: Lerps two doubles based on "alpha"
---
### Colors
- `Color4 Lerp(Color4 a, Color4 b, float alpha)`: Lerps two Colors based on "alpha"
- `Color4 Multiply(Color4 a, Color4 b)`: Multiplies two Colors
- `Color4 Divide(Color4 a, Color4 b)`: Divides two Colors
- `Color4 Add(Color4 a, Color4 b)`: Adds two Colors
- `Color4 Subtract(Color4 a, Color4 b)`: Subtracts two Colors
- `Color4 Invert(Color4 c, bool invertAlpha = false)`: Returns an inverted color
---
### Enums
- `DamageType`: Enumeration for most common damage types [None, Generic, Piercing, Falling, Burning, Magic, ...]
- `CardinalDirection`: Enumeration for the directions in 2D space [North, NorthEast, East, SouthEast, ...]
---
### Rand
- `int RangeInt(int min, int max)`: Returns a random int in range (inclusive)
- `float RangeFloat(float min, float max)`: Returns a random float in range
- `double RangeDouble(double min, double max)`: Returns random double in range
- `bool RandBool()`: Returns a random bool, true or false
- `bool RandBoolByChance(double chance = 0.5)`: returns true randomly based on the decimal chance it would do it
---
### Vec2D
- `Vector2d Midpoint2D(Vector2d point1, Vector2d point2)`: Returns the center between two 2D vectors
- `Vector2d RotateAroundPoint(Vector2d pointToRotate, Vector2d centerPoint, double degreesAngle)`: Returns the position of a point after being rotated by a given degrees around a given point
- `DirectionToVec2D(Enums.CardinalDirection cardinalDirection)`: Converts the Cardinal Directions into a normalized vector 2D
- `Lerp(Vector2d a, Vector2d b, double alpha)`: Lerps two Vector2s based on "alpha"
---
### Vec3D
- `double Distance3D(Vector3d point1, Vector3d point2)`: Calculates the 3D distance between two points/vectors
- `Vector3d Midpoint3D(Vector3d point1, Vector3d point2)`: Returns the center between two 3D vectors
- `Vector2d Vec3DToVec2D(Vector3d vec3D, double depth = 100)`: Converts 3D Vectors into 2D vectors with depth (Can be used to render simple 3D graphics)
- ` Vector3d Lerp(Vector3d a, Vector3d b, double alpha)`: Lerps two Vector3s based on "alpha"

## MainWindow
`MainWindow` is the single most important class that extends OpenTK's `GameWindow` that does almost everything including the game loop.

Here are all of the public attributes it has (not including stuff it inherited from `GameWindow`):
- `bool isGamePaused`: Controls whether the game is paused or not
- `double FPS`: Returns the Window's current FPS
- `double fixedFrameTime`: controls how many times FixedUpdate method is executed per second
- `bool showDebugHitbox`: controls whether to render the GameObject hitboxes
- `bool showStats`: Displays Stats showing FPS and Update time(ms) when true
- `double time`: Hold time in seconds since Window was created
- `Color4 bgColor`: Controls the background Color of the window
- `Font font`: Stores the Game's Font
- `World world`: Stores the game world
- `Vector2d mouseScreenPos`: The position of the mouse in the window
- `Vector2d mouseWorldPos`: The position of the mouse within the world

Here are all of the public methods it has (not including stuff it inherited from `GameWindow`):
- `void SetGameFont(Font font)`: Sets the Game's font
- `Camera GetWorldCamera()`: Returns the camera the world is using
- `void SetWorldCamera(Camera cam)`: Switches the world's Camera
- `void PlayAudio(string path, float volume = 1f)`: Plays Audio from file, volume ranges from 0-1
- `void LoadLevel(GameLevelScript level, Camera cam)`: Will Load level
- `void LoadLevel(GameLevelScript level)`: Will Load Level
