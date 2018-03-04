using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float MainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audiosource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();


	}
    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ProccesInput();
        }
    }
    private void ProccesInput()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                //do nothing
                break;
            case "Bad":
                state = State.Dying;
                audiosource.Stop();
                Invoke("LoadFirstLevel", 2f);
                break;
            case "Goal":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;

        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);//todo allow more than 2 levels
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