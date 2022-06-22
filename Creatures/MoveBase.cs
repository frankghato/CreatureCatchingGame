using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Creature/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;
    [TextArea][SerializeField] string description;
    [SerializeField] CreatureType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int uses;
    [SerializeField] bool isMagical;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public CreatureType Type { get { return type; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy; } }
    public int Uses { get { return uses; } }
    public bool IsMagical { get { return isMagical; } }
}
