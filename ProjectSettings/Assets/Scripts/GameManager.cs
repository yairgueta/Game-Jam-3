using Collectables;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
