using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ID
{
    UIRoot,

    Tools,
    Axe,
    WoodAxe,
    StoneAxe,
    IronAxe,

    Weapons,
    Sword,
    WoodSword,
    StoneSword,
    IronSword,

    Armour,
    Chestplate,
    Helmet,

    Structures,

    Log,
    Iron,
    Stone,
    Stick,
    Plank,
    Flint,
    Rock,
}

public static class ItemFactory
{

    public static Item makeItem(ID id, int amount)
    {
        switch(id)
        {
            case ID.UIRoot:
                return new Item()
                {
                    ID = id,
                    Icon = null,
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.Tools:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/Tools"),
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.Weapons:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/Weapons"),
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.Armour:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/Armor"),
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.Structures:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/Building"),
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.Axe:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/IronAxe"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "",
                    Description = ""
                };
            case ID.Sword:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/IronSword"),
                    MaxStack = 0,
                    StackSize = 0,
                    Name = "",
                    Description = ""
                };
            case ID.WoodSword:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/woodSword"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Wooden Sword",
                    Description = "A sword made out of wood, not very strong."
                };
            case ID.StoneSword:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/stoneSword"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Stone Sword",
                    Description = "A sword made out of stone."
                };
            case ID.IronSword:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/ironSword"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Iron Sword",
                    Description = "An iron sword! Now you can slay monsters effectively!"
                };
            case ID.WoodAxe:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/woodAxe"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Wooden Axe",
                    Description = "Used for cutting down trees."
                };
            case ID.StoneAxe:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/stoneAxe"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Stone Axe",
                    Description = "Used for cutting down trees."
                };
            case ID.IronAxe:
                return new Item()
                {
                    ID = id,
                    Icon = Resources.Load<Texture>("icons/ironAxe"),
                    MaxStack = 1,
                    StackSize = amount,
                    Name = "Iron Axe",
                    Description = "Used for cutting down trees."
                };
            case ID.Log:
                return new Item() 
                { ID = id,
                  Icon = Resources.Load<Texture>("icons/Wood"), 
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
                    Icon = Resources.Load<Texture>("icons/ironBar"),
                    MaxStack = 64,
                    StackSize = amount,
                    Name = "Iron",
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
