using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
	
    public static ObjectManager instance;
    private static object target;
    // private static List<Knockable> inRangeKnockables = new List<Knockable>();
    private static Collider[] _inRangeItems = null;

    // Masks
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private string toolsMaskName = "Tools";
    private string interactableMaskName = "Interactable";
    private int breakableMask;
    private int knockableMask;
    private int toolsMask;
    private int interactableMask;

    [Header("Detection (Outline, for all items)")]
    [SerializeField] private float detectDist;

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
        // inRangeKnockables.Clear();
        target = null;
        if (_inRangeItems != null)
        {
            foreach (Collider c in _inRangeItems) 
            {
                DisableOutline(c);
            }
        }

        // Update target & in range, in range at every frame using raycast
        RaycastHit hit;

        // Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms.
        Vector3 p1 = raccoon.transform.position + Vector3.up * 0.01f;
        Vector3 p2 = p1 + Vector3.up * raccoon.Controller.height;
        var raycastPaddedDist = raccoon.Controller.radius + raycastPadding;
        //Debug.Log(raycastPaddedDist);
        
        // Update targets
        for (float i = -3.14f; i < 3.14; i += 0.02f)
        {

            _inRangeItems = Physics.OverlapSphere(p1, detectDist, knockableMask | breakableMask | interactableMask);
            foreach (Collider c in _inRangeItems) 
            {
                EnableOutline(c);
            }

            var dir = raccoon.transform.TransformDirection(Vector3.forward) * 5 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i));
                // Debug.DrawRay(p1, raccoon.transform.TransformDirection(Vector3.forward) * 5 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), Color.yellow);

            // knockable layer
            // if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, knockableMask))
            // {
            //     Knockable knockable = hit.collider.gameObject.GetComponent<Knockable>() as Knockable;
            //     inRangeKnockables.Add(knockable);
            // }

            // current target selected according to precedence: interactable > tools > breakable

            // breakable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, breakableMask))
            {

                target = hit.collider.gameObject.GetComponent<Breakable>() as Breakable;
                //Debug.Log("[ObjectManager] target is Breakable");
            }

            // tools layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, toolsMask))
            {
                target = hit.collider.gameObject.GetComponent<ToolController>() as ToolController;
                //Debug.Log("[ObjectManager] target is Tool");
            }

            // interactable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, interactableMask))
            {
                // currently the only other interactable object is Stair
                target = hit.collider.gameObject.GetComponent<Stair>();
                //Debug.Log("[ObjectManager] target is Stair");
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

    void EnableOutline(Collider c)
    {
        if (c == null) return; 
        Outline ol = c.gameObject.GetComponent<Outline>() as Outline;
        if (ol) {ol.enabled = true;}
    }

    void DisableOutline(Collider c)
    {
        if (c == null) return; 
        Outline ol = c.gameObject.GetComponent<Outline>() as Outline;
        if (ol) {ol.enabled = false;}
    }

}