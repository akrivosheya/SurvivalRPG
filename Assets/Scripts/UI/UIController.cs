using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : BaseUIController
{
    [SerializeField] private GameObject DialogWindow;
    [SerializeField] private GameObject PauseWindow;
    [SerializeField] private Text Dialog;
    [SerializeField] private Text PersonName;
    [SerializeField] private Image PersonImage;
    [SerializeField] private Color NoPersonColor;
    [SerializeField] private Color PersonColor;
    [SerializeField] private float WaitToClearTextSeconds = 1;
    private string _emptyText = "";

    void Awake()
    {
        Messenger.AddListener(GameEvent.DIALOG_STARTED, OnDialogStarted);
        Messenger.AddListener(GameEvent.DIALOG_ENDED, OnDialogEnded);
        Messenger.AddListener(GameEvent.DIALOG_NEXT_SENTENCE, OnDialogNextSentence);
    }

    void OnDestroy()
    {
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
        PauseWindow.SetActive(false);
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !Managers.Conditions["IS_ENDING"])
        {
            OnPause();
        }
    }

    public override void OnExit()
    {
        Managers.Levels.LoadScene(SceneForLoading);
    }

    public void OnDialogStarted()
    {
        DialogWindow.SetActive(true);
        OnDialogNextSentence();
    }

    public void OnPause()
    {
        PauseWindow.SetActive(true);
        Managers.Conditions.AddCondition("IS_PAUSE");
    }

    public void OnResume()
    {
        PauseWindow.SetActive(false);
        Managers.Conditions.DeleteCondition("IS_PAUSE");
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
