package ConfigDraft;

import Config.WarpConfig;

public class WarpDraft {
	private WarpConfig cfg = null;
	private short mapid = 0;
	private float x = 0;
	private float y = 0;
	public WarpDraft(WarpConfig cfg, short mapid, float x, float y) {
		super();
		this.cfg = cfg;
		this.mapid = mapid;
		this.x = x;
		this.y = y;
	}
	public WarpConfig getCfg() {
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
