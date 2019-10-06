using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanScript : MonoBehaviour
{
    private Animator anim;
    private int jumpHash = Animator.StringToHash("jump");
    int walkStateHash = Animator.StringToHash("Base Layer.Walk");
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move);

        AnimaterStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == walkStateHash)
        {
            anim.SetTrigger(jumpHash);
        }
    }
}
