    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public class MoveTo : MonoBehaviour 
    {
       
       public Transform[] goals;
       Transform closestGoal = null;
       
       

       
       void Start () 
       {
         
        
       }

       void Update()
       {
            
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
   
            float closestDistance = float.MaxValue;

            for (int i = 0; i < goals.Length; i++)
            {
               NavMeshPath Path = new NavMeshPath();

              
               if (NavMesh.CalculatePath(transform.position, goals[i].position, agent.areaMask, Path))
               {
                  
    
                  float distance = Vector3.Distance(transform.position, Path.corners[0]);

                  for (int j =1; j< Path.corners.Length; j++)
                  {
                     distance += Vector3.Distance(Path.corners[j-1], Path.corners[j]);
                  }

                  if (distance < closestDistance)
                  {
                     closestDistance = distance;
                     NavMeshPath ShortestPath = Path;
                     closestGoal = goals[i];
                  }
                   
               }
               
            }
            

            agent.stoppingDistance = 0;
            agent.destination = closestGoal.position;
   
            



          }
             
       }
    