using UnityEngine;
using System.Collections;

public class ClickResponder : MonoBehaviour {
  public float explosionForce = 1f, explosionRadius = 1f, upwardsModifier = 0f;
  public ForceMode mode = ForceMode.Force;
  public GameObject impactEffect;

  public void OnCollisionEnter(Collision collision) {
    if(!GameController.Instance.IsStarted) return;
    if(GameController.Instance.IsPaused) return;
    if(GameController.Instance.PlayerFailsSoHard()) {
      rigidbody.Sleep();
      //Destroy(rigidbody);
    }
    if(impactEffect != null) {
      ContactPoint contact = collision.contacts[0];
      Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
      Vector3 pos = contact.point;
      Instantiate(impactEffect, pos, rot);
    }
  }

  public void OnMouseDown() {
    if(!GameController.Instance.IsStarted) return;
    if(GameController.Instance.IsPaused) return;
    rigidbody.AddExplosionForce(explosionForce, new Vector3(0, -1, 0), explosionRadius, upwardsModifier, mode);
    GameController.Instance.AddClick();
  }
}
