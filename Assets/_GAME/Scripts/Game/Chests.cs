using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ChestType
{
    woodChest,
    silverChest,
    goldenChest
}
public class Chests : MonoBehaviour
{
    public ChestType chestType;
    public EquipmentType dropType;
    public Transform openPoint;
    public Transform dropItemPivot;
    public GameObject dropItem;

    private void Start()
    {
        FillChestWithDrops();
    }
    public void FillChestWithDrops()
    {
        switch (chestType)
        {
            case ChestType.woodChest:
                dropItem = ManagerHub.Get<GameManager>().level1Equipments[Random.Range(0, ManagerHub.Get<GameManager>().level1Equipments.Count)];
                break;
            case ChestType.silverChest:
                dropItem = ManagerHub.Get<GameManager>().level2Equipments[Random.Range(0, ManagerHub.Get<GameManager>().level2Equipments.Count)];
                break;
            case ChestType.goldenChest:
                dropItem = ManagerHub.Get<GameManager>().level3Equipments[Random.Range(0, ManagerHub.Get<GameManager>().level3Equipments.Count)];
                break;
            default:
                break;
        }
        GameObject _tempDropItem = Instantiate(dropItem,dropItemPivot);
        _tempDropItem.transform.position = dropItemPivot.position;
        _tempDropItem.transform.localPosition = Vector3.zero;
        dropType = dropItem.GetComponent<EquipmentBase>().equipmentType;
    }
    public void OpenChest()
    {
        openPoint.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.InOutBack)/*.OnComplete(()=>dropItemPivot.transform.DOJump(new Vector3(transform.position.x, 0, transform.position.z - 1),2,1,1f))*/;
        if (dropType==EquipmentType.armor)
        {
            if (ManagerHub.Get<CharacterController>().currentArmor==null)
            {
                dropItemPivot.transform.DOJump(ManagerHub.Get<CharacterController>().centerPivot.position, 1, 1, 1).OnComplete(() => { ManagerHub.Get<CharacterController>().EquipNewArmor((int)dropItem.GetComponent<EquipmentBase>().equipmentLevel);dropItemPivot.gameObject.SetActive(false); });
                return;
            }
            if ((int) dropItem.GetComponent<EquipmentBase>().equipmentLevel > (int)ManagerHub.Get<CharacterController>().currentArmor.equipmentLevel)
            {
                dropItemPivot.transform.DOJump(ManagerHub.Get<CharacterController>().centerPivot.position, 1, 1, 1).OnComplete(() => { ManagerHub.Get<CharacterController>().EquipNewArmor((int)dropItem.GetComponent<EquipmentBase>().equipmentLevel);  });
                dropItemPivot.transform.DOScale(Vector3.zero, 1.2f).SetEase(Ease.InSine);
            }
            else
            {
                //shred
                Coin _tempCoin = ManagerHub.Get<GameManager>().coinParent;
                _tempCoin.transform.position = dropItemPivot.transform.position;
                _tempCoin.gameObject.SetActive(true);
                dropItemPivot.gameObject.SetActive(false);
                ManagerHub.Get<CharacterController>().ReturnToWalk();
            }
            
        }
        else
        {
            if ((int)dropItem.GetComponent<EquipmentBase>().equipmentLevel > (int)ManagerHub.Get<CharacterController>().currentWeapon.equipmentLevel)
            {
                dropItemPivot.transform.DOJump(ManagerHub.Get<CharacterController>().centerPivot.position, 1, 1, 1).OnComplete(() => ManagerHub.Get<CharacterController>().EquipNewSword((int)dropItem.GetComponent<EquipmentBase>().equipmentLevel));
                dropItemPivot.transform.DOScale(Vector3.zero, 1.2f).SetEase(Ease.InSine);
            }
            else
            {
                //shred
                Coin _tempCoin = ManagerHub.Get<GameManager>().coinParent;
                _tempCoin.transform.position = dropItemPivot.transform.position;
                _tempCoin.gameObject.SetActive(true);
                dropItemPivot.gameObject.SetActive(false);
                ManagerHub.Get<CharacterController>().ReturnToWalk();
                
            }

           
        }
    }
}
