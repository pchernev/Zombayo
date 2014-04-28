public class DogHead : NPC
{
  private enum AnimDesc
  {
    Idle,
    Count
  }

  private static readonly string[] animDescriptions;
  protected override string[] getDescriptionTexts() { return animDescriptions; }

  static DogHead()
  {
    animDescriptions = new string[(int)AnimDesc.Count];
    for (int i = 0; i < (int)AnimDesc.Count; ++i)
      animDescriptions[i] = ((AnimDesc)i).ToString();
  }
}
