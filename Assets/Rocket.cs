using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float MainThrust = 100f;
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
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                //do nothing
                break;
            case "Bad":
                print("Dead");
                break;

        }
    }
    private void Thrust()
    {
        float ThrustThisFrame = MainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * ThrustThisFrame);
            if (audiosource.isPlaying == false)
            {
                audiosource.Play();
            }
        }
        else
        {
            audiosource.Stop();
        }
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true;  //take manual control of rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control

    }
}