using UnityEngine;
using System.Collections;

public class Rifleman : MonoBehaviour {

	public Vector2 position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Combat(GameObject other)
	{
		if (other.GetComponent<Labourer> () != null) 
		{
			other.GetComponent<Unit> ().Death ();
		} 
		else if (other.GetComponent<Rifleman> () != null) 
		{
			int rand = Random.Range(0, 101);

			if (rand < 70)
			{
				other.GetComponent<Rifleman>().Death();
			}
		}
	}

	void Death()
	{
		Destroy (this.transform);
	}
}
