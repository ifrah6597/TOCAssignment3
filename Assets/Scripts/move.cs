using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class move : MonoBehaviour
{

    public NavMeshAgent agent = null;

    [SerializeField] float turnSpeed =45.0f;
    [SerializeField] float speed = 20.0f;
    private float horizontalInput;
    private float forwardInput;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        
        agent.transform.Translate(Vector3.forward * speed * Time.deltaTime * forwardInput);
        agent.transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
           
        
    }
}
