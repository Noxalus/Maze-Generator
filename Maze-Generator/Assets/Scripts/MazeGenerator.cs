using System.Collections.Generic;
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

    [SerializeField] int _width = 1;
    [SerializeField] int _height = 1;
    [SerializeField] int _cellSize = 1;

    [SerializeField] GameObject _wallPrefab = null;
    [SerializeField] GameObject _exitPrefab = null;
    [SerializeField] Transform _mazeHolder = null;

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

        // Fill the maze with walls
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _maze[x, y] = CellType.Wall;
            }
        }

        var exitPosition = GenerateExitPosition();
        _maze[exitPosition.x, exitPosition.y] = CellType.Exit;

        GeneratePerfectMaze(exitPosition);

        CreateMazeMeshes();
    }

    private void GeneratePerfectMaze(Point initialPosition)
    {
        var currentPosition = initialPosition;
        var possibleCells = new List<Point>();
        var previousPositions = new Stack<Point>();

        previousPositions.Push(initialPosition);

        do
        {
            possibleCells.Clear();

            Point previousPosition = previousPositions.Peek();

            possibleCells = FindPossibleCells(currentPosition, initialPosition);

            if (possibleCells.Count > 0)
            {
                currentPosition = possibleCells[Random.Range(0, possibleCells.Count)];
                _maze[currentPosition.x, currentPosition.y] = CellType.Empty;
                previousPositions.Push(currentPosition);
            }
            else
            {
                currentPosition = previousPositions.Pop();
            }
        } while (currentPosition != initialPosition);
    }

    private List<Point> GetNeighbors(Point position)
    {
        var neighbors = new List<Point>();

        var upPosition = new Point(position.x, position.y + 1);
        var rightPosition = new Point(position.x + 1, position.y);
        var downPosition = new Point(position.x, position.y - 1);
        var leftPosition = new Point(position.x - 1, position.y);

        if (upPosition.y < _height)
            neighbors.Add(upPosition);
        if (rightPosition.x < _width)
            neighbors.Add(rightPosition);
        if (downPosition.y >= 0)
            neighbors.Add(downPosition);
        if (leftPosition.x >= 0)
            neighbors.Add(leftPosition);

        return neighbors;
    }

    private List<Point> FindPossibleCells(Point position, Point previousPosition)
    {
        var possibleCells = new List<Point>();

        var neighbors = GetNeighbors(position);
        neighbors.RemoveAll(p => p == previousPosition);

        foreach (var neighbor in neighbors)
        {
            var neighborPosition = new Point(neighbor.x, neighbor.y);
            if (_maze[neighbor.x, neighbor.y] == CellType.Wall && IsPossibleCell(neighborPosition, position))
            {
                possibleCells.Add(neighborPosition);
            }
        }

        return possibleCells;
    }

    private bool IsPossibleCell(Point position, Point excludedPosition)
    {
        var wallNumber = 0;

        var neighbors = GetNeighbors(position);
        neighbors.RemoveAll(p => p == excludedPosition);

        foreach (var neighbor in neighbors)
        {
            var neighborPosition = new Point(neighbor.x, neighbor.y);
            if (_maze[neighbor.x, neighbor.y] == CellType.Wall)
            {
                wallNumber++;
            }
        }

        return (wallNumber == 3);
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

    private void CreateMazeMeshes()
    {
        _mazeMeshes = new GameObject[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var position = new Vector3(_cellSize * x, 0f, _cellSize * y);
                var prefab = _wallPrefab;

                switch (_maze[x, y])
                {
                    case CellType.Empty:
                        prefab = null;
                        break;
                    case CellType.Exit:
                        prefab = _exitPrefab;
                        break;
                }

                if (prefab)
                    _mazeMeshes[x, y] = Instantiate(prefab, position, Quaternion.identity, _mazeHolder);
            }
        }
    }
}
