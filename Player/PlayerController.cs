using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;
    private bool isMoving;
    private Vector2 input;
    public event Action OnEncountered;

    public void HandleUpdate()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
            if(input != Vector2.zero)
            {
                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                if (IsWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;

        CheckForEncounter();
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounter()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            int num = UnityEngine.Random.Range(1, 101);
            
            if(num <= 10)
            {
                //animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }
}
