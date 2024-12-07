using RoundBallGame.Gameplay.Levels;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "RoundBallGame/LevelCollection")]
public class LevelCollectionSO : ScriptableObject
{
    public LevelDescriptor[] Levels;
}
