using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public Creature Creature { get; set; }
    Image image;
    Vector3 originalPosition;
    Color originalColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPosition = image.transform.localPosition;
        originalColor = image.color;
    }
    public void Setup(Creature creature)
    {
        Creature = creature;
        //Creature = new Creature(cbase, level);
        if(isPlayerUnit)
        {
            image.sprite = Creature.CreatureBase.BackSprite;
        }
        else
        {
           image.sprite = Creature.CreatureBase.FrontSprite;
        }
        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if(isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPosition.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPosition.y);
        }
        image.transform.DOLocalMoveX(originalPosition.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if(isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

    public void PlayReturnAnimation()
    {
        image.transform.DOLocalMoveX(-500f, 1f);
    }
}
