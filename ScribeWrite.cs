using System.IO;
using System.Xml;

public static class ScribeWrite
{
	public static string ScribeString(Saveable sav)
	{
		//Discarded unreachable code: IL_006a, IL_007c
		using StringWriter stringWriter = new StringWriter();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.IndentChars = "\t";
		xmlWriterSettings.OmitXmlDeclaration = true;
		using (Scribe.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
		{
			LoadSaveMode mode = Scribe.mode;
			Scribe.mode = LoadSaveMode.Saving;
			Scribe.writingForDebug = true;
			Scribe.LookSaveable(ref sav, "Saveable");
			Scribe.mode = mode;
			Scribe.writingForDebug = false;
			return stringWriter.ToString();
		}
	}
}
