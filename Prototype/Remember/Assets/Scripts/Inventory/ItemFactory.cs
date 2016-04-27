using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ID
{
    Log,
    Stick,
    Iron,
    Plank,
    Flint,
    Rock,
    Stone
}

public static class ItemFactory
{

    public static Item makeItem(ID id, int amount)
    {
        switch(id)
        {
            case ID.Log:
                return new Item() 
                { ID = id, 
                    Icon = Resources.Load<Texture>("One"), 
                    MaxStack = 64, StackSize = amount, 
                    Name = "Log",
                  Description = "Logs can be found from trees."
                };
            case ID.Stick:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Two"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Stick",
                    Description = "Sticks can be found lying around in forests, or can be harvested from saplings."
                };
            case ID.Iron:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Three"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Log",
                    Description = "Logs can be found from trees."
                };
            case ID.Plank:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Four"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Log",
                    Description = "Logs can be found from trees."
                };
            case ID.Flint:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Five"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Log",
                    Description = "Logs can be found from trees."
                };
            case ID.Rock:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Six"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Log",
                    Description = "Logs can be found from trees."
                };
            case ID.Stone:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("Seven"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Log",
                    Description = "Logs can be found from trees."
                };
        }
        return null;
    }
}
