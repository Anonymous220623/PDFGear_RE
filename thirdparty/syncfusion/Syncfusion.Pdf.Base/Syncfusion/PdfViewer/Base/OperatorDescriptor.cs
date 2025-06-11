// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.OperatorDescriptor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Text;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class OperatorDescriptor
{
  internal const byte TwoByteOperatorFirstByte = 12;
  private readonly byte[] value;
  private readonly object defaultValue;
  private int hashCode;

  public object DefaultValue => this.defaultValue;

  public OperatorDescriptor(byte b0)
  {
    this.value = new byte[1];
    this.value[0] = b0;
    this.CalculateHashCode();
  }

  public OperatorDescriptor(byte[] bytes)
  {
    this.value = bytes;
    this.CalculateHashCode();
  }

  public OperatorDescriptor(byte b0, object defaultValue)
    : this(b0)
  {
    this.defaultValue = defaultValue;
  }

  public OperatorDescriptor(byte[] bytes, object defaultValue)
    : this(bytes)
  {
    this.defaultValue = defaultValue;
  }

  private void CalculateHashCode()
  {
    this.hashCode = 17;
    for (int index = 0; index < this.value.Length; ++index)
      this.hashCode = this.hashCode * 23 + (int) this.value[index];
  }

  public override bool Equals(object obj)
  {
    if (!(obj is OperatorDescriptor operatorDescriptor) || this.value.Length != operatorDescriptor.value.Length)
      return false;
    for (int index = 0; index < this.value.Length; ++index)
    {
      if ((int) this.value[index] != (int) operatorDescriptor.value[index])
        return false;
    }
    return true;
  }

  public override int GetHashCode() => this.hashCode;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (int num in this.value)
    {
      stringBuilder.Append(num);
      stringBuilder.Append(" ");
    }
    return stringBuilder.ToString().Trim();
  }
}
