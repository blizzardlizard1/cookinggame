using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        PreLeaving, // angry status
        Leaving
    }
    
    // Customer WaitingForSeat
    public float basePatience = 50f;
    public float patienceDownRate = 1f;
    public float currentPatience;
    
    // Customer Walking
    public float customerLocation; //Need to connect to RestaurantManager.table

    public Transform TablePos;
    public int TableIndex;
    // Customer Ordering 
    // public Order currentOrder;
    public Image ShowCustomerStatusImage;
    public List<Sprite> CustomerStatusImage;

    void Start()
    {
        basePatience = LevelManager.Instance.CustomerWaitingTime;
         
        currentPatience = basePatience;
        animator =this.GetComponent<Animator>();
    }
    
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
                customerState = CustomerState.PreLeaving;
                UpdateState();
            }
        }
        ShowCustomerStatusImage.transform.LookAt(Camera.main.transform.position);
    }

    public void LeaveRestaurant() 
    {
       Destroy(gameObject);
    }
    public void UpdateState()
    {
        switch (customerState)
        {
            //case CustomerState.WaitingForSeat
            case CustomerState.Walking:
                StartCoroutine(MoveTo());
                break;
            case CustomerState.Seated:
                animator.SetTrigger("sit");
                agent.enabled = false;

                gameObject.transform.position = TablePos.position;
                gameObject.transform.rotation = TablePos.rotation;

             customerState = CustomerState.OrderingFood;
                ThinkOrdering();
                Invoke("UpdateState", 5f);
                break;
            case CustomerState.OrderingFood:
                SetOrdering();
                break;
            case CustomerState.WaitingForFood:
                ShowCustomerStatusImage.sprite = CustomerStatusImage[1];
                break;
            case CustomerState.Eating:
                OrderingManager.Instance.CalculateTip(newOrder,currentPatience - basePatience);
                customerState = CustomerState.Leaving;
                ShowCustomerStatusImage.sprite = CustomerStatusImage[2];
                Invoke("UpdateState",5f);
                break;
            case CustomerState.PreLeaving:
                //RestaurantManager.Instance.ResetTableState(newOrder.TableID);
                //OrderingManager.Instance.FinishOrder(newOrder);
                customerState = CustomerState.Leaving;
                Invoke("UpdateState",5f);
                ShowCustomerStatusImage.sprite = CustomerStatusImage[3];
                break;
            case CustomerState.Leaving:
                RestaurantManager.Instance.ResetTableState(newOrder.TableID);
                OrderingManager.Instance.FinishOrder(newOrder);
                LeaveRestaurant();
                break;
        }
    }

    public bool GetFood(int OrderType) 
    {
        if (newOrder.OrderType == OrderType) 
        {

            customerState = CustomerState.Eating;
            UpdateState();
            return true;
        }
        return false;
    }
    
    public OrderingManager.OrderItem newOrder;

    public void ThinkOrdering()
    {
        int orderType = Random.Range(0, 3);
    
        ShowCustomerStatusImage.gameObject.SetActive(true);
        ShowCustomerStatusImage.sprite = CustomerStatusImage[0];
        float OrderTime = Time.time;

        newOrder = new OrderingManager.OrderItem();
        newOrder.OrderType = orderType;
        newOrder.Price= orderType == 0 ?10 : orderType == 1 ? 20 : 40;
        newOrder.OrderTime = OrderTime;
        newOrder.TableID = TableIndex;
    }

    public void SetOrdering() 
    {
        OrderingManager.Instance.SetOrder(newOrder);
        customerState = CustomerState.WaitingForFood;
        UpdateState();
    }

   IEnumerator MoveTo()
    {
        if (agent== null) 
        {
            agent = this.GetComponent<NavMeshAgent>();
        }
        agent.SetDestination(TablePos.position);
     
        while (true) 
        {
            if (Vector3.Distance(this.transform.position, TablePos.position) < 1f)
            {
                break;
            }
            else if (agent.isStopped) 
            {
                break;
            }

            agent.SetDestination(TablePos.position);
            yield return new  WaitForSeconds(1);
        }
        customerState = CustomerState.Seated;
        UpdateState();
    }
}