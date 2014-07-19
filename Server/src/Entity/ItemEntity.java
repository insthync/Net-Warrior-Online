package Entity;

import Config.Configuration;

public class ItemEntity extends NodeEntity {
	private int id = 0;
	private int itemid = 0;
	private long dropwhen = 0;
	private short amount = 0;
	private short equip = 0;
	private short refine = 0;
	private int attributeid = 0;
	private int slot1 = 0;
	private int slot2 = 0;
	private int slot3 = 0;
	private int slot4 = 0;
	
	public ItemEntity(int id, int itemid, short currentmap, float currentmap_x, float currentmap_y,	Configuration maincfg) {
		super(currentmap, currentmap_x, currentmap_y, maincfg);
		// TODO Auto-generated constructor stub
		this.id = id;
		this.itemid = itemid;
		this.dropwhen = System.currentTimeMillis();
	}
	public int getid() {
		return id;
	}
	public int getItemid() {
		return itemid;
	}
	public long dropWhen() {
		return dropwhen;
	}
	public String getEntityType() {
		return "Item";
	}
	public static String entityType() {
		return "Item";
	}
	public short getAmount() {
		return amount;
	}
	public short getEquip() {
		return equip;
	}
	public short getRefine() {
		return refine;
	}
	public int getAttributeid() {
		return attributeid;
	}
	public int getSlot1() {
		return slot1;
	}
	public int getSlot2() {
		return slot2;
	}
	public int getSlot3() {
		return slot3;
	}
	public int getSlot4() {
		return slot4;
	}

}
