// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocumentProperty
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.Utilities;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DocumentProperty
{
  private const int DEF_START_ID2 = 1000;
  private const int DEF_FILE_TIME_START_YEAR = 1600;
  private BuiltInProperty m_propertyId;
  private string m_strName;
  private object m_value;
  private Syncfusion.CompoundFile.DocIO.PropertyType m_type;
  private string m_strLinkSource;
  private byte m_bFlags;

  private DocumentProperty()
  {
  }

  internal DocumentProperty(string strName, object value)
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
        this.m_type = DocumentProperty.DetectPropertyType(value);
        break;
    }
  }

  internal DocumentProperty(string strName, object value, Syncfusion.CompoundFile.DocIO.PropertyType type)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty.");
      default:
        this.m_strName = strName;
        this.m_value = value;
        this.m_type = type;
        break;
    }
  }

  internal DocumentProperty(BuiltInProperty propertyId, object value)
  {
    this.m_propertyId = propertyId;
    this.m_value = value;
    this.m_type = DocumentProperty.DetectPropertyType(value);
  }

  internal DocumentProperty(IPropertyData variant, bool bSummary)
  {
    this.m_strName = variant != null ? variant.Name : throw new ArgumentNullException(nameof (variant));
    if (this.m_strName == null)
      this.m_propertyId = !bSummary ? (variant.Id != 1 ? (BuiltInProperty) (variant.Id + 1000 - 2) : (BuiltInProperty) variant.Id) : (BuiltInProperty) variant.Id;
    this.m_value = !bSummary || this.m_propertyId != BuiltInProperty.EditTime || !(variant.Value is DateTime) ? variant.Value : (object) TimeSpan.FromTicks(((DateTime) variant.Value).Ticks - 504911232000000000L);
    this.m_type = (Syncfusion.CompoundFile.DocIO.PropertyType) variant.Type;
  }

  internal bool IsBuiltIn => this.m_strName == null;

  internal BuiltInProperty PropertyId
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

  public PropertyValueType ValueType
  {
    get
    {
      if (this.m_value is string)
        return PropertyValueType.String;
      if (this.m_value is bool)
        return PropertyValueType.Boolean;
      if (this.m_value is DateTime)
        return PropertyValueType.Date;
      if (this.m_value is int || this.m_value is int || this.m_value is uint)
        return PropertyValueType.Int;
      if (this.m_value is double)
        return PropertyValueType.Double;
      if (this.Value is float)
        return PropertyValueType.Float;
      if (this.Value is byte[])
        return PropertyValueType.ByteArray;
      if (this.Value is ClipDataWrapper)
        return PropertyValueType.ClipData;
      throw new Exception("Property value is of unsupported type.");
    }
  }

  internal bool Boolean
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Bool)
        return Convert.ToBoolean(this.m_value);
      throw new InvalidCastException("Can't convert value to boolean.");
    }
    set
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Bool;
      this.m_value = (object) value;
    }
  }

  internal int Integer
  {
    get
    {
      this.DetectPropertyType();
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Int)
        return int.Parse(this.m_value.ToString());
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Int;
      this.m_value = (object) value;
    }
  }

  internal int Int32
  {
    get
    {
      this.DetectPropertyType();
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Int32)
        return Convert.ToInt32(this.m_value);
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Int32;
      this.m_value = (object) value;
    }
  }

  internal double Double
  {
    get
    {
      this.DetectPropertyType();
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Double)
        return Convert.ToDouble(this.m_value);
      throw new InvalidCastException("Can't convert value to integer.");
    }
    set
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Double;
      this.m_value = (object) value;
    }
  }

  internal string Text
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Empty)
        this.DetectPropertyType();
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.String || this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.AsciiString)
        return Convert.ToString(this.m_value);
      throw new InvalidCastException("Can't convert value to string.");
    }
    set
    {
      this.m_type = this.DetectStringType(value);
      this.m_value = (object) value;
    }
  }

  private Syncfusion.CompoundFile.DocIO.PropertyType DetectStringType(string value)
  {
    return Encoding.UTF8.GetByteCount(value) != value.Length ? Syncfusion.CompoundFile.DocIO.PropertyType.String : Syncfusion.CompoundFile.DocIO.PropertyType.AsciiString;
  }

  internal DateTime DateTime
  {
    get
    {
      try
      {
        this.DetectPropertyType();
        return this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.DateTime ? Convert.ToDateTime(this.m_value) : DateTime.MinValue;
      }
      catch
      {
        return DateTime.MinValue;
      }
    }
    set
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.DateTime;
      this.m_value = (object) value;
    }
  }

  internal TimeSpan TimeSpan
  {
    get => (TimeSpan) this.Value;
    set => this.m_value = (object) value;
  }

  internal byte[] Blob
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.Blob)
        return (byte[]) this.m_value;
      throw new InvalidCastException("Can't convert value to Blob.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Blob;
      this.m_value = (object) value;
    }
  }

  public ClipboardData ClipboardData
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.ClipboardData)
        return (ClipboardData) this.m_value;
      throw new InvalidCastException("Can't convert value to ClipboardData.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.ClipboardData;
      this.m_value = (object) value;
    }
  }

  internal string[] StringArray
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.StringArray)
        return (string[]) this.m_value;
      throw new InvalidCastException("Can't convert value to an array of strings.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.StringArray;
      this.m_value = (object) value;
    }
  }

  internal object[] ObjectArray
  {
    get
    {
      if (this.m_type == Syncfusion.CompoundFile.DocIO.PropertyType.ObjectArray)
        return (object[]) this.m_value;
      throw new InvalidCastException("Can't convert value to an array of strings.");
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.ObjectArray;
      this.m_value = (object) value;
    }
  }

  internal Syncfusion.CompoundFile.DocIO.PropertyType PropertyType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal string LinkSource
  {
    get => this.m_strLinkSource;
    set
    {
      if (this.IsBuiltIn)
        throw new InvalidOperationException("This operation can't be performed on built-in property.");
      this.m_strLinkSource = value;
      this.LinkToContent = true;
    }
  }

  internal bool LinkToContent
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal string InternalName => this.m_strName;

  public bool ToBool() => (bool) this.m_value;

  public DateTime ToDateTime() => ((DateTime) this.m_value).Date;

  public float ToFloat() => Convert.ToSingle(this.m_value);

  public double ToDouble() => (double) this.m_value;

  public int ToInt() => (int) this.m_value;

  public override string ToString() => (string) this.m_value;

  public byte[] ToByteArray() => (byte[]) this.m_value;

  internal ClipDataWrapper ToClipData() => (ClipDataWrapper) this.m_value;

  internal bool FillPropVariant(IPropertyData variant, int iPropertyId)
  {
    if (variant == null)
      throw new ArgumentNullException(nameof (variant));
    if (this.m_value == null)
      return false;
    if (this.IsBuiltIn)
    {
      int num = this.CorrectIndex(this.m_propertyId, out bool _);
      variant.Id = num;
    }
    else
      variant.Id = iPropertyId;
    object now = this.m_value;
    if (this.IsBuiltIn && variant.Id == 10 && this.m_value is TimeSpan)
      now = (object) DateTime.Now;
    return variant.SetValue(now, this.m_type);
  }

  internal int CorrectIndex(BuiltInProperty propertyId, out bool bSummary)
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

  internal static Syncfusion.CompoundFile.DocIO.PropertyType DetectPropertyType(object value)
  {
    Syncfusion.CompoundFile.DocIO.PropertyType propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.Null;
    switch (value)
    {
      case string _:
        propertyType = Encoding.UTF8.GetByteCount(value as string) == (value as string).Length ? Syncfusion.CompoundFile.DocIO.PropertyType.AsciiString : Syncfusion.CompoundFile.DocIO.PropertyType.String;
        break;
      case double _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.Double;
        break;
      case int _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.Int32;
        break;
      case bool _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.Bool;
        break;
      case DateTime _:
      case TimeSpan _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.DateTime;
        break;
      case object[] _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.ObjectArray;
        break;
      case string[] _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.StringArray;
        break;
      case byte[] _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.Blob;
        break;
      case ClipboardData _:
        propertyType = Syncfusion.CompoundFile.DocIO.PropertyType.ClipboardData;
        break;
    }
    return propertyType;
  }

  private void DetectPropertyType()
  {
    if (this.m_value is string)
      this.m_type = this.DetectStringType((string) this.m_value);
    else if (this.m_value is double)
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Double;
    else if (this.m_value is int)
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Int32;
    else if (this.m_value is bool)
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Bool;
    else if (this.m_value is DateTime || this.m_value is TimeSpan)
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.DateTime;
    else if (this.m_value is object[])
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.ObjectArray;
    else if (this.m_value is string[])
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.StringArray;
    else if (this.m_value is byte[])
    {
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.Blob;
    }
    else
    {
      if (!(this.m_value is ClipboardData))
        return;
      this.m_type = Syncfusion.CompoundFile.DocIO.PropertyType.ClipboardData;
    }
  }

  internal void SetLinkSource(IPropertyData variant)
  {
    if (variant == null)
      throw new ArgumentNullException(nameof (variant));
    this.LinkSource = variant.Type == VarEnum.VT_LPSTR || variant.Type == VarEnum.VT_LPWSTR ? variant.Value.ToString() : throw new ArgumentOutOfRangeException("LinkSource");
  }

  public DocumentProperty Clone()
  {
    DocumentProperty documentProperty = (DocumentProperty) this.MemberwiseClone();
    documentProperty.CloneValue();
    return documentProperty;
  }

  private void CloneValue()
  {
    if (this.m_value == null)
      return;
    switch (this.m_type)
    {
      case Syncfusion.CompoundFile.DocIO.PropertyType.Blob:
        this.m_value = (object) CloneUtils.CloneByteArray(this.Blob);
        break;
      case Syncfusion.CompoundFile.DocIO.PropertyType.ClipboardData:
        this.m_value = CloneUtils.CloneCloneable((ICloneable) this.ClipboardData);
        break;
      case Syncfusion.CompoundFile.DocIO.PropertyType.ObjectArray:
        this.m_value = (object) CloneUtils.CloneArray(this.ObjectArray);
        break;
      case Syncfusion.CompoundFile.DocIO.PropertyType.StringArray:
        this.m_value = (object) CloneUtils.CloneStringArray(this.StringArray);
        break;
    }
  }

  internal void Close()
  {
    if (this.m_value == null)
      return;
    this.m_value = (object) null;
  }
}
