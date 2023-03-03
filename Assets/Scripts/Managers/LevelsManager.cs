using UnityEngine;

public class LevelsManager : MonoBehaviour, IGameManager
{
    //список доступных уровней
    public ManagerStatus status { get; private set; }


    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
