// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfDictionary
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfDictionary : IPdfPrimitive, IPdfChangable
{
  private const string Prefix = "<<";
  private const string Suffix = ">>";
  private static object s_syncLock = new object();
  private Dictionary<PdfName, IPdfPrimitive> m_items;
  private bool m_archive = true;
  private bool m_encrypt;
  private bool m_isDecrypted;
  private bool m_bChanged;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;
  private PdfCrossTable m_crossTable;
  internal PdfDictionary m_clonedObject;
  internal bool isXfa;
  internal bool isSkip;
  private bool isFont;

  public IPdfPrimitive this[PdfName key]
  {
    get
    {
      if (key == (PdfName) null)
        throw new ArgumentNullException(nameof (key));
      return this.m_items.ContainsKey(key) ? this.m_items[key] : (IPdfPrimitive) null;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (key == (PdfName) null)
        throw new ArgumentNullException(nameof (key));
      this.m_items[key] = value;
      this.Modify();
    }
  }

  public IPdfPrimitive this[string key]
  {
    get
    {
      return key != null && !(key == string.Empty) ? this[new PdfName(key)] : throw new ArgumentNullException(nameof (key));
    }
    set
    {
      if (key == null || key == string.Empty)
        throw new ArgumentNullException(nameof (key));
      this[this.GetName(key)] = value;
      this.Modify();
    }
  }

  public int Count => this.m_items.Count;

  public Dictionary<PdfName, IPdfPrimitive>.ValueCollection Values => this.m_items.Values;

  internal bool Archive
  {
    get => this.m_archive;
    set => this.m_archive = value;
  }

  internal bool Encrypt
  {
    get => this.m_encrypt;
    set
    {
      this.m_encrypt = value;
      this.Modify();
    }
  }

  internal bool IsDecrypted
  {
    get => this.m_isDecrypted;
    set => this.m_isDecrypted = value;
  }

  internal Dictionary<PdfName, IPdfPrimitive>.KeyCollection Keys => this.m_items.Keys;

  internal Dictionary<PdfName, IPdfPrimitive> Items => this.m_items;

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal PdfCrossTable CrossTable => this.m_crossTable;

  public virtual IPdfPrimitive ClonedObject => (IPdfPrimitive) this.m_clonedObject;

  internal bool IsFont
  {
    get => this.isFont;
    set => this.isFont = value;
  }

  internal event SavePdfPrimitiveEventHandler BeginSave;

  internal event SavePdfPrimitiveEventHandler EndSave;

  internal PdfDictionary()
  {
    this.m_items = new Dictionary<PdfName, IPdfPrimitive>();
    this.m_encrypt = true;
  }

  internal PdfDictionary(PdfDictionary dictionary)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    this.m_items = new Dictionary<PdfName, IPdfPrimitive>();
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.m_items)
      this.m_items[keyValuePair.Key] = keyValuePair.Value;
    this.Status = dictionary.Status;
    this.FreezeChanges((object) this);
    this.m_encrypt = true;
  }

  public bool ContainsKey(string key) => this.ContainsKey(new PdfName(key));

  public bool ContainsKey(PdfName key) => this.m_items.ContainsKey(key);

  public void Remove(PdfName key)
  {
    if (key == (PdfName) null)
      throw new ArgumentNullException(nameof (key));
    this.m_items.Remove(key);
    this.Modify();
  }

  public void Remove(string key)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    this.Remove(new PdfName(key));
  }

  internal void Clear()
  {
    this.m_items.Clear();
    this.Modify();
  }

  public virtual IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    if (!(this is PdfStream))
    {
      if (this.m_clonedObject != null && this.m_clonedObject.CrossTable == crossTable && !this.IsFont)
        return (IPdfPrimitive) this.m_clonedObject;
      this.m_clonedObject = (PdfDictionary) null;
    }
    PdfDictionary pdfDictionary = new PdfDictionary();
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_items)
    {
      PdfName key = keyValuePair.Key;
      IPdfPrimitive pdfPrimitive = keyValuePair.Value.Clone(crossTable);
      if (!(pdfPrimitive is PdfNull))
        pdfDictionary[key] = pdfPrimitive;
    }
    pdfDictionary.Archive = this.m_archive;
    pdfDictionary.IsDecrypted = this.m_isDecrypted;
    pdfDictionary.Status = this.m_status;
    pdfDictionary.Encrypt = this.m_encrypt;
    pdfDictionary.FreezeChanges((object) this);
    pdfDictionary.m_crossTable = crossTable;
    if (!(this is PdfStream))
      this.m_clonedObject = pdfDictionary;
    return (IPdfPrimitive) pdfDictionary;
  }

  public IPdfPrimitive GetValue(PdfCrossTable crossTable, string key, string parentKey)
  {
    pdfDictionary = this;
    IPdfPrimitive pdfPrimitive = PdfCrossTable.Dereference(pdfDictionary[key]);
    while (pdfPrimitive == null && pdfDictionary != null)
    {
      if (PdfCrossTable.Dereference(pdfDictionary[parentKey]) is PdfDictionary pdfDictionary)
        pdfPrimitive = PdfCrossTable.Dereference(pdfDictionary[key]);
    }
    return pdfPrimitive;
  }

  public IPdfPrimitive GetValue(string key, string parentKey)
  {
    pdfDictionary = this;
    IPdfPrimitive pdfPrimitive = PdfCrossTable.Dereference(pdfDictionary[key]);
    while (pdfPrimitive == null && PdfCrossTable.Dereference(pdfDictionary[parentKey]) is PdfDictionary pdfDictionary)
      pdfPrimitive = PdfCrossTable.Dereference(pdfDictionary[key]);
    return pdfPrimitive;
  }

  internal PdfString GetString(string propertyName)
  {
    return PdfCrossTable.Dereference(this[propertyName]) as PdfString;
  }

  internal int GetInt(string propertyName)
  {
    PdfNumber pdfNumber = PdfCrossTable.Dereference(this[propertyName]) as PdfNumber;
    int num = 0;
    if (pdfNumber != null)
      num = pdfNumber.IntValue;
    return num;
  }

  internal virtual void SaveItems(IPdfWriter writer)
  {
    lock (PdfDictionary.s_syncLock)
    {
      if (writer.Document != null && writer.Document is PdfDocument && writer.Document.FileStructure.Version == PdfVersion.Version2_0 && this.ContainsKey("ProcSet"))
        this.Remove("ProcSet");
      bool flag = false;
      if (writer is PdfWriter && (writer as PdfWriter).isCompress)
        flag = true;
      else
        writer.Write("\r\n");
      if (writer.Document != null && writer.Document.m_isImported && this.ContainsKey("Type"))
      {
        PdfName pdfName = this["Type"] as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value == "SigRef")
          this.Remove(new PdfName("Data"));
      }
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_items)
      {
        PdfName key1 = keyValuePair.Key;
        key1.Save(writer);
        if (!flag || (object) (keyValuePair.Value as PdfName) == null)
          writer.Write(" ");
        IPdfPrimitive pdfPrimitive = keyValuePair.Value;
        if (key1.Value == "Fields")
        {
          PdfArray pdfArray = pdfPrimitive as PdfArray;
          List<PdfReferenceHolder> pdfReferenceHolderList = new List<PdfReferenceHolder>();
          if (pdfArray != null)
          {
            for (int index = 0; index < pdfArray.Count; ++index)
            {
              PdfReferenceHolder element = pdfArray.Elements[index] as PdfReferenceHolder;
              pdfReferenceHolderList.Add(element);
            }
            for (int index1 = 0; index1 < pdfArray.Count; ++index1)
            {
              PdfReferenceHolder element = pdfArray.Elements[index1] as PdfReferenceHolder;
              if (element != (PdfReferenceHolder) null && element.Object is PdfDictionary sender)
              {
                PdfName key2 = new PdfName("Kids");
                if (sender.BeginSave != null)
                {
                  SavePdfPrimitiveEventArgs ars = new SavePdfPrimitiveEventArgs(writer);
                  sender.BeginSave((object) sender, ars);
                }
                if (!sender.ContainsKey(key2) && sender.Items.ContainsKey(new PdfName("FT")) && (sender.Items[new PdfName("FT")] as PdfName).ToString() == "/Sig")
                {
                  for (int index2 = 0; index2 < pdfArray.Count; ++index2)
                  {
                    if (index2 != index1)
                    {
                      PdfDictionary pdfDictionary = (pdfArray.Elements[index2] as PdfReferenceHolder).Object as PdfDictionary;
                      if (pdfDictionary.Items.ContainsKey(new PdfName("T")) && sender.Items.ContainsKey(new PdfName("T")) && (pdfDictionary.Items[new PdfName("T")] as PdfString).Value == (sender.Items[new PdfName("T")] as PdfString).Value)
                        pdfArray.Remove((IPdfPrimitive) element);
                    }
                  }
                }
              }
            }
            pdfPrimitive = (IPdfPrimitive) pdfArray;
          }
        }
        pdfPrimitive.Save(writer);
        if (!flag)
          writer.Write("\r\n");
      }
    }
  }

  protected internal PdfName GetName(string name)
  {
    return name != null ? new PdfName(name) : throw new ArgumentNullException(nameof (name));
  }

  protected virtual void OnBeginSave(SavePdfPrimitiveEventArgs args)
  {
    lock (PdfDictionary.s_syncLock)
    {
      if (this.BeginSave == null)
        return;
      this.BeginSave((object) this, args);
    }
  }

  protected virtual void OnEndSave(SavePdfPrimitiveEventArgs args)
  {
    lock (PdfDictionary.s_syncLock)
    {
      if (this.EndSave == null)
        return;
      this.EndSave((object) this, args);
    }
  }

  public virtual void Save(IPdfWriter writer)
  {
    lock (new object())
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.Save(writer, true);
    }
  }

  internal void Save(IPdfWriter writer, bool bRaiseEvent)
  {
    writer.Write("<<");
    long num = 0;
    if (writer.Document.m_isStreamCopied)
      num = writer.Position;
    if (bRaiseEvent)
      this.OnBeginSave(new SavePdfPrimitiveEventArgs(writer));
    if (writer.Document.m_isStreamCopied && num != writer.Position && num > writer.Position)
      writer.Position = num;
    if (this.Count > 0)
    {
      PdfSecurity security = writer.Document.Security;
      bool enabled = security.Enabled;
      if (!this.m_encrypt)
        security.Enabled = false;
      this.SaveItems(writer);
      if (!this.m_encrypt)
        security.Enabled = enabled;
    }
    writer.Write(">>");
    if (!(writer is PdfWriter) || !(writer as PdfWriter).isCompress)
      writer.Write("\r\n");
    if (!bRaiseEvent)
      return;
    this.OnEndSave(new SavePdfPrimitiveEventArgs(writer));
  }

  internal void SetProperty(string key, IPdfPrimitive primitive)
  {
    if (primitive == null)
      this.m_items.Remove(new PdfName(key));
    else
      this[key] = primitive;
  }

  internal void SetProperty(PdfName key, IPdfPrimitive primitive)
  {
    if (primitive == null)
      this.m_items.Remove(key);
    else
      this[key] = primitive;
  }

  internal void SetProperty(string key, IPdfWrapper wrapper)
  {
    if (wrapper == null)
      this.m_items.Remove(new PdfName(key));
    else
      this.SetProperty(key, wrapper.Element);
  }

  internal static void SetProperty(PdfDictionary dictionary, string key, IPdfWrapper wrapper)
  {
    if (wrapper == null)
      dictionary.Remove(new PdfName(key));
    else
      PdfDictionary.SetProperty(dictionary, key, wrapper.Element);
  }

  internal static void SetProperty(PdfDictionary dictionary, string key, IPdfPrimitive primitive)
  {
    if (primitive == null)
      dictionary.Remove(new PdfName(key));
    else
      dictionary[key] = primitive;
  }

  internal void SetBoolean(string key, bool value)
  {
    if (this[key] is PdfBoolean pdfBoolean)
    {
      pdfBoolean.Value = value;
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfBoolean(value);
  }

  internal void SetNumber(string key, int value)
  {
    if (this[key] is PdfNumber pdfNumber)
    {
      pdfNumber.IntValue = value;
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfNumber(value);
  }

  internal void SetNumber(string key, float value)
  {
    if (this[key] is PdfNumber)
    {
      this[key] = (IPdfPrimitive) new PdfNumber(value);
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfNumber(value);
  }

  internal void SetArray(string key, params IPdfPrimitive[] list)
  {
    if (this[key] is PdfArray pdfArray)
    {
      pdfArray.Clear();
      this.Modify();
    }
    else
    {
      pdfArray = new PdfArray();
      this[key] = (IPdfPrimitive) pdfArray;
    }
    foreach (IPdfPrimitive element in list)
      pdfArray.Add(element);
  }

  internal void SetDateTime(string key, DateTime dateTime)
  {
    if (this[key] is PdfString pdfString)
    {
      pdfString.Value = PdfString.FromDate(dateTime);
      this.Modify();
    }
    else
    {
      int minutes = new DateTimeOffset(dateTime).Offset.Minutes;
      string str1 = minutes.ToString();
      if (minutes >= 0 && minutes <= 9)
        str1 = "0" + str1;
      int hours = new DateTimeOffset(dateTime).Offset.Hours;
      string str2 = hours.ToString();
      if (hours >= 0 && hours <= 9)
        str2 = "0" + str2;
      if (hours < 0)
      {
        if (str2.Length == 2)
          str2 = "-0" + (-hours).ToString();
        this[key] = (IPdfPrimitive) new PdfString($"{PdfString.FromDate(dateTime)}{str2}'{str1}'");
      }
      else
        this[key] = (IPdfPrimitive) new PdfString($"{PdfString.FromDate(dateTime)}+{str2}'{str1}'");
    }
  }

  internal DateTime GetDateTime(PdfString dateTimeStringValue)
  {
    if (dateTimeStringValue == null)
      throw new ArgumentNullException("dateTimeString");
    string str = "D:";
    PdfString pdfString = new PdfString(dateTimeStringValue.Value);
    pdfString.Value = pdfString.Value.Trim('(', ')', 'D', ':');
    if (pdfString.Value.StartsWith("191"))
      pdfString.Value = pdfString.Value.Remove(0, 3).Insert(0, "20");
    bool flag = pdfString.Value.Contains(str);
    string format = "yyyyMMddHHmmss";
    if (pdfString.Value.Contains("/"))
    {
      string[] strArray = pdfString.Value.Split('/');
      if (strArray.Length > 2)
      {
        if (strArray[0].Length <= 2)
        {
          DateTime result = DateTime.Now;
          try
          {
            result = Convert.ToDateTime(pdfString.Value, (IFormatProvider) new DateTimeFormatInfo()
            {
              ShortDatePattern = "MM/dd/yyyy HH:MM TT"
            });
            pdfString.Value = result.ToString("yyyyMMddHHmmss");
          }
          catch
          {
            DateTime.TryParse(pdfString.Value, out result);
            pdfString.Value = result.ToString("yyyyMMddHHmmss");
          }
        }
        else
        {
          DateTime dateTime = Convert.ToDateTime(pdfString.Value);
          pdfString.Value = dateTime.ToString("yyyyMMddHHmmss");
        }
      }
    }
    if (pdfString.Value.Length <= 8)
      format = "yyyyMMdd";
    else if (pdfString.Value.Length <= 10)
      format = "yyyyMMddHH";
    else if (pdfString.Value.Length <= 12)
      format = "yyyyMMddHHmm";
    string s = string.Empty.PadRight(format.Length);
    if (pdfString.Value.Length == 0)
      return DateTime.Now;
    if (pdfString.Value.Length >= s.Length)
      s = flag ? pdfString.Value.Substring(str.Length, s.Length) : pdfString.Value.Substring(0, s.Length);
    DateTime result1 = DateTime.Now;
    DateTime.TryParseExact(s, format, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result1);
    return result1 == new DateTime() ? DateTime.Now : result1;
  }

  internal void SetString(string key, string str)
  {
    if (this[key] is PdfString pdfString)
    {
      pdfString.Value = str;
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfString(str);
  }

  internal static void SetName(PdfDictionary dictionary, string key, string name)
  {
    PdfName pdfName = dictionary[key] as PdfName;
    if (pdfName != (PdfName) null)
    {
      pdfName.Value = name;
      dictionary.Modify();
    }
    else
      dictionary[key] = (IPdfPrimitive) new PdfName(name);
  }

  internal void SetName(string key, string name)
  {
    PdfName pdfName = this[key] as PdfName;
    if (pdfName != (PdfName) null)
    {
      pdfName.Value = name;
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfName(name);
  }

  internal void SetName(string key, string name, bool processSpecialCharacters)
  {
    PdfName pdfName = this[key] as PdfName;
    string str = name.Replace("#", "#23").Replace(" ", "#20").Replace("/", "#2F");
    if (pdfName != (PdfName) null)
    {
      pdfName.Value = str;
      this.Modify();
    }
    else
      this[key] = (IPdfPrimitive) new PdfName(str);
  }

  public bool Changed
  {
    get
    {
      if (!this.m_bChanged)
        this.m_bChanged = this.CheckChanges();
      return this.m_bChanged;
    }
  }

  private bool CheckChanges()
  {
    bool flag = false;
    foreach (IPdfPrimitive pdfPrimitive in this.Values)
    {
      if (pdfPrimitive is IPdfChangable pdfChangable && pdfChangable.Changed)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public void FreezeChanges(object freezer)
  {
    switch (freezer)
    {
      case PdfParser _:
      case PdfDictionary _:
        this.m_bChanged = false;
        break;
    }
  }

  internal void Modify() => this.m_bChanged = true;
}
