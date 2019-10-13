using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject
{
    public int playerDamage;

    Transform target;
    bool skipMove;

    Animator animator;

    public AudioClip attackSound1;
    public AudioClip attackSound2;

    public Pet attributes;

    // 伤害值
    public GameObject popupDamage;

    protected override void OnCantMove(GameObject blocker)
    {
        if (blocker.tag != null && blocker.tag == "Player") { 
            Player hitPlayer = blocker.GetComponent<Player>();

            animator.SetTrigger("EnemyAttack");

            SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);

            Debug.Log("enemy attack, hp: " + attributes.hp);
            Pet.attacking(attributes, GameController.instance.playerAttributes, out AttackInfo attackInfo);
            Debug.Log("attackInfo: " + attackInfo);
            Debug.Log("enemy hp: " + attributes.hp);

            hitPlayer.loseFood(attackInfo, this);
        }
    }

    // use this for initialization
    protected override void Start()
    {
        attributes = new Pet("enemy");

        animator = GetComponent<Animator>();

        GameController.instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();

    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        Debug.Log("Move Enemy:1");
        AttempMove(xDir, yDir);
    }

    protected override void AttempMove(int xDir, int yDir)
    {
        Debug.Log(transform.position + ", hp:" + attributes.hp);
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttempMove(xDir, yDir);
        skipMove = true;
    }

    internal void loseHp(AttackInfo attackInfo, Player player)
    {
        DamagePopup.show(popupDamage, player.transform.position, transform.position, attackInfo);

        if ( attributes.hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
