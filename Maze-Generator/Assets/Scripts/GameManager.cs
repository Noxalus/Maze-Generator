using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MazeGenerator _mazeGenerator;
    [SerializeField] private BoardController _boardController;
    [SerializeField] private Player _player;

    void Start()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        _mazeGenerator.GenerateMaze();

        var mazeOrigin = _mazeGenerator.GetOrigin();

        var newCameraPosition = _mainCamera.transform.localPosition;
        const float cameraHeightFactor = 1.75f;
        newCameraPosition.x = mazeOrigin.x;
        newCameraPosition.y = _mazeGenerator.Width * cameraHeightFactor;
        newCameraPosition.z = mazeOrigin.y;
        _mainCamera.transform.localPosition = newCameraPosition;

        _boardController.Initialize();
        _boardController.SetOrigin(mazeOrigin);

        _player.Initialize(_mazeGenerator.GeneratePlayerPosition());

        DeviceRotation.UpdateReferenceOrientation();
    }

    public void OnGenerateMazeButtonClicked()
    {
        GenerateMaze();
    }
}
