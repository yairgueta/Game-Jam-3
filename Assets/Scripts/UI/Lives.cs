using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    [SerializeField] private Text livesText;



    public void ShowLives(float lives)
    {
        livesText.text = lives.ToString();
    }
    



}
