using UnityEngine;
using System;
using System.Runtime.InteropServices;

#if UNITY_ANDROID
public class RevMobAndroidBanner : RevMobBanner {
	private AndroidJavaObject javaObject;

	public RevMobAndroidBanner(AndroidJavaObject activity, AndroidJavaObject listener, RevMob.Position position, int x, int y, int w, int h, AndroidJavaObject session) {
		this.javaObject = session;
		this.javaObject.Call("createBanner", activity, listener, (int)position, x, y, w, h);
	}
	
	public override void Show() {
		Debug.Log("BCRS showBanner");
		this.javaObject.Call("showBanner");
    }

    public override void Hide() {
		Debug.Log("BCRS hideBanner");
		this.javaObject.Call("hideBanner");
    }

	public override void Release() {
		Debug.Log("BCRS releaseBanner");
		this.javaObject.Call("releaseBanner");
	}
}
#endif