using UnityEngine;
using System.Collections;

// required
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;

public class TaskPlannerProcess : MonoBehaviour {
	
	private Process PProcessPlanner;
	public string workingDirectory = "";
	private string plannerFilename = "";
	private string PDDLdomFileName = ""; // name without .pddl extension
	private string PDDLprbFileName = ""; // name without .pddl extension
	private string solutionFilename = "";
	public string parsedSolution = "";
	public int actionsFound = 0;
	public Moves[] parsedMoves;
	public Actions[] parsedActions;
	
	public struct Moves
	{
		public int index;
		public string location;
		public string character;
		
	}
	
	public struct Actions
	{
		public int index;
		public string action;
		public string subject;
		public string character;
	}
	
	// generic instantiation and initialisation
	public void InstantiatePlanner()
	{
		PProcessPlanner = new Process();
		
		workingDirectory = Application.dataPath + @"/Planner";
		plannerFilename = workingDirectory + @"/metric-ff.exe";
		PDDLdomFileName = @"task-domain"; // name without .pddl extension
		PDDLprbFileName = @"task-problem"; // name without .pddl extension
		solutionFilename = workingDirectory + @"/ffSolution.soln";
	}
	
	public void ProcessStart()
	{
		// set the information for the Process and get it to run
		// make sure it runs from the Planner's folder
		PProcessPlanner.StartInfo.WorkingDirectory = workingDirectory;
		// metric-ff needs to run using the full explicit path ...
		PProcessPlanner.StartInfo.FileName = plannerFilename;
		PProcessPlanner.StartInfo.Arguments = string.Format("-o {0}.pddl -f {1}.pddl", PDDLdomFileName, PDDLprbFileName);
		PProcessPlanner.StartInfo.CreateNoWindow = true;
		PProcessPlanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		// run the process, and wait until it has closed
		PProcessPlanner.Start();
		PProcessPlanner.WaitForExit();
	}
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	public void SplitSolutions(string[] solutions) {
		
		int moves = 0;
		int actions = 0;
		
		for (int i = 0; i < solutions.Length; i++) {
			string s = solutions[i];
			s = s.Remove (0, 4);
			if (s.StartsWith ("MOVETO", System.StringComparison.CurrentCultureIgnoreCase))
			{
				moves++;
			}
			else
			{
				actions++;
			}
		}
		
		Moves[] moveArray = new Moves[moves];
		Actions[] actionArray = new Actions[actions];
		
		int movesIter = 0;
		int actionsIter = 0;
		for (int i = 0; i < solutions.Length; i++) {
			string s = solutions[i];
			s = s.Remove (0, 4);
			if (s.StartsWith ("MOVETO", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 6;
				int last = s.IndexOf(" ");
				string location = s.Substring(first, last - first);
				//s.Reverse();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf ("N");
				string character = s.Substring(nameindex, 5);
				moveArray[movesIter].index = i;
				moveArray[movesIter].location = location;
				moveArray[movesIter].character = character;
				movesIter++;
			}
			
			if (s.StartsWith("MINE", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 4;
				int last = s.IndexOf(" ");
				string subject = s.Substring (first, last - first);
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "mine";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("CUT", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 3;
				int last = s.IndexOf(" ");
				string subject = s.Substring (first, last - first);
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "cut";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("BUILD", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 5;
				int last = s.IndexOf(" ");
				string subject = s.Substring (first, last - first);
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "build";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("STORE", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 5;
				int last = s.IndexOf(" ");
				string subject = s.Substring (first, last - first);
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "store";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("SMELT", System.StringComparison.CurrentCultureIgnoreCase))
			{
				//int first = 5;
				//int last = s.IndexOf(" ");
				string subject = "coal and ore";
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "smelt";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("QUARRY", System.StringComparison.CurrentCultureIgnoreCase))
			{
				//int first = 3;
				//int last = s.IndexOf(" ");
				string subject = "";
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "quarry";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("SAW", System.StringComparison.CurrentCultureIgnoreCase))
			{
				//int first = 3;
				//int last = s.IndexOf(" ");
				string subject = "timber";
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "saw";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			if (s.StartsWith("MAKE", System.StringComparison.CurrentCultureIgnoreCase))
			{
				int first = 4;
				int last = s.IndexOf(" ");
				string subject = s.Substring (first, last - first);
				//s.Reverse ();
				//s = s.Remove (0, 6);
				//s.Reverse ();
				int nameindex = s.LastIndexOf("N");
				string character = s.Substring(nameindex, 5);
				actionArray[actionsIter].index = i;
				actionArray[actionsIter].action = "make";
				actionArray[actionsIter].subject = subject;
				actionArray[actionsIter].character = character;
				actionsIter++;
			}
			
			
			
		}
		
		parsedMoves = moveArray;
		parsedActions = actionArray;
		
	}
	
	public void ReadFile(){
		
		InstantiatePlanner();
		ProcessStart();
		UnityEngine.Debug.Log ("SOLUTION FOUND");
		
		// (sorry I couldn't resist to add this):
		// using Linq, extract all the lines containing ':', 
		// ie all the lines specifying the actions in the solution generated
		var result = File.ReadAllLines (solutionFilename).Where(s => s.Contains(":"));
		
		// just to show how many actions have been found (*debug*)
		actionsFound = result.Count();
		
		// show the first action parsed in the editor window
		//parsedSolution = result.ToList ()[0].ToString ();
		//UnityEngine.Debug.Log (parsedSolution);
		string[] parsedSolutionStrings = new string[result.Count ()];
		
		for (int i = 0; i < result.Count(); i++) {
			parsedSolution = result.ToList ()[i].ToString ();
			parsedSolutionStrings[i] = parsedSolution;
			UnityEngine.Debug.Log (parsedSolution);
		}
		
		SplitSolutions (parsedSolutionStrings);
		
		foreach (Moves m in parsedMoves) {
			UnityEngine.Debug.Log (m.index);
			UnityEngine.Debug.Log (m.location);
			UnityEngine.Debug.Log (m.character);

			int temp = 0;
			int.TryParse(m.location, out temp);
			GameObject.Find (m.character).GetComponent<Labourer>().newTask(temp, m.location);
		}
		
		foreach (Actions a in parsedActions) {
			UnityEngine.Debug.Log (a.index);
			UnityEngine.Debug.Log (a.action);
			UnityEngine.Debug.Log (a.subject);
			UnityEngine.Debug.Log (a.character);

			GameObject.Find(a.character).GetComponent<Labourer>().newTask(a.action, a.subject);
		}
		
		// delete the solution file, so you don't get to read it again next time you generate a new solution
		File.Delete (solutionFilename);
		UnityEngine.Debug.Log ("FILE DELETED");
		
		//GetComponent<WriteProblemFile> ().WriteFile ();


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
