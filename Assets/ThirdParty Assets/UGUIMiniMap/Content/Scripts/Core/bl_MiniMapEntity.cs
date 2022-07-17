using UnityEngine;
using UnityEngine.UI;
using Lovatto.MiniMap;

/// <summary>
/// Default minimap entity handler
/// This script is attached to the game objects that we wanna show in the minimap
/// If you want to use your custom script simply inherit your script from bl_MiniMapEntityBase and use this as reference.
/// </summary>
public class bl_MiniMapEntity : bl_MiniMapEntityBase {

    #region Public members
    [Header("TARGET")]
    [Tooltip("Transform to UI Icon will be follow")]
    public Transform Target = null;
    [Tooltip("Custom Position from target position")]
    public Vector3 OffSet = Vector3.zero;

    [Header("ICON")]
    public Sprite Icon = null;
    public Sprite DeathIcon = null;
    public Color IconColor = new Color(1, 1, 1, 0.9f);
    [Range(1, 100)] public float Size = 20;

    [Header("CIRCLE AREA")]
    public bool ShowCircleArea = false;
    [Range(1, 100)] public float CircleAreaRadius = 10;
    public Color CircleAreaColor = new Color(1, 1, 1, 0.9f);

    [Header("ICON BUTTON")]
    [Tooltip("UI can interact when press it?")]
    public bool isInteractable = true;
    public InteracableAction interacableAction = InteracableAction.OnHover;
    [TextArea(2, 2)] public string InfoItem = "Info Icon here";
    public bool keepTextAlwaysFacingUp = false;

    [Header("SETTINGS")]
    [Tooltip("Can Icon show when is off screen?")]
    public bool OffScreen = true;
    public bool DestroyWithObject = true;
    [Range(0, 5)] public float BorderOffScreen = 0.01f;
    [Range(1, 50)] public float OffScreenSize = 10;
    public bool isHoofdPunt = false;
    [Tooltip("Time before render/show item in minimap after instance")]
    [Range(0, 3)] public float RenderDelay = 0.3f;
    public ItemEffect m_Effect = ItemEffect.None;
    public bool useCustomIconPrefab = false;
    public GameObject CustomIconPrefab;
    #endregion

    #region Public properties
    public bl_MiniMapIconBase IconInstance { get; private set; }

    public override bool IsInteractable
    {
        get => isInteractable;
        set => isInteractable = value;
    }

    public override InteracableAction InteractAction
    {
        get => interacableAction;
        set => interacableAction = value;
    }

    public override Transform GetTarget
    {
        get => Target;
    } 
    #endregion

    #region Private members
    private Image Graphic = null;
    private RectTransform GraphicRect;
    private RectTransform RectRoot;
    private GameObject cacheItem = null;
    private RectTransform CircleAreaRect = null;
    private Vector2 position;
    private bool clampedY, clampedX = false;
    private Vector2 viewPortSize, viewPortFullSize;
    private Vector2 borderMarging = Vector2.zero;
    private Vector2 targetSize = Vector2.one;
    #endregion

    /// <summary>
    /// Get all required component in start
    /// </summary>
    void Start()
    {
        if (MiniMapOwner != null)
        {
            CreateIcon();
            MiniMapOwner.RegisterItem(this);
        }
        else { Debug.Log("You need a MiniMap in scene for use MiniMap Items."); }
    }

    /// <summary>
    /// 
    /// </summary>
    void CreateIcon()
    {
        if (MiniMapOwner.hasError || !this.enabled) return;
        //Instantiate UI in canvas
        GameObject g = bl_MiniMapData.Instance.IconPrefab;
        if(useCustomIconPrefab && CustomIconPrefab != null) { g = CustomIconPrefab; }
        cacheItem = Instantiate(g) as GameObject;
        RectRoot = OffScreen ? MiniMapOwner.MiniMapUI.root : MiniMapOwner.MiniMapUI.iconsPanel;
        //SetUp Icon UI
        IconInstance = cacheItem.GetComponent<bl_MiniMapIconBase>();
        IconInstance.SetUp(this);
        Graphic = IconInstance.GetImage;
        GraphicRect = Graphic.GetComponent<RectTransform>();
        if (Icon != null) { Graphic.sprite = Icon; Graphic.color = IconColor; }
        cacheItem.transform.SetParent(RectRoot.transform, false);
        GraphicRect.anchoredPosition = Vector2.zero;
        if (Target == null) { Target = transform; }
        StartEffect();
        IconInstance.SpawnedDelayed(RenderDelay);
        IconInstance.SetText(InfoItem);
        viewPortFullSize = RectRoot.rect.size;
        viewPortSize = viewPortFullSize * 0.5f;
        borderMarging = Vector2.one * BorderOffScreen;
        if (ShowCircleArea)
        {
           CircleAreaRect = IconInstance.SetCircleArea(CircleAreaRadius, CircleAreaColor);
        }
    }

    /// <summary>
    /// Transfer this icon to a new minimap rig
    /// </summary>
    /// <param name="newOwner"></param>
    public override void ChangeMiniMapOwner(bl_MiniMap newOwner)
    {
        _minimap = newOwner;
        RectRoot = OffScreen ? MiniMapOwner.MiniMapUI.root : MiniMapOwner.MiniMapUI.iconsPanel;
        cacheItem.transform.SetParent(RectRoot.transform, false);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnUpdateItem()
    {    
        //If a component missing, return for avoid bugs.
        if (Target == null)
            return;
        if (Graphic == null)
            return;

        if (Time.frameCount % 30 == 0) OnFrameUpdate();
        IconControl();
    }

    /// <summary>
    /// 
    /// </summary>
    void IconControl()
    {
        if (isHoofdPunt)
        {
            if (MiniMapOwner.Target != null)
            {
                transform.position = MiniMapOwner.Target.TransformPoint((MiniMapOwner.Target.forward) * 100);
            }
        }

        //Setting the modify position
        Vector3 CorrectPosition = TargetPosition + OffSet;
        //Convert the position of target in ViewPortPoint
        Vector2 wvp = MiniMapOwner.miniMapCamera.WorldToViewportPoint(CorrectPosition);
        //Calculate the position of target and convert into position of screen
        position.Set((wvp.x * viewPortFullSize.x) - viewPortSize.x, (wvp.y * viewPortFullSize.y) - viewPortSize.y);
        Vector2 UnClampPosition = position;

        //calculate the position of UI again, determine if off screen
        //if off screen reduce the size
        float Iconsize = Size;
        if (OffScreen)
        {
            if (MiniMapOwner.mapShape == bl_MiniMap.MapShape.Circle)
            {
                // Clamp the icon position inside the circle area
                if (position.sqrMagnitude > (MiniMapOwner.CompassSize * MiniMapOwner.CompassSize))
                {
                    position = position.normalized * MiniMapOwner.CompassSize;
                    Iconsize = OffScreenSize;
                }
                else
                {
                    Iconsize = Size;
                }
            }
            else
            {
                Vector2 border = viewPortSize + borderMarging;
                position.x = bl_MiniMapUtils.ClampBorders(position.x, -border.x, border.x, out clampedX);
                position.y = bl_MiniMapUtils.ClampBorders(position.y, -border.y, border.y, out clampedY);
                //check if the icon is out of the minimap bounds
                if (clampedX || clampedY)
                {
                    Iconsize = OffScreenSize;
                }
                else
                {
                    Iconsize = Size;
                }
            }
        }

        //Apply position to the UI (for follow)
        GraphicRect.anchoredPosition = position;
        if (CircleAreaRect != null) { CircleAreaRect.anchoredPosition = UnClampPosition; }
        //Change size with smooth transition
        targetSize = Vector2.one * (Iconsize * MiniMapOwner.IconMultiplier);
        GraphicRect.sizeDelta = Vector2.Lerp(GraphicRect.sizeDelta, targetSize, Time.deltaTime * 8);

        if (MiniMapOwner.iconsAlwaysFacingUp)
        {
            //with this the icon rotation always will facing up
            if (MiniMapOwner.canvasRenderMode == bl_MiniMap.RenderMode.Mode2D) { GraphicRect.up = Vector3.up; }
            else
            {
                Quaternion r = Quaternion.identity;
                r.x = Target.rotation.x;
                GraphicRect.localRotation = r;
            }
        }
        else
        {
            //with this the rotation icon will depend of target
            Vector3 vre = MiniMapOwner.transform.eulerAngles;
            Vector3 re = Vector3.zero;
            //Fix player rotation for apply to el icon.
            re.z = ((-Target.rotation.eulerAngles.y) + vre.y);
            Quaternion q = Quaternion.Euler(re);
            GraphicRect.rotation = q;

            if(keepTextAlwaysFacingUp) IconInstance.ForceFaceUp();
        }
    }

    /// <summary>
    /// Called each target frame rate
    /// </summary>
    void OnFrameUpdate()
    {
        viewPortFullSize = RectRoot.rect.size;
        viewPortSize = viewPortFullSize * 0.5f;
    }

    /// <summary>
    /// 
    /// </summary>
    void StartEffect()
    {
        Animator a = Graphic.GetComponent<Animator>();
        if (m_Effect == ItemEffect.Pulsing)
        {
            a.SetInteger("Type", 2);
        }
        else if (m_Effect == ItemEffect.Fade)
        {
            a.SetInteger("Type", 1);
        }
    }

    /// <summary>
    /// When player or the target die, disable,remove,etc..
    /// call this for remove the item UI from Map
    /// for change to other icon and disable in certain time
    /// or destroy immediate
    /// </summary>
    /// <param name="inmediate"></param>
    public override void DestroyIcon(bool inmediate)
    {
        if (IconInstance == null) return;

        if (DeathIcon == null || inmediate)
        {
            IconInstance.DestroyIcon(inmediate);
        }
        else
        {
            IconInstance.DestroyIcon(inmediate, DeathIcon);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newTarget"></param>
    public override void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ico"></param>
    public override void SetIcon(Sprite newIcon)
    {
        if (IconInstance == null) return;

        IconInstance.SetIcon(newIcon);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newIconColor"></param>
    public override void SetIconColor(Color newIconColor)
    {
        IconColor = newIconColor;

        if (IconInstance == null) return;

        IconInstance.SetColor(newIconColor);
    }

    /// <summary>
    /// Show a visible circle area in the minimap with this
    /// item as center
    /// </summary>
    public override void SetCircleArea(float radius, Color AreaColor)
    {
        if (IconInstance == null) return;

        CircleAreaRect = IconInstance.SetCircleArea(radius,AreaColor);
    }

    /// <summary>
    /// 
    /// </summary>
    public void HideCircleArea()
    {
        if (IconInstance == null) return;

        IconInstance.SetActiveCircleArea(false);
        CircleAreaRect = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public override void SetActiveIcon(bool active)
    {
        if (active) ShowItem();
        else HideItem();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public override void SetActiveCircleArea(bool active)
    {
        if (IconInstance == null) return;

        IconInstance.SetActiveCircleArea(active);
    }

    /// <summary>
    /// Call this for hide item in miniMap
    /// For show again just call "ShowItem()"
    /// NOTE: For destroy item call "DestroyItem(bool immediate)" instant this.
    /// </summary>
    public void HideItem()
    {
        if (cacheItem != null)
        {
            cacheItem.SetActive(false);
        }
        else
        {
            Debug.Log("There is no item to disable.");
        }
    }

    /// <summary>
    /// Call this for show again the item in miniMap when is hide
    /// </summary>
    public void ShowItem()
    {
        if (IconInstance != null)
        {
            cacheItem.SetActive(true);
            IconInstance.SetOpacity(1);
        }
        else
        {
            Debug.Log("There is no item to active.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void SetIconText(string text)
    {
        if (IconInstance == null) return;
        IconInstance.SetText(text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iconSettings"></param>
    public override void SetIconSettings(MiniMapIconSettings iconSettings)
    {
        Icon = iconSettings.Icon;
        IconColor = iconSettings.Color;
        Target = iconSettings.Target;
        Size = iconSettings.Size;
        m_Effect = iconSettings.ItemEffect;
        isInteractable = iconSettings.Interactable;
    }

    /// <summary>
    /// If you need destroy icon when this gameObject is destroy.
    /// </summary>
    void OnDestroy()
    {
       if(MiniMapOwner != null) MiniMapOwner.RemoveItem(this);
        if (DestroyWithObject)
        {
            DestroyIcon(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 TargetPosition
    {
        get
        {
            if (Target == null)
            {
                return Vector3.zero;
            }

            return new Vector3(Target.position.x, 0, Target.position.z);
        }
    }

    private bl_MiniMap _minimap = null;
    private bl_MiniMap MiniMapOwner
    {
        get
        {
            if (_minimap == null)
            {
                _minimap = bl_MiniMapUtils.GetMiniMap();
            }
            return _minimap;
        }
    }
}