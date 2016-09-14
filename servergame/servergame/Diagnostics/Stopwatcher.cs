using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servergame
{
    public class SW
    {
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        public SW()
        {
            watch.Start();
        }
        public void start()
        {
            watch.Start();
        }
        public long stop()
        {
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
