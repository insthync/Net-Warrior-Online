using System;
using System.Collections.Generic;
using System.Text;

namespace MMORPGCopierClient
{
    public class InventoryItemData
    {
        private int itemid;
        private short amount;
        private short refine;
        private int attributeid;
        private int slot1;
        private int slot2;
        private int slot3;
        private int slot4;
        public InventoryItemData(int itemid, short amount, short refine, int attributeid, int slot1, int slot2, int slot3, int slot4)
        {
            this.itemid = itemid;
            this.amount = amount;
            this.refine = refine;
            this.attributeid = attributeid;
            this.slot1 = slot1;
            this.slot2 = slot2;
            this.slot3 = slot3;
            this.slot4 = slot4;
        }
        public int getItemID()
        {
            return itemid;
        }
        public short getAmount()
        {
            return amount;
        }
        public short getRefine()
        {
            return refine;
        }
        public int getAttributeID()
        {
            return attributeid;
        }
        public int getSlot1()
        {
            return slot1;
        }
        public int getSlot2()
        {
            return slot2;
        }
        public int getSlot3()
        {
            return slot3;
        }
        public int getSlot4()
        {
            return slot4;
        }
    }
}
