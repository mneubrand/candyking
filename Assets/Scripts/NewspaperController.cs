using UnityEngine;
using System.Collections;

public class NewspaperController : MonoBehaviour {

	private static string[] nouns = {
		"Sofa",
		"History",
		"Rings",
		"Distance",
		"Pocket",
		"Chalk",
		"Art",
		"System",
		"Pencil",
		"Pipe",
		"Arm",
		"Rake",
		"Glove",
		"Neck",
		"String",
		"Legs",
		"Friend",
		"Writer",
		"Slope",
		"Box",
		"Minute",
		"Desire",
		"Wash",
		"Reaction",
		"Sheep",
		"Finger",
		"Plant",
		"Church",
		"Toes",
		"Meal",
		"Houses",
		"Cast",
		"Honey",
		"Reading",
		"Hobbies",
		"Question",
		"Join",
		"View",
		"Boundary",
		"Hook",
		"Door",
		"Ice"
	};

	private static string[] endings = {
		"Studio",
		"Entertainment",
		"Inc.",
		"Ltd.",
		"Games",
		"Software",
		"Corporation",
		"Interactive",
		"Electronics",
		"Productions"
	};

	private static string[] gameNouns = {
		"Ninja",
		"Stealth",
		"Action",
		"War",
		"Dragon",
		"Lightning",
		"Pet",
		"Killer",
		"World",
		"Space",
		"Metal"
	};

	private static string[] gameEndings = {
		"Adventure",
		"Assault",
		"Empire",
		"Heroes",
		"Champion",
		"Invader",
		"Madness",
		"Slots",
		"Bingo"
	};

	private static string[] studioHeadlines = {
		"Game announcement by {0}",
		"New game by {0}",
		"{0} teases new game"
	};

	private static string[] gameHeadlines = {
		"{0} announced",
		"Newly announced {0}",
		"{0} beta released",
		"Alpha review of {0}"
	};

	private static string[] paragraphs = {
		"{0} by {1}\nis looking very promising. Check out\nour detailed preview.",
		"In one of my favorite genres\n{0} is looking exciting\n{1} is a masterpiece.",
		"Indie studio {1}\nis giving us an exclusive preview of\ntheir new title {0}.",
		"{0} is unique.\n{1} delivered an\namazingly crafted experience.",
	};

	private static string[] genericParagraphs = {
		"According to the developers\nthis title will raise the bar\nto a whole new level.",
		"The game has spectacular\n3D graphics based on a\ncompletely new engine",
		"The trailer is breathtaking\nWe haven't looked forward to\na game this much in a long time",
	};

	public float speed;
	public float duration;
	public GameObject stripe;

	public string headline;
	public string paragraphA;
	public string paragraphB;
	public bool isKing = false;

	private GameObject hearts;
	private bool inPosition = false;
	private float startScale;
	private float startTime;

	// Use this for initialization
	void Start () {
		hearts = GameObject.FindGameObjectWithTag ("Finish");
		startScale = stripe.transform.localScale.z;
		int where = Random.Range (0, 2);

		string studioName = nouns [Random.Range (0, nouns.Length)] + " " + endings [Random.Range (0, endings.Length)];

		float random = Random.value;
		isKing = random < 0.5;

		string gameNoun = random < 0.25 ? "Candy" : gameNouns [Random.Range (0, gameNouns.Length)];
		string gameEnding = random >= 0.25 && random < 0.5 ? "Saga" : gameEndings [Random.Range (0, gameEndings.Length)]; 
		string gameName = gameNoun + " " + gameEnding;

		if(where == 0) {
			headline = string.Format(studioHeadlines[Random.Range (0, studioHeadlines.Length)], studioName);
		} else {
			headline = string.Format(gameHeadlines[Random.Range (0, studioHeadlines.Length)], gameName);
		}

		if (where == 0) {
			paragraphA = string.Format (paragraphs [Random.Range (0, paragraphs.Length)], gameName, studioName);
			paragraphB = genericParagraphs [Random.Range (0, genericParagraphs.Length)];
		} else {
			paragraphB = genericParagraphs [Random.Range (0, genericParagraphs.Length)];
			paragraphA = genericParagraphs [Random.Range (0, genericParagraphs.Length)];
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > -3.916f && !inPosition) {
			transform.Translate (0, - speed * Time.deltaTime, 0);
		} else if(!inPosition) {
			inPosition = true;
			startTime = Time.time;
			transform.position = new Vector3(transform.position.x, -3.916f, transform.position.z);
		}

		if (inPosition) {
			stripe.renderer.enabled = true;
			stripe.transform.localScale = new Vector3(Mathf.Lerp(startScale, 0, (Time.time - startTime) / duration), stripe.transform.localScale.y, stripe.transform.localScale.z);

			if (Time.time - startTime > duration) {
				if(isKing) {
					hearts.GetComponent<HeartController>().DecreaseHearts();
				}
				Destroy(gameObject);
			}
		}
	}
}