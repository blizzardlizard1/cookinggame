using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerController : MonoBehaviour
{
    //Use NavMesh for PathFinding System
    private NavMeshAgent agent;
    private Animator animator;
    
    public CustomerState customerState;
    public enum CustomerState
    {
        WaitingForSeat,
        Walking,
        Seated,
        OrderingFood,
        WaitingForFood,
        Eating,
        Billing,
        Leaving
    }
    
    // Customer WaitingForSeat
    public float basePatience = 500f;
    public float patienceDownRate = 1f;
    public float currentPatience;
    
    // Customer Walking
    public float customerLocation; //Need to connect to RestaurantManager.table

    public Vector3 TablePos;
    // Customer Ordering 
    //public Order currentOrder;
    
    
    // Start is called before the first frame update
    void Start()
    {
      
        //agent.speed = 2f;
        currentPatience = basePatience;
       // customerState = CustomerState.WaitingForSeat;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePatience();
       
    }

    private void UpdatePatience()
    {
        if (customerState == CustomerState.WaitingForSeat || customerState == CustomerState.WaitingForFood)
        {
            currentPatience -= patienceDownRate * Time.deltaTime;
            if (currentPatience <= 0)
            {
                //LeaveRestaurant()
            }
        }
    }

    public void UpdateState()
    {

        switch (customerState)
        {
            //case CustomerState.WaitingForSeat
            case CustomerState.Walking:
              StartCoroutine(MoveTo());
                break;
            //case CustomerState.Seated
            //case CustomerState.OrderingFood
            //case CustomerState.WaitingForFood
            //case CustomerState.Eating
            //case CustomerState.Billing
            //case CustomerState.Leaving
                
        }
    }

   IEnumerator MoveTo()
    {
        if (agent==null) 
        {
            agent = this.GetComponent<NavMeshAgent>();
        }
        agent.SetDestination(TablePos);
        //Customer move to certain place.
        //Need to coordinate with RestaurantManager status
        while (true) 
        {
            if(Vector3.Distance(this.transform.position, TablePos)<0.15f)
            {
                break;
            }

            agent.SetDestination(TablePos);
            yield return new  WaitForSeconds(1);
        }


        customerState = CustomerState.Seated;
        UpdateState();


    }

}