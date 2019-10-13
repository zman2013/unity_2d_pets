using UnityEngine;
using System.Collections;

public class Pet
{
    // 名字
    public string name = "";
    // 生命 //////
    // 生命上限          [100 ~ 1000]
    public int hpLimit = 100;
    // 生命              [100 ~ 1000]
    public int hp = 100;
    // 攻击恢复           [0 ~ 50]
    public int recovery = 0;
    // 吸血百分比        [0 ~ 20]
    public int leech = 0;

    ////// 防御 //////
    // 防御：伤害减免     [0 ~ 90]
    public int defend = 0;
    // 闪避：概率免伤     [0 ~ 50]
    public int dodge = 0;
    // 坚韧：降低暴击概率  [0 ~ 70]
    public int tough = 0;

    ////// 攻击 //////
    // 最小攻击力           [1 ~ 1000]
    public int minAttack = 10;
    // 最大攻击力：一定大于等于最小攻击力 [1 ~ 1000]
    public int maxAttack = 10;
    // 暴击率           [1 ~ 70]
    public int critical = 1;
    // 暴击伤害比         [150 ~ 500]
    public int criticalAttack = 150;

    public Pet(string name)
    {
        this.name = name;
        this.hpLimit = Random.Range(100, 1000);
        this.hp = hpLimit;
        this.recovery = Random.Range(0, 50);
        this.leech = Random.Range(0, 20);

        this.defend = Random.Range(0, 90);
        this.dodge = Random.Range(0, 50);
        this.tough = Random.Range(0, 70);

        this.minAttack = Random.Range(1, 1000);
        this.maxAttack = Random.Range(1, 1000);
        if (minAttack > maxAttack)
        {
            minAttack = maxAttack;
        }
        this.critical = Random.Range(1, 70);
        this.criticalAttack = Random.Range(150, 500);
    }

    public static void attacking(Pet attacker, Pet defender, out AttackInfo attackInfo)
    {
        attackInfo = new AttackInfo(false, false, 0, 0);

        // 1 判断对方是否闪避
        int dodge = Random.Range(0, 100);
        if( dodge < defender.dodge)
        {
            // 攻击被闪避
            attackInfo.dodge = true;
            return;
        }

        // 2 随机攻击力
        int attack = Random.Range(attacker.minAttack, attacker.maxAttack);

        // 3 计算是否发生暴击
        int critical = Random.Range(0, 100);
        // 3.1 计算实际暴击率 = 攻击者暴击率 - 对方坚韧值
        int realCritical = attacker.critical - defender.critical;
        if( realCritical > 0)
        {
            // 发生暴击
            if( critical < realCritical)
            {
                attack = attack * attacker.criticalAttack / 100;
                attackInfo.critical = true;
            }
        }

        // 4 计算实际伤害 = 攻击力 * (100 -对方防御)/100
        int damage = attack * (100 - defender.defend) / 100;

        // 5 如果对方的生命值小于实际伤害，则实际伤害=对方生命值
        if(defender.hp < damage)
        {
            damage = defender.hp;
        }

        attackInfo.damage = damage;

        // 6 计算吸血 = 实际伤害 * 吸血比 / 100
        int leech = damage * attacker.leech / 100;

        // 7 计算攻击者生命 = 当前生命值 + 吸血 + 回复，但小于生命上限
        int hp = attacker.hp + leech + attacker.recovery;
        if( hp > attacker.hpLimit)
        {
            hp = attacker.hpLimit;
        }

        attackInfo.recovery = hp - attacker.hp;

        attacker.hp = hp;

        // 8 计算对方生命值
        defender.hp -= damage;
    }
}
