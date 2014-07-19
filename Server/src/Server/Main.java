package Server;

import Config.Configuration;

public class Main {
	private Thread closeThread;
	private Runnable closeRunner;
	private Server s = null;
    private Configuration conf = null;
    public void InitThread() {
    	closeRunner = new Runnable() {
            public synchronized void run() {
            	Stop();
            }
		};
    	closeThread = new Thread(closeRunner, "closeThread");
    }
    public Thread getCloseThread() {
    	return closeThread;
    }
	public void Setup() {
		System.out.println("Initializing configuration...");
	    conf = new Configuration();
	}
	public void Start() {
		System.out.println("Starting server...");
		s = new Server(conf);
		s.run();
	}
	public void Stop() {
		System.out.println("Stopping Server...");
		s.close();
	}
	public static void main(String[] args) {
		Main m = new Main();
		m.Setup();
		m.Start();
		Runtime.getRuntime().addShutdownHook(m.getCloseThread());
		return;
	}
}
