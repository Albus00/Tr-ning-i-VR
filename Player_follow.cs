using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_follow : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {

        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
        }
    }

}
