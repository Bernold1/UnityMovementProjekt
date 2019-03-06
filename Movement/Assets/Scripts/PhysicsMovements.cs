using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhysicsMovements : MonoBehaviour
    {
        //For at kunne lave whacky ting med gravity måske senere i forløbet.
        public float gravityModification = 1f;
        public float standardGroundY = 0.65f;
        //Andre klasser kommer til at arve Physics klassen, derfor laver vi den private, så det kan tilgås men selvfølgelig ikke ændres
        protected Vector2 targetVelocity;
        protected Vector2 groundNormal;
        protected Vector2 velocity;
        protected Rigidbody2D rb2d;
        protected ContactFilter2D conctactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
        protected bool isGrounded;

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;


        void OnEnable()
        {
            //Get and store component reference
            rb2d = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {
            conctactFilter.useTriggers = false;
            conctactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            conctactFilter.useLayerMask = true;
        }

        // Update is called once per frame
        void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelociy();
        }

        protected virtual void ComputeVelociy()
        {

        }

        void FixedUpdate()
        {
            //Time.deltatime bruges for at flytte en objekt i spillet i y-retningen med størrelsen af gravityModificaton pr. sekund. 

            velocity += gravityModification * Physics2D.gravity * Time.deltaTime;//Bestemmer hastigheden  //Time.deltatime bruges for at flytte en objekt
            //i spillet i y-retningen med størrelsen af gravityModificaton pr. sekund. 
            velocity.x = targetVelocity.x;


            isGrounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime; //opretter en ny vector og Bestemmer hvor vores objekt vil være baseret efter bevægelsen fra tyngdekraften

            Vector2 moveground = new Vector2(groundNormal.y, -groundNormal.x); //Allows walking on slops

            Vector2 move = moveground * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;//beregner bevægelse 

            Movement(move, true); //Kalder bevægelses funktionen
        }

        void Movement(Vector2 move, bool yMovement)
        {
            float distance = move.magnitude;
            if (distance > minMoveDistance)
            {
                int count = rb2d.Cast(move, conctactFilter, hitBuffer, distance + shellRadius);
                hitBufferList.Clear();
                for (int i = 0; i < count; i++)
                {
                    hitBufferList.Add(hitBuffer[i]);
                }

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    Vector2 Normal = hitBufferList[i].normal;
                    //check if standing on ground
                    if (Normal.y > standardGroundY)
                    {
                        isGrounded = true;
                        if (yMovement)
                        {
                            groundNormal = Normal;
                            Normal.x = 0;
                        }
                    }
                    //getting difference between velocity and normal, to decide weather or not we need to subtract from our velocity to prevent the player from entering into another collider
                    float movementprojection = Vector2.Dot(velocity, Normal);
                    if (movementprojection < 0)
                    {
                        velocity = velocity - movementprojection * Normal;
                    }

                    float modifiedDistance = hitBufferList[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            //move object based on calculated values, by setting the position of the objects rigidbody2d
            //add movement vector to position of rigidbody2d object every frame
            rb2d.position = rb2d.position + move.normalized * distance;

        }
    }
}