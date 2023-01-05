using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameStates
{
    mainMenu,
    inGame,
    duringWar,
    gameOver
}
public class GameManager : BaseManager
{
    [Header("LevelAndOthers")]
    public int currentLevel;
    public int highestLevel;
    public GameStates currentGameState;
    public CharacterController characterController;
    [Header("Enemy")]
    public GameObject enemyParent;
    public List<GameObject> enemyList;
    public Enemy currentEnemy;
    [Header("Chests")]
    public List<GameObject> chestPrefabs;
    public List<Chests> chests;
    [Header("Chest Drops")]
    public List<GameObject> level1Equipments;
    public List<GameObject> level2Equipments;
    public List<GameObject> level3Equipments;
    public Coin coinParent;
    
    //public List<EquipmentBase> equipments;

    void Start()
    {
        currentLevel = 0;
        currentGameState = GameStates.mainMenu;
        for (int i = 0; i < enemyParent.transform.childCount; i++)
        {
            enemyList.Add(enemyParent.transform.GetChild(i).gameObject);
        }
        //DropRate
        for (int i = 0; i < 7; i++)
        {
            chests.Add(chestPrefabs[0].GetComponent<Chests>());
        }
        for (int i = 0; i < 4; i++)
        {
            chests.Add(chestPrefabs[1].GetComponent<Chests>());
        }
        chests.Add(chestPrefabs[2].GetComponent<Chests>());
        //DropRate
    }
    public void LevelUp()
    {
        currentLevel++;
    }
    void Update()
    {
       
    }
    public void PlayButton()
    {
        if (currentGameState==GameStates.mainMenu)
        {
            currentGameState = GameStates.inGame;
            characterController.ChangeState(CharacterState.walking);
        }

    }
    public Enemy NextEnemy()
    {
        List<GameObject> _tempListOfActiveEnemy = enemyList.Where(x => x.gameObject.activeInHierarchy == false).ToList();
        return _tempListOfActiveEnemy[Random.Range(0, _tempListOfActiveEnemy.Count)].GetComponent<Enemy>();
    }
    public void CallNextEnemy()
    {
        if (currentEnemy==null)
        {
            currentEnemy = NextEnemy();
            currentEnemy.transform.parent = null;
            currentEnemy.gameObject.SetActive(true);
        }
       
    }
    public void HitEnemy(int atackValue)
    {
        currentEnemy.ReactToAtack();
        currentEnemy.GetDamage(atackValue);
        //hp reduce.
    }
    public void HitPlayer(int atackValue)
    {
        characterController.ReactToAtack();
        characterController.GetDamage(atackValue);

    }
    public Chests GetRandomChest()
    {
        return chests[Random.Range(0, chests.Count)];
    }
}
