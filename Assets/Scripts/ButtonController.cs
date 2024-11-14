using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler 
{
    public AudioSource audioSource;
    
    private Vector2 localMousePosition;
    [SerializeField] 
    private Animator animator;
    [SerializeField]
    private ButtonState currentState;

    private bool transitionActive;
    private bool onButton;
    
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
                if (transitionActive)
                {
                    onButton = false;
                    updateAnimator(false,false);
                    transitionActive = false;
                }
                break;
            case ButtonState.Selected:
                if (transitionActive)
                {
                    onButton = true;
                    updateAnimator(true,false);
                    transitionActive = false;
                }
                break;
            case ButtonState.Pressed:
                if (transitionActive)
                {
                    transitionActive = false;
                    updateAnimator(true,true);
                }

                if (this.transform.name == "StartButton")
                {
                    SceneManager.LoadScene("MainScene");
                }
                else if (this.transform.name == "QuitButton")
                {
                    Application.Quit();
                }

                break;
        }
    }

    // Detect if the mouse is on the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        switchtoState(ButtonState.Selected);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        switchtoState(ButtonState.Deselected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onButton)
        {
            switchtoState(ButtonState.Pressed);
        }
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
