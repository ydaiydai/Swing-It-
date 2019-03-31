using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SwingController : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 20.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
//    public Camera cam;
    enum State { Swinging, Falling, Walking };
    State state;
    public Pendulum pendulum;

    Vector3 previousPosition;
    float distToGround;
    Vector3 hitPos;

    // slow motion 
    public TimeManager timeManager;
    public Text slowText;
    public int slowMotionCounts;
    public int maxCountsOfSlowMotion;
    // Timer
    public Text timerLabel;
    public float timeRemaining;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        state = State.Walking;
        pendulum.player_tr.transform.parent = pendulum.tether.tether_tr;
        previousPosition = transform.localPosition;

        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;

        // display initial slow counts 
        slowMotionCounts = 0;

        // display timer
        timeRemaining = 60.0f;
        timerLabel.text =  FormatTime(timeRemaining);
    }

    void Update()
    {
        DetermineState();

        switch (state)
        {
            case State.Swinging:
                DoSwingAction();
                break;
            case State.Falling:
                DoFallingAction();
                break;
            case State.Walking:
                DoWalkingAction();
                break;
        }
        previousPosition = transform.localPosition;

        // optional -> decreasing the length of arm
        // but it speeds up the velocity 
        // if (pendulum.arm.length > 1000f) {
        //     pendulum.arm.length -= 100f * Time.deltaTime;
        // }

        slowText.text = "Count: " + slowMotionCounts.ToString();
        if (timeRemaining >= 0){
            timeRemaining -= Time.deltaTime;
        }
        timerLabel.text =  FormatTime(timeRemaining);

    }

    bool IsGrounded()
    {
        print("Grounded");
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void DetermineState()
    {
        // Determine State
        if (IsGrounded())
        {
            state = State.Walking;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (state == State.Walking)
                {
                    pendulum.player.velocity = moveDirection;
                }
                pendulum.SwitchTether(hit.point);
                state = State.Swinging;

            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (state == State.Swinging)
            {
                state = State.Falling;
            }
        }
    }

    void DoSwingAction()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pendulum.player.velocity += pendulum.player.velocity.normalized * 100;
        }
        // Optional
        // Player can move left and right duiring the swing 
        // if (Input.GetKey(KeyCode.A))
        // {
        //     pendulum.bob.velocity += -cam.transform.right * 1.2f;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     pendulum.bob.velocity += cam.transform.right * 1.2f;
        // }
        transform.localPosition = pendulum.MovePlayer(transform.localPosition, previousPosition, Time.deltaTime);
        previousPosition = transform.localPosition;
    }

    void DoFallingAction()
    {
        pendulum.arm.length = Mathf.Infinity;
        transform.localPosition = pendulum.Fall(transform.localPosition, Time.deltaTime);
        previousPosition = transform.localPosition;

        // S -> trigger slow motion 
        if (Input.GetKey(KeyCode.S) && slowMotionCounts < maxCountsOfSlowMotion) {
            timeManager.DoSlowMotion();
            slowMotionCounts++;
            slowText.text = "Count: " + (maxCountsOfSlowMotion - slowMotionCounts).ToString();
        }
    }

    void DoWalkingAction()
    {
        pendulum.player.velocity = Vector3.zero;
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0.0f;
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Respawn")
        {
            //if too far from arena, reset level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // trigger next scene        
        if(hit.gameObject.name == "END") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        Vector3 undesiredMotion = collision.contacts[0].normal * Vector3.Dot(pendulum.player.velocity, collision.contacts[0].normal);
        pendulum.player.velocity = pendulum.player.velocity - (undesiredMotion * 1.2f);
        hitPos = transform.position;

        if (collision.gameObject.name == "Respawn")
        {
            //if too far from arena, reset level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // trigger nexct scene
        if (collision.gameObject.name == "END")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    // format the time to more readable form 
    string FormatTime(float timeInSeconds) {
        return string.Format("{0}:{1:00}", Mathf.FloorToInt(timeInSeconds/60), Mathf.FloorToInt(timeInSeconds % 60));
    }
}

