using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        //
        public float speed;
        public float jumpForce;
        public Transform feetPos;
        public float checkRadius;
        public LayerMask WhatIsGround;
        public float jumpTime;

        private float moveInput;
        private Rigidbody2D rb;
        private bool facingRight = true;
        private bool isGrounded;
        private float jumpTimeCounter;
        private bool isJumping;
        private Animator _animator;


        void Start()
        {
            _animator = GetComponent<Animator>();
            //Allows scripting of the rigidbody2d object
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
   
            //Get the left or right arrow keys
            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput*speed, rb.velocity.y);
            if (moveInput == 0)
            {
                _animator.SetBool("IsRunning", false);
            }
            else
            {
                _animator.SetBool("IsRunning", true);
            }
            /*Hvis at moveInput er positiv, betyder at man prøver at bevæge mod højre hvor x aksen
             vil stige og karakteren ikke vender mod højre så kaldes flip funktionen som udfører vores transform */

            if (facingRight  == false && moveInput > 0)
            {
                Flip();
            }
            //Læs kommentar øverst, bare mod venstre
            else if (facingRight == true && moveInput<0)
            {
                Flip();
            }
        }

        void Update()
        {
            //state position of circle to generate by the players feet
            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, WhatIsGround);
            //Jump time in the inspector allows for how long the player can hold down the space button to jump.
            //add a force to our Y-value on the vector
            if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetTrigger("TakeOff");
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.velocity = Vector2.up * jumpForce;
                
            }

            if (isGrounded == true)
            {
                _animator.SetBool("IsJumping",false);
            }
            else
            {
                _animator.SetBool("IsJumping",true);
            }
            //Only allows player to go higher if is jumping is == true
            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (jumpTimeCounter >0)
                {
                    
                    rb.velocity = Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
                
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }

        void Flip()
        {
            
            facingRight = !facingRight;
            //sets the players xyz values
            Vector3 Scaler = transform.localScale;
            //multiplying the x value by -1 get the opposite value, transforming the x rotation from either 1 to -1
            Scaler.x *= -1;
            //Setting the Scaler value
            transform.localScale = Scaler;
        }

    }
}