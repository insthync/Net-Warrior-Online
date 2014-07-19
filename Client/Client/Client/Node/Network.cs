using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MMORPGCopierClient
{
    public class Network
    {
        private TcpClient client;
        private Dictionary<Char, String> thaiCharKey;
        private Dictionary<String, Char> keyThaiChar;
        public Network(TcpClient client)
        {
            this.client = client;
            this.thaiCharKey = new Dictionary<Char, String>();
            this.keyThaiChar = new Dictionary<String, Char>();
            this.thaiCharKey.Add('ก', "'161'");
            this.thaiCharKey.Add('ข', "'162'");
            this.thaiCharKey.Add('ฃ', "'163'");
            this.thaiCharKey.Add('ค', "'164'");
            this.thaiCharKey.Add('ฅ', "'165'");
            this.thaiCharKey.Add('ฆ', "'166'");
            this.thaiCharKey.Add('ง', "'167'");
            this.thaiCharKey.Add('จ', "'168'");
            this.thaiCharKey.Add('ฉ', "'169'");
            this.thaiCharKey.Add('ช', "'170'");
            this.thaiCharKey.Add('ซ', "'171'");
            this.thaiCharKey.Add('ฌ', "'172'");
            this.thaiCharKey.Add('ญ', "'173'");
            this.thaiCharKey.Add('ฎ', "'174'");
            this.thaiCharKey.Add('ฏ', "'175'");
            this.thaiCharKey.Add('ฐ', "'176'");
            this.thaiCharKey.Add('ฑ', "'177'");
            this.thaiCharKey.Add('ฒ', "'178'");
            this.thaiCharKey.Add('ณ', "'179'");
            this.thaiCharKey.Add('ด', "'180'");
            this.thaiCharKey.Add('ต', "'181'");
            this.thaiCharKey.Add('ถ', "'182'");
            this.thaiCharKey.Add('ท', "'183'");
            this.thaiCharKey.Add('ธ', "'184'");
            this.thaiCharKey.Add('น', "'185'");
            this.thaiCharKey.Add('บ', "'186'");
            this.thaiCharKey.Add('ป', "'187'");
            this.thaiCharKey.Add('ผ', "'188'");
            this.thaiCharKey.Add('ฝ', "'189'");
            this.thaiCharKey.Add('พ', "'190'");
            this.thaiCharKey.Add('ฟ', "'191'");
            this.thaiCharKey.Add('ภ', "'192'");
            this.thaiCharKey.Add('ม', "'193'");
            this.thaiCharKey.Add('ย', "'194'");
            this.thaiCharKey.Add('ร', "'195'");
            this.thaiCharKey.Add('ฤ', "'196'");
            this.thaiCharKey.Add('ล', "'197'");
            this.thaiCharKey.Add('ฦ', "'198'");
            this.thaiCharKey.Add('ว', "'199'");
            this.thaiCharKey.Add('ศ', "'200'");
            this.thaiCharKey.Add('ษ', "'201'");
            this.thaiCharKey.Add('ส', "'202'");
            this.thaiCharKey.Add('ห', "'203'");
            this.thaiCharKey.Add('ฬ', "'204'");
            this.thaiCharKey.Add('อ', "'205'");
            this.thaiCharKey.Add('ฮ', "'206'");
            this.thaiCharKey.Add('ฯ', "'207'");
            this.thaiCharKey.Add('ะ', "'208'");
            this.thaiCharKey.Add('ั', "'209'");
            this.thaiCharKey.Add('า', "'210'");
            this.thaiCharKey.Add('ำ', "'211'");
            this.thaiCharKey.Add('ิ', "'212'");
            this.thaiCharKey.Add('ี', "'213'");
            this.thaiCharKey.Add('ึ', "'214'");
            this.thaiCharKey.Add('ื', "'215'");
            this.thaiCharKey.Add('ุ', "'216'");
            this.thaiCharKey.Add('ู', "'217'");
            this.thaiCharKey.Add('ฺ', "'218'");
            this.thaiCharKey.Add('฿', "'223'");
            this.thaiCharKey.Add('เ', "'224'");
            this.thaiCharKey.Add('แ', "'225'");
            this.thaiCharKey.Add('โ', "'226'");
            this.thaiCharKey.Add('ใ', "'227'");
            this.thaiCharKey.Add('ไ', "'228'");
            this.thaiCharKey.Add('ๅ', "'229'");
            this.thaiCharKey.Add('ๆ', "'230'");
            this.thaiCharKey.Add('็', "'231'");
            this.thaiCharKey.Add('่', "'232'");
            this.thaiCharKey.Add('้', "'233'");
            this.thaiCharKey.Add('๊', "'234'");
            this.thaiCharKey.Add('๋', "'235'");
            this.thaiCharKey.Add('์', "'236'");
            this.thaiCharKey.Add('ํ', "'237'");
            this.thaiCharKey.Add('๎', "'238'");
            this.thaiCharKey.Add('๏', "'239'");
            this.thaiCharKey.Add('๐', "'240'");
            this.thaiCharKey.Add('๑', "'241'");
            this.thaiCharKey.Add('๒', "'242'");
            this.thaiCharKey.Add('๓', "'243'");
            this.thaiCharKey.Add('๔', "'244'");
            this.thaiCharKey.Add('๕', "'245'");
            this.thaiCharKey.Add('๖', "'246'");
            this.thaiCharKey.Add('๗', "'247'");
            this.thaiCharKey.Add('๘', "'248'");
            this.thaiCharKey.Add('๙', "'249'");
            this.thaiCharKey.Add('๚', "'250'");
            this.thaiCharKey.Add('๛', "'251'");
            foreach (Char key in this.thaiCharKey.Keys)
            {
                this.keyThaiChar.Add(this.thaiCharKey[key], key);
            }
        }

        public void Connect(String ip, int port)
        {
            if (client != null && client.Connected)
                Close();
            client = null;
            client = new TcpClient();
            if (!client.Connected)
                client.Connect(ip, port);
        }

        public void Close()
        {
            if (client != null)
                client.Close();
        }

        public Boolean isConnected()
        {
            if (client == null)
                return false;
            return client.Connected;
        }

        public void Send(String message)
        {
            if (client.Connected)
            {
                Byte[] msg = System.Text.Encoding.UTF8.GetBytes(convertThaiCharToCode(message));
                NetworkStream stream = client.GetStream();
                stream.Write(msg, 0, msg.Length);
                stream.Flush();
                //Debug.WriteLine("Send: " + message);
            }
        }

        public String Receive()
        {
            String responseMsg = "";
            if (client.Connected)
            {
                Byte[] msg = new Byte[256];
                NetworkStream stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    int bytes = msg.Length;
                    while (bytes >= msg.Length)
                    {
                        bytes = stream.Read(msg, 0, msg.Length);
                        responseMsg += System.Text.Encoding.UTF8.GetString(msg, 0, bytes);
                    }
                }
            }
            //Debug.WriteLine("Receive: " + responseMsg);
            return convertCodeToThaiChar(responseMsg);
        }

        private String convertThaiCharToCode(String input)
        {
            String output = input;
            foreach (Char key in this.thaiCharKey.Keys)
            {
                output = output.Replace(Convert.ToString(key), this.thaiCharKey[key]);
            }
            return output;
        }

        private String convertCodeToThaiChar(String input)
        {
            String output = input;
            foreach (String key in this.keyThaiChar.Keys)
            {
                output = output.Replace(key, Convert.ToString(this.keyThaiChar[key]));
            }
            return output;
        }
    }
}
