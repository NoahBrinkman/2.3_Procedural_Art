using UnityEngine;
using UnityEditor;

namespace Demo {
	[CustomEditor(typeof(BuildingPainter))]
	public class BuildingPainterEditor : Editor {

        private BuildingPainter painter;

        private void OnEnable()
        {
            painter = (BuildingPainter)target;
        }

        public void OnSceneGUI() {
			Event e = Event.current;
			if (e.type == EventType.KeyDown )
            {
	            if (e.keyCode == KeyCode.E)
	            {
		            // TODO (Ex 2): raycast into the scene.
		            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

		            RaycastHit hit;
		            if (Physics.Raycast(ray, out hit))
		            {
			            //EditorGUI.BeginChangeCheck();
			            BuildingPainter bp = target as BuildingPainter;
			            Undo.RecordObject(bp, "buidings");
			            GameObject building = bp.CreateBuilding(hit.point);
			            Undo.RegisterCreatedObjectUndo(building, "spawnedBuilding");
					
					
					
			            Debug.Log($"TODO: Spawn a building, right here! {hit.point}");
		            }
	            }

	            if (e.keyCode == KeyCode.Delete)
	            {
		            if (painter.createdBuildings.Count > 0)
		            {
			            GameObject g = painter.createdBuildings[painter.createdBuildings.Count - 1];
			            painter.createdBuildings.Remove(g);
						 DestroyImmediate(g);
		            }
	            }
	            e.Use();
				//  If this painter object is hit, create a new house and add it to 
				//   BuildingPainter's list.
				//  Optionally: if a previously generated house is hit, destroy it again.
				//  Don't forget to register these action for undo, and mark the scene as dirty.


            }

            DrawBuildingTransforms();
		}


        private void DrawBuildingTransforms()
        {
            for (int i = 0; i < painter.createdBuildings.Count; i++)
            {
				// Take care of destroying buildings manually:
                GameObject building = painter.createdBuildings[i];
				if (building==null) {
					painter.createdBuildings.RemoveAt(i);
					i--;
					continue;
				}

				// TODO (Ex 2): Draw a handle at the position of this building.
				Vector3 newPosition = painter.createdBuildings[i].transform.position;
					Quaternion newRotation = painter.createdBuildings[i].transform.rotation;
					Vector3 newScale = painter.createdBuildings[i].transform.localScale;
					EditorGUI.BeginChangeCheck();
					Handles.TransformHandle(ref newPosition, ref newRotation, ref newScale);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(painter.createdBuildings[i].transform, "transformChange");
						painter.createdBuildings[i].transform.position = newPosition;
						painter.createdBuildings[i].transform.rotation = newRotation;
						painter.createdBuildings[i].transform.localScale = newScale;
					}
					//  Try to draw a position, rotation and scale gizmo at the same time.
				//  Don't forget to record changes in the undo list, and mark the scene as dirty.
            }
        }
	}
}