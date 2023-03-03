using System.Collections;
using UnityEngine;

public class DialogsManager : MonoBehaviour, IGameManager
{
    //изменить способ нахождения имен и выведения текста
    [SerializeField] private float DialogCooldownTime = 0.2f;
    public ManagerStatus status { get; private set; }
    public string CurrentSentence { get { return _sentences[_currentSentenceIndex]; } }
    public string CurrentPerson { get { return _persons[_currentSentenceIndex]; } }
    public bool IsDialog { get; private set; } = false;
    private string[] _sentences;
    private string[] _persons;
    private int _currentSentenceIndex;
    private bool _canPressKey = false;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    void Update()
    {
        if(IsDialog && _canPressKey)
        {
            if(Input.anyKeyDown)
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

    public void StartDialog(string[] sentences, string[] persons)
    {
        if(sentences.Length <= 0)
        {
            return;
        }
        if(persons.Length != sentences.Length)
        {
            throw new BadDialogParametersException("Length of persons " + persons.Length + 
                " is not equal to sentences length " + sentences.Length);
        }
        _currentSentenceIndex = 0;
        _sentences = sentences;
        _persons = persons;
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
