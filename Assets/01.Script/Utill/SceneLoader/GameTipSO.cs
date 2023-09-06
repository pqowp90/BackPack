using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTips", menuName = "SO/LoadingScene/GameTips", order = 0)]
public class GameTipSO : ScriptableObject
{
    [field: SerializeField] public List<string> GameTips { get; private set; }
}