using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalPowerSlider : MonoBehaviour {

    [SerializeField]
    private Transform Slider;

    [SerializeField]
    private Transform FullPosition;

    [SerializeField]
    private Transform EmptyPosition;

    [SerializeField]
    private AudioSource whistleSource;

    [HideInInspector]
    public float value;

	public void SetPowerScale(float percentage) {
        Slider.position = Vector3.Lerp(EmptyPosition.position, FullPosition.position, percentage);
        value = percentage;
        whistleSource.pitch = percentage;
        whistleSource.Play();
    }
}
