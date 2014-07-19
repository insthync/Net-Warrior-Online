package Config;

import java.util.HashMap;

public class DropConfig {
	public HashMap<Integer, Integer> itemList;
	public DropConfig() {
		itemList = new HashMap<Integer, Integer>();
	}

	public DropConfig(int itemid, int droprate) {
		itemList = new HashMap<Integer, Integer>();
		itemList.put(itemid, droprate);
	}
	
	public void addItem(int itemid, int droprate) {
		if (!itemList.containsKey(itemid)) {
			itemList.put(itemid, droprate);
		} else {
			itemList.remove(itemid);
			itemList.put(itemid, droprate);
		}
	}

	public int getItemDropRate(int itemid) {
		if (itemList.containsKey(itemid))
			return itemList.get(itemid);
		return 0;
	}
}
