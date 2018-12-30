using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private enum CellType
    {
        Empty = 0,
        Wall = 1,
        Start = 2,
        Exit = 3,
    }

    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] int _cellSize;

    [SerializeField] GameObject _wallPrefab;
    [SerializeField] GameObject _exitPrefab;
    [SerializeField] Transform _mazeHolder;

    private CellType[,] _maze;
    private GameObject[,] _mazeMeshes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_width < 1)
            _width = 1;
        if (_height < 1)
            _height = 1;
        if (_cellSize < 1)
            _cellSize = 1;
    }
#endif

    void Start()
    {
        GenerateMaze();
    }

    private void ClearMaze()
    {
        if (_mazeMeshes == null)
            return;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_mazeMeshes[x, y] != null)
                {
                    Destroy(_mazeMeshes[x, y]);
                }
            }
        }
    }

    public void GenerateMaze()
    {
        // Clear current maze
        ClearMaze();

        _maze = new CellType[_width, _height];
        _mazeMeshes = new GameObject[_width, _height];

        // Fill the maze with walls
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _maze[x, y] = CellType.Wall;
            }
        }

        var exitPosition = GenerateExitPosition();
        Debug.Log("Exit position: " + exitPosition);
        _maze[exitPosition.x, exitPosition.y] = CellType.Exit;

        Create3DStructure();
    }

    private Point GenerateExitPosition()
    {
        var exitPosition = new Point(Random.Range(0, _width), Random.Range(0, _height));

        if (exitPosition.x > _width / 2f)
        {
            if (exitPosition.y > exitPosition.x)
                exitPosition = new Point(exitPosition.x, _height - 1);
            else
                exitPosition = new Point(_width - 1, exitPosition.y);
        }
        else
        {
            if (exitPosition.y > exitPosition.x)
                exitPosition = new Point(0, exitPosition.y);
            else
                exitPosition = new Point(exitPosition.x, 0);
        }

        return exitPosition;
    }

    private void Create3DStructure()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var position = new Vector3(_cellSize * x, 0f, _cellSize * y);
                var prefab = _wallPrefab;

                switch (_maze[x, y])
                {
                    case CellType.Exit:
                        prefab = _exitPrefab;
                        break;
                }

                _mazeMeshes[x, y] = Instantiate(prefab, position, Quaternion.identity, _mazeHolder);
            }
        }
    }
}
