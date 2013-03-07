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
  }
}
