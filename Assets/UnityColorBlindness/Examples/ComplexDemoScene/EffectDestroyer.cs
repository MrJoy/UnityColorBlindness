using UnityEngine;
using System.Collections;

public class EffectDestroyer : MonoBehaviour {
  public ParticleSystem system;

  public void Update() {
    if(system == null) return;
    if(system.isPlaying) return;

    Destroy(gameObject);
  }
}
