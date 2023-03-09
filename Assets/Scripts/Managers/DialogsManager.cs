using System.Collections;
using UnityEngine;

public class DialogsManager : MonoBehaviour, IGameManager
{
    //изменить способ нахождения имен и выведения текста
    [SerializeField] private float DialogCooldownTime = 0.2f;
    public ManagerStatus status { get; private set; }
    public string CurrentSentence { get { return _sentences[_currentSentenceIndex]; } }
    public string CurrentPersonName { get { return _personsNames[_currentSentenceIndex]; } }
    public string CurrentPersonImage { get { return _personsImages[_currentSentenceIndex]; } }
    public bool IsDialog { get; private set; } = false;
    private string[] _sentences;
    private string[] _personsNames;
    private string[] _personsImages;
    private int _currentSentenceIndex;
    private bool _canPressKey = false;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    void Update()
    {
        if(IsDialog && _canPressKey && !Managers.Conditions["IS_PAUSE"])
        {
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                _canPressKey = false;
                ++_currentSentenceIndex;
                if(_currentSentenceIndex >= _sentences.Length)
                {
                    Messenger.Broadcast(GameEvent.DIALOG_ENDED);
                    StartCoroutine(StopDialog());
                }
                else
                {
                    Messenger.Broadcast(GameEvent.DIALOG_NEXT_SENTENCE);
                    StartCoroutine(AllowPressKey());
                }
            }
        }
    }

    public void StartDialog(string[] sentences, string[] personsNames, string[] personsImages)
    {
        if(sentences.Length <= 0)
        {
            return;
        }
        if(personsNames.Length != sentences.Length)
        {
            throw new BadDialogParametersException("Length of persons names " + personsNames.Length + 
                " is not equal to sentences length " + sentences.Length);
        }
        if(personsImages.Length != sentences.Length)
        {
            throw new BadDialogParametersException("Length of persons images " + personsImages.Length + 
                " is not equal to sentences length " + sentences.Length);
        }
        _currentSentenceIndex = 0;
        _sentences = sentences;
        _personsNames = personsNames;
        _personsImages = personsImages;
        IsDialog = true;
        Messenger.Broadcast(GameEvent.DIALOG_STARTED);
        StartCoroutine(AllowPressKey());
    }

    private IEnumerator StopDialog()
    {
        yield return new WaitForSeconds(DialogCooldownTime);

        IsDialog = false;
    }

    private IEnumerator AllowPressKey()
    {
        yield return new WaitForSeconds(DialogCooldownTime);

        _canPressKey = true;
    }
}
