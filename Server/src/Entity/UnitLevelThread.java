package Entity;

import java.util.ArrayList;
import Server.Client;

public class UnitLevelThread extends Thread {
	private UnitEntity entity = null;
    private ArrayList<Client> clientList = null;
	public UnitLevelThread(UnitEntity entity, ArrayList<Client> clientList) {
		this.entity = entity;
		this.clientList = clientList;
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
                			c.sendMessage("PLAYERNODELEVEL:" + ((PlayerEntity)entity).getid() + ";");
            			}
            			if (entity.getEntityType().equals(MonsterEntity.entityType())) {
                			c.sendMessage("MONSTERNODELEVEL:" + ((MonsterEntity)entity).getid() + ";");
            			}
						if (entity.getEntityType().equals(NPCEntity.entityType())) {
	            			c.sendMessage("NPCNODELEVEL:" + ((NPCEntity)entity).getid() + ";");
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
