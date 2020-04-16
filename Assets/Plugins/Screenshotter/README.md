# Screenshotter

![Screenshotter Logo](http://www.skatanicstudios.co.uk/wp-content/uploads/2020/04/logo.jpg)

#### Package for taking gameplay screenshots in the Unity Editor across all three pipeline types.

Screenshotter is a simple Camera controller plug-in for Unity that is designed to make the process of gameplay screenshots fast, easy and flexible! It allows you to fly around your scene with a game-pad to line-up a shot and adjust a Depth of Field post-process on the camera without interacting with controls in the editor.

**Features**:
  * Camera Controller using a Gamepad
  * Depth of Field Controls
  * One Click Screenshot Feature
  * Cross-Compatible with Built-In/URP/HDRP Renderers

## Installation

**Walkthrough Video**

[![Walkthrough Video](https://img.youtube.com/vi/frMOMNGxbN0/0.jpg)](https://www.youtube.com/watch?v=frMOMNGxbN0)

Simply open the package manager in Unity, choose the (+) sign and choose "Add Package From Git URL" and enter the url https://github.com/neon8100/screenshotter.git

Alternatively, download the Clone the project and import using the "Add Package From Disk" option. 

**Once the Package is Downloaded** right click in the hierarchy and choose "Screenshotter Camera". This will add a new Screenshotter Camera GameObject to scene and Screenshotter should detect your current render pipeline and adapt its settings/components accordingly. 

With your Gamepad active hit play. You should now be able to fly around your scene using the Screenshotter Camera.

## Controls
The screenshotter camera is designed primarily to work with an Xbox controller, but it should detect and use any Unity Supported Gamepad. The system uses the Player Input package to map its controls and send messages to the system, so feel free to adjust/remap them.

**Default Controls**

*General Controls*
 - View/Back : Take Screenshot 
 - Options/Start : Pause/Resume TimeScale
 - A: Change Mode
 - Y: Toggle Debug Text
 - L3 (Click Left Stick In): Toggle Speed 
 - Right Trigger: Zoom In
 - Left Trigger: Zoom Out
 - Right Bumper: Camera Up
 - Left Bumper: Camera Down
 
 
*Move Mode*
 - Left Stick  - Move
 - Right Stick - Fly

*DOF Mode*
- Left Stick Vertical - Adjust Focal Point
- Left Stick Horiztonal  - Adjust Narrownes
- Right Stick Vertical - Increase/Decrease Aperture

## TODO
* Support for cycling through different "Image Effect" post-processes 
