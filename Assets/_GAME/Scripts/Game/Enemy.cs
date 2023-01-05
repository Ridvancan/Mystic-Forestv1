using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum EnemyType
{
    orc,
    goblin
}
public enum EnemyStates
{
    walk,
    warIdle,
    duringWar,
    atacking,
    die

}
public abstract class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyStates enemyState;
    public float speed;
    public int Health;
    public int AtackValue;
    public int DropItemChance;
    public float AtackRange;
    public Animator selfAnimator;
    public Transform characterTransform;
    public bool hasDrop;

    public void Start()
    {
        this.selfAnimator = GetComponent<Animator>();
        this.characterTransform = ManagerHub.Get<GameManager>().characterController.gameObject.transform;
    }
    public void SetWarStateToIdle()
    {
        // Using By Animation Event
        enemyState = EnemyStates.warIdle;
    }
    public void HitPlayer()
    {
        ManagerHub.Get<GameManager>().HitPlayer(AtackValue);
    }
    public void OnEnable()
    {
        int _levelMultiply = ManagerHub.Get<GameManager>().currentLevel;
        hasDrop = WillDrop();
        Health = _levelMultiply * 5 + 10;
        AtackValue = _levelMultiply * 5 + 10;
    }//speed / kritik þans / hp++ / defans / round baþýna hp yenileme+++ / atak+++
    public virtual void Atack()
    {
        selfAnimator.SetTrigger("Atack");
        enemyState = EnemyStates.atacking;
    }
    public virtual void ReactToAtack()
    {
        if (enemyState != EnemyStates.atacking)
        {
            selfAnimator.SetTrigger("ReactToAtack");
        }
    }
    public virtual void GetDamage(int damageValue)
    {
        Health -= damageValue;
        CheckHealth();
    }
    public virtual void CheckHealth()
    {
        if (Health <= 0)
        {
            enemyState = EnemyStates.die;
            Die();
        }
    }
    public virtual bool WillDrop()
    {
        int _tempDropNumber = Random.Range(0, 100);
        if (_tempDropNumber <= DropItemChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Update()
    {
        if (enemyState == EnemyStates.walk)
        {
            float distance = Vector3.Distance(transform.position, characterTransform.position);
            if (distance >= AtackRange)
            {
                transform.position = Vector3.Lerp(transform.position, characterTransform.position, Time.deltaTime * speed);
            }
            else
            {
                enemyState = EnemyStates.warIdle;
                selfAnimator.SetBool("WarIdle", true);
                ManagerHub.Get<CharacterController>().ChangeState(CharacterState.warIdle);
            }
        }
        if (enemyState == EnemyStates.warIdle)
        {
            if (Input.GetMouseButtonDown(1))
            {
                SpawnChest();
            }
        }


    }
    public virtual void Die()
    {
        selfAnimator.SetTrigger("Die");
        ManagerHub.Get<GameManager>().currentEnemy = null;
        StartCoroutine(Funeral());

    }
    public IEnumerator Funeral()
    {
        yield return new WaitForSeconds(6);
        transform.DOMoveY(-3, 3f);
    }
    public virtual void DropChest()
    {
        if (hasDrop)
        {
            SpawnChest();
        }
        else
        {
            //continue
        }
    }
    public virtual void SpawnChest()
    {
        ChestType _spawnChestType = ManagerHub.Get<GameManager>().GetRandomChest().chestType;
        GameObject _spawnChest = Instantiate(ManagerHub.Get<GameManager>().chestPrefabs.Where(x => x.GetComponent<Chests>().chestType == _spawnChestType).FirstOrDefault());
        _spawnChest.transform.position = transform.position;
        _spawnChest.transform.DOJump(new Vector3(transform.position.x + 1, 0, transform.position.z - 1), 1, 1, 0.5f).OnComplete(() => _spawnChest.GetComponent<Chests>().OpenChest()); ;
    }
    public virtual void WalkToCharacter()
    {

    }


}
