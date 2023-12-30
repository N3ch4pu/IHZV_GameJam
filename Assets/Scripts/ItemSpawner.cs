using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
    public GameObject ore;
    public GameObject ingot;


    public GameObject CreateItem(string type, bool isOre, Transform transform)
    {
        GameObject item;
        Color color;

        if (isOre)
        {
            item = CreateItem(ore, transform);
            if (type == "Tin") { color = new Color(0.6f, 0.6f, 0.7f); }
            else if (type == "Coal") { color = new Color(0.1f, 0.1f, 0.1f); }
            else if (type == "Gold") { color = new Color(1.0f, 0.85f, 0.0f); }
            else if (type == "Iron") { color = new Color(0.55f, 0.25f, 0.2f); }
            else if (type == "Copper") { color = new Color(1.0f, 0.35f, 0.2f); }
            else if (type == "Silver") { color = new Color(0.75f, 0.75f, 0.75f); }
            else if (type == "Ferrous") { color = new Color(0.7f, 0.7f, 0.7f); }
            else { color = new Color(0.5f, 0.5f, 0.5f); } // Shoudn't happen
        }
        else
        {
            item = CreateItem(ingot, transform);
            if (type == "Invar") { color = new Color(0.7f, 0.7f, 0.7f); }
            else if (type == "Steel") { color = new Color(0.5f, 0.5f, 0.5f); }
            else if (type == "Bronze") { color = new Color(0.8f, 0.4f, 0.0f); }
            else if (type == "Electrum") { color = new Color(1.0f, 0.9f, 0.0f); }
            else { color = new Color(0.5f, 0.5f, 0.5f); } // Shoudn't happen
        }

        item.GetComponent<MeshRenderer>().material.color = color;
        item.name = type;

        return item;
    }

    public GameObject CreateItem(GameObject item, Transform transform)
    {
        GameObject spawned;

        spawned = Instantiate(item, transform.position, transform.rotation);

        spawned.layer = LayerMask.NameToLayer("Pickable");
        spawned.name = item.name;

        spawned.AddComponent<SphereCollider>();
        spawned.GetComponent<SphereCollider>().isTrigger = true;

        if (item.transform.localScale.x > 2)
        {
            // Ore
            spawned.GetComponent<SphereCollider>().radius = 0.03f;
            spawned.AddComponent<BoxCollider>().size = new Vector3(0.01f, 0.01f, 0.01f);
        }
        else        
        {
            // Ingot
            spawned.GetComponent<SphereCollider>().radius = 10f;
            spawned.AddComponent<BoxCollider>().size = new Vector3(3.5f, 2f, 8f);
        }

        spawned.AddComponent<Rigidbody>().useGravity = true;

        return spawned;
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
