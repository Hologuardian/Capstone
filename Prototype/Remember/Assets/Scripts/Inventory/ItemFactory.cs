using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ItemFactory
{
    public static Item makeItem(int id, int amount)
    {
        switch(id)
        {
            case 0:
                return new Item() { ID = 0, Icon = Resources.Load<Texture>("One"), MaxStack = 64, StackSize = amount, Name = "Log", Description = "Logs can be found from trees." };
        }
        return null;
    }
}
