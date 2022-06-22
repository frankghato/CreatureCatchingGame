using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Party : MonoBehaviour
{
    [SerializeField] List<Creature> creatures;

    public List<Creature> Creatures
    {
        get
        {
            return creatures;
        }
    }

    private void Start()
    {
        foreach(var creature in creatures)
        {
            creature.Init();
        }
    }

    public Creature GetHealthyCreature()
    {
        return creatures.Where(x => x.HP > 0).FirstOrDefault();
    }
}
