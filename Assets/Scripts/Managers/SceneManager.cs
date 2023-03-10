using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private float CellLength = 1f;
    [SerializeField] private int SceneWidth = 19;
    [SerializeField] private int SceneHeight = 19;
    private Dictionary<int, Vector2Int> _objectsPositions = new Dictionary<int, Vector2Int>();
    private readonly int EmptyId = 0;
    private readonly int WallId = 1;
    private int[] _grid;

    public void Startup()
    {
        _grid = new int[SceneWidth * SceneHeight];

        /*for(int i = 0; i < SceneHeight; ++i)
        {
            for(int j = 0; j < SceneWidth; ++j)
            {
                if(i % (SceneHeight - 1) == 0 || j % (SceneWidth - 1) == 0)
                {
                    _grid[i * SceneWidth + j] = WallId;
                }
                else
                {
                    _grid[i * SceneWidth + j] = EmptyId;
                }
            }
        }*/

        status = ManagerStatus.Started;
    }

    public bool TryMoveObject(int objectId, Vector2Int direction, out Vector2Int fixedDirection)
    {
        fixedDirection = direction;
        if(!_objectsPositions.ContainsKey(objectId))
        {
            //желательно обрабтать
            return false;
        }
        var objectPosition = _objectsPositions[objectId];
        FixDirection(objectPosition, ref fixedDirection);
        if(fixedDirection.x == 0 && fixedDirection.y == 0)
        {
            return false;
        }
        _grid[objectPosition.y * SceneWidth + objectPosition.x] = EmptyId;//тут м/ю что-то особенное
        objectPosition.x += fixedDirection.x;
        objectPosition.y += fixedDirection.y;
        _grid[objectPosition.y * SceneWidth + objectPosition.x] = objectId;
        _objectsPositions[objectId] = objectPosition;
        return true;
    }

    public void SetObjectPosition(int objectId, Vector3 sceneObjectPosition)
    {
        var objectPosition = new Vector2Int();
        objectPosition.x = (int)(sceneObjectPosition.x / CellLength);
        objectPosition.y = (int)(sceneObjectPosition.y / CellLength);
        _grid[objectPosition.y * SceneWidth + objectPosition.x] = objectId;
        _objectsPositions[objectId] = objectPosition;
    }

    public void SetObjectPosition(int objectId, Vector2Int gridObjectPosition)
    {
        var objectPosition = new Vector2Int();
        objectPosition.x = gridObjectPosition.x;
        objectPosition.y = gridObjectPosition.y;
        _grid[objectPosition.y * SceneWidth + objectPosition.x] = objectId;
        _objectsPositions[objectId] = objectPosition;//не для всех объектов
    }

    public void DeleteObject(Vector3 sceneObjectPosition)
    {
        SetObjectPosition(EmptyId, sceneObjectPosition);
    }

    public void DeleteObject(int objectId)
    {
        if(!_objectsPositions.ContainsKey(objectId))
        {
            return;
        }
        SetObjectPosition(EmptyId, _objectsPositions[objectId]);
    }

    public Vector3 GetObjectScenePosition(int objectId)
    {
        if(!_objectsPositions.ContainsKey(objectId))
        {
            throw new NoSuchObjectOnSceneException(objectId);
        }
        var objectPosition = _objectsPositions[objectId];
        return new Vector3(objectPosition.x * CellLength, objectPosition.y * CellLength);
    }

    private void FixDirection(Vector2Int position, ref Vector2Int direction)
    {
        //обработать случай с диагональным движением
        if(position.x + direction.x < SceneWidth && position.x + direction.x >= 0 &&
            position.y + direction.y < SceneHeight && position.y + direction.y >= 0 &&
            _grid[(position.y + direction.y) * SceneWidth + position.x + direction.x] != EmptyId)
        {
            direction.x = 0;
            direction.y = 0;
            return;
        }
        if(position.x + direction.x >= SceneWidth || position.x + direction.x < 0 || 
            _grid[position.y * SceneWidth + position.x + direction.x] != EmptyId)
        {
            direction.x = 0;
        }
        if(position.y + direction.y >= SceneHeight || position.y + direction.y < 0 || 
            _grid[(position.y + direction.y) * SceneWidth + position.x] != EmptyId)
        {
            direction.y = 0;
        }
    }
}
