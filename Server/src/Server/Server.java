package Server;

import java.io.*;
import java.net.*;
import java.util.*;

import Config.Configuration;

public class Server {
	private ServerSocket s = null;
	private MySQL sql = null;
	private boolean available = false;
    private ArrayList<Client> clientList = null;
    private Runnable acceptRunner = null; // Runnable (use with thread)
    private Thread acceptThread = null; // Client accepting thread
    private GameHandler game = null;
    private Configuration conf = null;
	public Server(Configuration conf) {
		this.conf = conf;
		this.clientList = new ArrayList<Client>();
		this.init();
		this.initThread();
		this.acceptThread.start();
	}
	
	private void init() {
		this.sql = new MySQL(conf.getServerConfig().dbhost, conf.getServerConfig().dbport, conf.getServerConfig().dbuser, conf.getServerConfig().dbpass, conf.getServerConfig().dbname);

		try {
			s = new ServerSocket(conf.getServerConfig().port);
			System.out.println("Server started.");
			available = true;
		} catch (IOException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while initializing server...");
			e.printStackTrace();
			available = false;
		}
		
		this.game = new GameHandler(clientList, conf);
	}
	
	private void initThread() {
		acceptRunner = new Runnable() {
            public synchronized void run() {
                try {
                    while(available) {
                        try {
                            Thread.sleep(100); // delay 100 ms
                        } catch(InterruptedException e) {
                        }
                        Socket c = s.accept(); // waiting for client
                        Client client = new Client(c, sql, game, conf, clientList);
                        clientList.add(client);
                        //System.out.println("New client logged in, Now have " + clientList.size() + " clients.");
                    }
                } catch (Exception e) {
                    System.err.println("Error while running server thread...");
                    e.printStackTrace();
                }
            }
		};
        acceptThread = new Thread(acceptRunner, "acceptThread");
	}
	
	@SuppressWarnings("deprecation")
	public void close() {
		try {
            while (!available && clientList.size() > 0) {
                for (int i = 0; i < clientList.size(); ++i ) {
             	   Client client = clientList.get(i);
             	   if (client.closed()) {
             		   if (client.SelectedCharacterID > 0) {
             			   while (game.playersEntity.containsKey(client.SelectedCharacterID)) {
             				   game.playersEntity.remove(client.SelectedCharacterID);
             			   }
             		   }
             		   clientList.remove(i);
 	               } else {
 	            	   client.close();
 	               }
                }
            }
			System.out.println("All client thread stopped.");
			sql.close();
			System.out.println("MySQL handler stopped.");
			game.close();
			System.out.println("Game handler stopped.");
			s.close();
			System.out.println("Server stopped.");
			available = false;
			while (acceptThread != null && acceptThread.isAlive()) {
				acceptThread.stop();
			}
			System.out.println("Client accept thread stopped.");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			System.err.println("Error while closing server...: " + e.toString());
		}
	}
	
	public void run() {
        try {
           while(available) {
                try {
                    Thread.sleep(100); // delay 100 ms
                } catch(InterruptedException e) {
                }
                for (int i = 0; i < clientList.size(); ++i ) {
                	Client client = clientList.get(i);
	               	if (client.disconnect) {
	               		if (client.closed()) {
	               			if (client.SelectedCharacterID > 0) {
	               				while (game.playersEntity.containsKey(client.SelectedCharacterID)) {
	               					game.playersEntity.remove(client.SelectedCharacterID);
	               				}
	               			}
	               			clientList.remove(i);
	               		} else {
	               			client.close();
	               		}
	               	}
                }
            }
        } catch (Exception e) {
            System.err.println("Error while running server thread...");
            e.printStackTrace();
        }
	}
}
