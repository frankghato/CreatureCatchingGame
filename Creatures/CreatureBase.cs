using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Creature/Create new creature")]
public class CreatureBase : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string name;
    [TextArea] [SerializeField] string description;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] CreatureType type1;
    [SerializeField] CreatureType type2;
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int magicalAttack;
    [SerializeField] int magicalDefense;
    [SerializeField] int speed;
    [SerializeField] List<LearnableMove> learnableMoves;
    public int ID { get { return id; } }
    public string Name { get { return name; } }
    public string Description { get { return description; } }   
    public int MaxHP { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int MagicalAttack { get { return magicalAttack; } }
    public int MagicalDefense { get { return magicalDefense; } }
    public int Speed { get { return speed; } }
    public Sprite FrontSprite { get { return frontSprite; } }
    public Sprite BackSprite { get { return backSprite; } }
    public CreatureType Type1 { get { return type1;} }
    public CreatureType Type2 { get { return type2;} }
    public List<LearnableMove> LearnableMoves { get { return learnableMoves; } }

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get { return moveBase; } }
    public int Level { get { return level; } }

}


public enum CreatureType
{ 
    None,
    Water,
    Fire,
    Grass,
    Electric,
    Flying,
    Bug,
    Rock,
    Demon,
    Material,
    Brawler,
    Magic,
    Normal,
    Draconic
}

public class TypeChart
{
    static float[][] chart =
    {
        //                            WAT,    FIR,    GRA,    ELE,    FLY,    BUG,    ROC,    DEM,    MAT,    BRA,    MAG,    NOR,    DRA
        /* Water    */ new float[] {  .5f,    2f,    .5f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,    .5f,     1f,     1f},
        /* Fire     */ new float[] {  .5f,   .5f,     2f,     1f,     1f,     2f,    .5f,    .5f,     2f,     1f,     1f,     1f,     1f},
        /* Grass    */ new float[] {   2f,   .5f,    .5f,     1f,    .5f,    .5f,     2f,     1f,     1f,     1f,     1f,     1f,     1f},
        /* Electric */ new float[] {   2f,    1f,    .5f,    .5f,     2f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f},
        /* Flying   */ new float[] {   1f,    1f,     2f,    .5f,     1f,     2f,    .5f,     1f,     1f,     1f,     1f,     1f,     1f},
        /* Bug      */ new float[] {   1f,   .5f,     2f,     1f,    .5f,     1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f},
        /* Rock     */ new float[] {   1f,    2f,    .5f,     1f,     2f,     2f,     1f,     1f,     1f,    .5f,     1f,     1f,     1f},
        /* Demon    */ new float[] {   1f,    1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f},
        /* Material */ new float[] {   1f,    1f,     1f,     1f,     1f,     1f,     2f,     2f,     1f,     1f,     1f,     1f,     1f},
        /* Brawler  */ new float[] {   1f,    1f,     1f,     1f,     1f,     1f,     2f,     1f,     2f,     1f,     1f,     1f,     1f},
        /* Magic    */ new float[] {   1f,    2f,     1f,     1f,     1f,     1f,     1f,     2f,     1f,     1f,     1f,     1f,     1f},
        /* Normal   */ new float[] {   1f,    1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f},
        /* Draconic */ new float[] {   1f,    1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f,     1f}
    };

    public static float GetEffectiveness(CreatureType attackType, CreatureType defenseType)
    {
        if(attackType == CreatureType.None || defenseType == CreatureType.None)
        {
            return 1;
        }
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}
