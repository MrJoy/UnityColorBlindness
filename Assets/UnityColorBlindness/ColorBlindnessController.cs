using UnityEngine;
using System.Collections;

public class ColorBlindnessController : MonoBehaviour {
  public GUISkin skin;
  private ColorBlindnessEffect effect = null;

  public void Awake() {
    useGUILayout = false;
  }

  private const int MIN_KEYCODE = (int)KeyCode.Alpha1,
                    MAX_KEYCODE = (int)KeyCode.Alpha9;

  public void Update() {
    int i = MIN_KEYCODE,
        j = (int)KeyCode.Keypad1; // Number pad is also contiguous but in a
                                  // different range.

    while(i <= MAX_KEYCODE) {
      if(Input.GetKeyDown((KeyCode)i) || Input.GetKeyDown((KeyCode)j))
        effect.mode = (ColorModification)(i - MIN_KEYCODE);
      i++;
      j++;
    }
  }

  private const float windowOffsetX = 5f;
  private static GUIContent windowTitle = new GUIContent("Simulate");
  private static GUIContent[] modeTitles = new GUIContent[] {
                                new GUIContent("[1] " + ((ColorModification)0).ToString()),
                                new GUIContent("[2] " + ((ColorModification)1).ToString()),
                                new GUIContent("[3] " + ((ColorModification)2).ToString()),
                                new GUIContent("[4] " + ((ColorModification)3).ToString()),
                                new GUIContent("[5] " + ((ColorModification)4).ToString()),
                                new GUIContent("[6] " + ((ColorModification)5).ToString()),
                                new GUIContent("[7] " + ((ColorModification)6).ToString()),
                                new GUIContent("[8] " + ((ColorModification)7).ToString()),
                                new GUIContent("[9] " + ((ColorModification)8).ToString())
                              };
  private const float modeLeft = 10f,
                      modeWidth = 130f,
                      modeHeight=21f,
                      heightOffset = 20f,
                      heightIncrement = 23f;
  private static Rect[] modeRects = new Rect[] {
                          new Rect(modeLeft, heightOffset + 0 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 1 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 2 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 3 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 4 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 5 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 6 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 7 * heightIncrement, modeWidth, modeHeight),
                          new Rect(modeLeft, heightOffset + 8 * heightIncrement, modeWidth, modeHeight)
                        };

  private int sWidth = 0;
  private GUISkin skinCached = null;
  private GUIStyle window = null,
                   leftButton = null;
  private Rect windowRect = new Rect(windowOffsetX, 5, 150, 235);
  public void OnGUI() {
    if(effect == null) {
      effect = Camera.main.GetComponent<ColorBlindnessEffect>();
      if(effect == null) {
        enabled = false;
        return;
      }
    }

    if(skinCached != skin) {
      skinCached = skin;
      window = skin.window;
      leftButton = skin.GetStyle("LeftButton");
    }

    if(sWidth != Screen.width) {
      sWidth = Screen.width;
      float w = windowRect.width;
      windowRect.xMin = sWidth - (windowOffsetX + w);
      windowRect.xMax = windowRect.xMin + w;
    }

    GUI.BeginGroup(windowRect, windowTitle, window);
      for(int k = 0; k < 9; k++) {
        ColorModification cm = (ColorModification)k;
        if(effect.mode == cm) {
          GUI.Toggle(modeRects[k], true, modeTitles[k], leftButton);
        } else {
          if(GUI.Button(modeRects[k], modeTitles[k], leftButton))
            effect.mode = cm;
        }
      }
    GUI.EndGroup();
  }
}
