package Entity;

import Config.ItemConfig;

public class EquipmentEntity {
	private ItemConfig itemcfg = null;
	private short refine = 0;
	private int attributeid = 0;
	private int slot1 = 0;
	private int slot2 = 0;
	private int slot3 = 0;
	private int slot4 = 0;
	
	public EquipmentEntity(ItemConfig itemcfg, short refine, int attributeid,
			int slot1, int slot2, int slot3, int slot4) {
		super();
		this.itemcfg = itemcfg;
		this.refine = refine;
		this.attributeid = attributeid;
		this.slot1 = slot1;
		this.slot2 = slot2;
		this.slot3 = slot3;
		this.slot4 = slot4;
	}
	public int getItemID() {
		return itemcfg.id;
	}
	public int getStrength() {
		return itemcfg.strength;
	}
	public int getDexterity() {
		return itemcfg.dexterity;
	}
	public int getIntelligent() {
		return itemcfg.intelligent;
	}
	public int getMaxHPBonus() {
		return itemcfg.add_maxhp;
	}
	public int getMaxSPBonus() {
		return itemcfg.add_maxsp;
	}
	public int getMaxPATKBonus() {
		return itemcfg.add_maxpatk;
	}
	public int getMaxMATKBonus() {
		return itemcfg.add_maxmatk;
	}
	public int getMinPATKBonus() {
		return itemcfg.add_minpatk;
	}
	public int getMinMATKBonus() {
		return itemcfg.add_minmatk;
	}
	public int getPDEFBonus() {
		return itemcfg.add_pdef;
	}
	public int getMDEFBonus() {
		return itemcfg.add_mdef;
	}
	public int getMoveSPDBonus() {
		return itemcfg.add_movespd;
	}
	public int getATKSPDBonus() {
		return itemcfg.add_atkspd;
	}
	public short getRefine() {
		return refine;
	}
	public void setRefine(short refine) {
		this.refine = refine;
	}
	public int getAttributeid() {
		return attributeid;
	}
	public void setAttributeid(int attributeid) {
		this.attributeid = attributeid;
	}
	public int getSlot1() {
		return slot1;
	}
	public void setSlot1(int slot1) {
		this.slot1 = slot1;
	}
	public int getSlot2() {
		return slot2;
	}
	public void setSlot2(int slot2) {
		this.slot2 = slot2;
	}
	public int getSlot3() {
		return slot3;
	}
	public void setSlot3(int slot3) {
		this.slot3 = slot3;
	}
	public int getSlot4() {
		return slot4;
	}
	public void setSlot4(int slot4) {
		this.slot4 = slot4;
	}
	public ItemConfig getItemcfg() {
		return itemcfg;
	}
	
}
