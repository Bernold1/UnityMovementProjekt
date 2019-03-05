using UnityEngine;

namespace Assets.Scripts
{
    public class Controller : PhysicsMovements
    {
        public float movementSpeed = 3;
        public float jumpSpeed = 7;
        // Start is called before the first frame update
        void Start()
        {

        }

        protected override void ComputeVelociy()
        {
            Vector2 move = Vector2.zero;
            //Horizontal
            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = jumpSpeed;
            }
            //Hvis at brugeren slipper hop knappen bliver y velocity halveret så de "stopper" midt hop
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            targetVelocity = move * movementSpeed;
        }
    }
}
