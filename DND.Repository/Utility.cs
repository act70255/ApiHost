using DND.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Repository
{
    public sealed class Dice
    {
        private static readonly Lazy<Dice> lazy = new Lazy<Dice>(() => new Dice());
        public static Dice Instance { get { return lazy.Value; } }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int Roll(int sides = 20,int start = 0)
        {
            lock (syncLock)
            {
                return random.Next(start + 1, sides + 1);
            }
        }
    }
}
