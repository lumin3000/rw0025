using UnityEngine;

public class CompPowerBattery : CompPower
{
	public float storedEnergyMax;

	public float efficiency;

	public float storedEnergy;

	public float AmountCanAccept => (storedEnergyMax - storedEnergy) / efficiency;

	public override void CompExposeData()
	{
		Scribe.LookField(ref storedEnergy, "StoredPower");
		if (storedEnergy > storedEnergyMax)
		{
			storedEnergy = storedEnergyMax;
		}
	}

	public void StorePower(float amount)
	{
		amount *= efficiency;
		storedEnergy += amount;
	}

	public void DrawPower(float amount)
	{
		if (storedEnergy < amount)
		{
			Debug.LogWarning("Drawing power we don't have from " + parent);
		}
		storedEnergy -= amount;
	}

	public override string CompInspectString()
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + "Stored: " + storedEnergy.ToString("######0.0") + " / " + storedEnergyMax.ToString("######0.0") + " Wd";
		empty = empty + "\nEfficiency: " + (efficiency * 100f).ToString("##0") + "%";
		return empty + "\n" + base.CompInspectString();
	}
}
