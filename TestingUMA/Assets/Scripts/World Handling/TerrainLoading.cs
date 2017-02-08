using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainLoading : MonoBehaviour
{
	static int origin, left, top, right, bottom, topleft, topright, bottomleft, bottomright;
	public Terrain t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24, t25;
	List<Terrain> terrain1, terrain2, terrain3, terrain4, terrain5, terrain6, terrain7, terrain8, terrain9, terrain10, terrain11, terrain12, terrain13, terrain14, terrain15, terrain16, terrain17, terrain18, terrain19, terrain20, terrain21, terrain22, terrain23, terrain24, terrain25;
	static List<List<Terrain>> terrains = new List<List<Terrain>> ();
	static List<Terrain> currentlyActive = new List<Terrain> ();


	// Use this for initialization
	void Start ()
	{
		origin = 0;
		left = 1;
		top = 2;
		right = 3;
		bottom = 4;
		topleft = 5;
		topright = 6;
		bottomleft = 7;
		bottomright = 8;
		Stitch();
		AddToArray ();
		DisableEverything ();

	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public static void UpdateWorld (Terrain currentTerrain)
	{
		for (int i = 0; i < terrains.Count; i++) {
			if (terrains [i].IndexOf (currentTerrain) == origin) {
				//Debug.Log ("Terrain is " + currentTerrain);
//				terrains [i] [left].SetActive (true);
//				terrains [i] [top].SetActive (true);
//				terrains [i] [right].SetActive (true);
//				terrains [i] [bottom].SetActive (true);
//				terrains[i][topleft].SetActive(true);
//				terrains[i][bottomleft].SetActive(true);
//				terrains[i][topright].SetActive(true);
//				terrains[i][bottomright].SetActive(true);

				for(int index = 0; index < terrains[i].Count; index++){
					if(terrains[i][index] != null){
						terrains[i][index].enabled = true;
					}
				}
//		
				List<Terrain> disableArray = new List<Terrain>();

				for(int j = 0; j < currentlyActive.Count; j++){
					if(!terrains[i].Contains(currentlyActive[j])){
						disableArray.Add(currentlyActive[j]);

					}
				}

				for(int k = 0; k< disableArray.Count; k++ ){
					disableArray[k].enabled = false;
					currentlyActive.Remove(disableArray[k]);
				}

				currentlyActive.TrimExcess();

				for(int v = 0; v < terrains[i].Count; v++){
					if(!currentlyActive.Contains(terrains[i][v])){
						currentlyActive.Add(terrains[i][v]);
					}
				}

				currentlyActive.TrimExcess();
				return;
			}
		}
	}

	void EnableNeighbours ()
	{

	}

	void DisableEverything ()
	{


		t2.enabled = false;
		t3.enabled = false;
		t4.enabled = false;
		t5.enabled = false;
		t6.enabled = false;
		t7.enabled = false;
		t8.enabled = false;
		t9.enabled = false;
		t10.enabled = false;
		t11.enabled = false;
		t12.enabled = false;
		t13.enabled = false;
		t14.enabled = false;
		t15.enabled = false;
		t16.enabled = false;
		t17.enabled = false;
		t18.enabled = false;
		t19.enabled = false;
		t20.enabled = false;
		t21.enabled = false;
		t22.enabled = false;
		t23.enabled = false;
		t24.enabled = false;
		t25.enabled = false;



	}

	void Stitch(){
		t1.SetNeighbors(t5,t3,t2,t8);
		t2.SetNeighbors(t1,t4,t19,t7);
		t3.SetNeighbors(t6,t23,t4,t1);
		t4.SetNeighbors(t3,t22,t20,t2);
		t5.SetNeighbors(t11,t6,t1,t9);
		t6.SetNeighbors(t10,t24,t3,t5);
		t7.SetNeighbors(t8,t2,t18,t16);
		t8.SetNeighbors(t9,t1,t7,t15);
		t9.SetNeighbors(t12,t5,t8,t14);
		t10.SetNeighbors(null,t25,t6,t11);
		t11.SetNeighbors(null,t10,t5,t12);
		t12.SetNeighbors(null,t11,t9,t13);
		t13.SetNeighbors(null,t12,t14,null);
		t14.SetNeighbors(t13,t9,t15,null);
		t15.SetNeighbors(t14,t8,t16,null);
		t16.SetNeighbors(t15,t7,t17,null);
		t17.SetNeighbors(t16,t18,null,null);
		t18.SetNeighbors(t7,t19,null,t17);
		t19.SetNeighbors(t2,t20,null,t18);
		t20.SetNeighbors(t4,t21,null,t19);
		t21.SetNeighbors(t22,null,null,t20);
		t22.SetNeighbors(t23,null,t21,t4);
		t23.SetNeighbors(t24,null,t22,t3);
		t24.SetNeighbors(t25,null,t23,t6);
		t25.SetNeighbors(null,null,t24,t10);
	}

	void AddToArray ()
	{
		terrain1 = new List<Terrain> ();
		terrain2 = new List<Terrain> ();
		terrain3 = new List<Terrain> ();
		terrain4 = new List<Terrain> ();
		terrain5 = new List<Terrain> ();
		terrain6 = new List<Terrain> ();
		terrain7 = new List<Terrain> ();
		terrain8 = new List<Terrain> ();
		terrain9 = new List<Terrain> ();
		terrain10 = new List<Terrain> ();
		terrain11 = new List<Terrain> ();
		terrain12 = new List<Terrain> ();
		terrain13 = new List<Terrain> ();
		terrain14 = new List<Terrain> ();
		terrain15 = new List<Terrain> ();
		terrain16 = new List<Terrain> ();
		terrain17 = new List<Terrain> ();
		terrain18 = new List<Terrain> ();
		terrain19 = new List<Terrain> ();
		terrain20 = new List<Terrain> ();
		terrain21 = new List<Terrain> ();
		terrain22 = new List<Terrain> ();
		terrain23 = new List<Terrain> ();
		terrain24 = new List<Terrain> ();
		terrain25 = new List<Terrain> ();



		terrain1.Add (t1);
		terrain1.Add (t5);
		terrain1.Add (t3);
		terrain1.Add (t2);
		terrain1.Add (t8);
		terrain1.Add(t6);
		terrain1.Add(t4);
		terrain1.Add(t9);
		terrain1.Add(t7);

		terrains.Add (terrain1);

		currentlyActive.Add (t1);
		currentlyActive.Add (t5);
		currentlyActive.Add (t3);
		currentlyActive.Add (t2);
		currentlyActive.Add (t8);
		currentlyActive.Add (t6);
		currentlyActive.Add (t4);
		currentlyActive.Add (t9);
		currentlyActive.Add (t7);



		terrain2.Add (t2);
		terrain2.Add (t1);
		terrain2.Add (t4);
		terrain2.Add (t19);
		terrain2.Add (t7);
		terrain2.Add (t3);
		terrain2.Add (t20);
		terrain2.Add (t8);
		terrain2.Add (t18);
		terrains.Add (terrain2);

		terrain3.Add (t3);
		terrain3.Add (t6);
		terrain3 .Add (t23);
		terrain3 .Add (t4);
		terrain3 .Add (t1);
		terrain3 .Add (t24);
		terrain3 .Add (t22);
		terrain3 .Add (t5);
		terrain3 .Add (t2);
		terrains.Add (terrain3);

		terrain4.Add (t4);
		terrain4.Add (t3);
		terrain4 .Add (t22);
		terrain4.Add (t20);
		terrain4 .Add (t2);
		terrain4 .Add (t23);
		terrain4 .Add (t21);
		terrain4 .Add (t1);
		terrain4 .Add (t19);
		terrains.Add (terrain4);

		terrain5 .Add (t5);
		terrain5 .Add (t11);
		terrain5.Add (t6);
		terrain5 .Add (t1);
		terrain5.Add (t9);
		terrain5.Add (t10);
		terrain5.Add (t3);
		terrain5.Add (t12);
		terrain5.Add (t8);
		terrains.Add (terrain5);

		terrain6 .Add (t6);
		terrain6.Add (t10);
		terrain6 .Add (t24);
		terrain6 .Add (t3);
		terrain6 .Add (t5);
		terrain6 .Add (t25);
		terrain6 .Add (t23);
		terrain6 .Add (t1);
		terrain6 .Add (t11);
		terrains.Add (terrain6);

		terrain7 .Add (t7);
		terrain7 .Add (t8);
		terrain7 .Add (t2);
		terrain7 .Add (t18);
		terrain7 .Add (t16);
		terrain7 .Add (t1);
		terrain7 .Add (t19);
		terrain7 .Add (t15);
		terrain7 .Add (t17);
		terrains.Add (terrain7);

		terrain8 .Add (t8);
		terrain8 .Add (t9);
		terrain8 .Add (t1);
		terrain8 .Add (t7);
		terrain8 .Add (t15);
		terrain8 .Add (t5);
		terrain8 .Add (t2);
		terrain8 .Add (t14);
		terrain8 .Add (t16);
		terrains.Add (terrain8);

		terrain9 .Add (t9);
		terrain9 .Add (t12);
		terrain9 .Add (t5);
		terrain9 .Add (t8);
		terrain9 .Add (t14);
		terrain9 .Add (t11);
		terrain9 .Add (t1);
		terrain9 .Add (t13);
		terrain9 .Add (t15);
		terrains.Add (terrain9);

		terrain10 .Add (t10);
		terrain10 .Add (null);
		terrain10 .Add (t25);
		terrain10 .Add (t6);
		terrain10 .Add (t11);
		terrain10 .Add (null);
		terrain10 .Add (t24);
		terrain10 .Add (t5);
		terrain10 .Add (null);
		terrains.Add (terrain10);

		terrain11 .Add (t11);
		terrain11 .Add (null);
		terrain11 .Add (t10);
		terrain11 .Add (t5);
		terrain11 .Add (t12);
		terrain11 .Add (null);
		terrain11 .Add (t6);
		terrain11 .Add (t9);
		terrain11 .Add (null);
		terrains.Add (terrain11);
		
		terrain12 .Add (t12);
		terrain12 .Add (null);
		terrain12 .Add (t11);
		terrain12 .Add (t9);
		terrain12 .Add (t13);
		terrain12 .Add (null);
		terrain12 .Add (t5);
		terrain12 .Add (t14);
		terrain12 .Add (null);
		terrains.Add (terrain12);
		
		terrain13 .Add (t13);
		terrain13 .Add (null);
		terrain13 .Add (t12);
		terrain13 .Add (t14);
		terrain13 .Add (null);
		terrain13 .Add (null);
		terrain13 .Add (t9);
		terrain13 .Add (null);
		terrain13 .Add (null);
		terrains.Add (terrain13);
		
		terrain14 .Add (t14);
		terrain14 .Add (t13);
		terrain14 .Add (t9);
		terrain14 .Add (t15);
		terrain14 .Add (null);
		terrain14 .Add (t12);
		terrain14 .Add (t8);
		terrain14 .Add (null);
		terrain14 .Add (null);
		terrains.Add (terrain14);
		
		terrain15 .Add (t15);
		terrain15.Add (t14);
		terrain15 .Add (t8);
		terrain15 .Add (t16);
		terrain15 .Add (null);
		terrain15 .Add (t9);
		terrain15 .Add (t7);
		terrain15 .Add (null);
		terrain15 .Add (null);
		terrains.Add (terrain15);
		
		terrain16 .Add (t16);
		terrain16 .Add (t15);
		terrain16 .Add (t7);
		terrain16 .Add (t17);
		terrain16 .Add (null);
		terrain16 .Add (t8);
		terrain16 .Add (t18);
		terrain16 .Add (null);
		terrain16 .Add (null);
		terrains.Add (terrain16);
		
		terrain17 .Add (t17);
		terrain17 .Add (t16);
		terrain17 .Add (t18);
		terrain17 .Add (null);
		terrain17 .Add (null);
		terrain17 .Add (t7);
		terrain17 .Add (null);
		terrain17 .Add (null);
		terrain17 .Add (null);
		terrains.Add (terrain17);
		
		terrain18 .Add (t18);
		terrain18 .Add (t7);
		terrain18 .Add (t19);
		terrain18 .Add (null);
		terrain18 .Add (t17);
		terrain18 .Add (t2);
		terrain18 .Add (null);
		terrain18 .Add (null);
		terrain18 .Add (t16);
		terrains.Add (terrain18);
		
		terrain19 .Add (t19);
		terrain19 .Add (t2);
		terrain19.Add (t20);
		terrain19 .Add (null);
		terrain19.Add (t18);
		terrain19.Add (t4);
		terrain19.Add (null);
		terrain19.Add (null);
		terrain19.Add (t7);
		terrains.Add (terrain19);
		
		terrain20 .Add (t20);
		terrain20.Add (t4);
		terrain20 .Add (t21);
		terrain20 .Add (null);
		terrain20 .Add (t19);
		terrain20 .Add (t22);
		terrain20 .Add (null);
		terrain20 .Add (null);
		terrain20 .Add (t2);
		terrains.Add (terrain20);
		
		terrain21 .Add (t21);
		terrain21 .Add (t22);
		terrain21 .Add (null);
		terrain21 .Add (null);
		terrain21 .Add (t20);
		terrain21 .Add (null);
		terrain21 .Add (null);
		terrain21 .Add (null);
		terrain21 .Add (t4);
		terrains.Add (terrain21);
		
		terrain22 .Add (t22);
		terrain22 .Add (t23);
		terrain22 .Add (null);
		terrain22 .Add (t21);
		terrain22 .Add (t4);
		terrain22 .Add (null);
		terrain22 .Add (null);
		terrain22 .Add (t20);
		terrain22 .Add (t3);
		terrains.Add (terrain22);
		
		terrain23 .Add (t23);
		terrain23 .Add (t24);
		terrain23 .Add (null);
		terrain23 .Add (t22);
		terrain23 .Add (t3);
		terrain23 .Add (null);
		terrain23 .Add (null);
		terrain23 .Add (t4);
		terrain23 .Add (t6);
		terrains.Add (terrain23);
		
		terrain24 .Add (t24);
		terrain24 .Add (t25);
		terrain24 .Add (null);
		terrain24 .Add (t23);
		terrain24.Add (t6);
		terrain24.Add (null);
		terrain24.Add (null);
		terrain24.Add (t3);
		terrain24.Add (t10);
		terrains.Add (terrain24);
		
		terrain25 .Add (t25);
		terrain25 .Add (null);
		terrain25.Add (null);
		terrain25 .Add (t24);
		terrain25 .Add (t10);
		terrain25 .Add (null);
		terrain25 .Add (null);
		terrain25 .Add (t6);
		terrain25 .Add (null);
		terrains.Add (terrain25);
	}


}
