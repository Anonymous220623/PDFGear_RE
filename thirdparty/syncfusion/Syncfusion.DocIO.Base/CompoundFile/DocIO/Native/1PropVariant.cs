// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.PropVariant
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO.Net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

public class PropVariant : IDisposable, IPropertyData
{
  public const int TYPE_OFFSET = 0;
  public const int FirstIntOffset = 8;
  private const int IntSize = 4;
  private const int DEF_SHORT_PROPERTY_TYPE_MASK = 255 /*0xFF*/;
  private const long DEF_LOW_INT_MASK = 4294967295 /*0xFFFFFFFF*/;
  private const ulong DEF_HIGH_INT_MASK = 18446744069414584320;
  private const int DEF_INT_BITS = 32 /*0x20*/;
  public const long DEF_FILETIME_TICKS_DIFFERENCE = 504911232000000000;
  internal const int DEF_LINK_BIT = 16777216 /*0x01000000*/;
  public static readonly int PropVariantSize = 8 + IntPtr.Size * 2;
  public static readonly int SecondIntOffset = 8 + IntPtr.Size;
  private List<IntPtr> m_arrFree = new List<IntPtr>();
  private List<IntPtr> m_arrGlobalFree = new List<IntPtr>();
  private IntPtr m_propVariant = Marshal.AllocHGlobal(PropVariant.PropVariantSize);
  private PROPSPEC m_prop = new PROPSPEC();
  private List<PropVariant> m_arrDispose = new List<PropVariant>();
  private bool m_bFreeVariant = true;

  public PropVariant()
  {
    this.m_prop.ulKind = (IntPtr) 1L;
    int num = 0;
    int ofs = 0;
    while (num < PropVariant.PropVariantSize / 4)
    {
      Marshal.WriteInt32(this.m_propVariant, ofs, 0);
      ++num;
      ofs += 4;
    }
  }

  public PropVariant(IntPtr ptr)
    : this()
  {
    this.IntPtr = ptr;
  }

  [CLSCompliant(false)]
  public PropVariant(tagSTATPROPSTG propInfo, IPropertyStorage propStorage, bool bBuiltIn)
    : this()
  {
    this.Read(propInfo, propStorage, bBuiltIn);
  }

  public short Int16
  {
    get => (short) this.FirstInt.ToInt64();
    set
    {
      this.FreeResources();
      this.Type = VarEnum.VT_I2;
      this.FirstInt = (IntPtr) (int) value;
      this.SecondInt = IntPtr.Zero;
    }
  }

  public int Int
  {
    get => (int) this.FirstInt.ToInt64();
    set
    {
      this.FreeResources();
      this.Type = VarEnum.VT_INT;
      this.FirstInt = (IntPtr) value;
      this.SecondInt = IntPtr.Zero;
    }
  }

  public int Int32
  {
    get => (int) this.FirstInt.ToInt64();
    set
    {
      this.FreeResources();
      this.Type = VarEnum.VT_I4;
      this.FirstInt = (IntPtr) value;
      this.SecondInt = IntPtr.Zero;
    }
  }

  public IntPtr IntPtr
  {
    get => this.m_propVariant;
    set
    {
      this.FreeResources();
      if (!(this.m_propVariant != value))
        return;
      this.m_propVariant = value;
      this.m_bFreeVariant = false;
    }
  }

  public PIDSI PropId
  {
    get => (PIDSI) (int) this.m_prop.propid;
    set
    {
      this.m_prop.ulKind = (IntPtr) 1L;
      this.m_prop.propid = (IntPtr) (long) value;
    }
  }

  public PIDDSI PropId2
  {
    get => (PIDDSI) (int) this.m_prop.propid;
    set => this.m_prop.propid = (IntPtr) (long) value;
  }

  public System.Runtime.InteropServices.ComTypes.FILETIME FileTime
  {
    get
    {
      System.Runtime.InteropServices.ComTypes.FILETIME fileTime = new System.Runtime.InteropServices.ComTypes.FILETIME();
      long num = Marshal.ReadInt64(this.m_propVariant, 8);
      fileTime.dwLowDateTime = (int) (num & (long) uint.MaxValue);
      fileTime.dwHighDateTime = (int) (num >> 32 /*0x20*/ & (long) uint.MaxValue);
      return fileTime;
    }
    set
    {
      this.Type = VarEnum.VT_FILETIME;
      Marshal.WriteInt64(this.m_propVariant, 8, ((long) value.dwHighDateTime << 32 /*0x20*/) + (long) (uint) value.dwLowDateTime);
    }
  }

  public bool Bool
  {
    get => this.FirstInt != IntPtr.Zero;
    set
    {
      this.Type = VarEnum.VT_BOOL;
      this.FirstInt = (IntPtr) (value ? 1 : 0);
      this.SecondInt = IntPtr.Zero;
    }
  }

  public string String
  {
    get => Marshal.PtrToStringUni(this.FirstInt);
    set
    {
      IntPtr hglobalUni = Marshal.StringToHGlobalUni(value);
      this.Type = VarEnum.VT_LPWSTR;
      this.FirstInt = hglobalUni;
      this.SecondInt = IntPtr.Zero;
      this.m_arrFree.Add(hglobalUni);
    }
  }

  public string AsciiString
  {
    get => Marshal.PtrToStringAnsi(this.FirstInt);
    set
    {
      IntPtr hglobalAnsi = Marshal.StringToHGlobalAnsi(value);
      this.Type = VarEnum.VT_LPSTR;
      this.FirstInt = hglobalAnsi;
      this.SecondInt = IntPtr.Zero;
      this.m_arrFree.Add(hglobalAnsi);
    }
  }

  public DateTime DateTime
  {
    get
    {
      System.Runtime.InteropServices.ComTypes.FILETIME fileTime = this.FileTime;
      DateTime dateTime = new DateTime(((long) fileTime.dwHighDateTime << 32 /*0x20*/) + (long) (uint) fileTime.dwLowDateTime + 504911232000000000L);
      if (this.PropId != PIDSI.EditTime)
        dateTime = dateTime.ToLocalTime();
      return dateTime;
    }
    set
    {
      if (this.PropId != PIDSI.EditTime)
        value = value.ToUniversalTime();
      ulong num = (ulong) (value.Ticks - 504911232000000000L);
      this.FileTime = new System.Runtime.InteropServices.ComTypes.FILETIME()
      {
        dwHighDateTime = (int) ((num & 18446744069414584320UL) >> 32 /*0x20*/),
        dwLowDateTime = (int) ((long) num & (long) uint.MaxValue)
      };
    }
  }

  public double Double
  {
    get => BitConverter.Int64BitsToDouble(Marshal.ReadInt64(this.m_propVariant, 8));
    set
    {
      this.Type = VarEnum.VT_R8;
      Marshal.WriteInt64(this.m_propVariant, 8, BitConverter.DoubleToInt64Bits(value));
    }
  }

  public string Name
  {
    get
    {
      return this.m_prop.ulKind == (IntPtr) 0L ? Marshal.PtrToStringUni(this.m_prop.propid) : (string) null;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.SetName(value);
    }
  }

  public object Value => this.GetValue();

  public bool IsLinkToSource
  {
    get
    {
      return this.m_prop.ulKind == (IntPtr) 1L && (this.m_prop.propid.ToInt64() & 16777216L /*0x01000000*/) != 0L;
    }
  }

  public int ParentId
  {
    get
    {
      return this.IsLinkToSource ? (int) (this.m_prop.propid.ToInt64() - 16777216L /*0x01000000*/) : (int) this.m_prop.propid.ToInt64();
    }
  }

  public int Id
  {
    get => (int) this.m_prop.propid.ToInt64();
    set
    {
      this.m_prop.ulKind = (IntPtr) 1L;
      this.m_prop.propid = (IntPtr) value;
    }
  }

  public string[] GetStringArray()
  {
    long int64 = this.FirstInt.ToInt64();
    string[] stringArray = new string[int64];
    IntPtr secondInt = this.SecondInt;
    for (int index = 0; (long) index < int64; ++index)
    {
      IntPtr ptrString = Marshal.ReadIntPtr(secondInt, index * IntPtr.Size);
      stringArray[index] = this.GetString(ptrString);
    }
    return stringArray;
  }

  private string GetString(IntPtr ptrString)
  {
    if (ptrString == IntPtr.Zero)
      throw new ArgumentNullException(nameof (ptrString));
    return (this.Type & (VarEnum) 255 /*0xFF*/) != VarEnum.VT_LPSTR ? Marshal.PtrToStringUni(ptrString) : this.GetShortString(ptrString);
  }

  private string GetShortString(IntPtr ptrString)
  {
    if (ptrString == IntPtr.Zero)
      throw new ArgumentNullException(nameof (ptrString));
    int length = 0;
    while (Marshal.ReadByte(ptrString, length) != (byte) 0)
      ++length;
    byte[] numArray = new byte[length];
    Marshal.Copy(ptrString, numArray, 0, length);
    return Encoding.UTF8.GetString(numArray, 0, length);
  }

  public object[] GetObjectArray()
  {
    long int64 = this.FirstInt.ToInt64();
    object[] objectArray = new object[int64];
    IntPtr secondInt = this.SecondInt;
    int index = 0;
    int num = 0;
    while ((long) index < int64)
    {
      PropVariant propVariant = new PropVariant((IntPtr) (this.SecondInt.ToInt64() + (long) num));
      objectArray[index] = propVariant.GetValue();
      ++index;
      num += PropVariant.PropVariantSize;
    }
    return objectArray;
  }

  public void SetStringArray(string[] value)
  {
    this.FreeResources();
    int length = value.Length;
    IntPtr[] numArray = new IntPtr[length];
    IntPtr ptr = Marshal.AllocHGlobal(length * IntPtr.Size);
    this.m_arrGlobalFree.Add(ptr);
    for (int index = 0; index < length; ++index)
    {
      numArray[index] = Marshal.StringToHGlobalUni(value[index]);
      this.m_arrFree.Add(numArray[index]);
    }
    int ofs = 0;
    for (int index = 0; index < length; ++index)
    {
      Marshal.WriteIntPtr(ptr, ofs, numArray[index]);
      ofs += IntPtr.Size;
    }
    this.Type = VarEnum.VT_LPWSTR | VarEnum.VT_VECTOR;
    this.FirstInt = (IntPtr) length;
    this.SecondInt = ptr;
  }

  public void SetObjectArray(object[] value)
  {
    this.FreeResources();
    this.Type = VarEnum.VT_VARIANT | VarEnum.VT_VECTOR;
    int length = value.Length;
    IntPtr num1 = Marshal.AllocHGlobal(PropVariant.PropVariantSize * length);
    this.m_arrGlobalFree.Add(num1);
    int index = 0;
    int num2 = 0;
    while (index < length)
    {
      if (value[index] is int)
        this.m_arrDispose.Add(new PropVariant((IntPtr) (num1.ToInt64() + (long) num2))
        {
          Int = (int) value[index],
          Type = VarEnum.VT_I4
        });
      else if (value[index] is string)
        this.m_arrDispose.Add(new PropVariant((IntPtr) (num1.ToInt64() + (long) num2))
        {
          String = (string) value[index]
        });
      ++index;
      num2 += PropVariant.PropVariantSize;
    }
    this.FirstInt = (IntPtr) length;
    this.SecondInt = num1;
  }

  public void SetBlob(byte[] value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    this.FreeResources();
    this.Type = VarEnum.VT_BLOB;
    int length = value.Length;
    this.FirstInt = (IntPtr) length;
    IntPtr destination = Marshal.AllocHGlobal(length);
    Marshal.Copy(value, 0, destination, length);
    this.SecondInt = destination;
    this.m_arrGlobalFree.Add(destination);
  }

  public void SetName(string strName)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty.");
      default:
        this.FreeName();
        this.m_prop.ulKind = (IntPtr) 0L;
        this.m_prop.propid = Marshal.StringToHGlobalUni(strName);
        break;
    }
  }

  public bool SetValue(object value, PropertyType type)
  {
    switch (type)
    {
      case PropertyType.Int16:
        this.Int16 = (short) value;
        break;
      case PropertyType.Int32:
        this.Int32 = (int) value;
        break;
      case PropertyType.Double:
        this.Double = (double) value;
        break;
      case PropertyType.Bool:
        this.Bool = (bool) value;
        break;
      case PropertyType.Int:
        this.Int = (int) value;
        break;
      case PropertyType.AsciiString:
        this.AsciiString = value.ToString();
        break;
      case PropertyType.String:
        this.String = value.ToString();
        break;
      case PropertyType.DateTime:
        this.DateTime = (DateTime) value;
        break;
      case PropertyType.Blob:
        this.SetBlob((byte[]) value);
        break;
      case PropertyType.ObjectArray:
        this.SetObjectArray((object[]) value);
        break;
      case PropertyType.StringArray:
        this.SetStringArray((string[]) value);
        break;
      default:
        return false;
    }
    return true;
  }

  private IntPtr FirstInt
  {
    get => Marshal.ReadIntPtr(this.m_propVariant, 8);
    set => Marshal.WriteIntPtr(this.m_propVariant, 8, value);
  }

  private IntPtr SecondInt
  {
    get => Marshal.ReadIntPtr(this.m_propVariant, PropVariant.SecondIntOffset);
    set => Marshal.WriteIntPtr(this.m_propVariant, PropVariant.SecondIntOffset, value);
  }

  public VarEnum Type
  {
    get => (VarEnum) Marshal.ReadInt16(this.m_propVariant, 0);
    set => Marshal.WriteInt16(this.m_propVariant, 0, (short) value);
  }

  private void FreeResources()
  {
    int index1 = 0;
    for (int count = this.m_arrFree.Count; index1 < count; ++index1)
      Marshal.FreeCoTaskMem(this.m_arrFree[index1]);
    this.m_arrFree.Clear();
    int index2 = 0;
    for (int count = this.m_arrGlobalFree.Count; index2 < count; ++index2)
      Marshal.FreeHGlobal(this.m_arrGlobalFree[index2]);
    this.m_arrGlobalFree.Clear();
    for (int index3 = 0; index3 < this.m_arrDispose.Count; ++index3)
      this.m_arrDispose[index3].Dispose();
    this.m_arrDispose.Clear();
  }

  private void FreeName()
  {
    double propid = (double) (long) this.m_prop.propid;
    if (!(this.m_prop.ulKind == (IntPtr) 0L) || propid == 0.0)
      return;
    Marshal.FreeHGlobal((IntPtr) (long) propid);
    this.m_prop.propid = IntPtr.Zero;
  }

  private object GetValue()
  {
    VarEnum type = this.Type;
    IntPtr ptr = (IntPtr) (this.m_propVariant.ToInt64() + 8L);
    switch (type)
    {
      case VarEnum.VT_I4:
      case VarEnum.VT_INT:
        return (object) Marshal.ReadInt32(ptr);
      case VarEnum.VT_R8:
        return (object) BitConverter.Int64BitsToDouble(Marshal.ReadInt64(ptr));
      case VarEnum.VT_BOOL:
        return (object) (Marshal.ReadInt32(ptr) != 0);
      case VarEnum.VT_LPSTR:
      case VarEnum.VT_LPWSTR:
        return this.FirstInt != IntPtr.Zero ? (object) this.GetString(this.FirstInt) : (object) string.Empty;
      case VarEnum.VT_FILETIME:
        return (object) this.DateTime;
      case VarEnum.VT_BLOB:
        byte[] destination = new byte[this.FirstInt.ToInt64()];
        Marshal.Copy(this.SecondInt, destination, 0, destination.Length);
        return (object) destination;
      case VarEnum.VT_VARIANT | VarEnum.VT_VECTOR:
        return (object) this.GetObjectArray();
      case VarEnum.VT_LPSTR | VarEnum.VT_VECTOR:
      case VarEnum.VT_LPWSTR | VarEnum.VT_VECTOR:
        object stringArray = (object) this.GetStringArray();
        this.Type = VarEnum.VT_LPWSTR | VarEnum.VT_VECTOR;
        return stringArray;
      default:
        return (object) null;
    }
  }

  [CLSCompliant(false)]
  public void Write(IPropertyStorage storProp)
  {
    PID propidNameFirst = PID.PID_FIRST_USABLE;
    if (this.m_prop.ulKind == (IntPtr) 1L)
      propidNameFirst = (PID) (int) this.m_prop.propid;
    storProp.WriteMultiple(1U, ref this.m_prop, this.IntPtr, propidNameFirst);
  }

  [CLSCompliant(false)]
  public void Read(tagSTATPROPSTG propInfo, IPropertyStorage storProp, bool bBuiltIn)
  {
    this.m_prop.propid = (IntPtr) (long) propInfo.propid;
    this.m_prop.ulKind = (IntPtr) 1L;
    storProp.ReadMultiple(1U, ref this.m_prop, this.m_propVariant);
    if (bBuiltIn || (this.m_prop.propid.ToInt64() & 16777216L /*0x01000000*/) != 0L)
      return;
    this.Name = propInfo.lpwstrName;
  }

  [CLSCompliant(false)]
  public void Read(IPropertyStorage storProp, bool bBuiltIn)
  {
    storProp.ReadMultiple(1U, ref this.m_prop, this.m_propVariant);
  }

  public void Dispose()
  {
    if (this.m_propVariant != IntPtr.Zero)
    {
      this.FreeResources();
      this.FreeName();
      if (this.m_bFreeVariant)
      {
        Marshal.FreeHGlobal(this.m_propVariant);
        this.m_propVariant = IntPtr.Zero;
      }
    }
    GC.SuppressFinalize((object) this);
  }

  ~PropVariant() => this.Dispose();
}
