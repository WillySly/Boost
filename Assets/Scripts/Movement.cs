using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Parameters
    [SerializeField] AudioClip thrustEngine;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotateThrust = 50f;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;


    //Cache
    Rigidbody rb;
    AudioSource audioSource;

    //State
    bool isAlive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();

        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();

        }
    }
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (audioSource.isPlaying == false)
            audioSource.PlayOneShot(thrustEngine);

        if (!mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
    }
    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }





    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void RotateRight()
    {
        if (!leftThrusterParticles.isPlaying)
            leftThrusterParticles.Play();
        ApplyRotation(-rotateThrust);
    }

    private void RotateLeft()
    {
        if (!rightThrusterParticles.isPlaying)
            rightThrusterParticles.Play();

        ApplyRotation(rotateThrust);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate.

        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so that physics system can use rotation
    }
}
