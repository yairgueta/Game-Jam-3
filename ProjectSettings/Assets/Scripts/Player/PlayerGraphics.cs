using System;
using UnityEngine;

namespace Player
{
    public class PlayerGraphics : MonoBehaviour
    {
        [SerializeField] private Player player;
    
        private Animator anim;
        private static readonly int AnimMoveX = Animator.StringToHash("AnimMoveX");
        private static readonly int AnimMoveY = Animator.StringToHash("AnimMoveY");
        private static readonly int AnimMoveMagnitude = Animator.StringToHash("AnimMoveMagnitude");
        private static readonly int AnimLastMoveX = Animator.StringToHash("AnimLastMoveX");
        private static readonly int AnimLastMoveY = Animator.StringToHash("AnimLastMoveY");

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            Animate();
        }
    
        private void Animate()
        {
            anim.SetFloat(AnimMoveX, player.MoveDirection.x);
            anim.SetFloat(AnimMoveY, player.MoveDirection.y);
            anim.SetFloat(AnimMoveMagnitude, player.MoveDirection.sqrMagnitude);
            anim.SetFloat(AnimLastMoveX, player.LastMoveDirection.x);
            anim.SetFloat(AnimLastMoveY, player.LastMoveDirection.y);
        }
    }
}
