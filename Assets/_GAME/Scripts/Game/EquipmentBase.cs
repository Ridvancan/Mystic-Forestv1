using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentLevel
{
    level1,
    level2,
    level3
}
public enum EquipmentType
{
    armor,
    weapon
}
public abstract class EquipmentBase : MonoBehaviour
{
    public EquipmentLevel equipmentLevel;
    public EquipmentType equipmentType;
    public int value;
}
