using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public class TableState 
    {
        public Transform TablePos;
        public bool HasCustomer;
    }

    public List<CustomerController> CustomerPerfab;

    public List<TableState> TableStates = new List<TableState> ();

    public List<Transform> TablePos;

    public bool ControlCustomerLoad = true;

    public Transform CustomOriginPos;

    void Start()
    {
        foreach (var t in TablePos) 
        {
            TableState newTable= new TableState ();
            newTable.TablePos = t.transform;
            newTable.HasCustomer = false;

            TableStates.Add(newTable);
        }

        StartCoroutine(LoadCustomer());
    }
    
    void Update()
    {
        
    }
    public int TableIndex;
    public Transform GetCustomPos() 
    {
       
        for (int i = 0; i< TableStates.Count; i++) 
        {
            if (!TableStates[i].HasCustomer) 
            {
                TableStates[i].HasCustomer = true;
                TableIndex = i + 1;
                return TableStates[i].TablePos;
            }
        }
        return CustomOriginPos;
    }
    
    public void ResetTableState(int tableindex) 
    {
        TableStates[tableindex - 1].HasCustomer = false;
    }
    IEnumerator LoadCustomer() 
    {
        yield return new WaitForSeconds(1);
        while (ControlCustomerLoad) 
        { 
            yield return new WaitForSeconds(LevelManager.Instance.CustomerGeneratingTime); 
            int TmpCustomerIndex = Random.Range(0, CustomerPerfab.Count);

            CustomerController NewCustomerPerfab = Instantiate(CustomerPerfab[TmpCustomerIndex]);

            NewCustomerPerfab.transform.position = CustomOriginPos.position;

            Transform pos = GetCustomPos();

            if (pos == CustomOriginPos)
            {
                NewCustomerPerfab.customerState = CustomerController.CustomerState.WaitingForSeat;
            }
            else 
            {
                NewCustomerPerfab.customerState = CustomerController.CustomerState.Walking;

                NewCustomerPerfab.TablePos = pos;
                NewCustomerPerfab.TableIndex = TableIndex;
                NewCustomerPerfab.UpdateState();
            }

        }
    }
}
