using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Reflection;
public class Hacks : MonoBehaviour
{
    
    public static Rect rc = new Rect(Screen.width/2, 100, 350, 220);
    public bool menu = false;
    public int one;
    public int two = 50;
    public static bool nograss = false;
    public static bool amlight = false;
    public static Texture2D texture;
    public static Texture2D inactivetexture;
    public static Texture2D texture2;
    public static Texture2D Activetexture;
    public static Texture2D backgroundtexture;
    public static Texture2D background;
    public static Texture2D hoverbackground;
    public static Color HoverColor = new Color32(149, 0, 1, 255);
    public static Color BackGround = new Color32(64, 64, 64, 220);
    public static Color BackGroundColor = new Color32(13, 13, 13, 255);
    public static Color hoverColor = new Color32(26, 26, 26, 255);
    public static Color Color3 = new Color32(0, 149, 1, 255);
    public static GUIStyle MenuItemStyle;
    public static GUIStyle ActiveMenuItemStyle;

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


    public static string nameToAdd = "client_bear";
    public static string objectlist;
    public static string NoGrassButton = "NoGrass: OFF";
    public static string NoTreeButton = "NoTree: OFF";
    public static string LightButton = "Brightness: OFF";
    public static string ChamsButton = "WALLHACK: OFF";
    public static string BoxButton = "BOX ESP: ON";
    public static string LineButton = "LINE ESP: ON";
    public static string DistButton = "DIST ESP: ON";
    public static string AimButton = "BONE: HEAD";
    public static string FovButton = "FOV: ";
    public static string SmoothButton = "SMOOTH: ";
    public static string KeyButton = "KEY: ";
    public static KeyCode aimkey = KeyCode.V;
    public static List<GameObject> badguys = new List<GameObject>();
    public static List<Vector3> targets = new List<Vector3>();
    public static List<GameObject> objects = new List<GameObject>();
    public static List<GameObject> grass = new List<GameObject>();
    public static List<GameObject> trees = new List<GameObject>();
    public static List<GameObject> chams = new List<GameObject>();
    public static Color AmbientColor = Color.white;
    public static Shader DefaultShader;
    public string Shit1;
    
    public static int smooth = 2;
    public static int fov = 3;
    public static float think = 2f;
    public static bool wallhack = false;
    public static bool BoxEsp = true;
    public static bool LineESP = true;
    public static bool DistESP = true;
    public static string bone = "NPC_Head";
    private static Texture2D aaLineTex = null;
    private static Texture2D lineTex = null;
    private static Material blitMaterial = null;
    private static Material blendMaterial = null;
    private static Rect lineRect = new Rect(0f, 0f, 1f, 1f);
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
    {
        float num = pointB.x - pointA.x;
        float num2 = pointB.y - pointA.y;
        float num3 = Mathf.Sqrt(num * num + num2 * num2);
        if (num3 < 0.001f)
        {
            return;
        }
        Texture2D texture2D;
        if (antiAlias)
        {
            width *= 3f;
            texture2D = aaLineTex;
            Material material = blendMaterial;
        }
        else
        {
            texture2D = lineTex;
            Material material2 = blitMaterial;
        }
        float num4 = width * num2 / num3;
        float num5 = width * num / num3;
        Matrix4x4 identity = Matrix4x4.identity;
        identity.m00 = num;
        identity.m01 = -num4;
        identity.m03 = pointA.x + 0.5f * num4;
        identity.m10 = num2;
        identity.m11 = num5;
        identity.m13 = pointA.y - 0.5f * num5;
        GL.PushMatrix();
        GL.MultMatrix(identity);
        GUI.color = color;
        GUI.DrawTexture(lineRect, texture2D);
        GL.PopMatrix();
    }

    public static void RectFilled(float x, float y, float width, float height, Texture2D text)
    {
        GUI.DrawTexture(new Rect(x, y, width, height), text);
    }

    public static void RectOutlined(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
    {
        RectFilled(x, y, thickness, height, text);
        RectFilled(x + width - thickness, y, thickness, height, text);
        RectFilled(x + thickness, y, width - thickness * 2f, thickness, text);
        RectFilled(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
    }

    public static void Box(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
    {
        RectOutlined(x - width / 2f, y - height, width, height, text, thickness);
    }

    private bool IsVisable(GameObject origin, Vector3 toCheck)
    {
        RaycastHit hit;
        if (Physics.Linecast(Camera.main.transform.position, toCheck, out hit))
        {
            if (hit.transform.name.Contains("NPC") || hit.transform.name.Contains("client")
                || hit.transform.name == Camera.main.name || hit.transform.name == Camera.main.gameObject.name 
                || hit.transform.name == Camera.main.transform.name || hit.transform.tag.Contains("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public static void GI(int id)
    {
        MenuItemStyle = new GUIStyle(GUI.skin.button);
        MenuItemStyle.fontStyle = FontStyle.Bold;
        MenuItemStyle.normal.textColor = Color.white;
        MenuItemStyle.normal.background = texture;
        MenuItemStyle.hover.textColor = Color.gray;
        MenuItemStyle.hover.background = inactivetexture;
        MenuItemStyle.active.background = inactivetexture;
       
        ActiveMenuItemStyle = new GUIStyle(GUI.skin.button);
        ActiveMenuItemStyle.fontStyle = FontStyle.Bold;
        ActiveMenuItemStyle.normal.textColor = Color.white;
        ActiveMenuItemStyle.normal.background = texture2;
        ActiveMenuItemStyle.hover.textColor = Color.gray;
        ActiveMenuItemStyle.hover.background = Activetexture;
        ActiveMenuItemStyle.active.background = Activetexture;

        GUIStyle TextStyle = new GUIStyle(GUI.skin.label);
        TextStyle.fontStyle = FontStyle.Bold;
        TextStyle.fontSize = 15;

        GUIStyle Button1;
        if (nameToAdd == "client_u") Button1 = ActiveMenuItemStyle;
        else Button1 = MenuItemStyle;

        GUIStyle Button2;
        if (nameToAdd == "client_bear") Button2 = ActiveMenuItemStyle;
        else Button2 = MenuItemStyle;

        GUIStyle Button3;
        if (nameToAdd == "client_player") Button3 = ActiveMenuItemStyle;
        else Button3 = MenuItemStyle;

        GUIStyle Button4;
        if (nograss) Button4 = ActiveMenuItemStyle;
        else Button4 = MenuItemStyle;

        GUIStyle Button5;
        if (NoTreeButton.Contains("ON")) Button5 = ActiveMenuItemStyle;
        else Button5 = MenuItemStyle;

        GUIStyle Button6;
        if (amlight) Button6 = ActiveMenuItemStyle;
        else Button6 = MenuItemStyle;

        GUIStyle Button7;
        if (ChamsButton.Contains("ON")) Button7 = ActiveMenuItemStyle;
        else Button7 = MenuItemStyle;

        GUIStyle Button8;
        if (BoxButton.Contains("ON")) Button8 = ActiveMenuItemStyle;
        else Button8 = MenuItemStyle;

        GUIStyle Button9;
        if (LineButton.Contains("ON")) Button9 = ActiveMenuItemStyle;
        else Button9 = MenuItemStyle;

        GUIStyle Button10;
        if (DistButton.Contains("ON")) Button10 = ActiveMenuItemStyle;
        else Button10 = MenuItemStyle;

        GUI.DrawTexture(new Rect(0, 0, 350, 20), hoverbackground);
        GUI.Label(new Rect(60, 0, 350, 20), "INTERIUM.OOO © KleskBY 2019", TextStyle);
        GUI.Label(new Rect(39, 20, 140, 20), "TARGET TEAM:", TextStyle);
        if (GUI.Button(new Rect(20, 40, 140, 20), "USEC", Button1))
        {
            badguys.Clear();
            nameToAdd = "client_u";
        }
        if (GUI.Button(new Rect(20, 60, 140, 20), "BEAR", Button2))
        {
            badguys.Clear();
            nameToAdd = "client_bear";
        }
        if (GUI.Button(new Rect(20, 80, 140, 20), "DeathMatch", Button3))
        {
            badguys.Clear();
            nameToAdd = "client_player";
        }
        GUI.Label(new Rect(67, 100, 140, 20), "MISC:", TextStyle);
        if (GUI.Button(new Rect(20, 120, 140, 20), NoGrassButton, Button4))
        {
            if (nograss)
            {
                nograss = false;
                NoGrassButton = "NoGrass: OFF";
                try
                {
                    foreach (GameObject go in grass) go.SetActive(true);
                }
                catch
                {
                    nograss = false;
                    NoGrassButton = "NoGrass: OFF";
                }
            }
            else
            {
                grass.Clear();
                foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.activeInHierarchy)
                    {
                        if (go.name.Contains("grass") || go.name.Contains("brush") || go.name.Contains("paporotnik") || go.name.Contains("bush"))
                        {
                            grass.Add(go);
                            go.SetActive(false);
                        }
                    }
                }
                nograss = true;
                NoGrassButton = "NoGrass: ON";
            }
        }
        if (GUI.Button(new Rect(20, 140, 140, 20), LightButton, Button6))
        {
            if (amlight)
            {
                amlight = false;
                try
                {
                    RenderSettings.fog = true;
                    RenderSettings.ambientLight = AmbientColor;
                    LightButton = "Brightness: OFF";
                }
                catch { amlight = false; }

            }
            else
            {
                AmbientColor = RenderSettings.ambientLight;
                RenderSettings.fog = false;
                RenderSettings.ambientLight = Color.white;
                LightButton = "Brightness: ON";
                amlight = true;
            }
        }
        if (GUI.Button(new Rect(20, 160, 140, 20), ChamsButton, Button7))
        {
            if(wallhack)
            {
                ChamsButton = "WALLHACK: OFF";
                wallhack = false;
            }
            else
            {
                ChamsButton = "WALLHACK: ON";
                wallhack = true;
            }
        }
        if (GUI.Button(new Rect(20, 180, 140, 20), "Patch chams", MenuItemStyle))
        {
            foreach (GameObject gameObj in badguys)
            {
                var distance = Vector3.Distance(Camera.main.transform.position, gameObj.transform.position);
                {
                    if (distance < 10 && distance > 2)
                    {
                        gameObj.SetActive(false);
                    }
                    else
                    {
                        var rend = gameObj.GetComponentsInChildren<Renderer>();
                        foreach (Renderer renderer in rend) renderer.material.mainTexture = texture;
                    }
                }
            }
        }

        GUI.Label(new Rect(215, 20, 140, 20), "VISUALS:", TextStyle);
        if (GUI.Button(new Rect(180, 40, 140, 20), BoxButton, Button8))
        {
            if (BoxEsp)
            {
                BoxEsp = false;
                BoxButton = "BOX ESP: OFF";
            }
            else
            {
                BoxEsp = true;
                BoxButton = "BOX ESP: ON";
            }
        }
        if (GUI.Button(new Rect(180, 60, 140, 20), LineButton, Button9))
        {
            if (LineESP)
            {
                LineESP = false;
                LineButton = "LINE ESP: OFF";
            }
            else
            {
                LineESP = true;
                LineButton = "LINE ESP: ON";
            }
        }
        if (GUI.Button(new Rect(180, 80, 140, 20), DistButton, Button10))
        {
            if (DistESP)
            {
                DistESP = false;
                DistButton = "DIST ESP: OFF";
            }
            else
            {
                DistESP = true;
                DistButton = "DIST ESP: ON";
            }
        }
        GUI.Label(new Rect(220, 100, 140, 20), "AIMBOT:", TextStyle);
        if (GUI.Button(new Rect(180, 120, 140, 20), AimButton, MenuItemStyle))
        {
           if(AimButton.Contains("HEAD"))
            {
                bone = "NPC_Neck";
                AimButton = "BONE: NECK";
            }
            else if (AimButton.Contains("NECK"))
            {
                bone = "NPC_Spine1";
                AimButton = "BONE: BODY";
            }
            else if (AimButton.Contains("BODY"))
            {
                bone = "NPC_Head";
                AimButton = "BONE: HEAD";
            }
        }
        FovButton = "FOV: " + fov.ToString();
        if (GUI.Button(new Rect(180, 140, 140, 20), FovButton, MenuItemStyle))
        {
            if (fov == 1) fov = 2;
            else if (fov == 2) fov = 3;
            else if (fov == 3) fov = 4;
            else if (fov == 4) fov = 5;
            else if (fov == 5) fov = 1;
        }
        SmoothButton = "SMOOTH: " + smooth.ToString();
        if (GUI.Button(new Rect(180, 160, 140, 20), SmoothButton, MenuItemStyle))
        {
            if (smooth == 1) smooth = 2;
            else if(smooth == 2) smooth = 3;
            else if(smooth == 3) smooth = 4;
            else if (smooth == 4) smooth = 5;
            else if (smooth == 5) smooth = 1;
        }
        SmoothButton = "SMOOTH: " + smooth.ToString();
        if (GUI.Button(new Rect(180, 160, 140, 20), SmoothButton, MenuItemStyle))
        {
            if (smooth == 1) smooth = 2;
            else if (smooth == 2) smooth = 3;
            else if (smooth == 3) smooth = 4;
            else if (smooth == 4) smooth = 5;
            else if (smooth == 5) smooth = 1;
        }
        KeyButton = "KEY: " + aimkey.ToString();
        if (GUI.Button(new Rect(180, 180, 140, 20), KeyButton, MenuItemStyle))
        {
            if (aimkey == KeyCode.V) aimkey = KeyCode.Mouse1;
            else if (aimkey == KeyCode.Mouse1) aimkey = KeyCode.Mouse0;
            else if (aimkey == KeyCode.Mouse0) aimkey = KeyCode.LeftShift;
            else if (aimkey == KeyCode.LeftShift) aimkey = KeyCode.LeftAlt;
            else if (aimkey == KeyCode.LeftAlt) aimkey = KeyCode.V;
        }

        GUI.DragWindow();
    }

    void Start()
    {
        texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, Color.red);
        texture.SetPixel(1, 0, Color.red);
        texture.SetPixel(0, 1, Color.red);
        texture.SetPixel(1, 1, Color.red);
        texture.Apply();

        inactivetexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        inactivetexture.SetPixel(0, 0, HoverColor);
        inactivetexture.SetPixel(1, 0, HoverColor);
        inactivetexture.SetPixel(0, 1, HoverColor);
        inactivetexture.SetPixel(1, 1, HoverColor);
        inactivetexture.Apply();

        texture2 = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        texture2.SetPixel(0, 0, Color.green);
        texture2.SetPixel(1, 0, Color.green);
        texture2.SetPixel(0, 1, Color.green);
        texture2.SetPixel(1, 1, Color.green);
        texture2.Apply();

        Activetexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        Activetexture.SetPixel(0, 0, Color3);
        Activetexture.SetPixel(1, 0, Color3);
        Activetexture.SetPixel(0, 1, Color3);
        Activetexture.SetPixel(1, 1, Color3);
        Activetexture.Apply();

        backgroundtexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        backgroundtexture.SetPixel(0, 0, BackGroundColor);
        backgroundtexture.SetPixel(1, 0, BackGroundColor);
        backgroundtexture.SetPixel(0, 1, BackGroundColor);
        backgroundtexture.SetPixel(1, 1, BackGroundColor);
        backgroundtexture.Apply();

        hoverbackground = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        hoverbackground.SetPixel(0, 0, hoverColor);
        hoverbackground.SetPixel(1, 0, hoverColor);
        hoverbackground.SetPixel(0, 1, hoverColor);
        hoverbackground.SetPixel(1, 1, hoverColor);
        hoverbackground.Apply();

        background = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        background.SetPixel(0, 0, BackGround);
        background.SetPixel(1, 0, BackGround);
        background.SetPixel(0, 1, BackGround);
        background.SetPixel(1, 1, BackGround);
        backgroundtexture.Apply();

        if (lineTex == null)
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            lineTex.SetPixel(0, 1, UnityEngine.Color.white);
            lineTex.Apply();
        }
        if (aaLineTex == null)
        {
            aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, false);
            aaLineTex.SetPixel(0, 0, new UnityEngine.Color(1, 1, 1, 0));
            aaLineTex.SetPixel(0, 1, UnityEngine.Color.white);
            aaLineTex.SetPixel(0, 2, new UnityEngine.Color(1, 1, 1, 0));
            aaLineTex.Apply();
        }

        blitMaterial = (Material)typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        blendMaterial = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
    }

    void Update()
    {
        float minDist = 99999;
        Vector2 AimTarget = Vector2.zero;
        try
        {
            foreach (GameObject go in badguys)
            {
                Transform[] allChildren = go.transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.name == bone) //NPC_Spine1 "NPC_Neck"
                    {
                        var shit = Camera.main.WorldToScreenPoint(child.transform.position);
                        if (shit.z > -8)
                        {
                            float dist = System.Math.Abs(Vector2.Distance(new Vector2(shit.x, Screen.height - shit.y), new Vector2((Screen.width / 2), (Screen.height / 2))));
                            if (dist < 300) //in fov
                            {
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    AimTarget = new Vector2(shit.x, Screen.height - shit.y);
                                }
                            }
                        }
                    }
                }
            }
            if(AimTarget != Vector2.zero)
            {
                double DistX = AimTarget.x - Screen.width / 2.0f;
                double DistY = AimTarget.y - Screen.height / 2.0f;

                //aimsmooth
                DistX /= smooth;
                DistY /= smooth;

                //if aimkey is pressed
                if(!menu)
                if (Input.GetKey(aimkey)) mouse_event(0x0001, (int)DistX, (int)DistY, 0, 0);
            }
        }
         catch { }

        /////////MENU HOTKEY////////////
        if (Input.GetKeyDown(KeyCode.Insert)) menu = !menu;

        ////////////////ENEMY LIST UPDATE/////////
        try
        {
            if (one <= two)
            {
                one = one - 1;
                if (one == two - 1)
                {
                    foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
                    {
                        if (go.name.Contains(nameToAdd))
                        {
                            var distance = Vector3.Distance(Camera.main.transform.position, go.transform.position);
                            if (distance > 2)
                            {
                                if (!badguys.Contains(go))
                                {
                                    var rend = go.GetComponentInChildren<Renderer>();
                                    if (rend.material.name.Contains("S") || rend.material.name.Contains("B")) //"p0/Bumped Specular Mask" "p0/Reflective/Bumped Specular Mask W"
                                    {
                                        badguys.Add(go);
                                        if (wallhack)
                                        {
                                            var rend2 = go.GetComponentsInChildren<Renderer>();
                                            chams.Add(go);
                                            foreach (Renderer renderer in rend2)
                                            {
                                                DefaultShader = renderer.material.shader;
                                                renderer.material.shader = Shader.Find("Hidden/Internal-GUITexture");
                                            }
                                        }
                                    }
                                    else if (distance > 12) badguys.Add(go);
                                    else GameObject.Destroy(go);
                                }
                            }
                        }
                    }
                }
            }
            if (one <= 0)
            {
                badguys.Clear();
                if (wallhack)
                {
                    try
                    {
                        foreach (GameObject gameObj in chams)
                        {
                            if (gameObj != null)
                            {
                                var rend = gameObj.GetComponentsInChildren<Renderer>();
                                foreach (Renderer renderer in rend) renderer.material.shader = DefaultShader;
                            }
                        }
                    }
                    catch { }
                }
                one = two;
            }
        }
        catch { }
    }

    void OnGUI()
    {
        GUI.skin.window.active.background = backgroundtexture;
        GUI.skin.window.normal.background = backgroundtexture;
        GUI.skin.window.hover.background = backgroundtexture;
        GUI.skin.window.onFocused.background = backgroundtexture;
        GUI.skin.window.onHover.background = backgroundtexture;
        GUI.skin.window.onActive.background = backgroundtexture;
        GUI.skin.window.onNormal.background = backgroundtexture;
        GUI.skin.window.margin.left = 10;

        
        GUI.Label(new Rect(10, 100, 200, 20), Shit1);
        try
        {
            if (menu) rc = GUI.Window(0, rc, new GUI.WindowFunction(GI), "INTERIUM.OOO © KleskBY 2019");
            foreach (GameObject gameObj in badguys)
            {
                Transform[] allChildren = gameObj.transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.name == "NPC_Head") //NPC_Spine1 "NPC_Neck"
                    {
                        Vector3 w2s = Camera.main.WorldToScreenPoint(gameObj.transform.position);
                        Vector3 w2s2 = Camera.main.WorldToScreenPoint(child.position);
                        if (w2s.z > -1)
                        // if (w2s.z > 0)
                        {
                            // if ((w2s.x > 0) || (w2s.x < Screen.width) || (w2s.y > 0) || (w2s.y < Screen.height))
                            {
                                var distance = Vector3.Distance(Camera.main.transform.position, gameObj.transform.position);
                                //  if (distance > 10)
                                {
                                    float num = Mathf.Abs(w2s.y - w2s2.y);
                                    if (IsVisable(Camera.main.gameObject, child.position))
                                    {
                                        if (BoxEsp) Box(w2s.x, Screen.height - w2s.y, num / 1.8f, num, texture2, 1f);
                                        if (LineESP) DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(w2s.x, Screen.height - w2s.y), Color.green, think, false);
                                        if (DistESP) GUI.Label(new Rect((int)w2s.x - 5, Screen.height - w2s.y - 10, 40, 40), ((int)(distance)).ToString());
                                    }
                                    else
                                    {
                                        if (BoxEsp) Box(w2s.x, Screen.height - w2s.y, num / 1.8f, num, texture, 1f);
                                        if (LineESP) DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(w2s.x, Screen.height - w2s.y), Color.red, think, false);
                                        if (DistESP) GUI.Label(new Rect((int)w2s2.x - 5, Screen.height - w2s2.y - 14, 40, 40), ((int)(distance)).ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
}