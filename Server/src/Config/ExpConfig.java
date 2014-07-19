package Config;

import java.util.HashMap;

public class ExpConfig {
	public HashMap<Short, Integer> explist;
	private short maxLv = Short.MIN_VALUE;
	public ExpConfig() {
		explist = new HashMap<Short, Integer>();
	}

	public ExpConfig(short lv, int exp) {
		explist = new HashMap<Short, Integer>();
		explist.put(lv, exp);
	}
	
	public void addExp(short lv, int exp) {
		if (!explist.containsKey(lv)) {
			explist.put(lv, exp);
		} else {
			explist.remove(lv);
			explist.put(lv, exp);
		}
		if (lv > maxLv)
			maxLv = lv;
	}

	public int getExp(short lv) {
		if (explist.containsKey(lv))
			return explist.get(lv);
		return 0;
	}
	
	public short getMaxLv() {
		return maxLv;
	}
}
