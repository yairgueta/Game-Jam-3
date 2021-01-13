using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image manaFiller;
    [SerializeField] private PlayerSettingsObject playerSettings;
    
    public void RefreshManaFill()
    {
        manaFiller.fillAmount = playerSettings.curMana / playerSettings.maxMana;
    }
}
