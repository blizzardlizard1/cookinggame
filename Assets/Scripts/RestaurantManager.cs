using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    // a queue of waiting customers
    // FUNCTION: Add one of the waiting customer to an available seat
    // FUNCTION: Remove one seated customer from the seat
    // FUNCTION: Manage the waitingCustomer queue

    public class Tablestate 
    {
        public Vector3 TeablePos;
        public bool HaaCustomer;
    }

    public CustomerController CustomerPerfab;

    public List<Tablestate> Tablestates = new List<Tablestate> ();

    public List<Transform> TablePos;

    public bool ControlCustomerLoad = true;

    public Transform CustomOriginpos;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var t in TablePos) 
        {
            Tablestate newTable= new Tablestate ();
            newTable.TeablePos = t.transform.position;
            newTable.HaaCustomer = false;

            Tablestates.Add (newTable);
        }

        StartCoroutine(LoadCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetCustomPos() 
    {
       
        for (int i=0;i< Tablestates.Count;i++) 
        {
            if (!Tablestates[i].HaaCustomer) 
            {
                Tablestates[i].HaaCustomer = true;

                return Tablestates[i].TeablePos;
            }
        }
       

        return CustomOriginpos.position;
    }

    IEnumerator LoadCustomer() 
    {
        while (ControlCustomerLoad) 
        {
        
            yield return new WaitForSeconds(5);

            CustomerController NewcustomerPerfab = Instantiate(CustomerPerfab);

            NewcustomerPerfab.transform.position = CustomOriginpos.position;

            Vector3 pos = GetCustomPos();

            if (pos == CustomOriginpos.position)
            {
                NewcustomerPerfab.customerState = CustomerController.CustomerState.WaitingForSeat;
            }
            else 
            {
                NewcustomerPerfab.customerState = CustomerController.CustomerState.Walking;

                NewcustomerPerfab.TablePos = pos;

                NewcustomerPerfab.UpdateState();
            }

        }
    }
}
