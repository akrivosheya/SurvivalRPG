using System.Collections;
using UnityEngine;

public class DialogsManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public string CurrentSentence { get { return _sentences[_currentSentenceIndex]; } }
    public bool IsDialog { get; private set; } = false;
    private string[] _sentences;
    private int _currentSentenceIndex;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    void Update()
    {
        if(IsDialog)
        {
            if(Input.anyKeyDown)
            {
                ++_currentSentenceIndex;
                if(_currentSentenceIndex >= _sentences.Length)
                {
                    Messenger.Broadcast(GameEvent.DIALOG_ENDED);
                    StartCoroutine(StopDialog());
                }
                else
                {
                    Messenger.Broadcast(GameEvent.DIALOG_NEXT_SENTENCE);
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
        _currentSentenceIndex = 0;
        _sentences = sentences;
        IsDialog = true;
        Messenger.Broadcast(GameEvent.DIALOG_STARTED);
    }

    private IEnumerator StopDialog()
    {
        yield return new WaitForSeconds(0.5f);//константа

        IsDialog = false;
    }
}
