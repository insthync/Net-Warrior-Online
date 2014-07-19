package Config;

public class ItemConfig {
	public int id = 0;
	public String name = "";
	public short type = 0; // 0 -> misc, 1 -> usable, >= 2 -> equipment
	public short charclass = 0; // 0 = all class
	public int sellprice = 0;
	// At least ability
	public short level = 0;
	public short strength = 0;
	public short dexterity = 0;
	public short intelligent = 0;
	// Additional ability
	public short add_strength = 0;
	public short add_dexterity = 0;
	public short add_intelligent = 0;
	public int add_maxhp = 0;
	public int add_maxsp = 0;
	public int add_minpatk = 0;
	public int add_maxpatk = 0;
	public int add_minmatk = 0;
	public int add_maxmatk = 0;
	public int add_pdef = 0;
	public int add_mdef = 0;
	public int add_movespd = 0;
	public int add_atkspd = 0;
	public short skillid = 0; // Not available now...
	public ItemConfig(int id,String name, short type, short charclass, int sellprice,
			short level, short strength, short dexterity, short intelligent,
			short add_strength, short add_dexterity, short add_intelligent,
			int add_maxhp, int add_maxsp, int add_minpatk, int add_maxpatk,
			int add_minmatk, int add_maxmatk, int add_pdef, int add_mdef,
			int add_movespd, int add_atkspd, short skillid) {
		this.id = id;
		this.name = name;
		this.type = type;
		this.charclass = charclass;
		this.sellprice = sellprice;
		this.level = level;
		this.strength = strength;
		this.dexterity = dexterity;
		this.intelligent = intelligent;
		this.add_strength = add_strength;
		this.add_dexterity = add_dexterity;
		this.add_intelligent = add_intelligent;
		this.add_maxhp = add_maxhp;
		this.add_maxsp = add_maxsp;
		this.add_minpatk = add_minpatk;
		this.add_maxpatk = add_maxpatk;
		this.add_minmatk = add_minmatk;
		this.add_maxmatk = add_maxmatk;
		this.add_pdef = add_pdef;
		this.add_mdef = add_mdef;
		this.add_movespd = add_movespd;
		this.add_atkspd = add_atkspd;
		this.skillid = skillid;
	}
}
