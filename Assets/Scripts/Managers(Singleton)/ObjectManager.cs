using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util; // MyLayers, Controller

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager instance;
    private static GameObject target;
    private static List<GameObject> inRangeKnockables = new List<GameObject>();

    private bool stairTipOpen;
    private bool shouldStairMenuBeOpen;
    public static Tool curTool;

    [Header("Detection (Outline, for all items)")]
    [SerializeField] private float detectDist;

    [Header("Raytracing (Breakable)")]
    [SerializeField] private float raycastPadding = 21.2f;

    [Header("Unity")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthBarFill;
    // [SerializeField] private StairMenu stairMenu;
    
    // assignment code is in UIStorage
    public StairTips stairTips;
    public BreakTips breakTip;

    public bool verboseMode = true;

    private RaccoonController raccoon;
    private bool startUp = false;
    

    void Awake()
    {
        if (verboseMode) if (verboseMode) Debug.Log("[ObjectManager] Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (verboseMode) Debug.Log("[ObjectManager] Start");

        raccoon = GameManager.instance.Raccoon;

        startUp = true;
    }

    void Update()
    {
        //if (verboseMode) Debug.Log("[ObjectManager] Update");

        if (!raccoon)
        {
            if (!GameManager.instance.Raccoon)
            {
                if (verboseMode) Debug.Log("[ObjectManager] No Raccon");
                return;

            }
            else
                raccoon = GameManager.instance.Raccoon;
        }

        // reset target and in range
        DisableHighlight(target);
        target = null;
        foreach (var k in inRangeKnockables)
        {
            DisableHighlight(k);
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
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist * 3, MyLayers.knockableMask))
            {
                // if (verboseMode) Debug.Log("[ObjectManager] target is Knockable");
                inRangeKnockables.Add(hit.collider.gameObject);
            }

            // current target selected according to precedence: interactable > tools > breakable
            // ^ not really, the statement is true for each iteration, but not so overall  

            // breakable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, MyLayers.breakableMask))
            {
                if (hit.distance < targetDist && curTool != null && (target == null || target.GetComponent<Breakable>() != null))
                {
                    target = hit.collider.gameObject;
                    //if (verboseMode) Debug.Log("[ObjectManager] target is Breakable");
                    targetDist = hit.distance;
                }
            }

            // tools layer
            // if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, MyLayers.toolsMask))
            // {
            //     if (verboseMode) Debug.Log("[ObjectManager] tools hit something");
            //     if (verboseMode) Debug.Log(hit.collider.gameObject.name);
            //     if (hit.distance < targetDist && hit.collider.gameObject.GetComponent<Tool>() != null)
            //     {
            //         if (verboseMode) Debug.Log("[ObjectManager] target is Tool");
            //         target = hit.collider.gameObject;
            //         targetDist = hit.distance;
            //     }
            //     //Debug.Log("hit.distance < targetDist: " + (hit.distance < targetDist).ToString() + ", hit.collider.gameObject.GetComponent<Tool>() != null: " + (hit.collider.gameObject.GetComponent<Tool>() != null).ToString());
            //     if (hit.distance < targetDist && hit.collider.gameObject.GetComponent<ActiveToolController>() != null)
            //     {
            //         if (verboseMode) Debug.Log("[ObjectManager] target is Active Tool");
            //         target = hit.collider.gameObject;
            //         targetDist = hit.distance;
            //     }
            // }

            // interactable layer
            if (Physics.CapsuleCast(p1, p2, 0, dir, out hit, raycastPaddedDist, MyLayers.interactableMask))
            {
                // currently the only other interactable object is Stair
                if (hit.distance < targetDist)
                {
                    target = hit.collider.gameObject;
                    if (verboseMode) Debug.Log("[ObjectManager] target is Stair");
                    targetDist = hit.distance;
                }

            }
        }

        // interact if interact button is pressed
        if (!raccoon.isStunned && Controller.GetB()) Interact();


        // interact with stairs
        var stairTarget = target?.GetComponent<Stair>();
        if ((stairTarget != null))
        {
            if (verboseMode) Debug.Log("Stair target");

            // Show UI
            if (!stairTipOpen) {
                stairTips.Show(stairTarget.GetFloor());
                stairTipOpen = true;
            }

            var raccoon = GameManager.instance.Raccoon;
            // var stair = target.GetComponent<Stair>();
            if (stairTarget == null) return;
            if (Controller.GetX() && stairTarget.GetFloor() != 5)
            {
                raccoon.UseStairs(true);
                stairTipOpen = false;
                stairTips.Hide();
            }
            else if (Controller.GetY() && stairTarget.GetFloor() != 1)
            {
                raccoon.UseStairs(false);
                stairTipOpen = false;
                stairTips.Hide();
            }
        } else if (stairTipOpen) {
            stairTips.Hide();
            stairTipOpen = false;
        }

        // display target health
        // if (target != null)
        // {
        //     Breakable breakableTarget = target.GetComponent<Breakable>();
        //     if (curTool != null && breakableTarget != null)
        //     {
        //         if (verboseMode) Debug.Log("[ObjectManager] target is breakable");
        //         healthBar.SetActive(true);
        //         healthBarFill.fillAmount = breakableTarget.Health / breakableTarget.totalHealth;
        //         EnableHighlight(target);
        //     }
        //     else if (breakableTarget == null)
        //     {
        //         healthBar.SetActive(false);
        //         EnableHighlight(target);
        //     }
        // }
        // else
        // {
        //     healthBar.SetActive(false);
        // }

        // display break tip
        Breakable breakableTarget = target?.GetComponent<Breakable>();
        if (breakableTarget != null)
        {
            if (curTool != null && breakableTarget != null)
            {
                if (verboseMode) Debug.Log("[ObjectManager] target is breakable");
                breakTip.Show();
                // healthBar.SetActive(true);
                // healthBarFill.fillAmount = breakableTarget.Health / breakableTarget.totalHealth;
                // EnableHighlight(target);
            }
        }
        else
        {
            breakTip.Hide();
        }

        // Outline in range knockables
        foreach (var k in inRangeKnockables)
        {
            EnableHighlight(k.gameObject);
        }
    }

    void Interact()
    {
        if (verboseMode) Debug.Log("[ObjectManager] Interact");
        // triggered when interaction button is pressed
        if (target == null)
        {
            if (verboseMode) Debug.Log("No target");
            return;
        }

        if (verboseMode) Debug.Log("Has target");

        // attack target if target breakable
        var breakableTarget = target.GetComponent<Breakable>();
        if (curTool != null && (breakableTarget != null) && (Time.time > raccoon.nextHit))
        {
            if (verboseMode) Debug.Log("Breakable target");
            raccoon.nextHit = Time.time + raccoon.HitRate;
            breakableTarget.trigger(raccoon.AttackPower);
        }
    }


    void EnableHighlight(GameObject c)
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

    void DisableHighlight(GameObject c)
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

    public void EquipTool(Tool tool)
    {
        curTool = tool;
        
        // enable outline for breakables
        var breakables = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.GetComponent<Breakable>() != null);
        foreach(var breakable in breakables) 
        {
            var outline = breakable.GetComponent<Outline>();
            if (outline != null) outline.enabled = true;
        }
    }

    public void UnequipTool(Tool tool)
    {
        curTool = null;
         // disable outline for breakables
        var breakables = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.GetComponent<Breakable>() != null);
        foreach(var breakable in breakables) 
        {
            var outline = breakable.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;

            var highlight = breakable.GetComponent<Highlight>();
            if (highlight != null) highlight.enabled = false;
        }
    }

    public void RegisterHealthBarObj(GameObject hb)
    {
        healthBar = hb;
    }
}