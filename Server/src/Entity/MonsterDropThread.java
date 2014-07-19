package Entity;

import Config.DropConfig;
import Server.GameHandler;
import Utils.aRandom;

public class MonsterDropThread extends Thread {
	private GameHandler game = null;
	private DropConfig dropcfg = null;
	private short mapid = 0;
	private float x = 0.0f;
	private float y = 0.0f;
	public MonsterDropThread(GameHandler game, DropConfig dropcfg, short mapid, float x, float y) {
		this.game = game;
		this.dropcfg = dropcfg; 
		this.mapid = mapid;
		this.x = x;
		this.y = y;
	}
    public synchronized void run() {
        try {
            try {
               Thread.sleep(100); // delay 100 ms
            } catch(InterruptedException e) {
            }
            if (dropcfg != null && game != null) {
            	for (Integer itemid : dropcfg.itemList.keySet()) {
            		Integer droprate = dropcfg.getItemDropRate(itemid);
            		Integer randnum = aRandom.doRandom(0, 10000);
            		if (randnum < droprate) {
            			game.addDropEntity(itemid, mapid, x, y);
            		}
            	}
            }
        } catch (Exception e) {
            System.err.println("Error while handling the damage thread...");
            e.printStackTrace();
        }
    }
}
