using UnityEngine;

public class LevelsManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }


    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void LoadScene(int sceneId)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
