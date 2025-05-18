using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    void Start()
    {
        var delay = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        
        // Check if this is the dashEffect from PlayerData
        if (gameObject.name.Contains("Dash Effect"))
        {
            // Just deactivate instead of destroying
            Invoke("DeactivateObject", delay);
        }
        else
        {
            // For other effects, destroy as usual
            Destroy(gameObject, delay);
        }
    }
    
    void DeactivateObject()
    {
        Destroy(gameObject);
    }
}
