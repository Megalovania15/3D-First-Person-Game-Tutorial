using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform movePoint;

    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", false);
        //anim.SetTrigger("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = movePoint.position;

        if (Vector3.Magnitude(transform.position - movePoint.position) >= 0.6f)
        {
            Debug.Log(Vector3.Magnitude(transform.position - movePoint.position));
            anim.SetBool("isWalking", true);
            //anim.SetTrigger("isWalking");
        }
        else
        {
            Debug.Log("character stopped");
            Debug.Log(Vector3.Magnitude(transform.position - movePoint.position));
            anim.SetBool("isWalking", false);
            //anim.SetTrigger("isWalking");
        }
    }
}
