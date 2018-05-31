using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHighlight : MonoBehaviour {

    public float duration = 2f;
    public float height = 1f;
    public float maxScale = 3f;

    public TextMeshPro text;

	// Use this for initialization
	IEnumerator Start () {
        float time = 0f;
        Vector3 startPos = transform.position;
        Color startColor = text.color;
        Color endColor = startColor;
        endColor.a = 0.1f;
        while (time < duration) {
            float percent = time / duration;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * maxScale, percent);
            transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * height, percent);
            text.color = Color.Lerp(startColor, endColor, percent);
            yield return null;
            time += Time.deltaTime;
        }

        Destroy(this.gameObject);
	}
	
    public void SetPoints(float points) {
        text.text = "+" + points;
    }

}
