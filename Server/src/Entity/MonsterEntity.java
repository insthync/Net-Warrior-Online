package Entity;

import Config.Configuration;
import Config.DropConfig;
import Config.MonsterConfig;
import Server.GameHandler;
import Utils.*;

public class MonsterEntity extends UnitEntity {
	private int id = 0;
	private long reborntime = 0;
	private long relocatetime = 0;
	private short rebornmap = 0;
	private float rebornmap_x = 0.0f;
	private float rebornmap_y = 0.0f;
	private int movecycle = 0;
	private final long hidden_after_die = 3000;
	private DropConfig dropcfg = null;
	private boolean isdropped = false;
	public MonsterEntity(int id, String name, short charclass, short level,
			int exp,
			short strength, short dexterity, short intelligent,
			short model_hair, short model_face, long reborntime, short currentmap,
			float currentmap_x, float currentmap_y, Configuration cfg, GameHandler game) {
		super(name, charclass, level, exp,
				strength, dexterity, intelligent, model_hair, model_face, 
				currentmap, currentmap_x, currentmap_y, cfg, game);
		this.id = id;
		this.reborntime = reborntime;
		this.relocatetime = System.currentTimeMillis();
		this.rebornmap = currentmap;
		this.rebornmap_x = currentmap_x;
		this.rebornmap_y = currentmap_y;
		this.movecycle = aRandom.doRandom(1000, 10000);
	}
	public MonsterEntity(int id, MonsterConfig cfg, DropConfig dropcfg, short currentmap,
			float currentmap_x, float currentmap_y, Configuration maincfg, GameHandler game) {
		this(id, cfg.name, cfg.charclass, cfg.level, cfg.exp,
				cfg.strength, cfg.dexterity, cfg.intelligent, cfg.model_hair, cfg.model_face, cfg.reborntime,
				currentmap, currentmap_x, currentmap_y, maincfg, game);
		this.dropcfg = dropcfg;
	}
	public int getid() {
		return id;
	}
	private void doAttack() {
		// Attack player
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
		if (state == state_die) {
			if (!isdropped) {
				if (dropcfg != null) {
					game.monsterDoDrop(dropcfg, currentmap, currentmap_x, currentmap_y);
				}
				isdropped = true;
			}
			if (System.currentTimeMillis() - dietime >= hidden_after_die) {
				visible = false;
			}
			if (System.currentTimeMillis() - dietime >= reborntime) {
				curHP = maxHP;
				curSP = maxSP;
				currentmap = rebornmap;
				currentmap_x = rebornmap_x;
				currentmap_y = rebornmap_y;
				state = state_idle;
				visible = true;
				isdropped = false;
			}
		} else if (state == state_idle) {
			if (target_node == null && System.currentTimeMillis() - relocatetime >= movecycle) {
				target_x = currentmap_x - aRandom.doRandom(-16, 16);
				target_y = currentmap_y - aRandom.doRandom(-16, 16);
				//System.out.println(target_x + " " + target_y);
				movecycle = aRandom.doRandom(1000, 10000);
				relocatetime = System.currentTimeMillis();
			}
		}
	}
	@Override
	public void active() {
		// TODO Auto-generated method stub
		super.active();
		if (target_node != null) {
			if (target_node.getEntityType().equals(PlayerEntity.entityType())) {
				doAttack();
			}
		}
	}
	public long getReborntime() {
		return reborntime;
	}
	public void setReborntime(long reborntime) {
		this.reborntime = reborntime;
	}
	public String getEntityType() {
		return "Monster";
	}
	public static String entityType() {
		return "Monster";
	}
	@Override
	public int receiveDamage(UnitEntity target) {
		// TODO Auto-generated method stub
		int totalDmg = super.receiveDamage(target);
		target_node = target;
		if (target.getEntityType() == PlayerEntity.entityType()) {
			float percentDmg = (float)totalDmg / (float)maxHP * 100.0f; // got percent
			float exp = (percentDmg / 100) * this.exp;
			//System.out.println(percentDmg + " " + exp);
			exp = (exp < 1) ? 1 : exp;
			((PlayerEntity)target).receiveExp((int)exp);
		}
		return totalDmg;
	}
}
