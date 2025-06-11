// Decompiled with JetBrains decompiler
// Type: Standard.Win32Error
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Explicit)]
internal struct Win32Error(int i)
{
  [FieldOffset(0)]
  private readonly int _value = i;
  public static readonly Win32Error ERROR_SUCCESS = new Win32Error(0);
  public static readonly Win32Error ERROR_INVALID_FUNCTION = new Win32Error(1);
  public static readonly Win32Error ERROR_FILE_NOT_FOUND = new Win32Error(2);
  public static readonly Win32Error ERROR_PATH_NOT_FOUND = new Win32Error(3);
  public static readonly Win32Error ERROR_TOO_MANY_OPEN_FILES = new Win32Error(4);
  public static readonly Win32Error ERROR_ACCESS_DENIED = new Win32Error(5);
  public static readonly Win32Error ERROR_INVALID_HANDLE = new Win32Error(6);
  public static readonly Win32Error ERROR_OUTOFMEMORY = new Win32Error(14);
  public static readonly Win32Error ERROR_NO_MORE_FILES = new Win32Error(18);
  public static readonly Win32Error ERROR_SHARING_VIOLATION = new Win32Error(32 /*0x20*/);
  public static readonly Win32Error ERROR_INVALID_PARAMETER = new Win32Error(87);
  public static readonly Win32Error ERROR_INSUFFICIENT_BUFFER = new Win32Error(122);
  public static readonly Win32Error ERROR_NESTING_NOT_ALLOWED = new Win32Error(215);
  public static readonly Win32Error ERROR_KEY_DELETED = new Win32Error(1018);
  public static readonly Win32Error ERROR_NOT_FOUND = new Win32Error(1168);
  public static readonly Win32Error ERROR_NO_MATCH = new Win32Error(1169);
  public static readonly Win32Error ERROR_BAD_DEVICE = new Win32Error(1200);
  public static readonly Win32Error ERROR_CANCELLED = new Win32Error(1223);
  public static readonly Win32Error ERROR_CLASS_ALREADY_EXISTS = new Win32Error(1410);
  public static readonly Win32Error ERROR_INVALID_DATATYPE = new Win32Error(1804);

  public static explicit operator HRESULT(Win32Error error)
  {
    return error._value <= 0 ? new HRESULT((uint) error._value) : HRESULT.Make(true, Facility.Win32, error._value & (int) ushort.MaxValue);
  }

  public HRESULT ToHRESULT() => (HRESULT) this;

  public static Win32Error GetLastError() => new Win32Error(Marshal.GetLastWin32Error());

  public override bool Equals(object obj)
  {
    try
    {
      return ((Win32Error) obj)._value == this._value;
    }
    catch (InvalidCastException ex)
    {
      return false;
    }
  }

  public override int GetHashCode() => this._value.GetHashCode();

  public static bool operator ==(Win32Error errLeft, Win32Error errRight)
  {
    return errLeft._value == errRight._value;
  }

  public static bool operator !=(Win32Error errLeft, Win32Error errRight) => !(errLeft == errRight);
}
