using UnityEngine;

public class ToggleAnimationOnTab : MonoBehaviour
{
    private Animator animator;
    private bool toggleState = false;  // false = animation1, true = animation2

    // Animation trigger or state names - set these to match your Animator
    [SerializeField] private string animationState1 = "Animation1";
    [SerializeField] private string animationState2 = "Animation2";

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("ToggleAnimationOnTab: No Animator component found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleState = !toggleState;
            if (toggleState)
            {
                animator.Play(animationState2);
            }
            else
            {
                animator.Play(animationState1);
            }
        }
    }
}

