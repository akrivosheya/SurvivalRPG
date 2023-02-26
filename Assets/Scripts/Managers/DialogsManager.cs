using System.Collections;
using UnityEngine;

public class DialogsManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public string CurrentSentence { get { return _sentences[_currentSentenceIndex]; } }
    public bool IsDialog { get; private set; } = false;
    private string[] _sentences;
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

    public void StartDialog(string[] sentences)
    {
        if(sentences.Length <= 0)
        {
            return;
        }
        Debug.Log("Start dialog: " + sentences[0]);
        _currentSentenceIndex = 0;
        _sentences = sentences;
        IsDialog = true;
        Messenger.Broadcast(GameEvent.DIALOG_STARTED);
        StartCoroutine(AllowPressKey());
    }

    private IEnumerator StopDialog()
    {
        yield return new WaitForSeconds(0.5f);//константа

        IsDialog = false;
    }

    private IEnumerator AllowPressKey()
    {
        yield return new WaitForSeconds(0.5f);//константа

        _canPressKey = true;
    }
}
