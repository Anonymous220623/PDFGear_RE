// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BuiltinDocumentProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.DocIO.DLS.XML;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BuiltinDocumentProperties : SummaryDocumentProperties
{
  private Dictionary<int, DocumentProperty> m_documentHash = new Dictionary<int, DocumentProperty>();

  public string Category
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1000) ? (string) null : this[BuiltInProperty.Category].Text;
    }
    set
    {
      this.SetPropertyValue(BuiltInProperty.Category, (object) value);
      this[BuiltInProperty.Category].Text = value;
    }
  }

  public int BytesCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1002) ? int.MinValue : this[BuiltInProperty.ByteCount].Int32;
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.ByteCount, (object) value);
      this[BuiltInProperty.ByteCount].Int32 = value;
    }
  }

  public int LinesCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1003) ? int.MinValue : this[BuiltInProperty.LineCount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.LineCount, (object) value);
      this[BuiltInProperty.LineCount].Int32 = value;
    }
  }

  public int ParagraphCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1004) ? int.MinValue : this[BuiltInProperty.ParagraphCount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.ParagraphCount, (object) value);
      this[BuiltInProperty.ParagraphCount].Int32 = value;
    }
  }

  public int SlideCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1005) ? int.MinValue : this[BuiltInProperty.SlideCount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.SlideCount, (object) value);
      this[BuiltInProperty.SlideCount].Int32 = value;
    }
  }

  public int NoteCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1006) ? int.MinValue : this[BuiltInProperty.NoteCount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.NoteCount, (object) value);
      this[BuiltInProperty.NoteCount].Int32 = value;
    }
  }

  public int HiddenCount
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1007) ? int.MinValue : this[BuiltInProperty.HiddenCount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(BuiltInProperty.HiddenCount, (object) value);
      this[BuiltInProperty.HiddenCount].Int32 = value;
    }
  }

  public string Company
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1013) ? (string) null : this[BuiltInProperty.Company].Text;
    }
    set
    {
      this.SetPropertyValue(BuiltInProperty.Company, (object) value);
      this[BuiltInProperty.Company].Text = value;
    }
  }

  public string Manager
  {
    get
    {
      return !this.m_documentHash.ContainsKey(1012) ? (string) null : this[BuiltInProperty.Manager].Text;
    }
    set
    {
      this.SetPropertyValue(BuiltInProperty.Manager, (object) value);
      this[BuiltInProperty.Manager].Text = value;
    }
  }

  internal Dictionary<int, DocumentProperty> DocumentHash => this.m_documentHash;

  internal DocumentProperty this[BuiltInProperty property] => this.m_documentHash[(int) property];

  internal BuiltinDocumentProperties()
    : this(0, 0)
  {
  }

  internal BuiltinDocumentProperties(int docCount, int summCount)
    : base(summCount)
  {
    this.m_documentHash = new Dictionary<int, DocumentProperty>(docCount);
  }

  internal BuiltinDocumentProperties(WordDocument doc)
    : base(doc)
  {
    this.m_documentHash = new Dictionary<int, DocumentProperty>();
  }

  private bool HasKey(int key) => this.m_documentHash.ContainsKey(key);

  public BuiltinDocumentProperties Clone()
  {
    BuiltinDocumentProperties documentProperties = new BuiltinDocumentProperties(this.m_documentHash.Count, this.m_summaryHash.Count);
    foreach (int key in this.m_documentHash.Keys)
    {
      DocumentProperty documentProperty = this.m_documentHash[key];
      documentProperties.m_documentHash.Add(key, documentProperty.Clone());
    }
    foreach (int key in this.m_summaryHash.Keys)
    {
      DocumentProperty documentProperty = this.m_summaryHash[key];
      documentProperties.m_summaryHash.Add(key, documentProperty.Clone());
    }
    foreach (int key in this.m_internalSummaryHash.Keys)
      documentProperties.m_internalSummaryHash.Add(key, this.m_internalSummaryHash[key]);
    return documentProperties;
  }

  internal void SetPropertyValue(BuiltInProperty builtInProperty, object value)
  {
    if (this.m_documentHash.ContainsKey((int) builtInProperty))
    {
      this[builtInProperty].Value = value;
    }
    else
    {
      DocumentProperty documentProperty = new DocumentProperty(builtInProperty, value);
      this.m_documentHash[(int) builtInProperty] = documentProperty;
    }
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(15))
      writer.WriteValue("Company", this.Company);
    if (this.HasKey(14))
      writer.WriteValue("Manager", this.Manager);
    if (this.HasKey(2))
      writer.WriteValue("Category", this.Category);
    if (this.HasKey(4))
      writer.WriteValue("BytesCount", this.BytesCount);
    if (this.HasKey(5))
      writer.WriteValue("LinesCount", this.LinesCount);
    if (this.HasKey(6))
      writer.WriteValue("ParagraphCount", this.ParagraphCount);
    if (this.HasKey(7))
      writer.WriteValue("SlideCount", this.SlideCount);
    if (this.HasKey(8))
      writer.WriteValue("NoteCount", this.NoteCount);
    if (!this.HasKey(9))
      return;
    writer.WriteValue("HiddenCount", this.HiddenCount);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Company"))
      this.Company = reader.ReadString("Company");
    if (reader.HasAttribute("Manager"))
      this.Manager = reader.ReadString("Manager");
    if (reader.HasAttribute("Category"))
      this.Category = reader.ReadString("Category");
    if (reader.HasAttribute("BytesCount"))
      this.SetPropertyValue(BuiltInProperty.ByteCount, (object) reader.ReadInt("BytesCount"));
    if (reader.HasAttribute("LinesCount"))
      this.SetPropertyValue(BuiltInProperty.LineCount, (object) reader.ReadInt("LinesCount"));
    if (reader.HasAttribute("ParagraphCount"))
      this.SetPropertyValue(BuiltInProperty.ParagraphCount, (object) reader.ReadInt("ParagraphCount"));
    if (reader.HasAttribute("SlideCount"))
      this.SetPropertyValue(BuiltInProperty.SlideCount, (object) reader.ReadInt("SlideCount"));
    if (reader.HasAttribute("NoteCount"))
      this.SetPropertyValue(BuiltInProperty.NoteCount, (object) reader.ReadInt("NoteCount"));
    if (!reader.HasAttribute("HiddenCount"))
      return;
    this.SetPropertyValue(BuiltInProperty.HiddenCount, (object) reader.ReadInt("HiddenCount"));
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_documentHash == null)
      return;
    foreach (DocumentProperty documentProperty in this.m_documentHash.Values)
      documentProperty.Close();
    this.m_documentHash.Clear();
    this.m_documentHash = (Dictionary<int, DocumentProperty>) null;
  }
}
