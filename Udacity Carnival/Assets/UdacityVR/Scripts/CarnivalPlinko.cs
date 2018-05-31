using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalPlinko : MonoBehaviour {

    [Tooltip("Floating Score Prefab")]
    public ScoreHighlight ScoreHighlighterPrefab;

    [Tooltip("The Prefab used to spawn the Plinko Coin")]
    public PlinkoCoin PlinkoCoinPrefab;

    [Tooltip("The Center point for the Plinko Coin to osscilate between")]
    public Transform PlinkoCoinOrigin;

    [Tooltip("A Scalar for the distance the plinko coin should osscilate")]
    public float OscillationDistance = 0.5f;

    [Tooltip("A scalar for the speed the plinko coin should osscilate at")]
    public float OscillationSpeed = 2f;

    [Tooltip("A sound to play when the plinko coin drops")]
    public AudioSource BellDing;

    [Tooltip("A sound to play when the plinko coin hits the bottom")]
    public AudioSource Thud;

    private PlinkoCoin currentCoin; //the coin that we are in charge off
    private bool noActiveCoin = true;
    private float createCoinTime = 0f;
	
	// Update is called once per frame
	void Update () {
		if (noActiveCoin) {
            CreateCoin();
            noActiveCoin = false;
        }

        if (currentCoin != null)
            currentCoin.transform.position = PlinkoCoinOrigin.position + PlinkoCoinOrigin.forward * OscillationDistance * Mathf.Sin(OscillationSpeed * (Time.time - createCoinTime));

    }

    private void CreateCoin() {
        currentCoin = Instantiate(PlinkoCoinPrefab.gameObject).GetComponent<PlinkoCoin>();
        currentCoin.transform.position = PlinkoCoinOrigin.position;
        createCoinTime = Time.time;
    }

    //to be called from the Plinko Coin script
    public void CoinHitBottom(float points) { 
        noActiveCoin = true;
        CarnivalScores.Instance.IncrementPlinkoScore(points);
        ScoreHighlight sh = Instantiate(ScoreHighlighterPrefab, PlinkoCoinOrigin.transform.position, Quaternion.LookRotation(-PlinkoCoinOrigin.transform.right) );
        sh.SetPoints(points);

        Thud.Play();
    }

    public void DropCoin() {
        currentCoin.DropCoin(this);
        currentCoin = null;
        BellDing.Play();
    }
}
