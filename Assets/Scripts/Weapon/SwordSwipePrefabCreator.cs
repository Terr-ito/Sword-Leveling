using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Helper script to automatically create and configure a sword swipe prefab.
/// Use this from the Unity Editor menu to quickly set up a prefab.
/// </summary>
public class SwordSwipePrefabCreator : MonoBehaviour
{
    [MenuItem("Sword Leveling/Create Sword Swipe Prefab")]
    public static void CreateSwordSwipePrefab()
    {
        // Create prefab folder structure if it doesn't exist
        string prefabPath = "Assets/Prefabs/Weapon";
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            string prefabsPath = "Assets/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabsPath))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }
            AssetDatabase.CreateFolder(prefabsPath, "Weapon");
        }

        // Create the base GameObject
        GameObject swipeGO = new GameObject("SwordSwipePrefab");
        swipeGO.transform.position = Vector3.zero;

        // Add Sprite Renderer
        SpriteRenderer spriteRenderer = swipeGO.AddComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 0.8f, 0.3f, 0.6f);
        spriteRenderer.sortingOrder = 0;

        // Try to assign a circle sprite (built-in)
        Sprite circleSprite = Resources.Load<Sprite>("UI/Skin/UISprite");
        if (circleSprite != null)
        {
            spriteRenderer.sprite = circleSprite;
        }

        // Add Circle Collider 2D
        CircleCollider2D circleCollider = swipeGO.AddComponent<CircleCollider2D>();
        circleCollider.radius = 2.5f;
        circleCollider.isTrigger = true;
        circleCollider.offset = Vector2.zero;

        // Add Trail Renderer
        TrailRenderer trailRenderer = swipeGO.AddComponent<TrailRenderer>();
        trailRenderer.time = 0.15f;
        trailRenderer.startWidth = 0.5f;
        trailRenderer.endWidth = 0.1f;
        trailRenderer.startColor = new Color(1f, 0.8f, 0.3f, 0.8f);
        trailRenderer.endColor = new Color(1f, 0.8f, 0.3f, 0f);
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.minVertexDistance = 0.1f;

        // Add Audio Source (optional)
        AudioSource audioSource = swipeGO.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.8f;

        // Add SwordSwipe script
        swipeGO.AddComponent<SwordSwipe>();

        // Save as prefab
        string prefabFilePath = Path.Combine(prefabPath, "SwordSwipePrefab.prefab");
        PrefabUtility.SaveAsPrefabAsset(swipeGO, prefabFilePath);

        // Clean up the scene GameObject
        DestroyImmediate(swipeGO);

        // Refresh asset database
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Success!",
            $"Sword Swipe prefab created at:\n{prefabFilePath}\n\n" +
            "Next steps:\n" +
            "1. Select your Player GameObject\n" +
            "2. Drag the prefab into the 'Swipe Prefab' field of SwordWeapon script",
            "OK"
        );

        // Ping the prefab in the project
        var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabFilePath);
        EditorGUIUtility.PingObject(prefabAsset);
    }

    [MenuItem("Sword Leveling/Open Swipe Prefab Setup Guide")]
    public static void OpenSetupGuide()
    {
        string guideFile = "Assets/Scripts/Weapon/SWIPE_PREFAB_SETUP.txt";
        var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(guideFile);
        if (textAsset != null)
        {
            AssetDatabase.OpenAsset(textAsset);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", $"Could not find setup guide at {guideFile}", "OK");
        }
    }

    [MenuItem("Sword Leveling/Reset Sword Weapon Settings")]
    public static void ResetSwordWeaponSettings()
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player with PlayerMovement component not found!", "OK");
            return;
        }

        var swordWeapon = player.GetComponent<SwordWeapon>();
        if (swordWeapon == null)
        {
            EditorUtility.DisplayDialog("Error", "Player doesn't have SwordWeapon component!", "OK");
            return;
        }

        // Find and assign the prefab if it exists
        var swipePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Weapon/SwordSwipePrefab.prefab");
        if (swipePrefab != null)
        {
            EditorGUIUtility.PingObject(swipePrefab);
            EditorUtility.DisplayDialog("Found", "Sword Swipe prefab found. Drag it to the SwordWeapon component's 'Swipe Prefab' field.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Not Found", "Sword Swipe prefab not found. Run 'Create Sword Swipe Prefab' first.", "OK");
        }
    }
}
#endif
