using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servergame
{
    public class Character
    {
        public int id;
        public int owner;
        public short race;
        public short cls;
        public string name;
        public Equipment equip;
        public int exp;
        public int lvl;
        public Position pos;
        public int hp;
        public int maxhp;
        public int mp;
        public int maxmp;      
        public List<Item> items;

        private void Getlvl(int exp)
        {
            lvl = exp / 10234;
        }
    }
}
