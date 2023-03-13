using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private float CellLength = 1f;
    [SerializeField] private int GridWidth = 19;
    [SerializeField] private int GridHeight = 19;
    private Dictionary<int, Vector2Int> _objectsPositions = new Dictionary<int, Vector2Int>();
    private readonly int EmptyId = 0;
    private readonly int WallId = 1;
    private int[] _grid;

    public void Startup()
    {
        _grid = new int[GridWidth * GridHeight];

        status = ManagerStatus.Started;
    }

    public bool TryMoveObject(int objectId, Vector2Int direction, out Vector2Int fixedDirection)
    {
        fixedDirection = direction;
        if(!_objectsPositions.ContainsKey(objectId))
        {
            throw new NoSuchObjectOnSceneException(objectId);
        }
        var objectPosition = _objectsPositions[objectId];
        FixDirection(objectPosition, ref fixedDirection);
        if(fixedDirection.x == 0 && fixedDirection.y == 0)
        {
            return false;
        }
        _grid[objectPosition.y * GridWidth + objectPosition.x] = EmptyId;//тут м/ю что-то особенное
        objectPosition.x += fixedDirection.x;
        objectPosition.y += fixedDirection.y;
        _grid[objectPosition.y * GridWidth + objectPosition.x] = objectId;
        _objectsPositions[objectId] = objectPosition;
        return true;
    }

    public void SetObjectPosition(int objectId, Vector3 sceneObjectPosition)
    {
        var objectPosition = new Vector2Int();
        objectPosition.x = (int)(sceneObjectPosition.x / CellLength);
        objectPosition.y = (int)(sceneObjectPosition.y / CellLength);
        if(objectPosition.x >= GridWidth || objectPosition.x < 0 || objectPosition.y >= GridHeight || objectPosition.y < 0)
        {
            throw new OutOfGridPositionException(objectPosition.x, objectPosition.y, GridWidth, GridHeight);
        }
        _grid[objectPosition.y * GridWidth + objectPosition.x] = objectId;
        _objectsPositions[objectId] = objectPosition;
    }

    public void SetObjectPosition(int objectId, Vector2Int objectPosition)
    {
        if(objectPosition.x >= GridWidth || objectPosition.x < 0 || objectPosition.y >= GridHeight || objectPosition.y < 0)
        {
            throw new OutOfGridPositionException(objectPosition.x, objectPosition.y, GridWidth, GridHeight);
        }
        _grid[objectPosition.y * GridWidth + objectPosition.x] = objectId;
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
            throw new NoSuchObjectOnSceneException(objectId);
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

    private void FixDirection(Vector2Int objectPosition, ref Vector2Int movement)
    {
        //обработать случай с диагональным движением
        if(objectPosition.x >= GridWidth || objectPosition.x < 0 || objectPosition.y >= GridHeight || objectPosition.y < 0)
        {
            throw new OutOfGridPositionException(objectPosition.x, objectPosition.y, GridWidth, GridHeight);
        }
        if(objectPosition.x + movement.x < GridWidth && objectPosition.x + movement.x >= 0 &&
            objectPosition.y + movement.y < GridHeight && objectPosition.y + movement.y >= 0 &&
            _grid[(objectPosition.y + movement.y) * GridWidth + objectPosition.x + movement.x] != EmptyId)
        {
            movement.x = 0;
            movement.y = 0;
            return;
        }
        if(objectPosition.x + movement.x >= GridWidth || objectPosition.x + movement.x < 0 || 
            _grid[objectPosition.y * GridWidth + objectPosition.x + movement.x] != EmptyId)
        {
            movement.x = 0;
        }
        if(objectPosition.y + movement.y >= GridHeight || objectPosition.y + movement.y < 0 || 
            _grid[(objectPosition.y + movement.y) * GridWidth + objectPosition.x] != EmptyId ||
            movement.x != 0)
        {
            movement.y = 0;
        }
    }
}
