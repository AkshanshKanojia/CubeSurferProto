using UnityEngine;

public class InputManager : MonoBehaviour
{
    bool isOnPc = false;//implement cross platform funtionality for testing
    Vector3 tapPos;
    PlayerController playerCont;

    private void Start()
    {
#if UNITY_EDITOR
        isOnPc = true;
#elif PLATFORM_ANDROID
isOnPc = false;
#endif
        playerCont = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        GetInputs();
    }

    void GetInputs()//detects touch and key inputs and applies action as needed
    {
        if(isOnPc)
        {
            if(Input.GetMouseButtonDown(0))
            {
                tapPos = Input.mousePosition;
            }
            if(Input.GetMouseButton(0))
            {
                if (Vector3.Distance(tapPos, Input.mousePosition) > 0.2f)//move if mouse is dragged while avoiding floating points
                {
                    playerCont.MovePlayer(tapPos.x - Input.mousePosition.x);
                    tapPos = Input.mousePosition;
                }
            }
        }
        else
        {
            if(Input.touchCount>0)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    tapPos = Input.GetTouch(0).position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (Vector3.Distance(tapPos, Input.mousePosition) > 0.2f)//move if mouse is dragged while avoiding floating points
                    {
                        playerCont.MovePlayer(tapPos.x - Input.GetTouch(0).position.x);
                        tapPos = Input.GetTouch(0).position;
                    }
                }
            }
        }
    }
}
