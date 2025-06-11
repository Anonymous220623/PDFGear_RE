// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.DataType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class DataType
{
  internal static readonly DataType Int8Unsigned = new DataType("BYTE", DataTypeID.Int8Unsigned, 1);
  internal static readonly DataType String = new DataType("STRING", DataTypeID.String, 1);
  internal static readonly DataType Int16Unsigned = new DataType("USHORT", DataTypeID.Int16Unsigned, 2);
  internal static readonly DataType Int32Unsigned = new DataType("ULONG", DataTypeID.Int32Unsigned, 4);
  internal static readonly DataType RationalUnsigned = new DataType("URATIONAL", DataTypeID.RationalUnsigned, 8);
  internal static readonly DataType Int8Signed = new DataType("SBYTE", DataTypeID.Int8Signed, 1);
  internal static readonly DataType Undefined = new DataType("UNDEFINED", DataTypeID.Undefined, 1);
  internal static readonly DataType Int16Signed = new DataType("SSHORT", DataTypeID.Int16Signed, 2);
  internal static readonly DataType Int32Signed = new DataType("SLONG", DataTypeID.Int32Signed, 4);
  internal static readonly DataType RationalSigned = new DataType("SRATIONAL", DataTypeID.RationalSigned, 8);
  internal static readonly DataType Single = new DataType("SINGLE", DataTypeID.Single, 4);
  internal static readonly DataType Double = new DataType("DOUBLE", DataTypeID.Double, 8);
  private string m_name;
  private int m_size;
  private DataTypeID m_type;

  internal static DataType FromTiffFormatCode(DataTypeID type)
  {
    switch (type)
    {
      case DataTypeID.Int8Unsigned:
        return DataType.Int8Unsigned;
      case DataTypeID.String:
        return DataType.String;
      case DataTypeID.Int16Unsigned:
        return DataType.Int16Unsigned;
      case DataTypeID.Int32Unsigned:
        return DataType.Int32Unsigned;
      case DataTypeID.RationalUnsigned:
        return DataType.RationalUnsigned;
      case DataTypeID.Int8Signed:
        return DataType.Int8Signed;
      case DataTypeID.Undefined:
        return DataType.Undefined;
      case DataTypeID.Int16Signed:
        return DataType.Int16Signed;
      case DataTypeID.Int32Signed:
        return DataType.Int32Signed;
      case DataTypeID.RationalSigned:
        return DataType.RationalSigned;
      case DataTypeID.Single:
        return DataType.Single;
      case DataTypeID.Double:
        return DataType.Double;
      default:
        return (DataType) null;
    }
  }

  internal string Name => this.m_name;

  internal int Size => this.m_size;

  internal DataTypeID Type => this.m_type;

  private DataType(string name, DataTypeID type, int size)
  {
    this.m_name = name;
    this.m_type = type;
    this.m_size = size;
  }

  public override string ToString() => this.Name;
}
