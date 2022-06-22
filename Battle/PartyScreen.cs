using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;
    PartyMemberUI[] members;
    List<Creature> creatures;

    public void Init()
    {
        members = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Creature> creatures)
    {
        this.creatures = creatures;
        for(int i = 0; i < members.Length; i++)
        {
            if(i < creatures.Count)
            {
                members[i].SetValues(creatures[i]);
            }
            else
            {
                members[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "Choose a creature";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for(int i = 0; i < creatures.Count; i++)
        {
            if(i == selectedMember)
            {
                members[i].HighlightMember(true);
            }
            else
            {
                members[i].HighlightMember(false);
            }
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
