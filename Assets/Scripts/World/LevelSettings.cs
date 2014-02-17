using System;
using UnityEngine;
using UnityEditor;


//[Serializable]
public class LevelSettings : MonoBehaviour
{
	// values
	public int CoinValue = 10;
	public int[] SweatSpotValue = { 10, 30, 100, 150, 300 };
	public ColorPoint point;

	// powerups
	[SettingsList(showSize = false)]
	public int[] Wings;// = { 200, 700, 2000, 3500, 5000 };
	[SettingsList]
	public int[] Rocket = { 250, 1000, 2500, 4500, 8000 };
	[SettingsList]
	public int[] Bladder = { 150, 600, 1800, 4000, 7500 };
//	[SettingsList]
	public int[] Magnet = { 300, 800, 1200, 3500, 6000 };
//	[SettingsList]
	public int[] Amrmor = { 250, 650, 1500, 3700, 6800 };
//	[SettingsList]
	public int[] Catapult = { 0, 500, 700, 1000, 1500 };
//	[SettingsList]
	public int[] Ballista = { 3000, 2500, 3500, 4500, 6000 };
//	[SettingsList]
	public int[] Cannon = { 7500, 6500, 7100, 8000, 10000 };
//	[SettingsList]
	public int[] SweatSpot = { 200, 750, 2100, 3500, 6500 };

}
