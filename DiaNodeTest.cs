using System.IO;
using System.Xml.Serialization;

public static class DiaNodeTest
{
	public static void SaveSampleNodes()
	{
		DiaNodeSet diaNodeSet = new DiaNodeSet();
		DiaNodeDef diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "MissionA";
		diaNodeDef.Texts.Add("A local town boss is terrorizing the populace.");
		diaNodeDef.Texts.Add("A pedophile is under guard and being taken to trial, but the judge is his friend. A local father offers you a reward if you kill him.");
		diaNodeDef.Texts.Add("A slave trader is reported to have murdered a slave recently here.");
		DiaOptionDef diaOptionDef = new DiaOptionDef();
		diaOptionDef.Text = "Mount a mission to kill him.";
		MissionDef missionDef = new MissionDef();
		missionDef.Attitude = EnemyDisposition.Defensive;
		missionDef.MapList.Add("CityA");
		missionDef.MissionType = MissionType.Assassination;
		missionDef.VicNodeNames.Add("MissionAVic");
		diaOptionDef.MissionToStart = missionDef;
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaOptionDef = new DiaOptionDef();
		diaOptionDef.Text = "Do nothing.";
		diaOptionDef.ChildNodeNames.Add("MissionOpt1");
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "MissionOpt1";
		diaNodeDef.Texts.Add("You allow the crimes to go on and continue on your way.");
		diaOptionDef = new DiaOptionDef();
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "MissionAVic";
		diaNodeDef.Texts.Add("With the crimes punished, the local people offer you a reward.");
		diaNodeDef.Reward = RewardCode.StandardResources;
		diaOptionDef = new DiaOptionDef();
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "MotherInsult";
		diaNodeDef.Texts.Add("A burly shop owner emerges from an old corner store. He seems to think he knows you. He insults your mother.");
		diaOptionDef = new DiaOptionDef();
		diaOptionDef.Text = "Ignore him and walk away.";
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaOptionDef = new DiaOptionDef();
		diaOptionDef.Text = "Insult his mother back.";
		diaOptionDef.ChildNodeNames.Add("ListInsult");
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "WelcomingPeople";
		diaNodeDef.Texts.Add("The people of this town welcome you and supply you with some goods. Perhaps they were just lonely.");
		diaNodeDef.Reward = RewardCode.SmallResources;
		diaOptionDef = new DiaOptionDef();
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "SilentEye";
		diaNodeDef.Texts.Add("The people here eye you from their barricaded homes as you enter town. Nobody comes out to talk, but nobody bothers you either.");
		diaOptionDef = new DiaOptionDef();
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		diaNodeDef = new DiaNodeDef();
		diaNodeDef.Name = "UniqueTest";
		diaNodeDef.Unique = true;
		diaNodeDef.Texts.Add("A fellow named John Smith approaches from a house with worn plastic flamingoes on the lawn and offers you something in thanks. It seems your arrival drove off some scavengers.");
		diaNodeDef.Reward = RewardCode.StandardResources;
		diaOptionDef = new DiaOptionDef();
		diaNodeDef.OptionList.Add(diaOptionDef);
		diaNodeSet.NodeDefList.Add(diaNodeDef);
		DiaNodeList diaNodeList = new DiaNodeList();
		diaNodeList.Name = "ListInsult";
		DiaNodeDef diaNodeDef2 = new DiaNodeDef();
		diaNodeDef2.Texts.Add("He mutters something about foreigners and walks away. In his shop you find a few useful supplies.");
		diaNodeDef2.Reward = RewardCode.SmallResources;
		diaNodeList.Nodes.Add(diaNodeDef2);
		diaNodeDef2 = new DiaNodeDef();
		diaNodeDef2.Texts.Add("He and his friends attack.");
		DiaOptionDef diaOptionDef2 = new DiaOptionDef();
		missionDef = new MissionDef();
		missionDef.Attitude = EnemyDisposition.Aggressive;
		missionDef.MapList.Add("CitySmallA");
		missionDef.MissionType = MissionType.Elimination;
		DiaNodeDef diaNodeDef3 = new DiaNodeDef();
		diaNodeDef3.Texts.Add("Those assholes will think twice about insulting you again.\n\nSearching their corpses, you find a few useful supplies.");
		diaNodeDef3.Reward = RewardCode.SmallResources;
		missionDef.VicNodes.Add(diaNodeDef3);
		diaOptionDef2.MissionToStart = missionDef;
		diaNodeDef2.OptionList.Add(diaOptionDef2);
		diaNodeList.Nodes.Add(diaNodeDef2);
		diaNodeSet.NodeDefListList.Add(diaNodeList);
		SaveDialogNodes(diaNodeSet);
	}

	public static void SaveDialogNodes(DiaNodeSet SavingSet)
	{
		Stream stream = null;
		string path = "Assets/Resources/Dialog/GeneratedDialogs.xml";
		stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(DiaNodeSet));
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add(string.Empty, string.Empty);
		xmlSerializer.Serialize(stream, SavingSet, xmlSerializerNamespaces);
		stream?.Close();
	}
}
