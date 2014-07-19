package Config;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Scanner;

import Utils.cNumber;

import ConfigDraft.*;

public class Configuration {
	private final String start_cfgfile = "conf/server.conf";
	private ServerConfig serverCfg = new ServerConfig();
	private PlayerConfig playerCfg = new PlayerConfig();
	// Entity
	private ArrayList<MonsterDraft> monstersDraft;
	private ArrayList<NpcDraft> npcsDraft;
	private ArrayList<WarpDraft> warpsDraft;
	// Config
	private HashMap<Short, MapConfig> mapsConfig;
	private HashMap<Short, ClassConfig> classesConfig;
	private HashMap<Short, ExpConfig> expsConfig;
	private HashMap<Integer, MonsterConfig> monstersConfig;
	private HashMap<Integer, NpcConfig> npcsConfig;
	private HashMap<Integer, WarpConfig> warpsConfig;
	private HashMap<Integer, ItemConfig> itemsConfig;
	private HashMap<Integer, DropConfig> dropsConfig;
	public Configuration() {
		// Entity
		monstersDraft = new ArrayList<MonsterDraft>();
		npcsDraft = new ArrayList<NpcDraft>();
		warpsDraft = new ArrayList<WarpDraft>();
		// Config
		mapsConfig = new HashMap<Short, MapConfig>();
		classesConfig = new HashMap<Short, ClassConfig>();
		expsConfig = new HashMap<Short, ExpConfig>();
		monstersConfig = new HashMap<Integer, MonsterConfig>();
		npcsConfig = new HashMap<Integer, NpcConfig>();
		warpsConfig = new HashMap<Integer, WarpConfig>();
		itemsConfig = new HashMap<Integer, ItemConfig>();
		dropsConfig = new HashMap<Integer, DropConfig>();
		// File Reader
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(start_cfgfile));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + start_cfgfile + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				readServerConfig(scanner.nextLine());
			}
			scanner.close();
		}
		System.out.println("Configuration initialized...");
	}
	public ArrayList<MonsterDraft> getMonstersDraft() {
		return this.monstersDraft;
	}
	public ArrayList<NpcDraft> getNpcsDraft() {
		return this.npcsDraft;
	}
	public ArrayList<WarpDraft> getWarpsDraft() {
		return this.warpsDraft;
	}
	public ServerConfig getServerConfig() {
		return serverCfg;
	}
	public PlayerConfig getPlayerConfig() {
		return playerCfg;
	}
	public HashMap<Short, MapConfig> getMapsConfig() {
		return mapsConfig;
	}
	public HashMap<Short, ClassConfig> getClassesConfig() {
		return classesConfig;
	}
	public HashMap<Short, ExpConfig> getExpsConfig() {
		return expsConfig;
	}
	public HashMap<Integer, MonsterConfig> getMonstersConfig() {
		return monstersConfig;
	}
	public HashMap<Integer, NpcConfig> getNpcsConfig() {
		return npcsConfig;
	}
	public HashMap<Integer, WarpConfig> getWarpsConfig() {
		return warpsConfig;
	}
	public HashMap<Integer, ItemConfig> getItemsConfig() {
		return itemsConfig;
	}
	public HashMap<Integer, DropConfig> getDropsConfig() {
		return dropsConfig;
	}
	private void readServerConfig(String message) {
        String[] line = message.split(";");
        for (int i = 0; i < line.length; ++i ) {
        	String[] msg = line[i].split(":");
        	// server cfg
           	if (msg[0].equals("port") && msg.length == 2) {
           		this.serverCfg.port = Integer.valueOf(msg[1]);
           	}
           	if (msg[0].equals("name") && msg.length == 2) {
           		this.serverCfg.name = msg[1];
           	}
           	if (msg[0].equals("dbhost") && msg.length == 2) {
           		this.serverCfg.dbhost = msg[1];
           	}
           	if (msg[0].equals("dbport") && msg.length == 2) {
           		this.serverCfg.dbport = Integer.valueOf(msg[1]);
           	}
           	if (msg[0].equals("dbuser") && msg.length == 2) {
           		this.serverCfg.dbuser = msg[1];
           	}
           	if (msg[0].equals("dbpass") && msg.length == 2) {
           		this.serverCfg.dbpass = msg[1];
           	}
           	if (msg[0].equals("dbname") && msg.length == 2) {
           		this.serverCfg.dbname = msg[1];
           	}
           	// player cfg
           	if (msg[0].equals("startmoney") && msg.length == 2) {
           		this.playerCfg.startmoney = Integer.valueOf(msg[1]);
           	}
           	if (msg[0].equals("startmap") && msg.length == 2) {
           		this.playerCfg.startmap = Short.valueOf(msg[1]);
           	}
           	if (msg[0].equals("startmap_x") && msg.length == 2) {
           		this.playerCfg.startmap_x = Float.valueOf(msg[1]);
           	}
           	if (msg[0].equals("startmap_y") && msg.length == 2) {
           		this.playerCfg.startmap_y = Float.valueOf(msg[1]);
           	}
           	// game cfg
           	if (msg[0].equals("mapcfg") && msg.length == 2) {
           		readMapConfig(msg[1]);
           	}
           	if (msg[0].equals("classcfg") && msg.length == 2) {
           		readClassConfig(msg[1]);
           	}
           	if (msg[0].equals("expcfg") && msg.length == 2) {
           		readExpConfig(msg[1]);
           	}
           	if (msg[0].equals("monstercfg") && msg.length == 2) {
           		readMonsterConfig(msg[1]);
           	}
           	if (msg[0].equals("npccfg") && msg.length == 2) {
           		readNpcConfig(msg[1]);
           	}
           	if (msg[0].equals("warpcfg") && msg.length == 2) {
           		readWarpConfig(msg[1]);
           	}
           	if (msg[0].equals("itemcfg") && msg.length == 2) {
           		readItemConfig(msg[1]);
           	}
           	if (msg[0].equals("dropcfg") && msg.length == 2) {
           		readDropConfig(msg[1]);
           	}
           	// game script
           	if (msg[0].equals("script") && msg.length == 2) {
           		readScript(msg[1]);
           	}
        }
	}
	private void readMapConfig(String filepath) {
		System.out.println("Reading map configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		MapConfig newObj = parseMapConfig(msg[1]);
			        		if (newObj != null) {
			        			mapsConfig.put(Short.valueOf(msg[0]), newObj);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readClassConfig(String filepath) {
		System.out.println("Reading class configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
		        			ClassConfig newObj = parseClassConfig(msg[1].split(","));
		        			if (newObj != null)
		        				classesConfig.put(Short.valueOf(msg[0]), newObj);
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readExpConfig(String filepath) {
		System.out.println("Reading exp configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		short classid = Short.valueOf(msg[0]);
			        		if (expsConfig.containsKey(classid)) {
				        		parseExpConfig(msg[1].split(","), classid, expsConfig.get(classid));
			        		} else {
				        		ExpConfig newObj = parseExpConfig(msg[1].split(","));
				        		if (newObj != null)
				        			expsConfig.put(Short.valueOf(msg[0]), newObj);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readMonsterConfig(String filepath) {
		System.out.println("Reading monster configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		MonsterConfig newObj = parseMonsterConfig(Integer.valueOf(msg[0]), msg[1].split(","));
			        		if (newObj != null) {
			        			monstersConfig.put(Integer.valueOf(msg[0]), newObj);
			        			System.out.println("Readed new monster configuration " + newObj.name);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readNpcConfig(String filepath) {
		System.out.println("Reading npc configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		NpcConfig newObj = parseNpcConfig(Integer.valueOf(msg[0]), msg[1].split(","));
			        		if (newObj != null) {
			        			npcsConfig.put(Integer.valueOf(msg[0]), newObj);
			        			System.out.println("Readed new npc configuration " + newObj.name);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readWarpConfig(String filepath) {
		System.out.println("Reading warp configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		WarpConfig newObj = parseWarpConfig(Integer.valueOf(msg[0]), msg[1].split(","));
			        		if (newObj != null) {
			        			warpsConfig.put(Integer.valueOf(msg[0]), newObj);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readItemConfig(String filepath) {
		System.out.println("Reading item configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		ItemConfig newObj = parseItemConfig(Integer.valueOf(msg[0]), msg[1].split(","));
			        		if (newObj != null) {
			        			itemsConfig.put(Integer.valueOf(msg[0]), newObj);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private void readDropConfig(String filepath) {
		System.out.println("Reading drop configuration file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading configuration file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		if (cNumber.isNumber(msg[0])) {
			        		int monsterid = Integer.valueOf(msg[0]);
			        		if (dropsConfig.containsKey(monsterid)) {
			        			parseDropConfig(msg[1].split(","), monsterid, dropsConfig.get(monsterid));
			        		} else {
				        		DropConfig newObj = parseDropConfig(msg[1].split(","));
				        		if (newObj != null)
				        			dropsConfig.put(Integer.valueOf(msg[0]), newObj);
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private MapConfig parseMapConfig(String mapfile) {
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(mapfile));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + mapfile + "\" not found!, while loading configuration file");
		}
		if (scanner != null) {
			int size = Integer.valueOf(scanner.nextLine().trim());
			Integer[][] map = new Integer[size][size];
			for (int n = 0; n < size; ++n) {
				String path_y = null;
				if (scanner.hasNextLine())
					path_y = scanner.nextLine(); // rows
				for (int m = 0; m < size; ++m) {
					if (path_y == null) {
						map[n][m] = new Integer(0);
					} else {
						if (m < path_y.length())
							map[n][m] = new Integer(""+path_y.charAt(m)); // columns
						if (path_y.length() < m)
							map[n][m] = new Integer(0);
					}
				}
			}
			return new MapConfig(map);
		}
		return null;
	}
	private ClassConfig parseClassConfig(String[] value) {
		if (value.length == 3) {
			return new ClassConfig(Short.valueOf(value[0]), Short.valueOf(value[1]), Short.valueOf(value[2]));
		}
		return null;
	}
	private ExpConfig parseExpConfig(String[] value) {
		if (value.length == 2) {
			return new ExpConfig(Short.valueOf(value[0]), Integer.valueOf(value[1]));
		}
		return null;
	}
	private void parseExpConfig(String[] value, short classid, ExpConfig cfg) {
		if (value.length == 2) {
			cfg.addExp(Short.valueOf(value[0]), Integer.valueOf(value[1]));
			this.expsConfig.remove(classid);
			this.expsConfig.put(classid, cfg);
		}
	}
	private MonsterConfig parseMonsterConfig(int id, String[] value) {
		if (value.length == 10) {
			return new MonsterConfig(id, value[0], Short.valueOf(value[1]), Short.valueOf(value[2]),
					Integer.valueOf(value[3]),
					Short.valueOf(value[4]), Short.valueOf(value[5]), Short.valueOf(value[6]),
					Short.valueOf(value[7]), Short.valueOf(value[8]), Long.valueOf(value[9]));
		}
		return null;
	}
	private NpcConfig parseNpcConfig(int id, String[] value) {
		if (value.length == 10) {
			return new NpcConfig(id, value[0], Short.valueOf(value[1]), Short.valueOf(value[2]),
					Integer.valueOf(value[3]),
					Short.valueOf(value[4]), Short.valueOf(value[5]), Short.valueOf(value[6]),
					Short.valueOf(value[7]), Short.valueOf(value[8]), value[9]);
		}
		return null;
	}
	private WarpConfig parseWarpConfig(int id, String[] value) {
		if (value.length == 3) {
			return new WarpConfig(id, Short.valueOf(value[0]), Float.valueOf(value[1]), Float.valueOf(value[2]));
		}
		return null;
	}
	private ItemConfig parseItemConfig(int id, String[] value) {
		if (value.length == 22) {
			return new ItemConfig(id, value[0], Short.valueOf(value[1]), Short.valueOf(value[2]), Integer.valueOf(value[3]),
					Short.valueOf(value[4]), Short.valueOf(value[5]), Short.valueOf(value[6]), Short.valueOf(value[7]),
					Short.valueOf(value[8]), Short.valueOf(value[9]), Short.valueOf(value[10]),
					Integer.valueOf(value[11]), Integer.valueOf(value[12]), Integer.valueOf(value[13]), Integer.valueOf(value[14]),
					Integer.valueOf(value[15]), Integer.valueOf(value[16]), Integer.valueOf(value[17]), Integer.valueOf(value[18]),
					Integer.valueOf(value[19]), Integer.valueOf(value[20]), Short.valueOf(value[21]));
		}
		return null;
	}
	private DropConfig parseDropConfig(String[] value) {
		if (value.length == 2) {
			return new DropConfig(Integer.valueOf(value[0]), Integer.valueOf(value[1]));
		}
		return null;
	}
	private void parseDropConfig(String[] value, int monsterid, DropConfig cfg) {
		if (value.length == 2) {
			cfg.addItem(Integer.valueOf(value[0]), Integer.valueOf(value[1]));
			this.dropsConfig.remove(monsterid);
			this.dropsConfig.put(monsterid, cfg);
		}
	}
	private void readScript(String filepath) {
		System.out.println("Reading script file from " + filepath);
		Scanner scanner = null;
		try {
			scanner = new Scanner(new File(filepath));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			System.err.println("Error file \"" + filepath + "\" not found!, while loading script file");
			return;
		}
		if (scanner != null) {
			while (scanner.hasNextLine()) {
				String message = scanner.nextLine();
		        String[] line = message.split(";");
		        for (int i = 0; i < line.length; ++i ) {
		        	String[] msg = line[i].split(":");
		        	if (msg.length == 2) {
		        		String cmd = msg[0];
		        		if (cmd.equals("monster")) {
		        			MonsterDraft newObj = parseMonsterScript(msg[1].split(","));
			        		if (newObj != null) {
			        			monstersDraft.add(newObj);
			        			System.out.println("Added new monster at (" + newObj.getMapid() + ":" + newObj.getX() + "," + newObj.getY() + ")");
			        		}
		        		}
		        		if (cmd.equals("npc")) {
		        			NpcDraft newObj = parseNPCScript(msg[1].split(","));
			        		if (newObj != null) {
			        			npcsDraft.add(newObj);
			        			System.out.println("Added new npc at (" + newObj.getMapid() + ":" + newObj.getX() + "," + newObj.getY() + ")");
			        		}
		        		}
		        		if (cmd.equals("warp")) {
		        			WarpDraft newObj = parseWarpScript(msg[1].split(","));
			        		if (newObj != null) {
			        			warpsDraft.add(newObj);
			        			System.out.println("Added new warp at (" + newObj.getMapid() + ":" + newObj.getX() + "," + newObj.getY() + ")");
			        		}
		        		}
		        	}
		        }
			}
			scanner.close();
		}
	}
	private MonsterDraft parseMonsterScript(String[] value) {
		// map,x,y,id
		if (value.length == 4) {
			int key = Integer.valueOf(value[3]);
			if (monstersConfig.containsKey(key)) {
				return new MonsterDraft(monstersConfig.get(key), dropsConfig.get(key), Short.valueOf(value[0]), Float.valueOf(value[1]), Float.valueOf(value[2]));
			}
		}
		return null;
	}
	private NpcDraft parseNPCScript(String[] value) {
		// map,x,y,id
		if (value.length == 4) {
			int key = Integer.valueOf(value[3]);
			if (npcsConfig.containsKey(key)) {
				return new NpcDraft(npcsConfig.get(key), Short.valueOf(value[0]), Float.valueOf(value[1]), Float.valueOf(value[2]));
			}
		}
		return null;
	}
	private WarpDraft parseWarpScript(String[] value) {
		// map,x,y,id
		if (value.length == 4) {
			int key = Integer.valueOf(value[3]);
			if (warpsConfig.containsKey(key)) {
				return new WarpDraft(warpsConfig.get(key), Short.valueOf(value[0]), Float.valueOf(value[1]), Float.valueOf(value[2]));
			}
		}
		return null;
	}
}