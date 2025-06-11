// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IWorkbookSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IWorkbookSerializator
{
  void Serialize(string fullName, WorkbookImpl book, ExcelSaveType saveType);

  void Serialize(Stream stream, WorkbookImpl book, ExcelSaveType saveType);
}
