using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    public CharacterController characterController;
    void Start()
    {
        
    }

    void Update()
    {
        if (characterController.currentState==CharacterState.walking)
        {
            transform.Translate(Vector3.forward * characterController.speed * Time.deltaTime);
        }
    }
}
