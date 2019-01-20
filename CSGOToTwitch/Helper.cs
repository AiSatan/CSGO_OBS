using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSGOToTwitch.Program;

namespace CSGOToTwitch
{
    public static class Helper
    {
        public static string GifToString(this Gifs gif)
        {
            return ((int)gif).ToString();
        }
    }
}
