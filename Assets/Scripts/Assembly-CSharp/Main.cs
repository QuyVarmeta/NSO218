using System.Threading;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main main;

	public static GameCanvas canvas;

	public static mGraphics g;

	public static GameMidlet midlet;

	public static string res = "res";

	public static string mainThreadName;

	public static bool started;

	public static bool isIpod;

	public static bool isAppTeam;

	public static bool isPC;

	public static bool isWp;

	public static bool IphoneVersionApp;

	public static string IMEI;

	public static int versionIp;

	public static int level;

	public bool isWorldver;

	private int updateCount;

	private int paintCount;

	private Vector2 lastMousePos = default(Vector2);

	public static int a = 1;

	public static bool isCompactDevice = true;

	private void Start()
	{
		if (!started)
		{
			if (isPC)
			{
				level = RMS.loadRMSInt("levelScreenKN");
				if (level == 1)
				{
					Screen.SetResolution(480, 320, false);
				}
				else
				{
					Screen.SetResolution(1024, 600, false);
				}
			}
			if (Thread.CurrentThread.Name != "Main")
			{
				Thread.CurrentThread.Name = "Main";
			}
			mainThreadName = Thread.CurrentThread.Name;
			if (iPhoneSettings.generation == iPhoneGeneration.iPodTouch4Gen)
			{
				isIpod = true;
			}
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Application.runInBackground = true;
			Application.targetFrameRate = 60;
			base.useGUILayout = false;
			isCompactDevice = detectCompactDevice();
			if (main == null)
			{
				main = this;
			}
			started = true;
			ScaleGUI.initScaleGUI();
			IMEI = SystemInfo.deviceUniqueIdentifier;
			if(Application.platform is RuntimePlatform.Android || Application.platform is RuntimePlatform.IPhonePlayer)
            {
				isPC = false;
				if(Application.platform is RuntimePlatform.IPhonePlayer)
                {
					IphoneVersionApp = true;
                }
            }
            else
            {
				isPC = true;
            }
			isWp = false;
			isAppTeam = false;
			if (isPC)
			{
				Screen.fullScreen = false;
			}
			g = new mGraphics();
			midlet = new GameMidlet();
			canvas = new GameCanvas();
			TileMap.loadTileMapArr();
			SplashScr.gI().switchToMe();
			Sound.init();
		}
	}

	public void doClearRMS()
	{
		if (isPC)
		{
			int num = RMS.loadRMSInt("lastZoomlevel");
			if (num != mGraphics.zoomLevel)
			{
				RMS.clearRMS();
				RMS.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
				RMS.saveRMSInt("levelScreenKN", level);
			}
		}
	}

	private void OnGUI()
	{
		checkInput();
		Session_ME.update();
		if (Event.current.type.Equals(EventType.Repaint) && paintCount <= updateCount)
		{
			canvas.paint(g);
			paintCount++;
			g.reset();
		}
	}

	private void FixedUpdate()
	{
		ipKeyboard.update();
		canvas.update();
		RMS.update();
		Image.update();
		DataInputStream.update();
		Net.update();
		updateCount++;
		Sound.update();
		Code.Fixed();
	}

	private void Update()
	{
		if (!isPC)
		{
			int num = 1 / a;
		}
	}

	private void checkInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = Input.mousePosition;
			canvas.pointerPressed((int)(mousePosition.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition.y) / (float)mGraphics.zoomLevel));
			lastMousePos.x = mousePosition.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition.y / (float)mGraphics.zoomLevel;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition2 = Input.mousePosition;
			canvas.pointerDragged((int)(mousePosition2.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition2.y) / (float)mGraphics.zoomLevel));
			lastMousePos.x = mousePosition2.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition2.y / (float)mGraphics.zoomLevel;
		}
		if (Input.GetMouseButtonUp(0))
		{
			Vector3 mousePosition3 = Input.mousePosition;
			lastMousePos.x = mousePosition3.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition3.y / (float)mGraphics.zoomLevel;
			canvas.pointerReleased((int)(mousePosition3.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition3.y) / (float)mGraphics.zoomLevel));
		}
		if (Input.anyKeyDown && Event.current.type == EventType.KeyDown)
		{
			int num = MyKeyMap.map(Event.current.keyCode);
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				switch (Event.current.keyCode)
				{
				case KeyCode.Alpha2:
					num = 64;
					break;
				case KeyCode.Minus:
					num = 95;
					break;
				}
			}
			if (num != 0)
			{
				canvas.keyPressed(num);
			}
		}
		if (Event.current.type == EventType.KeyUp)
		{
			int num2 = MyKeyMap.map(Event.current.keyCode);
			if (num2 != 0)
			{
				canvas.keyReleased(num2);
			}
		}
	}

	private void OnApplicationQuit()
	{
		Code.T = null;
		Debug.LogWarning("APP QUIT");
		GameCanvas.bRun = false;
		Session_ME.gI().close();
		Code.Stop();
		if (isPC)
		{
			Application.Quit();
		}
	}

	public static void exit()
	{
		if (isPC)
		{
			main.OnApplicationQuit();
		}
		else
		{
			a = 0;
		}
	}

	public static bool detectCompactDevice()
	{
		if (iPhoneSettings.generation == iPhoneGeneration.iPhone || iPhoneSettings.generation == iPhoneGeneration.iPhone3G || iPhoneSettings.generation == iPhoneGeneration.iPodTouch1Gen || iPhoneSettings.generation == iPhoneGeneration.iPodTouch2Gen)
		{
			return false;
		}
		return true;
	}

	public static bool checkCanSendSMS()
	{
		if (iPhoneSettings.generation == iPhoneGeneration.iPhone3GS || iPhoneSettings.generation == iPhoneGeneration.iPhone4 || iPhoneSettings.generation > iPhoneGeneration.iPodTouch4Gen)
		{
			return true;
		}
		return false;
	}
}
