using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audiosource;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProccesInput();
	}

    private void ProccesInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * 90 * Time.deltaTime);
            if (audiosource.isPlaying == false)
            {
                audiosource.Play();
            }
        }
        else
        {
            audiosource.Stop();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * 90 * Time.deltaTime);
        }
        }
    }