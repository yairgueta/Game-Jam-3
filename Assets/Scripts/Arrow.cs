using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private Transform sheepsTransform;
    private Vector3 sheepsPos;
    

    void Start()
    {
        sheepsPos = sheepsTransform.position;
    }

    void Update()
    {
        Aiming();
    }
    
    
    private void Aiming()
    {
        Vector3 aimDirection = (sheepsPos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle);
    }
}
