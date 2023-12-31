using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameCustomerFactorySystem : MonoBehaviour
    {
        public static IngameCustomerFactorySystem Instance { get; private set; }

        public Customer customerPrefab;
        public List<VisualizationCharacter> customerVisualizationList = new List<VisualizationCharacter>();
        public int isGroup;

        public Transform spawnPoint;
        [MinMaxSlider(2, 10, true)]
        public Vector2Int npcGroupSpawnRange = new Vector2Int(2, 10);

        public Character_Waitress waitress;

        private float currentTime;
        private float spawnMaxTime = 20f;
        private float spawnDecreaseTime = 3f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpawnCustomer();
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        public int GetRandom()
        {
            int randomNum = Random.Range(0, customerVisualizationList.Count);
            return randomNum;
        }

        [Button("Spawn Customer")]
        public void SpawnCustomer()
        {
            if (IngameCustomerWaitingSystem.Instance.IsFullWaitingSlot)
                return;

            int rand = Random.Range(0, 2);
            bool isGroupSpawn = rand % 2 == 0;
            int spawnCount = 1;
            if (isGroupSpawn)
            {
                spawnCount = Random.Range(npcGroupSpawnRange.x, npcGroupSpawnRange.y + 1);
            }

            if (spawnCount > IngameCustomerWaitingSystem.Instance.EmptyWaitingSlotCount)
                return;

            if (IngameCustomerWaitingSystem.Instance.TryGetEmptySlots(spawnCount, out int groupId, out var waitingSlots))
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    int num = GetRandom();
                    Customer customerInstance = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                    customerInstance.gameObject.SetActive(true);
                    customerInstance.isGroup = isGroupSpawn;
                    customerInstance.CustomerJobType = CustomerJobTypes.Doctor;

                    VisualizationCharacter visualCharacter = Instantiate(customerVisualizationList[num], customerInstance.graphicRoot);
                    visualCharacter.gameObject.SetActive(true);
                    customerInstance.visualization = visualCharacter;

                    customerInstance.waitingPos = waitingSlots[i].transform;

                    customerInstance.groupID = groupId;
                    customerInstance.SetCustomerState(CustomerState.Waiting);

                    customerInstance.exitTarget = this.transform;
                    waitingSlots[i].customer = customerInstance;
                }
                IngameCustomerWaitingSystem.Instance.CheckWaitingCustomerPossibleEnter();
            }
        }

        void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime > spawnMaxTime)
            {
                SpawnCustomer();
                currentTime = 0;
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            Destroy(customer.gameObject);
        }

        public void MakeSpawnFast()
        {
            spawnMaxTime -= spawnDecreaseTime;
        }
    }
}

