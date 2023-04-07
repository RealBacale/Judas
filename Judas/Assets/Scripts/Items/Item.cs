using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string _itemName;
    public string ItemName
    {
        get { return _itemName; }
        set { _itemName = value; }
    }

    private Sprite _itemSprite;
    public Sprite ItemSprite
    {
        get { return _itemSprite; }
        set { _itemSprite = value; }
    }
    
}
