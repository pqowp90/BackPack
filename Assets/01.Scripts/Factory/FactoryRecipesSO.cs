using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public Item item;
    public int count;
    public Recipe(Item _item, int _count)
    {
        item = _item;
        count = _count;
    }
}


[CreateAssetMenu( fileName = "FactoryRecipe", menuName = "Factorys/FactoryRecipes" )]
public class FactoryRecipesSO : ScriptableObject
{
    public string recipeName;
    public FactoryType factoryType;
    public int costTime;
    public List<Recipe> ingredients;
    public List<Recipe> result;
}
[CreateAssetMenu( fileName = "FactoryRecipes", menuName = "Factorys/FactoryRecipesSOs" )]
public class FactoryRecipeConstruct : ScriptableObject
{
    public List<FactoryRecipesSO> factoryRecipesSOs;
}