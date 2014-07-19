using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MMORPGCopierClient
{
    public class GameClassConfig
    {
        public String name = "";
        public String description = "";
        public short modelID = 0;
        public GameClassConfig(String name, String description, short modelID)
        {
            this.name = name;
            this.description = description;
            this.modelID = modelID;
        }
    }
}
