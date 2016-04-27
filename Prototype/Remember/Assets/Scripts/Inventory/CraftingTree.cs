using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CraftingTree
{
    public static Dictionary<ID, ID[]> Required = new Dictionary<ID, ID[]>();

    public static Dictionary<ID, ID[]> craftingUI = new Dictionary<ID, ID[]>();

    public static void LoadCrafting()
    {
        craftingUI.Add(ID.UIRoot, new ID[] { ID.Tools, ID.Weapons, ID.Armour, ID.Structures });
        craftingUI.Add(ID.Tools, new ID[] { ID.Axe });
        craftingUI.Add(ID.Weapons, new ID[] { ID.Sword });
        craftingUI.Add(ID.Armour, new ID[] {  });
        craftingUI.Add(ID.Structures, new ID[] { });
        craftingUI.Add(ID.Axe, new ID[] { ID.WoodAxe, ID.StoneAxe, ID.IronAxe });
        craftingUI.Add(ID.Sword, new ID[] { ID.WoodSword, ID.StoneSword, ID.IronSword });

        Required.Add(ID.WoodAxe, new ID[] { ID.Stick, ID.Stick, ID.Log });
        Required.Add(ID.StoneAxe, new ID[] { ID.Stick, ID.Stick, ID.Stone });
        Required.Add(ID.IronAxe, new ID[] { ID.Stick, ID.Stick, ID.Iron });
        Required.Add(ID.WoodSword, new ID[] { ID.Stick, ID.Log, ID.Log });
        Required.Add(ID.StoneSword, new ID[] { ID.Stick, ID.Stone, ID.Stone });
        Required.Add(ID.IronSword, new ID[] { ID.Stick, ID.Iron, ID.Iron });
    }
}
