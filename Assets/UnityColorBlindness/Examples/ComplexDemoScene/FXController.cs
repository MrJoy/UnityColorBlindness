using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AntialiasingAsPostEffect))]
[RequireComponent(typeof(BloomAndLensFlares))]
[RequireComponent(typeof(SSAOEffect))]
[RequireComponent(typeof(ColorBlindnessEffect))]
public class FXController : MonoBehaviour {
  public GUISkin skin;
  public AntialiasingAsPostEffect fsaaEffect;
  public BloomAndLensFlares bloomEffect;
  public SSAOEffect ssaoEffect;
  public ColorBlindnessEffect colorBlindnessEffect;

  public void Awake() {
    useGUILayout = false;
  }

  public void Update() {
    if(Input.GetKeyDown(KeyCode.F)) { fsaaEffect.enabled = !fsaaEffect.enabled; }
    if(Input.GetKeyDown(KeyCode.B)) { bloomEffect.enabled = !bloomEffect.enabled; }
    if(Input.GetKeyDown(KeyCode.S)) { ssaoEffect.enabled = !ssaoEffect.enabled; }
    if(Input.GetKeyDown(KeyCode.C)) { colorBlindnessEffect.enabled = !colorBlindnessEffect.enabled; }
  }

  private const float offset = 5;
  private static GUIContent windowTitle = new GUIContent("Options"),
                            fsaaLabel   = new GUIContent("[F]SAA"),
                            bloomLabel  = new GUIContent("[B]loom"),
                            ssaoLabel   = new GUIContent("[S]SAO"),
                            cbLabel     = new GUIContent("[C]olor Blindness");

  private const float buttonLeft = 10f,
                      heightOffset = 20f,
                      heightIncrement = 23f,
                      lineWidth = 130f,
                      lineHeight = 21f;
  private static Rect fsaaRect  = new Rect(buttonLeft, heightOffset + 0 * heightIncrement, lineWidth, lineHeight),
                      bloomRect = new Rect(buttonLeft, heightOffset + 1 * heightIncrement, lineWidth, lineHeight),
                      ssaoRect  = new Rect(buttonLeft, heightOffset + 2 * heightIncrement, lineWidth, lineHeight),
                      cbRect    = new Rect(buttonLeft, heightOffset + 3 * heightIncrement, lineWidth, lineHeight);

  private Rect windowRect = new Rect(offset, offset, 150, 120);
  private GUISkin cachedSkin = null;
  private GUIStyle window = null,
                   leftButton = null;
  private int sHeight = 0;

  public void OnGUI() {
    if(cachedSkin != skin) {
      cachedSkin = skin;
      window = skin.window;
      leftButton = skin.GetStyle("LeftButton");
    }

    if(sHeight != Screen.height) {
      sHeight = Screen.height;
      float h = windowRect.height;
      windowRect.yMin = Screen.height - (offset + h);
      windowRect.yMax = windowRect.yMin + h;
    }

    GUI.BeginGroup(windowRect, windowTitle, window);
      LabeledValue(fsaaRect, fsaaLabel, fsaaEffect);
      LabeledValue(bloomRect, bloomLabel, bloomEffect);
      LabeledValue(ssaoRect, ssaoLabel, ssaoEffect);
      LabeledValue(cbRect, cbLabel, colorBlindnessEffect);
    GUI.EndGroup();
  }

  private void LabeledValue(Rect r, GUIContent label, MonoBehaviour value) {
    GUI.changed = false;
    bool result = GUI.Toggle(r, value.enabled, label, leftButton);
    if(GUI.changed) {
      value.enabled = result;
    }
  }
}
