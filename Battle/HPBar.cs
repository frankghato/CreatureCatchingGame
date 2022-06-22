using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator HPAnimation(float newHP)
    {
        float currentHP = health.transform.localScale.x;
        float deltaHP = currentHP - newHP;
        while(currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= deltaHP * Time.deltaTime;
            health.transform.localScale = new Vector3(currentHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
