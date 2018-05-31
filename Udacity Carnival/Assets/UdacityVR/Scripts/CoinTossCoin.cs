using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTossCoin : MonoBehaviour {

    public delegate void CoinLandedAction();
    public static event CoinLandedAction OnCoinMissed;

    void OnCollisionEnter(Collision other) {
        if (OnCoinMissed != null && !other.gameObject.tag.Equals("CoinToss")) {
            OnCoinMissed();
        }
    }
}
