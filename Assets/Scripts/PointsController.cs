using UnityEngine;
using System.Collections;

public class PointsController : MonoBehaviour {

	private TextMesh text;
	private AudioSource source;
	private int points = 0;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMesh> ();
		text.text = "0";

		source = GetComponent<AudioSource> ();
	}
	
	public void IncreasePoints() {
		text.text = "" + (++points);
		source.Play ();
	}
}
