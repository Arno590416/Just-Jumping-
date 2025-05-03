using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float inputAxis;
    //public Vector3 velocity;
    public float speed = 8f;
    public float speed_x_constraint ;
    private new Rigidbody2D rigidbody;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;//上去又下来
    public int hp ;
    public float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        hp = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);//向上跳跃的力，冲击力
            //velocity.y = jumpForce;
            //jumping = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddForce(Vector2.right * speed, ForceMode2D.Force);//向右的力，持续力
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddForce(Vector2.left * speed, ForceMode2D.Force);//向左的力，持续力
        }
        if (rigidbody.velocity.x > speed_x_constraint)
        {
            rigidbody.velocity = new Vector2(speed_x_constraint, rigidbody.velocity.y);
        }
        if (rigidbody.velocity.x < -speed_x_constraint)
        {
            rigidbody.velocity = new Vector2(-speed_x_constraint, rigidbody.velocity.y); ;
        }
        /*inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * speed, 3f * speed * Time.deltaTime);//deltatime从上一帧到当前帧的时间，以秒为单位
        this.gameObject.transform.position += velocity * Time.deltaTime;//物体的移动速度
        */
        /*if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }*/
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
           print(collision.gameObject.name);
            hp-= 1;
        }
    }
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            collision.gameObject.GetComponent<Portal>().ChangeScenes();
        }
    }
}
