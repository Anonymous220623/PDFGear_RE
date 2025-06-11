// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IWorkbooks
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IWorkbooks : IEnumerable<IWorkbook>, IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  IWorkbook this[int Index] { get; }

  object Parent { get; }

  IWorkbook Create();

  IWorkbook Create(int worksheetsQuantity);

  IWorkbook Create(string[] names);

  IWorkbook Add();

  IWorkbook Add(ExcelVersion version);

  IWorkbook Add(string strTemplateFile);

  IWorkbook Add(string strTemplateFile, ExcelVersion version);

  IWorkbook Add(string strTemplateFile, ExcelParseOptions options);

  IWorkbook Add(string strTemplateFile, ExcelParseOptions options, ExcelVersion version);

  IWorkbook Open(string filename);

  IWorkbook Open(string filename, ExcelVersion version);

  IWorkbook Open(string Filename, ExcelParseOptions options);

  IWorkbook Open(string fileName, string separator, int row, int column);

  IWorkbook Open(string fileName, string separator, int row, int column, Encoding encoding);

  IWorkbook Open(string fileName, string separator);

  IWorkbook Open(string fileName, string separator, Encoding encoding);

  IWorkbook Open(string fileName, Encoding encoding);

  IWorkbook OpenReadOnly(string strFileName);

  IWorkbook OpenReadOnly(string strFileName, string seperator);

  IWorkbook OpenReadOnly(string strFileName, ExcelParseOptions options);

  IWorkbook OpenReadOnly(string strFileName, ExcelOpenType openType, ExcelParseOptions options);

  IWorkbook Open(string strFileName, ExcelParseOptions options, bool bReadOnly, string password);

  IWorkbook Open(
    string fileName,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelVersion version);

  IWorkbook Open(
    string fileName,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelOpenType openType);

  IWorkbook Open(string fileName, ExcelOpenType openType);

  IWorkbook Open(string fileName, ExcelOpenType openType, ExcelParseOptions options);

  IWorkbook Open(string fileName, ExcelOpenType openType, ExcelVersion version);

  IWorkbook OpenFromXml(string strPath, ExcelXmlOpenType openType);

  IWorkbook Open(Stream stream);

  IWorkbook Open(Stream stream, ExcelVersion version);

  IWorkbook Open(Stream stream, ExcelParseOptions options);

  IWorkbook Open(Stream stream, string separator, int row, int column);

  IWorkbook Open(Stream stream, string separator, int row, int column, Encoding encoding);

  IWorkbook Open(Stream stream, string separator);

  IWorkbook Open(Stream stream, string separator, Encoding encoding);

  IWorkbook Open(Stream stream, Encoding encoding);

  IWorkbook Open(Stream stream, ExcelParseOptions options, bool bReadOnly, string password);

  IWorkbook Open(
    Stream stream,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelVersion version);

  IWorkbook Open(
    Stream stream,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelOpenType openType);

  IWorkbook Open(Stream stream, ExcelOpenType openType);

  IWorkbook Open(Stream stream, ExcelOpenType openType, ExcelParseOptions options);

  IWorkbook Open(Stream stream, ExcelOpenType openType, ExcelVersion version);

  IWorkbook OpenFromXml(Stream stream, ExcelXmlOpenType openType);

  IWorkbook OpenFromXml(XmlReader reader, ExcelXmlOpenType openType);

  void Close();

  IWorkbook PasteWorkbook();
}
