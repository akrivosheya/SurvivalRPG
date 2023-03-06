using UnityEngine;

public class BaseUIController : MonoBehaviour
{
    [SerializeField] private string SceneForLoading;

    public void OnStart()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneForLoading);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
