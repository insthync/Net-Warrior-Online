package Server;

import java.io.*;
import java.net.*;
import java.util.ArrayList;
import Config.*;
import Entity.*;

public class Client {
    private Runnable runner = null; // Runnable (use with thread)
    private Thread updateThread = null; // Server update thread
    private Runnable serverMsgRunner = null;
    private Thread serverMsgThread = null;
	
	private Network c = null; // Network controller
	private MySQL sql = null;
    private GameHandler game = null;
    private Configuration cfg = null;
    private ArrayList<Client> clientList = null;
    
    public boolean disconnect = false;
    private boolean isClosed = false;
    
    public int LoggedUserID = 0;
    public int SelectedCharacterID = 0;
    
    public long savetime = 0;
    public long responsetime = 0;
    public boolean requestedNpc = false;
    
	public Client(Socket c, MySQL sql, GameHandler game, Configuration cfg, ArrayList<Client> clientList) {
		this.c = new Network(c);
		this.clientList = clientList;
		this.sql = sql;
		this.game = game;
		this.cfg = cfg;
		initThread();
		responsetime = System.currentTimeMillis();
		serverMsgThread.start();
		updateThread.start();
	}
	public boolean closed() {
		return isClosed;
	}
	public Client getInstance() {
		return this;
	}
	public Network getNetwork() {
		return c;
	}
	public MySQL getDatabaseControl() {
		return sql;
	}
	public GameHandler getGameHandler() {
		return game;
	}
	public Configuration getConfiguration() {
		return cfg;
	}
	public boolean isLoginID(int loginID) {
		return (LoggedUserID == loginID);
	}
	public PlayerEntity getEntity() {
		if (SelectedCharacterID > 0) {
        	PlayerEntity myEnt = game.playersEntity.get(SelectedCharacterID);
        	return myEnt;
		}
		return null;
	}
	@SuppressWarnings("deprecation")
	public void close() {
		disconnect = true;
		while (serverMsgThread != null && serverMsgThread.isAlive())
			serverMsgThread.stop();
		while (updateThread != null && updateThread.isAlive())
			updateThread.stop();
		c.Close();
		isClosed = true;
	}
	private void initThread() {
		serverMsgRunner = new Runnable() {
            public synchronized void run() {
                while (!disconnect) {
                    try {
                        Thread.sleep(20); // delay 20 ms
                    } catch(InterruptedException ex) {
                    }
                    if (c.isConnected() && !c.isClosed() && SelectedCharacterID > 0) {
                    	// Player
                    	PlayerEntity myEnt = game.playersEntity.get(SelectedCharacterID);
                    	if (myEnt != null) {
	                    	int i;
                    		try {
	                    		// Player
		                    	String ListPlayerMessage = "LISTPLAYER:";
		                    	i = 0;
		                    	for (PlayerEntity ent : game.playersEntity.values()) {
		                    		if (ent.getCurrentmap() == myEnt.getCurrentmap() && ent.isVisible()) {
		                    			if (i > 0)
		                    				ListPlayerMessage += " ";
		                    			ListPlayerMessage += ent.getid();
		                            	// id, posx, posy, state, curHP, maxHP, curSP, maxSP, rotation
		                    			c.Send("NODEPLAYERUPDATE:" + ent.getid() + " " + ent.getCurrentmap_x() + " " + ent.getCurrentmap_y() + " " + ent.getState() + " " + ent.getCurHP() + " " + ent.getMaxHP() + " " + ent.getCurSP() + " " + ent.getMaxSP() + " " + ent.getRotation() + ";");
		    	                    	// Equipment Entity
		                    			c.Send("NODEPLAYEREQUIPMENT:" + ent.getid() + " " + ent.getEquipmentItemID(UnitEntity.itemidx_head) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_body) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_hand) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_foot) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponR) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponL) + ";");
		                    			i++;
		                    		}
		                    	}
		                    	ListPlayerMessage += ";";
		                    	if (i > 0)
		                    		c.Send(ListPlayerMessage);
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                    		try {
		                    	// Monster
		                    	String ListMonsterMessage = "LISTMONSTER:";
		                    	i = 0;
		                    	for (MonsterEntity ent : game.monstersEntity.values()) {
		                    		if (ent.getCurrentmap() == myEnt.getCurrentmap() && ent.isVisible()) {
		                    			if (i > 0)
		                    				ListMonsterMessage += " ";
		                    			ListMonsterMessage += ent.getid();
		                            	// id, posx, posy, state, curHP, maxHP, curSP, maxSP, rotation
		                    			c.Send("NODEMONSTERUPDATE:" + ent.getid() + " " + ent.getCurrentmap_x() + " " + ent.getCurrentmap_y() + " " + ent.getState() + " " + ent.getCurHP() + " " + ent.getMaxHP() + " " + ent.getCurSP() + " " + ent.getMaxSP() + " " + ent.getRotation() + ";");
		                    			// Equipment Entity
		                    			c.Send("NODEMONSTEREQUIPMENT:" + ent.getid() + " " + ent.getEquipmentItemID(UnitEntity.itemidx_head) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_body) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_hand) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_foot) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponR) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponL) + ";");
		                    			i++;
		                    		}
		                    	}
		                    	ListMonsterMessage += ";";
		                    	if (i > 0)
		                    		c.Send(ListMonsterMessage);
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                    		try {
		                    	// NPC
		                    	String ListNpcMessage = "LISTNPC:";
		                    	i = 0;
		                    	for (NPCEntity ent : game.npcsEntity.values()) {
		                    		if (ent.getCurrentmap() == myEnt.getCurrentmap() && ent.isVisible()) {
		                    			if (i > 0)
		                    				ListNpcMessage += " ";
		                        		ListNpcMessage += ent.getid();
		                                // id, posx, posy, state, curHP, maxHP, curSP, maxSP, rotation
		                        		c.Send("NODENPCUPDATE:" + ent.getid() + " " + ent.getCurrentmap_x() + " " + ent.getCurrentmap_y() + " " + ent.getState() + " " + ent.getCurHP() + " " + ent.getMaxHP() + " " + ent.getCurSP() + " " + ent.getMaxSP() + " " + ent.getRotation() + ";");
		                        		// Equipment Entity
		                        		c.Send("NODENPCEQUIPMENT:" + ent.getid() + " " + ent.getEquipmentItemID(UnitEntity.itemidx_head) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_body) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_hand) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_foot) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponR) + " " + ent.getEquipmentItemID(UnitEntity.itemidx_weaponL) + ";");
		                        		i++;
		                        	}
		                        }
		                        ListNpcMessage += ";";
		                        if (i > 0)
		                        	c.Send(ListNpcMessage);
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                    		try {
		                        // Warp
		                        String ListWarpMessage = "LISTWARP:";
		                        i = 0;
		                        for (WarpEntity ent : game.warpsEntity.values()) {
		                        	if (ent.getCurrentmap() == myEnt.getCurrentmap() && ent.isVisible()) {
		                        		if (i > 0)
		                        			ListWarpMessage += " ";
		                        		ListWarpMessage += ent.getid();
		                        		i++;
		                        	}
		                        }
		                        ListWarpMessage += ";";
		                        if (i > 0)
		                        	c.Send(ListWarpMessage);
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                    		try {
		                        // Item
		                        String ListItemMessage = "LISTITEM:";
		                        i = 0;
		                        for (ItemEntity ent : game.itemsEntity.values()) {
		                        	if (ent.getCurrentmap() == myEnt.getCurrentmap() && ent.isVisible()) {
		                        		if (i > 0)
		                        			ListItemMessage += " ";
		                        		ListItemMessage += ent.getid();
		                        		i++;
		                        	}
		                        }
		                        ListItemMessage += ";";
		                        if (i > 0)
		                        	c.Send(ListItemMessage);
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                    		try {
		                        if (System.currentTimeMillis() - savetime >= 500) {
		                        	MySQL _sql = sql.clone();
		                        	_sql.QueryNotExecute("UPDATE user_char SET class='"+myEnt.getCharclass()+"', level='"+myEnt.getLevel()+"', exp='"+myEnt.getExp()+"', gold='"+myEnt.getGold()+"'" +
			                       			", cur_hp='"+myEnt.getCurHP()+"', cur_sp='"+myEnt.getCurSP()+"', stpoint='"+myEnt.getStpoint()+"', skpoint='"+myEnt.getSkpoint()+"', strength='"+myEnt.getStrength()+"', dexterity='"+myEnt.getDexterity()+"', intelligent='"+myEnt.getIntelligent()+"'" +
			                       					", model_hair='"+myEnt.getModel_hair()+"', model_face='"+myEnt.getModel_face()+"', currentmap='"+myEnt.getCurrentmap()+"', currentmap_x='"+myEnt.getCurrentmap_x()+"', currentmap_y='"+myEnt.getCurrentmap_y()+"'" +
			                       							", savemap='"+myEnt.getSavemap()+"', savemap_x='"+myEnt.getSavemap_x()+"', savemap_y='"+myEnt.getSavemap_y()+"' WHERE charid='"+SelectedCharacterID+"' AND loginid='"+LoggedUserID+"'");
		                        	_sql.close();
			                       	savetime = System.currentTimeMillis();
		                        }
            	            } catch (Exception ex) {
            	            	System.err.println("Error while running update message thread...");
            	            	ex.printStackTrace();
            	            }
                        } // end if myEnt != null
                    } // end if connected
                } // end while
            } // end function run()
		};
        runner = new Runnable() {
            public synchronized void run() {
                try {
                    while (!disconnect) {
                        try {
                            Thread.sleep(20); // delay 20 ms
                        } catch(InterruptedException e) {
                        }
                        // Receiving message.
                        if (c.isConnected() && !c.isClosed() && (System.currentTimeMillis() - responsetime) / 1000 < 30) {
	                        InputStream input = c.getInputStream();
	                        if (input.available() > 0) {
	                    		responsetime = System.currentTimeMillis();
		                        String responseMsg = c.Receive();
		                        //System.out.println(responseMsg);
		                        Thread msgThread = new ClientMessageThread(getInstance(), clientList, responseMsg, "msgThread");
		                        msgThread.start();
	                        }
                        } else {
                        	disconnect = true;
                        }
                    }
                } catch (Exception e) {
                    System.err.println("Error while running client thread...");
                    e.printStackTrace();
                    if (c.isConnected() && !c.isClosed())
                    	c.Close();
                    disconnect = true;
                }
            }
        };
        serverMsgThread = new Thread(serverMsgRunner, "serverMsgThread");
        updateThread = new Thread(runner, "updateThread");
	}
	
	public void sendMessage(String message) {
        if (c.isConnected() && !c.isClosed()) {
        	c.Send(message);
        }
	}
}
