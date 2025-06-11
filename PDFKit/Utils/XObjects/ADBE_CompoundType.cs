// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.XObjects.ADBE_CompoundType
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.Utils.XObjects;

public class ADBE_CompoundType
{
  public ADBE_CompoundType(string @private, string lastModified, string docSettings)
  {
    this.Private = @private;
    this.LastModified = lastModified;
    this.DocSettings = docSettings;
  }

  public string Private { get; }

  public string LastModified { get; }

  public string DocSettings { get; }

  public bool IsHeaderFooter => this.Private == "Header" || this.Private == "Footer";
}
