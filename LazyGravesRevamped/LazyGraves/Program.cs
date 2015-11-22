using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace LazyGraves
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName == ("Graves"))
                Init.LoadMenu();
        }
    }
}
