public class Storyteller_Friendly : Storyteller
{
	public Storyteller_Friendly()
	{
		name = "Phoebe Friendly";
		description = "Phoebe doesn't want to hurt anyone. She just wants a relaxing tale about people building a colony. So she'll be very friendly.";
		quotation = "Can't we all just get along? I don't want a story that makes me feel tense all the time. Let's make one about friendship and hope.";
		incidentMaker = new IncidentMaker_Friendly();
		intenderPopulation.desiredPopulationMin = 3f;
		intenderPopulation.desiredPopulationMax = 16f;
		intenderPopulation.desiredPopulationCritical = 50f;
	}
}
