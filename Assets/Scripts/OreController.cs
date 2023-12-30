using UnityEngine;
using UnityEngine.UI;

public class OreType
{
    public static string Tin = "Tin";
    public static string Coal = "Coal";
    public static string Gold = "Gold";
    public static string Iron = "Iron";
    public static string Copper = "Copper";
    public static string Silver = "Silver";
}


public class OreController : MonoBehaviour
{
    public Image infoUI;
    public Image miningUI;
    public Image cooldownUI;

    public float cooldownTime = 5f;

    public float miningTime = 3f;
    private float counter = 0f;

    public float cooldown = 0f;
    public bool mined = false;

    public string type;

    public GameObject fractured;

    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        // Convert world position to screen space
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.y += (int) (Screen.height * 0.0833);

        // Set the position of the pickaxe image
        infoUI.rectTransform.position = screenPos;
        miningUI.rectTransform.position = screenPos;
        cooldownUI.rectTransform.position = screenPos;

        if (type == "Tin") { color = new Color(0.6f, 0.6f, 0.7f); }
        else if (type == "Coal") { color = new Color(0.1f, 0.1f, 0.1f); }
        else if (type == "Gold") { color = new Color(1.0f, 0.85f, 0.0f); }
        else if (type == "Iron") { color = new Color(0.55f, 0.25f, 0.2f); }
        else if (type == "Copper") { color = new Color(1.0f, 0.35f, 0.2f); }
        else if (type == "Silver") { color = new Color(0.75f, 0.75f, 0.75f); }
        else if (type == "Ferrous") { color = new Color(0.7f, 0.7f, 0.7f); }
        else { color = new Color(0.5f, 0.5f, 0.5f); } // Shoudn't happen

        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            if (t.gameObject.name.Contains("Ore"))
            {
                t.gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }

    public void ShowUI()
    {
        Debug.Log("Showing UI");
        infoUI.enabled = true;
    }

    public void HideUI()
    {
        counter = 0f;
        infoUI.enabled = false;
        miningUI.enabled = false;
    }

    public void MineUI()
    {
        UpdateMineUI();
        miningUI.enabled = true;

    }

    public void ResetUI()
    {
        HideUI();
        miningUI.enabled = false;
    }

    void Update()
    {
        if (mined)
        {
            cooldown += Time.deltaTime;
            cooldownUI.fillAmount = cooldown / cooldownTime;
        }
    }

    public void UpdateMineUI()
    {
        if (!mined)
        {
            counter += Time.deltaTime;
        }

        if (counter > miningTime) 
        {
            ResetUI();
            Destruction();
        }
        miningUI.fillAmount = counter / miningTime;
    }

    public void Destruction()
    {
        Debug.Log("Rock succesfully mined, starting destruction");
        
        ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
        itemSpawner.CreateItem(type.ToString(), true, this.transform);

        // Switch objects to create destruction effect
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach(PlayerMovement player in players) 
        { 
            if ((player.gameObject.transform.position - transform.position).magnitude < 5)
            {
                player.OnTriggerExit(gameObject.GetComponent<Collider>());
            }
        }
        
        foreach(Transform t in GetComponentInChildren<Transform>())
        {
            t.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        GameObject frac = Instantiate(fractured, transform.position, transform.rotation);

        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
        {
            Destroy(rb.gameObject, Random.Range(0.0f, 0.5f));

            if (rb.gameObject.name.Contains("Ore"))
            {
                rb.gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        mined = true;
        GetComponent<SphereCollider>().enabled = false;
        miningUI.enabled = false;
        infoUI.enabled = true;
        cooldownUI.enabled = true;

        Invoke("SetTrue", cooldownTime);
    }

    void SetTrue()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            t.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }


        GetComponent<SphereCollider>().enabled = true;
        mined = false;
        cooldownUI.enabled = false;
        infoUI.enabled = false;
        cooldown = 0f;
    }
}
