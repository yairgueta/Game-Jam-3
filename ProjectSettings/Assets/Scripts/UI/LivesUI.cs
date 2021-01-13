using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LivesUI : MonoBehaviour
    {
        [SerializeField] private Text livesText;
        
        public void ShowLives(float lives)
        {
            livesText.text = Mathf.FloorToInt(lives).ToString();
        }
    }
}
