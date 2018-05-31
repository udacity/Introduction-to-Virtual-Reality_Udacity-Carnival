using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoCoin : MonoBehaviour {

    private AudioSource ding;
    private Rigidbody rBody;
    private CarnivalPlinko myBoard;

    private bool firstHitBottom = true;

    void Start() {
        ding = GetComponent<AudioSource>();
        rBody = GetComponent<Rigidbody>();
        rBody.isKinematic = true;
    }

    public void DropCoin(CarnivalPlinko plinko) {
        rBody.isKinematic = false;
        myBoard = plinko;
    }

    public void OnCollisionStay(Collision collision) { //we use stay since we are using one collision mesh
        if (collision.impulse.magnitude > .4f) {
            ding.Play();
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (firstHitBottom) {
            firstHitBottom = false;
            myBoard.CoinHitBottom(float.Parse(other.gameObject.GetComponentInChildren<TMPro.TextMeshPro>().text) );
            StartCoroutine(DeleteAfterSeconds(5f));
        }
    }

    IEnumerator DeleteAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
