// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.SummaryDocumentProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Native;
using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class SummaryDocumentProperties : XDLSSerializableBase
{
  internal const int ContentStatusKey = 1;
  protected Dictionary<int, DocumentProperty> m_summaryHash;
  protected Dictionary<int, string> m_internalSummaryHash;

  public string Author
  {
    get => !this.m_summaryHash.ContainsKey(4) ? (string) null : this[PIDSI.Author].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Author, (object) value);
      this[PIDSI.Author].Text = value;
    }
  }

  public string ApplicationName
  {
    get => !this.m_summaryHash.ContainsKey(18) ? (string) null : this[PIDSI.Appname].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Appname, (object) value);
      this[PIDSI.Appname].Text = value;
    }
  }

  public string Title
  {
    get => !this.m_summaryHash.ContainsKey(2) ? (string) null : this[PIDSI.Title].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Title, (object) value);
      this[PIDSI.Title].Text = value;
    }
  }

  public string Subject
  {
    get => !this.m_summaryHash.ContainsKey(3) ? (string) null : this[PIDSI.Subject].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Subject, (object) value);
      this[PIDSI.Subject].Text = value;
    }
  }

  public string Keywords
  {
    get => !this.m_summaryHash.ContainsKey(5) ? (string) null : this[PIDSI.Keywords].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Keywords, (object) value);
      this[PIDSI.Keywords].Text = value;
    }
  }

  public string Comments
  {
    get => !this.m_summaryHash.ContainsKey(6) ? (string) null : this[PIDSI.Comments].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Comments, (object) value);
      this[PIDSI.Comments].Text = value;
    }
  }

  public string Template
  {
    get => !this.m_summaryHash.ContainsKey(7) ? (string) null : this[PIDSI.Template].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Template, (object) value);
      this[PIDSI.Template].Value = (object) value;
    }
  }

  public string LastAuthor
  {
    get => !this.m_summaryHash.ContainsKey(8) ? (string) null : this[PIDSI.LastAuthor].Text;
    set
    {
      this.SetPropertyValue(PIDSI.LastAuthor, (object) value);
      this[PIDSI.LastAuthor].Text = value;
    }
  }

  public string RevisionNumber
  {
    get => !this.m_summaryHash.ContainsKey(9) ? (string) null : this[PIDSI.Revnumber].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Revnumber, (object) value);
      this[PIDSI.Revnumber].Value = (object) value;
    }
  }

  public TimeSpan TotalEditingTime
  {
    get
    {
      if (!this.m_summaryHash.ContainsKey(10))
        return TimeSpan.MinValue;
      return !(this[PIDSI.EditTime].TimeSpan < TimeSpan.Zero) ? this[PIDSI.EditTime].TimeSpan : TimeSpan.Zero;
    }
    set
    {
      this.SetPropertyValue(PIDSI.EditTime, (object) value);
      this[PIDSI.EditTime].Value = (object) value;
    }
  }

  public DateTime LastPrinted
  {
    get
    {
      return !this.m_summaryHash.ContainsKey(11) ? DateTime.MinValue : this[PIDSI.LastPrinted].DateTime;
    }
    set
    {
      this.SetPropertyValue(PIDSI.LastPrinted, (object) value);
      this[PIDSI.LastPrinted].DateTime = value;
    }
  }

  public DateTime CreateDate
  {
    get => !this.m_summaryHash.ContainsKey(12) ? DateTime.Now : this[PIDSI.Create_dtm].DateTime;
    set
    {
      if (!value.Equals(new DateTime()))
      {
        if (value.CompareTo(new DateTime(1900, 12, 31 /*0x1F*/)) > 0)
        {
          this.SetPropertyValue(PIDSI.Create_dtm, (object) value);
          this[PIDSI.Create_dtm].DateTime = value;
        }
        else if (!this.Document.IsOpening)
          throw new Exception("Date time value must be after 12/31/1900(MM/DD/YYYY).");
      }
      else
      {
        if (!this.m_summaryHash.ContainsKey(12))
          return;
        this.m_summaryHash.Remove(12);
      }
    }
  }

  public DateTime LastSaveDate
  {
    get
    {
      return !this.m_summaryHash.ContainsKey(13) ? this.CreateDate : this[PIDSI.LastSave_dtm].DateTime;
    }
    set
    {
      if (!value.Equals(new DateTime()))
      {
        if (value.CompareTo(new DateTime(1900, 12, 31 /*0x1F*/)) > 0)
        {
          this.SetPropertyValue(PIDSI.LastSave_dtm, (object) value);
          this[PIDSI.LastSave_dtm].DateTime = value;
        }
        else if (!this.Document.IsOpening)
          throw new Exception("Date time value must be after 12/31/1900(MM/DD/YYYY).");
      }
      else
      {
        if (!this.m_summaryHash.ContainsKey(13))
          return;
        this.m_summaryHash.Remove(13);
      }
    }
  }

  public int PageCount
  {
    get => !this.m_summaryHash.ContainsKey(14) ? int.MinValue : this[PIDSI.Pagecount].ToInt();
    internal set
    {
      this.SetPropertyValue(PIDSI.Pagecount, (object) value);
      this[PIDSI.Pagecount].Int32 = value;
    }
  }

  public int WordCount
  {
    get => !this.m_summaryHash.ContainsKey(15) ? int.MinValue : this[PIDSI.Wordcount].ToInt();
    internal set
    {
      this.SetPropertyValue(PIDSI.Wordcount, (object) value);
      this[PIDSI.Wordcount].Int32 = value;
    }
  }

  public int CharCount
  {
    get
    {
      return !this.m_summaryHash.ContainsKey(16 /*0x10*/) ? int.MinValue : this[PIDSI.Charcount].ToInt();
    }
    internal set
    {
      this.SetPropertyValue(PIDSI.Charcount, (object) value);
      this[PIDSI.Charcount].Int32 = value;
    }
  }

  public string Thumbnail
  {
    get => !this.m_summaryHash.ContainsKey(17) ? (string) null : this[PIDSI.Thumbnail].Text;
    set
    {
      this.SetPropertyValue(PIDSI.Thumbnail, (object) value);
      this[PIDSI.Thumbnail].Text = value;
    }
  }

  public int DocSecurity
  {
    get => !this.m_summaryHash.ContainsKey(19) ? int.MinValue : this[PIDSI.Doc_security].ToInt();
    set
    {
      this.SetPropertyValue(PIDSI.Doc_security, (object) value);
      this[PIDSI.Doc_security].Int32 = value;
    }
  }

  internal string ContentStatus
  {
    get => this.HasKeyValue(1) ? this.m_internalSummaryHash[1] : (string) null;
    set => this.SetKeyValue(1, value);
  }

  internal DocumentProperty this[PIDSI pidsi] => this.m_summaryHash[(int) pidsi];

  public int Count => this.m_summaryHash.Count;

  internal Dictionary<int, DocumentProperty> SummaryHash => this.m_summaryHash;

  internal SummaryDocumentProperties()
    : this(0)
  {
  }

  internal SummaryDocumentProperties(int count)
    : base((WordDocument) null, (Entity) null)
  {
    this.m_summaryHash = new Dictionary<int, DocumentProperty>(count);
    this.m_internalSummaryHash = new Dictionary<int, string>();
  }

  internal SummaryDocumentProperties(WordDocument doc)
    : base(doc, (Entity) null)
  {
    this.m_summaryHash = new Dictionary<int, DocumentProperty>();
    this.m_internalSummaryHash = new Dictionary<int, string>();
  }

  private bool HasKey(int key) => this.m_summaryHash.ContainsKey(key);

  public void Add(int key, DocumentProperty props) => this.m_summaryHash.Add(key, props);

  internal void SetPropertyValue(PIDSI pidsi, object value)
  {
    if (this.m_summaryHash.ContainsKey((int) pidsi))
    {
      this[pidsi].Value = value;
    }
    else
    {
      DocumentProperty documentProperty = new DocumentProperty((BuiltInProperty) pidsi, value);
      this.m_summaryHash[(int) pidsi] = documentProperty;
    }
  }

  internal bool HasKeyValue(int Key)
  {
    return this.m_internalSummaryHash != null && this.m_internalSummaryHash.ContainsKey(Key);
  }

  internal void SetKeyValue(int propKey, string value)
  {
    this.m_internalSummaryHash[propKey] = value;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(4))
      writer.WriteValue("Author", this.Author);
    if (this.HasKey(18))
      writer.WriteValue("ApplicationName", this.ApplicationName);
    if (this.HasKey(2))
      writer.WriteValue("Title", this.Title);
    if (this.HasKey(3))
      writer.WriteValue("Subject", this.Subject);
    if (this.HasKey(5))
      writer.WriteValue("Keywords", this.Keywords);
    if (this.HasKey(6))
      writer.WriteValue("Comments", this.Comments);
    if (this.HasKey(7))
      writer.WriteValue("Template", this.Template);
    if (this.HasKey(8))
      writer.WriteValue("LastAuthor", this.LastAuthor);
    if (this.HasKey(9))
      writer.WriteValue("RevisionNumber", this.RevisionNumber);
    if (this.HasKey(10))
      writer.WriteValue("EditTime", this.TotalEditingTime.TotalMinutes.ToString());
    if (this.HasKey(11))
      writer.WriteValue("LastPrinted", this.LastPrinted);
    if (this.HasKey(12))
      writer.WriteValue("CreateDate", this.CreateDate);
    if (this.HasKey(13))
      writer.WriteValue("LastSaveDate", this.LastSaveDate);
    if (this.HasKey(14))
      writer.WriteValue("PageCount", this.PageCount);
    if (this.HasKey(15))
      writer.WriteValue("WordCount", this.WordCount);
    if (this.HasKey(16 /*0x10*/))
      writer.WriteValue("CharCount", this.CharCount);
    if (this.HasKey(17))
      writer.WriteValue("Thumbnail", this.Thumbnail);
    if (!this.HasKey(19))
      return;
    writer.WriteValue("DocSecurity", this.DocSecurity);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Author"))
      this.Author = reader.ReadString("Author");
    if (reader.HasAttribute("ApplicationName"))
      this.ApplicationName = reader.ReadString("ApplicationName");
    if (reader.HasAttribute("Title"))
      this.Title = reader.ReadString("Title");
    if (reader.HasAttribute("Subject"))
      this.Subject = reader.ReadString("Subject");
    if (reader.HasAttribute("Keywords"))
      this.Keywords = reader.ReadString("Keywords");
    if (reader.HasAttribute("Comments"))
      this.Comments = reader.ReadString("Comments");
    if (reader.HasAttribute("Template"))
      this.Template = reader.ReadString("Template");
    if (reader.HasAttribute("LastAuthor"))
      this.LastAuthor = reader.ReadString("LastAuthor");
    if (reader.HasAttribute("RevisionNumber"))
      this.RevisionNumber = reader.ReadString("RevisionNumber");
    if (reader.HasAttribute("EditTime"))
      this.TotalEditingTime = TimeSpan.FromMinutes((double) reader.ReadInt("EditTime"));
    if (reader.HasAttribute("LastPrinted"))
      this.LastPrinted = reader.ReadDateTime("LastPrinted");
    if (reader.HasAttribute("CreateDate"))
      this.CreateDate = reader.ReadDateTime("CreateDate");
    if (reader.HasAttribute("LastSaveDate"))
      this.LastSaveDate = reader.ReadDateTime("LastSaveDate");
    if (reader.HasAttribute("PageCount"))
      this.SetPropertyValue(PIDSI.Pagecount, (object) reader.ReadInt("PageCount"));
    if (reader.HasAttribute("WordCount"))
      this.SetPropertyValue(PIDSI.Wordcount, (object) reader.ReadInt("WordCount"));
    if (reader.HasAttribute("CharCount"))
      this.SetPropertyValue(PIDSI.Charcount, (object) reader.ReadInt("CharCount"));
    if (reader.HasAttribute("Thumbnail"))
      this.Thumbnail = reader.ReadString("Thumbnail");
    if (!reader.HasAttribute("DocSecurity"))
      return;
    this.DocSecurity = reader.ReadInt("DocSecurity");
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_summaryHash != null)
    {
      foreach (DocumentProperty documentProperty in this.m_summaryHash.Values)
        documentProperty.Close();
      this.m_summaryHash.Clear();
      this.m_summaryHash = (Dictionary<int, DocumentProperty>) null;
    }
    if (this.m_internalSummaryHash == null)
      return;
    this.m_internalSummaryHash.Clear();
    this.m_internalSummaryHash = (Dictionary<int, string>) null;
  }
}
