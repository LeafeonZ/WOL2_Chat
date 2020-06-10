using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStorage : MonoBehaviour {
	// Start is called before the first frame update
	public static Dictionary<string, string> npcChat;

	private void Awake() {
		npcChat = ToDictionary(ReadFile("NPC_Dialogue"), '	');
	}
	// Update is called once per frame

	public static List<string> ReadFile(string fileName) {
		List<string> fileOut = new List<string>();
		try {
			using (StreamReader reader = new StreamReader("Assets/Resources/Sheets/" + fileName + ".tsv")) {
				string line;
				while ((line = reader.ReadLine()) != null && line.Trim() != "") {
					fileOut.Add(line);
				}
				reader.Close();
			}
		} catch {
			Debug.LogError("File Error");
		}


		return fileOut;
	}
	/*public static string[,] ToArray(List<string> file) {
		// first line, [] setup!
		string[] line = file[0].Split(',');
		int length = line.Length;
		string[,] fileOut = new string[line.Length, file.Count];
		for (int y = 1; y < file.Count; y++) {
			line = file[y].Split(',');
			for (int x = 0; x < line.Length; x++) {
				fileOut[x, y] = line[x];
			}
		}
		
		return fileOut;
	}*/
	public static Dictionary<string, string> ToDictionary(List<string> row, char splitter) {
		Dictionary<string, string> d = new Dictionary<string, string>();
		string[] xKey = row[0].Split(splitter);// [X,0] (top line)
		string[] yKey = new string[row.Count]; // fill in as you go. [0,Y] (Left col.)

		string[] cell;
		string testing = "";
		for (int y = 1; y < yKey.Length; y++) {
			cell = row[y].Split(splitter);
			yKey[y] = cell[0];

			for (int x = 1; x < xKey.Length; x++) {
				if (cell[x].Trim() != "") {
					// event stuff would go here, too! 
					d.Add(xKey[x] + yKey[y], cell[x]);

					string v = "";
					d.TryGetValue(xKey[x] + yKey[y], out v);
					testing += "<" + xKey[x] + yKey[y] + ">, " + v + "\n";
				}
			}
		}
		Debug.Log(testing);

		return d;
	}

}
//https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netcore-3.1

//https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
