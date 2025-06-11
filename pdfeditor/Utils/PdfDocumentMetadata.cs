// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfDocumentMetadata
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using XmpCore;
using XmpCore.Options;

#nullable disable
namespace pdfeditor.Utils;

public class PdfDocumentMetadata
{
  private readonly PdfDocument document;

  public PdfDocumentMetadata(PdfDocument document)
  {
    this.document = document;
    this.ThrowIfDisposed();
  }

  public int PdfFileVersion
  {
    get
    {
      PdfTypeDictionary root = this.document.Root;
      if (root.ContainsKey("Version") && root["Version"].Is<PdfTypeName>())
      {
        string str = root["Version"].As<PdfTypeName>().Value;
        int pdfFileVersion = 0;
        for (int index = 0; index < str.Length; ++index)
        {
          switch (str[index])
          {
            case '.':
              continue;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
              pdfFileVersion = pdfFileVersion * 10 + ((int) str[index] - 48 /*0x30*/);
              continue;
            default:
              pdfFileVersion = 0;
              goto label_7;
          }
        }
label_7:
        if (pdfFileVersion > 0)
          return pdfFileVersion;
      }
      return this.document.Version;
    }
    set
    {
      if (value < 10 || value >= 100)
        return;
      this.document.Root["Version"] = (PdfTypeBase) PdfTypeName.Create($"{value / 10}.{value % 10}");
    }
  }

  public string Title
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Title, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Title, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string Description
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Description, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Description, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string[] Author
  {
    get
    {
      return this.GetMetaValue<string[]>(PdfDocumentMetadataConstants.Author, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Author, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string Subject
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Subject, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Subject, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string Keywords
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Keywords, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      string str = "";
      string[] strArray = Array.Empty<string>();
      if (!string.IsNullOrEmpty(value))
      {
        str = value.Replace("\r", "").Replace("\n", " ").Trim();
        string[] source = str.Split(',', ';', '，', '；');
        if (source.Length != 0)
          strArray = ((IEnumerable<string>) source).Select<string, string>((Func<string, string>) (c => c.Trim())).Where<string>((Func<string, bool>) (c => !string.IsNullOrWhiteSpace(c))).ToArray<string>();
      }
      if (!this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Keywords, CultureInfo.InvariantCulture, (object) str))
        return;
      PdfDocumentMetadata.MetaPropertyInfo<string[]> propertyInfo = new PdfDocumentMetadata.MetaPropertyInfo<string[]>();
      propertyInfo.XmpPropertyKey = "subject";
      propertyInfo.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      propertyInfo.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.ArrayAlternate;
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) propertyInfo, CultureInfo.InvariantCulture, (object) strArray);
    }
  }

  public string[] XmpSubjects
  {
    get
    {
      PdfDocumentMetadata.MetaPropertyInfo<string[]> propertyInfo = new PdfDocumentMetadata.MetaPropertyInfo<string[]>();
      propertyInfo.XmpPropertyKey = "subject";
      propertyInfo.XmpPropertyNameSpace = "http://purl.org/dc/elements/1.1/";
      propertyInfo.XmpPropertyType = PdfDocumentMetadata.XmpPropertyType.ArrayAlternate;
      return this.GetMetaValue<string[]>(propertyInfo, CultureInfo.CurrentUICulture, out bool _) ?? Array.Empty<string>();
    }
  }

  public string Creator
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Creator, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Creator, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string Producer
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Producer, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Producer, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public DateTimeOffset CreationDate
  {
    get
    {
      return this.GetMetaValue<DateTimeOffset>(PdfDocumentMetadataConstants.CreationDate, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.CreationDate, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public DateTimeOffset ModificationDate
  {
    get
    {
      return this.GetMetaValue<DateTimeOffset>(PdfDocumentMetadataConstants.ModificationDate, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.ModificationDate, CultureInfo.InvariantCulture, (object) value);
    }
  }

  public string Trapped
  {
    get
    {
      return this.GetMetaValue<string>(PdfDocumentMetadataConstants.Trapped, CultureInfo.CurrentUICulture, out bool _);
    }
    set
    {
      this.SetMetaValue((PdfDocumentMetadata.MetaPropertyInfo) PdfDocumentMetadataConstants.Trapped, CultureInfo.InvariantCulture, (object) value);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public T GetMetaValue<T>(
    PdfDocumentMetadata.MetaPropertyInfo<T> propertyInfo,
    CultureInfo cultureInfo,
    out bool isXmpProperty)
  {
    object metaValueCore = this.GetMetaValueCore((PdfDocumentMetadata.MetaPropertyInfo) propertyInfo, cultureInfo, out isXmpProperty);
    return metaValueCore == null ? default (T) : (T) metaValueCore;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public object GetMetaValue(
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    out bool isXmpProperty)
  {
    return this.GetMetaValueCore(propertyInfo, cultureInfo, out isXmpProperty);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool SetMetaValue(
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    object value)
  {
    return this.SetMetaValueCore(propertyInfo, cultureInfo, value);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool SetMetaText<T>(
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    T value)
  {
    return this.SetMetaValueCore(propertyInfo, cultureInfo, (object) value);
  }

  public bool RemoveMetaValue(PdfDocumentMetadata.MetaPropertyInfo propertyInfo)
  {
    this.ThrowIfDisposed();
    if (propertyInfo == (PdfDocumentMetadata.MetaPropertyInfo) null)
      return false;
    IXmpMeta xmpMeta = this.GetXmpMeta() ?? XmpMetaFactory.Create();
    bool flag = false;
    if (!string.IsNullOrEmpty(propertyInfo.XmpPropertyKey) && propertyInfo.XmpPropertyType != PdfDocumentMetadata.XmpPropertyType.None && xmpMeta.GetProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey) != null)
    {
      xmpMeta.DeleteProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
      if (!this.SetXmpMeta(xmpMeta))
        return false;
      flag = true;
    }
    if (propertyInfo.DocumentTag.HasValue)
      Pdfium.FPDF_SetMetaText(this.document.Handle, propertyInfo.DocumentTag.Value, (string) null);
    return flag;
  }

  [DllImport("pdfium", EntryPoint = "FPDF_GetMetaText", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern int FPDF_GetMetaText_native(
    IntPtr document,
    [MarshalAs(UnmanagedType.LPStr)] string tag,
    [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
    int buflen);

  private object GetMetaValueCore(
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    out bool isXmpProperty)
  {
    this.ThrowIfDisposed();
    isXmpProperty = false;
    object xmpValue = this.GetXmpValue(this.GetXmpMeta(), propertyInfo, cultureInfo);
    if (xmpValue != null)
    {
      isXmpProperty = true;
      return xmpValue;
    }
    string metaText = Pdfium.FPDF_GetMetaText(this.document.Handle, propertyInfo.InfoDictionaryKey);
    switch (propertyInfo.XmpPropertyType)
    {
      case PdfDocumentMetadata.XmpPropertyType.String:
      case PdfDocumentMetadata.XmpPropertyType.LocalizedString:
        return (object) metaText ?? (object) string.Empty;
      case PdfDocumentMetadata.XmpPropertyType.ArrayOrdered:
      case PdfDocumentMetadata.XmpPropertyType.ArrayAlternate:
        if (metaText == null)
          return (object) Array.Empty<string>();
        return (object) new string[1]{ metaText };
      case PdfDocumentMetadata.XmpPropertyType.DateTimeOffset:
        DateTimeOffset dateTime;
        return PdfObjectExtensions.TryParseModificationDate(metaText, out dateTime) ? (object) dateTime : (object) null;
      default:
        return (object) null;
    }
  }

  private bool SetMetaValueCore(
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    object value)
  {
    this.ThrowIfDisposed();
    if (propertyInfo == (PdfDocumentMetadata.MetaPropertyInfo) null)
      return false;
    IXmpMeta xmpMeta = this.GetXmpMeta() ?? XmpMetaFactory.Create();
    if (!string.IsNullOrEmpty(propertyInfo.XmpPropertyKey) && propertyInfo.XmpPropertyType != PdfDocumentMetadata.XmpPropertyType.None && (!this.SetXmpValue(xmpMeta, propertyInfo, cultureInfo, value, string.IsNullOrEmpty(cultureInfo?.Name)) || !this.SetXmpMeta(xmpMeta)))
      return false;
    if (propertyInfo.DocumentTag.HasValue)
    {
      string text = "";
      if (value != null)
      {
        switch (propertyInfo.XmpPropertyType)
        {
          case PdfDocumentMetadata.XmpPropertyType.ArrayOrdered:
          case PdfDocumentMetadata.XmpPropertyType.ArrayAlternate:
            string[] source = (string[]) value;
            text = (source != null ? ((IEnumerable<string>) source).FirstOrDefault<string>() : (string) null) ?? "";
            break;
          case PdfDocumentMetadata.XmpPropertyType.DateTimeOffset:
            text = ((DateTimeOffset) value).ToModificationDateString();
            break;
          default:
            text = value.ToString();
            break;
        }
      }
      Pdfium.FPDF_SetMetaText(this.document.Handle, propertyInfo.DocumentTag.Value, text);
    }
    return true;
  }

  private bool SetXmpValue(
    IXmpMeta xmpMeta,
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo,
    object value,
    bool removeOtherLanguageValues)
  {
    if (xmpMeta == null || propertyInfo == (PdfDocumentMetadata.MetaPropertyInfo) null || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.None || (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.String || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.LocalizedString) && !(value is string) || (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayOrdered || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayAlternate) && !(value is string[]) || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.DateTimeOffset && !(value is DateTimeOffset))
      return false;
    if (!string.IsNullOrEmpty(propertyInfo.XmpPropertyKey))
    {
      try
      {
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.String)
        {
          xmpMeta.SetProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, value);
          return true;
        }
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.LocalizedString)
        {
          if (removeOtherLanguageValues)
            xmpMeta.DeleteProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
          string genericLang;
          string specificLang;
          this.GetLanguages(cultureInfo, out genericLang, out specificLang);
          xmpMeta.SetLocalizedText(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, genericLang, specificLang, (string) value);
          return true;
        }
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayOrdered || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayAlternate)
        {
          xmpMeta.DeleteProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
          string[] strArray = (string[]) value;
          xmpMeta.SetProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, (object) null, new PropertyOptions()
          {
            IsArray = true,
            IsArrayAlternate = propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayAlternate,
            IsArrayOrdered = propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayOrdered
          });
          for (int index = 0; index < strArray.Length; ++index)
            xmpMeta.InsertArrayItem(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, index + 1, strArray[index]);
          return true;
        }
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.DateTimeOffset)
        {
          string propValue = PdfDocumentMetadata.FormatISO8601((DateTimeOffset) value);
          xmpMeta.SetProperty(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, (object) propValue);
          return true;
        }
      }
      catch
      {
      }
    }
    return false;
  }

  private object GetXmpValue(
    IXmpMeta xmpMeta,
    PdfDocumentMetadata.MetaPropertyInfo propertyInfo,
    CultureInfo cultureInfo)
  {
    if (xmpMeta == null || propertyInfo == (PdfDocumentMetadata.MetaPropertyInfo) null)
      return (object) null;
    if (!string.IsNullOrEmpty(propertyInfo.XmpPropertyKey))
    {
      try
      {
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.LocalizedString)
        {
          string genericLang;
          string specificLang;
          this.GetLanguages(cultureInfo, out genericLang, out specificLang);
          return (object) xmpMeta.GetLocalizedText(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, genericLang, specificLang)?.Value;
        }
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.String)
          return (object) xmpMeta.GetPropertyString(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayOrdered || propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.ArrayAlternate)
        {
          int capacity = xmpMeta.CountArrayItems(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
          List<string> stringList = new List<string>(capacity);
          for (int itemIndex = 1; itemIndex <= capacity; ++itemIndex)
          {
            string str = xmpMeta.GetArrayItem(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey, itemIndex)?.Value;
            if (!string.IsNullOrEmpty(str))
              stringList.Add(str);
          }
          return stringList.Count == 0 ? (object) (string[]) null : (object) stringList.ToArray();
        }
        if (propertyInfo.XmpPropertyType == PdfDocumentMetadata.XmpPropertyType.DateTimeOffset)
        {
          string propertyString = xmpMeta.GetPropertyString(propertyInfo.XmpPropertyNameSpace, propertyInfo.XmpPropertyKey);
          if (!string.IsNullOrEmpty(propertyString))
          {
            DateTimeOffset result;
            if (PdfDocumentMetadata.TryParseISO8601(propertyString, out result))
              return (object) result;
          }
        }
      }
      catch
      {
        return (object) null;
      }
    }
    return (object) null;
  }

  private void GetLanguages(
    CultureInfo cultureInfo,
    out string genericLang,
    out string specificLang)
  {
    genericLang = string.Empty;
    specificLang = string.Empty;
    for (; cultureInfo != null && !(cultureInfo.Name == string.Empty); cultureInfo = cultureInfo.Parent)
    {
      if (string.IsNullOrEmpty(specificLang))
        specificLang = cultureInfo.Name;
      if (cultureInfo.IsNeutralCulture)
        genericLang = cultureInfo.Name;
    }
    if (!string.IsNullOrEmpty(specificLang))
      return;
    specificLang = "x-default";
    genericLang = "x-default";
  }

  private IXmpMeta GetXmpMeta()
  {
    this.ThrowIfDisposed();
    try
    {
      if (this.document.Root.ContainsKey("Metadata"))
      {
        if (this.document.Root["Metadata"].Is<PdfTypeStream>())
        {
          PdfTypeStream stream = this.document.Root["Metadata"].As<PdfTypeStream>();
          if (stream.Dictionary.ContainsKey("Type"))
          {
            if (stream.Dictionary["Type"].Is<PdfTypeName>())
            {
              if (stream.Dictionary["Type"].As<PdfTypeName>().Value == "Metadata")
              {
                if (stream.Dictionary.ContainsKey("Subtype"))
                {
                  if (stream.Dictionary["Subtype"].Is<PdfTypeName>())
                  {
                    if (stream.Dictionary["Subtype"].As<PdfTypeName>().Value == "XML")
                      return XmpMetaFactory.ParseFromString(PdfDocumentMetadata.GetDecodedText(stream));
                  }
                }
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    return (IXmpMeta) null;
  }

  private bool SetXmpMeta(IXmpMeta xmpMeta)
  {
    this.ThrowIfDisposed();
    try
    {
      PdfTypeStream indirectObject = (PdfTypeStream) null;
      if (this.document.Root.ContainsKey("Metadata") && this.document.Root["Metadata"].Is<PdfTypeStream>())
        indirectObject = this.document.Root["Metadata"].As<PdfTypeStream>();
      if (indirectObject == null)
      {
        indirectObject = PdfTypeStream.Create();
        indirectObject.InitEmpty();
        indirectObject.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Metadata");
        indirectObject.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("XML");
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.document);
        int objectNumber = list.Add((PdfTypeBase) indirectObject);
        this.document.Root.SetIndirectAt("Metadata", list, objectNumber);
      }
      byte[] buffer = XmpMetaFactory.SerializeToBuffer(xmpMeta, new SerializeOptions()
      {
        Indent = "",
        UseCanonicalFormat = true,
        EncodeUtf16Be = false,
        EncodeUtf16Le = false,
        EncodeUtf8WithBom = false
      });
      indirectObject.SetContent(buffer, false);
      return true;
    }
    catch
    {
    }
    return false;
  }

  private static string GetDecodedText(PdfTypeStream stream)
  {
    byte[] decodedData = stream.DecodedData;
    Span<byte> span = decodedData.AsSpan<byte>();
    byte[] preamble1 = Encoding.UTF8.GetPreamble();
    byte[] preamble2 = Encoding.Unicode.GetPreamble();
    byte[] preamble3 = Encoding.BigEndianUnicode.GetPreamble();
    byte[] preamble4 = Encoding.UTF32.GetPreamble();
    if (span.Slice(0, Math.Min(30, decodedData.Length)).IndexOf<byte>((ReadOnlySpan<byte>) preamble1) != -1)
      return Encoding.UTF8.GetString(decodedData);
    if (span.Slice(0, Math.Min(60, decodedData.Length)).IndexOf<byte>((ReadOnlySpan<byte>) preamble2) != -1)
      return Encoding.Unicode.GetString(decodedData);
    if (span.Slice(0, Math.Min(60, decodedData.Length)).IndexOf<byte>((ReadOnlySpan<byte>) preamble3) != -1)
      return Encoding.BigEndianUnicode.GetString(decodedData);
    return span.Slice(0, Math.Min(120, decodedData.Length)).IndexOf<byte>((ReadOnlySpan<byte>) preamble4) != -1 ? Encoding.UTF32.GetString(decodedData) : Encoding.UTF8.GetString(decodedData);
  }

  private void ThrowIfDisposed()
  {
    if (this.document.IsDisposed)
      throw new ObjectDisposedException("document");
  }

  private static string FormatISO8601(DateTimeOffset dateTimeOffset)
  {
    string format = dateTimeOffset.Offset == TimeSpan.Zero ? "yyyy-MM-ddTHH:mm:ss.fffZ" : "yyyy-MM-ddTHH:mm:ss.fffzzz";
    return dateTimeOffset.ToString(format, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private static bool TryParseISO8601(string iso8601String, out DateTimeOffset result)
  {
    return DateTimeOffset.TryParseExact(iso8601String, new string[1]
    {
      "yyyy-MM-dd'T'HH:mm:ss.FFFK"
    }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
  }

  public class MetaPropertyInfo<T> : 
    PdfDocumentMetadata.MetaPropertyInfo,
    IEquatable<PdfDocumentMetadata.MetaPropertyInfo<T>>
  {
    public bool Equals(PdfDocumentMetadata.MetaPropertyInfo<T> other)
    {
      return this.Equals((PdfDocumentMetadata.MetaPropertyInfo) other);
    }
  }

  public class MetaPropertyInfo : IEquatable<PdfDocumentMetadata.MetaPropertyInfo>
  {
    public DocumentTags? DocumentTag { get; set; }

    public string InfoDictionaryKey { get; set; }

    public string XmpPropertyKey { get; set; }

    public string XmpPropertyNameSpace { get; set; }

    public PdfDocumentMetadata.XmpPropertyType XmpPropertyType { get; set; }

    public bool Equals(PdfDocumentMetadata.MetaPropertyInfo other)
    {
      if (other != (PdfDocumentMetadata.MetaPropertyInfo) null)
      {
        DocumentTags? documentTag1 = this.DocumentTag;
        DocumentTags? documentTag2 = other.DocumentTag;
        if (documentTag1.GetValueOrDefault() == documentTag2.GetValueOrDefault() & documentTag1.HasValue == documentTag2.HasValue && this.InfoDictionaryKey == other.InfoDictionaryKey && this.XmpPropertyKey == other.XmpPropertyKey && this.XmpPropertyNameSpace == other.XmpPropertyNameSpace)
          return this.XmpPropertyType == other.XmpPropertyType;
      }
      return false;
    }

    public override bool Equals(object obj)
    {
      PdfDocumentMetadata.MetaPropertyInfo other = obj as PdfDocumentMetadata.MetaPropertyInfo;
      return (object) other != null && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<DocumentTags?, string, string, string, PdfDocumentMetadata.XmpPropertyType>(this.DocumentTag, this.InfoDictionaryKey, this.XmpPropertyKey, this.XmpPropertyNameSpace, this.XmpPropertyType);
    }

    public static bool operator ==(
      PdfDocumentMetadata.MetaPropertyInfo left,
      PdfDocumentMetadata.MetaPropertyInfo right)
    {
      if ((object) left == null && (object) right == null)
        return true;
      return (object) left != null && (object) right != null && left.Equals(right);
    }

    public static bool operator !=(
      PdfDocumentMetadata.MetaPropertyInfo left,
      PdfDocumentMetadata.MetaPropertyInfo right)
    {
      return !(left == right);
    }
  }

  public enum XmpPropertyType
  {
    None,
    String,
    LocalizedString,
    ArrayOrdered,
    ArrayAlternate,
    DateTimeOffset,
  }
}
