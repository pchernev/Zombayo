var startText : String;
var consolewindow = Rect (0,0,375,200); 				// ConsoleWindow properties
private var consoleInput = ArrayList(); 				// Array for Console Text
private var inputField : String = "";                	// Inputfield
private var scrollPosition : Vector2; 
private var developerconsole : boolean=false;;
private var processed : String;	

var spawn : Transform;									//EXAMPLE Object

//*********************************************************************
function Awake()
{
	DontDestroyOnLoad (transform.gameObject);
	//STARTTEXT Example
	startText = "# Developer console version 1.0.0.1";
}
//*********************************************************************
function Start()
{
ConsoleText(startText);
}	

//*********************************************************************
function Update () 
{
	//********************************
	if(Input.GetKeyDown (KeyCode.Tab))					//Console enable/disable with pressing TAB
	{
		if(developerconsole == false)					//if false
		{	
			developerconsole = true;					//Console enable
		}												
		else
		{
			developerconsole = false;					//Console disable
		}					
	}

	//********************************
	//Commands	EXAMPLES (Here you need to add your functions)
	switch(processed)
	{
		case ".commands": 			ConsoleText("# .load_levelX\t\t|| 1 = Scene1; 2 = Scene2\n"	+
												"# .spawn\t\t\t|| Spawn 1Object\n"	+
												"# .spawn_endless\t|| Spawn endless Objects\n\t\t\t\t\t\t||Stop with input into console\n"	+
												"# .destroy_all\t\t||Destroys all Objects");		 break;
		
		//Examples
		case ".load_level1":	Application.LoadLevel("Scene1"); ConsoleText("# Loaded - Scene1 -");	break;
		case ".load_level2":	Application.LoadLevel("Scene2"); ConsoleText("# Loaded - Scene2 -");	break;	
		case ".spawn":			Instantiate (spawn, Vector3(0, 2, 0), transform.rotation); ConsoleText("# spawned Object finished\n"); break;
		case ".spawn_endless":	Instantiate (spawn, Vector3(0, 2, 0), transform.rotation); break;		//Stop with any Input into console
		case ".destroy_all":	var objects = GameObject.FindGameObjectsWithTag ("Object");
								for (var delete in objects)
								Destroy(delete);
								break;
								
	}
}

//********************************************************************
//DO NOT CHANGE THIS SCRIPT PART

function OnGUI(){

	if(developerconsole == true)	{ consolewindow = GUI.Window (1, consolewindow, ConsoleWindow, "Developer console"); }
	
}

function ConsoleWindow (id : int) 
{
	scrollPosition = GUILayout.BeginScrollView (scrollPosition);

	for (var output : String in consoleInput)
	{
		processed = output;		
		GUILayout.BeginHorizontal();
		GUILayout.Label(output);
		GUILayout.EndHorizontal();	
	}
    GUILayout.EndScrollView (); 
	
	if ( 
		 Event.current.character == "\n" &&
		 Event.current.type == EventType.keyDown &&
		 inputField.Length > 0
		)
	{      		
		ConsoleText(inputField); 
		inputField = "";
	}
	inputField = GUILayout.TextField(inputField);	
	GUI.DragWindow();
}

//******************************
//Add Console Text
function ConsoleText(Text : String)
{
	consoleInput.Add(Text);
}



