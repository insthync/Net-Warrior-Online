package Entity;

import java.awt.geom.Point2D;
import java.util.ArrayList;
import java.util.HashMap;

import Config.*;
import Server.Client;
import Server.GameHandler;
import Server.GlobalVariable;
import Utils.*;

public abstract class UnitEntity extends NodeEntity {
	public static final short 
		state_idle = 0,
		state_walk = 1,
		state_run = 2,
		state_attackidle = 3,
	    state_attack1 = 4,
	    state_attack2 = 5,
	    state_attack3 = 6,
	    state_die = 7;
	public static final short
		itemidx_head = 2,
		itemidx_body = 4,
		itemidx_hand = 8,
		itemidx_foot = 16,
		itemidx_weaponR = 32,
		itemidx_weaponL = 64;
	protected String name = "";
	protected short charclass = 0;
	protected short level = 0;
	protected int exp = 0;
	protected int maxHP = 0;
	protected int curHP = 0;
	protected int maxSP = 0;
	protected int curSP = 0;
	protected short strength = 0;
	protected short dexterity = 0;
	protected short intelligent = 0;
	protected short model_hair = 0;
	protected short model_face = 0;
	protected float rotation = 0;
	protected short state = 0;
	protected float target_x = -1;
	protected float target_y = -1;
	protected NodeEntity target_node = null;
	protected NodeEntity nearest_node = null;
	protected float movespd = 0.1f;
	protected int atkspd = 1;
	protected int min_patk = 1;
	protected int max_patk = 2;
	protected int min_matk = 1;
	protected int max_matk = 2;
	protected int pdef = 2;
	protected int mdef = 2;
	protected long dietime = 0;
	protected ClassConfig classcfg = null;
	protected boolean activated = false;
	protected long activetime = 0;
	protected long recoveryhptime = 0;
	protected long recoverysptime = 0;
	protected ArrayList<Client> clientList;
	protected GameHandler game = null;
	// Equipment System
	protected HashMap<Integer, EquipmentEntity> equipmentList = null;
	protected int bonus_maxHP = 0;
	protected int bonus_maxSP = 0;
	protected int bonus_atkspd = 0;
	protected float bonus_movespd = 0;
	protected int bonus_min_patk = 0;
	protected int bonus_max_patk = 0;
	protected int bonus_min_matk = 0;
	protected int bonus_max_matk = 0;
	protected int bonus_pdef = 0;
	protected int bonus_mdef = 0;
	protected short bonus_strength = 0;
	protected short bonus_dexterity = 0;
	protected short bonus_intelligent = 0;
	public UnitEntity(String name,
			short charclass, short level, int exp,
			short strength, short dexterity, short intelligent,
			short model_hair, short model_face, short currentmap,
			float currentmap_x, float currentmap_y, Configuration cfg, GameHandler game) {
		super(currentmap, currentmap_x, currentmap_y, cfg);
		this.game = game;
		if (cfg != null)
			this.classcfg = cfg.getClassesConfig().get(charclass);
		this.name = name;
		this.charclass = charclass;
		this.level = level;
		this.exp = exp;
		this.strength = strength;
		this.dexterity = dexterity;
		this.intelligent = intelligent;
		this.model_hair = model_hair;
		this.model_face = model_face;
		this.currentmap = currentmap;
		this.currentmap_x = currentmap_x;
		this.currentmap_y = currentmap_y;
		this.rotation = aRandom.doRandom(0, 360);
		curHP = maxHP = ClassConfig.hpMultiplier(level, strength, dexterity, intelligent, classcfg); 
		curSP = maxSP = ClassConfig.spMultiplier(level, strength, dexterity, intelligent, classcfg);
		// Equipment System
		this.equipmentList = new HashMap<Integer, EquipmentEntity>();
	}
	public float getRotation() {
		return rotation;
	}
	public void setRotation(short rotation) {
		this.rotation = rotation;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public short getCharclass() {
		return charclass;
	}
	public void setCharclass(short charclass) {
		this.charclass = charclass;
	}
	public short getLevel() {
		return level;
	}
	public void setLevel(short level) {
		this.level = level;
	}
	public int getExp() {
		return exp;
	}
	public void setExp(int exp) {
		this.exp = exp;
	}
	public int getMaxHP() {
		return maxHP;
	}
	public void setMaxHP(int maxHP) {
		this.maxHP = maxHP;
	}
	public int getCurHP() {
		return curHP;
	}
	public void setCurHP(int curHP) {
		this.curHP = curHP;
	}
	public int getMaxSP() {
		return maxSP;
	}
	public void setMaxSP(int maxSP) {
		this.maxSP = maxSP;
	}
	public int getCurSP() {
		return curSP;
	}
	public void setCurSP(int curSP) {
		this.curSP = curSP;
	}
	public short getStrength() {
		return strength;
	}
	public void setStrength(short strength) {
		this.strength = strength;
	}
	public short getDexterity() {
		return dexterity;
	}
	public void setDexterity(short dexterity) {
		this.dexterity = dexterity;
	}
	public short getIntelligent() {
		return intelligent;
	}
	public void setIntelligent(short intelligent) {
		this.intelligent = intelligent;
	}
	public short getModel_hair() {
		return model_hair;
	}
	public void setModel_hair(short model_hair) {
		this.model_hair = model_hair;
	}
	public short getModel_face() {
		return model_face;
	}
	public void setModel_face(short model_face) {
		this.model_face = model_face;
	}
	public short getState() {
		return state;
	}
	public void setState(short state) {
		this.state = state;
	}
	public void setTarget(float x, float y) {
		if (this.target_x != x && this.target_y != y) {
			this.target_x = x;
			this.target_y = y;
			this.target_node = null;
			this.activated = false;
		}
		//System.out.println("(" + target_x + "," + target_y + "); " + target_node);
	}
	public float getTargetX() {
		return target_x;
	}
	public float getTargetY() {
		return target_y;
	}
	public void setNodeTarget(NodeEntity node) {
		if (this.target_node == null || !this.target_node.equals(node)) {
			this.target_node = node;
			this.target_x = -1;
			this.target_y = -1;
			this.activated = false;
		}
		//System.out.println("(" + target_x + "," + target_y + "); " + target_node);
	}
	public void setNearestNode(NodeEntity node) {
		this.nearest_node = node;
	}
	public void update(ArrayList<Client> clientList) {
		if (this.clientList == null)
			this.clientList = clientList;
		// Character setting
		movespd = ClassConfig.movespd(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg) * 0.0001f;
		atkspd = ClassConfig.atkspd(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg);
		min_patk = ClassConfig.patkMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg)[0];
		max_patk = ClassConfig.patkMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg)[1];
		min_matk = ClassConfig.matkMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg)[0];
		max_matk = ClassConfig.matkMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg)[1];
		pdef = ClassConfig.pdefMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg);
		mdef = ClassConfig.mdefMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg);
		maxHP = ClassConfig.hpMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg); 
		maxSP = ClassConfig.spMultiplier(level, (short)(strength + bonus_strength), (short)(dexterity + bonus_dexterity), (short)(intelligent + bonus_intelligent), classcfg);
		
		// Equipment System
		movespd += bonus_movespd;
		atkspd += bonus_atkspd;
		min_patk += bonus_min_patk;
		max_patk += bonus_max_patk;
		min_matk += bonus_min_matk;
		max_matk += bonus_max_matk;
		pdef += bonus_pdef;
		mdef += bonus_mdef;
		maxHP += bonus_maxHP;
		maxSP += bonus_maxSP;
		
		// Their activities
		activities();
		if (curHP > 0 && curHP < maxHP) {
			if (System.currentTimeMillis() - recoveryhptime >= 3000) {
				curHP += (maxHP * 2 / 100);
				recoveryhptime = System.currentTimeMillis();
			}
		}
		if (curSP > 0 && curSP < maxSP) {
			if (System.currentTimeMillis() - recoverysptime >= 3000) {
				curSP += (maxSP * 2 / 100);
				recoverysptime = System.currentTimeMillis();
			}
		}
		if (curHP <= 0) {
			die();
			return;
		}
		if (curSP <= 0) {
			curSP = 0;
		}
		if (curHP >= maxHP) {
			curHP = maxHP;
		}
		if (curSP >= maxSP) {
			curSP = maxSP;
		}
		// Moving
		Point2D currentPos = null;
		Point2D targetPos = null;
		// Map setting
		MapConfig currentMap = cfg.getMapsConfig().get(currentmap);
		if (currentMap != null) {
			int mapsize = currentMap.path.length;
			if (currentmap_x < 0 || currentmap_y > mapsize) {
				currentmap_x = aRandom.doRandom(2, mapsize - 2);
			}
			if (currentmap_y < 0 || currentmap_y > mapsize) {
				currentmap_y = aRandom.doRandom(2, mapsize - 2);
			}
			while (currentMap.path[(int)currentmap_y][(int)currentmap_x] == 1) {
				currentmap_x = aRandom.doRandom(2, mapsize - 2);
				currentmap_y = aRandom.doRandom(2, mapsize - 2);
			}
			if (target_node != null) {
				boolean targetIsAvailable = false;
				if (target_node.getEntityType().equals(WarpEntity.entityType())) {
					targetIsAvailable = true;
				} else {
					if (((UnitEntity)target_node).getState() != state_die) 
						targetIsAvailable = true;
					else {
						target_node = null;
						target_x = -1;
						target_y = -1;
					}
				}
				if (targetIsAvailable) {
					currentPos = new Point2D.Float(currentmap_x, currentmap_y);
					targetPos = new Point2D.Float(target_node.getCurrentmap_x(), target_node.getCurrentmap_y());
					rotation = (float)(((Math.atan2(targetPos.getX() - currentPos.getX(), targetPos.getY() - currentPos.getY())*180)/3.14)-90);
					if (currentPos.distance(targetPos) > GlobalVariable.activing_distance  && !activated) {
						int path_x = (int)Math.floor(currentmap_x + (movespd * Math.cos((rotation) * 3.14f / 180.0f)));
						int path_y = (int)Math.floor(currentmap_y - (movespd * Math.sin((rotation) * 3.14f / 180.0f)));
						if (path_x >= 0 && path_x < mapsize && path_y >= 0 && path_y < mapsize && currentMap.path[path_y][path_x] == 0) {
							currentmap_x += (movespd * Math.cos((rotation) * 3.14f / 180.0f));
				            currentmap_y -= (movespd * Math.sin((rotation) * 3.14f / 180.0f));
				            state = state_run;
						} else {
				            state = state_idle;
							target_node = null;
							target_x = -1;
							target_y = -1;
						}
						activated = false;
					} else {
			            active();
					}
				}
			} else {
				target_node = null;
				if (target_x >= 0 && target_y >= 0
					&& target_x < mapsize && target_y < mapsize) {
					currentPos = new Point2D.Float(currentmap_x, currentmap_y);
					targetPos = new Point2D.Float(target_x, target_y);
					rotation = (float)(((Math.atan2(targetPos.getX() - currentPos.getX(), targetPos.getY() - currentPos.getY())*180)/3.14)-90);
					if (currentPos.distance(targetPos) > 1) {
						int path_x = (int)Math.floor(currentmap_x + (movespd * Math.cos((rotation) * 3.14f / 180.0f)));
						int path_y = (int)Math.floor(currentmap_y - (movespd * Math.sin((rotation) * 3.14f / 180.0f)));
						if (path_x >= 0 && path_x < mapsize && path_y >= 0 && path_y < mapsize
								&& currentMap != null && currentMap.path != null && currentMap.path[path_y][path_x] == 0)
						{
							currentmap_x += (movespd * Math.cos((rotation) * 3.14f / 180.0f));
				            currentmap_y -= (movespd * Math.sin((rotation) * 3.14f / 180.0f));
				            state = state_run;
						} else {
				            state = state_idle;
							target_node = null;
							target_x = -1;
							target_y = -1;
						}
					} else {
						target_x = -1;
						target_y = -1;
			            state = state_idle;
					}
				} else {
					target_x = -1;
					target_y = -1;
		            state = state_idle;
				}
			}
			if (nearest_node != null) {
				if (nearest_node.getEntityType().equals(MonsterEntity.entityType()) || nearest_node.getEntityType().equals(PlayerEntity.entityType())) {
					if (((UnitEntity)nearest_node).getState() == state_die) {
						nearest_node = null;
					}
				}
			}
		}
	}
	public void die() {
		curHP = 0;
		target_node = null;
		target_x = -1;
		target_y = -1;
		nearest_node = null;
		if (state != state_die) {
			state = state_die;
			dietime = System.currentTimeMillis();
		}
	}
	public abstract void activities();
	public void active() {
		if (!activated) {
			activetime = System.currentTimeMillis();
			activated = true;
		}
	}
	public int receiveDamage(UnitEntity target) {
		// Physic
		int recpDmg = target.randomDamage() - pdef;
		if (recpDmg < 0) {
			recpDmg = 0;
		}
		// Magic
		int recmDmg = target.randomMagicDamage() - mdef;
		if (recmDmg < 0) {
			recmDmg = 0;
		}
		int totalDmg = recpDmg + recmDmg;
		curHP -= totalDmg;
		UnitDamageThread dmgThread = new UnitDamageThread(this, this.clientList, totalDmg);
		dmgThread.start();
		return totalDmg;
	}
	public int randomDamage() {
		return aRandom.doRandom(min_patk, max_patk);
	}
	public int randomMagicDamage() {
		return aRandom.doRandom(min_matk, max_matk);
	}
	public int distanceFrom(NodeEntity target) {
		Point2D currentPos = new Point2D.Float(currentmap_x, currentmap_y);
		Point2D targetPos = new Point2D.Float(target.getCurrentmap_x(), target.getCurrentmap_y());
		if (currentmap == target.getCurrentmap()) {
			return (int)currentPos.distance(targetPos);
		}
		return Integer.MAX_VALUE;
	}
	public void setNoTarget() {
		target_node = null;
		target_x = -1;
		target_y = -1;
	}
	// Equipment System
	public boolean useItem(ItemConfig itemcfg) {
		if ((itemcfg.charclass == 0 || itemcfg.charclass == this.charclass)
				&& itemcfg.level <= this.level
				&& itemcfg.strength <= this.strength && itemcfg.dexterity <= this.dexterity && itemcfg.intelligent <= this.intelligent)
		{
			
			return true;
		}
		return false;
	}
	public boolean unEquipItem(int equip) {
		if (equipmentList.containsKey(equip)) {
			equipmentList.remove(equip);
			// Update Bonus
			int bonus_maxHP = 0;
			int bonus_maxSP = 0;
			int bonus_atkspd = 0;
			float bonus_movespd = 0;
			int bonus_min_patk = 0;
			int bonus_max_patk = 0;
			int bonus_min_matk = 0;
			int bonus_max_matk = 0;
			int bonus_pdef = 0;
			int bonus_mdef = 0;
			short bonus_strength = 0;
			short bonus_dexterity = 0;
			short bonus_intelligent = 0;
			for (int _equip : equipmentList.keySet()) {
				if (_equip == itemidx_head || _equip == itemidx_body || _equip == itemidx_foot || _equip == itemidx_hand || _equip == itemidx_weaponL || _equip == itemidx_weaponR) {
					EquipmentEntity equipEnt = equipmentList.get(_equip);
					bonus_maxHP += equipEnt.getMaxHPBonus();
					bonus_maxSP += equipEnt.getMaxSPBonus();
					bonus_atkspd += equipEnt.getATKSPDBonus();
					bonus_movespd += equipEnt.getMoveSPDBonus();
					bonus_min_patk += equipEnt.getMinPATKBonus();
					bonus_max_patk += equipEnt.getMaxPATKBonus();
					bonus_min_matk += equipEnt.getMinMATKBonus();
					bonus_max_matk += equipEnt.getMaxMATKBonus();
					bonus_pdef += equipEnt.getPDEFBonus();
					bonus_mdef += equipEnt.getMDEFBonus();
					bonus_strength += equipEnt.getStrength();
					bonus_dexterity += equipEnt.getDexterity();
					bonus_intelligent += equipEnt.getIntelligent();
				}
			}
			this.bonus_maxHP = bonus_maxHP;
			this.bonus_maxSP = bonus_maxSP;
			this.bonus_atkspd = bonus_atkspd;
			this.bonus_movespd = bonus_movespd;
			this.bonus_min_patk = bonus_min_patk;
			this.bonus_max_patk = bonus_max_patk;
			this.bonus_min_matk = bonus_min_matk;
			this.bonus_max_matk = bonus_max_matk;
			this.bonus_pdef = bonus_pdef;
			this.bonus_mdef = bonus_mdef;
			this.bonus_strength = bonus_strength;
			this.bonus_dexterity = bonus_dexterity;
			this.bonus_intelligent = bonus_intelligent;
			return true;
		}
		return false;
	}
	public boolean equipItem(ItemConfig itemcfg, short refine, int attributeid, int slot1, int slot2, int slot3, int slot4) {
		int equip = itemcfg.type;
		if (itemcfg != null && (itemcfg.charclass == 0 || itemcfg.charclass == this.charclass)
				&& itemcfg.level <= this.level
				&& itemcfg.strength <= this.strength && itemcfg.dexterity <= this.dexterity && itemcfg.intelligent <= this.intelligent)
		{

			EquipmentEntity value = new EquipmentEntity(itemcfg, refine, attributeid, slot1, slot2, slot3, slot4);
			if (equipmentList.containsKey(equip)) {
				equipmentList.remove(equip);
				equipmentList.put(equip, value);
			} else {
				equipmentList.put(equip, value);
			}
			// Update Bonus
			int bonus_maxHP = 0;
			int bonus_maxSP = 0;
			int bonus_atkspd = 0;
			float bonus_movespd = 0;
			int bonus_min_patk = 0;
			int bonus_max_patk = 0;
			int bonus_min_matk = 0;
			int bonus_max_matk = 0;
			int bonus_pdef = 0;
			int bonus_mdef = 0;
			short bonus_strength = 0;
			short bonus_dexterity = 0;
			short bonus_intelligent = 0;
			for (int _equip : equipmentList.keySet()) {
				if (_equip == itemidx_head || _equip == itemidx_body || _equip == itemidx_foot || _equip == itemidx_hand || _equip == itemidx_weaponL || _equip == itemidx_weaponR) {
					EquipmentEntity equipEnt = equipmentList.get(_equip);
					bonus_maxHP += equipEnt.getMaxHPBonus();
					bonus_maxSP += equipEnt.getMaxSPBonus();
					bonus_atkspd += equipEnt.getATKSPDBonus();
					bonus_movespd += equipEnt.getMoveSPDBonus();
					bonus_min_patk += equipEnt.getMinPATKBonus();
					bonus_max_patk += equipEnt.getMaxPATKBonus();
					bonus_min_matk += equipEnt.getMinMATKBonus();
					bonus_max_matk += equipEnt.getMaxMATKBonus();
					bonus_pdef += equipEnt.getPDEFBonus();
					bonus_mdef += equipEnt.getMDEFBonus();
					bonus_strength += equipEnt.getStrength();
					bonus_dexterity += equipEnt.getDexterity();
					bonus_intelligent += equipEnt.getIntelligent();
				}
			}
			this.bonus_maxHP = bonus_maxHP;
			this.bonus_maxSP = bonus_maxSP;
			this.bonus_atkspd = bonus_atkspd;
			this.bonus_movespd = bonus_movespd;
			this.bonus_min_patk = bonus_min_patk;
			this.bonus_max_patk = bonus_max_patk;
			this.bonus_min_matk = bonus_min_matk;
			this.bonus_max_matk = bonus_max_matk;
			this.bonus_pdef = bonus_pdef;
			this.bonus_mdef = bonus_mdef;
			this.bonus_strength = bonus_strength;
			this.bonus_dexterity = bonus_dexterity;
			this.bonus_intelligent = bonus_intelligent;
			
			return true;
		}
		return false;
	}
	public int getEquipmentItemID(int equip) {
		if (this.equipmentList.containsKey(equip)) {
			return this.equipmentList.get(equip).getItemID();
		}
		return 0;
	}
}
