using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    // todo fix lighting bug 
    // Game variables
    [SerializeField]float rcsThrust = 100f; // Gjør at vi kan endre verdi i unity ide istedenfor her
    [SerializeField] float mainThrust = 100f; // Gjør at vi kan endre verdi i unity ide istedenfor her

    Rigidbody rigidBody;
    AudioSource audiosource;

    // Game state
    enum State { Alive, Dying, Trancending };
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
    void Update () {
        // todo stop sound on dead
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){return;} // gidder ikke å kjøre all koden under hvis vi allerede har dau
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK"); // todo remove 
                break;
            case "Finish":
                state = State.Trancending;
                Invoke("LoadNextScene", 1f); // todo lag en serialized filed så vi kan justere denne ved testing
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private static void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private static void LoadNextScene() // todo allow for more than two levels 
    {
        SceneManager.LoadScene(1);
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // Denne stopper spin slik at vi kan ta kontrol over rotasjon
      
        float rotationFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotationFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationFrame);
        }
        rigidBody.freezeRotation = false; // Etter vi har tatt kontrol så enabler physics igjen. 
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up*thrustFrame);
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
        else
        {
            audiosource.Stop();
        }
    }
}
