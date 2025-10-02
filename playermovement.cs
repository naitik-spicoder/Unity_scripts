using UnityEngine;

public class playermovement : MonoBehaviour
{
    public CharacterController controller;
    //reference the controller of character
    public Animator animator;

    public float speed = 6f;// variable which containe speed of player
    public Transform cam;
    float turnsmoothtime = 0.1f;
    float turnsmoothvelocity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //input which take axix if we press left and right or "a" and "d"
        //hence,the value of horizontal is 1 or -1

        float vertical = Input.GetAxisRaw("Vertical");
        //same as horizontal. 

        Vector3 direction = new Vector3(horizontal , 0f, vertical).normalized;
        /*it assigned the new direction in which our player move with adding and 
         subtracting 1 from Transform and hence,our player move
         normalised is slow the speed when we go diagonaly
        */
        if (direction.magnitude >= 0.1f )//change the vector into magnitude
        {
            animator.SetBool("naitik",true);
            float ta = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, ta, ref turnsmoothvelocity, turnsmoothtime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movedirection = Quaternion.Euler(0f, ta, 0f) * Vector3.forward;
            controller.Move(movedirection.normalized * speed * Time.deltaTime);          
        }
    }
}
