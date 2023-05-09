using UnityEngine;

public class animal_movement : MonoBehaviour
{
    public float moveSpeed = 1f; // The speed at which the animal moves
    public float roamRadius = 5f; // The radius within which the animal can roam
    public float roamTimer = 3f; // The time in seconds that the animal roams before changing direction

    private float timer; // The current timer for the animal's roam time
    private Vector3 roamTarget; // The current target location for the animal's roam
    private Animator animator; // The animator component attached to the animal GameObject
    private bool isEating = false;

    void Start()
    {
        // Set the initial roam target to a random location within the roam radius
        roamTarget = transform.position + Random.insideUnitSphere * roamRadius;
        roamTarget.y = transform.position.y;

        // Get the animator component attached to the animal GameObject
        animator = GetComponent<Animator>();

        // Register an animation event to set isEating to false at the end of the Eat animation
        AnimationClip eatClip = animator.runtimeAnimatorController.animationClips[1]; // Get the Eat animation clip
        AnimationEvent endEvent = new AnimationEvent();
        endEvent.time = eatClip.length; // Set the event time to the end of the animation clip
        endEvent.functionName = "OnEatEnd"; // Set the event function to call when the event is triggered
        eatClip.AddEvent(endEvent); // Add the event to the animation clip
    }

    void Update()
{
    // Move the animal towards the roam target
    transform.position = Vector3.MoveTowards(transform.position, roamTarget, moveSpeed * Time.deltaTime);

    // If the animal reaches the roam target, select a new roam target and reset the timer
    if (Vector3.Distance(transform.position, roamTarget) < 0.1f)
    {
        timer += Time.deltaTime;
        if (timer >= roamTimer)
        {
            roamTarget = transform.position + Random.insideUnitSphere * roamRadius;
            roamTarget.y = transform.position.y;
            timer = 0f;
        }
    }

    // Rotate the animal towards the direction it is moving
    if (transform.position != roamTarget)
    {
        transform.rotation = Quaternion.LookRotation(roamTarget - transform.position);
        animator.Play("Walk"); // Play the "Walk" animation when moving
    }
    else
    {
        if (!isEating)
        {
            if (Random.value < 0.5f)
            {
                animator.Play("Idle"); // Play the "Idle" animation
            }
            else
            {
                isEating = true;
                animator.Play("Eat"); // Play the "Eat" animation
            }
        }
        else
        {
            animator.Play("Eat"); // Keep playing the "Eat" animation while the animal is eating
        }
    }
}

    // Animation event function that sets isEating to false at the end of the Eat animation
    void OnEatEnd()
    {
        isEating = false;
        animator.Play("Idle"); // Play the "Idle" animation after the Eat animation ends
    }
}
