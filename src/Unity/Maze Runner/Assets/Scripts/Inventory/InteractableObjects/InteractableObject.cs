using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{    
    public abstract void OnInteract(GameObject player);
}