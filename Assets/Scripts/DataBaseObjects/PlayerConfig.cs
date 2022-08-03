using UnityEngine;
using SQLite4Unity3d;

public class PlayerConfig {
	[PrimaryKey]
	public string PlayerName { get; set; }
	// setting config
	public InputSetting inputSetting { get; set; }
	public ScreenResolution screenResolution { get; set; }
	public SoundSetting soundSetting { get; set; }
	public GraphicQuality graphicQuality { get; set; }
}

public class ScreenResolution {
	public int width;
	public int height;
}

public class SoundSetting {
	public float MusicVolume;
	public float MasterVolume;
}

public class InputSetting {
	public float LookSpeed;
	public string OpenInventory;
}

public class GraphicQuality {

}
