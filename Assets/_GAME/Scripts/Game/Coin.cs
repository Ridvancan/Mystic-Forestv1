using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] List<GameObject> coins;
    private void OnEnable()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            GameObject _coin = coins[i];
            _coin.SetActive(true);
            _coin.transform.DOLocalJump(new Vector3(_coin.transform.localPosition.x, _coin.transform.localPosition.y, _coin.transform.localPosition.z), 1, 1, 1).OnComplete(() => _coin.transform.DOJump(ManagerHub.Get<CharacterController>().transform.position, 0.5f, 1, 2f)).OnComplete(() =>
            {//danis
                _coin.SetActive(false); /*ManagerHub.Get<CharacterController>().ReturnToWalk();*/ this.gameObject.SetActive(false);
            });
        }
    }

private void OnDisable()
{
    for (int i = 0; i < coins.Count; i++)
    {
        coins[i].transform.localPosition = Vector3.zero;
        coins[i].gameObject.SetActive(true);
    }
}
void Start()
{

}

void Update()
{

}
}
