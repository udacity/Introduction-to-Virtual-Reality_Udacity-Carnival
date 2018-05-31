using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoWin : MonoBehaviour {

    [Tooltip("A sound to celebrate")]
    public AudioSource Yay;

    [Tooltip("A particle effect to burst and celebrate")]
    public ParticleSystem ps;

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlinkoCoin>() != null) {
            Yay.Play();
            ps.Emit(50);
        }
    }
}
