using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : BaseUIController
{
    [SerializeField] private Text NewItem;
    [SerializeField] private GameObject DialogWindow;
    [SerializeField] private Text Dialog;
    [SerializeField] private Text PersonName;
    [SerializeField] private Image PersonImage;
    [SerializeField] private Color NoPersonColor;
    [SerializeField] private Color PersonColor;
    [SerializeField] private float WaitToClearTextSeconds = 1;
    private string _emptyText = "";

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
        if(DialogWindow == null)
        {
            return;
        }
        DialogWindow.SetActive(false);
        NewItem.text = _emptyText;
    }

    public void OnItemAdded()
    {
        NewItem.text = "You got " + Managers.Inventory.NewItem;
        StartCoroutine(ClearText(NewItem));
    }

    public void OnDialogStarted()
    {
        DialogWindow.SetActive(true);
        OnDialogNextSentence();
    }

    public void OnDialogNextSentence()
    {
        Dialog.text = Managers.Dialogs.CurrentSentence;
        PersonName.text = Managers.Dialogs.CurrentPersonName;
        if(Managers.Dialogs.CurrentPersonImage.Equals(_emptyText))
        {
            PersonImage.color = NoPersonColor;
            PersonImage.sprite = null;
        }
        else
        {
            PersonImage.color = PersonColor;
            PersonImage.sprite = Resources.Load<Sprite>(Managers.Dialogs.CurrentPersonImage);
        }
    }

    public void OnDialogEnded()
    {
        DialogWindow.SetActive(false);
    }

    private IEnumerator ClearText(Text text)
    {
        yield return new WaitForSeconds(WaitToClearTextSeconds);

        text.text = _emptyText;
    }
}
