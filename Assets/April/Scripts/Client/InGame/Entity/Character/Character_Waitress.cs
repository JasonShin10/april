using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace April
{
    public class Character_Waitress : CharacterBase
    {
        public static List<Character_Waitress> SpawnedWaitlessList = new List<Character_Waitress>();

        
        public override CharacterType CharacterType => CharacterType.Waitress;

        public Transform waitingPosition;
        
        private Customer currentTargetCustomer;
        public WaitressTable WaitressTable;

        private List<Customer> waitingOrderCustomerList;
        private List<Customer> waitingFoodCustomerList;

        private Dish dish;
        protected override void Awake()
        {
            base.Awake();
            SpawnedWaitlessList.Add(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SpawnedWaitlessList.Remove(this);
        }

        [Button("Receive Customer Order")]
        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            this.SetDestination(currentTargetCustomer.transform.position, OnDestinationCustomer);
        }
        
        private void Start()
        {
            waitingOrderCustomerList = IngameWaiterSystem.Instance.waitingOrderCustomerList;
            waitingFoodCustomerList = IngameWaiterSystem.Instance.waitingFoodCustomerList;
            WaitressTable.OnFoodArrived += HandleFoodArrived;
        }

        public void HandleFoodArrived()
        {
            //this.SetDestination(WaitressTable.transform.position,)
        }
        

        public void FindWaitingOrderCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;
                   
                foreach (Customer customer in waitingOrderCustomerList)
                {
                    if ((int)customer.state < minStateValue)
                    {
                        minStateValue = (int)customer.state;
                    }
                    if (customer.patienceSlider.value < minPatienceValue)
                    {
                        minPatiecneCustomer = customer;
                    }
                }
            
            ReceiveCustomerOrder(minPatiecneCustomer);
        }

        public void FindWaitingFoodCustomer()
        {
            float minPatienceValue = float.MaxValue;
            float minStateValue = float.MaxValue;
            Customer minPatiecneCustomer = null;

            foreach (Customer customer in waitingFoodCustomerList)
            {
                if ((int)customer.state < minStateValue)
                {
                    minStateValue = (int)customer.state;
                }
                if (customer.patienceSlider.value < minPatienceValue)
                {
                    minPatiecneCustomer = customer;
                }
            }

            ReceiveCustomerOrder(minPatiecneCustomer);
        }
        protected override void Update()
        {
            base.Update();
            //Debug.Log(customerList.Count);
            // To do : Ray Casting For Customer
            // 1. if RayHit Customer ? => OnDestinationCustomer();
            // 2. Vector3.Distance(currentTargetCustomer.transform.position transform.position) <= 1f ? => OnDestinationCustomer();
            // 3. this.SetDestination(customer.transform.position, OnDestinationCustomer);
        }

        public void RegisterCustomer(Customer customer)
        {
            customer.OnStateChange += HandleStateChange;
        }

        private void HandleStateChange(CustomerState oldState, Customer customer)
        {
            switch (oldState)
            {
                case CustomerState.WaitingOrder:
                    waitingOrderCustomerList.Remove(customer);
                    break;
                case CustomerState.WaitingFood:
                    waitingFoodCustomerList.Remove(customer);
                    break;

            }
            switch (customer.State)
            {
                case CustomerState.WaitingOrder:
                    waitingOrderCustomerList.Add(customer);
                    break;
                case CustomerState.WaitingFood:
                    waitingFoodCustomerList.Add(customer);
                    break;
                case CustomerState.Leaving:
                    break;

            }
        }
        private void OnDestinationCustomer()
        {
            if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(null);
                currentTargetCustomer = null;

                this.SetDestination(waitingPosition.position);
            }
        }

        private void OnWaitressTable()
        {
            currentTargetCustomer.Interact(null);
            
        }
    }

    //private void OnDestinationWaitressTable()
    //{
        
    //}
}

