public class Blueprint_Door : Blueprint
{
	public override void Draw()
	{
		rotation = Building_Door.DoorRotationAt(base.Position);
		base.Draw();
	}
}
