using System;
using UnityEngine;

namespace Player
{
    public class Sheep : MonoBehaviour
    {
        [SerializeField] private Sprite awake, sleep;
        private SpriteRenderer sr;
        
        void Start()
        {
            transform.parent = SheepManager.Instance.transform;
            SheepManager.Instance.sheeps.Add(this);
            sr = GetComponent<SpriteRenderer>();
        }
        
        internal void Sleep()
        {
            sr.sprite = sleep;
        }

        internal void WakeUp()
        {
            sr.sprite = awake;
        }
    }
}
