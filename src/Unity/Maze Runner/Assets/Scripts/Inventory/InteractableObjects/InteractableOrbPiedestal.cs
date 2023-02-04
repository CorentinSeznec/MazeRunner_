using UnityEngine;

public class InteractableOrbPiedestal : InteractableObject
{
    [SerializeField] GameObject orbObject;

    private GameObject orbHandler;

    private GameObject orb = null;

    private GameManager gameManager;

    [SerializeField] int layerHold = 0;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        orbHandler = transform.Find("orbHandler").gameObject;
        if(layerHold == 0)
        {
            gameObject.layer = 2;
        }
    }

    public int GetLayerHold()
    {
        return layerHold;
    }

    public bool HoldOrb()
    {
        return orb != null;
    }

    public void Clear()
    {
        if(orb != null)
        {
            Destroy(orb);
            orb = null;
        }
    }

    public bool SpawnOrb()
    {
        if(orb == null)
        {
            orb = Instantiate(orbObject, orbHandler.transform.position, orbHandler.transform.rotation);
            orb.transform.parent = transform;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnInteract(GameObject player)
    {
        if(!HoldOrb() && player.layer == layerHold)
        {
            OrbManager orbManager = player.GetComponent<OrbManager>();
            if(orbManager.HasOrb())
            {
                orbManager.DestroyOrb();
                SpawnOrb();
                orb.transform.GetChild(0).gameObject.layer = 2;
                gameManager.IPlacedTheOrb(player);
                gameObject.layer = 2;
            }
        }
    }
}