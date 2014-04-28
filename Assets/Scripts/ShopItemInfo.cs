using UnityEngine;

public class ShopItemInfo : MonoBehaviour
{
  public UpgradeLevel.Type type;
  public int level;
  public int priceNextLevel;

  public UILabel priceLabel;
  public UISprite[] starSprites;

  void Start()
  {
    showStars(level);
  }

  void Update()
  {
    showStars(level);
  }

  private void showStars(int n)
  {
    bool isFull = priceNextLevel == 0;
    priceLabel.text = isFull ? "Full" : priceNextLevel.ToString();
    for (int i = 0; i < starSprites.Length; ++i)
      starSprites[i].enabled = i < n;
  }
}
