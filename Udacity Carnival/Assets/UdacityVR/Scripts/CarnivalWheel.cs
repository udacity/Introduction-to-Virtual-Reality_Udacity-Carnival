using System.Collections;
using UnityEngine;

public class CarnivalWheel : MonoBehaviour {

    [Tooltip("Floating Score Prefab")]
    public ScoreHighlight ScoreHighlighterPrefab;

    [Tooltip("The Transform component of the Wheel of Fortune that awards players with points")]
    public Transform WheelOfFortune;

    [Tooltip("The sound component for the wheel")]
    public AudioSource wheelSpinning;

    [Tooltip("The Initial Speed that is the wheel starts spinning at")]
    public float SpinForce = 10f;

    private bool isWheelSpinning = false;

    //Called when clicked
    public void SpinWheel() {
        if (!isWheelSpinning)
            StartCoroutine(SpinForSeconds(Random.Range(4f, 8f))); //spins for a random time between 4 and 8 seconds
    }

    IEnumerator SpinForSeconds(float totalTime) {
        float time = 0f;
        isWheelSpinning = true;
        wheelSpinning.Play();
        wheelSpinning.pitch = 1f;
        float percent = (1f - time / totalTime);
        while (percent > .05f) { // 95% done
            WheelOfFortune.Rotate(Vector3.forward * percent * SpinForce);
            wheelSpinning.pitch = percent;
            yield return null;
            time += Time.deltaTime;
            percent = (1f - time / totalTime);
        }

        CarnivalNeedle.OnSpokeHit += CarnivalNeedle_OnSpokeHit;

        while (isWheelSpinning) {
            WheelOfFortune.Rotate(Vector3.forward * percent * SpinForce);
            yield return null;
        }

        CarnivalNeedle.OnSpokeHit -= CarnivalNeedle_OnSpokeHit;
        wheelSpinning.Stop();

    }

    private void CarnivalNeedle_OnSpokeHit(float points) {
        isWheelSpinning = false;
        CarnivalScores.Instance.IncrementWheelScore(points);

        ScoreHighlight sh = Instantiate(ScoreHighlighterPrefab, WheelOfFortune.transform.position + WheelOfFortune.transform.forward.normalized *.5f, 
            Quaternion.LookRotation(-WheelOfFortune.transform.forward));
        sh.SetPoints(points);
    }
}
