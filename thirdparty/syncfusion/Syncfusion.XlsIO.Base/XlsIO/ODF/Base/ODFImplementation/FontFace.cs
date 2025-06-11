// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.FontFace
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
