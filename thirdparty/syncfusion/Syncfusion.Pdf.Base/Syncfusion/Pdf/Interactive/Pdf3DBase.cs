// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class Pdf3DBase : IPdfWrapper
{
  private string m_fileName = string.Empty;
  private Pdf3DStream m_stream = new Pdf3DStream();

  public Pdf3DStream Stream
  {
    get => this.m_stream;
    set => this.m_stream = value;
  }

  public string FileName
  {
    get => this.m_fileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArithmeticException("FileName can't be empty string.");
        default:
          this.m_fileName = Path.GetFullPath(value);
          break;
      }
    }
  }

  public Pdf3DBase(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    Utils.CheckFilePath(fileName);
    this.FileName = fileName;
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
  }

  private void Stream_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected void Save()
  {
    using (FileStream fileStream = new FileStream(this.FileName, FileMode.Open, FileAccess.Read))
    {
      byte[] bytes = Pdf3DStream.StreamToBytes((System.IO.Stream) fileStream);
      this.m_stream.Clear();
      this.m_stream.InternalStream.Write(bytes, 0, bytes.Length);
    }
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_stream;
}
