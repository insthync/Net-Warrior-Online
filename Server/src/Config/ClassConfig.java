package Config;

public class ClassConfig {
	public short strength = 0;
	public short dexterity = 0;
	public short intelligent = 0;
	public ClassConfig(short strength, short dexterity, short intelligent) {
		this.strength = strength;
		this.dexterity = dexterity;
		this.intelligent = intelligent;
	}
	public static int hpMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return strength * 3;
	}
	public static int spMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return intelligent * 2;
	}
	public static int[] patkMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		int[] atk = new int[2];
		atk[0] = (int)(strength * 1.25);
		atk[1] = (int)(strength * 1.5);
		return atk;
	}
	public static int[] matkMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		int[] atk = new int[2];
		atk[0] = (int)(intelligent * 1.25);
		atk[1] = (int)(intelligent * 1.5);
		return atk;
	}
	public static int pdefMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return strength * 1 + (int)(dexterity * 0.5);
	}
	public static int mdefMultiplier(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return intelligent * 1 + (int)(dexterity * 0.5);
	}
	public static int movespd(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return (int)(dexterity * 0.075) + 100;
	}
	public static int atkspd(short lv, short strength, short dexterity, short intelligent, ClassConfig cfg/* may use for some bonus */) {
		return (int)(dexterity * 0.075) + 1;
	}
}
