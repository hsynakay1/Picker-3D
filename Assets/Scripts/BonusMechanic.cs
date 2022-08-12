using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMechanic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            GameManager.Instance.ShowPalette();
            gameObject.SetActive(false);
        }
    }
}
