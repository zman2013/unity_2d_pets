using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;
    public float moveTime = 0.1f;

    BoxCollider2D boxCollider2D;
    Rigidbody2D rb2D;
    float inverseMoveTime;

    // 初始化
    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;

    }

    // 协同程序进行平滑移动
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    // 调用Linecast()进行线性投射检测前方是否存在障碍物，没有则进行平移，有责返回false
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider2D.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider2D.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }


    //根据传入的参数调用Move()进行移动判断和操作，不能移动的情况下根据返回的hit结构体信息判断是否调用OnCantMove()进行处理
    protected virtual void AttempMove (int xDir, int yDir)
    {
        RaycastHit2D hit;

        bool canMove = Move(xDir, yDir, out hit);

        if( hit.transform == null)
        {
            return;
        }

        if (!canMove && hit.transform != null)
        {
            OnCantMove(hit.transform.gameObject);
        }


    }

    protected abstract void OnCantMove (GameObject blocker);
}
