using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }


    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public int rows = 8;
    public int columns = 8;

    public GameObject[] floorTiles;
    public GameObject[] outerWallTiles;
    public GameObject exitTile;

    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;

    Transform boardHolder;

    List<Vector3> gridPositions = new List<Vector3>();

    // 生成关卡
    public void SetupScene(int level)
    {
        Debug.Log("level: " + level);
        BoardSetup();
        Instantiate(exitTile, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

    }

    // 铺设地板floor，判断如果是四条边沿坐标则铺设外墙outerFloor
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for(int x=-1; x <= columns; x++)
        {
            for(int y=-1; y<=rows; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if( x==-1 || x==columns || y==-1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // 初始化随机区域的坐标集
    void InitializeList()
    {
        gridPositions.Clear();
        for( int x = 1; x < columns -1; x++)
            for( int y =1; y < rows-1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
    }

    // 从gridPositions中随机取一个坐标randomPosition并返回
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    // 编写随机生成物品的方法
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maxmum)
    {
        int objectCount = Random.Range(minimum, maxmum);
        for( int i = 0; i < objectCount; i ++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
