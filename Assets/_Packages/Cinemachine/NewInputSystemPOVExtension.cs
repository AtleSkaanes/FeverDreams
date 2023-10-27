using UnityEngine;
using Cinemachine;

public class NewInputSystemPOVExtension : CinemachineExtension
{
    private InputController inputController;

    protected override void Awake()
    {
        
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
