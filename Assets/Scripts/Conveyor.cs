using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed, conveyorSpeed;

    public Vector3 direction;

    public List<GameObject> onBelt;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().material.mainTextureOffset += new Vector2(0, 1) * conveyorSpeed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < onBelt.Count; i++)
        {
            if (onBelt[i] != null)
            {
                onBelt[i].GetComponent<Rigidbody>().AddForce(speed * direction * (-1));
            }
            else
            {
                onBelt.RemoveAt(i);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject);
    }
}
