using System;
using System.Collections.Generic;
using System.Linq;

    public class GameData
    {
        public Statistics PlayerStats { get; set; }

        public ShopItem[] ShopItems;// shop upgrades

        public GameData()
        {
            // for the xml serializer we have to add empty ctor
        }

        public GameData(bool initNewGameData)
        {
            if (initNewGameData)
            {
                this.PlayerStats = new Statistics { Coins = 0, Points = 0, Distance = 0 };
                ShopItems = new ShopItem[ShopConfig.ShopItemsCount]; // for now we have 3 upg

                for (int i = 0; i < ShopItems.Length; i++)
                {
                    ShopItems[i] = new ShopItem();
                    ShopItems[i].Name = ShopConfig.ShopItemNames[i];
                    ShopItems[i].Prices = ShopConfig.ItemsConfigPrices[i];
                    ShopItems[i].Values = ShopConfig.ItemsConfigValues[i];
                    ShopItems[i].UpgradesCount = 0;
                }
            }
        }
    }

    public static class ShopConfig
    {
        public const int ShopItemsCount = 5;
 
        public static string[] ShopItemNames = new string[ShopItemsCount] { "Bladder", "Magnet", "Armor", "Fart", "Wings" };
        public static List<float[]> ItemsConfigValues
        {
            get
            {
                itemsConfigValues = new List<float[]>();
                itemsConfigValues.Add(BladderValues);
                itemsConfigValues.Add(MagnetValues);
                itemsConfigValues.Add(ArmorValues);
                itemsConfigValues.Add(FartValues);
                itemsConfigValues.Add(WingValues);
                return itemsConfigValues;
            }
            set { }
        }
        public static List<int[]> ItemsConfigPrices
        {
            get
            {
                itemsConfigPrices = new List<int[]>();
                itemsConfigPrices.Add(BladderPrices);
                itemsConfigPrices.Add(MagnetPrices);
                itemsConfigPrices.Add(ArmorPrices);
                itemsConfigPrices.Add(FartPrices);
                itemsConfigPrices.Add(WingPrices);
                return itemsConfigPrices;
            }
            set { }
        }

        private static List<int[]> itemsConfigPrices;

        private static List<float[]> itemsConfigValues;

        private static float[] BladderValues = { 0, 0.7f, 1, 2, 2.5f, 4 };

        private static int[] BladderPrices = { 0, 250, 650, 1500, 3700, 6800 };

        private static float[] MagnetValues = { 0, 1.5f, 2.8f, 5, 7.5f, 10 };

        private static int[] MagnetPrices = {0, 250, 650, 1500, 3700, 6800 };

        private static float[] ArmorValues = { 0, 1, 2, 3, 4, 5 };

        private static int[] ArmorPrices = { 0, 250, 650, 1500, 3700, 6800 };

        private static float[] FartValues = { 0, 10, 20, 30, 40, 50 };

        private static int[] FartPrices = { 0, 250, 650, 1500, 3700, 6800 };

        private static float[] WingValues = { 0, 0.5f, 1, 2, 2.5f, 3 };

        private static int[] WingPrices = { 0, 250, 650, 1500, 3700, 6800 };
    }
   

