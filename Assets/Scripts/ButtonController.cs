using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource audioSource;
    public Camera cam;
    
    [SerializeField] 
    private Vector2 localMousePosition;
    [SerializeField] 
    private Animator animator;
    [SerializeField]
    private ButtonState currentState;

    private bool transitionActive;
    
    private enum ButtonState
    {
        Pressed,
        Selected,
        Deselected,
    }

    // Start is called before the first frame update
    void Start()
    {
        transitionActive = false;
        switchtoState(ButtonState.Deselected);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ButtonState.Deselected:
                if (IsMouseOverButton())
                {
                    switchtoState(ButtonState.Selected);
                    break;
                }
                if (transitionActive)
                {
                    updateAnimator(false,false);
                    transitionActive = false;
                }
                break;
            case ButtonState.Selected:
                if (transitionActive)
                {
                    updateAnimator(true,false);
                    transitionActive = false;
                }
                break;
            case ButtonState.Pressed:
                if (transitionActive)
                {
                    transitionActive = false;
                }
                break;
        }
    }
    // Detect if the mouse is on the button
    private bool IsMouseOverButton()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        localMousePosition = Input.mousePosition;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    rectTransform, Input.mousePosition, cam, out localMousePosition);
        //Vector2 vc = new Vector2(0, 0);
        //Debug.Log(rectTransform.rect.Contains(vc));
        return rectTransform.rect.Contains(localMousePosition);
    }
    
    private void switchtoState(ButtonState newState)
    {
        transitionActive = true;
        currentState = newState;
    }

    private void updateAnimator(bool Selected, bool Pressed)
    {
        animator.SetBool("Selected", Selected);
        animator.SetBool("Pressed", Pressed);
    }
}
