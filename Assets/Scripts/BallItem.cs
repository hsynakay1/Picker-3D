using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallItem : MonoBehaviour
{
    [SerializeField] int bonusBallIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            gameObject.SetActive(false);
            GameManager.Instance.AddBonusBalls(bonusBallIndex);
        }
    }
}
