using UnityEngine;
using System.Collections;

public class ColorBlindnessController : MonoBehaviour {
  //---------------------------------------------------------------------------
  // User-configuration.  You may want to mess with this, to ensure the
  // keyboard shortcuts don't interfere with your game.
  //---------------------------------------------------------------------------
  public bool displayOnStart = true;
  public KeyCode[] toggleModifierKeys = new KeyCode[] {};
  public KeyCode[] toggleMainKeyCodes = new KeyCode[] {
    KeyCode.BackQuote
  };
  public KeyCode[] modeModifierKeys = new KeyCode[] {
    KeyCode.LeftControl,
    KeyCode.LeftShift
  };


  //---------------------------------------------------------------------------
  // Static configuration -- you shouldn't need to mess with this.
  //---------------------------------------------------------------------------
  private static KeyCode[][] modeMainKeyCodes = new KeyCode[][] {
    new KeyCode[] { KeyCode.Alpha1, KeyCode.Keypad1 },
    new KeyCode[] { KeyCode.Alpha2, KeyCode.Keypad2 },
    new KeyCode[] { KeyCode.Alpha3, KeyCode.Keypad3 },
    new KeyCode[] { KeyCode.Alpha4, KeyCode.Keypad4 },
    new KeyCode[] { KeyCode.Alpha5, KeyCode.Keypad5 },
    new KeyCode[] { KeyCode.Alpha6, KeyCode.Keypad6 },
    new KeyCode[] { KeyCode.Alpha7, KeyCode.Keypad7 },
    new KeyCode[] { KeyCode.Alpha8, KeyCode.Keypad8 },
    new KeyCode[] { KeyCode.Alpha9, KeyCode.Keypad9 },
  };
  private const float windowOffsetX = 5f,
                      modeLeft = 10f,
                      modeWidth = 130f,
                      modeHeight=21f,
                      heightOffset = 20f,
                      heightIncrement = 23f;
  private static GUIContent windowTitle = new GUIContent("Simulate...");
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


  //---------------------------------------------------------------------------
  // Internal state.  Don't mess with this.
  //---------------------------------------------------------------------------
  private ColorBlindnessEffect effect = null;
  private bool isActive = false;
  private int sWidth = 0;
  private GUIStyle window = null,
                   leftButton = null;
  private Rect windowRect = new Rect(windowOffsetX, 5, 150, 235);


  //---------------------------------------------------------------------------
  // Initialization code.
  //---------------------------------------------------------------------------
  public void Awake() {
    // Skip the overhead of GUILayout, as we're managing this all ourself....
    useGUILayout = false;
    if(displayOnStart)
      isActive = true;
  }


  //---------------------------------------------------------------------------
  // Input handling, and generally our real logic.
  //---------------------------------------------------------------------------
  private bool CheckModifiers(KeyCode[] modifiers) {
    foreach(KeyCode code in modifiers) {
      // No sense checking whether something is pressed if the modifier keys
      // aren't being held down...
      if(!Input.GetKey(code))
        return false;
    }
    return true;
  }

  private void CheckForAndApplyModeSwitch() {
    if(!CheckModifiers(modeModifierKeys))
      return;

    int pressedMainKeyIdx = -1;
    for(int i = 0; i < modeMainKeyCodes.GetLength(0) && pressedMainKeyIdx == -1; i++) {
      KeyCode[] keyCodes = modeMainKeyCodes[i];
      for(int j = 0; j < keyCodes.Length && pressedMainKeyIdx == -1; j++) {
        if(Input.GetKeyDown(keyCodes[j]))
          pressedMainKeyIdx = i;
      }
    }

    if(pressedMainKeyIdx > -1) {
      effect.mode = (ColorModification)pressedMainKeyIdx;
    }
  }

  public void CheckForAndApplyStateSwitch() {
    if(!CheckModifiers(toggleModifierKeys))
      return;

    bool pressedToggleKey = false;
    foreach(KeyCode code in toggleMainKeyCodes) {
      if(Input.GetKeyDown(code)) {
        pressedToggleKey = true;
        break;
      }
    }

    if(pressedToggleKey)
      isActive = !isActive;
  }

  public void Update() {
    if(isActive)
      CheckForAndApplyModeSwitch();

    CheckForAndApplyStateSwitch();
  }


  //---------------------------------------------------------------------------
  // User interface.
  //---------------------------------------------------------------------------
  private void UpdateCaches() {
    if(effect == null) {
      effect = Camera.main.GetComponent<ColorBlindnessEffect>();
      if(effect == null) {
        enabled = false;
        return;
      }
    }

    if(window == null)
      window = GUI.skin.window;

    if(leftButton == null) {
      leftButton = new GUIStyle(GUI.skin.button) {
        name = "LeftButton",
        alignment = TextAnchor.MiddleLeft
      };
    }

    if(sWidth != Screen.width) {
      sWidth = Screen.width;
      float w = windowRect.width;
      windowRect.xMin = sWidth - (windowOffsetX + w);
      windowRect.xMax = windowRect.xMin + w;
    }
  }

  public void OnGUI() {
    if(!isActive)
      return;

    UpdateCaches();

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
