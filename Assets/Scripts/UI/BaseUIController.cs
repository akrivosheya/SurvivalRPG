using UnityEngine;

public class BaseUIController : MonoBehaviour
{
    [SerializeField] protected string SceneForLoading;

    public virtual void OnStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneForLoading);
    }

    public virtual void OnExit()
    {
        Application.Quit();
    }

    public virtual void OnBack()
    {
    }

    public virtual void OnNext()
    {
    }
}
