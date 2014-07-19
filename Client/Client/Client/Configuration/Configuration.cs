using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace MMORPGCopierClient
{
    public class Configuration
    {
        private XmlDocument xmldoc;
        private List<ServerAddressConfig> serverlist;
        private Dictionary<short, GameClassConfig> classlist;
        private Dictionary<int, GameItemConfig> itemlist;
        private Dictionary<short, GameModelConfig> modellist;
        private Dictionary<short, GameMapConfig> maplist;
        private Dictionary<String, String> dialoglist;
        public Configuration()
        {
            xmldoc = new XmlDocument();
            serverlist = new List<ServerAddressConfig>();
            classlist = new Dictionary<short, GameClassConfig>();
            itemlist = new Dictionary<int, GameItemConfig>();
            modellist = new Dictionary<short, GameModelConfig>();
            maplist = new Dictionary<short, GameMapConfig>();
            dialoglist = new Dictionary<String, String>();
        }

        public List<ServerAddressConfig> getServerList()
        {
            return serverlist;
        }
        public Dictionary<short, GameClassConfig> getClassList()
        {
            return classlist;
        }
        public Dictionary<int, GameItemConfig> getItemList()
        {
            return itemlist;
        }
        public Dictionary<short, GameModelConfig> getModelList()
        {
            return modellist;
        }
        public Dictionary<short, GameMapConfig> getMapList()
        {
            return maplist;
        }
        public Dictionary<String, String> getDialogList()
        {
            return dialoglist;
        }
        public void parsing()
        {
            parseServerAddressConfig();
            parseGameClassConfig();
            parseGameItemConfig();
            parseGameModelConfig();
            parseGameMapConfig();
            parseGameDialogConfig();
        }

        public void parseServerAddressConfig()
        {
            xmldoc.Load("Config/serverinfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("server");
            foreach (XmlNode n in nList)
            {
                string address = "127.0.0.1";
                int port = 5000;
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("address"))
                        address = childnode.InnerText;
                    if (childnode.Name.Equals("port"))
                        port = Convert.ToInt32(childnode.InnerText);
                }
                serverlist.Add(new ServerAddressConfig(address, port));
            }
        }

        public void parseGameClassConfig()
        {
            xmldoc.Load("Config/classinfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("class");
            foreach (XmlNode n in nList)
            {
                short id = 1;
                string name = "";
                string description = "";
                short modelid = 0;
                Dictionary<short, String> stateClip = new Dictionary<short, String>();
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("id"))
                        id = Convert.ToInt16(childnode.InnerText);
                    if (childnode.Name.Equals("name"))
                        name = childnode.InnerText;
                    if (childnode.Name.Equals("description"))
                        description = childnode.InnerText;
                    if (childnode.Name.Equals("modelid"))
                        modelid = Convert.ToInt16(childnode.InnerText);
                }
                if (!classlist.ContainsKey(id))
                    classlist.Add(id, new GameClassConfig(name, description, modelid));
            }
        }

        public void parseGameItemConfig()
        {
            xmldoc.Load("Config/iteminfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("item");
            foreach (XmlNode n in nList)
            {
                int id = 1;
                string path = "";
                Vector3 position = new Vector3();
                Vector3 rotation = new Vector3();
                float scale = 1.0f;
                string name = "";
                string description = "";
                short equipid = 0;
                string invenpath = "";
                Dictionary<short, String> stateClip = new Dictionary<short, String>();
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("id"))
                        id = Convert.ToInt32(childnode.InnerText);
                    if (childnode.Name.Equals("path"))
                        path = childnode.InnerText;
                    if (childnode.Name.Equals("position"))
                    {
                        position.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        position.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        position.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("rotation"))
                    {
                        rotation.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        rotation.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        rotation.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("scale"))
                        scale = Convert.ToSingle(childnode.InnerText);
                    if (childnode.Name.Equals("name"))
                        name = childnode.InnerText;
                    if (childnode.Name.Equals("description"))
                        description = childnode.InnerText;
                    if (childnode.Name.Equals("equipid"))
                        equipid = Convert.ToInt16(childnode.InnerText);
                    if (childnode.Name.Equals("invenpath"))
                        invenpath = childnode.InnerText;
                }
                if (!itemlist.ContainsKey(id))
                    itemlist.Add(id, new GameItemConfig(path, position, rotation, scale, name, description, equipid, invenpath));
            }
        }

        public void parseGameModelConfig()
        {
            xmldoc.Load("Config/modelinfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("model");
            foreach (XmlNode n in nList)
            {
                short id = 1;
                string path = "";
                Vector3 position = new Vector3();
                Vector3 rotation = new Vector3();
                float scale = 1.0f;
                Dictionary<short, String> stateClip = new Dictionary<short, String>();
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("id"))
                        id = Convert.ToInt16(childnode.InnerText);
                    if (childnode.Name.Equals("path"))
                        path = childnode.InnerText;
                    if (childnode.Name.Equals("position"))
                    {
                        position.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        position.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        position.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("rotation"))
                    {
                        rotation.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        rotation.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        rotation.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("scale"))
                        scale = Convert.ToSingle(childnode.InnerText);
                    if (childnode.Name.Equals("clip"))
                    {
                        short state = (short)GameState.anim_idle;
                        String clipname = "Idle";
                        state = Convert.ToInt16(childnode.Attributes["state"].Value);
                        clipname = childnode.Attributes["clipname"].Value;
                        if (!stateClip.ContainsKey((short)(GameState.anim_anim + state)))
                        {
                            stateClip.Add((short)(GameState.anim_anim + state), clipname);
                        }
                    }
                }
                if (!modellist.ContainsKey(id))
                    modellist.Add(id, new GameModelConfig(path, position, rotation, scale, stateClip));
            }
        }

        public void parseGameMapConfig()
        {
            xmldoc.Load("Config/mapinfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("map");
            foreach (XmlNode n in nList)
            {
                short id = 1;
                string path = "";
                string bgm = "";
                Vector3 position = new Vector3();
                Vector3 rotation = new Vector3();
                float scale = 1.0f;
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("id"))
                        id = Convert.ToInt16(childnode.InnerText);
                    if (childnode.Name.Equals("path"))
                        path = childnode.InnerText;
                    if (childnode.Name.Equals("bgm"))
                        bgm = childnode.InnerText;
                    if (childnode.Name.Equals("position"))
                    {
                        position.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        position.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        position.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("rotation"))
                    {
                        rotation.X = Convert.ToSingle(childnode.Attributes["x"].Value);
                        rotation.Y = Convert.ToSingle(childnode.Attributes["y"].Value);
                        rotation.Z = Convert.ToSingle(childnode.Attributes["z"].Value);
                    }
                    if (childnode.Name.Equals("scale"))
                        scale = Convert.ToSingle(childnode.InnerText);
                }
                if (!maplist.ContainsKey(id))
                    maplist.Add(id, new GameMapConfig(path, bgm, position, rotation, scale));
            }
        }

        public void parseGameDialogConfig()
        {
            xmldoc.Load("Config/dialoginfo.xml");
            XmlNodeList nList = xmldoc.GetElementsByTagName("dialog");
            foreach (XmlNode n in nList)
            {
                String id = "";
                string path = "";
                foreach (XmlNode childnode in n.ChildNodes)
                {
                    if (childnode.Name.Equals("id"))
                        id = childnode.InnerText;
                    if (childnode.Name.Equals("path"))
                        path = childnode.InnerText;
                }
                if (!dialoglist.ContainsKey(id))
                    dialoglist.Add(id, path);
            }
        }
    }
}
