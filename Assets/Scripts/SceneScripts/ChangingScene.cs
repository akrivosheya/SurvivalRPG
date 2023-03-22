using UnityEngine;

public class ChangingScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Font;
    [SerializeField] private float BlurSpeed;
    private bool _sceneIsBlack = false;
    private bool _needBlackScene = false;

    void Awake()
    {
        Messenger.AddListener(GameEvent.NEED_BLACK_SCENE, OnBlackScene);
        Messenger.AddListener(GameEvent.NEED_CLEAR_SCENE, OnClearScene);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.NEED_BLACK_SCENE, OnBlackScene);
        Messenger.RemoveListener(GameEvent.NEED_CLEAR_SCENE, OnClearScene);
    }

    void Update()
    {
        if(!_sceneIsBlack && _needBlackScene && !Managers.Dialogs.IsDialog)
        {
            if(Font != null)
            {
                var color = Font.color;
                color.a += Time.deltaTime * BlurSpeed;
                Font.color = color;
            }
            if(Font == null || Font.color.a >= 1)
            {
                _sceneIsBlack = true;
                Managers.Conditions.DeleteCondition("SCENE_IS_CHANGING");
                Messenger.Broadcast(GameEvent.BLACK_SCENE);
            }
        }
        else if(_sceneIsBlack && !_needBlackScene && !Managers.Dialogs.IsDialog)
        {
            if(Font != null)
            {
                var color = Font.color;
                color.a -= Time.deltaTime * BlurSpeed;
                Font.color = color;
            }
            if(Font == null || Font.color.a <= 0)
            {
                _sceneIsBlack = false;
                Managers.Conditions.DeleteCondition("SCENE_IS_CHANGING");
                //Messenger.Broadcast(GameEvent.CLEAR_SCENE);
            }
        }
    }

    void OnBlackScene()
    {
        Managers.Conditions.AddCondition("SCENE_IS_CHANGING");
        _needBlackScene = true;
    }

    void OnClearScene()
    {
        Managers.Conditions.AddCondition("SCENE_IS_CHANGING");
        _needBlackScene = false;
    }
}
