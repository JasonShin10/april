using April;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitressTable : InteractionBase
{
    public override bool IsAutoInteractable => false;
    public override InteractionObjectType InterationObjectType => InteractionObjectType.WaitressTable;
    private PlayerController player;
    private InteractionItem item;
    public float offset;
    public event Action OnFoodArrived;
    // Start is called before the first frame update
    void waitTressTableInteract()
    {
        if (player.item != null)
        {
            if (player.item is Dish)
            {
                if (item is Food)
                {
                    var dish = player.item as Dish;
                    var food = item as Food;
                    dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                    item = null;
                    
                    return;
                }
                else
                {
                    item = player.item;
                    var dish = item as Dish;
                    dish.transform.SetParent(this.transform);
                    dish.transform.localPosition = new Vector3(0, offset, 0);
                    player.item = null;
                    OnFoodArrived?.Invoke();
                }
            }
            else
            {
                if (item is Dish)
                {
                    var dish = item as Dish;
                    var food = player.item as Food;
                    dish.AddItem(food, new Vector3(0, food.offsetOnDish, 0));
                    player.item = null;
                }
                else
                {
                    if (item == null)
                    {
                        item = player.item;
                        item.transform.SetParent(this.transform);
                        item.transform.localPosition = new Vector3(0, offset, 0);
                        player.item = null;
                        Debug.Log("Food Insert To Table!");
                    }

                }

            }
        }
        else
        {
            if (item != null)
            {
                player.item = item;
                item.transform.SetParent(player.transform);
                item.transform.localPosition = Vector3.up + Vector3.forward;
                item = null;
            }
        }
    }

    public override void Interact(PlayerController player)
    {
        this.player = player;
        waitTressTableInteract();
        //var interactUI = UIManager.Show<InteractionUI>(UIList.InteractionUI);
        //interactUI.InitActions(interactActionDatas);
    }

    public override void Exit()
    {

    }
}