// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.PDFExtStampDictionary
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Utils.StampUtils.StampTemplates;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Utils.StampUtils;

public class PDFExtStampDictionary
{
  public string Type { get; set; }

  public string Template { get; set; }

  public string Content { get; set; }

  internal bool AllPropertiesAreDefault
  {
    get
    {
      return string.IsNullOrEmpty(this.Type) && string.IsNullOrEmpty(this.Template) && string.IsNullOrEmpty(this.Content);
    }
  }

  public void SetContentDictionary(Dictionary<string, string> contentDict)
  {
    this.Content = ContentSerializer.Serialize(contentDict);
  }

  public Dictionary<string, string> GetContentDictionary()
  {
    return ContentSerializer.Deserialize(this.Content);
  }
}
