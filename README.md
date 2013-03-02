# UnityColorBlindness

UnityColorBlindness is a tool for Unity3D to allow developers with normal color
vision to simulate the various forms of "color blindness" (atypical color
perception) in your game.

It will apply after image effects, and UnityGUI have been rendered.

It provides you the ability to select any of the eight basic forms of atypical
color perception and view your game as if you were affected by that form of
vision anomoly.


## Compatibility

Unity3D Pro version 3.5 or above is required.


## Installation

Import the UnityColorBlindness package into your project.

You must also import (or have imported) the following files from the
"Image Effects (Pro Only)" package into your project:

* `Standard Assets/Image Effects (Pro Only)/ImageEffectBase.cs`
* `Standard Assets/Image Effects (Pro Only)/ImageEffects.cs`


## Caveats

I have not tried this code with scenes that utilize multiple cameras rendering
to the screen in layers.


## License

The code is licensed under the MIT X11 License.  See the file 'LICENSE' for
details.
