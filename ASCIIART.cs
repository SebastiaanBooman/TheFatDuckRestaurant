using System;
using System.Collections.Generic;
using System.Text;

namespace TheFatDuckRestaurant
{
    public static class ASCIIART
    {
        //ASCII Art voor de Fat Duck naam. In losse strings zodat het toevoegen van andere art (bijvoorbeeld een eend) een stuk makkelijker + dynamish is.
        private static string ASCIILogoL1 = "  _______ _            ______    _     _____             _     ";
        private static string ASCIILogoL2 = " |__   __| |          |  ____|  | |   |  __ \\           | |    ";
        private static string ASCIILogoL3 = "    | |  | |__   ___  | |__ __ _| |_  | |  | |_   _  ___| | __ ";
        private static string ASCIILogoL4 = "    | |  | '_ \\ / _ \\ |  __/ _` | __| | |  | | | | |/ __| |/ / ";
        private static string ASCIILogoL5 = "    | |  | | | |  __/ | | | (_| | |_  | |__| | |_| | (__|   <  ";
        private static string ASCIILogoL6 = "    |_|  |_| |_|\\___| |_|  \\__,_|\\__| |_____/ \\__,_|\\___|_|\\_\\ ";

        //ASCII Art voor de Fat Duck Duck.
        private static string ASCIIDuckL1 = "           ..\n";
        private static string ASCIIDuckL2 = "          ( '`<\n";
        private static string ASCIIDuckL3 = "           )(\n";
        private static string ASCIIDuckL4 = "    ( ----'  '.\n";
        private static string ASCIIDuckL5 = "    (         ;\n";
        private static string ASCIIDuckL6 = "     (_______,'\n";

        public static string GeneralArt()
        {
            string ASCIILogoAll = ASCIILogoL1 + ASCIIDuckL1 + ASCIILogoL2 + ASCIIDuckL2 + ASCIILogoL3 + ASCIIDuckL3 + ASCIILogoL4 + ASCIIDuckL4 + ASCIILogoL5 + ASCIIDuckL5 + ASCIILogoL6 + ASCIIDuckL6;
            return ASCIILogoAll;
        }
        public static string MenuArt()
        {
            //ASCII Art voor het Menu
            string ASCIIMenuL1 = "  __  __                  ";
            string ASCIIMenuL2 = " |  \\/  |                 ";
            string ASCIIMenuL3 = " | \\  / | ___ _ __  _   _ ";
            string ASCIIMenuL4 = " | |\\/| |/ _ \\ '_ \\| | | |";
            string ASCIIMenuL5 = " | |  | |  __/ | | | |_| |";
            string ASCIIMenuL6 = " |_|  |_|\\___|_| |_|\\__,_|";

            string ASCIILogoAll = ASCIIMenuL1 + ASCIIDuckL1 + ASCIIMenuL2 + ASCIIDuckL2 + ASCIIMenuL3 + ASCIIDuckL3 + ASCIIMenuL4 + ASCIIDuckL4 + ASCIIMenuL5 + ASCIIDuckL5 + ASCIIMenuL6 + ASCIIDuckL6;
            return ASCIILogoAll;
        }
        public static string LoginArt()
        {
            string ASCIILoginL1 = "  _                 _       ";
            string ASCIILoginL2 = " | |               (_)      ";
            string ASCIILoginL3 = " | |     ___   __ _ _ _ __  ";
            string ASCIILoginL4 = " | |    / _ \\ / _` | | '_ \\ ";
            string ASCIILoginL5 = " | |___| (_) | (_| | | | | |";
            string ASCIILoginL6 = " |______\\___/ \\__, |_|_| |_|";
            string ASCIILoginL7 = "               __/ |        \n";
            string ASCIILoginL8 = "              |___/         \n";

            string ASCIILogoAll = ASCIILoginL1 + ASCIIDuckL1 + ASCIILoginL2 + ASCIIDuckL2 + ASCIILoginL3 + ASCIIDuckL3 + ASCIILoginL4 + ASCIIDuckL4 + ASCIILoginL5 + ASCIIDuckL5 + ASCIILoginL6 + ASCIIDuckL6 + ASCIILoginL7 + ASCIILoginL8;
            return ASCIILogoAll;
        }
        public static string InformatieArt()
        {
            string ASCIIInfoL1 = "  _____        __                           _   _      ";
            string ASCIIInfoL2 = " |_   _|      / _|                         | | (_)     ";
            string ASCIIInfoL3 = "   | |  _ __ | |_ ___  _ __ _ __ ___   __ _| |_ _  ___ ";
            string ASCIIInfoL4 = "   | | | '_ \\|  _/ _ \\| '__| '_ ` _ \\ / _` | __| |/ _ \\";
            string ASCIIInfoL5 = "  _| |_| | | | || (_) | |  | | | | | | (_| | |_| |  __/";
            string ASCIIInfoL6 = " |_____|_| |_|_| \\___/|_|  |_| |_| |_|\\__,_|\\__|_|\\___|";

            string ASCIILogoAll = ASCIIInfoL1 + ASCIIDuckL1 + ASCIIInfoL2 + ASCIIDuckL2 + ASCIIInfoL3 + ASCIIDuckL3 + ASCIIInfoL4 + ASCIIDuckL4 + ASCIIInfoL5 + ASCIIDuckL5 + ASCIIInfoL6 + ASCIIDuckL6;
            return ASCIILogoAll;
        }
        public static string ReserverenArt()
        {
            string ASCIIReserverenL1 = "  _____                                              ";
            string ASCIIReserverenL2 = " |  __ \\                                             ";
            string ASCIIReserverenL3 = " | |__) |___  ___  ___ _ ____   _____ _ __ ___ _ __  ";
            string ASCIIReserverenL4 = " |  _  // _ \\/ __|/ _ \\ '__\\ \\ / / _ \\ '__/ _ \\ '_ \\ ";
            string ASCIIReserverenL5 = " | | \\ \\  __/\\__ \\  __/ |   \\ V /  __/ | |  __/ | | |";
            string ASCIIReserverenL6 = " |_|  \\_\\___||___/\\___|_|    \\_/ \\___|_|  \\___|_| |_|";

            string ASCIILogoAll = ASCIIReserverenL1 + ASCIIDuckL1 + ASCIIReserverenL2 + ASCIIDuckL2 + ASCIIReserverenL3 + ASCIIDuckL3 + ASCIIReserverenL4 + ASCIIDuckL4 + ASCIIReserverenL5 + ASCIIDuckL5 + ASCIIReserverenL6 + ASCIIDuckL6;
            return ASCIILogoAll;
        }
        public static string RegistrerenArt()
        {
            string ASCIIRegistrerenL1 = "  _____            _     _                           ";
            string ASCIIRegistrerenL2 = " |  __ \\          (_)   | |                          ";
            string ASCIIRegistrerenL3 = " | |__) |___  __ _ _ ___| |_ _ __ ___ _ __ ___ _ __  ";
            string ASCIIRegistrerenL4 = " |  _  // _ \\/ _` | / __| __| '__/ _ \\ '__/ _ \\ '_ \\ ";
            string ASCIIRegistrerenL5 = " | | \\ \\  __/ (_| | \\__ \\ |_| | |  __/ | |  __/ | | |";
            string ASCIIRegistrerenL6 = " |_|  \\_\\___|\\__, |_|___/\\__|_|  \\___|_|  \\___|_| |_|";
            string ASCIIRegistrerenL7 = "              __/ |                                  \n";
            string ASCIIRegistrerenL8 = "             |___/                                   \n";

            string ASCIILogoAll = ASCIIRegistrerenL1 + ASCIIDuckL1 + ASCIIRegistrerenL2 + ASCIIDuckL2 + ASCIIRegistrerenL3 + ASCIIDuckL3 + ASCIIRegistrerenL4 + ASCIIDuckL4 + ASCIIRegistrerenL5 + ASCIIDuckL5 + ASCIIRegistrerenL6 + ASCIIDuckL6 + ASCIIRegistrerenL7 + ASCIIRegistrerenL8;
            return ASCIILogoAll;
        }
    }
}
