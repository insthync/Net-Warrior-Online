package ConfigDraft;

import Config.MonsterConfig;
import Config.DropConfig;

public class MonsterDraft {
	private MonsterConfig cfg = null;
	private DropConfig dropcfg = null;
	private short mapid = 0;
	private float x = 0;
	private float y = 0;
	public MonsterDraft(MonsterConfig cfg, DropConfig dropcfg, short mapid, float x, float y) {
		super();
		this.cfg = cfg;
		this.dropcfg = dropcfg;
		this.mapid = mapid;
		this.x = x;
		this.y = y;
	}
	public MonsterConfig getCfg() {
		return cfg;
	}
	public DropConfig getDropCfg() {
		return dropcfg;
	}
	public short getMapid() {
		return mapid;
	}
	public float getX() {
		return x;
	}
	public float getY() {
		return y;
	}
	
}
