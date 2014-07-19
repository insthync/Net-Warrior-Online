package Server;

import java.sql.ResultSet;
import java.util.ArrayList;

import Config.ClassConfig;
import Config.Configuration;
import Config.ExpConfig;
import Config.ItemConfig;

import Entity.ItemEntity;
import Entity.MonsterEntity;
import Entity.NPCEntity;
import Entity.PlayerEntity;
import Entity.UnitEntity;
import Entity.WarpEntity;

public class ClientMessageThread extends Thread {
	private Client client = null;
	private Network c = null; // Network controller
    private Configuration cfg = null;
	private MySQL sql = null;
	private GameHandler game = null;
    private String responseMsg = null;
    private ArrayList<Client> clientList = null;
	public ClientMessageThread(Client client, ArrayList<Client> clientList, String responseMsg, String threadName) {
		super(threadName);
		this.client = client;
		this.c = client.getNetwork();
		this.cfg = client.getConfiguration();
		this.sql = client.getDatabaseControl();
		this.game = client.getGameHandler();
		this.responseMsg = responseMsg;
		this.clientList = clientList;
	}
	public synchronized void run() {
        	boolean reading = true;
            try {
                while (reading) {
                    try {
                        Thread.sleep(100); // delay 100 ms
                    } catch(InterruptedException e) {
                    }
                    String[] line = responseMsg.split(";");
                    for (int i = 0; i < line.length; ++i ) {
                    	String[] msg = line[i].split(":");
                       	if (msg[0].equals("LOGIN") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		// value: username, password
                       		try {
                           		if (value.length == 2) {
                           			sql.Query("SELECT * FROM user_login WHERE username='"+value[0]+"' AND password='"+value[1]+"'");
                               		ResultSet result = sql.getResultSet();
                               		if (result.next()) {
                               			int loginid = Integer.parseInt(result.getString("loginid"));
                               			boolean isFoundLoggedInID = false;
                               			for (int j = 0 ; j < clientList.size(); ++j) {
                               				if (isFoundLoggedInID = clientList.get(j).isLoginID(loginid)) 
                               					break;
                               			}
                               			if (isFoundLoggedInID) {
                               				c.Send("LOGIN:NONE 1;");
                               			} else {
                               				client.LoggedUserID = loginid;
                               				c.Send("LOGIN:OK " + loginid + ";");
                               			}
                             			reading = false;
                             		} else {
                             			c.Send("LOGIN:NONE 0;");
                               			reading = false;
                               		}
                           		}
                       		} catch (Exception e) {
                            	System.err.println("Error while query login at message thread...");
                            	e.printStackTrace();
                            	c.Send("LOGIN:NONE 0;");
                       			reading = false;
                            }  
                       	}
                       	if (msg[0].equals("LOGOUT") && msg.length == 2) {
                       		try {
                            	c.Send("LOGOUT:0;");
                                try {
                                	Thread.sleep(3000);
                                } catch(InterruptedException e) {
                                }
                                if (c.isConnected() && !c.isClosed())
                                	c.Close();
                                while (game.playersEntity.containsKey(client.SelectedCharacterID))
                                	game.playersEntity.remove(client.SelectedCharacterID);
                                client.SelectedCharacterID = 0;
                                client.disconnect = true;
                                reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query logout at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                       		}
                       	}
                       	if (msg[0].equals("CHARCREATEABLE") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		// value loginid
                       		try {
                           		if (value.length == 1) {
                           			short[] charClassAvailID = GlobalVariable.charClassAvailID;
                           			String idList = "";
                           			for (int j = 0; j < charClassAvailID.length; ++j) {
                           				if (j > 0) {
                           					idList += (" " + charClassAvailID[j]);
                           				} else {
                           					idList += ("" + charClassAvailID[j]);
                           				}
                           			}
                       				c.Send("CHARCREATEABLE:" + idList + ";");
                           		}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query create character available list at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARCREATE") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		// value loginid, name, classid, model_hair, model_face
                       		try {
                           		if (value.length == 5) {
                           			sql.Query("SELECT name FROM user_char WHERE name='"+value[1]+"'");
                           			ResultSet result = sql.getResultSet();
                           			if (result.next()) {
                           				c.Send("CHARCREATE:NONE 1;");
                               			reading = false;
                           			} else {
                           				int loginid = Integer.parseInt(value[0]);
                           				String name = value[1];
                           				short charclass = Short.parseShort(value[2]);
                           				short model_hair = Short.parseShort(value[3]);
                           				short model_face = Short.parseShort(value[4]);
                           				short charstr = 1, chardex = 1, charint = 1;
                           				int hp = 5, sp = 3;
                           				if (cfg.getClassesConfig().containsKey(charclass)) {
                           					ClassConfig charcfg = cfg.getClassesConfig().get(charclass);
                           					charstr = charcfg.strength;
                           					chardex = charcfg.dexterity;
                           					charint = charcfg.intelligent;
                           					hp = ClassConfig.hpMultiplier((short) 1, charstr, chardex, charint, charcfg);
                           					sp = ClassConfig.hpMultiplier((short) 1, charstr, chardex, charint, charcfg);
                           				}
                           				short baseCenter_mapid = cfg.getPlayerConfig().startmap;
                           				float baseCenter_x = cfg.getPlayerConfig().startmap_x;
                           				float baseCenter_y = cfg.getPlayerConfig().startmap_y;
                           				sql.QueryNotExecute("INSERT INTO user_char (loginid, name, class, level, exp, gold, cur_hp, cur_sp, stpoint, skpoint, strength, dexterity, intelligent, model_hair, model_face, currentmap, currentmap_x, currentmap_y, savemap, savemap_x, savemap_y) " +
                           						"VALUES ('"+loginid+"', '"+name+"', '"+charclass+"', '1', '0', '"+cfg.getPlayerConfig().startmoney+"', '"+hp+"', '"+sp+"', '0', '0', '"+charstr+"', '"+chardex+"', '"+charint+"', '"+model_hair+"', '"+model_face+"' , '"+baseCenter_mapid+"', '"+baseCenter_x+"', '"+baseCenter_y+"', '"+baseCenter_mapid+"', '"+baseCenter_x+"', '"+baseCenter_y+"')");
                           				c.Send("CHARCREATE:OK 0;");
                               			reading = false;			
                           			}
                           		}
                       		} catch (Exception e) {
                            	System.err.println("Error while query create character at message thread...");
                            	e.printStackTrace();
                            	c.Send("CHARCREATE:NONE 0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARGET") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                           		if (value.length == 2) {
                           			if (Short.parseShort(value[1]) < GlobalVariable.maxchar) {
	                           			sql.Query("SELECT * FROM user_char WHERE loginid='"+value[0]+"' ORDER BY charid LIMIT "+value[1]+",1");
	                           			ResultSet result = sql.getResultSet();
	                           			if (result.next()) {
	                           				c.Send("CHARGET:OK "+result.getString("charid")+";");
	                               			reading = false;
	                           			} else {
	                           				c.Send("CHARGET:NONE 0;");
	                               			reading = false;	                           				
	                           			}
                           			} else {
                           				c.Send("CHARGET:NONE 0;");
                               			reading = false;	                           				
                           			}
                           		}
                       		} catch (Exception e) {
                            	System.err.println("Error while query get character at message thread...");
                            	e.printStackTrace();
                            	c.Send("CHARGET:NONE 0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARINFO") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                           		if (value.length == 2) {
	                           		sql.Query("SELECT * FROM user_char WHERE loginid='"+value[0]+"' AND charid='"+value[1]+"'");
	                           		ResultSet result = sql.getResultSet();	   
	                           		if (result.next()) {
	                           			int max_hp = 5;
	                           			int max_sp = 3;
	                           			String name = result.getString("name");
	                           			short char_class = result.getShort("class");
	                           			short level = result.getShort("level");
	                           			int exp = result.getInt("exp");
	                           			int nextexp = 0;
	                           			int gold = result.getInt("gold");
	                           			int cur_hp = result.getInt("cur_hp");
	                           			int cur_sp = result.getInt("cur_sp");
	                           			int stpoint = result.getInt("stpoint");
	                           			int skpoint = result.getInt("skpoint");
	                           			short strength = result.getShort("strength");
	                           			short dexterity = result.getShort("dexterity");
	                           			short intelligent = result.getShort("intelligent");
	                           			short currentmap = result.getShort("currentmap");
	                           			float currentmap_x = result.getFloat("currentmap_x");
	                           			float currentmap_y = result.getFloat("currentmap_y");
	                           			short savemap = result.getShort("savemap");
	                           			float savemap_x = result.getFloat("savemap_x");
	                           			float savemap_y = result.getFloat("savemap_y");
	                           			ExpConfig exp_config = null;
	                           			if (cfg.getExpsConfig().containsKey(char_class)) {
	                           				exp_config = cfg.getExpsConfig().get(char_class);
	                           				nextexp = exp_config.getExp(level);
	                           			}
	                           			ClassConfig class_config = null;
	                           			if (cfg.getClassesConfig().containsKey(char_class))
		                           			class_config = cfg.getClassesConfig().get(char_class);
	                           			max_hp = ClassConfig.hpMultiplier(level, strength, dexterity, intelligent, class_config);
	                           			max_sp = ClassConfig.spMultiplier(level, strength, dexterity, intelligent, class_config);
	                           			c.Send(("CHARINFO:OK "+name+" "+char_class+" "+level+
	                               				" "+nextexp+" "+exp+" "+gold+" "+max_hp+" "+cur_hp+" "+max_sp+" "+cur_sp+
	                               				" "+stpoint+" "+skpoint+" "+strength+" "+dexterity+" "+intelligent+" "+result.getShort("model_hair")+" "+result.getShort("model_face")+
	                               				" "+currentmap+" "+currentmap_x+" "+currentmap_y+
	                               				" "+savemap+" "+savemap_x+" "+savemap_y+";"));
	                               		reading = false;
	                           		} else {
	                           			c.Send("CHARINFO:NONE 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0;");
	                               		reading = false;	                           				
	                           		}
                           		}
                       		} catch (Exception e) {
                            	System.err.println("Error while query character info at message thread...");
                            	e.printStackTrace();
                            	c.Send("CHARINFO:NONE 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARINFOSELF") && msg.length == 2) {
                       		try {
                       			if (client.SelectedCharacterID > 0) {
                           			PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
	                           		if (myEnt != null) {
	                           			int max_hp = 5;
	                           			int max_sp = 3;
	                           			String name = myEnt.getName();
	                           			short char_class = myEnt.getCharclass();
	                           			short level = myEnt.getLevel();
	                           			int exp = myEnt.getExp();
	                           			int nextexp = 0;
	                           			int gold = myEnt.getGold();
	                           			int cur_hp = myEnt.getCurHP();
	                           			int cur_sp = myEnt.getCurSP();
	                           			int stpoint = myEnt.getStpoint();
	                           			int skpoint = myEnt.getSkpoint();
	                           			short strength = myEnt.getStrength();
	                           			short dexterity = myEnt.getDexterity();
	                           			short intelligent = myEnt.getIntelligent();
	                           			short currentmap = myEnt.getCurrentmap();
	                           			float currentmap_x = myEnt.getCurrentmap_x();
	                           			float currentmap_y = myEnt.getCurrentmap_y();
	                           			short savemap = myEnt.getSavemap();
	                           			float savemap_x = myEnt.getSavemap_x();
	                           			float savemap_y = myEnt.getSavemap_y();
	                           			ExpConfig exp_config = null;
	                           			if (cfg.getExpsConfig().containsKey(char_class)) {
	                           				exp_config = cfg.getExpsConfig().get(char_class);
	                           				exp = exp - exp_config.getExp((short)(level - 1));
	                           				nextexp = exp_config.getExp(level) - exp_config.getExp((short)(level - 1));
	                           			}
	                           			ClassConfig class_config = null;
	                           			if (cfg.getClassesConfig().containsKey(char_class))
		                           			class_config = cfg.getClassesConfig().get(char_class);
	                           			max_hp = ClassConfig.hpMultiplier(level, strength, dexterity, intelligent, class_config);
	                           			max_sp = ClassConfig.spMultiplier(level, strength, dexterity, intelligent, class_config);
	                           			c.Send(("CHARINFOSELF:OK "+name+" "+char_class+" "+level+
	                               				" "+nextexp+" "+exp+" "+gold+" "+max_hp+" "+cur_hp+" "+max_sp+" "+cur_sp+
	                               				" "+stpoint+" "+skpoint+" "+strength+" "+dexterity+" "+intelligent+" "+myEnt.getModel_hair()+" "+myEnt.getModel_face()+
	                               				" "+currentmap+" "+currentmap_x+" "+currentmap_y+
	                               				" "+savemap+" "+savemap_x+" "+savemap_y+";"));
	                               		reading = false;
	                           		} else {
	                           			c.Send("CHARINFOSELF:NONE 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0;");
	                               		reading = false;	                           				
	                           		}
                           		}
                       		} catch (Exception e) {
                            	System.err.println("Error while query character info at message thread...");
                            	e.printStackTrace();
                            	c.Send("CHARINFOSELF:NONE 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARDELETE") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                           		if (value.length == 2) {
                           			sql.QueryNotExecute("DELETE FROM user_char WHERE loginid='"+value[0]+"' AND charid='"+value[1]+"'");
                           		}
                       			c.Send("CHARDELETE:0;");
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query delete character at message thread...");
                            	e.printStackTrace();
                       			c.Send("CHARDELETE:0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("CHARSELECT") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                           		if (value.length == 2) {
	                           		sql.Query("SELECT * FROM user_char WHERE loginid='"+value[0]+"' AND charid='"+value[1]+"'");
	                           		ResultSet result = sql.getResultSet();	   
	                           		if (result.next()) {
	                           			int charid = Integer.parseInt(value[1]);
	                           			c.Send("CHARSELECT:OK " + charid + ";");
	                           			PlayerEntity ent = new PlayerEntity(charid, result.getString("name"), result.getShort("class"), result.getShort("level")
	                               				, result.getInt("exp"), result.getInt("gold"), result.getInt("cur_hp"), result.getInt("cur_sp")
	                               				, result.getInt("stpoint"), result.getInt("skpoint"), result.getShort("strength"), result.getShort("dexterity"), result.getShort("intelligent")
	                               				, result.getShort("model_hair"), result.getShort("model_face")
	                               				, result.getShort("currentmap"), result.getFloat("currentmap_x"), result.getFloat("currentmap_y")
	                               				, result.getShort("savemap"), result.getFloat("savemap_x"), result.getFloat("savemap_y"), client, cfg, game);
	                           			if (ent != null && !game.playersEntity.containsKey(charid)) {

	                                    	// Inventory System
	                                       	MySQL _sql = sql.clone();
	                                       	_sql.Query("SELECT * FROM inventory WHERE charid='" + charid + "' AND inventoryidx < " + GlobalVariable.max_inventory);
	                                       	ResultSet _result = _sql.getResultSet();
	                                       	String itemidList = "";
	                                       	int countResult = 0;
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
	    	                                       	MySQL __sql = sql.clone();
	    	                                       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + charid + "' AND inventoryidx='" + inventoryidx + "'");
	    	                                        __sql.close();
	                                        	}
	                                        }
	                                        if (itemidList.length() > 0 && !itemidList.equals("")) {
	                                           	c.Send("INVENTORYINFO:"+itemidList+";");
	                                        }
	                                        _sql.close();
	                                        // Inventory System::End
	                                        
	                           				game.playersEntity.put(charid, ent);
	                           				client.SelectedCharacterID = charid;
	                           				client.savetime = System.currentTimeMillis();
	                           			}
	                           		} else {
	                           			c.Send("CHARSELECT:NONE 0;");                				
	                           		}
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query select character at message thread...");
                            	e.printStackTrace();
                            	c.Send("CHARSELECT:NONE 0;");
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("TARGETPOSITION") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                           		if (client.SelectedCharacterID > 0 && value.length == 2) {
	                           		// Set position x, y
                           			float posx = Float.parseFloat(value[0]);
                           			float posy = Float.parseFloat(value[1]);
                           			game.playersEntity.get(client.SelectedCharacterID).setTarget(posx, posy);
                                	c.Send("TARGETPOSITION:" + posx + " " + posy + ";"); // Passed
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("TARGETPLAYERNODE") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (client.SelectedCharacterID > 0 && value.length == 1) {
                           			// Set node type, id
                           			UnitEntity node = game.playersEntity.get(Integer.parseInt(value[0]));
                       				if (node != null) {
                       					game.playersEntity.get(client.SelectedCharacterID).setNodeTarget(node);
                       				}
                       				c.Send("TARGETPLAYERNODE:" + value[0] + ";");
                       			}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target player at message thread...");
                            	e.printStackTrace();
                       			reading = false;                           			
                       		}
                       	}
                       	if (msg[0].equals("TARGETNPCNODE") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (client.SelectedCharacterID > 0 && value.length == 1) {
                           			// Set node type, id
                           			UnitEntity node = game.npcsEntity.get(Integer.parseInt(value[0]));
                       				if (node != null) {
                       					game.playersEntity.get(client.SelectedCharacterID).setNodeTarget(node);
                       				}
                       				c.Send("TARGETNPCNODE:" + value[0] + ";");
                       			}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target npc at message thread...");
                            	e.printStackTrace();
                       			reading = false;                           			
                       		}
                       	}
                       	if (msg[0].equals("TARGETMONSTERNODE") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (client.SelectedCharacterID > 0 && value.length == 1) {
                           			// Set node type, id
                           			UnitEntity node = game.monstersEntity.get(Integer.parseInt(value[0]));
                       				if (node != null) {
                       					game.playersEntity.get(client.SelectedCharacterID).setNodeTarget(node);
                       				}
                       				c.Send("TARGETMONSTERNODE:" + value[0] + ";");
                       			}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target monster at message thread...");
                            	e.printStackTrace();
                       			reading = false;                           			
                       		}
                       	}
                       	if (msg[0].equals("TARGETWARPNODE") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (client.SelectedCharacterID > 0 && value.length == 1) {
                           			// Set node type, id
                           			WarpEntity node = game.warpsEntity.get(Integer.parseInt(value[0]));
                       				if (node != null) {
                       					game.playersEntity.get(client.SelectedCharacterID).setNodeTarget(node);
                       				}
                       				//c.Send("TARGETWARPNODE:" + value[0] + ";");
                       			}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target warp at message thread...");
                            	e.printStackTrace();
                       			reading = false;                           			
                       		}
                       	}
                       	if (msg[0].equals("TARGETITEMNODE") && msg.length == 2 && !client.requestedNpc) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (client.SelectedCharacterID > 0 && value.length == 1) {
                           			// Set node type, id
                           			ItemEntity node = game.itemsEntity.get(Integer.parseInt(value[0]));
                       				if (node != null) {
                       					game.playersEntity.get(client.SelectedCharacterID).setNodeTarget(node);
                       				}
                       				c.Send("TARGETITEMNODE:" + value[0] + ";");
                       			}
                       			reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set target item at message thread...");
                            	e.printStackTrace();
                       			reading = false;                           			
                       		}
                       	}
                       	if (msg[0].equals("NODEPLAYER") && msg.length == 2) {
                       		try {
                           		int charid = Integer.parseInt(msg[1]);
                           		if (game.playersEntity.containsKey(charid))
                           		{
                           			PlayerEntity ent = game.playersEntity.get(charid);
                                    // id, name, modelKey, modelHair, modelFace, hp, sp
                           			String Name =  ent.getName();
                           			Name = Name.replace("'", "'39'");
                           			Name = Name.replace(" ", "'32'");
                           			Name = Name.replace(":", "'58'");
                           			Name = Name.replace(";", "'59'");
                           			c.Send("NODEPLAYER:" + charid + " " + Name + " " + ent.getCharclass() + " " + ent.getModel_hair() + " " + ent.getModel_face() + " " + ent.getMaxHP() + " " + ent.getMaxSP() + ";");
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set node at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("NODEMONSTER") && msg.length == 2) {
                       		try {
                           		int monsterid = Integer.parseInt(msg[1]);
                           		if (game.monstersEntity.containsKey(monsterid))
                           		{
                           			MonsterEntity ent = game.monstersEntity.get(monsterid);
                                    // id, name, modelKey, modelHair, modelFace, hp, sp
                           			String Name =  ent.getName();
                           			Name = Name.replace("'", "'39'");
                           			Name = Name.replace(" ", "'32'");
                           			Name = Name.replace(":", "'58'");
                           			Name = Name.replace(";", "'59'");
                           			c.Send("NODEMONSTER:" + monsterid + " " + Name + " " + ent.getCharclass() + " " + ent.getModel_hair() + " " + ent.getModel_face() + " " + ent.getMaxHP() + " " + ent.getMaxSP() + ";");
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set node at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("NODENPC") && msg.length == 2) {
                       		try {
                           		int npcid = Integer.parseInt(msg[1]);
                           		if (game.npcsEntity.containsKey(npcid))
                           		{
                           			NPCEntity ent = game.npcsEntity.get(npcid);
                                    // id, name, modelKey, modelHair, modelFace, hp, sp
                           			String Name =  ent.getName();
                           			Name = Name.replace("'", "'39'");
                           			Name = Name.replace(" ", "'32'");
                           			Name = Name.replace(":", "'58'");
                           			Name = Name.replace(";", "'59'");
                           			c.Send("NODENPC:" + npcid + " " + Name + " " + ent.getCharclass() + " " + ent.getModel_hair() + " " + ent.getModel_face() + " " + ent.getMaxHP() + " " + ent.getMaxSP() + ";");
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set node at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("NODEWARP") && msg.length == 2) {
                       		try {
                           		int warpid = Integer.parseInt(msg[1]);
                           		if (game.warpsEntity.containsKey(warpid))
                           		{
                           			WarpEntity ent = game.warpsEntity.get(warpid);
                                    // id, x, y
                           			c.Send("NODEWARP:" + warpid + " " + ent.getCurrentmap_x() + " " + ent.getCurrentmap_y() + ";");
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set node at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("NODEITEM") && msg.length == 2) {
                       		try {
                           		int dropid = Integer.parseInt(msg[1]);
                           		if (game.itemsEntity.containsKey(dropid))
                           		{
                           			ItemEntity ent = game.itemsEntity.get(dropid);
                                    // id, x, y
                           			c.Send("NODEITEM:" + dropid + " " + ent.getItemid() + " " + ent.getCurrentmap_x() + " " + ent.getCurrentmap_y() + ";");
                           		}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query set node at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("NPCDIALOG") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length == 2 && client.SelectedCharacterID > 0) {
                       				int npcid = Integer.parseInt(value[0]);
                       				String reqPage = value[1];
                                	if (reqPage.equals("none")) {
                                		client.requestedNpc = false;
                                	} else {
	                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                		if (game.npcsEntity.containsKey(npcid))
                                		{
                                			NPCEntity ent = game.npcsEntity.get(npcid);
                                			ent.response(myEnt, reqPage);
                                		}
                                	}
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query npc dialog at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("REBORN") && msg.length == 2) {
                       		try {
                       			if (client.SelectedCharacterID > 0) {
                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                	myEnt.setCurHP(myEnt.getMaxHP());
                                	myEnt.setCurSP(myEnt.getMaxSP());
                                	myEnt.setCurrentmap(myEnt.getSavemap());
                                	myEnt.setCurrentmap_x(myEnt.getSavemap_x());
                                	myEnt.setCurrentmap_y(myEnt.getSavemap_y());
                                	myEnt.setState(UnitEntity.state_idle);
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query reborn at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                       		}
                       	}
                       	// Attribute System
                       	if (msg[0].equals("ADDSTR") && msg.length == 2) {
                       		try {
                       			if (client.SelectedCharacterID > 0) {
                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                	if (myEnt.getStpoint() > 0) {
                                		myEnt.setStrength((short)(myEnt.getStrength() + 1));
                                		myEnt.setStpoint((short)(myEnt.getStpoint() - 1));
                                	}
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query add str at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("ADDDEX") && msg.length == 2) {
                       		try {
                       			if (client.SelectedCharacterID > 0) {
                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                	if (myEnt.getStpoint() > 0) {
                                		myEnt.setDexterity((short)(myEnt.getDexterity() + 1));
                                		myEnt.setStpoint((short)(myEnt.getStpoint() - 1));
                                	}
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query add str at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	if (msg[0].equals("ADDINT") && msg.length == 2) {
                       		try {
                       			if (client.SelectedCharacterID > 0) {
                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                	if (myEnt.getStpoint() > 0) {
                                		myEnt.setIntelligent((short)(myEnt.getIntelligent() + 1));
                                		myEnt.setStpoint((short)(myEnt.getStpoint() - 1));
                                	}
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query add str at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                       	// Inventory System
                       	if (msg[0].equals("INVENTORYDRAG") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length == 2 && client.SelectedCharacterID > 0) {
                       				int fromidx = Integer.parseInt(value[0]);
                       				int toidx = Integer.parseInt(value[1]);
                       				sql.Query("SELECT * FROM inventory WHERE inventoryidx='" + toidx + "' AND charid='" + client.SelectedCharacterID + "'");
                       				ResultSet result = sql.getResultSet();
                       				if (result.next()) {
                       					// Temp data
                       					int itemid = result.getInt("itemid");
                       					short amount = result.getShort("amount");
                       					short equip = result.getShort("equip");
                       					short refine = result.getShort("refine");
                       					int attributeid = result.getInt("attributeid");
                       					int slot1 = result.getInt("slot1");
                       					int slot2 = result.getInt("slot2");
                       					int slot3 = result.getInt("slot3");
                       					int slot4 = result.getInt("slot4");
                       					
                       					// Delete
                       					sql.QueryNotExecute("DELETE FROM inventory WHERE inventoryidx='" + toidx + "' AND charid='" + client.SelectedCharacterID + "'");
                       					// Change
                           				sql.QueryNotExecute("UPDATE inventory SET inventoryidx='" + toidx + "' WHERE inventoryidx='" + fromidx + "' AND charid='" + client.SelectedCharacterID + "'");
                           				// Add
                       					sql.QueryNotExecute("INSERT INTO inventory(charid, inventoryidx, itemid, amount, equip, refine, attributeid, slot1, slot2, slot3, slot4) VALUES ('"+client.SelectedCharacterID+"', '"+fromidx+"', '"+itemid+"' ,'"+amount+"', '"+equip+"', '"+refine+"', '"+attributeid+"', '"+slot1+"', '"+slot2+"', '"+slot3+"', '"+slot4+"')");
                           				
                       				} else {
                           				sql.QueryNotExecute("UPDATE inventory SET inventoryidx='" + toidx + "' WHERE inventoryidx='" + fromidx + "' AND charid='" + client.SelectedCharacterID + "'");
                       				}
                       			}
                            	// Inventory System
                       			MySQL _sql = sql.clone();
                               	_sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                               	ResultSet _result = _sql.getResultSet();
                               	String itemidList = "";
                               	int countResult = 0;
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
                                       	MySQL __sql = sql.clone();
                                       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + inventoryidx + "'");
                                        __sql.close();
                                	}
                                }
                                if (itemidList.length() > 0 && !itemidList.equals("")) {
                                   	c.Send("INVENTORYINFO:"+itemidList+";");
                                }
                                _sql.close();
                                // Inventory System::End
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query inventory drag at message thread...");
                            	e.printStackTrace();
                           		reading = false;
                            }
                       	}
                       	if (msg[0].equals("INVENTORYUSE") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length == 1 && client.SelectedCharacterID > 0) {
                       				int idx = Integer.parseInt(value[0]);
                       				sql.Query("SELECT * FROM inventory WHERE inventoryidx='" + idx + "' AND charid='" + client.SelectedCharacterID + "'");
                       				ResultSet result = sql.getResultSet();
                       				if (result.next()) {
                       					int itemid = result.getInt("itemid");
                       					short amount = result.getShort("amount");
                       					short refine = result.getShort("refine");
                       					int attributeid = result.getInt("attributeid");
                       					int slot1 = result.getInt("slot1");
                       					int slot2 = result.getInt("slot2");
                       					int slot3 = result.getInt("slot3");
                       					int slot4 = result.getInt("slot4");
	                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                       					if (cfg.getItemsConfig().containsKey(itemid)) {
                       						ItemConfig itemcfg = cfg.getItemsConfig().get(itemid);
                       						int itemtype = itemcfg.type;
                       						if (itemtype == 0) {
                       							// misc
                       						}
                       						if (itemtype == 1) {
	                       						// usable
	                       						// do something
                       							if (myEnt.useItem(itemcfg)) {
		                       						// update quantity
		                       						if (amount - 1 <= 0) {
		                       							MySQL __sql = sql.clone();
		                                               	__sql.QueryNotExecute("DELETE FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + idx + "'");
		                       							__sql.close();
		                       						} else {
		                       							MySQL __sql = sql.clone();
		                                               	__sql.QueryNotExecute("UPDATE inventory SET amount='" + (amount - 1) + "' WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + idx + "'");
		                       							__sql.close();
		                       						}
                       							}
                       						}
                       						if (itemtype >= 2) {
	                       						// equipment
                       							if (myEnt.equipItem(itemcfg, refine, attributeid, slot1, slot2, slot3, slot4)) {
	                       							MySQL _sql = sql.clone();
	                                   				_sql.Query("SELECT * FROM inventory WHERE equip='" + itemtype + "' AND charid='" + client.SelectedCharacterID + "'");
	                                   				ResultSet _result = _sql.getResultSet();
	                       							if (!_result.next()) {
		                       							MySQL __sql = sql.clone();
		                                               	__sql.QueryNotExecute("UPDATE inventory SET inventoryidx='-1', equip='" + itemtype + "'  WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + idx + "'");
		                       							__sql.close();
	                       							}
	                       							_sql.clone();
                       							}
                       						}
                       					}
                       				}

                                	// Inventory System
                           			MySQL _sql = sql.clone();
                                   	_sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                                   	ResultSet _result = _sql.getResultSet();
                                   	String itemidList = "";
                                   	int countResult = 0;
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
	                                       	MySQL __sql = sql.clone();
	                                       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + inventoryidx + "'");
	                                        __sql.close();
                                    	}
                                    }
                                    if (itemidList.length() > 0 && !itemidList.equals("")) {
                                       	c.Send("INVENTORYINFO:"+itemidList+";");
                                    }
                                    _sql.close();
                                    // Inventory System::End
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query inventory use at message thread...");
                            	e.printStackTrace();
                           		reading = false;
                            }
                       	}
                       	// Equipment
                       	if (msg[0].equals("UNWEAR") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length == 1 && client.SelectedCharacterID > 0) {
                       				int idx = Integer.parseInt(value[0]);
                       				for (int j = 0; j < GlobalVariable.max_inventory; ++j) {
                               			MySQL _sql = sql.clone();
                                       	_sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + j + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                                       	ResultSet _result = _sql.getResultSet();
                                       	if (!_result.next()) {
                                       		PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                       		if (myEnt != null)
                                       			myEnt.unEquipItem(idx);
                                       		sql.QueryNotExecute("UPDATE inventory SET equip='0', inventoryidx='" + j + "' WHERE charid='" + client.SelectedCharacterID + "' AND equip='" + idx + "'");
                                       		break;
                                       	}
                                       	_sql.close();
                       				}
                                    // Inventory System
                       				MySQL _sql = sql.clone();
                                   	_sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                                   	ResultSet _result = _sql.getResultSet();
                                   	String itemidList = "";
                                   	int countResult = 0;
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
	                                       	MySQL __sql = sql.clone();
	                                       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + inventoryidx + "'");
	                                        __sql.close();
                                    	}
                                    }
                                    if (itemidList.length() > 0 && !itemidList.equals("")) {
                                       	c.Send("INVENTORYINFO:"+itemidList+";");
                                    }
                                    _sql.close();
                                    // Inventory System::End
                       			}
                       			
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query unwear at message thread...");
                            	e.printStackTrace();
                           		reading = false;
                            }
                       	}
                       	if (msg[0].equals("UNWEARDRAGTO") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length == 2 && client.SelectedCharacterID > 0) {
                       				int idx = Integer.parseInt(value[0]);
                       				int toidx = Integer.parseInt(value[1]);
                                    sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + toidx + "'");
                                    ResultSet result = sql.getResultSet();
                                    if (!result.next()) {
                                   		PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                   		if (myEnt != null)
                                   			myEnt.unEquipItem(idx);
                                    	sql.QueryNotExecute("UPDATE inventory SET equip='0', inventoryidx='" + toidx + "' WHERE charid='" + client.SelectedCharacterID + "' AND equip='" + idx + "'");
                                    }
                                    // Inventory System
                       				MySQL _sql = sql.clone();
                                   	_sql.Query("SELECT * FROM inventory WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx < " + GlobalVariable.max_inventory);
                                   	ResultSet _result = _sql.getResultSet();
                                   	String itemidList = "";
                                   	int countResult = 0;
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
	                                       	MySQL __sql = sql.clone();
	                                       	__sql.QueryNotExecute("DELETE FROM inventory WHERE WHERE charid='" + client.SelectedCharacterID + "' AND inventoryidx='" + inventoryidx + "'");
	                                        __sql.close();
                                    	}
                                    }
                                    if (itemidList.length() > 0 && !itemidList.equals("")) {
                                       	c.Send("INVENTORYINFO:"+itemidList+";");
                                    }
                                    _sql.close();
                                    // Inventory System::End
                       			}
                       			
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query unwear at message thread...");
                            	e.printStackTrace();
                           		reading = false;
                            }
                       	}
                       	// Chat
                       	if (msg[0].equals("CHAT") && msg.length == 2) {
                       		String[] value = msg[1].split(" ");
                       		try {
                       			if (value.length > 0 && client.SelectedCharacterID > 0) {
                               		int msgType = Integer.parseInt(value[0]);
                                	PlayerEntity myEnt = game.playersEntity.get(client.SelectedCharacterID);
                                	String recMessage = "";
                               		switch (msgType) {
                               			case 0:
                               				// General
                               				if (value.length > 1) {
	                               				for (int index = 1; index < value.length; ++index)
	                               					recMessage += (value[index] + " ");
	            	                        	for (PlayerEntity ent : game.playersEntity.values()) {
	            	                        		if (myEnt.distanceFrom(ent) <= 128) {
	            	                        			ent.getClient().sendMessage("CHAT:0 " + client.SelectedCharacterID + " " + recMessage + ";");
	            	                        		}	                        			
	            	                        	}
                               				}
                               				break;
                               			case 1:
                               				// Whisper
                               				if (value.length > 2) {
                               					String receiverName = value[1];
	                               				for (int index = 2; index < value.length; ++index)
	                               					recMessage += (value[index] + " ");
	            	                        	for (PlayerEntity ent : game.playersEntity.values()) {
	            	                        		if (ent.getName().equals(receiverName)) {
	            	                        			ent.getClient().sendMessage("CHAT:1 " + client.SelectedCharacterID + " " + recMessage + ";");
	            	                        		}	                        			
	            	                        	}
                               				}
                               				break;
                               			case 2:
                               				// Party
                               				/*if (value.length > 1) {
	                               				for (int index = 1; index < msg.length; ++index)
	                               					recMessage += (" " + value[index]);
	            	                        	for (PlayerEntity ent : game.playersEntity.values()) {
	            	                        		if () {
	            	                        			ent.getClient().sendMessage("CHAT:2 " + SelectedCharacterID + " " + recMessage + ";");
	            	                        		}	                        			
	            	                        	}
                               				}*/
                               				break;
                               			case 3:
                               				// Guild
                               				/*if (value.length > 1) {
	                               				for (int index = 1; index < msg.length; ++index)
	                               					recMessage += (" " + value[index]);
	            	                        	for (PlayerEntity ent : game.playersEntity.values()) {
	            	                        		if () {
	            	                        			ent.getClient().sendMessage("CHAT:3 " + SelectedCharacterID + " " + recMessage + ";");
	            	                        		}
	            	                        	}
                               				}*/
                               				break;
                               		}
                       			}
                           		reading = false;
                       		} catch (Exception e) {
                            	System.err.println("Error while query chat at message thread...");
                            	e.printStackTrace();
                       			reading = false;
                            }
                       	}
                    }
                }
            } catch (Exception e) {
            	System.err.println("Error while running message thread...");
            	e.printStackTrace();
            }
	}
}