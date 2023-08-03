using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerTable_InteractSlot : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;

        public CustomerTable parentTable;
        public bool customerAssigned;
        public InteractionItem item;


        public override void Exit()
        {

        }

        public override void Interact(PlayerController player)
        {
            if (item == null)
                return;

            player.item = item;
            player.item.transform.SetParent(player.transform);
            player.item.transform.localPosition = Vector3.up + Vector3.forward;
        }
    }
}