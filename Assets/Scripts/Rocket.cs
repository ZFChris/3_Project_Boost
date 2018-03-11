using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float MainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    Rigidbody rigidBody;
    AudioSource audiosource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    [SerializeField]bool collisionsDisabled = false;
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
            RespondToThrustInput();
            RespondToRotateInput();
        }
        //todo only if debug on
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Bad":
                DeathScene();
                break;
            case "Goal":
                SuccessScene();
                break;

        }


    }
     private void DebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKey(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; // toggle collision
        }
    }
private void SuccessScene()
    {
        mainEngineParticles.Stop();
        state = State.Transcending;
        audiosource.Stop();
        audiosource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private void DeathScene()
    {
        mainEngineParticles.Stop();
        state = State.Dying;
        audiosource.Stop();
        audiosource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", 2f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int CurrentsceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (CurrentsceneIndex == 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(CurrentsceneIndex + 1);//todo allow more than 2 levels
        }
    }

    private void RespondToThrustInput()
    {
        float ThrustThisFrame = MainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
            ApplyThrust(ThrustThisFrame);
        else
        {
            audiosource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float ThrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * ThrustThisFrame);
        if (audiosource.isPlaying == false)
        {
            audiosource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
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