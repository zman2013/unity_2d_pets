using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader
   : MonoBehaviour
{
	public GameObject gameManager;

    // 当前不存在GameController实例时，生成gameManager
    void Awake()
	{
        if(GameController.instance == null)
		{
			Instantiate(gameManager);
		}
	}
}
