// Version 2023
//  (Updates: supports different root positions)
using UnityEngine;

namespace Demo {
	public class GridCity : MonoBehaviour {
		public int rows = 10;
		public int columns = 10;
		public int rowWidth = 10;
		public int columnWidth = 10;
		public GameObject[] buildingPrefabs;
		
		public float buildDelaySeconds = 0.1f;

		[SerializeField] private float minXOffset;
		[SerializeField] private float maxXOffset;
		[SerializeField] private float minZOffset;
		[SerializeField] private float maxZOffset;
		void Start() {
			Generate();
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.G)) {
				DestroyChildren();
				Generate();
			}
		}

		void DestroyChildren() {
			for (int i = 0; i<transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}

		void Generate() {
			for (int row = 0; row<rows; row++) {
				for (int col = 0; col<columns; col++) {
					// Create a new building, chosen randomly from the prefabs:
					int buildingIndex = Random.Range(0, buildingPrefabs.Length);
					GameObject newBuilding = Instantiate(buildingPrefabs[buildingIndex], transform);

					// Place it in the grid:
					float xOffset = Random.Range(minXOffset,maxXOffset);
					float zOffset = Random.Range(minZOffset, maxZOffset);
					newBuilding.transform.localPosition = new Vector3(col * columnWidth + xOffset, 0, row*rowWidth + zOffset);

					// If the building has a Shape (grammar) component, launch the grammar:
					Shape shape = newBuilding.GetComponent<Shape>();
					if (shape!=null) {
						shape.Generate(buildDelaySeconds);
					}
				}
			}
		}
	}
}