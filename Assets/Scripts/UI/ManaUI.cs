using System.Collections;
using System.Collections.Generic;
using Events;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image manaFiller;
    [SerializeField] private Image heart;

    
    public void RefreshManaFill()
    {
        manaFiller.fillAmount = PlayerController.PlayerSettings.curMana / PlayerController.PlayerSettings.maxMana;
        PlayerController.PlayerSettings.onOutOfMana.Register(gameObject, arg0 => OutOfMana());
    }



    private void OutOfMana()
    {
        StartCoroutine(Flicker(2, 0.2f, 0.2f));
    }

    IEnumerator Flicker(int nTimes, float timeOn, float timeOff)
    {
        var color = heart.color;

        while (nTimes > 0)
        {
            color = new Color(color.r,color.b, color.g, 1f);
            heart.color = color;
            yield return new WaitForSeconds(timeOn);
            color = new Color(color.r,color.b, color.g, 0f);
            heart.color = color;
            yield return new WaitForSeconds(timeOff);
            color = new Color(color.r,color.b, color.g, 1f);
            heart.color = color;
            nTimes--;
        }
    }
}
