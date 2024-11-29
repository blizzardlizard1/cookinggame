using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderingManager : MonoBehaviour
{
    public static OrderingManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<OrderImage> MenuItems = new List<OrderImage> ();
 
    public OrderImage OrderImage;

    public GameObject OrderShowPos;

    public float preparationTime;

    public bool isComplete;

    public float Tip;

    public float AllMoney;
    public int PeopleServed;

    public TMP_Text MoneyText;
    public TMP_Text PeopleServedText;
    public class OrderItem
    {
       public int OrderType; 
       public int TableID;   
       public float OrderTime;
       public float Price;
     
    }
    public List<OrderItem> Orders = new List<OrderItem> ();

    // Money = orderPrice + tip * randomness
    public void CalculateTip(OrderItem order, float waitingTime)
    {
        int Randomness = UnityEngine.Random.Range(1, 5);
        float tip = MathF.Ceiling(waitingTime * 0.1f + order.Price * (float)Randomness / 10); //negative waitingTime

        //Debug.Log("Current Tip = " + tip);
        AllMoney += order.Price;
        AllMoney += tip;

        MoneyText.text = $"{AllMoney}";

        PeopleServed++;
        PeopleServedText.text = PeopleServed.ToString();
    }

    public void SetOrder(OrderItem order) 
    {
        Orders.Add(order);

        OrderImage orderDetail =  Instantiate(OrderImage, OrderShowPos.transform);

        orderDetail.ShowOrder(order);
     

        MenuItems.Add(orderDetail);
    }

    public void FinishOrder(OrderItem order) 
    {
        if (Orders.Contains(order)) 
        {
            int orderIndex = Orders.IndexOf(order);
            Destroy(MenuItems[orderIndex].gameObject);
            MenuItems.RemoveAt(orderIndex);
            Orders.Remove(order);
        }
    }

}
