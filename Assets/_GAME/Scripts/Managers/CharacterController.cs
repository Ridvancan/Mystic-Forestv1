using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    idle,
    walking,
    warIdle,
    war,
    win,
    fail
}
public enum WarState
{//easy road 3d
    walk,
    Idle,
    onAtack
}
public class CharacterController : BaseManager
{
    [Header("CharacterUpgradesAndItems")]
    public int HealthLevel;
    public int AtackLevel;
    public int DefenceLevel;
    public Armor currentArmor;
    public Weapon currentWeapon;
    [Header("CharacterProperties")]
    public int TotalHealth;
    public int CurrentHealth;
    public int CriticAtackChance;
    public int AtackValue;
    public int AtackSpeed;
    public int EquipmentAtackValue;
    public int EquipmentDefenceValue;
    public int DefenceValue;
    public int HealthPerRound;
    [Header("General")]
    public float speed;
    public CharacterState currentState;
    public WarState warState;
    public Animator selfAnimator;
    public List<Armor> armors;
    public List<Weapon> weapons;
    public Transform centerPivot;
    void Start()
    {
        selfAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (currentState==CharacterState.walking)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (currentState==CharacterState.warIdle)
        {
            if (Input.GetMouseButtonDown(0) && !(warState==WarState.onAtack))
            {
                warState = WarState.onAtack;
                Atack();
            }
        }
    }
    public void SpawnEnemy()
    {
        ManagerHub.Get<GameManager>().CallNextEnemy();
    }
    public void ReturnToWalk()
    {
        ManagerHub.Get<GameManager>().LevelUp();
        currentState = CharacterState.walking;
        selfAnimator.SetBool("WarIdle", false);
    }
    public void SetWarStateToIdle()
    {
        // Using By Animation Event
        warState = WarState.Idle;
    }
    public void ChangeState(CharacterState newCharacterState)
    {
        currentState = newCharacterState;
        AfterStateChange();
    }
    public void AfterStateChange()
    {
        switch (currentState)
        {
            case CharacterState.idle:
                selfAnimator.SetBool("Idle", true);
                break;
            case CharacterState.walking:
                selfAnimator.SetBool("Walk", true);
                break;
            case CharacterState.warIdle:
                selfAnimator.SetBool("WarIdle", true);
                break;
            case CharacterState.war:
                break;
            case CharacterState.win:
                break;
            case CharacterState.fail:
                break;
            default:
                break;
        }
    }
    public void EquipNewSword(int indexOfWeapon)
    {
        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }
        weapons[indexOfWeapon].gameObject.SetActive(true);
        currentWeapon = weapons[indexOfWeapon];
        AtackValue -= EquipmentAtackValue;
        EquipmentAtackValue = currentWeapon.value;
        ReturnToWalk();
        CalculateAtackValue();
    }
   
    public void EquipNewArmor(int indexOfArmor)
    {
        foreach (var item in armors)
        {
            item.gameObject.SetActive(false);
        }
        armors[indexOfArmor].gameObject.SetActive(true);
        currentArmor = armors[indexOfArmor];
        DefenceValue -= EquipmentDefenceValue;
        EquipmentAtackValue = currentArmor.value;
        ReturnToWalk();
        CalculateDefenceValue();
    }
    public void CalculateAtackValue()
    {
        AtackValue = currentWeapon.value + AtackLevel * 3;
    }
    public void CalculateDefenceValue()
    {
        DefenceValue = currentArmor.value + DefenceLevel * 3;
    }
    public void NormalAtackToEnemy()
    {
        // Using By Animation Event
        ManagerHub.Get<GameManager>().HitEnemy(AtackValue);
       
    }
    public void CriticAtackToEnemy()
    {
        ManagerHub.Get<GameManager>().HitEnemy(AtackValue*2);
        
    }
    public void Atack()
    {
        if (IsCriticAtack())
        {
            selfAnimator.SetTrigger("CriticAtack");
        }
        else
        {
            selfAnimator.SetTrigger("Atack");
        }
    }
    public bool IsCriticAtack()
    {
        int _criticAtackRollNumber = UnityEngine.Random.Range(0, 100);
        if (_criticAtackRollNumber<=CriticAtackChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void ReactToAtack()
    {
        if (warState != WarState.onAtack)
        {
            selfAnimator.SetTrigger("ReactToAtack");
        }
    }
    public virtual void GetDamage(int damageValue)
    {
        CurrentHealth -=Mathf.Clamp(damageValue-DefenceValue,0, damageValue - DefenceValue);
        CheckHealth();
    }
    public virtual void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            currentState = CharacterState.fail;
            Die();
        }
    }
    public virtual void Die()
    {
        selfAnimator.SetTrigger("Die");
    }
}
