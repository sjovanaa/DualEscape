using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Transform grondCheckTransform = null;//stopala igraca
    [SerializeField] private LayerMask playerMask;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody rb;
    private int superJumpsRemaining;
    
    //private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    //is called once every phisycs update
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(horizontalInput, rb.linearVelocity.y, 0);

        /*if(!isGrounded)
        {
            return;
        }*/

        //if(Physics.OverlapSphere(grondCheckTransform.position,0.1f).Length == 1)// jer se uvek colliduje sa samim sobom

        if (Physics.OverlapSphere(grondCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (jumpKeyWasPressed)
        {
            float jumpPower = 5f;
            if(superJumpsRemaining>0)
            {
                jumpPower *= 2;
                superJumpsRemaining--;
            }
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

        
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded= false;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }
}
