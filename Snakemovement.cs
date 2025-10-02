using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;

public class Snakemovement : MonoBehaviour
{

    Vector3 Snakedirection = Vector3.right;
    List<Vector3> pos_of_seg = new List<Vector3>(); 
    List<GameObject> seg = new List<GameObject>();
    public GameObject segment;
    Quaternion rotation = Quaternion.Euler(0, 0, 0);
    public GameObject coin;
    bool spawn_seg = false;
    bool can_move = false;
    void Start()
    {
        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x), Mathf.Round(this.transform.position.y),0);
        pos_of_seg.Add(this.transform.position);
        for (int i = 1;i < 2; i++)
        {
            generate_seg(new Vector3(this.transform.position.x + i*-1,this.transform.position.y,0));
        }
        spawn_coin();
    }

    // Update is called once per frame
    void Update()
    {
        Inputsys();
    }
    private void move_snake()
    {
        if (can_move == false){
            return;
        }
        Vector3 head_snake = this.transform.position;
        this.transform.position += Snakedirection;
        this.transform.rotation = rotation;
        for (int i = 0;i < seg.Count; i++)
        {
            pos_of_seg[i + 1] = seg[i].transform.position;
        }
        for (int i = 0; i < seg.Count; i++)
        {
            if (i == 0)
            {
                seg[i].transform.position = head_snake;
            }
            else
            {
                seg[i].transform.position = pos_of_seg[i];
            } 
        }
        if (spawn_seg)
        {
            generate_seg(pos_of_seg[pos_of_seg.Count - 1]);
            spawn_seg = false;
        }
    }
    private void FixedUpdate()
    {
        move_snake();
        check_range();
    }
    private void Inputsys()
    {
        if (Input.GetKeyDown(KeyCode.W) && Snakedirection != Vector3.down)
        {
            Snakedirection = Vector3.up;
            rotation = Quaternion.Euler(0, 0, 0);
            can_move = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && Snakedirection != Vector3.up)
        {
            Snakedirection = Vector3.down;
            rotation = Quaternion.Euler(0, 0, 180);
            can_move = true;
        }
        if (Input.GetKeyDown(KeyCode.A) && Snakedirection != Vector3.right)
        {
            Snakedirection = Vector3.left;
            rotation = Quaternion.Euler(0, 0, 90);
            can_move = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && Snakedirection != Vector3.left)
        {
            Snakedirection = Vector3.right;
            rotation = Quaternion.Euler(0, 0, -90);
            can_move = true;
        }
    }
    void generate_seg(Vector3 pos)
    {
        GameObject snake_seg_gb = Instantiate(segment, pos, Quaternion.identity);
        seg.Add(snake_seg_gb);
        pos_of_seg.Add(snake_seg_gb.transform.position);
    }
    void check_range()
    {
        if (this.transform.position.y >= 5 || this.transform.position.y <= -5 || this.transform.position.x >= 10 || this.transform.position.x <= -10)
        {
            game_over();
        }
    }
    void game_over()
    {
        Debug.Log("game over");
    }
    void spawn_coin()
    {
        while (true)
        {
            Vector3 coin_pos = new Vector3(UnityEngine.Random.Range(-8, 8), UnityEngine.Random.Range(-3, 3), 0);
            if (pos_of_seg.Contains(coin_pos) == false)
            {
                Instantiate(coin, coin_pos, Quaternion.identity);
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            spawn_coin();
            spawn_seg = true;
        }
    }
}
