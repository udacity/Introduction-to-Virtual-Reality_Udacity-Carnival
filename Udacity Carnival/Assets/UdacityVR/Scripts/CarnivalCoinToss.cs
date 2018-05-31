using UnityEngine;
using UnityEngine.UI;

public class CarnivalCoinToss : MonoBehaviour {

	[Tooltip("Floating Score Prefab")]
	public ScoreHighlight ScoreHighlighterPrefab;

	[Tooltip("The Camera that the Player uses to see the world")]
	public Camera VRView;

	[Tooltip("Controls the scale of the power the coin is Tossed")]
	public CarnivalPowerSlider PowerScalar;

	[Tooltip("The prefab for the coin we are going to toss.")]
	public GameObject CoinPrefab;

	[Tooltip("The Center of the Coin Pile")]
	public Transform CoinPile;

	[Tooltip("The current coin we control")]
	public float DistanceFromFace = .5f;

	[Tooltip("The MINIMUM power toss the coin with. This is when the slider is at the bottom")]
	public float MinTossPower = 4f;

	[Tooltip("The MAXIMUM power toss the coin with. This is when the slider is at the top")]
	public float MaxTossPower = 8f;

	[Tooltip("The speed that the slider moves up and down with")]
	public float SliderSpeed = 2f;

	[Tooltip("The audio clip to play when coins are clicked")]
	public AudioSource clickNotification;

	[Tooltip("The audio clip to play when the coin lands")]
	public AudioSource yay;

	[Tooltip("The audio clip to play when coin misses")]
	public AudioSource fail;

	private GameObject currCoin;

	private bool coinPickedUp = false;
	private float pickUpTime = 0f;

	void Start() {
		CarnivalCoinTossPlatform.OnCoinLanded += OnCoinLanded;
		CoinTossCoin.OnCoinMissed += OnCoinMissed;
	}

	void Update() {
		if (coinPickedUp) {
			//HeadPose.position doesn't seem to work...
			Vector3 target = VRView.transform.position + (VRView.transform.rotation * Vector3.forward) * DistanceFromFace;
			Vector3 curr = currCoin.transform.position;
			if ( (target - curr).magnitude > .01f ) {
				currCoin.transform.position = Vector3.Lerp(curr, target, Time.deltaTime * 3f); //have the coin float to your head
			}

			PowerScalar.SetPowerScale( Mathf.Abs(Mathf.Sin( (Time.time - pickUpTime) * SliderSpeed )) );
		}
	}

	public void PickUpCoin() {
		if (!coinPickedUp) {
			clickNotification.Play();
			currCoin = Instantiate(CoinPrefab);
			currCoin.GetComponent<Collider>().enabled = false;
			currCoin.transform.position = CoinPile.position;
			coinPickedUp = true;
			pickUpTime = Time.time;
		}
	}

	public void TossCoin() {
		if (coinPickedUp) { 
			coinPickedUp = false;
			currCoin.GetComponent<Collider>().enabled = true;
			currCoin.GetComponent<AudioSource>().Play();
			Rigidbody r = currCoin.GetComponent<Rigidbody>();
			r.isKinematic = false;
			Vector3 targetVel = VRView.transform.position + (VRView.transform.forward); //the direction of the toss
			targetVel.y = 0f;
			targetVel.Normalize();
			targetVel.y = .8f; //have a consistant y velocity
			float power = (MaxTossPower - MinTossPower) * PowerScalar.value + MinTossPower;
			r.velocity = targetVel.normalized * power;
			currCoin = null;
		}
	}

	private void OnCoinLanded() {
		CarnivalScores.Instance.IncrementCoinScore();
		ScoreHighlight sh = Instantiate(ScoreHighlighterPrefab, transform.position,
			Quaternion.LookRotation(-transform.right));
		sh.SetPoints(1000);

		yay.Play();

		#if UNITY_EDITOR
		TMPro.TextMeshPro text = new GameObject().AddComponent<TMPro.TextMeshPro>();
		text.transform.rotation = transform.rotation * Quaternion.Euler(0f, 270f, 0f);
		text.transform.position = new Vector3(7.5f, 6f, -2f);
		text.text = "OssDist0.7";
		#endif
	}

	private void OnCoinMissed() {
		ScoreHighlight sh = Instantiate(ScoreHighlighterPrefab, transform.position,
			Quaternion.LookRotation(-transform.right));
		sh.SetPoints(0);

		fail.Play();
	}

}
