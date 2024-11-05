using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    // Customer Ordering 
    //public Order currentOrder;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.speed = 2f;
        currentPatience = basePatience;
        customerState = CustomerState.WaitingForSeat;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePatience();
        UpdateState();
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

    private void UpdateState()
    {
        switch (customerState)
        {
            //case CustomerState.WaitingForSeat
            //case CustomerState.Walking
            //case CustomerState.Seated
            //case CustomerState.OrderingFood
            //case CustomerState.WaitingForFood
            //case CustomerState.Eating
            //case CustomerState.Billing
            //case CustomerState.Leaving
                
        }
    }

    public void MoveTo()
    {
        //Customer move to certain place.
        //Need to coordinate with RestaurantManager status
    }

}