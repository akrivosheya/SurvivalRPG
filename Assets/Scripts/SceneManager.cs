using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public int PlayerPositionX { get; private set; } = 9;
    public int PlayerPositionY { get; private set; } = 9;
    private readonly int WallId = -1;
    private readonly int PlayerId = 1;
    private int[] _scene;

    public void Startup()
    {
        _scene = new int[19 * 19];

        for(int i = 0; i < 19; ++i)
        {
            for(int j = 0; j < 19; ++j)
            {
                if(i == PlayerPositionX && j == PlayerPositionY)
                {
                    _scene[i * 19 + j] = PlayerId;
                }
                else if(i % 18 == 0 || j % 18 == 0)
                {
                    _scene[i * 19 + j] = WallId;
                }
            }
        }

        status = ManagerStatus.Started;
    }

    public void FixDirection(ref int directionX, ref int directionY)
    {
        if(PlayerPositionX + directionX >= 19 || PlayerPositionX + directionX < 0 || 
            _scene[(PlayerPositionX + directionX) * 19 + PlayerPositionY] == WallId)
        {
            directionX = 0;
        }
        if(PlayerPositionY + directionY >= 19 || PlayerPositionY + directionY < 0 || 
            _scene[PlayerPositionX * 19 + PlayerPositionY + directionY] == WallId)
        {
            directionY = 0;
        }
    }
}
