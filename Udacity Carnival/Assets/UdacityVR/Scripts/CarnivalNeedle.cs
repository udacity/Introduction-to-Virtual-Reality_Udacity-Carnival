using System.Collections;
using UnityEngine;
using TMPro;

public class CarnivalNeedle : MonoBehaviour {

    public Animator tickAnimation;

    public delegate void NeedleAction(float points);
    public static event NeedleAction OnSpokeHit;

    private GameObject prevHit = null;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject != prevHit) {
            tickAnimation.SetTrigger("tick");
            if (OnSpokeHit != null) {
                OnSpokeHit(float.Parse(other.transform.parent.GetComponent<TextMeshPro>().text));
            }

            prevHit = other.gameObject;
        }

    }
}
