using UnityEngine;

public static class PowerOverlayMats
{
	public static readonly Material MatTransmitterAtlas;

	public static readonly Material MatConnectorBase;

	public static readonly Material MatConnectorLine;

	public static readonly Material MatConnectorAnticipated;

	public static readonly Material MatConnectorBaseAnticipated;

	static PowerOverlayMats()
	{
		MatTransmitterAtlas = MaterialPool.MatFrom("Icons/Special/Power/TransmitterAtlas", MatBases.MetaOverlay);
		MatConnectorBase = MaterialPool.MatFrom("Icons/Special/Power/OverlayBase", MatBases.MetaOverlay);
		MatConnectorLine = MaterialPool.MatFrom("Icons/Special/Power/OverlayWire", MatBases.MetaOverlay);
		MatConnectorAnticipated = MaterialPool.MatFrom("Icons/Special/Power/OverlayWireAnticipated", MatBases.MetaOverlay);
		MatConnectorBaseAnticipated = MaterialPool.MatFrom("Icons/Special/Power/OverlayBaseAnticipated", MatBases.MetaOverlay);
		MatTransmitterAtlas.renderQueue = 3600;
		MatConnectorBase.renderQueue = 3600;
		MatConnectorLine.renderQueue = 3600;
	}
}
