// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageResources
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageResources
{
  private Dictionary<string, object> m_resources;
  internal Dictionary<string, FontStructure> fontCollection = new Dictionary<string, FontStructure>();

  public Dictionary<string, object> Resources => this.m_resources;

  public object this[string key]
  {
    get
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      return this.m_resources.ContainsKey(key) ? this.m_resources[key] : (object) null;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      this.m_resources[key] = value;
    }
  }

  public bool isSameFont()
  {
    int num = 0;
    foreach (KeyValuePair<string, FontStructure> font1 in this.fontCollection)
    {
      foreach (KeyValuePair<string, FontStructure> font2 in this.fontCollection)
      {
        if (font1.Value.FontName != font2.Value.FontName)
          num = 1;
      }
    }
    return num == 0;
  }

  public PdfPageResources() => this.m_resources = new Dictionary<string, object>();

  public void Add(string resourceName, object resource)
  {
    if (string.Equals(resourceName, "ProcSet") || this.m_resources.ContainsKey(resourceName))
      return;
    this.m_resources.Add(resourceName, resource);
    if (!(resource.GetType().Name == "FontStructure"))
      return;
    this.fontCollection.Add(resourceName, resource as FontStructure);
  }

  public bool ContainsKey(string key) => this.m_resources.ContainsKey(key);

  internal void Dispose()
  {
    if (this.m_resources != null)
    {
      this.m_resources.Clear();
      this.m_resources = (Dictionary<string, object>) null;
    }
    if (this.fontCollection == null)
      return;
    this.fontCollection.Clear();
    this.fontCollection = (Dictionary<string, FontStructure>) null;
  }
}
