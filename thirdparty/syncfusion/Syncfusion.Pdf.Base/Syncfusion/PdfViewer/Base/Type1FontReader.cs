// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Type1FontReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Type1FontReader(byte[] data) : PostScriptParser(data)
{
  public static byte[] StripData(byte[] data)
  {
    return Chars.IsValidHexCharacter((int) data[0]) ? new StdFontReader().ReadData(data) : data;
  }
}
