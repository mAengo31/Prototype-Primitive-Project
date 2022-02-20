using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public GameObject projectilePrefab;

    Rigidbody rb;

    public float speed = 10f;
    private float InputX;
    private float InputY;
    bool jumpDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxisRaw("Vertical");
        InputY = Input.GetAxisRaw("Horizontal");
        
        transform.Translate(Vector3.forward * InputX * Time.deltaTime * speed);
        transform.Translate(Vector3.right * InputY * Time.deltaTime * speed);


        //reset input
        jumpDown = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //spawn projectile from player pos
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }

   

}
