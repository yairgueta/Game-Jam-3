using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image manaFiller;
    [SerializeField] private Image heart;
    private Tween tween;
    [SerializeField] private Ease scaleEaseIn;
    [SerializeField] private float toColorDuration;
    [SerializeField] private float fromColorDuration;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color noManaColor;
    public void RefreshManaFill()
    {
        manaFiller.fillAmount = PlayerController.PlayerSettings.curMana / PlayerController.PlayerSettings.maxMana;
        PlayerController.PlayerSettings.onOutOfMana.Register(gameObject, arg0 => OutOfMana());
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         OutOfMana();
    //     }
    // }


    private void OutOfMana()
    {
        FlashAnimation(noManaColor, originalColor,2);
        // StartCoroutine(Flicker(2, 0.2f, 0.2f));
    }
    
    
    
    private void FlashAnimation(Color targetColor, Color originColor, int loops)
    {
        tween?.Kill(true);
        tween = DOTween.Sequence()
            .Append(heart.DOColor(targetColor, toColorDuration).SetEase(scaleEaseIn))
            // .Join(sheepTransform.DOScale(originalScale*1.5f,toColorDuration))
            .Append(heart.DOColor(originColor, fromColorDuration).SetEase(scaleEaseIn))
            // .Join(sheepTransform.DOScale(originalScale,toColorDuration))
            .SetLoops(loops);
    }

    IEnumerator Flicker(int nTimes, float timeOn, float timeOff)
    {
        //TODO: BUGGGG
        var color = heart.color;

        while (nTimes > 0)
        {
            color = new Color(color.r,color.b, color.g, 1f);
            heart.color = color;
            yield return new WaitForSeconds(timeOn);
            color = new Color(color.r,color.b, color.g, 0.2f);
            heart.color = color;
            yield return new WaitForSeconds(timeOff);
            color = new Color(color.r,color.b, color.g, 1f);
            heart.color = color;
            nTimes--;
        }
    }
}
