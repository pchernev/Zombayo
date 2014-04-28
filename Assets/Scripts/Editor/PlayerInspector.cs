using UnityEditor;

[CustomEditor(typeof(Player), true)]
public class PlayerInspector : Editor
{
  private Player player;

  void OnEnable()
  {
    player = target as Player;
    player.ensureDataConsistency();
  }
}
