using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LivesUI : MonoBehaviour
    {
        [SerializeField] private Image filler;

        public void RefreshHealthBar()
        {
            filler.fillAmount = PlayerController.PlayerSettings.curHealth / PlayerController.PlayerSettings.maxHealth;
        }
    }
}
