    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public class MoveToMino : MonoBehaviour 
    {
       
       public Transform goal;


      // animator to switch from movements to attacks
      private Animator _animator = null;

       
       void Start () 
       {
         _animator = GetComponent<Animator>();
     
        
       }

       void Update()
       {

        

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
   
            float closestDistance = 0;

  
            NavMeshPath Path = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, goal.position, agent.areaMask, Path))
            {
               closestDistance = Vector3.Distance(transform.localPosition, goal.position);
               agent.destination = goal.position;
            }
            else
            {
               agent.destination = new Vector3(0,0,0);
               
            }
            
            
            agent.updateRotation = true;
            agent.stoppingDistance = 1f;
            
            if (closestDistance < 7f)
            {
               _animator.SetBool("_isInRange", true);
            }
            else
            {
             
               _animator.SetBool("_isInRange", false);
            }
          }
             
       }
    