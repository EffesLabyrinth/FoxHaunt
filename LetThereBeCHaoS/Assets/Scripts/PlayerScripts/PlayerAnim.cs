using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    PlayerManager manager;
    Animator anim;
    Rigidbody rb;
    [SerializeField] Transform sprites;
    [SerializeField] Transform tailPos;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (rb.velocity.z > 0) anim.Play("player_idle_back_right");
        else if (rb.velocity.z < 0) anim.Play("player_idle_front_right");

        if (rb.velocity.x > 0 && sprites.localScale.x < 0) Flip();
        else if (rb.velocity.x < 0 && sprites.localScale.x > 0) Flip();
    }
    void Flip()
    {
        sprites.localScale = new Vector3(sprites.localScale.x * -1, sprites.localScale.y, sprites.localScale.z);
        tailPos.localPosition = new Vector3(tailPos.localPosition.x * -1, tailPos.localPosition.y, tailPos.localPosition.z);
    }
}
