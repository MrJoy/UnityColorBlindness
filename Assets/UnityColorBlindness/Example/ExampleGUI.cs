using UnityEngine;
using System.Collections;

public class ExampleGUI : MonoBehaviour {
  public void OnGUI() {
    Color originalColor = GUI.color;

    GUILayout.BeginArea(new Rect(0, 0, 150, 250), "Colorful!", "window");
      GUI.color = Color.red;
      GUILayout.Label("I am red.", "button");
      GUI.color = Color.green;
      GUILayout.Label("I am green.", "button");
      GUI.color = Color.yellow;
      GUILayout.Label("I am yellow.", "button");
      GUI.color = Color.blue;
      GUILayout.Label("I am blue.", "button");
    GUILayout.EndArea();

    GUI.color = originalColor;

    float xSize = 350.0f;
    float xOffset = (Screen.width - xSize) / 2.0f;
    GUILayout.BeginArea(new Rect(xOffset, 0, 350, 150), "Instructions", "window");
      GUILayout.Label("Press ` to enable/disable the colorblindness control panel.");
      GUILayout.Label("Press left-ctrl+left-shift+N to switch between colorblindness modes.");
    GUILayout.EndArea();
  }
}
