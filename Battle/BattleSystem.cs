using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit player;
    [SerializeField] BattleHUD HUD;
    [SerializeField] BattleUnit enemy;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;
    public event Action<bool> OnBattleOver;
    Party playerParty;
    Creature wildCreature;

    public void StartBattle(Party playerParty, Creature wildCreature)
    {
        this.playerParty = playerParty;
        this.wildCreature = wildCreature;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        player.Setup(playerParty.GetHealthyCreature());
        HUD.SetValues(player.Creature);
        enemy.Setup(wildCreature);
        enemyHUD.SetValues(enemy.Creature);
        dialogBox.SetMoveNames(player.Creature.Moves);
        partyScreen.Init();
        int num = UnityEngine.Random.Range(1, 101);
        if(num <= 10)
        {
            yield return dialogBox.TypeDialog($"A surprisingly attractive {enemy.Creature.CreatureBase.Name} appeared!");
        }
        else if(num <= 20)
        {
            yield return dialogBox.TypeDialog($"An unreasonably aggressive {enemy.Creature.CreatureBase.Name} appeared! What's his problem?");
        }
        else if(num <= 30)
        {
            yield return dialogBox.TypeDialog($"You interrupted {enemy.Creature.CreatureBase.Name}'s nap. He's pissed.");
        }
        else if(num <= 40)
        {
            yield return dialogBox.TypeDialog($"{enemy.Creature.CreatureBase.Name} said something about your mom. Kick his ass.");
        }
        else if(num <=55)
        {
            yield return dialogBox.TypeDialog($"Another random encounter! How monotonous.");
        }
        else if(num <= 60)
        {
            yield return dialogBox.TypeDialog($"Blah blah blah, wild {enemy.Creature.CreatureBase.Name}, blah blah blah.");
        }
        else if (num <= 70)
        {
            yield return dialogBox.TypeDialog($"A wild {enemy.Creature.CreatureBase.Name} appeared! He might be rare. Might not be, too.");
        }
        else
        {
            yield return dialogBox.TypeDialog($"A wild {enemy.Creature.CreatureBase.Name} appeared!");
        }
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialogText("What will you do?");
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Creatures);
        partyScreen.gameObject.SetActive(true);
    }


    IEnumerator PerformSelectedMove()
    {
        state = BattleState.Busy;
        var move = player.Creature.Moves[currentMove];
        move.Uses--;
        yield return dialogBox.TypeDialog($"{player.Creature.CreatureBase.Name} used {move.Base.Name}!");
        player.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemy.PlayHitAnimation();
        var damageDetails = enemy.Creature.TakeDamage(move, player.Creature);
        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"You knocked out {enemy.Creature.CreatureBase.Name}!");
            enemy.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemy.Creature.GetEnemyMove();
        move.Uses--;
        yield return dialogBox.TypeDialog($"{enemy.Creature.CreatureBase.Name} used {move.Base.Name}!");
        enemy.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        player.PlayHitAnimation();
        var damageDetails = player.Creature.TakeDamage(move, enemy.Creature);
        yield return HUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"Your {player.Creature.CreatureBase.Name} has been knocked out!");
            player.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
            var nextCreature = playerParty.GetHealthyCreature();
            if(nextCreature != null)
            {
                OpenPartyScreen();
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails details)
    {
        if(details.Critical > 1)
        {
            yield return dialogBox.TypeDialog("That attack seemed unreasonably effective, as if by random chance.");
        }
        if(details.TypeEffectiveness > 1)
        {
            yield return dialogBox.TypeDialog("That attack was super effective. Nice.");
        }
        else if(details.TypeEffectiveness < 1)
        {
            yield return dialogBox.TypeDialog("That attack was not very effective. Come on, dude.");
        }
    }

    public void HandleUpdate()
    {
        if(state==BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if(state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(currentAction==0)
            {
                //fight
                PlayerMove(); 

            }
            else if(currentAction==1)
            {
                //bag
            }
            else if (currentAction == 2)
            {
                //party
                OpenPartyScreen();

            }
            else if (currentAction == 3)
            {
                //run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove -= 2;
        }

        currentMove = Mathf.Clamp(currentMove, 0, player.Creature.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, player.Creature.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformSelectedMove());
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember -= 2;
        }

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Creatures.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Creatures[currentMember];
            if(selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a knocked out Creature, doofus.");
                return;
            }
            if(selectedMember == player.Creature)
            {
                partyScreen.SetMessageText("He's already out, doofus.");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchCreature(selectedMember));
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }

    }

    IEnumerator SwitchCreature(Creature newCreature)
    {
        if(player.Creature.HP > 0)
        {
            yield return dialogBox.TypeDialog($"{player.Creature.CreatureBase.Name}, return!");
            player.PlayReturnAnimation();
            yield return new WaitForSeconds(1f);
        }
        
        player.Setup(newCreature);
        HUD.SetValues(newCreature);
        dialogBox.SetMoveNames(newCreature.Moves);
        yield return dialogBox.TypeDialog($"Go get 'em {newCreature.CreatureBase.Name}!");
        StartCoroutine(EnemyMove());
    }
}
