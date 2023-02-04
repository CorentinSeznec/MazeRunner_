using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntObjectDetector : MonoBehaviour
{
    [SerializeField] float interactionRadius = 4f;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Transform handler;

    public delegate bool FuncDelegate(GameObject obj);

    public float GetInteractionRadius()
    {
        return interactionRadius;
    }

    bool IsInteractable(GameObject obj)
    {
        return obj.GetComponent<InteractableObject>() != null;
    }

    bool IsItem(GameObject obj)
    {   
        return obj.GetComponent<InteractablePickableObject>() != null;
    }

    bool IsOptionalWall(GameObject obj)
    {
        return obj.GetComponent<OptionalWall>() != null;
    }

    bool IsOrb(GameObject obj)
    {
        return obj.GetComponent<InteractableOrb>() != null;
    }

    bool IsPedestal(GameObject obj)
    {
        InteractableOrbPiedestal p = obj.GetComponent<InteractableOrbPiedestal>();
        if(p  != null && p.GetLayerHold() == gameObject.layer)
        {
            return true;
        }
        return false;
    }

    bool IsPortal(GameObject obj)
    {
        return obj.GetComponent<InteractablePortal>() != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(handler.position, interactionRadius);

    }

    public Collider[] GetColliders(float radius)
    {
        return Physics.OverlapSphere(handler.position, radius, layerMask);
    }

    public List<GameObject> GetList(FuncDelegate f, float radius)
    {
        List<GameObject> ret = new List<GameObject>();
        Collider[] colliders = GetColliders(radius);
        foreach(Collider c in colliders)
        {
            GameObject obj = c.gameObject;
            if(f(obj))
            {
                ret.Add(obj);
            }
        }
        return ret;
    }

    public List<GameObject> GetItemList(float radius)
    {
        return GetList(IsItem, radius);
    }

    public List<GameObject> GetOptionalWallList(float radius)
    {
        return GetList(IsOptionalWall, radius);
    }

    public List<GameObject> GetOrbList(float radius)
    {
        return GetList(IsOrb, radius);
    }

    public List<GameObject> GetPedestalsList(float radius)
    {
        return GetList(IsPedestal, radius);
    }

    public GameObject GetPortal(float radius)
    {
        Collider[] colliders = GetColliders(radius);
        foreach(Collider c in colliders)
        {
            GameObject obj = c.gameObject;
            if(IsPortal(obj))
            {
                return obj;
            }
        }
        return null;
    }

    public bool InteractWith(GameObject obj)
    {
        List<GameObject> objects= GetList(IsInteractable, interactionRadius);
        int index = objects.FindIndex(x => x.name == obj.name);
        if(index != -1)
        {
            objects[index].GetComponent<InteractableObject>().OnInteract(gameObject);
            return true;
        }
        return false;
    }
}
