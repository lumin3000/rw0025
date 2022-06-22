public class Storyteller_ClassicHard : Storyteller_Classic
{
	public Storyteller_ClassicHard()
	{
		name = "Tough Kassandra Classic";
		description = "Cassandra Classic's less forgiving cousin.";
		quotation = "A narrative should climax at a struggle of great intensity. Because the greatest depth of humanity is found in suffering.";
		incidentMaker = new IncidentMaker_ClassicHard();
		intenderPopulation.desiredPopulationMin = 3f;
		intenderPopulation.desiredPopulationMax = 9f;
		intenderPopulation.desiredPopulationCritical = 14f;
	}
}
