package Entity;

import java.util.ArrayList;

import Config.Configuration;
import Config.ExpConfig;
import Server.Client;
import Server.GameHandler;
import Utils.aRandom;

public class PlayerEntity extends UnitEntity {
	private int id = 0;
	private int stpoint;
	private int skpoint;
	private short savemap;
	private float savemap_x;
	private float savemap_y;
	private int gold = 0;
	private Client client = null;
	public PlayerEntity(int id, String name, short charclass, short level,
			int exp, int gold, int curHP, int curSP,
			int stpoint, int skpoint, short strength, short dexterity, short intelligent,
			short model_hair, short model_face,
			short currentmap, float currentmap_x, float currentmap_y,
			short savemap, float savemap_x, float savemap_y, Client client, Configuration cfg, GameHandler game) {
		super(name, charclass, level, exp,
				strength, dexterity, intelligent, model_hair,
				model_face, currentmap, currentmap_x, currentmap_y, cfg, game);
		this.id = id;
		this.stpoint = stpoint;
		this.skpoint = skpoint;
		this.savemap = savemap;
		this.savemap_x = savemap_x;
		this.savemap_y = savemap_y;
		this.gold = gold;
		this.curHP = curHP;
		this.curSP = curSP;
		this.client = client;
	}
	public int getid() {
		return id;
	}
	public int getStpoint() {
		return stpoint;
	}
	public void setStpoint(int stpoint) {
		this.stpoint = stpoint;
	}
	public int getSkpoint() {
		return skpoint;
	}
	public void setSkpoint(int skpoint) {
		this.skpoint = skpoint;
	}
	public short getSavemap() {
		return savemap;
	}
	public void setSavemap(short savemap) {
		this.savemap = savemap;
	}
	public float getSavemap_x() {
		return savemap_x;
	}
	public void setSavemap_x(float savemap_x) {
		this.savemap_x = savemap_x;
	}
	public float getSavemap_y() {
		return savemap_y;
	}
	public void setSavemap_y(float savemap_y) {
		this.savemap_y = savemap_y;
	}
	public int getGold() {
		return gold;
	}
	public void setGold(int gold) {
		this.gold = gold;
	}
	private void doAttack() {
		if (System.currentTimeMillis() - activetime >= 750 - atkspd * 10) {
			if (state != state_attack1 && state != state_attack2 && state != state_attack3)
				state = (short)aRandom.doRandom(state_attack1, state_attack3);
			if (System.currentTimeMillis() - activetime >= 1000 - atkspd * 10) {
				if (target_node != null) {
					if (target_node.getEntityType().equals(MonsterEntity.entityType()))
						((MonsterEntity)target_node).receiveDamage(this);
					if (target_node.getEntityType().equals(PlayerEntity.entityType()))
						((PlayerEntity)target_node).receiveDamage(this);
				}
				activetime = System.currentTimeMillis();
				activated = false;
			}
		} else {
			state = state_attackidle;
		}
	}
	@Override
	public void activities() {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void active() {
		// TODO Auto-generated method stub
		super.active();
		if (target_node != null) {
			if (target_node.getEntityType().equals(MonsterEntity.entityType())) {
				// Attack target
				doAttack();
				return;
			}
			if (target_node.getEntityType().equals(NPCEntity.entityType())) {
				// Talking with target
	            state = state_idle;
	            if (!client.requestedNpc && target_node != null)
	            	((NPCEntity)target_node).response(this);
				return;
			}
			if (target_node.getEntityType().equals(PlayerEntity.entityType())) {
				// Do nothing
	            state = state_idle;
				return;
			}
			if (target_node.getEntityType().equals(WarpEntity.entityType())) {
				// Warping
	            state = state_idle;
	            this.setCurrentmap(((WarpEntity)target_node).getTomap());
	            this.setCurrentmap_x(((WarpEntity)target_node).getTomap_x());
	            this.setCurrentmap_y(((WarpEntity)target_node).getTomap_y());
	            this.target_node = null;
	            this.target_x = -1;
	            this.target_y = -1;
				return;
			}
		}
	}
	public void update(ArrayList<Client> clientList) {
		super.update(clientList);
		ExpConfig expcfg = this.cfg.getExpsConfig().get(this.charclass);
		if (expcfg != null && this.exp >= expcfg.getExp(level) && level < expcfg.getMaxLv()) {
			++level;
			stpoint += 3;
			UnitLevelThread lvThread = new UnitLevelThread(this, this.clientList);
			lvThread.start();
		}
	}
	public String getEntityType() {
		return "Player";
	}
	public static String entityType() {
		return "Player";
	}
	public Client getClient() {
		return client;
	}
	public void receiveExp(int exp) {
		this.exp += exp;
	}
}
