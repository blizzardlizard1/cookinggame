using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderImage : MonoBehaviour
{
    public TMP_Text TableID;
    public TMP_Text OrderType;
    public TMP_Text Price;
    
    public void ShowOrder(OrderingManager.OrderItem Item)
    {
        TableID.text = $"TableID: {Item.TableID.ToString()}";

        string tempType = Item.OrderType == 0 ? "Small" : Item.OrderType == 1 ? "Medium" : "Large";

        OrderType.text = $"OrderType: {tempType}";

        Price.text = $"Price: {Item.Price}";
    }
}
