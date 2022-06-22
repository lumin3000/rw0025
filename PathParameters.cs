public static class PathParameters
{
	public static PathingParameters smart;

	public static PathingParameters animal;

	public static PathingParameters robot;

	public static PathingParameters stupid;

	static PathParameters()
	{
		smart = new PathingParameters();
		smart.lockedHostileDoorExtraCost = 250;
		animal = new PathingParameters();
		animal.lockedHostileDoorExtraCost = 100;
		robot = new PathingParameters();
		robot.lockedHostileDoorExtraCost = 50;
		stupid = new PathingParameters();
		stupid.lockedHostileDoorExtraCost = 50;
	}
}
