package Config;

public class WarpConfig {
	public int id = 0;
	public WarpConfig(int id, Short tomap, Float tomap_x, Float tomap_y) {
		// TODO Auto-generated constructor stub
		this.id = id;
		this.tomap = tomap;
		this.tomap_x = tomap_x;
		this.tomap_y = tomap_y;
	}
	public short tomap = 0;
	public float tomap_x = -1;
	public float tomap_y = -1;
}
