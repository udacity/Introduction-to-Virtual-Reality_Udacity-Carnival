using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalCoinTossPlatform : MonoBehaviour {

    public delegate void CoinLandedAction();
    public static event CoinLandedAction OnCoinLanded;

    Dictionary<GameObject, float> coins = new Dictionary<GameObject, float>(); //maps coins to time they landed

	void OnCollisionEnter(Collision coin) {
        coins.Add(coin.gameObject, Time.time);
        Destroy(coin.gameObject, 15f); //destroy coin after 15 seconds to save rendering
    }

    void OnCollisionExit(Collision coin) {
        if (coins.ContainsKey(coin.gameObject)) {
            coins.Remove(coin.gameObject);
        }
    }

    void Update() {
        List<GameObject> keys = new List<GameObject>();
        foreach(KeyValuePair<GameObject, float> coin in coins) {
            if ( (Time.time - coin.Value) > 1f) {
                if (OnCoinLanded != null) {
                    OnCoinLanded();
                }
                keys.Add(coin.Key);
            }
        }

        foreach (GameObject key in keys)
            coins.Remove(key);
    }
}
