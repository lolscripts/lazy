using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LazyLucian
{
    internal class Init
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, MiscMenu, DrawMenu;

        public static void LoadMenu()
        {
            Bootstrap.Init(null);

            Menu = MainMenu.AddMenu("Lazy Lucian", "LazyLucian");
            Menu.AddGroupLabel("Lazy Lucian");
            Menu.AddLabel("LolScript.net");
            Menu.AddSeparator();

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *       _____                _             __  __                  
            *      / ____|              | |           |  \/  |                 
            *     | |     ___  _ __ ___ | |__   ___   | \  / | ___ _ __  _   _ 
            *     | |    / _ \| '_ ` _ \| '_ \ / _ \  | |\/| |/ _ \ '_ \| | | |
            *     | |___| (_) | | | | | | |_) | (_) | | |  | |  __/ | | | |_| |
            *      \_____\___/|_| |_| |_|_.__/ \___/  |_|  |_|\___|_| |_|\__,_|
            *                                                                  
            *                                                                  
            */

            ComboMenu = Menu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Configuracion");
            ComboMenu.AddLabel("Q - Luz Lacerante");
            ComboMenu.Add("useQcombo", new CheckBox("Usar en Combo"));
            ComboMenu.Add("useQextended", new CheckBox("Uar Q en Combo"));
            ComboMenu.Add("qMana", new Slider("Min Mna a usar: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("W - Resplandor Ardiente");
            ComboMenu.Add("useWaaRange", new CheckBox("Usar en AA - Rango"));
            ComboMenu.Add("useWalways", new CheckBox("Usar fuera de AA - Rango"));
            ComboMenu.Add("wMana", new Slider("Min Mana a usar: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("E - Presecusion Implacable");
            ComboMenu.Add("useEcombo", new CheckBox("Usar E Logica"));
            ComboMenu.Add("useEmouse", new CheckBox("Usar E con Mouse"));
            ComboMenu.Add("eMana", new Slider("Min Mana a usar: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("R - El Sacrificio");
            ComboMenu.Add("useRkillable", new CheckBox("Usar si el target es matable"));
            ComboMenu.Add("useRlock", new CheckBox("Bloqueo en Target"));
            //ComboMenu.Add("rMana", new Slider("Min Mana to use: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("Otras config (Combo)");
            ComboMenu.Add("spellWeaving", new CheckBox("Usar pasiva (Spellweaving)"));
            ComboMenu.Add("useYoumuu", new CheckBox("Usar Youmuu'"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      _    _                           __  __                  
            *     | |  | |                         |  \/  |                 
            *     | |__| | __ _ _ __ __ _ ___ ___  | \  / | ___ _ __  _   _ 
            *     |  __  |/ _` | '__/ _` / __/ __| | |\/| |/ _ \ '_ \| | | |
            *     | |  | | (_| | | | (_| \__ \__ \ | |  | |  __/ | | | |_| |
            *     |_|  |_|\__,_|_|  \__,_|___/___/ |_|  |_|\___|_| |_|\__,_|
            *                                                               
            *                                                               
            */

            HarassMenu = Menu.AddSubMenu("Tirar", "Tirar");
            HarassMenu.AddGroupLabel("Tirar Config");
            HarassMenu.AddLabel("Q - Luz Lacerante");
            HarassMenu.Add("useQharass", new CheckBox("Usar en tirar"));
            HarassMenu.Add("useQextended", new CheckBox("Usar extendida Q en tirar"));
            HarassMenu.Add("qMana", new Slider("Min Mana a usar: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("W - Resplandor Ardiente");
            HarassMenu.Add("useWaaRange", new CheckBox("Usar en AA - Rango"));
            HarassMenu.Add("useWalways", new CheckBox("Usar fuera de AA - Rango"));
            HarassMenu.Add("wMana", new Slider("Min Mana a usar: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("Otras Config (Tirar)");
            //HarassMenu.Add("manaCheck", new CheckBox("")); // soon(TM)
            HarassMenu.Add("spellWeaving", new CheckBox("Usar Passiva (Spellweaving)"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      ______                     __  __                  
            *     |  ____|                   |  \/  |                 
            *     | |__ __ _ _ __ _ __ ___   | \  / | ___ _ __  _   _ 
            *     |  __/ _` | '__| '_ ` _ \  | |\/| |/ _ \ '_ \| | | |
            *     | | | (_| | |  | | | | | | | |  | |  __/ | | | |_| |
            *     |_|  \__,_|_|  |_| |_| |_| |_|  |_|\___|_| |_|\__,_|
            *                                                         
            *                                                         
            */

            FarmMenu = Menu.AddSubMenu("Farm", "Farm");
            FarmMenu.AddGroupLabel("Farm Config");
            FarmMenu.AddLabel("Q - Luz Lacerante");
            FarmMenu.Add("useQfarm", new CheckBox("Usar en LaneClear"));
            FarmMenu.Add("qManaLane", new Slider("Min Mana a usar en LaneClear: ", 20, 1));
            FarmMenu.Add("qMinionsLane", new Slider("Min Minions a usar en LaneClear: ", 3, 1, 5));
            FarmMenu.AddSeparator();
            FarmMenu.Add("useQjungle", new CheckBox("Usar en JungleClear"));
            FarmMenu.Add("qManaJungle", new Slider("Min Mana a usar en JungleClear: ", 20, 1));
            FarmMenu.AddSeparator();

            FarmMenu.AddLabel("W - Resplandor ardiente");
            FarmMenu.Add("useWfarm", new CheckBox("Usar en LaneClear"));
            FarmMenu.Add("wManaLane", new Slider("Min Mana a usar en LaneClear: ", 20, 1));
            FarmMenu.AddSeparator();
            FarmMenu.Add("useWjungle", new CheckBox("Usar en JungleClear"));
            FarmMenu.Add("wManaJungle", new Slider("Min Mana a usar en JungleClear: ", 20, 1));
            FarmMenu.AddSeparator();

            FarmMenu.AddLabel("Otras config (Farm)");
            FarmMenu.Add("spellWeaving", new CheckBox("Usar Passiva (Spellweaving)"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      __  __ _            __  __                  
            *     |  \/  (_)          |  \/  |                 
            *     | \  / |_ ___  ___  | \  / | ___ _ __  _   _ 
            *     | |\/| | / __|/ __| | |\/| |/ _ \ '_ \| | | |
            *     | |  | | \__ \ (__  | |  | |  __/ | | | |_| |
            *     |_|  |_|_|___/\___| |_|  |_|\___|_| |_|\__,_|
            *                                                  
            *                                                  
            */

            MiscMenu = Menu.AddSubMenu("Otras", "Otras");
            MiscMenu.AddGroupLabel("Diversas Configuraciones");
            MiscMenu.AddLabel("Anti Gapcloser Settings");
            MiscMenu.Add("gapcloser", new CheckBox("Usar E - para evitar no específica"));
            MiscMenu.Add("gapcloserT", new CheckBox("Usar E - para evitar dirigida"));
            MiscMenu.AddSeparator();
            MiscMenu.AddGroupLabel("Otras config");
            MiscMenu.Add("useKs", new CheckBox("Use KillSecure - Logic"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      _____                       __  __                  
            *     |  __ \                     |  \/  |                 
            *     | |  | |_ __ __ ___      __ | \  / | ___ _ __  _   _ 
            *     | |  | | '__/ _` \ \ /\ / / | |\/| |/ _ \ '_ \| | | |
            *     | |__| | | | (_| |\ V  V /  | |  | |  __/ | | | |_| |
            *     |_____/|_|  \__,_| \_/\_/   |_|  |_|\___|_| |_|\__,_|
            *                                                          
            *                                                          
            */

            DrawMenu = Menu.AddSubMenu("Circulos", "Circulos");
            DrawMenu.AddGroupLabel("Circulos Config");
            DrawMenu.AddLabel("Hechizos Rango");
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Rango"));
            DrawMenu.Add("drawQextended", new CheckBox("Draw Extended Q Rango"));
            DrawMenu.Add("drawW", new CheckBox("Draw W Rango"));
            DrawMenu.Add("drawE", new CheckBox("Draw E Rango"));
            DrawMenu.Add("drawR", new CheckBox("Draw R Rango"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      ______               _       
            *     |  ____|             | |      
            *     | |____   _____ _ __ | |_ ___ 
            *     |  __\ \ / / _ \ '_ \| __/ __|
            *     | |___\ V /  __/ | | | |_\__ \
            *     |______\_/ \___|_| |_|\__|___/
            *                                   
            *                                   
            */

            Game.OnUpdate += Events.OnUpdate;
            Gapcloser.OnGapcloser += Events.OnGapCloser;
            Obj_AI_Base.OnProcessSpellCast += Events.OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Events.OnCastSpell;
            Drawing.OnDraw += Events.OnDraw;
            //Orbwalker.OnPostAttack += Events.OnAfterAttack;
        }
    }
}
