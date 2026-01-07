using UnityEngine;
using UnityEngine.UI;

public class DestinationButton : MonoBehaviour
{
    public RectTransform destinationPoint;
    public MapController mapController;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        mapController.MoveCharacterTo(destinationPoint);
    }
}
