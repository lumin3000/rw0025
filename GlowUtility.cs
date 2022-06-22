public static class GlowUtility
{
	public static string HumanName(this PsychGlow gl)
	{
		return gl switch
		{
			PsychGlow.Dark => "Dark", 
			PsychGlow.Lit => "Lit", 
			PsychGlow.Overlit => "Brightly lit", 
			_ => "Glow error", 
		};
	}
}
