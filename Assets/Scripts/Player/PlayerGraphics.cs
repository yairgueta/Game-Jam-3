using System;
using System.Collections;
using Selections;
using UnityEngine;

namespace Player
{
    public class PlayerGraphics : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
    
        private Animator anim;
        private static readonly int AnimMoveX = Animator.StringToHash("AnimMoveX");
        private static readonly int AnimMoveY = Animator.StringToHash("AnimMoveY");
        private static readonly int AnimMoveMagnitude = Animator.StringToHash("AnimMoveMagnitude");
        private static readonly int AnimLastMoveX = Animator.StringToHash("AnimLastMoveX");
        private static readonly int AnimLastMoveY = Animator.StringToHash("AnimLastMoveY");

        private void Awake()
        {
            anim = GetComponent<Animator>();
            anim.SetFloat("AnimMoveMagnitude", 0f);
        }

        private void Update()
        {
            Animate();
        }

        private void Animate()
        {
            anim.SetFloat(AnimMoveX, playerController.MoveDirection.x);
            anim.SetFloat(AnimMoveY, playerController.MoveDirection.y);
            anim.SetFloat(AnimMoveMagnitude, playerController.MoveDirection.sqrMagnitude);
            anim.SetFloat(AnimLastMoveX, playerController.LastMoveDirection.x);
            anim.SetFloat(AnimLastMoveY, playerController.LastMoveDirection.y);
        }
    }
}
