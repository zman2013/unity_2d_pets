using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour
{
    public int value;

    //目标位置
    private Vector3 mTarget;
    //屏幕坐标
    private Vector3 mScreen;


    //文本宽度
    public float ContentWidth = 100;
    //文本高度
    public float ContentHeight = 50;

    //GUI坐标
    private Vector2 mPoint;

    // gui style
    private GUIStyle guiStyle;

    //销毁时间
    public float FreeTime = 1.5F;


    private void Start()
    {
        //获取目标位置
        mTarget = transform.position;
        //获取屏幕坐标
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
		//开启自动销毁线程

        StartCoroutine("Free");
    }

    private void Update()
    {
        //使文本在垂直方向山产生一个偏移
        transform.Translate(Vector3.up * 2F * Time.deltaTime);
        //重新计算坐标
        mTarget = transform.position;
        //获取屏幕坐标
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
    }

        IEnumerator Free()
    {
        yield return new WaitForSeconds(FreeTime);
        Destroy(gameObject);
    }


    void OnGUI()
    {
        //保证目标在摄像机前方
        if (mScreen.z > 0)
        {
            //内部使用GUI坐标进行绘制
            GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), value.ToString(), guiStyle);
        }
    }

    public static void show(GameObject popupDamage, Vector2 attackerPosition, Vector2 defenderPosition, AttackInfo attackInfo)
    {
        GameObject damageObject = Instantiate(popupDamage, defenderPosition, Quaternion.identity);
        damageObject.GetComponent<DamagePopup>().value = attackInfo.damage;
        GUIStyle damageGuiStyle = new GUIStyle();
        damageGuiStyle.fontSize = 18;
        damageGuiStyle.normal.textColor = Color.red;
        if (attackInfo.critical)
        {
            damageGuiStyle.fontSize = 36;
            damageGuiStyle.fontStyle = FontStyle.Bold;
        }
        damageObject.GetComponent<DamagePopup>().guiStyle = damageGuiStyle;


        GameObject recoveryObject = Instantiate(popupDamage, attackerPosition, Quaternion.identity);
        recoveryObject.GetComponent<DamagePopup>().value = attackInfo.recovery;
        GUIStyle recoveryGuiStyle = new GUIStyle();
        recoveryGuiStyle.fontSize = 24;
        recoveryGuiStyle.normal.textColor = Color.green;
        recoveryObject.GetComponent<DamagePopup>().guiStyle = recoveryGuiStyle;
    }
}