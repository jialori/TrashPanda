using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager instance;
    private static GameObject target;
    private static List<GameObject> inRangeKnockables = new List<GameObject>();

    // Masks
    private string breakableMaskName = "Breakable";
    private string knockableMaskName = "Knockable";
    private string toolsMaskName = "Tools";
    private string interactableMaskName = "Interactable";
    private int breakableMask;
    private int knockableMask;
    private int toolsMask;
    private int interactableMask;
    private bool stairMenuOpen;

    [Header("Detection (Outline, for all items)")]
    [SerializeField] private float detectDist;

    [Header("Raytracing (Breakable)")]
    [SerializeField] private float raycastPadding = 21.2f;

    [Header("Unity")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private StairMenu stairMenu;

    private RaccoonController raccoon;
    private bool startUp = false;

    void Awake()
    {
        Debug.Log("[ObjectManager] Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("[ObjectManager] Start");

        raccoon = GameManager.instance.Raccoon;
        breakableMask = 1 << LayerMask.NameToLayer(breakableMaskName);
        knockableMask = 1 << LayerMask.NameToLayer(knockableMaskName);
        toolsMask = 1 << LayerMask.NameToLayer(toolsMaskName);
        interactableMask = 1 << LayerMask.NameToLayer(interactableMaskName);

        startUp = true;
    }

    void Update()
    {
        //Debug.Log("[ObjectManager] Update");

        if (!raccoon)
        {
            if (!GameManager.instance.Raccoon)
            {
                Debug.Log("[ObjectManager] No Raccon");
                return;

            }
            else
                raccoon = GameManager.instance.Raccoon;
        }

        // reset target and in range
        DisableOutline(target);
        target = null;
        foreach (var k in inRangeKnockables)
        {
            DisableOutline(k);
        }
        inRangeKnockables.Clear();

        // Update target & in range, in range at every frame using raycast
        RaycastHit hit;

        // Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms.
        Vector3 p1 = raccoon.transform.position - Vector3.up * 0.20f;
        Vector3 p2 = p1 + Vector3.up * raccoon.charController.height;
        var raycastPaddedDist = raccoon.charController.radius + raycastPadding;

        // Update target
        // try to find the closest target
        var targetDist = raycastPaddedDist + 1;
        for (float i = -3.14f; i < 3.14; i += 0.02f)
        {
            var dir = raccoon.transform.TransformDirection(Vector3.forward) * 5 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i));

            // knockable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist * 3, knockableMask))
            {
                inRangeKnockables.Add(hit.collider.gameObject);
            }

            // current target selected according to precedence: interactable > tools > breakable

            // breakable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, breakableMask))
            {
                if (hit.distance < targetDist && (target == null || target.GetComponent<Breakable>() != null))
                {
                    target = hit.collider.gameObject;
                    //Debug.Log("[ObjectManager] target is Breakable");
                    targetDist = hit.distance;
                }
            }

            // tools layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, toolsMask))
            {
                if (hit.distance < targetDist && target?.GetComponent<ToolController>() == null)
                {
                    target = hit.collider.gameObject;
                    //Debug.Log("[ObjectManager] target is Tool");
                    targetDist = hit.distance;
                }
            }

            // interactable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, interactableMask))
            {
                // currently the only other interactable object is Stair
                if (hit.distance < targetDist)
                {
                    target = hit.collider.gameObject;
                    // Debug.Log("[ObjectManager] target is Stair");
                    targetDist = hit.distance;
                }

            }
        }

        // interact if interact button is pressed
        if (!raccoon.isStunned && GetInteract()) Interact();

        if (target != null && target.GetComponent<Stair>() == null && stairMenuOpen)
        {
            stairMenuOpen = false;
            stairMenu.Hide();
        }

        // Go up or down stairs
        if (target != null && stairMenuOpen)
        {
            var raccoon = GameManager.instance.Raccoon;
            var stair = target.GetComponent<Stair>();
            if (stair == null) return;
            if (GetStairUp() && stair.GetFloor() != 5)
            {
                raccoon.UseStairs(true);
                stairMenuOpen = false;
                stairMenu.Hide();
            }
            else if (GetStairDown() && stair.GetFloor() != 1)
            {
                raccoon.UseStairs(false);
                stairMenuOpen = false;
                stairMenu.Hide();
            }
        }

        // display target health
        if (target != null)
        {
            Breakable breakableTarget = target.GetComponent<Breakable>();
            if (breakableTarget != null)
            {
                //Debug.Log("[ObjectManager] target is breakable");
                healthBar.SetActive(true);
                healthBarFill.fillAmount = breakableTarget.Health / breakableTarget.totalHealth;
            }
            else
            {
                healthBar.SetActive(false);
            }

            // Outline target
            EnableOutline(target);
        }
        else
        {
            healthBar.SetActive(false);
        }

        // Outline in range knockables
        foreach (var k in inRangeKnockables)
        {
            EnableOutline(k.gameObject);
        }
    }

    void Interact()
    {
        //Debug.Log("[ObjectManager] Interact");
        // triggered when interaction button is pressed
        if (target == null)
        {
            Debug.Log("No target");
            return;
        }

        //Debug.Log("Has target");

        // attack target if target breakable
        var breakableTarget = target.GetComponent<Breakable>();
        if ((breakableTarget != null) && (Time.time > raccoon.nextHit))
        {
            //Debug.Log("Breakable target");
            raccoon.nextHit = Time.time + raccoon.HitRate;
            breakableTarget.trigger(raccoon.AttackPower);
        }

        var stairTarget = target.GetComponent<Stair>();
        if ((stairTarget != null))
        {
            //Debug.Log("Stair target");
            if (!stairMenuOpen)
            {
                stairMenuOpen = true;
                stairMenu.Show(stairTarget.GetFloor());
            }
            else
            {
                stairMenuOpen = false;
                stairMenu.Hide();
            }
        }
    }

    private bool GetInteract()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("B");
        }
        else
        {
            return Input.GetKeyDown("e");
        }
    }

    private bool GetStairUp()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("X");
        }
        else
        {
            return Input.GetKeyDown("x");
        }
    }

    private bool GetStairDown()
    {
        if (GameManager.instance.UseController)
        {
            return Input.GetButtonDown("Y");
        }
        else
        {
            return Input.GetKeyDown("y");
        }
    }

    void EnableOutline(GameObject c)
    {
        if (c == null) return;
        Outline ol = c.GetComponent<Outline>() as Outline;
        Highlight hl = c.GetComponent<Highlight>() as Highlight;
        if (ol && hl)
        {
            ol.enabled = false;
            hl.enabled = true;
        }
        else if (ol)
        {
            ol.enabled = true;
        }
    }

    void DisableOutline(GameObject c)
    {
        if (c == null) return;
        Outline ol = c.GetComponent<Outline>() as Outline;
        // if (ol) { ol.enabled = false; }
        Highlight hl = c.GetComponent<Highlight>() as Highlight;
        if (ol && hl)
        {
            hl.enabled = false;
            ol.enabled = true;
        }
        else if (ol)
        {
            ol.enabled = false;
        }
    }

    public void Reset()
    {
        target = null;
        inRangeKnockables.Clear();
    }
}