using UnityEngine;

public class JobReport
{
	public string text = "Erroring.";

	public Texture2D overlayTex;

	public static JobReport Error => new JobReport("JobReport error.", null);

	public JobReport(string text, Texture2D overlayTex)
	{
		this.text = text;
		this.overlayTex = overlayTex;
	}
}
