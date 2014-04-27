using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricGrid : MonoBehaviour {

	public enum Mode { Normal, TurningOn, TurningOff }
	
	public GameObject ElectricCellPrefab;
	public float ElectricCellSize = 0.32f;
	public int ElectricGridHeight = 5;
	public int ElectricGridWidth = 5;
	public Color GlowColor = Color.cyan;
	public float GlowSpeed;
	public AnimationCurve GlowCurve;
	public AnimationCurve OffCurve;
	public AnimationCurve OnCurve;
	public Mode AnimationMode = Mode.Normal;

	SpriteRenderer[][] cellsInSlices;
	float animationTime;
	
	void Start() {
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
		switch (AnimationMode) {
		case Mode.Normal:
			for (int slice = 0; slice < cellsInSlices.Length; slice++) {
				var cells = cellsInSlices[slice];
				var curveTime = animationTime + slice * (1f / GlowSpeed);
				
				var interp = GlowCurve.Evaluate(curveTime);
				var color = Color.Lerp(Color.white, GlowColor, interp);
				foreach (var cell in cells) {
					cell.color = color;
				}
			}
			break;
		case Mode.TurningOn:
			for (int slice = 0; slice < cellsInSlices.Length; slice++) {
				var cells = cellsInSlices[slice];
				var interp = OnCurve.Evaluate(animationTime);
				var color = Color.Lerp(Color.gray, Color.white, interp);
				foreach (var cell in cells) {
					cell.color = color;
				}
			}
			break;
		case Mode.TurningOff:
			for (int slice = 0; slice < cellsInSlices.Length; slice++) {
				var cells = cellsInSlices[slice];
				var interp = OffCurve.Evaluate(animationTime);
				var color = Color.Lerp(Color.white, Color.gray, interp);
				foreach (var cell in cells) {
					cell.color = color;
				}
			}
			break;
		}
	}

	public void TurnOff() {
		AnimationMode = Mode.TurningOff;
		animationTime = 0f;
	}

	public void TurnOn() {
		var onTime = OnCurve.keys[OnCurve.length - 1].time;
		AnimationMode = Mode.TurningOn;
		animationTime = 0f;
		StartCoroutine(Util.PerformActionWithDelay(onTime,() => {
			AnimationMode = Mode.Normal;
			animationTime = 0f;
		}));
	}

	void OnDrawGizmos() {
		Gizmos.color = GlowColor;
		var offset = new Vector3(-ElectricCellSize/2, ElectricCellSize/2, 0);
		Gizmos.DrawLine(
			transform.position + new Vector3(0, 0, 0) + offset,
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, 0, 0) + offset);
		Gizmos.DrawLine(
			transform.position + new Vector3(0, 0, 0) + offset, 
			transform.position + new Vector3(0, -ElectricGridHeight * ElectricCellSize, 0) + offset);
		Gizmos.DrawLine(
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, 0, 0) + offset, 
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, -ElectricGridHeight * ElectricCellSize, 0) + offset);
		Gizmos.DrawLine(
			transform.position + new Vector3(0, -ElectricGridHeight * ElectricCellSize, 0) + offset, 
			transform.position + new Vector3(ElectricGridWidth * ElectricCellSize, -ElectricGridHeight * ElectricCellSize, 0) + offset);
	}
}