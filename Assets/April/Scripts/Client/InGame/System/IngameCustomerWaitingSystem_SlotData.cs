using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerWaitingSystem_SlotData : MonoBehaviour
    {
        public bool IsExistCustomer => customer != null;

        public Customer customer;

    }
}