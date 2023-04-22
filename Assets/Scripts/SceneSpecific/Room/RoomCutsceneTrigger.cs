using UnityEngine;
using Cinemachine;

public class RoomCutsceneTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera newCamera;
    public CinemachineVirtualCamera exitCamera;
    private void OnTriggerEnter(Collider other)
    {
        CameraSwitcher.instance.SwitchCamera(newCamera);
    }
    private void OnTriggerExit(Collider other)
    {
        CameraSwitcher.instance.SwitchCamera(exitCamera);
    }
}
