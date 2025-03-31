# LocalizationPatch

A runtime translation patch for Unity Il2Cpp games using MelonLoader and Harmony.  
Originally built for **Schedule I**, this mod intercepts UI texts (`TextMeshProUGUI` and `UnityEngine.UI.Text`) and replaces them with localized strings from a JSON file.

---

## ✨ Features

- Patches `text` setters at runtime using Harmony
- Supports both `TMP` and `UI.Text` (almost)
- Loads translations from a single `fr.json` file
- Easy to expand and customize for other languages

---

## ❓ Q&A

**Q: Does it work right now?**  
**A:** Not really, I'm still debugging it — this is literally my first mod. Cut me some slack, I'm learning.

**Q: Then how the hell did you even make this?**  
**A:** I reverse engineered similar mods, read a ton of MelonLoader/Harmony source code, and broke everything 949,911,136 times until it finally compiled without catching fire.  
Trial and error, brute force, and way too much caffeine.

---
## 🔧 Setup

1. **Install [MelonLoader](https://melonwiki.xyz/#/?id=installation)** (compatible with your game version).
2. **Build this mod** (or grab the compiled `.dll`) and place it in your `Mods` folder.
3. Create a file named `fr.json` in your `UserData` folder with this structure:
   
```json
{
  "Play": "Jouer",
  "Settings": "Paramètres",
  "Exit": "Quitter"
}
