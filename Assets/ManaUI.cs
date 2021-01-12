using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image manaFiller;
    
    public void RefreshManaFill()
    {
        manaFiller.fillAmount = PlayerController.PlayerSettings.curMana / PlayerController.PlayerSettings.maxMana;
    }
}
