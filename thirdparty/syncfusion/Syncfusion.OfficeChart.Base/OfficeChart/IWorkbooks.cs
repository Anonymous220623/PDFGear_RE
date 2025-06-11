// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IWorkbooks
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IWorkbooks : IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  IWorkbook this[int Index] { get; }

  object Parent { get; }

  IWorkbook Create();

  IWorkbook Create(int worksheetsQuantity);

  IWorkbook Create(string[] names);

  IWorkbook Add();

  IWorkbook Add(OfficeVersion version);

  IWorkbook Add(string strTemplateFile);

  IWorkbook Add(string strTemplateFile, OfficeVersion version);

  IWorkbook Add(string strTemplateFile, OfficeParseOptions options);

  IWorkbook Add(string strTemplateFile, OfficeParseOptions options, OfficeVersion version);

  IWorkbook Open(string filename);

  IWorkbook Open(string filename, OfficeVersion version);

  IWorkbook Open(string Filename, OfficeParseOptions options);

  IWorkbook Open(string fileName, string separator, int row, int column);

  IWorkbook Open(string fileName, string separator, int row, int column, Encoding encoding);

  IWorkbook Open(string fileName, string separator);

  IWorkbook Open(string fileName, string separator, Encoding encoding);

  IWorkbook OpenReadOnly(string strFileName);

  IWorkbook OpenReadOnly(string strFileName, string seperator);

  IWorkbook OpenReadOnly(string strFileName, OfficeParseOptions options);

  IWorkbook OpenReadOnly(string strFileName, OfficeOpenType openType, OfficeParseOptions options);

  IWorkbook Open(string strFileName, OfficeParseOptions options, bool bReadOnly, string password);

  IWorkbook Open(
    string fileName,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeVersion version);

  IWorkbook Open(
    string fileName,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeOpenType openType);

  IWorkbook Open(string fileName, OfficeOpenType openType);

  IWorkbook Open(string fileName, OfficeOpenType openType, OfficeParseOptions options);

  IWorkbook Open(string fileName, OfficeOpenType openType, OfficeVersion version);

  IWorkbook OpenFromXml(string strPath, OfficeXmlOpenType openType);

  IWorkbook Open(Stream stream);

  IWorkbook Open(Stream stream, OfficeVersion version);

  IWorkbook Open(Stream stream, OfficeParseOptions options);

  IWorkbook Open(Stream stream, string separator, int row, int column);

  IWorkbook Open(Stream stream, string separator, int row, int column, Encoding encoding);

  IWorkbook Open(Stream stream, string separator);

  IWorkbook Open(Stream stream, string separator, Encoding encoding);

  IWorkbook Open(Stream stream, OfficeParseOptions options, bool bReadOnly, string password);

  IWorkbook Open(
    Stream stream,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeVersion version);

  IWorkbook Open(
    Stream stream,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeOpenType openType);

  IWorkbook Open(Stream stream, OfficeOpenType openType);

  IWorkbook Open(Stream stream, OfficeOpenType openType, OfficeParseOptions options);

  IWorkbook Open(Stream stream, OfficeOpenType openType, OfficeVersion version);

  IWorkbook OpenFromXml(Stream stream, OfficeXmlOpenType openType);

  IWorkbook OpenFromXml(XmlReader reader, OfficeXmlOpenType openType);

  void Close();

  IWorkbook PasteWorkbook();
}
