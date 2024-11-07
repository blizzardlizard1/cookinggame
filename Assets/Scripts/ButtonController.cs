using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] 
    private Animator animator;
    [SerializeField]
    private ButtonState currentState;
    
    private enum ButtonState
    {
        Pressed,
        Selected,
        Deselected,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
