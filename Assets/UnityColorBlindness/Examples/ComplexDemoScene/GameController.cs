using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
  /////////////////////////////////////////////////////////////////////////////
  // Main Game Logic
  /////////////////////////////////////////////////////////////////////////////

  //////////////////////////////////////////////////////////////////////////
  // Configuration
  //////////////////////////////////////////////////////////////////////////
  public GUISkin skin;
  public int pointsPerClick = 100;
  public float scoreMultiplier = 1.1f;
  public int lives = 3;
  //////////////////////////////////////////////////////////////////////////
  // Game State
  //////////////////////////////////////////////////////////////////////////
  private int bestStreak = 0, streak = 0, score = 0, livesLeft = 0;
  private bool newbestStreak = false, isStarted = false, isPaused = false;
  //////////////////////////////////////////////////////////////////////////
  // Setup Code
  //////////////////////////////////////////////////////////////////////////
  public void Awake() {
    useGUILayout = false;

    Time.timeScale = 0f;
    isStarted = false;
    isPaused = false;
    livesLeft = lives;
    score = 0;
    bestStreak = 0;
    streak = 0;
  }
  //////////////////////////////////////////////////////////////////////////
  // External API
  //////////////////////////////////////////////////////////////////////////
  public static GameController Instance {
    get {
      return ((GameController)GameObject.FindObjectOfType(typeof(GameController))).GetComponent<GameController>();
    }
  }

  public bool IsStarted { get { return isStarted; } }
  public bool IsPaused { get { return isPaused; } }

  public void AddClick() {
    if(!enabled) return;
    if(!isStarted) return;

    streak++;
    if(streak > bestStreak) {
      bestStreak = streak;
      newbestStreak = true;
    }

    score += (int)(pointsPerClick * Mathf.Pow(scoreMultiplier, (streak - 1)));
  }

  public bool PlayerFailsSoHard() {
    if(!enabled) return false;

    streak = 0;
    if(newbestStreak) {
      // TODO: Some sort of award / recognition...
    }
    livesLeft--;
    if(livesLeft <= 0) {
      GetComponent<GameOverScreen>().Trigger(bestStreak, score);
      return true;
    }
    return false;
  }

  public void StartGame() {
    isStarted = true;
    Time.timeScale = 1f;
  }

  public void TogglePause() {
    isPaused = !isPaused;
    if(isPaused) Time.timeScale = 0f;
    else Time.timeScale = 1f;
  }


  /////////////////////////////////////////////////////////////////////////////
  // Main Game GUI
  /////////////////////////////////////////////////////////////////////////////

  //////////////////////////////////////////////////////////////////////////
  // Shared Caches
  //////////////////////////////////////////////////////////////////////////
  private GUIStyle window = null,
                   button = null,
                   label = null,
                   boldLabel = null,
                   tightLabel = null,
                   boldTightLabel = null;
  private GUISkin  cachedSkin = null;
  private Color baseContentColor, baseBackgroundColor;

  //////////////////////////////////////////////////////////////////////////
  // Input Handling
  //////////////////////////////////////////////////////////////////////////
  public void Update() {
    if(isStarted) {
      if(Input.GetKeyDown(KeyCode.P)) TogglePause();
    } else {
      if(Input.GetKeyDown(KeyCode.G)) StartGame();
    }
  }

  //////////////////////////////////////////////////////////////////////////
  // Instructions Window
  //////////////////////////////////////////////////////////////////////////

  //////////////////////////////////////////////////////////////////////
  // Local Caches
  //////////////////////////////////////////////////////////////////////
  private int sWidth = 0, sHeight = 0;
  //////////////////////////////////////////////////////////////////////
  // Internal State
  //////////////////////////////////////////////////////////////////////
  private Vector2 scrollPos = Vector2.zero;
  private Rect windowRect = new Rect(0, 0, 370, 280);
  //////////////////////////////////////////////////////////////////////
  // Static Configuration
  //////////////////////////////////////////////////////////////////////
  private static Rect shortcutsRect = new Rect(10f, 20f, 350f, 36f),
                      instructionsRect = new Rect(10f, 80f, 350f, 47f),
                      playButtonRect = new Rect(10f, 151f, 350f, 21f),
                      changesTitleRect = new Rect(10f, 196f, 350f, 21f),
                      changesViewRect = new Rect(10f, 221f, 350f, 49f),
                      changelogRect = new Rect(4f, 4f, 326f, 171f);
  private static GUIContent instructionsTitle = new GUIContent("How to Play..."),
                            shortcutsMessage = new GUIContent("If you have trouble with keyboard shortcuts, right-click and choose full-screen!"),
                            instructionsMessage = new GUIContent("Keep the ball in the air by clicking on it.  You lose a life every time it hits the ground, and you get bonus points the more consecutive clicks you land."),
                            playButton = new GUIContent("Ready to play? [G]o!"),
                            changesTitle = new GUIContent("Changes:"),
                            changelog = new GUIContent(
                              "v1.1, 2012-12-16:\n" +
                              " - Fix for inverted view on Windows.\n" +
                              " - Visual tweaks for score window.\n" +
                              " - Visual tweaks for SSAO, and bloom.\n" +
                              " - Improve keyboard input handling.\n" +
                              " - Show score panel before game begins.\n" +
                              " - Add control panel for enabling/disabling various visual effects.\n" +
                              "\n" +
                              "v1.0, 2012-12-06:\n" +
                              " - Initial version."
                            );
  private static Color instructionsBackgroundColor = new Color(1f, 0.75f, 0f) * 2f,
                       instructionsContentColor = new Color(1f, 0.75f, 0f);
  //////////////////////////////////////////////////////////////////////
  // Code
  //////////////////////////////////////////////////////////////////////
  private void ShowInstructions() {
    if(sWidth != Screen.width || sHeight != Screen.height) {
      sWidth = Screen.width;
      sHeight = Screen.height;
      float w = windowRect.width, h = windowRect.height;
      windowRect.xMin = (sWidth - windowRect.width) / 2;
      windowRect.xMax = windowRect.xMin + w;
      windowRect.yMin = (sHeight - windowRect.height) / 2;
      windowRect.yMax = windowRect.yMin + h;
    }

    GUI.backgroundColor = instructionsBackgroundColor;
    GUI.contentColor = instructionsContentColor;

    GUI.BeginGroup(windowRect, instructionsTitle, window);
      GUI.Label(shortcutsRect, shortcutsMessage, boldLabel);

      GUI.Label(instructionsRect, instructionsMessage, tightLabel);

      if(GUI.Button(playButtonRect, playButton, button)) {
        StartGame();
      }

      GUI.Label(changesTitleRect, changesTitle, label);

      scrollPos = GUI.BeginScrollView(changesViewRect, scrollPos, changelogRect);
        GUI.Label(changelogRect, changelog, label);
      GUI.EndScrollView();
    GUI.EndGroup();

    // Superfluous since we set it explicitly just after calling this method.
    //GUI.backgroundColor = baseBackgroundColor;
    //GUI.contentColor = baseContentColor;
  }


  //////////////////////////////////////////////////////////////////////////
  // Main Game Window
  //////////////////////////////////////////////////////////////////////////

  //////////////////////////////////////////////////////////////////////
  // Local Caches
  //////////////////////////////////////////////////////////////////////
  private int scoreCached = -1,
              streakCached = -1,
              bestStreakCached = -1,
              livesLeftCached = -1;
  private GUIContent scoreValue = new GUIContent(""),
                     streakValue = new GUIContent(""),
                     bestStreakValue = new GUIContent(""),
                     livesLeftValue = new GUIContent("");
  //////////////////////////////////////////////////////////////////////
  // Static Configuration
  //////////////////////////////////////////////////////////////////////
  private const float lineHeight = 17f,
                      lineIncrement = 19f,
                      labelWidth = 76f,
                      labelLeft = 10f,
                      valueWidth = 35f,
                      valueLeft = 140f - valueWidth,
                      initialHeight = 20f;
  private static Rect mainWindowRect  = new Rect(5, 5, 150, 130),
                      scoreLabelRect  = new Rect(labelLeft, initialHeight + 0*lineIncrement, labelWidth, lineHeight),
                      scoreValueRect  = new Rect(valueLeft, initialHeight + 0*lineIncrement, valueWidth, lineHeight),
                      streakLabelRect = new Rect(labelLeft, initialHeight + 1*lineIncrement, labelWidth, lineHeight),
                      streakValueRect = new Rect(valueLeft, initialHeight + 1*lineIncrement, valueWidth, lineHeight),
                      bestLabelRect   = new Rect(labelLeft, initialHeight + 2*lineIncrement, labelWidth, lineHeight),
                      bestValueRect   = new Rect(valueLeft, initialHeight + 2*lineIncrement, valueWidth, lineHeight),
                      livesLabelRect  = new Rect(labelLeft, initialHeight + 3*lineIncrement, labelWidth, lineHeight),
                      livesValueRect  = new Rect(valueLeft, initialHeight + 3*lineIncrement, valueWidth, lineHeight),
                      pauseButtonRect = new Rect(labelLeft, initialHeight + 4*lineIncrement + 2, 130f, 21f);
  private static GUIContent windowTitle     = new GUIContent("BounceBouncy v1.1"),
                            scoreLabel      = new GUIContent("Score:"),
                            streakLabel     = new GUIContent("Clicks:"),
                            bestStreakLabel = new GUIContent("Best Streak:"),
                            livesLeftLabel  = new GUIContent("Lives Left:"),
                            pauseLabel      = new GUIContent("[P]ause"),
                            unpauseLabel    = new GUIContent("Un[p]ause");
  private static Color mainWindowBackgroundColor = Color.magenta,
                       mainWindowContentColor    = (((Vector4)Color.magenta) + ((Vector4)Color.red)) * 0.5f;
  //////////////////////////////////////////////////////////////////////
  // Code
  //////////////////////////////////////////////////////////////////////
  public void OnGUI() {
    if(skin != cachedSkin) {
      cachedSkin = skin;

      window = skin.window;
      button = skin.button;
      label = skin.label;
      boldLabel = skin.GetStyle("BoldLabel");
      tightLabel = skin.GetStyle("TightLabel");
      boldTightLabel = skin.GetStyle("BoldTightLabel");

      baseContentColor = GUI.contentColor;
      baseBackgroundColor = GUI.backgroundColor;
    }

    if(score != scoreCached) { scoreCached = score; scoreValue.text = score.ToString(); }
    if(streak != streakCached) { streakCached = streak; streakValue.text = streak.ToString(); }
    if(bestStreak != bestStreakCached) { bestStreakCached = bestStreak; bestStreakValue.text = bestStreak.ToString(); }
    if(livesLeft != livesLeftCached) { livesLeftCached = livesLeft; livesLeftValue.text = livesLeft.ToString(); }

    if(!isStarted) {
      ShowInstructions();
    }

    GUI.backgroundColor = mainWindowBackgroundColor;
    GUI.contentColor = mainWindowContentColor;

    GUI.BeginGroup(mainWindowRect, windowTitle, window);
      GUI.Label(scoreLabelRect, scoreLabel, boldTightLabel);
      GUI.Label(scoreValueRect, scoreValue, boldTightLabel);

      GUI.Label(streakLabelRect, streakLabel, boldTightLabel);
      GUI.Label(streakValueRect, streakValue, boldTightLabel);

      GUI.Label(bestLabelRect, bestStreakLabel, boldTightLabel);
      GUI.Label(bestValueRect, bestStreakValue, boldTightLabel);

      GUI.Label(livesLabelRect, livesLeftLabel, boldTightLabel);
      GUI.Label(livesValueRect, livesLeftValue, boldTightLabel);

      if(GUI.Button(pauseButtonRect, isPaused ? unpauseLabel : pauseLabel, button)) {
        TogglePause();
      }
    GUI.EndGroup();

    GUI.backgroundColor = baseBackgroundColor;
    GUI.contentColor = baseContentColor;
  }
}
