// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleObjects
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OleObjects : 
  List<IOleObject>,
  IOleObjects,
  IList<IOleObject>,
  ICollection<IOleObject>,
  IEnumerable<IOleObject>,
  IEnumerable
{
  private string m_choiceRequries;
  private WorksheetImpl m_sheet;

  internal string Requries
  {
    get => this.m_choiceRequries;
    set => this.m_choiceRequries = value;
  }

  public OleObjects(WorksheetImpl sheet)
  {
    this.m_sheet = sheet != null ? sheet : throw new ArgumentNullException(nameof (sheet));
  }

  public void Add(OleObject ole)
  {
    if (ole.FileNativeData == null && ole.OleType == OleLinkType.Embed)
    {
      ole.StorageName = OleTypeConvertor.GetOleFileName();
      ole.SetOleFile(ole.FileName, ole.StorageName);
      OleObjects.SetOleObject(ole);
    }
    else if (ole.OleType == OleLinkType.Link || ole.IsStream)
      OleObjects.SetOleObject(ole);
    this.Add((IOleObject) ole);
  }

  public IOleObject Add(string fileName, Image image, OleLinkType linkType)
  {
    IPictureShape shape = this.m_sheet.Pictures.AddPicture(1, 1, image);
    if (linkType == OleLinkType.Link)
    {
      fileName = Path.GetFullPath(fileName);
      this.CreateExternalLink(fileName);
    }
    OleObject ole = new OleObject(fileName, shape, linkType);
    this.Add(ole);
    return (IOleObject) ole;
  }

  private ExternWorkbookImpl CreateExternalLink(string fileName)
  {
    WorkbookImpl parentWorkbook = this.m_sheet.ParentWorkbook;
    string fileName1 = Path.GetFileName(fileName);
    string filePath = fileName.Substring(0, fileName.Length - fileName1.Length);
    int index = parentWorkbook.ExternWorkbooks.Add(filePath, fileName1, (List<string>) null, new string[1]
    {
      "'"
    });
    ExternWorkbookImpl externWorkbook = parentWorkbook.ExternWorkbooks[index];
    externWorkbook.ProgramId = "Package";
    ExternNameRecord record = externWorkbook.ExternNames[0].Record;
    record.OleLink = true;
    record.Ole = false;
    record.WantPicture = true;
    record.WantAdvise = true;
    record.BuiltIn = false;
    return externWorkbook;
  }

  internal static void SetOleObject(OleObject ole)
  {
  }
}
