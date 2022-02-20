using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityOrbit : MonoBehaviour
{

    public float Gravity;

    public bool FixedDirection = true; //if the gravity of this section is pulling player down

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<gravityControl>())
        {
            //if this object has gravity script, set this as planet
            other.GetComponent<gravityControl>().Gravity = this.GetComponent<gravityOrbit>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
