using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(LevelsManager))]
[RequireComponent(typeof(ConditionsManager))]
[RequireComponent(typeof(DialogsManager))]

public class Managers : MonoBehaviour
{
    public static SceneManager Scene { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static LevelsManager Levels { get; private set; }
    public static ConditionsManager Conditions { get; private set; }
    public static DialogsManager Dialogs { get; private set; }

    private List<IGameManager> _startSequence;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Scene = GetComponent<SceneManager>();
        Inventory = GetComponent<InventoryManager>();
        Levels = GetComponent<LevelsManager>();
        Conditions = GetComponent<ConditionsManager>();
        Dialogs = GetComponent<DialogsManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Scene);
        _startSequence.Add(Inventory);
        _startSequence.Add(Levels);
        _startSequence.Add(Conditions);
        _startSequence.Add(Dialogs);

        StartCoroutine(StartupManager());
    }

    private IEnumerator StartupManager()
    {
        foreach(IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while(numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in _startSequence)
            {
                if(manager.status == ManagerStatus.Started)
                {
                    ++numReady;
                }
            }

            if(numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
            }

            yield return null;
        }
        
        Debug.Log("All managers started up");
        //Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }
}
