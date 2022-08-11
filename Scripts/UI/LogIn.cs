using UnityEngine;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{
    [SerializeField] int buildIndex; 
    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(buildIndex + 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
