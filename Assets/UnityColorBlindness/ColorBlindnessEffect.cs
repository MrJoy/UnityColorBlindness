/*
  Red/Green Color-Blindness Simulation Effect
  (C)Copyright 2008, MrJoy Inc, All rights reserved.

  Version: 1.0, 2008-04-28

  Changes:
    -2008-04-28, jfrisby: Initial version.

  Notes: This is intended to help you look for problems that may make it
  difficult for users of your game to play if they suffer from red/green color
  blindness.  This is NOT based on a specific study of how red/green color
  blindness behaves but is a coarse approximation and should be treated as
  such.
  Specifically, this may be useful in spotting major color issues but NOT
  finding issues with it does not mean you have no color issues and this should
  absolutely not be used for "fine tuning" the color palette of your game!

  License: Feel free to use this code however you wish, but please give credit
  where credit is due.
*/
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Blindness")]
public class ColorBlindnessEffect : ImageEffectBase {
  // Called by camera to apply image effect
  void OnRenderImage (RenderTexture source, RenderTexture destination) {
    Graphics.Blit(source, destination, material);
  }
}
