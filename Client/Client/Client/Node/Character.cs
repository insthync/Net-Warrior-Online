using System;
using System.Collections.Generic;
using System.Text;

namespace MMORPGCopierClient
{

    public class CharacterInformation
    {
        private int charid;
        private String char_name;
        private short char_class;
        private short char_level;
        private int char_curexp;
        private int char_exp;
        private int char_gold;
        private int char_hp;
        private int char_curhp;
        private int char_sp;
        private int char_cursp;
        private int char_stpoint;
        private int char_skpoint;
        private short char_str;
        private short char_dex;
        private short char_int;
        private short char_hair;
        private short char_face;
        private short char_curmap;
        private float char_curmapx;
        private float char_curmapy;
        private short char_savmap;
        private float char_savmapx;
        private float char_savmapy;

        public CharacterInformation(int charid, String char_name,
				short char_class, short char_level, int char_exp, int char_curexp, int char_gold,
				int char_hp, int char_curhp, int char_sp, int char_cursp, int char_stpoint, int char_skpoint,
				short char_str, short char_dex, short char_int, short char_hair, short char_face,
                short char_curmap, float char_curmapx, float char_curmapy,
                short char_savmap, float char_savmapx, float char_savmapy) {
			this.charid = charid;
			this.char_name = char_name;
			this.char_class = char_class;
			this.char_level = char_level;
			this.char_exp = char_exp;
            this.char_curexp = char_curexp;
			this.char_gold = char_gold;
            this.char_hp = char_hp;
            this.char_curhp = char_curhp;
            this.char_sp = char_sp;
			this.char_cursp = char_cursp;
			this.char_stpoint = char_stpoint;
			this.char_skpoint = char_skpoint;
			this.char_str = char_str;
			this.char_dex = char_dex;
			this.char_int = char_int;
			this.char_hair = char_hair;
			this.char_face = char_face;
			this.char_curmap = char_curmap;
			this.char_curmapx = char_curmapx;
			this.char_curmapy = char_curmapy;
			this.char_savmap = char_savmap;
			this.char_savmapx = char_savmapx;
			this.char_savmapy = char_savmapy;
		}
		
		public int getCharid() {
			return charid;
		}
		public String getChar_name() {
			return char_name;
		}
        public short getChar_class()
        {
			return char_class;
		}
        public short getChar_level()
        {
			return char_level;
		}
        public int getChar_exp()
        {
            return char_exp;
        }
        public int getChar_curexp()
        {
            return char_curexp;
        }
		public int getChar_gold() {
			return char_gold;
		}
		public int getChar_hp() {
			return char_hp;
        }
        public int getChar_curhp()
        {
            return char_curhp;
        }
        public int getChar_sp()
        {
            return char_sp;
        }
		public int getChar_cursp() {
			return char_cursp;
		}
		public int getChar_stpoint() {
			return char_stpoint;
		}
		public int getChar_skpoint() {
			return char_skpoint;
		}
        public short getChar_str()
        {
			return char_str;
		}
        public short getChar_dex()
        {
			return char_dex;
		}
        public short getChar_int()
        {
			return char_int;
		}
        public short getChar_hair()
        {
			return char_hair;
		}
        public short getChar_face()
        {
			return char_face;
		}
		public short getChar_curmap() {
			return char_curmap;
		}
		public float getChar_curmapx() {
			return char_curmapx;
		}
		public float getChar_curmapy() {
			return char_curmapy;
		}
		public short getChar_savmap() {
			return char_savmap;
		}
		public float getChar_savmapx() {
			return char_savmapx;
		}
		public float getChar_savmapy() {
			return char_savmapy;
		}
    }
}
