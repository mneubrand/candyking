using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	public Animator headAnimator;
	public Animator animator;

	public GameObject newspaper;
	public GameObject headline;
	public GameObject paragraphA;
	public GameObject paragraphB;
	public GameObject cease;

	public GameObject hearts;
	public GameObject points;

	private float minX, maxX;
	private bool stamp = false;
	private float nextLaugh;
	private AudioSource source;
	private GameObject currentNewspaper;	


	// Use this for initialization
	void Start () {
		nextLaugh = Time.time + Random.Range (3, 8);
		animator.speed = 1.5f;
		source = GetComponent<AudioSource> ();

		float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
		minX = -cameraWidth + 5;
		maxX = cameraWidth - 5;

		cease.renderer.enabled = false;

		SetNewspaperEnabled(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			if(hearts.GetComponent<HeartController>().IsDead()) {
				Application.LoadLevel(0);
			} else if(!animator.GetCurrentAnimatorStateInfo(0).IsName ("stamp") && currentNewspaper != null) {
				stamp = true;
			}
		}

		// Laugh randomly every 3 to 8 seconds
		if (Time.time > nextLaugh) {
			nextLaugh = Time.time + Random.Range (3, 8);

			source.Play();
			headAnimator.SetTrigger("Laugh");
		}
	}

	void FixedUpdate() {
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("stamp")) {
			// Cache the horizontal input.
			float h = Input.GetAxis ("Horizontal");

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			animator.SetFloat ("Speed", Mathf.Abs (h));

			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if (h * rigidbody2D.velocity.x < maxSpeed)
					// ... add a force to the player.
					rigidbody2D.AddForce (Vector2.right * h * moveForce);

			// If the player's horizontal velocity is greater than the maxSpeed...
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
					// ... set the player's velocity to the maxSpeed in the x axis.
					rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (h > 0 && !facingRight)
				// ... flip the player.
				Flip ();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (h < 0 && facingRight)
				// ... flip the player.
				Flip ();

			if (transform.position.x < minX || transform.position.x > maxX) {
				transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
				rigidbody2D.velocity = new Vector2(0, 0);
			}
		}

		if (stamp) {
			animator.SetTrigger("Stamp");
			rigidbody2D.velocity = new Vector2(0, 0);
			stamp = false;

			NewspaperController newspaper = currentNewspaper.GetComponent<NewspaperController> ();
			if(newspaper.isKing) {
				cease.renderer.enabled = true;
				points.GetComponent<PointsController>().IncreasePoints();
			} else {
				hearts.GetComponent<HeartController>().DecreaseHearts();
			}

			Destroy (currentNewspaper);
			currentNewspaper = null;
			StartCoroutine(DisableNewspaper());
		}
	}

	IEnumerator DisableNewspaper() {
		yield return new WaitForSeconds(.5f);
		cease.renderer.enabled = false;
		SetNewspaperEnabled (false);
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other) {
		SetNewspaperEnabled(true);
		cease.renderer.enabled = false;

		NewspaperController newspaper = other.gameObject.GetComponent<NewspaperController> ();

		TextMesh headlineText = headline.GetComponent<TextMesh> ();
		headlineText.text = newspaper.headline;

		TextMesh paragraphAText = paragraphA.GetComponent<TextMesh> ();
		paragraphAText.text = newspaper.paragraphA;

		TextMesh paragraphBText = paragraphB.GetComponent<TextMesh> ();
		paragraphBText.text = newspaper.paragraphB;

		currentNewspaper = other.gameObject;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject == currentNewspaper) {
			SetNewspaperEnabled(false);
			currentNewspaper = null;
		}
	}

	void SetNewspaperEnabled(bool enabled) {
		newspaper.renderer.enabled = enabled;
		headline.renderer.enabled = enabled;
		paragraphA.renderer.enabled = enabled;
		paragraphB.renderer.enabled = enabled;
	}

}
