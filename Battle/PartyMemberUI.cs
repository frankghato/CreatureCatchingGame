using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Color highlightedColor;
    [SerializeField] Image partyMemberImage;
    Creature referenceCreature;

    public void SetValues(Creature creature)
    {
        referenceCreature = creature;
        nameText.text = creature.CreatureBase.Name;
        partyMemberImage.sprite = creature.CreatureBase.FrontSprite;
        Debug.Log(creature.CreatureBase.Name);
        levelText.text = "Lvl " + creature.Level;
        hpBar.SetHP((float)creature.HP / creature.MaxHP);
    }

    public void HighlightMember(bool selected)
    {
        if(selected)
        {
            nameText.color = highlightedColor;
        }
        else
        {
            nameText.color = Color.black;
        }
    }
}
