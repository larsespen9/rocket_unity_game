using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    // Game variables
    [SerializeField]float rcsThrust = 100f; // Gjør at vi kan endre verdi i unity ide istedenfor her
    [SerializeField] float mainThrust = 100f; // Gjør at vi kan endre verdi i unity ide istedenfor her

    Rigidbody rigidBody;
    AudioSource audiosource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
    void Update () {
        Thrust();
        Rotate();
	}
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK"); // todo remove 
                break;
            case "Finish":
                print("Finish"); // todo remove
                break;
            default:
                print("Dead");
                break;
        }
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
