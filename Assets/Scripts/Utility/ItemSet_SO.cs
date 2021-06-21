using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSet", menuName = "Scriptable Objects/Item set")]
public class ItemSet_SO : ScriptableObject
{
    public List<Item> items;
}
