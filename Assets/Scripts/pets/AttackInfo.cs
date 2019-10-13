using System;
public class AttackInfo
{
    // 是否闪避
    public bool critical;
    // 伤害值
    public int damage;
    // 恢复值
    public int recovery;
    // 是否闪避
    public bool dodge;

    public AttackInfo(bool critical, bool dodge, int damage, int recovery)
    {
        this.critical = critical;
        this.dodge = dodge;
        this.damage = damage;
        this.recovery = recovery;
    }

    public override string ToString()
    {
        return "dodge: " + dodge + ", critical: " + critical + ", damage: " + damage + ", recovery: " + recovery;
    }
}
