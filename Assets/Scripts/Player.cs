using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    Animator animator;

    // 音乐
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    // 伤害值
    public GameObject popupDamage;

    private Pet playerAttributes;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        playerAttributes = GameController.instance.playerAttributes;
        foodText.text = "Food: " + playerAttributes.hp;
        base.Start();
    }

    // 检测到碰撞对象的tag，如果是food或者soda则增加生命然后让这些食物隐藏
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Food")
        {
            playerAttributes.hp += pointsPerSoda;
            if( playerAttributes.hp > playerAttributes.hpLimit)
            {
                playerAttributes.hpLimit = playerAttributes.hp;
            }
            foodText.text = "+"+pointsPerSoda+", Food: " + playerAttributes.hp;
            collision.gameObject.SetActive(false);
            Debug.Log(playerAttributes.hp);

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
        }
        else if(collision.tag == "Soda")
        {
            playerAttributes.hp += pointsPerFood;
            if (playerAttributes.hp > playerAttributes.hpLimit)
            {
                playerAttributes.hpLimit = playerAttributes.hp;
            }
            foodText.text = "+" + pointsPerFood + ", Food: " + playerAttributes.hp;
            collision.gameObject.SetActive(false);
            Debug.Log(playerAttributes.hp);

            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
        }else if(collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
    }

    void Restart()
    {
        // 重新加载场景
        SceneManager.LoadScene(0);
    }

    // 获取键盘方向键输入
    void Update()
	{
		if (!GameController.instance.playerTurn)
		{
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical = (int)Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
		{
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0)
		{
			AttempMove(horizontal, vertical);
		}
	}

    protected override void OnCantMove(GameObject blocker)
    {
        if (blocker.tag == "Wall") { 
            blocker.GetComponent<Wall>().DamageWall(wallDamage);
            animator.SetTrigger("PlayerChop");
        }else if(blocker.tag == "Enemy")
        {
            AttackInfo attackInfo;
            Debug.Log("player attack, hp" + playerAttributes.hp );
            Pet.attacking(playerAttributes, blocker.GetComponent<Enemy>().attributes, out attackInfo);
            Debug.Log("attackInfo: " + attackInfo.ToString());
            Debug.Log("player hp" + playerAttributes.hp);

            // 创建并显示伤害值UI
            blocker.GetComponent<Enemy>().loseHp( attackInfo, this);
            
        }

    }

    // 当玩家被攻击时，激活hit动画，生命扣除，检测是否生命值为0从而结束游戏
    public void loseFood(AttackInfo attackInfo, Enemy enemy)
    {
        animator.SetTrigger("PlayerHit");
        Debug.Log("player lose hp, current hp: " + playerAttributes.hp + ", damage: " + attackInfo.damage);
        foodText.text = "-"+attackInfo.damage+", Food: " + playerAttributes.hp;
        if (playerAttributes.hp < 0)
        {
            Debug.Log("GameOver");
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameController.instance.GameOver();
        }

        // 创建并显示伤害值UI
        Debug.Log(transform.position);


        DamagePopup.show(popupDamage, enemy.transform.position, transform.position, attackInfo);

    }

    // 当物体被销毁时，调用此方法，把当前关卡的生命值返回到游戏管理器
    private void OnDisable()
    {
        GameController.instance.playerAttributes = playerAttributes;
    }

    protected override void AttempMove(int xDir, int yDir)
    {
        playerAttributes.hp--;
        foodText.text = "Food: " + playerAttributes.hp;
        base.AttempMove(xDir, yDir);

        RaycastHit2D hit;
        if(Move(xDir,yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        if (playerAttributes.hp < 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
        
            SoundManager.instance.musicSource.Stop();
            GameController.instance.GameOver();
        }

        GameController.instance.playerTurn = false;
        
    }
}
