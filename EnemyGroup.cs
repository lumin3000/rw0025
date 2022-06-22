using System.Collections.Generic;

public class EnemyGroup
{
	public int weight;

	public int cost;

	public List<string> pawnKindNames = new List<string>();

	public EnemyGroup(int weight, int cost)
	{
		this.weight = weight;
		this.cost = cost;
	}
}
