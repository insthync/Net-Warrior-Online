package Entity;

import Config.*;

public class NodeEntity {
	protected short currentmap = 0;
	protected float currentmap_x = 0;
	protected float currentmap_y = 0;
	protected Configuration cfg = null;
	protected boolean visible = true;
	public NodeEntity(short currentmap, float currentmap_x, float currentmap_y, Configuration cfg) {
		this.currentmap = currentmap;
		this.currentmap_x = currentmap_x;
		this.currentmap_y = currentmap_y;
		this.cfg = cfg;
	}
	public boolean isVisible() {
		return visible;
	}
	public short getCurrentmap() {
		return currentmap;
	}
	public void setCurrentmap(short currentmap) {
		this.currentmap = currentmap;
	}
	public float getCurrentmap_x() {
		return currentmap_x;
	}
	public void setCurrentmap_x(float currentmap_x) {
		this.currentmap_x = currentmap_x;
	}
	public float getCurrentmap_y() {
		return currentmap_y;
	}
	public void setCurrentmap_y(float currentmap_y) {
		this.currentmap_y = currentmap_y;
	}
	public String getEntityType() {
		return "none";
	}
	public static String entityType() {
		return "none";
	}
}
