using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LoseScreen : MonoBehaviour
    {

        [SerializeField] private Button mainMenuButton;

        public void InitButtons(UnityAction mainMenuClick)
        {
            mainMenuButton.onClick.AddListener(mainMenuClick);
        }
    }
}
