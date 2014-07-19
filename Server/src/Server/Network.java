package Server;

import java.io.*;
import java.net.*;
import java.util.HashMap;

public class Network {
	private Socket client = null;
	private HashMap<Character, String> thaiCharKey;
	public Network(Socket client) {
		this.client = client;
		this.thaiCharKey = new HashMap<Character, String>();
        this.thaiCharKey.put('¡', "'161'");
        this.thaiCharKey.put('¢', "'162'");
        this.thaiCharKey.put('£', "'163'");
        this.thaiCharKey.put('¤', "'164'");
        this.thaiCharKey.put('¥', "'165'");
        this.thaiCharKey.put('¦', "'166'");
        this.thaiCharKey.put('§', "'167'");
        this.thaiCharKey.put('¨', "'168'");
        this.thaiCharKey.put('©', "'169'");
        this.thaiCharKey.put('ª', "'170'");
        this.thaiCharKey.put('«', "'171'");
        this.thaiCharKey.put('¬', "'172'");
        this.thaiCharKey.put('­', "'173'");
        this.thaiCharKey.put('®', "'174'");
        this.thaiCharKey.put('¯', "'175'");
        this.thaiCharKey.put('°', "'176'");
        this.thaiCharKey.put('±', "'177'");
        this.thaiCharKey.put('²', "'178'");
        this.thaiCharKey.put('³', "'179'");
        this.thaiCharKey.put('´', "'180'");
        this.thaiCharKey.put('µ', "'181'");
        this.thaiCharKey.put('¶', "'182'");
        this.thaiCharKey.put('·', "'183'");
        this.thaiCharKey.put('¸', "'184'");
        this.thaiCharKey.put('¹', "'185'");
        this.thaiCharKey.put('º', "'186'");
        this.thaiCharKey.put('»', "'187'");
        this.thaiCharKey.put('¼', "'188'");
        this.thaiCharKey.put('½', "'189'");
        this.thaiCharKey.put('¾', "'190'");
        this.thaiCharKey.put('¿', "'191'");
        this.thaiCharKey.put('À', "'192'");
        this.thaiCharKey.put('Á', "'193'");
        this.thaiCharKey.put('Â', "'194'");
        this.thaiCharKey.put('Ã', "'195'");
        this.thaiCharKey.put('Ä', "'196'");
        this.thaiCharKey.put('Å', "'197'");
        this.thaiCharKey.put('Æ', "'198'");
        this.thaiCharKey.put('Ç', "'199'");
        this.thaiCharKey.put('È', "'200'");
        this.thaiCharKey.put('É', "'201'");
        this.thaiCharKey.put('Ê', "'202'");
        this.thaiCharKey.put('Ë', "'203'");
        this.thaiCharKey.put('Ì', "'204'");
        this.thaiCharKey.put('Í', "'205'");
        this.thaiCharKey.put('Î', "'206'");
        this.thaiCharKey.put('Ï', "'207'");
        this.thaiCharKey.put('Ð', "'208'");
        this.thaiCharKey.put('Ñ', "'209'");
        this.thaiCharKey.put('Ò', "'210'");
        this.thaiCharKey.put('Ó', "'211'");
        this.thaiCharKey.put('Ô', "'212'");
        this.thaiCharKey.put('Õ', "'213'");
        this.thaiCharKey.put('Ö', "'214'");
        this.thaiCharKey.put('×', "'215'");
        this.thaiCharKey.put('Ø', "'216'");
        this.thaiCharKey.put('Ù', "'217'");
        this.thaiCharKey.put('Ú', "'218'");
        this.thaiCharKey.put('ß', "'223'");
        this.thaiCharKey.put('à', "'224'");
        this.thaiCharKey.put('á', "'225'");
        this.thaiCharKey.put('â', "'226'");
        this.thaiCharKey.put('ã', "'227'");
        this.thaiCharKey.put('ä', "'228'");
        this.thaiCharKey.put('å', "'229'");
        this.thaiCharKey.put('æ', "'230'");
        this.thaiCharKey.put('ç', "'231'");
        this.thaiCharKey.put('è', "'232'");
        this.thaiCharKey.put('é', "'233'");
        this.thaiCharKey.put('ê', "'234'");
        this.thaiCharKey.put('ë', "'235'");
        this.thaiCharKey.put('ì', "'236'");
        this.thaiCharKey.put('í', "'237'");
        this.thaiCharKey.put('î', "'238'");
        this.thaiCharKey.put('ï', "'239'");
        this.thaiCharKey.put('ð', "'240'");
        this.thaiCharKey.put('ñ', "'241'");
        this.thaiCharKey.put('ò', "'242'");
        this.thaiCharKey.put('ó', "'243'");
        this.thaiCharKey.put('ô', "'244'");
        this.thaiCharKey.put('õ', "'245'");
        this.thaiCharKey.put('ö', "'246'");
        this.thaiCharKey.put('÷', "'247'");
        this.thaiCharKey.put('ø', "'248'");
        this.thaiCharKey.put('ù', "'249'");
        this.thaiCharKey.put('ú', "'250'");
        this.thaiCharKey.put('û', "'251'");
	}

    public void Connect(SocketAddress ip, int port)
    {
        try {
			client.connect(ip, port);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }

    public void Close()
    {
        try {
			client.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }

    public Boolean isConnected()
    {
        return client.isConnected();
    }

    public Boolean isClosed()
    {
        return client.isClosed();
    }

    public void Send(String message)
    {
    	if (client.isConnected() && !client.isClosed()) {
	        try
	        {
	        	OutputStream output = client.getOutputStream();
	        	output.write(convertThaiCharToCode(message).getBytes());
	        	output.flush();
	        }
	        catch (Exception e)
	        {
	        }
    	}
    }

    public String Receive()
    {
        String responseMsg = "";
    	if (client.isConnected() && !client.isClosed()) {
	        try
	        {
	            InputStream input = client.getInputStream();
	            if (input.available() > 0) {
	                byte[] byteInput = new byte[input.available()];
	                int reads = input.read(byteInput, 0, byteInput.length);
	                if(reads < byteInput.length)	//Just to be sure, not necessary I think
	                {
	                    byte[] byteInput_ = new byte[reads];
	                    System.arraycopy(byteInput, 0, byteInput_, 0, reads);
	                    byteInput = byteInput_;
	                }
	                responseMsg = new String(byteInput);
	            }
	            //System.Console.WriteLine(responseMsg);
	        }
	        catch (Exception e)
	        {
	        }
    	}
        return responseMsg;
    }
    
    public InputStream getInputStream() {
    	try {
			return client.getInputStream();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			return null;
		}
    }
    
    public OutputStream getOutputStream() {
    	try {
			return client.getOutputStream();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			return null;
		}
    }
    
    private String convertThaiCharToCode(String input)
    {
        String output = input;
        for (Character key : this.thaiCharKey.keySet())
        {
            output = output.replace(String.valueOf(key), this.thaiCharKey.get(key));
        }
        return output;
    }
}
