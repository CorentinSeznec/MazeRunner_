using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] float minSpawn = 5;
    [SerializeField] float maxSpawn = 10;
    [SerializeField] int maxItemOnGround = 100;
    private float cooldown;

    private ItemDatabase database;
    private Labyrinth labyrinth;
    private List<int> rarityRate;
    private int rateSum;

    private int globalId;
    private List<int> positionItem;
    private int itemOnGround;

    private List<GameObject> path;

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        labyrinth = GameObject.Find("Labyrinth").GetComponent<Labyrinth>();
        path = labyrinth.GetItemBlocks();
        rarityRate = database.GetRarityRatio();

        cooldown = Random.Range(minSpawn, maxSpawn);
        globalId = 1;

        itemOnGround = 0;

        positionItem = new List<int>(path.Count);
        for (int i=0; i < path.Count; ++i)
        {
            positionItem.Add(-1);
        }

        rateSum = 0;
        foreach (int i in rarityRate)
        {
            rateSum = rateSum + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (itemOnGround < maxItemOnGround)
        {
            cooldown = cooldown - Time.deltaTime;
            if (cooldown < 0)
            {
                // Spawn Item
                int raritySpawn = GetRaritySpawn();
                Item item = GetRandomItemByRarity(raritySpawn);

                // Get ItemHandler List and spawn item
                if (item != null)
                {
                    InGameItem model = item.GetInHandModel().GetComponent<InGameItem>();
                    if (model != null)
                    {
                        int rand;
                        do {
                            rand = Random.Range(0, path.Count - 1);
                        } while (positionItem[rand] != -1);

                        GameObject handler = path[rand].transform.GetChild(0).gameObject;
                        GameObject temp = model.CreateItem(handler.transform, ++globalId);
                        temp.transform.parent = transform;
                        positionItem[rand] = globalId;
                        ++itemOnGround;
                    }
                }

                // New Cooldown
                cooldown = Random.Range(minSpawn, maxSpawn);
            }
        }
    }

    public void PickedItem(int id)
    {
        int removeId = positionItem.FindIndex(x => x == id);
        positionItem.RemoveAt(removeId);
        --itemOnGround;
    }

    public List<GameObject> GetSpawnedItems()
    {
        List<GameObject> spawned = new List<GameObject>();
        for(int i = 0 ; i < positionItem.Count ; i++)
        {
            if(positionItem[i] != -1)
            {
                GameObject item = path[i].transform.GetChild(0).transform.GetChild(0).gameObject;
                spawned.Add(item);
            }
        }
        return spawned;
    }

    private int GetRaritySpawn()
    {
        int rand = Random.Range(1, rateSum);
        int rarity = 0;
        foreach (int i in rarityRate)
        {
            rand = rand - i;
            if (rand <= 0)
            {
                return rarity;
            }
            else
            {
                ++rarity;
            }
        }
        return -1;
    }

    private Item GetRandomItemByRarity(int rarity)
    {
        if (rarity < 0)
        {
            return null;
        }
        List<Item> itemList = database.GetItemsByRarity(rarity);
        int count = itemList.Count;
        if (count < 1)
        {
            return null;
        }
        else
        {
            int index = Random.Range(0, count);
            return itemList[index];
        }
    }
}
