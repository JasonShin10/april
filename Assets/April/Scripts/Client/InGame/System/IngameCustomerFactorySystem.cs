using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerFactorySystem : MonoBehaviour
    {
        public static IngameCustomerFactorySystem Instance { get; private set; }

        //public Dictionary<int, List<Customer>> waitingCustomersNum = new Dictionary<int, List<Customer>>();
        public int isGroup;
        public Customer customerPrefab;
        public Transform spawnPoint;



        [MinMaxSlider(2, 10, true)]
        public Vector2Int npcGroupSpawnRnage = new Vector2Int(2, 10);

        public List<CustomerTable> tables = new List<CustomerTable>();
        //public List<Customer> waitingCustomers = new List<Customer>();

        public bool allAsigned;
        private float currentTime;
        private float maxTime = 10f;
        public int waitingNum;

        private void Awake()
        {
            Instance = this;
            customerPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            SpawnCustomer();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SpawnCustomer()
        {
            if (IngameCustomerWaitingSystem.Instance.IsFullWaitingSlot)
                return;

            allAsigned = true;

            int rand = Random.Range(0, 2);
            bool isGroupSpawn = rand % 2 == 0;

            int spawnCount = 1;
            if (isGroupSpawn)
            {
                spawnCount = Random.Range(npcGroupSpawnRnage.x, npcGroupSpawnRnage.y + 1);
            }

            for (int i = 0; i < spawnCount; i++)
            {
                Customer customerInstance = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                customerInstance.gameObject.SetActive(true);
                foreach (CustomerTable table in tables)
                {
                    if (!table.customerAssigned)
                    {
                        allAsigned = false;
                    }
                }

                customerInstance.isGroup = isGroupSpawn;
                //if (isGroup == 1)
                //{
                //    customerInstance.isGroup = true;
                //}

                if (allAsigned == true)
                {
                    //waitingCustomers.Add(customerInstance);
                    foreach(IngameCustomerWaitingSystem_SlotData slot in IngameCustomerWaitingSystem.Instance.waitingSlots)
                    {
                        if (slot.customer == null)
                        {
                        slot.customer = customerInstance;
                        slot.customer.waitingPos = slot.transform;
                        slot.customer.waitingNum = waitingNum;
                        slot.customer.currentCustomerState = Customer.CustomerState.Waiting;
                       
                            //slot.customer.SetCustomerState(Customer.CustomerState.Waiting);
                        //customerInstance.waiting = true;
                        break;
                        }
                    }
                }

                //FindTarget(customerInstance);
                customerInstance.exitTarget = this.transform;
                // customerInstance.tables = tables;

                var randomColor = new Color();
                randomColor.r = Random.Range(0, 256) / 255f;
                randomColor.g = Random.Range(0, 256) / 255f;
                randomColor.b = Random.Range(0, 256) / 255f;
                randomColor.a = Random.Range(0, 256) / 255f;

                customerInstance.GraphicColor = randomColor;

            }

            if (allAsigned == true)
            {
                //waitingCustomersNum[waitNum] = new List<Customer>(waitingCustomers);
                waitingNum++;
            }
        }

        void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxTime)
            {
                SpawnCustomer();
                currentTime = 0;
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            Destroy(customer.gameObject);
        }
    }
}
