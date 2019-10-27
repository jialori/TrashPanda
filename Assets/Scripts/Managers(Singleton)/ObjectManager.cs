using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
	
    public static ObjectManager instance;
    private static object target;
    private static List<Knockable> inRangeKnockables = new List<Knockable>();

    // Masks
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private string toolsMaskName = "Tools";
    private string interactableMaskName = "Interactable";
    private int breakableMask;
    private int knockableMask;
    private int toolsMask;
    private int interactableMask;

    [Header("Raytracing (Breakable)")]
    [SerializeField] private float raycastPadding = 21.2f;

    [Header("Unity")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthBarFill;

    private RaccoonController raccoon;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        raccoon = GameManager.instance.Raccoon;
        breakableMask = 1 << LayerMask.NameToLayer(breakableMaskName);
        knockableMask = 1 << LayerMask.NameToLayer(knockableMaskName);
        toolsMask = 1 << LayerMask.NameToLayer(toolsMaskName);
        interactableMask = 1 << LayerMask.NameToLayer(interactableMaskName);
    }

    void Update()
    {
        if (!raccoon) return;
        inRangeKnockables.Clear();
        target = null;

        // Update target & in range, in range at every frame using raycast
        RaycastHit hit;

        // Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms.
        Vector3 p1 = raccoon.transform.position + Vector3.up * 0.01f;
        Vector3 p2 = p1 + Vector3.up * raccoon.Controller.height;
        var raycastPaddedDist = raccoon.Controller.radius + raycastPadding;
        
        // Update targets
        for (int i = 180; i < 360; i += 1)
        {
            // knockable layer
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist, knockableMask))
            {
                Knockable knockable = hit.collider.gameObject.GetComponent<Knockable>() as Knockable;
                inRangeKnockables.Add(knockable);
            }

            // current target selected according to precedence: interactable > tools > breakable

            // breakable layer
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist, breakableMask))
            {
                target = hit.collider.gameObject.GetComponent<Breakable>() as Breakable;
            }

            // tools layer
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist, toolsMask))
            {
                target = hit.collider.gameObject.GetComponent<ToolController>() as ToolController;
            }

            // interactable layer
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, raycastPaddedDist, interactableMask))
            {
                // currently the only other interactable object is Stair
                target = hit.collider.gameObject.GetComponent<Stair>();
            }

            // display target health
            Breakable breakableTarget = target as Breakable;
            if (breakableTarget != null) {
                Debug.Log("[ObjectManager] target is breakable");
                healthBar.SetActive(true);
                healthBarFill.fillAmount = breakableTarget.Health / breakableTarget.totalHealth;
            } else {
                healthBar.SetActive(false);
            }
        }

        // interact if interact button is pressed
        if (GetInteract()) Interact();
    }

    void Interact() 
    {
        // triggered when interaction button is pressed
        if (target == null) return;
        
        // attack target if target breakable
        var breakableTarget = target as Breakable;
        if ((breakableTarget != null) && (Time.time > raccoon.nextHit))
        {
            raccoon.nextHit = Time.time + raccoon.HitRate;
            breakableTarget.trigger(raccoon.AttackPower);
        }
    }

    private bool GetInteract()
    {
        if (GameManager.instance.UseController) 
        {
            return Input.GetButtonDown("B");
        } else {
            return Input.GetKeyDown("e");
        }
    }
}