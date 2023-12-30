using UnityEngine;

public class SubmitController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string name = other.gameObject.name;

        if (name == "Steel")
        {
            AddScore(100);
        }

        else if (name == "Bronze")
        {
            AddScore(500);
        }

        else if (name == "Invar")
        {
            AddScore(300);
        }

        else if (name == "Electrum")
        {
            AddScore(200);
        }

        else
        {
            AddScore(10);
        }

        Destroy(other.gameObject);        
    }

    public void AddScore(int amount)
    {
        FindObjectOfType<GameManager>().AddScore(amount);
    }
}
