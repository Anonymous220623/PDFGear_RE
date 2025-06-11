// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.SystemVersionInfo
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Data;

public readonly struct SystemVersionInfo(int major, int minor, int build)
{
  public static SystemVersionInfo Windows10 => new SystemVersionInfo(10, 0, 10240);

  public static SystemVersionInfo Windows10_1809 => new SystemVersionInfo(10, 0, 17763);

  public static SystemVersionInfo Windows10_1903 => new SystemVersionInfo(10, 0, 18362);

  public int Major { get; } = major;

  public int Minor { get; } = minor;

  public int Build { get; } = build;

  public bool Equals(SystemVersionInfo other)
  {
    return this.Major == other.Major && this.Minor == other.Minor && this.Build == other.Build;
  }

  public override bool Equals(object obj) => obj is SystemVersionInfo other && this.Equals(other);

  public override int GetHashCode()
  {
    int num1 = this.Major;
    int hashCode1 = num1.GetHashCode();
    num1 = this.Minor;
    int hashCode2 = num1.GetHashCode();
    int num2 = hashCode1 ^ hashCode2;
    num1 = this.Build;
    int hashCode3 = num1.GetHashCode();
    return num2 ^ hashCode3;
  }

  public static bool operator ==(SystemVersionInfo left, SystemVersionInfo right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(SystemVersionInfo left, SystemVersionInfo right)
  {
    return !(left == right);
  }

  public int CompareTo(SystemVersionInfo other)
  {
    if (this.Major != other.Major)
      return this.Major.CompareTo(other.Major);
    if (this.Minor != other.Minor)
      return this.Minor.CompareTo(other.Minor);
    return this.Build != other.Build ? this.Build.CompareTo(other.Build) : 0;
  }

  public int CompareTo(object obj)
  {
    if (obj is SystemVersionInfo other)
      return this.CompareTo(other);
    throw new ArgumentException();
  }

  public static bool operator <(SystemVersionInfo left, SystemVersionInfo right)
  {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <=(SystemVersionInfo left, SystemVersionInfo right)
  {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator >(SystemVersionInfo left, SystemVersionInfo right)
  {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >=(SystemVersionInfo left, SystemVersionInfo right)
  {
    return left.CompareTo(right) >= 0;
  }

  public override string ToString() => $"{this.Major}.{this.Minor}.{this.Build}";
}
