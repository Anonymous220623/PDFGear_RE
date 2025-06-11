// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.WorksheetDataHolder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Xlsb;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class WorksheetDataHolder : IDisposable
{
  private const string VmlDrawingItemFormat = "xl/drawings/vmlDrawing{0}.vml";
  private const string CommentItemFormat = "xl/comments{0}.xml";
  private const string DrawingItemFormat = "xl/drawings/drawing{0}.xml";
  private const string VmlExtension = "vml";
  private List<string> m_chartRelationsToRemove;
  private ZipArchiveItem m_archiveItem;
  private FileDataHolder m_parentHolder;
  private MemoryStream m_startStream = new MemoryStream();
  internal MemoryStream m_cfStream = new MemoryStream();
  internal Stream m_cfsStream;
  private string m_strBookRelationId;
  private string m_strSheetId;
  private RelationCollection m_relations;
  private RelationCollection m_drawingsRelation;
  private RelationCollection m_hfDrawingsRelation;
  private string m_strVmlDrawingsId;
  private string m_strVmlHFDrawingsId;
  private string m_strCommentsId;
  private string m_strDrawingsId;
  private Stream m_streamControls;
  private Dictionary<string, RelationCollection> m_preservedPivotTable;
  private Dictionary<ChartStyleElements, ShapeStyle> m_defaultChartStyleElements;
  private double[][] m_defaultColorVariations;

  public WorksheetDataHolder(FileDataHolder holder, Relation relation, string parentPath)
  {
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (relation == null)
      throw new ArgumentNullException(nameof (relation));
    this.m_archiveItem = holder[relation, parentPath];
    this.m_parentHolder = holder;
  }

  public WorksheetDataHolder(FileDataHolder holder, ZipArchiveItem item)
  {
    this.m_archiveItem = item != null ? item : throw new ArgumentNullException(nameof (item));
    this.m_parentHolder = holder;
  }

  public FileDataHolder ParentHolder => this.m_parentHolder;

  public ZipArchiveItem ArchiveItem
  {
    get => this.m_archiveItem;
    set => this.m_archiveItem = value;
  }

  public string RelationId
  {
    get => this.m_strBookRelationId;
    set => this.m_strBookRelationId = value;
  }

  public string SheetId
  {
    get => this.m_strSheetId;
    set => this.m_strSheetId = value;
  }

  public RelationCollection Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new RelationCollection();
      return this.m_relations;
    }
  }

  public RelationCollection DrawingsRelations
  {
    get
    {
      if (this.m_drawingsRelation == null)
        this.m_drawingsRelation = new RelationCollection();
      return this.m_drawingsRelation;
    }
  }

  public RelationCollection HFDrawingsRelations
  {
    get
    {
      if (this.m_hfDrawingsRelation == null)
        this.m_hfDrawingsRelation = new RelationCollection();
      return this.m_hfDrawingsRelation;
    }
  }

  public string VmlDrawingsId
  {
    get => this.m_strVmlDrawingsId;
    set => this.m_strVmlDrawingsId = value;
  }

  public string VmlHFDrawingsId
  {
    get => this.m_strVmlHFDrawingsId;
    set => this.m_strVmlHFDrawingsId = value;
  }

  public string CommentNotesId
  {
    get => this.m_strCommentsId;
    set => this.m_strCommentsId = value;
  }

  public string DrawingsId
  {
    get => this.m_strDrawingsId;
    set => this.m_strDrawingsId = value;
  }

  public Stream ControlsStream
  {
    get => this.m_streamControls;
    set => this.m_streamControls = value;
  }

  internal Dictionary<ChartStyleElements, ShapeStyle> DefaultChartStyleElements
  {
    get => this.m_defaultChartStyleElements;
  }

  internal double[][] DefaultColorVariationArray => this.m_defaultColorVariations;

  internal List<string> ChartRelationsToRemove
  {
    get
    {
      if (this.m_chartRelationsToRemove == null)
        this.m_chartRelationsToRemove = new List<string>();
      return this.m_chartRelationsToRemove;
    }
  }

  internal void ParseBinaryWorksheetData(
    WorksheetImpl sheet,
    Dictionary<int, int> dictUpdateSSTIndexes,
    bool parseOnDemand,
    XlsbDataHolder dataHolder)
  {
    if (this.m_archiveItem == null)
      return;
    Excel2007Parser parser = this.m_parentHolder.Parser;
    string itemName = this.m_archiveItem.ItemName;
    int num = itemName.LastIndexOf('/');
    string strParentPath = itemName.Substring(0, num);
    string str = itemName.Insert(num, '/'.ToString() + "_rels") + ".rels";
    ZipArchiveItem zipArchiveItem = this.m_parentHolder.Archive[str];
    if (zipArchiveItem != null)
    {
      zipArchiveItem.DataStream.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream);
      this.m_relations = parser.ParseRelations(reader);
      this.m_relations.ItemPath = str;
    }
    if (sheet.ParseDataOnDemand && !sheet.ParseOnDemand)
    {
      sheet.ParseOnDemand = true;
    }
    else
    {
      if (parseOnDemand)
      {
        sheet.ParseDataOnDemand = false;
        sheet.ParseOnDemand = true;
      }
      Stream dataStream = this.m_archiveItem.DataStream;
      sheet.IsParsed = true;
      dataHolder.ParseSheet(dataStream, sheet, strParentPath, dataHolder.XFIndexes, this.m_parentHolder.ItemsToRemove, dictUpdateSSTIndexes);
      dataHolder.ItemsToRemove.Add(this.m_archiveItem.ItemName, (object) null);
      dataHolder.ItemsToRemove.Add(str, (object) null);
      if (this.m_relations != null)
        this.m_relations.Clear();
      this.m_archiveItem = (ZipArchiveItem) null;
    }
  }

  public void ParseConditionalFormatting(List<DxfImpl> dxfStyles, WorksheetImpl sheet)
  {
    if (this.m_cfStream != null && this.m_cfStream.Length != 0L)
    {
      this.m_cfStream.Position = 0L;
      Excel2007Parser parser = this.m_parentHolder.Parser;
      XmlReader reader = UtilityMethods.CreateReader((Stream) this.m_cfStream);
      if (reader.LocalName == "root")
        reader.Read();
      sheet.m_parseCondtionalFormats = false;
      sheet.m_parseCF = false;
      parser.ParseSheetConditionalFormatting(reader, sheet.ConditionalFormats, dxfStyles);
      reader.Close();
      this.m_cfStream.Close();
      this.m_cfStream = (MemoryStream) null;
    }
    if (this.m_cfsStream == null || this.m_cfsStream.Length == 0L)
      return;
    this.m_cfsStream.Position = 0L;
    Excel2007Parser parser1 = this.m_parentHolder.Parser;
    XmlReader reader1 = UtilityMethods.CreateReader(this.m_cfsStream);
    if (reader1.LocalName == "root")
      reader1.Read();
    if (reader1.LocalName == "conditionalFormattings")
    {
      while (reader1.NodeType != XmlNodeType.EndElement)
      {
        parser1.ParseSheetConditionalFormatting(reader1, sheet.ConditionalFormats, dxfStyles);
        if (reader1.NodeType == XmlNodeType.None)
          break;
      }
    }
    sheet.m_parseCondtionalFormats = false;
    this.m_cfsStream = (Stream) null;
  }

  public void ParseWorksheetData(
    WorksheetImpl sheet,
    Dictionary<int, int> dictUpdateSSTIndexes,
    bool parseOnDemand)
  {
    if (this.m_archiveItem == null)
      return;
    Excel2007Parser parser = this.m_parentHolder.Parser;
    string itemName = this.m_archiveItem.ItemName;
    int num = itemName.LastIndexOf('/');
    string strParentPath = itemName.Substring(0, num);
    string str = itemName.Insert(num, '/'.ToString() + "_rels") + ".rels";
    ZipArchiveItem zipArchiveItem = this.m_parentHolder.Archive[str];
    if (zipArchiveItem != null)
    {
      zipArchiveItem.DataStream.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream);
      this.m_relations = parser.ParseRelations(reader);
      this.m_relations.ItemPath = str;
    }
    this.m_archiveItem.OptimizedDecompress = true;
    if (sheet.ParseDataOnDemand && !sheet.ParseOnDemand)
    {
      sheet.ParseOnDemand = true;
    }
    else
    {
      if (parseOnDemand)
      {
        sheet.ParseDataOnDemand = false;
        sheet.ParseOnDemand = true;
      }
      XmlReader reader = UtilityMethods.CreateReader(this.m_archiveItem.DataStream);
      parser.ParseSheet(reader, sheet, strParentPath, ref this.m_startStream, ref this.m_cfStream, this.m_parentHolder.XFIndexes, this.m_parentHolder.ItemsToRemove, dictUpdateSSTIndexes);
      if (this.m_relations != null && this.m_relations.Count > 0)
        this.CollectPivotRelations(itemName);
      this.m_parentHolder.ItemsToRemove.Add(this.m_archiveItem.ItemName, (object) null);
      this.m_parentHolder.ItemsToRemove.Add(str, (object) null);
      this.m_archiveItem = (ZipArchiveItem) null;
    }
  }

  public void CollectPivotRelations(string itemName)
  {
    this.m_preservedPivotTable = new Dictionary<string, RelationCollection>();
    RelationCollection relationCollection1 = new RelationCollection();
    RelationCollection relationCollection2 = new RelationCollection();
    foreach (KeyValuePair<string, Relation> relation in this.m_relations)
    {
      if (relation.Value.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable")
        relationCollection1.Add(relation.Value);
      relationCollection1.ItemPath = this.m_relations.ItemPath;
    }
    this.m_preservedPivotTable.Add(itemName, relationCollection1);
  }

  public void ParsePivotTable(IWorksheet sheet)
  {
    if (this.m_preservedPivotTable == null || this.m_preservedPivotTable.Count <= 0)
      return;
    foreach (KeyValuePair<string, RelationCollection> keyValuePair in this.m_preservedPivotTable)
    {
      string key = keyValuePair.Key;
      int length = key.LastIndexOf('/');
      string strParentPath = key.Substring(0, length);
      this.ParsePivotTables(sheet, strParentPath, keyValuePair.Value);
    }
  }

  public void ParseChartsheetData(ChartImpl chart)
  {
    Excel2007Parser parser = this.m_parentHolder.Parser;
    string itemName = this.m_archiveItem.ItemName;
    int length = itemName.LastIndexOf('/');
    itemName.Substring(0, length);
    this.m_relations = this.m_parentHolder.ParseRelations(FileDataHolder.GetCorrespondingRelations(itemName));
    XmlReader reader = UtilityMethods.CreateReader(this.m_archiveItem.DataStream);
    parser.ParseChartsheet(reader, chart);
  }

  internal Stream ParseDialogMacrosheetData()
  {
    Stream dialogMacrosheetData = (Stream) null;
    if (this.m_archiveItem != null && this.m_archiveItem.DataStream.Length > 0L)
    {
      Excel2007Parser parser = this.m_parentHolder.Parser;
      string itemName1 = this.m_archiveItem.ItemName;
      int num = itemName1.LastIndexOf('/');
      itemName1.Substring(0, num);
      string itemName2 = itemName1.Insert(num, '/'.ToString() + "_rels") + ".rels";
      ZipArchiveItem zipArchiveItem = this.m_parentHolder.Archive[itemName2];
      if (zipArchiveItem != null)
      {
        zipArchiveItem.DataStream.Position = 0L;
        XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream);
        this.m_relations = parser.ParseRelations(reader);
        this.m_relations.ItemPath = itemName2;
      }
      this.m_archiveItem.OptimizedDecompress = true;
      XmlReader reader1 = UtilityMethods.CreateReader(this.m_archiveItem.DataStream);
      while (reader1.NodeType != XmlNodeType.Element)
        reader1.Read();
      if (reader1.LocalName == "dialogsheet" || reader1.LocalName == "macrosheet")
        dialogMacrosheetData = ShapeParser.ReadNodeAsStream(reader1);
      if (this.m_relations != null && this.m_relations.Count > 0)
        this.CollectPivotRelations(itemName1);
      this.m_archiveItem = (ZipArchiveItem) null;
    }
    return dialogMacrosheetData;
  }

  public void SerializeWorksheet(
    WorksheetImpl sheet,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    this.SerializeWorksheetPart(sheet, hashNewXFIndexes);
    this.SerializeWorksheetDrawings((WorksheetBaseImpl) sheet);
    this.SerializeComments(sheet);
    this.SerializeHeaderFooterImages((WorksheetBaseImpl) sheet, (RelationCollection) null);
    this.SerializeOleStreamFile(sheet);
    this.SerializePivotTables(sheet, cacheFiles);
    this.SerializeWorksheetRelations();
  }

  private void SerializeOleStreamFile(WorksheetImpl sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!sheet.HasOleObject)
      return;
    foreach (OleObject oleObject in (List<IOleObject>) sheet.OleObjects)
    {
      if (oleObject.OleType == OleLinkType.Embed)
        this.SerializeOle(sheet, oleObject);
    }
  }

  private void SerializeOle(WorksheetImpl sheet, OleObject oleObject)
  {
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    if (oleObject.StorageName == null)
      oleObject.StorageName = OleTypeConvertor.GetOleFileName();
    string itemName = "xl/embeddings/" + oleObject.StorageName;
    MemoryStream newDataStream = !oleObject.IsContainer ? (MemoryStream) this.GetOleDataStreamBin(oleObject, serializator) : (MemoryStream) oleObject.Container;
    this.m_parentHolder.Archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
    this.Relations[oleObject.ShapeRId] = new Relation('/'.ToString() + itemName, oleObject.RelationType);
    this.m_parentHolder.OverriddenContentTypes['/'.ToString() + itemName] = oleObject.ContentType;
  }

  private Stream GetOleDataStreamBin(OleObject oleObject, Excel2007Serializator serializator)
  {
    string storageName = oleObject.StorageName;
    oleObject.Container.Position = 0L;
    if (oleObject.Container == null)
      throw new Exception("Null value");
    ICompoundStorage compoundStorage = new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile(oleObject.Container).RootStorage.OpenStorage(storageName);
    Syncfusion.CompoundFile.XlsIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile();
    compoundFile.Directory.Entries[0].StorageGuid = OleTypeConvertor.GetGUID();
    int index = 0;
    for (int length = compoundStorage.Streams.Length; index < length; ++index)
    {
      CompoundStream stream = compoundFile.RootStorage.CreateStream(compoundStorage.Streams[index]);
      CompoundStream compoundStream = compoundStorage.OpenStream(compoundStorage.Streams[index]);
      byte[] buffer = new byte[compoundStream.Length];
      compoundStream.Read(buffer, 0, buffer.Length);
      stream.Write(buffer, 0, buffer.Length);
      compoundStream.Close();
      compoundStream.Dispose();
      stream.Close();
      stream.Dispose();
    }
    MemoryStream oleDataStreamBin = new MemoryStream();
    compoundFile.Save((Stream) oleDataStreamBin);
    compoundFile.Dispose();
    oleDataStreamBin.Position = 0L;
    return (Stream) oleDataStreamBin;
  }

  public void SerializeChartsheet(ChartImpl chart)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    this.SerializeChartsheetPart(chart);
    this.SerializeVmlDrawings((WorksheetBaseImpl) chart);
    this.SerializeHeaderFooterImages((WorksheetBaseImpl) chart, (RelationCollection) null);
    this.SerializeWorksheetRelations();
  }

  internal void SerializeDialogsheet(Stream DialogStream, WorkbookImpl book)
  {
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    ZippedContentStream zippedContentStream = new ZippedContentStream(new ZipArchive.CompressorCreator(book.AppImplementation.CreateCompressor));
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) zippedContentStream));
    serializator.SerializeDialogMacroStream(writer, DialogStream);
    writer.Flush();
    zippedContentStream.Flush();
  }

  internal void SerializeMacrosheet(Stream MacroStream, WorkbookImpl book)
  {
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    ZippedContentStream zippedContentStream = new ZippedContentStream(new ZipArchive.CompressorCreator(book.AppImplementation.CreateCompressor));
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) zippedContentStream));
    serializator.SerializeDialogMacroStream(writer, MacroStream);
    writer.Flush();
    zippedContentStream.Flush();
  }

  public RelationCollection ParseVmlShapes(
    ShapeCollectionBase shapes,
    string relationId,
    RelationCollection relations)
  {
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    RelationCollection relations1 = (RelationCollection) null;
    if (relations == null)
      relations = this.m_relations;
    if (relations != null)
    {
      Relation relation = relations[relationId];
      if (relation == null)
        throw new ArgumentException(nameof (relationId));
      string parentItemPath = Path.GetDirectoryName(this.m_archiveItem.ItemName).Replace('\\', '/');
      string strItemPath;
      XmlReader readerAndFixBr = this.m_parentHolder.CreateReaderAndFixBr(relation, parentItemPath, out strItemPath);
      relations1 = this.m_parentHolder.ParseRelations(FileDataHolder.GetCorrespondingRelations(strItemPath));
      int length = strItemPath.LastIndexOf('/');
      if (length >= 0)
        strItemPath = strItemPath.Substring(0, length);
      this.m_parentHolder.Parser.ParseVmlShapes(readerAndFixBr, shapes, relations1, strItemPath);
      WorksheetImpl worksheet = shapes.Worksheet;
      if (worksheet != null)
      {
        IComments comments = worksheet.Comments;
        if (comments != null)
        {
          int count = comments.Count;
        }
        Relation relationByContentType = this.m_relations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments", out this.m_strCommentsId);
        if (relationByContentType != null)
          this.m_parentHolder.Parser.ParseComments(this.m_parentHolder.CreateReader(relationByContentType, parentItemPath), worksheet);
        else
          worksheet.Comments.Clear();
      }
    }
    return relations1;
  }

  public void ParseOleData(WorksheetBaseImpl sheet, string relationId, OleObject oleObject)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (this.m_relations == null)
      return;
    Relation relation = this.m_relations[relationId];
    if (relation == null)
      throw new ArgumentException(nameof (relationId));
    string str = Path.GetDirectoryName(this.m_archiveItem.ItemName).Replace('\\', '/');
    byte[] data = sheet.DataHolder.ParentHolder.GetData(relation, str, true);
    string target = relation.Target;
    string path = FileDataHolder.CombinePath(str, target).Replace('\\', '/');
    string contentType = this.m_parentHolder.GetContentType('/'.ToString() + path);
    oleObject.ContentType = contentType;
    oleObject.RelationType = relation.Type;
    if (oleObject.OleObjectType == OleTypeConvertor.ToOleType("Document"))
    {
      oleObject.Container = (Stream) new MemoryStream(data);
      oleObject.Container.Position = 0L;
      oleObject.IsContainer = true;
      oleObject.FileNativeData = new byte[0];
      string fileName = Path.GetFileName(path);
      oleObject.StorageName = fileName;
    }
    else
    {
      oleObject.Container = (Stream) new MemoryStream(data);
      oleObject.Container.Position = 0L;
      oleObject.IsContainer = true;
      oleObject.FileNativeData = new byte[0];
      oleObject.StorageName = Path.GetFileName(path);
    }
  }

  private bool GetOleObjectType(string oleType)
  {
    bool oleObjectType = false;
    switch (oleType)
    {
      case "PowerPoint.Show.8":
      case "PowerPoint.Slide.12":
      case "PowerPoint.Show.12":
      case "Word.DocumentMacroEnabled.12":
      case "PowerPoint.SlideMacroEnabled.12":
      case "Word.Document.12":
      case "PowerPoint.ShowMacroEnabled.12":
      case "Word.Document.8":
        return true;
      default:
        return oleObjectType;
    }
  }

  public void ParseDrawings(
    WorksheetBaseImpl sheet,
    string relationId,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (this.m_relations == null)
      return;
    Relation relation = this.m_relations[relationId];
    this.ParseDrawings(sheet, relation, dictItemsToRemove, false);
  }

  public void ParseDrawings(
    WorksheetBaseImpl sheet,
    Relation drawingRelation,
    Dictionary<string, object> dictItemsToRemove)
  {
    this.ParseDrawings(sheet, drawingRelation, dictItemsToRemove, false);
  }

  internal void ParseDrawings(
    WorksheetBaseImpl sheet,
    Relation drawingRelation,
    Dictionary<string, object> dictItemsToRemove,
    bool isChartShape)
  {
    if (drawingRelation == null)
      throw new ArgumentException("relationId");
    string str = Path.GetDirectoryName(this.m_archiveItem.ItemName).Replace('\\', '/');
    XmlReader reader = this.m_parentHolder.CreateReader(drawingRelation, str);
    string itemName = FileDataHolder.CombinePath(str, drawingRelation.Target);
    Excel2007Parser parser = this.m_parentHolder.Parser;
    string correspondingRelations = FileDataHolder.GetCorrespondingRelations(itemName);
    RelationCollection relations = this.m_parentHolder.ParseRelations(correspondingRelations);
    bool flag = sheet is ChartImpl;
    RelationCollection relationCollection = (RelationCollection) null;
    if (relations != null)
    {
      if (flag && this.m_drawingsRelation != null)
        relationCollection = this.m_drawingsRelation;
      this.m_drawingsRelation = relations;
    }
    string path;
    FileDataHolder.SeparateItemName(itemName, out path);
    List<string> lstRelationIds = new List<string>();
    parser.ParseDrawings(reader, sheet, path, lstRelationIds, dictItemsToRemove, isChartShape);
    ZipArchive archive = this.m_parentHolder.Archive;
    archive.RemoveItem(correspondingRelations);
    archive.RemoveItem(itemName);
    if (this.m_drawingsRelation != null)
    {
      int index = 0;
      for (int count = lstRelationIds.Count; index < count; ++index)
        this.m_drawingsRelation.Remove(lstRelationIds[index]);
    }
    if (!flag || relationCollection == null)
      return;
    this.m_drawingsRelation = relationCollection;
  }

  private void SerializeWorksheetPart(WorksheetImpl sheet, Dictionary<int, int> hashNewXFIndexes)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.ParseDataOnDemand)
      return;
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    ZippedContentStream stream = new ZippedContentStream(new ZipArchive.CompressorCreator(sheet.AppImplementation.CreateCompressor));
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) stream));
    serializator.SerializeWorksheet(writer, sheet, (Stream) this.m_startStream, (Stream) this.m_cfStream, hashNewXFIndexes, this.m_cfsStream);
    writer.Flush();
    stream.Flush();
    this.m_archiveItem.Update(stream);
  }

  private void SerializeChartsheetPart(ChartImpl chart)
  {
    string str = chart != null ? this.GenerateDrawingsName(chart.Workbook) : throw new ArgumentNullException(nameof (chart));
    string relationId = this.Relations.GenerateRelationId();
    this.m_relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing");
    this.m_parentHolder.OverriddenContentTypes['/'.ToString() + str] = "application/vnd.openxmlformats-officedocument.drawing+xml";
    ChartSerializator chartSerializator = new ChartSerializator();
    ZippedContentStream stream = new ZippedContentStream(new ZipArchive.CompressorCreator(chart.AppImplementation.CreateCompressor));
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) stream));
    chartSerializator.SerializeChartsheet(writer, chart, relationId);
    writer.Flush();
    stream.Flush();
    this.m_archiveItem.Update(stream);
    RelationCollection relationCollection = new RelationCollection();
    string dummyChartRelation = (string) null;
    string chartSheetDrawingId = this.SerializeChartSheetDrawing(chart, str, relationCollection, out dummyChartRelation);
    this.SerializeChartObject(chart, relationCollection, chartSheetDrawingId, dummyChartRelation);
    this.SerializeRelations(relationCollection, str, (WorksheetDataHolder) null, (WorksheetBaseImpl) chart);
  }

  private void SerializeChartObject(
    ChartImpl chart,
    RelationCollection drawingRelations,
    string chartSheetDrawingId,
    string dummyChartRelationId)
  {
    string str1 = (string) null;
    bool flag = ChartImpl.IsChartExSerieType(chart.ChartType);
    string type = flag ? "http://schemas.microsoft.com/office/2014/relationships/chartEx" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart";
    string str2 = flag ? "application/vnd.ms-office.chartex+xml" : "application/vnd.openxmlformats-officedocument.drawingml.chart+xml";
    string str3;
    if (flag)
    {
      str3 = ChartShapeSerializator.GetChartExFileName(this, chart);
      str1 = ChartShapeSerializator.GetChartFileName(this, chart);
      drawingRelations[dummyChartRelationId] = new Relation(str1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart");
      this.m_parentHolder.OverriddenContentTypes[str1] = "application/vnd.openxmlformats-officedocument.drawingml.chart+xml";
    }
    else
      str3 = ChartShapeSerializator.GetChartFileName(this, chart);
    drawingRelations[chartSheetDrawingId] = new Relation(str3, type);
    this.m_parentHolder.OverriddenContentTypes[str3] = str2;
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    if (chart.DataHolder == null)
      this.m_parentHolder.CreateDataHolder((WorksheetBaseImpl) chart, str3);
    if (flag)
    {
      new ChartExSerializator().SerializeChartEx(writer, chart);
      this.SerializeDummyChartForChartEx(chart.ParentWorkbook, this.m_parentHolder, str1, dummyChartRelationId);
    }
    else
      new ChartSerializator().SerializeChart(writer, chart, str3);
    writer.Flush();
    data.Flush();
    string str4 = UtilityMethods.RemoveFirstCharUnsafe(str3);
    this.m_parentHolder.Archive.UpdateItem(str4, (Stream) newDataStream, true, FileAttributes.Archive);
    this.SerializeRelations(chart.Relations, str4, (WorksheetDataHolder) null, (WorksheetBaseImpl) chart);
  }

  private void SerializeDummyChartForChartEx(
    WorkbookImpl workbook,
    FileDataHolder holder,
    string chartName,
    string chartRelationId)
  {
    ChartImpl chartImpl = workbook.Charts.Add() as ChartImpl;
    chartImpl.ChartType = ExcelChartType.Column_Clustered;
    if (chartImpl.DataHolder == null)
      holder.CreateDataHolder((WorksheetBaseImpl) chartImpl, chartName);
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    new ChartSerializator(true).SerializeChart(writer, chartImpl, chartName);
    writer.Flush();
    data.Flush();
    chartName = UtilityMethods.RemoveFirstCharUnsafe(chartName);
    holder.Archive.UpdateItem(chartName, (Stream) newDataStream, true, FileAttributes.Archive);
    workbook.Charts.Remove(chartImpl.Name);
  }

  private string SerializeChartSheetDrawing(
    ChartImpl chart,
    string drawingItemName,
    RelationCollection drawingRelations,
    out string dummyChartRelation)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (drawingRelations == null)
      throw new ArgumentNullException();
    if (drawingItemName == null || drawingItemName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (drawingItemName));
    ShapeCollectionBase innerShapesBase = chart.InnerShapesBase;
    dummyChartRelation = (string) null;
    string relationId = drawingRelations.GenerateRelationId();
    drawingRelations[relationId] = (Relation) null;
    string str = "";
    if (ChartImpl.IsChartExSerieType(chart.ChartType))
    {
      dummyChartRelation = drawingRelations.GenerateRelationId();
      str = ";" + dummyChartRelation;
      drawingRelations[dummyChartRelation] = (Relation) null;
    }
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    new ChartSerializator().SerializeChartsheetDrawing(writer, chart, relationId + str);
    writer.Flush();
    data.Flush();
    this.m_parentHolder.Archive.UpdateItem(drawingItemName, (Stream) newDataStream, true, FileAttributes.Archive);
    return relationId;
  }

  private void SerializePivotTables(
    WorksheetImpl sheet,
    Dictionary<PivotCacheImpl, string> dictCacheFiles)
  {
    PivotTableCollection innerPivotTables = sheet.InnerPivotTables;
    if (innerPivotTables == null || innerPivotTables.Count <= 0)
      return;
    int index = 0;
    for (int count = innerPivotTables.Count; index < count; ++index)
      this.SerializePivotTable((PivotTableImpl) innerPivotTables[index], dictCacheFiles);
  }

  private void SerializePivotTable(
    PivotTableImpl table,
    Dictionary<PivotCacheImpl, string> dictCacheFiles)
  {
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    string pivotTableName = this.m_parentHolder.GeneratePivotTableName(table.Workbook.LastPivotTableIndex++ + table.Workbook.PivotTableCount);
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    PivotTableSerializator.SerializePivotTable(writer, table);
    writer.Flush();
    this.m_parentHolder.Archive.UpdateItem(pivotTableName, (Stream) memoryStream, true, FileAttributes.Archive);
    this.m_parentHolder.AddOverriddenContentType(pivotTableName, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotTable+xml");
    string relationId1 = this.Relations.GenerateRelationId();
    if (this.Relations.FindRelationByTarget("/" + pivotTableName) == null)
      this.Relations[relationId1] = new Relation('/'.ToString() + pivotTableName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable");
    RelationCollection relations = new RelationCollection();
    string relationId2 = relations.GenerateRelationId();
    string dictCacheFile = dictCacheFiles[table.Cache];
    relations[relationId2] = new Relation('/'.ToString() + dictCacheFile, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheDefinition");
    this.m_parentHolder.SaveRelations(pivotTableName, relations);
  }

  internal void SerializeWorksheetRelations()
  {
    if (this.m_relations == null || this.m_relations.Count <= 0)
      return;
    string itemName1 = this.m_archiveItem.ItemName;
    int startIndex = itemName1.LastIndexOf('/');
    string itemName2 = itemName1.Insert(startIndex, '/'.ToString() + "_rels") + ".rels";
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    this.m_relations.ItemPath = itemName2;
    serializator.SerializeRelations(writer, this.m_relations, (WorksheetDataHolder) null);
    writer.Flush();
    data.Flush();
    this.m_parentHolder.Archive.UpdateItem(itemName2, (Stream) newDataStream, true, FileAttributes.Archive);
  }

  private void SerializeWorksheetDrawings(WorksheetBaseImpl sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int count = sheet.Shapes.Count;
    if (count == 0 && sheet is IWorksheet && !sheet.UnknownVmlShapes)
    {
      if (this.m_strDrawingsId != null)
      {
        this.m_relations.Remove(this.m_strDrawingsId);
        this.m_strDrawingsId = (string) null;
      }
      if (!sheet.HasVmlShapes)
        return;
      string vmlDrawingsName = this.GenerateVmlDrawingsName();
      if (this.m_parentHolder.Archive[vmlDrawingsName] == null)
        return;
      this.m_parentHolder.Archive.UpdateItem(vmlDrawingsName, (Stream) null, true, FileAttributes.Archive);
    }
    else
    {
      if (count == 0 && !sheet.UnknownVmlShapes)
        return;
      this.SerializeVmlDrawings(sheet);
      if (this.SerializeDrawings(sheet) || this.m_strDrawingsId == null)
        return;
      this.m_relations.Remove(this.m_strDrawingsId);
      this.m_strDrawingsId = (string) null;
    }
  }

  public bool SerializeDrawings(WorksheetBaseImpl sheet)
  {
    return this.SerializeDrawings(sheet, this.Relations, ref this.m_strDrawingsId, "application/vnd.openxmlformats-officedocument.drawing+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing");
  }

  public bool SerializeDrawings(
    WorksheetBaseImpl sheet,
    RelationCollection relations,
    ref string id,
    string contentType,
    string relationType)
  {
    ShapesCollection shapes = sheet != null ? sheet.InnerShapes : throw new ArgumentNullException(nameof (sheet));
    int count = sheet is WorksheetImpl worksheetImpl ? worksheetImpl.AutoFilters.Count : 0;
    int num = 0;
    if (worksheetImpl != null && worksheetImpl.ListObjects.Count > 0)
    {
      foreach (IListObject listObject in (IEnumerable<IListObject>) worksheetImpl.ListObjects)
      {
        if (listObject.ShowAutoFilter)
          num += listObject.AutoFilters.Count;
      }
    }
    if (shapes.Count - sheet.VmlShapesCount - count - num <= 0 && !Excel2007Serializator.HasAlternateContent((IShapes) shapes))
      return false;
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    this.m_parentHolder.Serializator.SerializeDrawings(writer, shapes, this);
    writer.Flush();
    data.Flush();
    newDataStream.Flush();
    string drawingsName = this.GenerateDrawingsName(sheet.Workbook);
    this.m_parentHolder.Archive.UpdateItem(drawingsName, (Stream) newDataStream, true, FileAttributes.Archive);
    string str = '/'.ToString() + drawingsName;
    relations[id] = new Relation(str, relationType);
    this.m_parentHolder.OverriddenContentTypes[str] = contentType;
    if (sheet is WorksheetImpl)
    {
      if (this.m_drawingsRelation != null && this.m_drawingsRelation.Count > 0)
        this.SerializeRelations(this.m_drawingsRelation, drawingsName, (WorksheetDataHolder) null, (WorksheetBaseImpl) null);
      foreach (string id1 in this.ChartRelationsToRemove)
        this.m_drawingsRelation.Remove(id1);
      this.ChartRelationsToRemove.Clear();
    }
    else if (this.m_drawingsRelation != null && this.m_drawingsRelation.Count > 0 && sheet.DataHolder.m_drawingsRelation.Count == this.m_drawingsRelation.Count)
      this.SerializeRelations(this.m_drawingsRelation, drawingsName, (WorksheetDataHolder) null, sheet);
    return true;
  }

  internal void SerializeChartExFallbackShape(
    WorksheetBaseImpl sheet,
    RelationCollection relations,
    ref string id,
    string chartItemName,
    string contentType,
    string relationType)
  {
    ShapesCollection shapesCollection = sheet != null ? sheet.InnerShapes : throw new ArgumentNullException(nameof (sheet));
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    Excel2007Serializator serializator = this.m_parentHolder.Serializator;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("c", "userShapes", "http://schemas.openxmlformats.org/drawingml/2006/chart");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("cdr", "relSizeAnchor", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
    writer.WriteAttributeString("xmlns", "cdr", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
    writer.WriteStartElement("from", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
    writer.WriteElementString("x", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing", "0");
    writer.WriteElementString("y", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing", "0");
    writer.WriteEndElement();
    writer.WriteStartElement("to", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing");
    writer.WriteElementString("x", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing", "1");
    writer.WriteElementString("y", "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing", "1");
    writer.WriteEndElement();
    this.SerializeChartExFallBackShapeContent(writer, true);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.Flush();
    data.Flush();
    newDataStream.Flush();
    string drawingsName = this.GenerateDrawingsName(sheet.Workbook);
    this.m_parentHolder.Archive.UpdateItem(drawingsName, (Stream) newDataStream, true, FileAttributes.Archive);
    string str = '/'.ToString() + drawingsName;
    relations[id] = new Relation(str, relationType);
    this.m_parentHolder.OverriddenContentTypes[str] = contentType;
    this.SerializeRelations(relations, chartItemName, (WorksheetDataHolder) null, (WorksheetBaseImpl) null);
  }

  internal void SerializeChartExFallBackShapeContent(XmlWriter writer, bool isChartSheet)
  {
    string ns = isChartSheet ? "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
    writer.WriteStartElement("sp", ns);
    Excel2007Serializator.SerializeAttribute(writer, "macro", "", (string) null);
    Excel2007Serializator.SerializeAttribute(writer, "textlink", "", (string) null);
    writer.WriteStartElement("nvSpPr", ns);
    writer.WriteStartElement("cNvPr", ns);
    writer.WriteAttributeString("id", "0");
    writer.WriteAttributeString("name", "");
    writer.WriteEndElement();
    writer.WriteStartElement("cNvSpPr", ns);
    writer.WriteStartElement("a", "spLocks", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Excel2007Serializator.SerializeAttribute(writer, "noTextEdit", true, false);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("spPr", ns);
    if (isChartSheet)
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 0, 8666049, 6293304);
    else
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", 0, 0, 0, 0);
    writer.WriteStartElement("prstGeom", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("prst", "rect");
    writer.WriteStartElement("avLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("prstClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "white");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("w", "1");
    writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("prstClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "green");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteStartElement("txBody", ns);
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("vertOverflow", "clip");
    writer.WriteAttributeString("horzOverflow", "clip");
    writer.WriteEndElement();
    writer.WriteStartElement("lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteStartElement("rPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("lang", "en-US");
    writer.WriteAttributeString("sz", "1100");
    writer.WriteEndElement();
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString("This chart isn't available in your version of Excel. Editing this shape or saving this workbook into a different file format will permanently break the chart.");
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeVmlDrawings(WorksheetBaseImpl sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!sheet.HasVmlShapes)
    {
      if (this.m_strVmlDrawingsId == null)
        return;
      this.m_relations.Remove(this.m_strVmlDrawingsId);
      this.m_strVmlDrawingsId = (string) null;
    }
    else
    {
      this.m_parentHolder.DefaultContentTypes["vml"] = "application/vnd.openxmlformats-officedocument.vmlDrawing";
      Excel2007Serializator serializator = this.m_parentHolder.Serializator;
      string vmlDrawingsName = this.GenerateVmlDrawingsName();
      RelationCollection relationCollection = new RelationCollection();
      MemoryStream newDataStream = new MemoryStream();
      StreamWriter data = new StreamWriter((Stream) newDataStream);
      XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data, true);
      serializator.SerializeVmlShapes(writer, (ShapeCollectionBase) sheet.InnerShapes, this, serializator.VmlSerializators, relationCollection);
      data.Flush();
      newDataStream.Flush();
      this.m_parentHolder.Archive.UpdateItem(vmlDrawingsName, (Stream) newDataStream, true, FileAttributes.Archive);
      this.Relations[this.m_strVmlDrawingsId] = new Relation('/'.ToString() + vmlDrawingsName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
      this.SerializeRelations(relationCollection, vmlDrawingsName, (WorksheetDataHolder) null, (WorksheetBaseImpl) null);
    }
  }

  public void SerializeHeaderFooterImages(WorksheetBaseImpl sheet, RelationCollection relations)
  {
    HeaderFooterShapeCollection shapes = sheet != null ? sheet.InnerHeaderFooterShapes : throw new ArgumentNullException(nameof (sheet));
    if (relations == null)
      relations = this.Relations;
    if (shapes == null || shapes.Count == 0)
    {
      if (this.m_strVmlHFDrawingsId == null || relations[this.m_strVmlHFDrawingsId] == null || !relations[this.m_strVmlHFDrawingsId].Target.Contains("vmlDrawing"))
        return;
      relations.Remove(this.m_strVmlHFDrawingsId);
      this.m_strVmlHFDrawingsId = (string) null;
    }
    else
    {
      this.m_parentHolder.DefaultContentTypes["vml"] = "application/vnd.openxmlformats-officedocument.vmlDrawing";
      Excel2007Serializator serializator = this.m_parentHolder.Serializator;
      string vmlDrawingsName = this.GenerateVmlDrawingsName();
      MemoryStream newDataStream = new MemoryStream();
      StreamWriter data = new StreamWriter((Stream) newDataStream);
      XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data, true);
      serializator.SerializeVmlShapes(writer, (ShapeCollectionBase) shapes, this, serializator.HFVmlSerializators, relations);
      data.Flush();
      newDataStream.Flush();
      this.m_parentHolder.Archive.UpdateItem(vmlDrawingsName, (Stream) newDataStream, true, FileAttributes.Archive);
      relations[this.m_strVmlHFDrawingsId] = new Relation('/'.ToString() + vmlDrawingsName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
      this.SerializeRelations(this.m_hfDrawingsRelation, vmlDrawingsName, (WorksheetDataHolder) null, (WorksheetBaseImpl) null);
    }
  }

  public void SerializeRelations(string strParentItemName)
  {
    if (this.m_relations == null || this.m_relations.Count <= 0)
      return;
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    this.m_parentHolder.Serializator.SerializeRelations(writer, this.m_relations, (WorksheetDataHolder) null);
    writer.Flush();
    data.Flush();
    newDataStream.Flush();
    int startIndex = strParentItemName.LastIndexOf('/');
    this.m_parentHolder.Archive.UpdateItem(strParentItemName.Insert(startIndex, '/'.ToString() + "_rels") + ".rels", (Stream) newDataStream, true, FileAttributes.Archive);
  }

  public void SerializeRelations(
    RelationCollection relations,
    string strParentItemName,
    WorksheetDataHolder holder)
  {
    this.SerializeRelations(relations, strParentItemName, holder, (WorksheetBaseImpl) null);
  }

  internal void SerializeRelations(
    RelationCollection relations,
    string strParentItemName,
    WorksheetDataHolder holder,
    WorksheetBaseImpl chart)
  {
    if (strParentItemName == null || strParentItemName.Length == 0)
      throw new ArgumentOutOfRangeException(strParentItemName);
    relations = this.SerializeChartExStyles(relations, strParentItemName, holder, chart);
    if (relations != null && relations.Count > 0)
    {
      MemoryStream newDataStream = new MemoryStream();
      StreamWriter data = new StreamWriter((Stream) newDataStream);
      XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
      this.m_parentHolder.Serializator.SerializeRelations(writer, relations, holder);
      writer.Flush();
      data.Flush();
      newDataStream.Flush();
      int startIndex = strParentItemName.LastIndexOf('/');
      if (strParentItemName[0] == '/')
      {
        strParentItemName = UtilityMethods.RemoveFirstCharUnsafe(strParentItemName);
        --startIndex;
      }
      this.m_parentHolder.Archive.UpdateItem(strParentItemName.Insert(startIndex, '/'.ToString() + "_rels") + ".rels", (Stream) newDataStream, true, FileAttributes.Archive);
    }
    else
    {
      if (!new Regex("^\\/{0,1}xl\\/charts\\/chart\\d+\\.xml").Match(strParentItemName).Success)
        return;
      int startIndex = strParentItemName.LastIndexOf('/');
      if (strParentItemName[0] == '/')
      {
        strParentItemName = UtilityMethods.RemoveFirstCharUnsafe(strParentItemName);
        --startIndex;
      }
      this.m_parentHolder.Archive.RemoveItem(strParentItemName.Insert(startIndex, '/'.ToString() + "_rels") + ".rels");
    }
  }

  private RelationCollection SerializeChartExStyles(
    RelationCollection relations,
    string strParentItemName,
    WorksheetDataHolder holder,
    WorksheetBaseImpl chart)
  {
    if (chart is ChartImpl chart1 && strParentItemName.ToLower().Contains("chartex") && ChartImpl.IsChartExSerieType(chart1.ChartType))
    {
      if (!chart1.m_isChartStyleSkipped)
      {
        if (relations == null)
          relations = new RelationCollection();
        string relationId = relations.GenerateRelationId();
        string fileName = this.TryAndGetFileName(UtilityMethods.RemoveFirstCharUnsafe("/xl/charts/style{0}.xml"), this.m_parentHolder.Archive);
        string str = "/" + fileName;
        relations[relationId] = new Relation(str, "http://schemas.microsoft.com/office/2011/relationships/chartStyle");
        MemoryStream newDataStream = new MemoryStream();
        StreamWriter data = new StreamWriter((Stream) newDataStream);
        this.SerializeDefaultChartStyles(UtilityMethods.CreateWriter((TextWriter) data), chart1, chart1.AppImplementation);
        data.Flush();
        newDataStream.Flush();
        this.m_parentHolder.Archive.UpdateItem(fileName, (Stream) newDataStream, true, FileAttributes.Archive);
        this.m_parentHolder.OverriddenContentTypes[str] = "application/vnd.ms-office.chartstyle+xml";
      }
      if (!chart1.m_isChartColorStyleSkipped)
      {
        if (relations == null)
          relations = new RelationCollection();
        string relationId = relations.GenerateRelationId();
        string fileName = this.TryAndGetFileName(UtilityMethods.RemoveFirstCharUnsafe("/xl/charts/colors{0}.xml"), this.m_parentHolder.Archive);
        string str = "/" + fileName;
        relations[relationId] = new Relation(str, "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle");
        MemoryStream newDataStream = new MemoryStream();
        StreamWriter data = new StreamWriter((Stream) newDataStream);
        this.SerializeDefaultChartColorStyles(UtilityMethods.CreateWriter((TextWriter) data), chart1.AppImplementation);
        data.Flush();
        newDataStream.Flush();
        this.m_parentHolder.Archive.UpdateItem(fileName, (Stream) newDataStream, true, FileAttributes.Archive);
        this.m_parentHolder.OverriddenContentTypes[str] = "application/vnd.ms-office.chartcolorstyle+xml";
      }
    }
    return relations;
  }

  private string TryAndGetFileName(string itemFormatName, ZipArchive zipArchive)
  {
    int num = 1;
    string itemName;
    for (itemName = string.Format(itemFormatName, (object) num); zipArchive[itemName] != null; itemName = string.Format(itemFormatName, (object) num))
      ++num;
    return itemName;
  }

  private void SerializeDefaultChartColorStyles(XmlWriter writer, ApplicationImpl applicationImpl)
  {
    writer.WriteStartDocument(true);
    writer.WriteStartElement("cs", "colorStyle", "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "cs", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
    writer.WriteAttributeString("id", "10");
    writer.WriteAttributeString("meth", "cycle");
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent1");
    writer.WriteEndElement();
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent2");
    writer.WriteEndElement();
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent3");
    writer.WriteEndElement();
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent4");
    writer.WriteEndElement();
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent5");
    writer.WriteEndElement();
    writer.WriteStartElement("a", "schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", "accent6");
    writer.WriteEndElement();
    writer.WriteElementString("cs", "variation", "http://schemas.microsoft.com/office/drawing/2012/chartStyle", "");
    if (this.m_defaultColorVariations == null)
      this.InitializeChartColorElements();
    for (int index = 0; index < this.m_defaultColorVariations.Length; ++index)
    {
      writer.WriteStartElement("cs", "variation", "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
      writer.WriteStartElement("a", "lumMod", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", this.m_defaultColorVariations[index][0].ToString());
      writer.WriteEndElement();
      if (this.m_defaultColorVariations[index].Length == 2)
      {
        writer.WriteStartElement("a", "lumOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
        writer.WriteAttributeString("val", this.m_defaultColorVariations[index][1].ToString());
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.Flush();
  }

  private void SerializeDefaultChartStyles(
    XmlWriter writer,
    ChartImpl chart,
    ApplicationImpl applicationImpl)
  {
    writer.WriteStartDocument(true);
    writer.WriteStartElement("cs", "chartStyle", "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
    writer.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("xmlns", "cs", (string) null, "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
    writer.WriteAttributeString("id", "419");
    if (this.m_defaultChartStyleElements == null)
      this.InitializeChartStyleElements();
    for (int key = 0; key < 32 /*0x20*/; ++key)
    {
      if (this.m_defaultChartStyleElements.ContainsKey((ChartStyleElements) key))
      {
        ChartStyleElements chartStyleElements = ChartStyleElements.extLst;
        ShapeStyle chartStyleElement = this.m_defaultChartStyleElements[(ChartStyleElements) key];
        if (chartStyleElement != null)
        {
          if (chart.IsTreeMapOrSunBurst)
          {
            switch ((ChartStyleElements) key)
            {
              case ChartStyleElements.axisTitle:
                chartStyleElement.ShapeProperties = new StyleEntryShapeProperties();
                chartStyleElement.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
                chartStyleElement.ShapeProperties.ShapeFillColorValue = "bg1";
                chartStyleElement.ShapeProperties.ShapeFillLumModValue = 65000.0;
                chartStyleElement.ShapeProperties.BorderWeight = 19050.0;
                chartStyleElement.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
                chartStyleElement.ShapeProperties.BorderFillColorValue = "bg1";
                chartStyleElements = ChartStyleElements.axisTitle;
                break;
              case ChartStyleElements.dataLabel:
                chartStyleElement.FontRefstyleEntry.ColorValue = "lt1";
                chartStyleElement.FontRefstyleEntry.LumOffValue1 = -1.0;
                chartStyleElement.FontRefstyleEntry.LumModValue = -1.0;
                chartStyleElements = ChartStyleElements.dataLabel;
                break;
              case ChartStyleElements.dataPoint:
                chartStyleElement.ShapeProperties.BorderWeight = 19050.0;
                chartStyleElement.ShapeProperties.BorderFillColorValue = "lt1";
                chartStyleElements = ChartStyleElements.dataPoint;
                break;
            }
          }
          if (chart.ChartType == ExcelChartType.Funnel && key == 1)
          {
            chartStyleElement.ShapeProperties.BorderWeight = 0.0;
            chartStyleElements = ChartStyleElements.categoryAxis;
          }
          chartStyleElement.Write(writer, ((ChartStyleElements) key).ToString());
        }
        else if (key == 9)
        {
          writer.WriteStartElement("cs", ChartStyleElements.dataPointMarkerLayout.ToString(), "http://schemas.microsoft.com/office/drawing/2012/chartStyle");
          writer.WriteAttributeString("size", "5");
          writer.WriteAttributeString("symbol", "circle");
          writer.WriteEndElement();
        }
        switch (chartStyleElements)
        {
          case ChartStyleElements.axisTitle:
            chartStyleElement.ShapeProperties = (StyleEntryShapeProperties) null;
            continue;
          case ChartStyleElements.categoryAxis:
            chartStyleElement.ShapeProperties.BorderWeight = 9525.0;
            continue;
          case ChartStyleElements.dataLabel:
            chartStyleElement.FontRefstyleEntry.ColorValue = "tx1";
            chartStyleElement.FontRefstyleEntry.LumOffValue1 = 75000.0;
            chartStyleElement.FontRefstyleEntry.LumModValue = 25000.0;
            continue;
          case ChartStyleElements.dataPoint:
            chartStyleElement.ShapeProperties.BorderWeight = -1.0;
            chartStyleElement.ShapeProperties.BorderFillColorValue = "phClr";
            continue;
          default:
            continue;
        }
      }
    }
    writer.WriteEndElement();
    writer.Flush();
  }

  private void SerializeComments(WorksheetImpl sheet)
  {
    CommentsCollection commentsCollection = sheet != null ? sheet.InnerComments : throw new ArgumentNullException(nameof (sheet));
    if (commentsCollection == null || commentsCollection.Count == 0)
    {
      string relationId;
      this.Relations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments", out relationId);
      if (relationId == null)
        return;
      this.m_relations.Remove(relationId);
    }
    else
    {
      string commentsName = this.GenerateCommentsName();
      MemoryStream newDataStream = new MemoryStream();
      StreamWriter data = new StreamWriter((Stream) newDataStream);
      XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
      this.m_parentHolder.Serializator.SerializeCommentNotes(writer, sheet);
      writer.Flush();
      data.Flush();
      newDataStream.Flush();
      this.m_parentHolder.Archive.UpdateItem(commentsName, (Stream) newDataStream, true, FileAttributes.Archive);
      this.Relations[this.m_strCommentsId] = new Relation('/'.ToString() + commentsName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments");
      this.m_parentHolder.OverriddenContentTypes['/'.ToString() + commentsName] = "application/vnd.openxmlformats-officedocument.spreadsheetml.comments+xml";
    }
  }

  private string GenerateDrawingsName(IWorkbook workbook)
  {
    string itemName;
    if (workbook.Saved)
    {
      itemName = $"xl/drawings/drawing{++this.m_parentHolder.LastDrawingIndex}.xml";
    }
    else
    {
      do
      {
        itemName = $"xl/drawings/drawing{++this.m_parentHolder.LastDrawingIndex}.xml";
      }
      while (this.m_parentHolder.Archive.Find(itemName) != -1);
    }
    return itemName;
  }

  private string GenerateVmlDrawingsName()
  {
    return $"xl/drawings/vmlDrawing{++this.m_parentHolder.LastVmlIndex}.vml";
  }

  private string GenerateCommentsName()
  {
    return $"xl/comments{++this.m_parentHolder.LastCommentIndex}.xml";
  }

  public void SerializeTables(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    IListObjects innerListObjects = (IListObjects) sheet.InnerListObjects;
    int count = innerListObjects != null ? innerListObjects.Count : 0;
    if (count == 0)
      return;
    writer.WriteStartElement("tableParts");
    writer.WriteAttributeString("count", count.ToString());
    for (int index = 0; index < count; ++index)
    {
      string str = this.SerializeTable(innerListObjects[index], index + 1);
      writer.WriteStartElement("tablePart");
      writer.WriteAttributeString("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", str);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private string SerializeTable(IListObject listObject, int index)
  {
    return this.Relations.Add(new Relation('/'.ToString() + this.m_parentHolder.SerializeTable(listObject), "http://schemas.openxmlformats.org/officeDocument/2006/relationships/table"), index);
  }

  internal void ParseTablePart(IWorksheet sheet, string strRelation, string sheetPath)
  {
    Excel2007Parser parser = this.m_parentHolder.Parser;
    string strItemPath1;
    XmlReader reader1 = this.m_parentHolder.CreateReader(this.m_relations[strRelation] ?? throw new XmlException(), sheetPath, out strItemPath1);
    while (reader1.NodeType != XmlNodeType.Element)
      reader1.Read();
    TableParser tableParser = new TableParser();
    IListObject Table = tableParser.Parse(reader1, sheet);
    if (Table.TableType == ExcelTableType.queryTable)
    {
      string parentItemPath = "xl/tables";
      string str = strItemPath1;
      int num = str.LastIndexOf('/');
      str.Substring(0, num);
      string itemName = str.Insert(num, '/'.ToString() + "_rels") + ".rels";
      ZipArchiveItem zipArchiveItem = this.m_parentHolder.Archive[itemName];
      RelationCollection relationCollection = new RelationCollection();
      if (zipArchiveItem != null)
      {
        XmlReader reader2 = UtilityMethods.CreateReader(zipArchiveItem.DataStream);
        relationCollection = parser.ParseRelations(reader2);
        relationCollection.ItemPath = itemName;
      }
      string strItemPath2;
      XmlReader reader3 = this.m_parentHolder.CreateReader(relationCollection["rId1"], parentItemPath, out strItemPath2);
      tableParser.ParseQueryTable(reader3, Table);
      this.m_relations.Remove(strRelation);
      this.m_parentHolder.Archive.RemoveItem(itemName);
      this.m_parentHolder.ItemsToRemove[strItemPath2] = (object) null;
    }
    this.m_parentHolder.ItemsToRemove[strItemPath1] = (object) null;
  }

  internal void ParsePivotTables(
    IWorksheet sheet,
    string strParentPath,
    RelationCollection relations)
  {
    string strItemPath = (string) null;
    PivotTableCollection pivotTables = sheet.PivotTables as PivotTableCollection;
    foreach (KeyValuePair<string, Relation> relation in relations)
    {
      if (relation.Value.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable")
      {
        XmlReader reader = this.m_parentHolder.CreateReader(relation.Value, strParentPath, out strItemPath);
        IPivotCaches pivotCaches = sheet.Workbook.PivotCaches;
        PivotTableImpl pivotTableImpl = new PivotTableImpl(sheet.Application, (object) sheet);
        PivotTableParser.ParsePivotTable(reader, pivotTableImpl);
        pivotTables.Add(pivotTableImpl);
        this.m_parentHolder.ItemsToRemove.Add(strItemPath, (object) null);
        this.m_relations.Remove(relation.Key);
      }
    }
  }

  internal void AssignDrawingrelation(RelationCollection relation)
  {
    this.m_drawingsRelation = relation;
  }

  private void InitializeChartStyleElements()
  {
    this.m_defaultChartStyleElements = new Dictionary<ChartStyleElements, ShapeStyle>(31 /*0x1F*/);
    string attributeValue = "cs";
    string nameSpaceValue = "http://schemas.microsoft.com/office/drawing/2012/chartStyle";
    ShapeStyle shapeStyle1 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      DefaultRunParagraphProperties = new TextSettings()
    };
    shapeStyle1.DefaultRunParagraphProperties.FontSize = new float?(10f);
    shapeStyle1.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle1.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle1.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.axisTitle, shapeStyle1);
    ShapeStyle shapeStyle2 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle2.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle2.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle2.ShapeProperties.BorderLineStyle = Excel2007ShapeLineStyle.sng;
    shapeStyle2.ShapeProperties.IsInsetPenAlignment = false;
    shapeStyle2.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle2.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle2.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle2.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle2.ShapeProperties.BorderIsRound = true;
    shapeStyle2.DefaultRunParagraphProperties = new TextSettings();
    shapeStyle2.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle2.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle2.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle2.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.categoryAxis, shapeStyle2);
    ShapeStyle shapeStyle3 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      DefaultRunParagraphProperties = new TextSettings()
    };
    shapeStyle3.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle3.DefaultRunParagraphProperties.KerningValue = -1f;
    shapeStyle3.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle3.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.trendlineLabel, shapeStyle3);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.valueAxis, shapeStyle3);
    ShapeStyle shapeStyle4 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.allowNoFillOverride | StyleEntryModifierEnum.allowNoLineOverride)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle4.ShapeProperties.ShapeFillType = ExcelFillType.SolidColor;
    shapeStyle4.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle4.ShapeProperties.ShapeFillColorValue = "bg1";
    shapeStyle4.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle4.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle4.ShapeProperties.BorderLineStyle = Excel2007ShapeLineStyle.sng;
    shapeStyle4.ShapeProperties.IsInsetPenAlignment = false;
    shapeStyle4.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle4.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle4.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle4.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle4.ShapeProperties.BorderIsRound = true;
    shapeStyle4.DefaultRunParagraphProperties = new TextSettings();
    shapeStyle4.DefaultRunParagraphProperties.FontSize = new float?(10f);
    shapeStyle4.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle4.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle4.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.chartArea, shapeStyle4);
    ShapeStyle shapeStyle5 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle5.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle5.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle5.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle5.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle5.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle5.DefaultRunParagraphProperties = new TextSettings();
    shapeStyle5.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle5.DefaultRunParagraphProperties.KerningValue = -1f;
    shapeStyle5.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle5.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataTable, shapeStyle5);
    ShapeStyle shapeStyle6 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 75000.0, 25000.0, -1.0, -1.0),
      DefaultRunParagraphProperties = new TextSettings()
    };
    shapeStyle6.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle6.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle6.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle6.DefaultRunParagraphProperties.Baseline = -1;
    shapeStyle6.TextBodyProperties = new TextBodyPropertiesHolder();
    shapeStyle6.TextBodyProperties.WrapTextInShape = true;
    shapeStyle6.TextBodyProperties.SetLeftMargin(38100);
    shapeStyle6.TextBodyProperties.SetTopMargin(19050);
    shapeStyle6.TextBodyProperties.SetRightMargin(38100);
    shapeStyle6.TextBodyProperties.SetBottomMargin(19050);
    shapeStyle6.TextBodyProperties.TextDirection = TextDirection.Horizontal;
    shapeStyle6.TextBodyProperties.VerticalAlignment = ExcelVerticalAlignment.Middle;
    shapeStyle6.TextBodyProperties.IsAutoSize = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataLabel, shapeStyle6);
    ShapeStyle shapeStyle7 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle7.ShapeProperties.ShapeFillType = ExcelFillType.SolidColor;
    shapeStyle7.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle7.ShapeProperties.ShapeFillColorValue = "lt1";
    shapeStyle7.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle7.ShapeProperties.BorderFillColorValue = "dk1";
    shapeStyle7.ShapeProperties.BorderFillLumModValue = 25000.0;
    shapeStyle7.ShapeProperties.BorderFillLumOffValue1 = 75000.0;
    shapeStyle7.DefaultRunParagraphProperties = new TextSettings();
    shapeStyle7.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle7.DefaultRunParagraphProperties.KerningValue = -1f;
    shapeStyle7.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle7.DefaultRunParagraphProperties.Baseline = -1;
    shapeStyle7.TextBodyProperties = new TextBodyPropertiesHolder();
    shapeStyle7.TextBodyProperties.WrapTextInShape = true;
    shapeStyle7.TextBodyProperties.SetLeftMargin(36576);
    shapeStyle7.TextBodyProperties.SetTopMargin(18288);
    shapeStyle7.TextBodyProperties.SetRightMargin(36576);
    shapeStyle7.TextBodyProperties.SetBottomMargin(18288);
    shapeStyle7.TextBodyProperties.TextDirection = TextDirection.Horizontal;
    shapeStyle7.TextBodyProperties.VerticalAlignment = ExcelVerticalAlignment.MiddleCentered;
    shapeStyle7.TextBodyProperties.TextVertOverflowType = TextVertOverflowType.Clip;
    shapeStyle7.TextBodyProperties.TextHorzOverflowType = TextHorzOverflowType.Clip;
    shapeStyle7.TextBodyProperties.IsAutoSize = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataLabelCallout, shapeStyle7);
    ShapeStyle shapeStyle8 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.styleClr, "auto", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.styleClr, "auto", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle8.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle8.ShapeProperties.ShapeFillColorValue = "phClr";
    shapeStyle8.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle8.ShapeProperties.BorderFillColorValue = "phClr";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPoint, shapeStyle8);
    ShapeStyle shapeStyle9 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.styleClr, "auto", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle9.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle9.ShapeProperties.ShapeFillColorValue = "phClr";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPoint3D, shapeStyle9);
    ShapeStyle shapeStyle10 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.styleClr, "auto", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle10.ShapeProperties.BorderWeight = 28575.0;
    shapeStyle10.ShapeProperties.LineCap = EndLineCap.rnd;
    shapeStyle10.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle10.ShapeProperties.BorderFillColorValue = "phClr";
    shapeStyle10.ShapeProperties.BorderIsRound = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPointLine, shapeStyle10);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPointWireframe, shapeStyle10);
    ShapeStyle shapeStyle11 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.styleClr, "auto", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle11.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle11.ShapeProperties.ShapeFillColorValue = "phClr";
    shapeStyle11.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle11.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle11.ShapeProperties.BorderFillColorValue = "lt1";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPointMarker, shapeStyle11);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dataPointMarkerLayout, (ShapeStyle) null);
    ShapeStyle shapeStyle12 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none);
    shapeStyle12.LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle12.FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle12.EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle12.FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.wall, shapeStyle12);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.dropLine, shapeStyle12);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.errorBar, shapeStyle12);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.floor, shapeStyle12);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.hiLoLine, shapeStyle12);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.leaderLine, shapeStyle12);
    ShapeStyle shapeStyle13 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "dk1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle13.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle13.ShapeProperties.ShapeFillColorValue = "dk1";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.downBar, shapeStyle13);
    ShapeStyle shapeStyle14 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "dk1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle14.ShapeProperties.ShapeFillColorModelType = ColorModel.schemeClr;
    shapeStyle14.ShapeProperties.ShapeFillColorValue = "lt1";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.upBar, shapeStyle14);
    ShapeStyle shapeStyle15 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      DefaultRunParagraphProperties = new TextSettings()
    };
    shapeStyle15.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle15.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle15.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle15.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.legend, shapeStyle15);
    ShapeStyle shapeStyle16 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.allowNoFillOverride | StyleEntryModifierEnum.allowNoLineOverride);
    shapeStyle16.LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle16.FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle16.EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0);
    shapeStyle16.FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.plotArea, shapeStyle16);
    this.m_defaultChartStyleElements.Add(ChartStyleElements.plotArea3D, shapeStyle16);
    ShapeStyle shapeStyle17 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle17.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle17.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle17.ShapeProperties.BorderLineStyle = Excel2007ShapeLineStyle.sng;
    shapeStyle17.ShapeProperties.IsInsetPenAlignment = false;
    shapeStyle17.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle17.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle17.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle17.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle17.ShapeProperties.BorderIsRound = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.gridlineMajor, shapeStyle17);
    ShapeStyle shapeStyle18 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle18.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle18.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle18.ShapeProperties.BorderLineStyle = Excel2007ShapeLineStyle.sng;
    shapeStyle18.ShapeProperties.IsInsetPenAlignment = false;
    shapeStyle18.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle18.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle18.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle18.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle18.ShapeProperties.BorderFillLumOffValue2 = 10000.0;
    shapeStyle18.ShapeProperties.BorderIsRound = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.gridlineMinor, shapeStyle18);
    ShapeStyle shapeStyle19 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      DefaultRunParagraphProperties = new TextSettings()
    };
    shapeStyle19.DefaultRunParagraphProperties.FontSize = new float?(14f);
    shapeStyle19.DefaultRunParagraphProperties.Bold = new bool?(false);
    shapeStyle19.DefaultRunParagraphProperties.KerningValue = 12f;
    shapeStyle19.DefaultRunParagraphProperties.SpacingValue = 0;
    shapeStyle19.DefaultRunParagraphProperties.Baseline = 0;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.title, shapeStyle19);
    ShapeStyle shapeStyle20 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", -1.0, -1.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle20.ShapeProperties.BorderWeight = 19050.0;
    shapeStyle20.ShapeProperties.LineCap = EndLineCap.rnd;
    shapeStyle20.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle20.ShapeProperties.BorderFillColorValue = "phClr";
    this.m_defaultChartStyleElements.Add(ChartStyleElements.trendline, shapeStyle20);
    ShapeStyle shapeStyle21 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle21.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle21.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle21.ShapeProperties.BorderLineStyle = Excel2007ShapeLineStyle.sng;
    shapeStyle21.ShapeProperties.IsInsetPenAlignment = false;
    shapeStyle21.ShapeProperties.BorderFillColorModelType = ColorModel.schemeClr;
    shapeStyle21.ShapeProperties.BorderFillColorValue = "tx1";
    shapeStyle21.ShapeProperties.BorderFillLumModValue = 15000.0;
    shapeStyle21.ShapeProperties.BorderFillLumOffValue1 = 85000.0;
    shapeStyle21.ShapeProperties.BorderIsRound = true;
    shapeStyle21.DefaultRunParagraphProperties = new TextSettings();
    shapeStyle21.DefaultRunParagraphProperties.FontSize = new float?(9f);
    shapeStyle21.DefaultRunParagraphProperties.KerningValue = -1f;
    shapeStyle21.DefaultRunParagraphProperties.SpacingValue = -1;
    shapeStyle21.DefaultRunParagraphProperties.Baseline = -1;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.seriesAxis, shapeStyle21);
    ShapeStyle shapeStyle22 = new ShapeStyle(attributeValue, nameSpaceValue, StyleEntryModifierEnum.none)
    {
      LineRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FillRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      EffectRefStyleEntry = new StyleOrFontReference(0, ColorModel.none, "", -1.0, -1.0, -1.0, -1.0),
      FontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "tx1", 65000.0, 35000.0, -1.0, -1.0),
      ShapeProperties = new StyleEntryShapeProperties()
    };
    shapeStyle22.ShapeProperties.BorderWeight = 9525.0;
    shapeStyle22.ShapeProperties.LineCap = EndLineCap.flat;
    shapeStyle22.ShapeProperties.BorderFillColorModelType = ColorModel.srgbClr;
    shapeStyle22.ShapeProperties.BorderFillColorValue = "D9D9D9";
    shapeStyle22.ShapeProperties.BorderIsRound = true;
    this.m_defaultChartStyleElements.Add(ChartStyleElements.seriesLine, shapeStyle22);
  }

  private void InitializeChartColorElements()
  {
    this.m_defaultColorVariations = new double[8][];
    this.m_defaultColorVariations[0] = new double[1]
    {
      60000.0
    };
    this.m_defaultColorVariations[1] = new double[2]
    {
      80000.0,
      20000.0
    };
    this.m_defaultColorVariations[2] = new double[1]
    {
      80000.0
    };
    this.m_defaultColorVariations[3] = new double[2]
    {
      60000.0,
      40000.0
    };
    this.m_defaultColorVariations[4] = new double[1]
    {
      50000.0
    };
    this.m_defaultColorVariations[5] = new double[2]
    {
      70000.0,
      30000.0
    };
    this.m_defaultColorVariations[6] = new double[1]
    {
      70000.0
    };
    this.m_defaultColorVariations[7] = new double[2]
    {
      50000.0,
      50000.0
    };
  }

  public WorksheetDataHolder Clone(FileDataHolder dataHolder)
  {
    WorksheetDataHolder worksheetDataHolder = (WorksheetDataHolder) this.MemberwiseClone();
    worksheetDataHolder.m_parentHolder = dataHolder;
    if (this.m_archiveItem != null)
      worksheetDataHolder.m_archiveItem = dataHolder.Archive[this.m_archiveItem.ItemName];
    worksheetDataHolder.m_relations = (RelationCollection) CloneUtils.CloneCloneable((ICloneable) this.m_relations);
    worksheetDataHolder.m_drawingsRelation = (RelationCollection) CloneUtils.CloneCloneable((ICloneable) this.m_drawingsRelation);
    worksheetDataHolder.m_hfDrawingsRelation = (RelationCollection) CloneUtils.CloneCloneable((ICloneable) this.m_hfDrawingsRelation);
    if (this.m_cfStream != null)
    {
      byte[] buffer = new byte[this.m_cfStream.Length];
      this.m_cfStream.Position = 0L;
      this.m_cfStream.Read(buffer, 0, buffer.Length);
      this.m_cfStream.Position = 0L;
      worksheetDataHolder.m_cfStream = new MemoryStream(buffer);
    }
    if (this.m_cfsStream != null)
    {
      byte[] buffer = new byte[this.m_cfsStream.Length];
      this.m_cfsStream.Position = 0L;
      this.m_cfsStream.Read(buffer, 0, buffer.Length);
      this.m_cfsStream.Position = 0L;
      worksheetDataHolder.m_cfsStream = (Stream) new MemoryStream(buffer);
    }
    worksheetDataHolder.m_startStream = (MemoryStream) CloneUtils.CloneStream((Stream) this.m_startStream);
    worksheetDataHolder.m_streamControls = CloneUtils.CloneStream(this.m_streamControls);
    return worksheetDataHolder;
  }

  public void Dispose()
  {
    this.m_archiveItem = (ZipArchiveItem) null;
    this.m_cfStream = (MemoryStream) null;
    this.m_cfsStream = (Stream) null;
    if (this.m_startStream != null)
    {
      this.m_startStream.Dispose();
      this.m_startStream = (MemoryStream) null;
    }
    if (this.m_streamControls != null)
    {
      this.m_streamControls.Dispose();
      this.m_streamControls = (Stream) null;
    }
    this.m_parentHolder.Dispose();
    if (this.m_drawingsRelation != null)
    {
      this.m_drawingsRelation.Dispose();
      this.m_drawingsRelation = (RelationCollection) null;
    }
    if (this.m_hfDrawingsRelation != null)
    {
      this.m_hfDrawingsRelation.Dispose();
      this.m_hfDrawingsRelation = (RelationCollection) null;
    }
    if (this.m_relations != null)
    {
      this.m_relations.Dispose();
      this.m_relations = (RelationCollection) null;
    }
    if (this.m_preservedPivotTable != null)
    {
      this.m_preservedPivotTable.Clear();
      this.m_preservedPivotTable = (Dictionary<string, RelationCollection>) null;
    }
    if (this.m_defaultChartStyleElements != null)
    {
      this.m_defaultChartStyleElements.Clear();
      this.m_defaultChartStyleElements = (Dictionary<ChartStyleElements, ShapeStyle>) null;
    }
    this.m_defaultColorVariations = (double[][]) null;
    GC.SuppressFinalize((object) this);
  }
}
