using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingJob {

	public BuildingJob
	(
		Image BuildingJobIcon, 
		string BuildingJobName, 
		float LabourNeeded, 
		float overFlow, 
		OnLabourDoneDelegate OnLabourDone, 
		ProductionBonusDelegate ProductionBonusFunction
	)
	{
		if (OnLabourDone == null)
			throw new UnityException ();
		this.BuildingJobIcon = BuildingJobIcon;
		this.BuildingJobName = BuildingJobName;
		this.LabourNeeded = LabourNeeded;
		LabourDone = float overFlow;
		this.OnLabourDone = OnLabourDone;
		this.ProductionBonusFunction = ProductionBonusFunction;
	}

	public float LabourNeeded;
	public float LabourDone;

	public Image BuildingJobIcon;
	public string BuildingJobName;

	public delegate void OnLabourDoneDelegate();
	public event OnLabourDoneDelegate OnLabourDone;

	public delegate float ProductionBonusDelegate();
	public ProductionBonusDelegate ProductionBonusFunction;

	public float DoWork(float BaseLabour)
	{
		if (ProductionBonusFunction != null) 
		{
			BaseLabour *= ProductionBonusFunction ();
		}
		LabourDone += BaseLabour;
		if (LabourDone >= LabourNeeded) 
		{
			OnLabourDone ();
		}

		return LabourNeeded - LabourDone;

	}

}
