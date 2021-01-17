using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    private Vector3 targetPosotion;
    private RectTransform pointerTrans;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private float borderSize = 50f;
    [SerializeField] private float x = 200f;
    [SerializeField] private float y = 100f;
    [SerializeField] private float z = -400f;
    [SerializeField] private float w = 95f;
    [SerializeField] private float t = -200f;

    
    
    void Start()
    {
        targetPosotion = new Vector3(-2.29f, -0.85f, 0);
        pointerTrans = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 toPosition = targetPosotion;
        Vector3 fromPosition = mainCamera.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);
        pointerTrans.localEulerAngles = new Vector3(0,0, angle);


        Vector3 targetPosScreenPoint = mainCamera.WorldToScreenPoint(targetPosotion);
        bool isOffScreen = targetPosScreenPoint.x <= borderSize+z || targetPosScreenPoint.x >= Screen.width - borderSize+x ||
                           targetPosScreenPoint.y <= borderSize+t || targetPosScreenPoint.y >= Screen.height - borderSize+y;


        if (isOffScreen)
        {
            pointerTrans.gameObject.SetActive(true);
            Vector3 cappedTargetPos = targetPosScreenPoint;
            if (cappedTargetPos.x <= borderSize) cappedTargetPos.x = borderSize;
            if (cappedTargetPos.x >= Screen.width-borderSize) cappedTargetPos.x = Screen.width-borderSize;
            if (cappedTargetPos.y <= borderSize) cappedTargetPos.y = borderSize+w;
            if (cappedTargetPos.y >= Screen.height-borderSize) cappedTargetPos.y = Screen.height-borderSize;


            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(cappedTargetPos);
            pointerTrans.position = pointerWorldPos;
            pointerTrans.localPosition = new Vector3(pointerTrans.localPosition.x, pointerTrans.localPosition.y, 0f);
        }
        else
        {
            pointerTrans.gameObject.SetActive(false);
        }
    }
}
