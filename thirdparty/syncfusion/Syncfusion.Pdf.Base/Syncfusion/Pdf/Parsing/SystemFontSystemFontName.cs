// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSystemFontName
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontSystemFontName : SystemFontTrueTypeTableBase
{
  internal override uint Tag => SystemFontTags.NAME_TABLE;

  public abstract string FontFamily { get; }

  internal static SystemFontSystemFontName ReadNameTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    SystemFontSystemFontName fontSystemFontName;
    switch (reader.ReadUShort())
    {
      case 0:
        fontSystemFontName = (SystemFontSystemFontName) new SystemFontNameFormat0(fontSource);
        break;
      case 1:
        fontSystemFontName = (SystemFontSystemFontName) new SystemFontNameFormat1(fontSource);
        break;
      default:
        return (SystemFontSystemFontName) null;
    }
    fontSystemFontName.Read(reader);
    return fontSystemFontName;
  }

  public SystemFontSystemFontName(SystemFontOpenTypeFontSourceBase fontSource)
    : base(fontSource)
  {
  }

  internal abstract string ReadName(ushort languageID, ushort nameID);
}
