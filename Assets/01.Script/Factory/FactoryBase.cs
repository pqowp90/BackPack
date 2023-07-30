using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactoryType
{
    Combiner,
    Foundry,
    SteelWorks,
}
public class FactoryBase : MonoBehaviour
{
     private Recipe curRecipe;
}
