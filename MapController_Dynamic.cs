using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapController_Dynamic : MonoBehaviour
{
    [Header ("UI References")]
    public RectTransform mapParent;
    public GameObject areaPrefab;
    public RectTransform playerIcon;

    [Header ("Colours")]
    public Color defaultColour = Color.gray; //in area
    public Color currentAreaColor = Color.green; // outside

    [Header ("Settings")]
    public GameObject mapBounds;
    public PolygonCollider2D initialArea;
    public float mapScale = 10f;

    private PolygonCollider2D[] mapAreas;
    private Dictionary<string, RectTransform> uiAreas = new Dictionary<string, RectTransform>();

    public static MapController_Dynamic Instance { get; set;}

    private void Start()
    {
        if (mapAreas != null && mapAreas.Length > 0)
        {
            GenerateMap();
        }
        else
        {
            Debug.LogError("MapAreas не знайдені в mapBounds!");
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mapAreas = mapBounds.GetComponentsInChildren<PolygonCollider2D>();
    }

    public void GenerateMap(PolygonCollider2D newCurrentArea = null)
    {
        PolygonCollider2D currentArea = newCurrentArea != null ? newCurrentArea : initialArea;

        ClearMap();

        foreach (PolygonCollider2D area in mapAreas)
        {
            CreateAreaUI(area, area == currentArea);
        }

        MovePlayerIcon(currentArea.name);
    }

    private void ClearMap()
{
    foreach(Transform child in mapParent)
    {
        if (child == playerIcon) continue; 
        
        Destroy(child.gameObject);
    }
    uiAreas.Clear();
}

    private void CreateAreaUI(PolygonCollider2D area, bool isCurrent)
    {
        GameObject areaImage = Instantiate(areaPrefab, mapParent);
        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();

        Bounds bounds = area.bounds;

        rectTransform.sizeDelta = new Vector2(bounds.size.x * mapScale, bounds.size.y * mapScale);
        rectTransform.anchoredPosition = bounds.center * mapScale;

        areaImage.GetComponent<Image>().color = isCurrent ? currentAreaColor : defaultColour;

        uiAreas[area.name] = rectTransform;
    }

    public void UpdateCurrentArea(string newCurrentArea)
    {
        foreach(KeyValuePair<string, RectTransform> area in uiAreas)
        {
            area.Value.GetComponent<Image>().color = area.Key == newCurrentArea ? currentAreaColor : defaultColour;
        }
        
        MovePlayerIcon(newCurrentArea);
    }

    private void MovePlayerIcon(string newCurrentArea)
{
    if(uiAreas.TryGetValue(newCurrentArea, out RectTransform areaUI))
    {
        playerIcon.anchoredPosition = areaUI.anchoredPosition;
    }
}
}
