using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class Salad : Food
    {
        public override MenuList MenuType => MenuList.Salad;
        public override int CookingState 
        {
            get
            {
                return 0;
            }
        }
    }
}