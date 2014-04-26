using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricGrid : MonoBehaviour {
	
	public GameObject ElectricCellPrefab;
	public float ElectricCellSize = 0.32f;
	public int ElectricGridHeight = 5;
	public int ElectricGridWidth = 5;
	public Color GlowColor = Color.cyan;
	public float GlowSpeed;
	public AnimationCurve GlowCurve;
	
	SpriteRenderer[][] cellsInSlices;
	float animationTime = 0;
	
	void Start() {
		var collider = GetComponent<BoxCollider2D>();
		if (!collider) {
			collider = gameObject.AddComponent<BoxCollider2D>();
		}
		collider.size = new Vector2(ElectricGridWidth * ElectricCellSize, ElectricGridHeight * ElectricCellSize);
		collider.center = new Vector2((ElectricGridWidth - 1)/2f * ElectricCellSize, -(ElectricGridHeight - 1)/2f * ElectricCellSize);

		//create cells and order them in a diagonal pattern
		var numSlices = ElectricGridHeight + ElectricGridWidth - 1;
		var smallerDim = Mathf.Min(ElectricGridHeight, ElectricGridWidth);
		var largerDim = Mathf.Max(ElectricGridHeight, ElectricGridWidth);
		bool widthIsSmaller = smallerDim == ElectricGridWidth;
		
		cellsInSlices = new SpriteRenderer[numSlices][];
		var cellsInSlice = 0;
		for (int slice = 0; slice < numSlices; slice++) {
			if (slice < smallerDim)
				cellsInSlice++;
			else if (slice >= largerDim)
				cellsInSlice--;
			cellsInSlices[slice] = new SpriteRenderer[cellsInSlice];
			
			var currentX = slice < smallerDim ? slice : smallerDim - 1;
			var currentY = slice - currentX;
			for (int i = 0; i < cellsInSlice; i++) {
				var newCell = (GameObject) Instantiate(ElectricCellPrefab);
				newCell.transform.parent = transform;
				if (widthIsSmaller)
					newCell.transform.localPosition = new Vector3(currentX * ElectricCellSize, -currentY * ElectricCellSize, 0);
				else
					newCell.transform.localPosition = new Vector3(currentY * ElectricCellSize, -currentX * ElectricCellSize, 0);
				
				cellsInSlices[slice][i] = newCell.GetComponent<SpriteRenderer>();
				currentX--;
				currentY++;
			}
		}
	}
	
	void Update() {
		animationTime += Time.deltaTime;
		for (int slice = 0; slice < cellsInSlices.Length; slice++) {
			var cells = cellsInSlices[slice];
			var curveTime = animationTime + slice * (1f / GlowSpeed);

			var interp = GlowCurve.Evaluate(curveTime);
			var color = Color.Lerp(Color.white, GlowColor, interp);
			foreach (var cell in cells) {
				cell.color = color;
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = GlowColor;
		Gizmos.DrawLine(
			transform.position + new Vector3(0, 0, 0),
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, 0, 0));
		Gizmos.DrawLine(
			transform.position + new Vector3(0, 0, 0), 
			transform.position + new Vector3(0, -ElectricGridHeight * ElectricCellSize, 0));
		Gizmos.DrawLine(
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, 0, 0), 
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, -ElectricGridHeight * ElectricCellSize, 0));
		Gizmos.DrawLine(
			transform.position + new Vector3(0, -ElectricGridHeight * ElectricCellSize, 0), 
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, -ElectricGridHeight * ElectricCellSize, 0));

	}
}