package Server;

import java.util.*;
import Config.*;
import ConfigDraft.*;
import Entity.*;

public class GameHandler {
	private ArrayList<Client> clientList;
	public HashMap<Integer, PlayerEntity> playersEntity;
	public HashMap<Integer, MonsterEntity> monstersEntity;
	public HashMap<Integer, NPCEntity> npcsEntity;
	public HashMap<Integer, WarpEntity> warpsEntity;
	public HashMap<Integer, ItemEntity> itemsEntity;
	private int dropId = 1;
	private int monsterId = 1;
	private int npcId = 1;
	private int warpId = 1;
	private boolean available = true;
	private Runnable playerRunner = null;
	private Thread playerThread = null;
	private Runnable monsterRunner = null;
	private Thread monsterThread = null;
	private Runnable npcRunner = null;
	private Thread npcThread = null;
	private Runnable itemRunner = null;
	private Thread itemThread = null;
	private Configuration cfg = null;
	public GameHandler(ArrayList<Client> clientList, Configuration cfg) {
		this.clientList = clientList;
		this.cfg = cfg;
		// Player
		this.playersEntity = new HashMap<Integer, PlayerEntity>();
		// Script Entity (Monster, NPC, Warp)
		this.monstersEntity = new HashMap<Integer, MonsterEntity>();
		for (MonsterDraft draft : cfg.getMonstersDraft()) {
			this.addMonsterEntity(draft.getCfg(), draft.getDropCfg(), draft.getMapid(), draft.getX(), draft.getY());
		}
		this.npcsEntity = new HashMap<Integer, NPCEntity>();
		for (NpcDraft draft : cfg.getNpcsDraft()) {
			this.addNPCEntity(draft.getCfg(), draft.getMapid(), draft.getX(), draft.getY());
		}
		this.warpsEntity = new HashMap<Integer, WarpEntity>();
		for (WarpDraft draft : cfg.getWarpsDraft()) {
			this.addWarpEntity(draft.getCfg(), draft.getMapid(), draft.getX(), draft.getY());
		}
		// Drop Item
		this.itemsEntity = new HashMap<Integer, ItemEntity>();
		initThread();
		playerThread.start();
		monsterThread.start();
		npcThread.start();
		itemThread.start();
	}
	
	public void monsterDoDrop(DropConfig dropcfg, short mapid, float x, float y) {
		MonsterDropThread dropThread = new MonsterDropThread(this, dropcfg, mapid, x, y);
		dropThread.start();
	}
	
	public void addMonsterEntity(MonsterConfig monCfg, DropConfig dropCfg, short mapid, float x, float y) {
		this.monstersEntity.put(monsterId, new MonsterEntity(monsterId++, monCfg, dropCfg, mapid, x, y, cfg, this));
	}
	
	public void addNPCEntity(NpcConfig npcCfg, short mapid, float x, float y) {
		this.npcsEntity.put(npcId, new NPCEntity(npcId++, npcCfg, mapid, x, y, cfg, this));
	}
	
	public void addWarpEntity(WarpConfig warpCfg, short mapid, float x, float y) {
		this.warpsEntity.put(warpId, new WarpEntity(warpId++, warpCfg, mapid, x, y, cfg));
	}
	
	public void addDropEntity(int itemid, short mapid, float x, float y) {
		this.itemsEntity.put(dropId, new ItemEntity(dropId++, itemid, mapid, x, y, cfg));
	}
	
	private void initThread() {
		// Player update thread
		playerRunner = new Runnable() {
            public synchronized void run() {
                while(available) {
                    try {
                        Thread.sleep(1); // delay 1 ms
                    } catch(InterruptedException ex) {
                    }
                    try {
	                	for (PlayerEntity ent : playersEntity.values()) {
	                		ent.update(clientList);
	                	}
	                } catch (Exception ex) {
	                    System.err.println("Error while handling the game thread...");
	                    ex.printStackTrace();
	                }
                }
            }
		};
        playerThread = new Thread(playerRunner, "playerThread");
        // Monster update thread
		monsterRunner = new Runnable() {
            public synchronized void run() {
                while(available) {
                    try {
                        Thread.sleep(1); // delay 1 ms
                    } catch(InterruptedException ex) {
                    }
                    try {
	                	for (MonsterEntity ent : monstersEntity.values()) {
	                		ent.update(clientList);
	                	}
                    } catch (Exception ex) {
                        System.err.println("Error while handling the game thread...");
                        ex.printStackTrace();
                    }
                }
            }
		};
        monsterThread = new Thread(monsterRunner, "monsterThread");
        // NPC Update thread
		npcRunner = new Runnable() {
            public synchronized void run() {
                while(available) {
                    try {
                        Thread.sleep(1); // delay 1 ms
                    } catch(InterruptedException ex) {
                    }
                    try {
	                	for (NPCEntity ent : npcsEntity.values()) {
	                		ent.update(clientList);
	                	}
	                } catch (Exception ex) {
	                    System.err.println("Error while handling the game thread...");
	                    ex.printStackTrace();
	                }
                }
            }
		};
        npcThread = new Thread(npcRunner, "npcThread");
        // Item Update thread
        itemRunner = new Runnable() {
			public synchronized  void run() {
				// TODO Auto-generated method stub
                while(available) {
                    try {
                        Thread.sleep(1); // delay 1 ms
                    } catch(InterruptedException ex) {
                    }
                    try {
	                	for (Integer key : itemsEntity.keySet()) {
	                		if (System.currentTimeMillis() - itemsEntity.get(key).dropWhen() >= GlobalVariable.dropAliveTime) {
	                			itemsEntity.remove(key);
	                		}
	                	}
                    } catch (Exception ex) {
                        System.err.println("Error while handling the game thread...");
                        ex.printStackTrace();
                    }
                }
			}
        };
        itemThread = new Thread(itemRunner, "itemThread");
	}
	
	@SuppressWarnings("deprecation")
	public void close() {
		available = false;
		while (playerThread != null && playerThread.isAlive())
			playerThread.stop();
		while (monsterThread != null && monsterThread.isAlive())
			monsterThread.stop();
		while (npcThread != null && npcThread.isAlive())
			npcThread.stop();
		while (itemThread != null && itemThread.isAlive())
			itemThread.stop();
	}
}
