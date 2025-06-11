// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.DocumentPropertyImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.CompoundFile.XlsIO.Net;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO;

public class DocumentPropertyImpl : IDocumentProperty, ICloneable
{
  private const int DEF_START_ID2 = 1000;
  public const int DEF_FILE_TIME_START_YEAR = 1600;
  private BuiltInProperty m_propertyId;
  private string m_strName;
  private object m_value;
  private PropertyType m_type;
  private string m_strLinkSource;
  private bool m_bLinkToContent;

  private DocumentPropertyImpl()
  {
  }

  public DocumentPropertyImpl(string strName, object value)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty.");
      default:
        this.m_strName = strName;
        this.Value = value;
        break;
    }
  }

  public DocumentPropertyImpl(BuiltInProperty propertyId, object value)
  {
    this.m_propertyId = propertyId;
    this.m_value = value;
  }

  public DocumentPropertyImpl(IPropertyData variant, bool bSummary)
  {
    this.m_strName = variant != null ? variant.Name : throw new ArgumentNullException(nameof (variant));
    if (this.m_strName == null)
      this.m_propertyId = !bSummary ? (BuiltInProperty) (variant.Id + 1000 - 2) : (BuiltInProperty) variant.Id;
    this.m_value = variant.Value;
    this.m_type = (PropertyType) variant.Type;
  }

  public bool IsBuiltIn => this.m_strName == null;

  public BuiltInProperty PropertyId
  {
    get => this.m_propertyId;
    set => this.m_propertyId = value;
  }

  public string Name => this.m_strName != null ? this.m_strName : this.m_propertyId.ToString();

  public object Value
  {
    get => this.m_value;
    set
    {
      this.m_value = value;
      this.DetectPropertyType();
    }
  }

  public bool Boolean
  {
    get
    {
      if (this.m_type == PropertyType.Bool)
        return Convert.ToBoolean(this.m_value);
      throw new InvalidCastException("Can't convert value to boolean.");
    }
    set
    {
      this.m_type = PropertyType.Bool;
      this.m_value = (object) value;
    }
  }

  public int Integer
  {
    get
    {
      if (this.m_type == PropertyType.Int)
        return Convert.ToInt32(this.m_value);
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = PropertyType.Int;
      this.m_value = (object) value;
    }
  }

  public int Int32
  {
    get
    {
      if (this.m_type == PropertyType.Int32)
        return Convert.ToInt32(this.m_value);
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = PropertyType.Int32;
      this.m_value = (object) value;
    }
  }

  public double Double
  {
    get
    {
      if (this.m_type == PropertyType.Double)
        return Convert.ToDouble(this.m_value);
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = PropertyType.Double;
      this.m_value = (object) value;
    }
  }

  public string Text
  {
    get
    {
      return this.m_type == PropertyType.String || this.m_type == PropertyType.AsciiString ? Convert.ToString(this.m_value) : string.Empty;
    }
    set
    {
      this.m_type = this.DetectStringType(value);
      this.m_value = (object) value;
    }
  }

  private PropertyType DetectStringType(string value)
  {
    return Encoding.UTF8.GetByteCount(value) != value.Length ? PropertyType.String : PropertyType.AsciiString;
  }

  public DateTime DateTime
  {
    get
    {
      if (this.m_type == PropertyType.DateTime)
        return Convert.ToDateTime(this.m_value);
      throw new InvalidCastException("Can't convert value to DateTime.");
    }
    set
    {
      this.m_type = PropertyType.DateTime;
      this.m_value = (object) value;
    }
  }

  public TimeSpan TimeSpan
  {
    get
    {
      DateTime dateTime = this.DateTime;
      dateTime = dateTime.AddYears(-1600);
      return new TimeSpan(dateTime.Ticks);
    }
    set
    {
      DateTime dateTime = new DateTime(value.Ticks);
      dateTime = dateTime.AddYears(1600);
      this.DateTime = dateTime;
    }
  }

  public byte[] Blob
  {
    get
    {
      if (this.m_type == PropertyType.Blob)
        return (byte[]) this.m_value;
      throw new InvalidCastException("Can't convert value to Blob.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = PropertyType.Blob;
      this.m_value = (object) value;
    }
  }

  public ClipboardData ClipboardData
  {
    get
    {
      if (this.m_type == PropertyType.ClipboardData)
        return (ClipboardData) this.m_value;
      throw new InvalidCastException("Can't convert value to ClipboardData.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = PropertyType.ClipboardData;
      this.m_value = (object) value;
    }
  }

  public string[] StringArray
  {
    get
    {
      if (this.m_type == PropertyType.StringArray)
        return (string[]) this.m_value;
      throw new InvalidCastException("Can't convert value to an array of strings.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = PropertyType.StringArray;
      this.m_value = (object) value;
    }
  }

  public object[] ObjectArray
  {
    get
    {
      if (this.m_type == PropertyType.ObjectArray)
        return (object[]) this.m_value;
      throw new InvalidCastException("Can't convert value to an array of strings.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = PropertyType.ObjectArray;
      this.m_value = (object) value;
    }
  }

  public PropertyType PropertyType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public string LinkSource
  {
    get => this.m_strLinkSource;
    set
    {
      if (this.IsBuiltIn)
        throw new InvalidOperationException("This operation can't be performed on built-in property.");
      this.m_strLinkSource = value;
      this.m_bLinkToContent = true;
    }
  }

  public bool LinkToContent
  {
    get => this.m_bLinkToContent;
    set => this.m_bLinkToContent = value;
  }

  public string InternalName => this.m_strName;

  public bool FillPropVariant(IPropertyData variant, int iPropertyId)
  {
    if (variant == null)
      throw new ArgumentNullException(nameof (variant));
    if (this.m_value == null)
      return false;
    if (this.IsBuiltIn)
    {
      int num = DocumentPropertyImpl.CorrectIndex(this.m_propertyId, out bool _);
      variant.Id = num;
    }
    else
      variant.Id = iPropertyId;
    return variant.SetValue(this.m_value, this.m_type);
  }

  public static int CorrectIndex(BuiltInProperty propertyId, out bool bSummary)
  {
    int num = (int) propertyId;
    if (num >= 1000)
    {
      num -= 998;
      bSummary = false;
    }
    else
      bSummary = true;
    return num;
  }

  private void DetectPropertyType()
  {
    if (this.m_value is string)
      this.m_type = this.DetectStringType((string) this.m_value);
    else if (this.m_value is double)
      this.m_type = PropertyType.Double;
    else if (this.m_value is int)
      this.m_type = PropertyType.Int;
    else if (this.m_value is bool)
      this.m_type = PropertyType.Bool;
    else if (this.m_value is DateTime)
      this.m_type = PropertyType.DateTime;
    else if (this.m_value is object[])
      this.m_type = PropertyType.ObjectArray;
    else if (this.m_value is string[])
      this.m_type = PropertyType.StringArray;
    else if (this.m_value is byte[])
    {
      this.m_type = PropertyType.Blob;
    }
    else
    {
      if (!(this.m_value is ClipboardData))
        return;
      this.m_type = PropertyType.ClipboardData;
    }
  }

  public void SetLinkSource(IPropertyData variant)
  {
    if (variant == null)
      throw new ArgumentNullException(nameof (variant));
    this.LinkSource = variant.Type == VarEnum.VT_LPSTR || variant.Type == VarEnum.VT_LPWSTR ? variant.Value.ToString() : throw new ArgumentOutOfRangeException("LinkSource");
  }

  [CLSCompliant(false)]
  public void Write(IPropertyStorage storProp, PropVariant variant, int iPropertyId)
  {
    if (storProp == null)
      throw new ArgumentNullException(nameof (storProp));
    if (variant == null)
      throw new ArgumentNullException(nameof (variant));
    this.FillPropVariant((IPropertyData) variant, iPropertyId);
    variant.Write(storProp);
    if (!this.IsBuiltIn)
    {
      uint rgpropid = (uint) iPropertyId;
      storProp.WritePropertyNames(1U, ref rgpropid, ref this.m_strName);
    }
    if (!this.LinkToContent)
      return;
    variant.PropId = (PIDSI) (iPropertyId + 16777216 /*0x01000000*/);
    variant.String = this.m_strLinkSource;
    variant.Write(storProp);
  }

  public object Clone()
  {
    DocumentPropertyImpl documentPropertyImpl = (DocumentPropertyImpl) this.MemberwiseClone();
    documentPropertyImpl.CloneValue();
    return (object) documentPropertyImpl;
  }

  private void CloneValue()
  {
    if (this.m_value == null)
      return;
    switch (this.m_type)
    {
      case PropertyType.Blob:
        this.m_value = (object) CloneUtils.CloneByteArray(this.Blob);
        break;
      case PropertyType.ClipboardData:
        this.m_value = CloneUtils.CloneCloneable((ICloneable) this.ClipboardData);
        break;
      case PropertyType.ObjectArray:
        this.m_value = (object) CloneUtils.CloneArray(this.ObjectArray);
        break;
      case PropertyType.StringArray:
        this.m_value = (object) CloneUtils.CloneStringArray(this.StringArray);
        break;
    }
  }
}
