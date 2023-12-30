using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    public float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    private CharacterController controller;

    public Vector2 movementInput = Vector2.zero;

    private bool canMine = false;
    private bool canPick = false;
    private bool holdingItem = false;
    private bool nearFurnace = false;

    private bool canInteract = false;

    private GameObject interacted = null;

    public GameObject objectPoint;

    public float pickCooldown = 0.5f;

    public Animator animator;


    private float counter = 0f;
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        if (FindObjectOfType<GameManager>().gameEnded == true)
        {
            Destroy(gameObject);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        canInteract = context.ReadValue<float>() > 0;
    }

    void Update()
    {
        counter += Time.deltaTime;

        // Movement
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(move * speed * Time.deltaTime);
        }
        animator.SetFloat("Speed", move.magnitude);


        // Interactions

        if (canInteract && holdingItem)
        {
            if (counter > pickCooldown)
            { 
                if (nearFurnace)
                {
                    FindAnyObjectByType<SmelterController>().AddItem(objectPoint.name);
                    holdingItem = false;
                    objectPoint.SetActive(false);
                }
                else
                {
                    DropItem();
                }
                counter = 0f;
            }
        }

        else if (canInteract && canPick && !holdingItem)
        {
            if (counter > pickCooldown)
            {
                PickItem();
                counter = 0f;
            }
        }

        else if (canInteract && canMine)
        {
            Mine();
        }

        else if (!canInteract && canMine && !interacted)
        {
            Debug.Log("You cannot mine now!");
            animator.SetBool("Mining", false);
            canMine = false;
        }

    }

    public void PickItem()
    {
        if (!interacted)
        {
            Debug.Log("Can't interact with null");
            return;
        }

        Debug.Log("Picking item: " + interacted.name);

        // Mesh Renderer
        objectPoint.GetComponent<MeshRenderer>().materials = interacted.GetComponent<MeshRenderer>().materials;

        // Mesh Filter
        objectPoint.GetComponent<MeshFilter>().mesh = interacted.GetComponent<MeshFilter>().mesh;

        // Scale
        objectPoint.transform.localScale = interacted.transform.localScale;

        // Name
        objectPoint.name = interacted.name;


        // Change objects visibility
        holdingItem = true;
        objectPoint.SetActive(true);
        animator.SetBool("HoldingItem", true);

        // Disable collider
        Destroy(interacted);
        interacted = null;
    }

    public void DropItem()
    {
        FindAnyObjectByType<ItemSpawner>().CreateItem(objectPoint, objectPoint.transform);

        // Change objects visibility
        holdingItem = false;
        objectPoint.SetActive(false);
//        animator.Play("Carry-Putdown");
        animator.SetBool("HoldingItem", false);

    }

    public void Mine()
    {
        if (interacted)
        {
            animator.SetBool("Mining", true);
            interacted.GetComponent<OreController>().MineUI();
        }
        else
        {
            canMine = false;
            animator.SetBool("Mining", false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering the: " + other.gameObject.name);
        interacted = other.gameObject;
        int layer = other.gameObject.layer;

        canMine = (layer == LayerMask.NameToLayer("Mineable"));
        canPick = (layer == LayerMask.NameToLayer("Pickable"));
        nearFurnace = (layer == LayerMask.NameToLayer("Smelter"));

        if (canMine) 
        {
            interacted.GetComponent<OreController>().ShowUI();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving the: " + other.gameObject.name);
        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Mineable"))
        {
            if (interacted)
            {
                interacted.GetComponent<OreController>().HideUI();
            }
            canMine = false;
            animator.SetBool("Mining", false);

        }
        else if (layer == LayerMask.NameToLayer("Pickable"))
        {
            canPick = false;
        }
        else if (layer == LayerMask.NameToLayer("Smelter"))
        {
            nearFurnace = false;
        }

        interacted = null;
    }
}
