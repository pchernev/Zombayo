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
        // public int coinValue = 10;
        public const int ShopItemsCount = 3; // for now we have 3

        public static string[] ShopItemNames = new string[ShopItemsCount] { "Wings", "Magnet", "Armor" };

        public static List<float[]> ItemsConfigValues
        {
            get
            {
                itemsConfigValues = new List<float[]>();
                itemsConfigValues.Add(WingsValues);
                itemsConfigValues.Add(MagnetValues);
                itemsConfigValues.Add(ArmorValues);
                return itemsConfigValues;
            }
            set { }
        }
        public static List<int[]> ItemsConfigPrices
        {
            get
            {
                itemsConfigPrices = new List<int[]>();
                itemsConfigPrices.Add(WingsPrices);
                itemsConfigPrices.Add(MagnetPrices);
                itemsConfigPrices.Add(ArmorPrices);
                return itemsConfigPrices;
            }
            set { }
        }

        private static List<int[]> itemsConfigPrices;

        private static List<float[]> itemsConfigValues;

        private static float[] WingsValues = { 0, 0.5f, 1, 2, 2.5f, 3 };

        private static int[] WingsPrices = { 0, 250, 650, 1500, 3700, 6800 };

        private static float[] MagnetValues = { 0, 0.5f, 1, 2, 2.5f, 3 };

        private static int[] MagnetPrices = {0, 250, 650, 1500, 3700, 6800 };

        private static float[] ArmorValues = { 0, 250, 650, 1500, 3700, 6800 };

        private static int[] ArmorPrices = { 0, 250, 650, 1500, 3700, 6800 };

    }
   

