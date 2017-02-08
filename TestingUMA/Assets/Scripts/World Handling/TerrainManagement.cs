using UnityEngine;
using System.Collections;

public class TerrainManagement : MonoBehaviour {

	public GameObject currentTerrain;
	
	void Start () {
		currentTerrain = null;
	}
	
	// Update is called once per frame
	void Update () {
		//RaycastHit hitinfo;


	}



	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.normal.y > 0.9f) {
			//Debug.Log ("Colliding with " + hit.collider.gameObject);

			if(currentTerrain != hit.collider.gameObject || currentTerrain == null){
				TerrainLoading.UpdateWorld(hit.collider.GetComponent<Terrain>());

			}
			currentTerrain = hit.collider.gameObject;
			//Debug.Log ("Current Terrain is " + currentTerrain);
		}   //Change if we want incline
			
	}


	
}
