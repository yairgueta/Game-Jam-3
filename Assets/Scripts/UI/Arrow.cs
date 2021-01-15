using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform sheepsTransform;
    private Vector3 sheepsPos;
    [SerializeField] private GameObject upArrowImg;
    [SerializeField] private GameObject downArrowImg;
    [SerializeField] private GameObject leftArrowImg;
    [SerializeField] private GameObject rightArrowImg;
    [SerializeField] private Camera camera;
    private float cameraWidth;
    void Start()
    {
        sheepsPos = sheepsTransform.position;
        cameraWidth = Screen.width;
        DisableArrows();
    }

    private void DisableArrows()
    {
        upArrowImg.SetActive(false);
        downArrowImg.SetActive(false);
        leftArrowImg.SetActive(false);
        rightArrowImg.SetActive(false);
    }

    void Update()
    {
        Vector2 screenBounds =
            camera.ScreenToWorldPoint(new Vector3(cameraWidth, Screen.height, camera.transform.position.z));
        
        DisableArrows();
        if (sheepsPos.x < screenBounds.x-25)
        {
            leftArrowImg.SetActive(true);
            Aim(leftArrowImg);
        }
  

        else if (sheepsPos.x > screenBounds.x)
        {
            rightArrowImg.SetActive(true);
            Aim(rightArrowImg);
        }

        else if (sheepsPos.y > screenBounds.y)
        {
            upArrowImg.SetActive(true);
            Aim(upArrowImg);
        }

        else if (sheepsPos.y < screenBounds.y - 15)
        {
            downArrowImg.SetActive(true);
            Aim(downArrowImg);
        }
    }
    
    
    private void Aim(GameObject arrow)
    {
        Vector3 aimDirection = (sheepsPos - arrow.transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        arrow.transform.eulerAngles = new Vector3(0,0, angle);
    }
}
