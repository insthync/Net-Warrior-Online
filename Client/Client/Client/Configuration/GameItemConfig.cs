using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MMORPGCopierClient
{
    public class GameItemConfig
    {
        // Drop model
        public String dropModelPath = "";
        public Vector3 dropPosition = new Vector3(0, 0, 0);
        public Vector3 dropRotation = new Vector3(0, 0, 0);
        public float dropScale = 1;
        // Information
        public String name = "";
        public String description = "";
        public short equipModelID = 0;
        public String inventoryTexturePath = "";
        public GameItemConfig(String dropModelPath, Vector3 dropPosition, Vector3 dropRotation, float dropScale, String name, String description, short equipModelID, String inventoryTexturePath)
        {
            // Drop model
            this.dropModelPath = dropModelPath;
            this.dropPosition = dropPosition;
            this.dropRotation = dropRotation;
            this.dropScale = dropScale;
            // Information
            this.name = name;
            this.description = description;
            this.equipModelID = equipModelID;
            this.inventoryTexturePath = inventoryTexturePath;
        }
    }
}
