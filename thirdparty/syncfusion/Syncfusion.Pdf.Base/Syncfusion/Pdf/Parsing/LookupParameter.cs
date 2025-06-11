// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.LookupParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class LookupParameter
{
  private byte[] data;

  internal byte[] Data => this.data;

  public LookupParameter()
  {
  }

  public LookupParameter(PdfStream stream)
  {
    if (stream == null)
      return;
    this.data = stream.Data;
  }

  public LookupParameter(byte[] lookupData)
  {
    if (lookupData == null)
      return;
    this.data = lookupData;
  }

  internal void Load(PdfStream stream)
  {
    if (stream == null)
      return;
    this.data = stream.Data;
  }

  internal void Load(PdfString str)
  {
    if (str == null)
      return;
    this.data = str.Bytes;
  }

  internal void Load(PdfReferenceHolder indirectObject)
  {
    if (indirectObject.Object is PdfStream stream)
    {
      this.Load(stream);
    }
    else
    {
      if (!(indirectObject.Object is PdfString str))
        return;
      this.Load(str);
    }
  }
}
