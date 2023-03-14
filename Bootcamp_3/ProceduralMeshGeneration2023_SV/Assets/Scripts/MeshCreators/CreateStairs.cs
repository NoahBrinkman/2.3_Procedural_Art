using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout {
	public class CreateStairs : MonoBehaviour {
		public int numberOfSteps = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;

		MeshBuilder builder;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				numberOfSteps++;
				CreateShape ();
				GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				numberOfSteps--;
				CreateShape ();
				GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
			}
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		void CreateShape() {
			builder.Clear ();

			/**/
			// V1: single step, hard coded:
			// bottom:
			/*int v1 = builder.AddVertex (new Vector3 (2, 0, 0), new Vector2 (1, 0));	
			int v2 = builder.AddVertex (new Vector3 (-2, 0, 0), new Vector2 (0, 0));
			// top front:
			int v3 = builder.AddVertex (new Vector3 (2, 1, 0), new Vector2 (1, 0.5f));	
			int v4 = builder.AddVertex (new Vector3 (-2, 1, 0), new Vector2 (0, 0.5f));
			// top back:
			int v5 = builder.AddVertex (new Vector3 (2, 1, 1), new Vector2 (0, 1));	
			int v6 = builder.AddVertex (new Vector3 (-2, 1, 1), new Vector2 (1, 1));

			builder.AddTriangle (v1, v2, v3);
			builder.AddTriangle (v2, v3, v4);
			builder.AddTriangle (v3, v4, v5);
			builder.AddTriangle (v4, v6, v5);*/

			
			// V2, with for loop:
			for (int i = 0; i < numberOfSteps; i++) {
				Vector3 offset = new Vector3 (width, height * i, depth * i); 

				// TODO 1: use the width and height parameters from the inspector to change the step width and height
				
				// TODO 4: Fix the uvs:
				// bottom:
				int v1 = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (1, 0));	
				int v2 = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (0, 0));
				// top front:
				int v3 = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (1, 0.5f));	
				int v4 = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (0, 0.5f));
				// top back:
				int v5 = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));	
				int v6 = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));

				// TODO 2: Fix the winding order (everything clockwise):
				int v7 = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (1, 0.5f));	
				int v8 = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (0, 0.5f));
				
				builder.AddTriangle(v4,v3,v1);
				builder.AddTriangle(v4,v1,v2);
				builder.AddTriangle(v8,v5,v7);
				builder.AddTriangle(v8,v6,v5);
				
				// TODO 3: make the mesh solid by adding left, right and back side.
				
				int v1S = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (0, 0));	
				int v3S = builder.AddVertex (offset + new Vector3 (width, height, 0), new Vector2 (0, 1));
				int v5S = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));
				
				int v2S = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (1, 0));
				int v4S = builder.AddVertex (offset + new Vector3 (-width, height, 0), new Vector2 (1, 1));
				int v6S = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 0));
				
				int v1B = builder.AddVertex (offset + new Vector3 (width, 0, 0), new Vector2 (1, 0));
				int v2B = builder.AddVertex (offset + new Vector3 (-width, 0, 0), new Vector2 (0, 0));
				int v5B = builder.AddVertex (offset + new Vector3 (width, height, 1), new Vector2 (1, 1));
				int v6B = builder.AddVertex (offset + new Vector3 (-width, height, 1), new Vector2 (0, 1));
				
				builder.AddTriangle(v1S,v3S,v5S); //Right
				builder.AddTriangle(v6S,v4S,v2S); // Left
				
				builder.AddTriangle(v1B,v5B,v6B);//Backleft
				builder.AddTriangle(v1B,v6B,v2B);//BackRight
				
				// TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals (solve it by creating more vertices at the same position)
			}
			/**/
		}
		
	}
}