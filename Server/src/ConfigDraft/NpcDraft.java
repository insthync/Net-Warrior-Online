package ConfigDraft;

import Config.NpcConfig;

public class NpcDraft {
	private NpcConfig cfg = null;
	private short mapid = 0;
	private float x = 0;
	private float y = 0;
	public NpcDraft(NpcConfig cfg, short mapid, float x, float y) {
		super();
		this.cfg = cfg;
		this.mapid = mapid;
		this.x = x;
		this.y = y;
	}
	public NpcConfig getCfg() {
		return cfg;
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
