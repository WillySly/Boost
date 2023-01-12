using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{

    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip crashSound;

    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem crashParticles;

    [SerializeField] float leveLoadDelay = 2f;

    AudioSource audioSource;
    bool collisionDisabled = false;

    bool isTransitioning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckKeys();
    }

    void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("collision Disabled= " + collisionDisabled);
            collisionDisabled = !collisionDisabled;            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (isTransitioning || collisionDisabled) return;

        Debug.Log("collidign");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":

                StartSuccessSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }

    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        winParticles.Play();

        audioSource.PlayOneShot(winSound);

        GetComponent<Movement>().enabled = false;

        Invoke("LoadNextLevel", leveLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;

        audioSource.Stop();
        crashParticles.Play();


        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadLevel", leveLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }



}
