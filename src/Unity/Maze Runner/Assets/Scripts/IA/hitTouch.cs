using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class hitTouch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter(Collision collision)
    {
   
        if (collision.gameObject.CompareTag("Player"))
        {
      
            collision.gameObject.GetComponent<OrbManager>().ThrowOrb();
    
            //player.updatePosition = false;
            if (collision.gameObject.GetComponent<NavMeshAgent>())
            {
                
                collision.gameObject.GetComponent<NavMeshAgent>().Warp(new Vector3(0, 0, 0));
            }
  
            else
            {
                collision.gameObject.transform.position = new Vector3(0, 0, 0);
            }


        }
    }

}
