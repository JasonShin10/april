using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace April
{
    public abstract class Food : InteractionItem
    {
        public abstract MenuList MenuType { get; }
        public abstract int CookingState { get; }
        public float progressValue;

        public float offsetOnDish;

        
    }
}
