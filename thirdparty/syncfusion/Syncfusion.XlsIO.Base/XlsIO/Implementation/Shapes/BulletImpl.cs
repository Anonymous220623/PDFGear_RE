// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.BulletImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

internal class BulletImpl
{
  private string m_typeFace = "Wingdings";
  private string m_panose = "05000000000000000000";
  private int m_pitchFamily = 2;
  private int m_charSet = 2;
  private string m_bulletChar = "";
  private static string m_bulletStatic = "";

  internal string TypeFace
  {
    get => this.m_typeFace;
    set => this.m_typeFace = value;
  }

  internal string Panose
  {
    get => this.m_panose;
    set => this.m_panose = value;
  }

  internal int PitchFamily
  {
    get => this.m_pitchFamily;
    set => this.m_pitchFamily = value;
  }

  internal int CharSet
  {
    get => this.m_charSet;
    set => this.m_charSet = value;
  }

  internal string BulletChar
  {
    get => this.m_bulletChar;
    set
    {
      this.m_bulletChar = value;
      BulletImpl.m_bulletStatic = value;
    }
  }

  internal BulletType Type
  {
    get => BulletImpl.GetBulletType(this.m_bulletChar);
    set
    {
      this.m_bulletChar = this.GetBulletChar(value);
      this.SetDefaults(value);
    }
  }

  private void SetDefaults(BulletType type)
  {
    switch (type)
    {
      case BulletType.FilledRound:
        this.m_typeFace = "Arial";
        this.m_panose = "020B0604020202020204";
        this.m_pitchFamily = 34;
        this.m_charSet = 0;
        break;
      case BulletType.HollowRound:
        this.m_typeFace = "Courier New";
        this.m_panose = "02070309020205020404";
        this.m_pitchFamily = 49;
        this.m_charSet = 0;
        break;
      case BulletType.FilledSquare:
      case BulletType.HollowSquare:
      case BulletType.Star:
      case BulletType.Arrow:
      case BulletType.CheckMark:
        if (type != BulletType.CheckMark)
          this.m_panose = "05000000000000000000";
        this.m_typeFace = "Wingdings";
        this.m_pitchFamily = 2;
        this.m_charSet = 2;
        break;
      case BulletType.Custom:
        this.m_typeFace = "Calibri";
        this.m_panose = "020F0502020204030204";
        this.m_pitchFamily = 34;
        this.m_charSet = 0;
        break;
    }
  }

  internal static BulletType GetBulletType(string value)
  {
    if (value == null || value.Trim() == string.Empty)
      return BulletType.None;
    switch (value)
    {
      case "•":
        return BulletType.FilledRound;
      case "o":
        return BulletType.HollowRound;
      case "§":
        return BulletType.FilledSquare;
      case "q":
        return BulletType.HollowSquare;
      case "v":
        return BulletType.Star;
      case "Ø":
        return BulletType.Arrow;
      case "ü":
        return BulletType.CheckMark;
      default:
        return BulletType.Custom;
    }
  }

  internal string GetBulletChar(BulletType type)
  {
    switch (type)
    {
      case BulletType.FilledRound:
        return "•";
      case BulletType.HollowRound:
        return "o";
      case BulletType.FilledSquare:
        return "§";
      case BulletType.HollowSquare:
        return "q";
      case BulletType.Star:
        return "v";
      case BulletType.Arrow:
        return "Ø";
      case BulletType.CheckMark:
        return "ü";
      case BulletType.Custom:
        return BulletImpl.m_bulletStatic;
      default:
        return "";
    }
  }
}
