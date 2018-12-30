using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MazeGenerator _mazeGenerator;
    [SerializeField] private BoardController _boardController;

    void Start()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        _mazeGenerator.GenerateMaze();

        var boardControllerOrigin = Vector2.zero;
        boardControllerOrigin.x = ((_mazeGenerator.Width * _mazeGenerator.CellSize) / 2f) - (_mazeGenerator.CellSize / 2f);
        boardControllerOrigin.y = ((_mazeGenerator.Height * _mazeGenerator.CellSize) / 2f) - (_mazeGenerator.CellSize / 2f);

        //_boardController.SetOrigin(boardControllerOrigin);
        var newCameraPosition = _mainCamera.transform.localPosition;
        const float cameraHeightFactor = 1.3f;
        newCameraPosition.y = _mazeGenerator.Width * cameraHeightFactor;

        _mainCamera.transform.localPosition = newCameraPosition;
    }

    public void OnGenerateMazeButtonClicked()
    {
        GenerateMaze();
    }
}
