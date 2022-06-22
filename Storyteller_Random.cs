public class Storyteller_Random : Storyteller
{
	public Storyteller_Random()
	{
		name = "Randy Random";
		description = "Randy doesn't follow rules. He'll generate random events, and he doesn't care if they make a story of triumph or utter hopelessness. It's all drama to him.";
		quotation = "The world is random, dude. Stories should be too! Hey, you got a light?";
		incidentMaker = new IncidentMaker_Random();
		intenderPopulation.desiredPopulationMin = 3f;
		intenderPopulation.desiredPopulationMax = 10f;
		intenderPopulation.desiredPopulationCritical = 15f;
	}
}
