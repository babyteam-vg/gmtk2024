using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DragonType
{
    Rojo,
    Verde,
    Azul,
    Morado,
    Negro,
    Blanco,
    Bronce,
    Plata,
    Oro,
    Pezcado
}

[CreateAssetMenu]
public class DragonData : ScriptableObject
{
    public DragonType type;
    public float sleepTotalTime;
    public DragonController prefab;
    public ItemDescription item;
    public float scaleTime;
}
