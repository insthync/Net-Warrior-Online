package Entity;

import java.util.ArrayList;
import Server.Client;

public class UnitDamageThread extends Thread {
	private UnitEntity entity = null;
    private ArrayList<Client> clientList = null;
    private int damage = 0;
	public UnitDamageThread(UnitEntity entity, ArrayList<Client> clientList, int damage) {
		this.entity = entity;
		this.clientList = clientList;
		this.damage = damage;
	}
    public synchronized void run() {
        try {
            try {
               Thread.sleep(100); // delay 20 ms
            } catch(InterruptedException e) {
            }
            if (clientList != null) {
            	for (Client c : clientList) {
            		PlayerEntity p = c.getEntity();
            		if (p != null && p.getCurrentmap() == entity.getCurrentmap() && p.distanceFrom(entity) <= 128) {
            			if (entity.getEntityType().equals(PlayerEntity.entityType())) {
                			c.sendMessage("PLAYERNODEDAMAGE:" + ((PlayerEntity)entity).getid() + " " + damage + ";");
            			}
            			if (entity.getEntityType().equals(MonsterEntity.entityType())) {
                			c.sendMessage("MONSTERNODEDAMAGE:" + ((MonsterEntity)entity).getid() + " " + damage + ";");
            			}
						if (entity.getEntityType().equals(NPCEntity.entityType())) {
	            			c.sendMessage("NPCNODEDAMAGE:" + ((NPCEntity)entity).getid() + " " + damage + ";");
						}
            		}
            	}
            }
        } catch (Exception e) {
            System.err.println("Error while handling the damage thread...");
            e.printStackTrace();
        }
    }
}
