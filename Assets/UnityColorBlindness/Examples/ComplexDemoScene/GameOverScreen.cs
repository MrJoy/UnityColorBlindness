using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
  public GUISkin skin;
  public string highestClickStreak = "", finalScore = "";

  //public void OnEnable() { Time.timeScale = 0f; }
  public void OnDisable() { Time.timeScale = 1f; }

  public void Trigger(int hcs, int fs) {
    highestClickStreak = hcs.ToString();
    finalScore = fs.ToString();
    enabled = true;
    StartCoroutine(SlowTimeDown());
  }

  public IEnumerator SlowTimeDown() {
    float s = Time.realtimeSinceStartup;
    float n = s;
    while(((n - s) < 3.0f) && enabled) {
      n = Time.realtimeSinceStartup;
      Time.timeScale = Mathf.Lerp(1f, 0f, Mathf.Clamp01(n-s));
      yield return new WaitForEndOfFrame();
    }
    Time.timeScale = 0f;
  }

  public void OnGUI() {
    GUI.skin = skin;
    Color baseline = GUI.color,
          contentBaseline = GUI.contentColor;
    GUI.contentColor = Color.red * 0.75f;
    GUI.color = Color.red * 2.0f;

    GUILayout.BeginArea(new Rect((Screen.width - 150) / 2, (Screen.height - 100) / 2, 150, 100), "GAME OVER!", "window");
      GUILayout.Label("LOSER!", "CenteredLabel");
      GUI.contentColor = Color.green * 0.75f;
      GUI.color = Color.green * 2.0f;
      if(GUILayout.Button("Play again?")) {
        Application.LoadLevel(Application.loadedLevel);
      }
    GUILayout.EndArea();

    GUI.color = baseline;
    GUI.contentColor = contentBaseline;
  }
}
