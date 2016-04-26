using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ItemFactory
{
    public static Item makeItem(uint id)
    {
        throw new KeyNotFoundException();
    }
}
