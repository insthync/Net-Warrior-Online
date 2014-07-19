package Entity;

import Config.Configuration;
import Config.WarpConfig;

public class WarpEntity extends NodeEntity {
	private int id = 0;
	private short tomap = 0;
	private float tomap_x = 0;
	private float tomap_y = 0;
	public WarpEntity(int id, short tomap, float tomap_x, float tomap_y, short currentmap, float currentmap_x, float currentmap_y, Configuration cfg) {
		super(currentmap, currentmap_x, currentmap_y, cfg);
		this.id = id;
		this.tomap = tomap;
		this.tomap_x = tomap_x;
		this.tomap_y = tomap_y;
	}
	public WarpEntity(int id, WarpConfig cfg, short currentmap, float currentmap_x, float currentmap_y, Configuration maincfg) {
		this(id, cfg.tomap, cfg.tomap_x, cfg.tomap_y,currentmap, currentmap_x, currentmap_y, maincfg);
	}
	public int getid() {
		return id;
	}
	public void setid(int id) {
		this.id = id;
	}
	public short getTomap() {
		return tomap;
	}
	public void setTomap(short tomap) {
		this.tomap = tomap;
	}
	public float getTomap_x() {
		return tomap_x;
	}
	public void setTomap_x(float tomap_x) {
		this.tomap_x = tomap_x;
	}
	public float getTomap_y() {
		return tomap_y;
	}
	public void setTomap_y(float tomap_y) {
		this.tomap_y = tomap_y;
	}
	public String getEntityType() {
		return "Warp";
	}
	public static String entityType() {
		return "Warp";
	}
}
