using UnityEngine;
using UnityEngine.UI;

public class SmelterController : MonoBehaviour
{
    public string type1;
    public int count1;
    public string type2;
    public int count2;

    public string outputName;
    public int outputCount;

    public Transform output;

    public Image smeltingUI;
    public Image infoUI;


    public float smeltingTime = 5f;

    private float counter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        type1 = type2 = "";
        count1 = count2 = 0;

        infoUI.enabled = false;
        smeltingUI.enabled = false;

        // Convert world position to screen space
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.y += (int)(Screen.height * 0.0833);

        // Set the position of the pickaxe image
        infoUI.rectTransform.position = screenPos;
        smeltingUI.rectTransform.position = screenPos;
    }

    public void AddItem(string name)
    {
        if (type1 == "" || type1 == name)
        {
            type1 = name;
            count1 += 1;
        }

        else if (type2 == "" || type2 == name)
        {
            type2 = name;
            count2 += 1;
        }

        if (type1.CompareTo(type2) < 0)
        {
            string temp = type1;
            type1 = type2;
            type2 = temp;

            int cnt = count1;
            count1 = count2;
            count2 = cnt;
        }

        CheckRecipes();
    }

    private void Update()
    {
        if (outputName != "")
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0f;
        }
        
        smeltingUI.fillAmount = counter / smeltingTime;

        if (smeltingUI.fillAmount == 1)
        {
            infoUI.enabled = false;
            smeltingUI.enabled = false;
            counter = 0f;
        }
    }

    public void CheckRecipes()
    {
        // 2x Iron   + 1x Coal     -> 2x Steel
        // 1x Tin    + 3x Copper   -> 4x Bronze
        // 2x Iron   + 1x Ferrous  -> 3x Invar
        // 1x Gold   + 1x Silver   -> 2x Electrum

        // Steel
        if (type1 == "Iron" && count1 >= 2 && type2 == "Coal" && count2 >= 1)
        {
            count1 -= 2;
            count2 -= 1;

            outputName = "Steel";
            outputCount = 2;
        }
        // Bronze
        else if (type1 == "Tin" && count1 >= 1 && type2 == "Copper" && count2 >= 3)
        {
            count1 -= 1;
            count2 -= 3;

            outputName = "Bronze";
            outputCount = 4;

        }
        // Invar
        else if (type1 == "Iron" && count1 >= 2 && type2 == "Ferrous" && count2 >= 1)
        {
            count1 -= 2;
            count2 -= 1;

            outputName = "Invar";
            outputCount = 3;
        }
        // Electrum
        else if (type1 == "Silver" && count1 >= 1 && type2 == "Gold" && count2 >= 1)
        {
            count1 -= 1;
            count2 -= 1;

            outputName = "Electrum";
            outputCount = 2;
        }

        if (count1 == 0) 
        {
            type1 = "";
        }

        if (count2 == 0)
        {
            type2 = "";
        }

        if (outputName != "")
        {
            Invoke("SpawnIngot", smeltingTime);
            counter = 0f;
            infoUI.enabled = true;
            smeltingUI.enabled = true;
        }
    }

    public void SpawnIngot()
    {
        for (int i = 0; i < outputCount; i++)
        {
            FindAnyObjectByType<ItemSpawner>().CreateItem(outputName, false, output);
        }

        if (counter >= smeltingTime)
        {
            infoUI.enabled = false;
            smeltingUI.enabled = false;
            counter = 0f;
        }

        outputCount = -1;
    }


}
