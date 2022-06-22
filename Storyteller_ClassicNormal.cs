public class Storyteller_ClassicNormal : Storyteller_Classic
{
	public Storyteller_ClassicNormal()
	{
		name = "Cassandra Classic";
		description = "Cassandra creates story events on a steadily-increasing curve of challenge and tension. She's good for a first game.";
		quotation = "The essence of good storytelling is the perfect progression of rising tension.";
		incidentMaker = new IncidentMaker_ClassicNormal();
		intenderPopulation.desiredPopulationMin = 3f;
		intenderPopulation.desiredPopulationMax = 10f;
		intenderPopulation.desiredPopulationCritical = 15f;
	}
}
