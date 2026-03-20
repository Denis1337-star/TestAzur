using UnityEngine;

public class UIButtonAnim : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ReleseAnim()
    {
        animator.SetTrigger("Release");
    }
}
