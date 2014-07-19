package Config;

public class MapConfig {
	public Integer[][] path; // x, y
	public MapConfig(Integer[][] path) {
		this.path = path;
	}
	public String toString() {
		String str = "";
		for (int j = 0; j < path.length; ++j) {
			for (int i = 0; i < path[j].length; i++) {
				str += path[j][i];
			}
			str += "\n";
		}
		return str;
	}
}
