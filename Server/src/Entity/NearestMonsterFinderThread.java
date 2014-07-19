package Entity;

import java.awt.geom.Point2D;
import Server.GameHandler;

public class NearestMonsterFinderThread extends Thread {
	private GameHandler game = null;
	private UnitEntity pathOwner = null;
	private short pathOwner_map = 0;
	private Point2D pathOwner_pos = null;
	private float range = -1; // range = -1 mean all range
	public NearestMonsterFinderThread(GameHandler game, UnitEntity pathOwner, float range) {
		this.game = game;
		this.pathOwner = pathOwner;
		this.pathOwner_map = pathOwner.getCurrentmap();
		this.pathOwner_pos = new Point2D.Float(pathOwner.getCurrentmap_x(), pathOwner.getCurrentmap_y());
		this.range = range;
	}
    public synchronized void run() {
        try {
            try {
               Thread.sleep(100); // delay 100 ms
            } catch(InterruptedException e) {
            }
            if (game != null && pathOwner != null && pathOwner_map > 0 && pathOwner_pos != null) {
            	float nearest = Float.MAX_VALUE;
            	MonsterEntity nearestMonster = null; 
            	for (MonsterEntity monster : game.monstersEntity.values()) {
            		short pathTarget_map = monster.getCurrentmap();
            		Point2D pathTarget_pos = new Point2D.Float(monster.getCurrentmap_x(), monster.getCurrentmap_y());
            		if (pathOwner_map == pathTarget_map) {
            			float currentDistance = Float.MAX_VALUE;
	            		if (range <= -1) {
	            			if ((currentDistance = (float)pathOwner_pos.distance(pathTarget_pos)) < nearest) {
	            				nearest = currentDistance;
	            				nearestMonster = monster;
	            			}
	            		} else {
	            			if (pathOwner_pos.distance(pathTarget_pos) < range && (currentDistance = (float)pathOwner_pos.distance(pathTarget_pos)) < nearest) {
	            				nearest = currentDistance;
	            				nearestMonster = monster;
	            			}
	            		}
            		}
            	}
            	pathOwner.setNearestNode(nearestMonster);
            }
        } catch (Exception e) {
            System.err.println("Error while handling the nearest node finder thread...");
            e.printStackTrace();
        }
    }
}
