package Config;

public class MonsterConfig {
	public int id = 0;
	public String name = "";
	public short charclass = 0;
	public short level = 0;
	public int exp = 0;
	public short strength = 0;
	public short dexterity = 0;
	public short intelligent = 0;
	public short model_hair = 0;
	public short model_face = 0;
	public long reborntime = 0;
	// May have itemlist ...
	public MonsterConfig(int id, String name, short charclass, short level, int exp, short strength, short dexterity, short intelligent,
			short model_hair, short model_face, long reborntime) {
		this.id = id;
		this.name = name;
		this.charclass = charclass;
		this.level = level;
		this.exp = exp;
		this.strength = strength;
		this.dexterity = dexterity;
		this.intelligent = intelligent;
		this.model_hair = model_hair;
		this.model_face = model_face;
		this.reborntime = reborntime;
	}
}
