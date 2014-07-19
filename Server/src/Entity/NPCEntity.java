package Entity;

import java.awt.geom.Point2D;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.*;

import org.keplerproject.luajava.*;

import Server.Client;
import Server.GameHandler;
import Server.GlobalVariable;
import Server.MySQL;

import Config.Configuration;
import Config.NpcConfig;

public class NPCEntity extends UnitEntity {
	private int id = 0;
	private String scriptFile = null;
	private LuaState luastate = null;
	private short rebornmap = 0;
	private float rebornmap_x = 0.0f;
	private float rebornmap_y = 0.0f;
	public NPCEntity(int id, String name, short charclass, short level,
			int exp,
			short strength, short dexterity, short intelligent,
			short model_hair, short model_face, short currentmap,
			float currentmap_x, float currentmap_y, String scriptFile, Configuration cfg, GameHandler game) {
		super(name, charclass, level, exp,
				strength, dexterity, intelligent, model_hair,
				model_face, currentmap, currentmap_x, currentmap_y, cfg, game);
		this.id = id;
		this.scriptFile = scriptFile;
	}
	public NPCEntity(int id, NpcConfig cfg, short currentmap,
			float currentmap_x, float currentmap_y, Configuration maincfg, GameHandler game) {
		this(id, cfg.name, cfg.charclass, cfg.level, cfg.exp,
				cfg.strength, cfg.dexterity, cfg.intelligent, cfg.model_hair,
				cfg.model_face, currentmap, currentmap_x, currentmap_y, cfg.scriptFile, maincfg, game);
	}
	public void initLUA(LuaState luastate, final String[] messages, final ArrayList<String> menus, final MySQL db, final PlayerEntity player) {
		JavaFunction mes = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				messages[0] = L.toString(2);
				return 0;
			}
		};
		JavaFunction menu = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				for (int i = 2; i <= L.getTop(); ++i) {
					String menuStr = L.toString(i);
					menuStr = menuStr.replace("'", "'39'");
					menuStr = menuStr.replace(" ", "'32'");
					menuStr = menuStr.replace(":", "'58'");
					menuStr = menuStr.replace(";", "'59'");
					menus.add(menuStr);
				}
				return 0;
			}
		};
		JavaFunction getregval = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				String str = L.toString(2);
				MySQL clonedb = db.clone();
				clonedb.Query("SELECT * FROM char_reg_value WHERE charid='" + player.getid() + "' AND str='" + str + "'");
				ResultSet result = db.getResultSet();
				try {
					result.next();
					L.pushString(result.getString("value"));
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				clonedb.clone();
				return 1;
			}
		};
		JavaFunction hasregval = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				String str = L.toString(2);
				MySQL clonedb = db.clone();
				clonedb.Query("SELECT * FROM char_reg_value WHERE charid='" + player.getid() + "' AND str='" + str + "'");
				ResultSet result = clonedb.getResultSet();
				try {
					L.pushBoolean(result.next());
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					L.pushBoolean(false);
					e.printStackTrace();
				}
				clonedb.close();
				return 1;
			}
		};
		JavaFunction checkregval = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				String str = L.toString(2);
				String value = L.toString(3);
				MySQL clonedb = db.clone();
				clonedb.Query("SELECT * FROM char_reg_value WHERE charid='" + player.getid() + "' AND str='" + str + "' AND value='" + value + "'");
				ResultSet result = clonedb.getResultSet();
				try {
					L.pushBoolean(result.next());
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					L.pushBoolean(false);
					e.printStackTrace();
				}
				clonedb.close();
				return 1;
			}
		};
		JavaFunction setregval = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				String str = L.toString(2);
				String value = L.toString(3);
				MySQL clonedb = db.clone();
				clonedb.QueryNotExecute("DELETE FROM char_reg_value WHERE charid='" + player.getid() + "' AND str='" + str + "'");
				clonedb.QueryNotExecute("INSERT INTO char_reg_value VALUES ('" + player.getid() + "','" + str + "','" + value + "')");
				clonedb.close();
				return 0;
			}
		};
		JavaFunction healhp = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				double value = L.toNumber(2);
				player.setCurHP(player.getCurHP() + (int)value);
				return 0;
			}
		};
		JavaFunction healsp = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				double value = L.toNumber(2);
				player.setCurSP(player.getCurSP() + (int)value);
				return 0;
			}
		};
		JavaFunction warp = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				short mapid = (short)L.toNumber(2);
				float posx = (float)L.toNumber(3);
				float posy = (float)L.toNumber(4);
				player.setCurrentmap(mapid);
				player.setCurrentmap_x(posx);
				player.setCurrentmap_y(posy);
				return 0;
			}
		};
		JavaFunction checklevel = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				short level = (short)L.toNumber(2);
				L.pushBoolean(player.getLevel() >= level);
				return 1;
			}
		};
		JavaFunction addexp = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				int exp = (int)L.toNumber(2);
				player.setExp(player.getExp() + exp);
				return 0;
			}
		};
		JavaFunction giveitem = new JavaFunction(luastate) {
			@Override
			public int execute() throws LuaException {
				// TODO Auto-generated method stub
				int _itemid = (int)L.toNumber(2);
				if (cfg.getItemsConfig().containsKey(_itemid)) {
					for (int j = 0; j < GlobalVariable.max_inventory; ++j) {
               			MySQL _sql = db.clone();
                       	_sql.Query("SELECT * FROM inventory WHERE charid='" + player.getid() + "' AND inventoryidx='" + j + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                       	ResultSet _result = _sql.getResultSet();
                       	try {
							if (!_result.next()) {
								db.QueryNotExecute("INSERT INTO inventory(charid, inventoryidx, itemid, amount, equip, refine, attributeid, slot1, slot2, slot3, slot4) VALUES ('"+player.getid()+"', '"+j+"', '"+_itemid+"' ,'"+1+"', '"+0+"', '"+0+"', '"+0+"', '"+0+"', '"+0+"', '"+0+"', '"+0+"')");
								break;
							}
						} catch (SQLException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
                       	_sql.close();
       				}
                    // Inventory System
       				MySQL _sql = db.clone();
                   	_sql.Query("SELECT * FROM inventory WHERE charid='" + player.getid() + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                   	ResultSet _result = _sql.getResultSet();
                   	String itemidList = "";
                   	int countResult = 0;
                    try {
						while (_result.next()) {
							int inventoryidx = _result.getInt("inventoryidx");
							int itemid = _result.getInt("itemid");
							short amount = _result.getShort("amount");
							short equip = _result.getShort("equip");
							short refine = _result.getShort("refine");
							int attributeid = _result.getInt("attributeid");
							int slot1 = _result.getInt("slot1");
							int slot2 = _result.getInt("slot2");
							int slot3 = _result.getInt("slot3");
							int slot4 = _result.getInt("slot4");
							if (amount > 0) {
						    	if (countResult > 0) {
						       		itemidList += (" "+inventoryidx+"&"+itemid+"&"+amount+"&"+equip+"&"+refine+"&"+attributeid+"&"+slot1+"&"+slot2+"&"+slot3+"&"+slot4);
						       	} else {
						        	itemidList += inventoryidx+"&"+itemid+"&"+amount+"&"+equip+"&"+refine+"&"+attributeid+"&"+slot1+"&"+slot2+"&"+slot3+"&"+slot4;
						       	}
						       	++countResult;
							} else {
						       	MySQL __sql = db.clone();
						       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + player.getid() + "' AND inventoryidx='" + inventoryidx + "'");
						        __sql.close();
							}
						}
					} catch (SQLException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
                    if (itemidList.length() > 0 && !itemidList.equals("")) {
                       	player.getClient().getNetwork().Send("INVENTORYINFO:"+itemidList+";");
                    }
                    _sql.close();
                    // Inventory System::End
				}
				return 0;
			}
		};
		
		try {
			mes.register("mes");
			menu.register("menu");
			getregval.register("getregval");
			hasregval.register("hasregval");
			checkregval.register("checkregval");
			setregval.register("setregval");
			healhp.register("healhp");
			healsp.register("healsp");
			warp.register("warp");
			checklevel.register("checklevel");
			addexp.register("addexp");
			giveitem.register("giveitem");
		} catch (LuaException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while parsing NPC function...");
			e.printStackTrace();
		}
	}
	public int getid() {
		return id;
	}
	@Override
	public void activities() {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void active() {
		// TODO Auto-generated method stub
		
	}
	public String getEntityType() {
		return "NPC";
	}
	public static String entityType() {
		return "NPC";
	}
	@Override
	public int receiveDamage(UnitEntity target) {
		// TODO Auto-generated method stub
		return 0;
	}
	public void response(PlayerEntity player) {
		Point2D currentPos = new Point2D.Float(currentmap_x, currentmap_y);
		Point2D targetPos = new Point2D.Float(player.getCurrentmap_x(), player.getCurrentmap_y());
		rotation = (float)(((Math.atan2(targetPos.getX() - currentPos.getX(), targetPos.getY() - currentPos.getY())*180)/3.14)-90);
		if (scriptFile != null && scriptFile.length() > 0) {
			Client client = player.getClient();
			MySQL db = client.getDatabaseControl().clone();
			String[] message = new String[1];
			ArrayList<String> menu = new ArrayList<String>();
			luastate = LuaStateFactory.newLuaState();
			luastate.openLibs();
			initLUA(luastate, message, menu, db, player);
			luastate.LdoString("page = \"none\"");
			luastate.LdoFile(scriptFile);
			luastate.close();
			String menuStr = "";
			int i_menu = 0;
			for (int i = 0; i < menu.size(); ++i) {
				menuStr += menu.get(i);
				menuStr += ",";
				i_menu++;
			}
			if (i_menu > 0)
				menuStr = menuStr.substring(0, menuStr.length() - 1);
			client.sendMessage("NPCDIALOG:" + id + " " + message[0] + " " + menuStr + ";");
			db.close();
			player.setNoTarget();
			client.requestedNpc = true;
		}
	}
	public void response(PlayerEntity player, String reqPage) {
		if (scriptFile != null && scriptFile.length() > 0) {
			Client client = player.getClient();
			MySQL db = client.getDatabaseControl().clone();
			String[] message = new String[1];
			ArrayList<String> menu = new ArrayList<String>();
			luastate = LuaStateFactory.newLuaState();
			luastate.openLibs();
			initLUA(luastate, message, menu, db, player);
			luastate.LdoString("page = \"" + reqPage + "\"");
			luastate.LdoFile(scriptFile);
			luastate.close();
			String menuStr = "";
			int i_menu = 0;
			for (int i = 0; i < menu.size(); ++i) {
				menuStr += menu.get(i);
				menuStr += ",";
				i_menu++;
			}
			if (i_menu > 0)
				menuStr = menuStr.substring(0, menuStr.length() - 1);
			client.sendMessage("NPCDIALOG:" + id + " " + message[0] + " " + menuStr + ";");
			db.close();
			player.setNoTarget();
			client.requestedNpc = true;
		}
	}
	public void reborn() {
		curHP = maxHP;
		curSP = maxSP;
		currentmap = rebornmap;
		currentmap_x = rebornmap_x;
		currentmap_y = rebornmap_y;
		state = state_idle;
		visible = true;
	}
}
