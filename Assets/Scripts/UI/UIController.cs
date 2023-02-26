using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text NewItem;
    [SerializeField] private GameObject DialogWindow;
    [SerializeField] private Text Dialog;

    void Awake()
    {
        Messenger.AddListener(GameEvent.ITEM_ADDED, OnItemAdded);
        Messenger.AddListener(GameEvent.DIALOG_STARTED, OnDialogStarted);
        Messenger.AddListener(GameEvent.DIALOG_ENDED, OnDialogEnded);
        Messenger.AddListener(GameEvent.DIALOG_NEXT_SENTENCE, OnDialogNextSentence);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ITEM_ADDED, OnItemAdded);
        Messenger.RemoveListener(GameEvent.DIALOG_STARTED, OnDialogStarted);
        Messenger.RemoveListener(GameEvent.DIALOG_ENDED, OnDialogEnded);
        Messenger.RemoveListener(GameEvent.DIALOG_NEXT_SENTENCE, OnDialogNextSentence);
    }

    void Start()
    {
        DialogWindow.SetActive(false);
        NewItem.text = "";//константы
    }

    public void OnItemAdded()
    {
        NewItem.text = "You got " + Managers.Inventory.NewItem;//константы
        StartCoroutine(ClearText(NewItem));
    }

    public void OnDialogStarted()
    {
        DialogWindow.SetActive(true);
        Dialog.text = Managers.Dialogs.CurrentSentence;
    }

    public void OnDialogNextSentence()
    {
        Dialog.text = Managers.Dialogs.CurrentSentence;
    }

    public void OnDialogEnded()
    {
        DialogWindow.SetActive(false);
    }

    public void OnStart()
    {
        Managers.Levels.LoadScene(0);//что-то конкретное
    }

    public void OnExit()
    {
        Application.Quit();
    }

    private IEnumerator ClearText(Text text)
    {
        yield return new WaitForSeconds(1);//константы

        text.text = "";//константы
    }
}