using UnityEngine;
 
[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]

public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get { return multiplier; } private set { multiplier = value; } }

    [SerializeField]
    int level; // Not meant to be modified in the game (Only in Editor)
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // Prefab of the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon; // Not meant to be modified in the game (Only in Editor)
    public Sprite Icon { get => icon; private set => icon = value; }
}
