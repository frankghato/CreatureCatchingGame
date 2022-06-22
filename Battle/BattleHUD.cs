using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    Creature referenceCreature;
    public void SetValues(Creature creature)
    {
        referenceCreature = creature;
        nameText.text = creature.CreatureBase.Name;
        Debug.Log(creature.CreatureBase.Name);
        levelText.text = "Lvl " + creature.Level;
        hpBar.SetHP((float)creature.HP / creature.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.HPAnimation((float)referenceCreature.HP / referenceCreature.MaxHP);
    }
}
