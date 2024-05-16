using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    [SerializeField] private AudioSource carAudio, wheelAudio;

    public float minPitch;
    public float maxPitch;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            wheelAudio.Play();
        }

        currentSpeed = carRb.velocity.magnitude;

        float pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed));

        carAudio.pitch = pitch;
    }
}
