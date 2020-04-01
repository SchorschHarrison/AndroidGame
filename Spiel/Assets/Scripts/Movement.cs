using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    
    [SerializeField]
    private Transform targetBlock;
   
    [SerializeField]
    float movementspeed;

    [SerializeField]
    float jumpForce = 20f;

    [SerializeField]
    float gravity = -9.81f;


    CharacterController characterController;


    float gravityMultiplier = 1;
    Vector2 targetFlatPos;




    Vector3 forces;
    Vector3 velocity;

    [SerializeField]
    bool jumping;




   


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumping = false;
        velocity = new Vector3(0, 0, 0);
        //Physics.IgnoreLayerCollision(9, 10);
    }

    // Update is called once per frame
    void Update()
    {
        forces = new Vector3();
        Vector2 flatPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetFlatPos = new Vector2(targetBlock.position.x, targetBlock.position.z);
        Vector3 desiredDir = targetBlock.position - transform.position;
        desiredDir.y = 0;
        Vector3 movementDir = desiredDir.normalized;




        if(PlayerInput.current.jumpPressed && !jumping && GameStateManager.current.state != GameStateManager.LOSESTATE)
        {
            jumping = true;
            velocity.y = jumpForce;
            gravityMultiplier = 0.5f;
        }
        else if(PlayerInput.current.jumpPressed && jumping && velocity.y > 0)
        {
            gravityMultiplier = 0.5f;
        }
        else
        {
            gravityMultiplier = 1;
        }

        if (!characterController.isGrounded)
        {
            forces += Vector3.up * gravityMultiplier * gravity * Time.deltaTime;
        }
        else
        {
            jumping = false;
        }


        if (desiredDir.magnitude <= (movementDir.normalized * movementspeed * Time.deltaTime).magnitude)
        {
            GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>().PlayerTargetReached();
        }

        movementDir = movementDir.normalized * movementspeed;
        velocity.x = movementDir.x;
        velocity.z = movementDir.z;

        velocity += forces;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void setTarget(Transform target)
    {
        this.targetBlock =  target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), collision.collider);
        }
    }
}
