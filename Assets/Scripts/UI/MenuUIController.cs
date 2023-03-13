using UnityEngine;

public class MenuUIController : BaseUIController
{
    [SerializeField] private GameObject NextWindow;
    [SerializeField] private GameObject MainWindow;

    void Start()
    {
        OnBack();
    }

    public override void OnBack()
    {
        MainWindow.SetActive(false);
        NextWindow.SetActive(true);
    }

    public override void OnNext()
    {
        MainWindow.SetActive(true);
        NextWindow.SetActive(false);
    }
}
