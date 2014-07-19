using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using TomShane.Neoforce.Controls;

namespace MMORPGCopierClient
{
    public class GUIGameAttribute : Window
    {
        private Manager manager;
        private Network network;
        private Label lvLabel;
        private Label lvValue;
        private Label expLabel;
        private Label expValue;
        private Label strLabel;
        private Label dexLabel;
        private Label intLabel;
        private Label strValue;
        private Label dexValue;
        private Label intValue;
        private Button strBtn;
        private Button dexBtn;
        private Button intBtn;
        private Label stLabel;
        private Label stValue;
        private Label hpLabel;
        private Label spLabel;
        private Label hpValue;
        private Label spValue;
        private Label patkLabel;
        private Label pdefLabel;
        private Label matkLabel;
        private Label mdefLabel;
        private Label patkValue;
        private Label pdefValue;
        private Label matkValue;
        private Label mdefValue;
        private Label atkspdLabel;
        private Label movespdLabel;
        private Label atkspdValue;
        private Label movespdValue;
        public GUIGameAttribute(Manager manager, Network network)
            : base(manager)
        {
            this.manager = manager;
            this.network = network;
            Init();
            Text = "Attribute";
            Width = 170;
            Height = 340;
            Left = 15;
            Top = (manager.ScreenHeight - Height) / 2;
            Alpha = 220;
            Resizable = false;

            lvLabel = new Label(manager);
            lvLabel.Text = "LV:";
            lvLabel.Alignment = Alignment.TopRight;
            lvLabel.Width = 35;
            lvLabel.Height = 20;
            lvLabel.Top = 10;
            lvLabel.Left = 10;
            Add(lvLabel);

            expLabel = new Label(manager);
            expLabel.Text = "Exp:";
            expLabel.Alignment = Alignment.TopRight;
            expLabel.Width = 35;
            expLabel.Height = 20;
            expLabel.Top = 30;
            expLabel.Left = 10;
            //Add(expLabel);

            lvValue = new Label(manager);
            lvValue.Text = "999";
            lvValue.Alignment = Alignment.TopRight;
            lvValue.Height = 20;
            lvValue.Top = 10;
            lvValue.Left = 50;
            Add(lvValue);

            expValue = new Label(manager);
            expValue.Text = "9999999 / 9999999";
            expValue.Alignment = Alignment.TopCenter;
            expValue.Width = 140;
            expValue.Height = 20;
            expValue.Top = 30;
            expValue.Left = 10;
            Add(expValue);

            strLabel = new Label(manager);
            strLabel.Text = "STR:";
            strLabel.Alignment = Alignment.TopRight;
            strLabel.Width = 35;
            strLabel.Height = 20;
            strLabel.Top = 50;
            strLabel.Left = 10;
            Add(strLabel);

            dexLabel = new Label(manager);
            dexLabel.Text = "DEX:";
            dexLabel.Alignment = Alignment.TopRight;
            dexLabel.Width = 35;
            dexLabel.Height = 20;
            dexLabel.Top = 70;
            dexLabel.Left = 10;
            Add(dexLabel);

            intLabel = new Label(manager);
            intLabel.Text = "INT:";
            intLabel.Alignment = Alignment.TopRight;
            intLabel.Width = 35;
            intLabel.Height = 20;
            intLabel.Top = 90;
            intLabel.Left = 10;
            Add(intLabel);

            strValue = new Label(manager);
            strValue.Text = "999";
            strValue.Alignment = Alignment.TopRight;
            strValue.Height = 20;
            strValue.Top = 50;
            strValue.Left = 50;
            Add(strValue);

            dexValue = new Label(manager);
            dexValue.Text = "999";
            dexValue.Alignment = Alignment.TopRight;
            dexValue.Height = 20;
            dexValue.Top = 70;
            dexValue.Left = 50;
            Add(dexValue);

            intValue = new Label(manager);
            intValue.Text = "999";
            intValue.Alignment = Alignment.TopRight;
            intValue.Height = 20;
            intValue.Top = 90;
            intValue.Left = 50;
            Add(intValue);

            strBtn = new Button(manager);
            strBtn.Text = "+";
            intValue.Alignment = Alignment.TopRight;
            strBtn.Width = 15;
            strBtn.Height = 15;
            strBtn.Top = 50;
            strBtn.Left = 120;
            strBtn.Click += new TomShane.Neoforce.Controls.EventHandler(upBtn_Click);
            Add(strBtn);

            dexBtn = new Button(manager);
            dexBtn.Text = "+";
            dexBtn.Width = 15;
            dexBtn.Height = 15;
            dexBtn.Top = 70;
            dexBtn.Left = 120;
            dexBtn.Click += new TomShane.Neoforce.Controls.EventHandler(upBtn_Click);
            Add(dexBtn);

            intBtn = new Button(manager);
            intBtn.Text = "+";
            intBtn.Width = 15;
            intBtn.Height = 15;
            intBtn.Top = 90;
            intBtn.Left = 120;
            intBtn.Click += new TomShane.Neoforce.Controls.EventHandler(upBtn_Click);
            Add(intBtn);

            stLabel = new Label(manager);
            stLabel.Text = "Point:";
            stLabel.Alignment = Alignment.TopRight;
            stLabel.Width = 35;
            stLabel.Height = 20;
            stLabel.Top = 110;
            stLabel.Left = 10;
            Add(stLabel);

            stValue = new Label(manager);
            stValue.Text = "999";
            stValue.Alignment = Alignment.TopRight;
            stValue.Height = 20;
            stValue.Top = 110;
            stValue.Left = 50;
            Add(stValue);

            hpLabel = new Label(manager);
            hpLabel.Text = "HP:";
            hpLabel.Alignment = Alignment.TopRight;
            hpLabel.Width = 35;
            hpLabel.Height = 20;
            hpLabel.Top = 130;
            hpLabel.Left = 10;
            Add(hpLabel);

            spLabel = new Label(manager);
            spLabel.Text = "SP:";
            spLabel.Alignment = Alignment.TopRight;
            spLabel.Width = 35;
            spLabel.Height = 20;
            spLabel.Top = 150;
            spLabel.Left = 10;
            Add(spLabel);

            hpValue = new Label(manager);
            hpValue.Text = "9999 / 9999";
            hpValue.Alignment = Alignment.TopRight;
            hpValue.Width = 85;
            hpValue.Height = 20;
            hpValue.Top = 130;
            hpValue.Left = 50;
            Add(hpValue);

            spValue = new Label(manager);
            spValue.Text = "9999 / 9999";
            spValue.Alignment = Alignment.TopRight;
            spValue.Width = 85;
            spValue.Height = 20;
            spValue.Top = 150;
            spValue.Left = 50;
            Add(spValue);

            patkLabel = new Label(manager);
            patkLabel.Text = "PAtk:";
            patkLabel.Alignment = Alignment.TopRight;
            patkLabel.Width = 35;
            patkLabel.Height = 20;
            patkLabel.Top = 170;
            patkLabel.Left = 10;
            Add(patkLabel);

            pdefLabel = new Label(manager);
            pdefLabel.Text = "PDef:";
            pdefLabel.Alignment = Alignment.TopRight;
            pdefLabel.Width = 35;
            pdefLabel.Height = 20;
            pdefLabel.Top = 190;
            pdefLabel.Left = 10;
            Add(pdefLabel);

            matkLabel = new Label(manager);
            matkLabel.Text = "MAtk:";
            matkLabel.Alignment = Alignment.TopRight;
            matkLabel.Width = 35;
            matkLabel.Height = 20;
            matkLabel.Top = 210;
            matkLabel.Left = 10;
            Add(matkLabel);

            mdefLabel = new Label(manager);
            mdefLabel.Text = "MDef:";
            mdefLabel.Alignment = Alignment.TopRight;
            mdefLabel.Width = 35;
            mdefLabel.Height = 20;
            mdefLabel.Top = 230;
            mdefLabel.Left = 10;
            Add(mdefLabel);

            patkValue = new Label(manager);
            patkValue.Text = "999 ~ 999";
            patkValue.Alignment = Alignment.TopRight;
            patkValue.Width = 85;
            patkValue.Height = 20;
            patkValue.Top = 170;
            patkValue.Left = 50;
            Add(patkValue);

            pdefValue = new Label(manager);
            pdefValue.Text = "999";
            pdefValue.Alignment = Alignment.TopRight;
            pdefValue.Width = 85;
            pdefValue.Height = 20;
            pdefValue.Top = 190;
            pdefValue.Left = 50;
            Add(pdefValue);

            matkValue = new Label(manager);
            matkValue.Text = "999 ~ 999";
            matkValue.Alignment = Alignment.TopRight;
            matkValue.Width = 85;
            matkValue.Height = 20;
            matkValue.Top = 210;
            matkValue.Left = 50;
            Add(matkValue);

            mdefValue = new Label(manager);
            mdefValue.Text = "999";
            mdefValue.Alignment = Alignment.TopRight;
            mdefValue.Width = 85;
            mdefValue.Height = 20;
            mdefValue.Top = 230;
            mdefValue.Left = 50;
            Add(mdefValue);

            atkspdLabel = new Label(manager);
            atkspdLabel.Text = "aspd:";
            atkspdLabel.Alignment = Alignment.TopRight;
            atkspdLabel.Width = 35;
            atkspdLabel.Height = 20;
            atkspdLabel.Top = 250;
            atkspdLabel.Left = 10;
            Add(atkspdLabel);

            movespdLabel = new Label(manager);
            movespdLabel.Text = "mspd:";
            movespdLabel.Alignment = Alignment.TopRight;
            movespdLabel.Width = 35;
            movespdLabel.Height = 20;
            movespdLabel.Top = 270;
            movespdLabel.Left = 10;
            Add(movespdLabel);

            atkspdValue = new Label(manager);
            atkspdValue.Text = "999";
            atkspdValue.Alignment = Alignment.TopRight;
            atkspdValue.Width = 85;
            atkspdValue.Height = 20;
            atkspdValue.Top = 250;
            atkspdValue.Left = 50;
            Add(atkspdValue);

            movespdValue = new Label(manager);
            movespdValue.Text = "999";
            movespdValue.Alignment = Alignment.TopRight;
            movespdValue.Width = 85;
            movespdValue.Height = 20;
            movespdValue.Top = 270;
            movespdValue.Left = 50;
            Add(movespdValue);

        }

        void upBtn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (sender.Equals(this.strBtn) || sender.Equals(this.dexBtn) || sender.Equals(this.intBtn))
            {
                if (sender.Equals(this.strBtn))
                {
                    // Send to server that player up str
                    // Send chat message
                    if (network.isConnected())
                        network.Send("ADDSTR:1;");
                }
                if (sender.Equals(this.dexBtn))
                {
                    // Send to server that player up dex
                    // Send chat message
                    if (network.isConnected())
                        network.Send("ADDDEX:1;");
                }
                if (sender.Equals(this.intBtn))
                {
                    // Send to server that player up int
                    // Send chat message
                    if (network.isConnected())
                        network.Send("ADDINT:1;");
                }
            }
        }

        public void setValues(CharacterInformation charinfo)
        {
            short _level = charinfo.getChar_level();
            int _curexp = charinfo.getChar_curexp();
            int _exp = charinfo.getChar_exp();
            short _str = charinfo.getChar_str();
            short _dex = charinfo.getChar_dex();
            short _int = charinfo.getChar_int();
            lvValue.Text = "" + _level;
            expValue.Text = _curexp + " / " + _exp;
            strValue.Text = "" + _str;
            dexValue.Text = "" + _dex;
            intValue.Text = "" + _int;
            stValue.Text = "" + charinfo.getChar_stpoint();
            if (charinfo.getChar_stpoint() <= 0)
            {
                strBtn.Enabled = false;
                dexBtn.Enabled = false;
                intBtn.Enabled = false;
            }
            else
            {
                strBtn.Enabled = true;
                dexBtn.Enabled = true;
                intBtn.Enabled = true;
            }
            hpValue.Text = charinfo.getChar_curhp() + " / " + hpMultiplier(_level, _str, _dex, _int);
            spValue.Text = charinfo.getChar_cursp() + " / " + spMultiplier(_level, _str, _dex, _int);
            patkValue.Text = patkMultiplier(_level, _str, _dex, _int)[0] + " ~ " + patkMultiplier(_level, _str, _dex, _int)[1];
            pdefValue.Text = "" + pdefMultiplier(_level, _str, _dex, _int);
            matkValue.Text = matkMultiplier(_level, _str, _dex, _int)[0] + " ~ " + matkMultiplier(_level, _str, _dex, _int)[1];
            mdefValue.Text = "" + mdefMultiplier(_level, _str, _dex, _int);
            atkspdValue.Text = "" + atkspd(_level, _str, _dex, _int);
            movespdValue.Text = "" + movespd(_level, _str, _dex, _int);
        }
        private int hpMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            return strength * 3;
        }
        private int spMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            return intelligent * 2;
        }
        private int[] patkMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            int[] atk = new int[2];
            atk[0] = (int)(strength * 1.25);
            atk[1] = (int)(strength * 1.5);
            return atk;
        }
        private int[] matkMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            int[] atk = new int[2];
            atk[0] = (int)(intelligent * 1.25);
            atk[1] = (int)(intelligent * 1.5);
            return atk;
        }
        private int pdefMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            return strength * 1 + (int)(dexterity * 0.5);
        }
        private int mdefMultiplier(short lv, short strength, short dexterity, short intelligent)
        {
            return intelligent * 1 + (int)(dexterity * 0.5);
        }
        private int movespd(short lv, short strength, short dexterity, short intelligent)
        {
            return (int)(dexterity * 0.05) + 1;
        }
        private int atkspd(short lv, short strength, short dexterity, short intelligent)
        {
            return (int)(dexterity * 0.05) + 1;
        }
    }
}
