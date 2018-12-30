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
    [SerializeField] Transform _mazeHolder;

    private CellType[,] _maze;

    void Start()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        _maze = new CellType[_width, _height];

        // Fill the maze with walls
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _maze[x, y] = CellType.Wall;
            }
        }

        Create3DStructure();
    }

    private void Create3DStructure()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_maze[x, y] == CellType.Wall)
                {
                    var position = new Vector3(_cellSize * x, 0f, _cellSize * y);
                    Instantiate(_wallPrefab, position, Quaternion.identity, _mazeHolder);
                }
            }
        }
    }
}
