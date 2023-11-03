using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WolfManager : MonoBehaviour
{
    GameObject player;
    Transform spriteTransform; // Added reference to the sprite's transform

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        spriteTransform = transform.GetChild(0); // Assuming the sprite is the first child of the GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player had died");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();

        // Make the sprite face the camera
        spriteTransform.LookAt(Camera.main.transform);
        spriteTransform.eulerAngles = new Vector3(0, spriteTransform.eulerAngles.y, 0); // Constrain rotation to prevent spinning
    }

    void Patrol()
    {
        if (!walkPointSet) SearchForDest();
        if (walkPointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkPointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
