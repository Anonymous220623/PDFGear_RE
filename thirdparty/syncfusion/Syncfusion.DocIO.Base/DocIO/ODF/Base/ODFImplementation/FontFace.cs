// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.FontFace
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class FontFace
{
  private FontFamilyGeneric m_fontfamilyGeneric;
  private FontPitch m_fontPitch;
  private string m_name;

  internal FontFamilyGeneric FontFamilyGeneric
  {
    get => this.m_fontfamilyGeneric;
    set => this.m_fontfamilyGeneric = value;
  }

  internal FontPitch FontPitch
  {
    get => this.m_fontPitch;
    set => this.m_fontPitch = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal FontFace(string name) => this.m_name = name;

  public override bool Equals(object obj) => this.Name == (obj as FontFace).Name;
}
