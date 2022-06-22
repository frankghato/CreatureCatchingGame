using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Creature 
{ 
    public int HP { get; set; }
    public List<Move> Moves { get; set; }
    [SerializeField] CreatureBase creatureBase;
    [SerializeField] int level;
    
    public CreatureBase CreatureBase
    {
        get { return creatureBase; }
    }

    public int Level
    {
        get { return level; }
    }

    public void Init()
    {
        //this.CreatureBase = creatureBase;
        //this.Level = level;
        HP = MaxHP;
        Moves = new List<Move>();
        foreach(var move in this.CreatureBase.LearnableMoves)
        {
            if(move.Level <= this.Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }

    public int MaxHP 
    { 
           get { return Mathf.FloorToInt(CreatureBase.MaxHP * Level / 100f) + 10; }
    }
    public int Attack 
    { 
           get { return Mathf.FloorToInt(CreatureBase.Attack * Level / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt(CreatureBase.Defense * Level / 100f) + 5; }
    }
    public int MagicalAttack
    {
        get { return Mathf.FloorToInt(CreatureBase.MagicalAttack * Level / 100f) + 5; }
    }
    public int MagicalDefense
    {
        get { return Mathf.FloorToInt(CreatureBase.MagicalDefense * Level / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt(CreatureBase.Speed * Level / 100f) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Creature attacker)
    {
        float critical = 1f;
        if(Random.value * 100f <= 5)
        {
            critical = 2f;
        }

        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.CreatureBase.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.CreatureBase.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.IsMagical) ? attacker.MagicalAttack : attacker.Attack;
        float defense = (move.Base.IsMagical) ? MagicalDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * typeEffectiveness * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted=true;
        }
        return damageDetails;
    }

    public Move GetEnemyMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}