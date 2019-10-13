using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 4;

    SpriteRenderer spriteRenderer;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 破坏Wall之后替换图片资源和扣血，如果Wall生命为0则隐藏游戏对象
    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
