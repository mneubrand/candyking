using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

	public SpriteRenderer heartA;
	public SpriteRenderer heartB;
	public SpriteRenderer heartC;

	public SpriteRenderer gameOver;

	private int numHearts = 3;
	private SpriteRenderer[] hearts;
	private AudioSource source;

	void Start() {
		hearts = new SpriteRenderer[] { heartA, heartB, heartC };
		source = GetComponent<AudioSource> ();
	}

	public void DecreaseHearts() {
		numHearts--;
		if (numHearts >= 0) {
			hearts [numHearts].enabled = false;
			source.Play ();
		}

		if (IsDead ()) {
			gameOver.enabled = true;
		}
	}

	public bool IsDead() {
		return numHearts <= 0;
	}
}
