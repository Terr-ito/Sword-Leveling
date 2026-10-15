# Sword Swipe Prefab Setup - Quick Visual Guide

## 📋 Overview
A prefab is a reusable template. You create it once, then spawn unlimited copies. The SwordWeapon system uses this to spawn visual sword swipes.

---

## ⚡ QUICK START (Automated)

### Option A: Use the Menu Helper (Recommended for beginners)

1. In Unity Editor, go to top menu: **Sword Leveling → Create Sword Swipe Prefab**
   - This automatically creates and configures everything!
   - The prefab will be saved to `Assets/Prefabs/Weapon/`

2. Next, select your **Player GameObject** in the Hierarchy
3. In the Inspector, find **SwordWeapon** component
4. Drag the newly created prefab into the **Swipe Prefab** field

**Done!** Your prefab is now set up and ready to use.

---

## 🛠️ Manual Setup (For More Control)

If you prefer to create it manually, follow these steps:

### Step 1: Create Base GameObject
```
Right-click in Hierarchy
→ 2D Object
→ Sprites
→ Circle  (or Square if Circle unavailable)
```
Name it: `SwordSwipePrefab`

### Step 2: Reset Transform
```
Inspector → Transform component → ⋮ (three dots)
→ Reset
```
This ensures Position = (0,0,0) and Scale = (1,1,1)

### Step 3: Configure Sprite Renderer
| Setting | Value |
|---------|-------|
| **Sprite** | Circle (built-in) |
| **Color** | R: 1, G: 0.8, B: 0.3, A: 0.6 (Golden Yellow) |
| **Sorting Order** | 0 |
| **Material** | Default (Sprites/Default) |

**Color Tips:**
- Transparent yellow: (1, 0.8, 0.3, 0.6)
- Bright yellow: (1, 1, 0, 0.5)
- Orange-gold: (0.9, 0.7, 0.2, 0.7)

### Step 4: Add Circle Collider 2D
```
Inspector → Add Component
→ Search "Circle Collider 2D"
```

| Setting | Value |
|---------|-------|
| **Offset X** | 0 |
| **Offset Y** | 0 |
| **Radius** | 2.5 |
| **Is Trigger** | ✓ TRUE (IMPORTANT!) |

⚠️ **CRITICAL:** "Is Trigger" MUST be checked!
- Trigger colliders detect collisions without physical interaction
- This prevents pushing enemies around

### Step 5: Add Trail Renderer (Motion Effect)
```
Inspector → Add Component
→ Search "Trail Renderer"
```

| Setting | Value |
|---------|-------|
| **Time** | 0.15 |
| **Start Width** | 0.5 |
| **End Width** | 0.1 |
| **Min Vertex Distance** | 0.1 |
| **Alignment** | View |

**Colors:**
| Setting | Value |
|---------|-------|
| **Start Color** | (1, 0.8, 0.3, 0.8) *Golden, solid* |
| **End Color** | (1, 0.8, 0.3, 0) *Golden, fade* |

**Material:**
- Material: Create a new material or use "Sprites/Default"

### Step 6: Add SwordSwipe Script
```
Inspector → Add Component
→ Search "Sword Swipe"
→ Select SwordSwipe.cs
```

No configuration needed - it initializes automatically!

### Step 7: Add Audio Source (Optional)
```
Inspector → Add Component
→ Search "Audio Source"
```

| Setting | Value |
|---------|-------|
| **Audio Clip** | (Optional - add later) |
| **Spatial Blend** | 1 (3D Audio) |
| **Volume** | 0.8 |
| **Play On Awake** | False |

### Step 8: Save as Prefab
```
In Project window:
1. Right-click → Create Folder → name it "Prefabs"
2. Inside Prefabs, create "Weapon" folder
3. Drag "SwordSwipePrefab" from Hierarchy to:
   Assets/Prefabs/Weapon/
```

The GameObject will turn **blue** in the Hierarchy - you've created a prefab!

### Step 9: Delete Scene Copy (Optional)
You can now delete the GameObject from your scene if you want:
- Right-click on it in Hierarchy
- Select "Delete"

The prefab is safely saved in your project!

---

## 🎯 Assign to SwordWeapon

1. **Select your Player GameObject** in the scene
2. In **Inspector**, find the **SwordWeapon** component
3. Drag the prefab from `Assets/Prefabs/Weapon/SwordSwipePrefab` 
   into the **Swipe Prefab** field

Optional: Set "Swipe Spawn Point" to your player's hand/sword position

---

## 📊 Component Breakdown

```
SwordSwipePrefab (GameObject)
├── Transform (Position, Rotation, Scale)
├── Sprite Renderer (Visual appearance)
├── Circle Collider 2D (Hitbox - Is Trigger!)
├── Trail Renderer (Motion effect)
├── Audio Source (Sound - optional)
└── Sword Swipe (Script - damage logic)
```

Each component has a specific job:
- **Sprite Renderer** = What you see
- **Circle Collider 2D** = What detects hits
- **Trail Renderer** = Motion effect
- **Audio Source** = Sound effects
- **SwordSwipe** = Game logic (damage, lifetime)

---

## ✅ Verification Checklist

- [ ] Sprite Renderer shows golden/yellow color
- [ ] Circle Collider 2D has "Is Trigger" checked
- [ ] Trail Renderer has fade colors configured
- [ ] SwordSwipe script added
- [ ] Saved in Assets/Prefabs/Weapon/ folder
- [ ] Assigned to SwordWeapon's Swipe Prefab field
- [ ] Player has PlayerMovement component
- [ ] Enemies implement IDamageable interface

---

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Swipe doesn't appear | Check Sprite Renderer alpha > 0, check sorting order |
| Swipe doesn't hit enemies | Check "Is Trigger" is TRUE, enemy has Rigidbody2D |
| Swipe lasts too long | Edit SwordSwipe.cs: change `lifetime = 0.3f` to smaller value |
| Can't find prefab | Manually create it or use menu helper |
| Trail effect missing | Add Trail Renderer component if removed |

---

## 🚀 No Prefab? No Problem!

If you skip prefab creation:
- Leave **Swipe Prefab** field empty in SwordWeapon
- The system creates swipes dynamically (slightly less optimized)
- Everything still works fine!

---

Made with ❤️ for the Sword Leveling game!
