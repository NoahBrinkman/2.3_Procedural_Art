using System.Collections;
using System.Collections.Generic;
using Demo;
using UnityEngine;

public class TreeStack : Shape
{
	public GameObject prefab;
	public GameObject leaf;
	public int HeightRemaining;
	public int depth;
		public void Initialize(GameObject pPrefab, int pHeightRemaining, int pDepth, GameObject pLeaf = null) {
			prefab=pPrefab;
			HeightRemaining=pHeightRemaining;
			depth = pDepth;
			leaf = pLeaf;
		}

		protected override void Execute() {
			// Spawn the (box) prefab as child of this game object:
			// (Optional parameters: localPosition, localRotation, alternative parent)
			//GameObject box = SpawnPrefab(prefab);
	
			// Example: fat box:
			//box.transform.localScale=new Vector3(7, 1, 1);
			for (int i = 0; i < HeightRemaining; i++)
			{
				GameObject box = SpawnPrefab(prefab, new Vector3(0,0 + i,0));
			}
			
			
			if (depth>0) {
				TreeStack branch = null;

				if (HeightRemaining>8) {
					HeightRemaining=8;
				}
				for (int i = 0; i<2; i++) {
					branch = CreateSymbol<TreeStack>("stack", new Vector3(i%2==0?.25f:-.25f, HeightRemaining + .25f, 0));
					branch.Initialize(prefab, HeightRemaining, depth-1, leaf);
					branch.transform.localScale=new Vector3(0.6f, 0.6f, 0.6f);
					branch.transform.localRotation = Quaternion.Euler(i % 2 == 0 ? 60 : -60,90,0);
					branch.Generate(buildDelay);
				}
				/**/
				// Simple stack:
				// Spawn a smaller stack on top of this:
				/*branch = CreateSymbol<TreeStack>("stack", new Vector3(0, 1, 0));
				branch.Initialize(prefab, HeightRemaining, depth-1);
				branch.transform.localScale = new Vector3(.9f, .9f, .9f);
				branch.transform.RotateAround(transform.up,new Vector3(1,0,0), 45);
				// Generate it with a short delay (when in play mode):
				branch.Generate(buildDelay);
				otherBranch = CreateSymbol<TreeStack>("stack", new Vector3(0, 1, 0));
				otherBranch.Initialize(prefab, HeightRemaining, depth-1);
				otherBranch.transform.localScale = new Vector3(.9f, .9f, .9f);
				otherBranch.transform.RotateAround(transform.up,new Vector3(1,0,0), -45);
				// Generate it with a short delay (when in play mode):
				otherBranch.Generate(buildDelay);*/

				/**
				// Scaling:
				// Every new stack gets a bit smaller:
				newStack = CreateSymbol<Stack>("stack", new Vector3(0, 1, 0));
				newStack.Initialize(prefab, HeightRemaining-1);
				newStack.transform.localScale=new Vector3(0.9f, 0.9f, 0.9f);
				newStack.Generate(buildDelay); 

				/**
				// Rotation:
				// Every new stack rotates by 30 degrees around the y-axis:
				newStack = CreateSymbol<Stack>("stack", new Vector3(0, 1, 0));
				newStack.Initialize(prefab, HeightRemaining-1);
				newStack.transform.localRotation = Quaternion.Euler(0, 30, 0);
				newStack.Generate(buildDelay); 

				/**
				// Rotation & scaling:
				// Every new stack rotates by 45 degrees around the z-axis, and becomes a bit smaller:
				newStack = CreateSymbol<Stack>("stack", new Vector3(-0.25f, 1.25f, 0));
				newStack.Initialize(prefab, HeightRemaining-1);
				newStack.transform.localRotation = Quaternion.Euler(0, 0, 45);
				newStack.transform.localScale=new Vector3(0.707f, 0.707f, 0.707f);
				newStack.Generate(buildDelay); 				
			
				/**
				// Two smaller child stacks, spawned with an offset:
				// **** WARNING: don't do this with HeighRemaining values larger than about 8! ****
				//      (exponential growth breaks computers!)
				if (HeightRemaining>8) {
					HeightRemaining=8;
				}
				for (int i = 0; i<2; i++) {
					newStack = CreateSymbol<Stack>("stack", new Vector3(i-0.5f, 1, 0));
					newStack.Initialize(prefab, HeightRemaining-1);
					newStack.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
					newStack.Generate(buildDelay);
				}
				/**/
			}
			else
			{
				float offSet = RandomFloat();
				GameObject leaf = SpawnPrefab(this.leaf, new Vector3(0 + offSet, 3 + offSet, 0));
				leaf.transform.localScale = new Vector3(25, 25, 25);
			}
		}
	}
