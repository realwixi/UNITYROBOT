using UnityEngine;

public class AssemblyManager : MonoBehaviour
{
    public int totalParts = 4; // change based on your model
    private int snappedParts = 0;

    public Animator robotAnimator; // reference to robot animator

    public void PartSnapped()
    {
        snappedParts++;

        if (snappedParts >= totalParts)
        {
            Debug.Log("Robot Fully Assembled!");
            PlayRobotAnimation();
        }
    }

    void PlayRobotAnimation()
    {
        if (robotAnimator != null)
        {
            robotAnimator.SetTrigger("Assembled");
        }
    }
}
