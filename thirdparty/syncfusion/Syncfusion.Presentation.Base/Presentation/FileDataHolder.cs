// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.FileDataHolder
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.CompoundFile.Presentation;
using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.CommentImplementation;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Resource;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.Security;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation;

internal class FileDataHolder : IDisposable
{
  public const string XmlNamespaceMain = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
  internal const string DEF_XMLNS_PREF = "xmlns";
  public const string WorksheetTagName = "worksheet";
  public const string RelationPrefix = "r";
  public const string RelationNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
  public const string X14Prefix = "x14";
  public const string X14Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main";
  public const string MCPrefix = "mc";
  public const string MCNamespace = "http://schemas.openxmlformats.org/markup-compatibility/2006";
  public const string DimensionTagName = "dimension";
  public const string RefAttributeName = "ref";
  public const string SheetDataTagName = "sheetData";
  public const string CellTagName = "c";
  public const string RowTagName = "row";
  public const string ReferenceAttributeName = "r";
  public const string CellValueTagName = "v";
  public const string RowIndexAttributeName = "r";
  private const string SpansTag = "spans";
  public const string CellDataTypeAttributeName = "t";
  public const string StyleIndexAttributeName = "s";
  public const string SharedStringTableTagName = "sst";
  public const string UniqueStringCountAttributeName = "uniqueCount";
  public const string TextTagName = "t";
  public const string StringItemTagName = "si";
  private RelationCollection _topRelations;
  private ZipArchive _archive;
  private List<string> _dictItemsToRemove;
  private List<string> _items;
  private Syncfusion.Presentation.Presentation _presentation;
  private bool _isDisposed;
  private Dictionary<string, string> _mediaItems;
  private Stream excelFile = (Stream) new MemoryStream();
  private Dictionary<ChartStyleElements, ShapeStyle> m_defaultChartStyleElements;
  private double[][] m_defaultColorVariations;

  internal FileDataHolder(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._archive = new ZipArchive();
    this._dictItemsToRemove = new List<string>();
    this.TopRelations = new RelationCollection();
    this._mediaItems = new Dictionary<string, string>();
  }

  internal RelationCollection TopRelations
  {
    get => this._topRelations;
    set => this._topRelations = value;
  }

  internal ZipArchive Archive => this._archive;

  internal bool IsDuplicateImage(Relation relation, Stream stream)
  {
    foreach (ZipArchiveItem zipArchiveItem in this._archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains("media/image") && this.CompareStreams(stream, zipArchiveItem.DataStream))
      {
        relation.Target = "../" + zipArchiveItem.ItemName.Remove(0, 4);
        return true;
      }
    }
    return false;
  }

  internal void AddImage(string imagePath, Stream stream, Relation relation)
  {
    if (this.IsDuplicateImage(relation, stream))
      return;
    this.AddImageToArchive(imagePath, stream as MemoryStream);
  }

  private bool CompareStreams(Stream a, Stream b)
  {
    if (a.Length < b.Length || a.Length > b.Length)
      return false;
    a.Position = 0L;
    b.Position = 0L;
    for (int index = 0; (long) index < a.Length; ++index)
    {
      if (a.ReadByte().CompareTo(b.ReadByte()) != 0)
        return false;
    }
    return true;
  }

  internal bool IsImageRefereceExist(BaseSlide baseSlide, string _picturePath)
  {
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) baseSlide.Presentation.Masters)
    {
      if (master.IsImageReferenceExists(_picturePath))
        return true;
      foreach (BaseSlide layoutSlide in (IEnumerable<ILayoutSlide>) master.LayoutSlides)
      {
        if (layoutSlide.IsImageReferenceExists(_picturePath))
          return true;
      }
      foreach (BaseSlide slide in (IEnumerable<ISlide>) baseSlide.Presentation.Slides)
      {
        if (slide.IsImageReferenceExists(_picturePath))
          return true;
      }
    }
    return false;
  }

  internal static LoadFormat CheckHeader(Stream stream, out bool isValid, out int header)
  {
    isValid = false;
    LoadFormat loadFormat = LoadFormat.Pptx;
    if (stream.Length < 8L)
    {
      header = 0;
      isValid = true;
      return LoadFormat.Pptx;
    }
    long num = new BinaryReader(stream).ReadInt64();
    header = (int) (num & (long) ushort.MaxValue);
    stream.Seek(-8L, SeekOrigin.Current);
    if ((num & (long) uint.MaxValue) != 67324752L)
      return loadFormat;
    isValid = true;
    return LoadFormat.Pptx;
  }

  internal Stream GetItemFromDefaultContentZip(string fileName)
  {
    using (Stream streamFromResource = ResourceManager.GetStreamFromResource("defaultContent"))
    {
      using (ZipArchive zipArchive = new ZipArchive())
      {
        zipArchive.Open(streamFromResource, true);
        ZipArchiveItem zipArchiveItem = zipArchive[fileName];
        MemoryStream destination = new MemoryStream();
        Stream dataStream = zipArchiveItem.DataStream;
        dataStream.Position = 0L;
        dataStream.CopyTo((Stream) destination);
        return (Stream) destination;
      }
    }
  }

  internal void AddDefaultContent()
  {
    this.InitializeItem();
    using (Stream streamFromResource = ResourceManager.GetStreamFromResource("defaultContent"))
    {
      using (ZipArchive zipArchive = new ZipArchive())
      {
        zipArchive.Open(streamFromResource, true);
        foreach (string itemName in this._items)
          this.AddItemToZipStream(zipArchive[itemName].Clone());
      }
    }
  }

  internal void AddImageToArchive(string imagePath, MemoryStream stream)
  {
    this.AddItemToZipStream(imagePath, (Stream) stream);
  }

  internal Stream GetImageStream(string imagePath) => this._archive[imagePath].DataStream;

  internal MemoryStream GetStreamFromTemplateZip()
  {
    using (Stream streamFromResource = ResourceManager.GetStreamFromResource("template"))
    {
      using (ZipArchive zipArchive = new ZipArchive())
      {
        zipArchive.Open(streamFromResource, true);
        MemoryStream destination = new MemoryStream();
        Stream dataStream = zipArchive.Items[0].DataStream;
        dataStream.Position = 0L;
        dataStream.CopyTo((Stream) destination);
        dataStream.Close();
        return destination;
      }
    }
  }

  internal void InitializeItem()
  {
    if (this._items == null)
      this._items = new List<string>();
    this._items.Add("ppt/slideMasters/_rels/slideMaster1.xml.rels");
    this._items.Add("ppt/slideMasters/slideMaster1.xml");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout1.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout2.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout3.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout4.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout5.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout6.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout7.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout8.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout9.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout10.xml.rels");
    this._items.Add("ppt/slideLayouts/_rels/slideLayout11.xml.rels");
    this._items.Add("ppt/slideLayouts/slideLayout1.xml");
    this._items.Add("ppt/slideLayouts/slideLayout2.xml");
    this._items.Add("ppt/slideLayouts/slideLayout3.xml");
    this._items.Add("ppt/slideLayouts/slideLayout4.xml");
    this._items.Add("ppt/slideLayouts/slideLayout5.xml");
    this._items.Add("ppt/slideLayouts/slideLayout6.xml");
    this._items.Add("ppt/slideLayouts/slideLayout7.xml");
    this._items.Add("ppt/slideLayouts/slideLayout8.xml");
    this._items.Add("ppt/slideLayouts/slideLayout9.xml");
    this._items.Add("ppt/slideLayouts/slideLayout10.xml");
    this._items.Add("ppt/slideLayouts/slideLayout11.xml");
    this._items.Add("ppt/presentation.xml");
    this._items.Add("ppt/theme/theme1.xml");
    this._items.Add("ppt/presProps.xml");
    this._items.Add("ppt/tableStyles.xml");
    this._items.Add("ppt/viewProps.xml");
    this._items.Add("docProps/core.xml");
    this._items.Add("docProps/app.xml");
    this._items.Add("ppt/_rels/presentation.xml.rels");
    if (this._presentation.CustomDocumentProperties.Count != 0 && !this._presentation.CustomDocumentProperties.Contains("docProps/custom.xml"))
      this._items.Add("docProps/custom.xml");
    if (this._presentation.HandoutList == null || this._presentation.HandoutList.Count <= 0)
      return;
    this.AddToItemsList("ppt/theme/theme2.xml");
    this.AddToItemsList("docProps/thumbnail.jpeg");
  }

  internal void AddToItemsList(string value)
  {
    if (this._items == null)
      this._items = new List<string>();
    this._items.Add(value);
  }

  internal void Open(Stream stream)
  {
    if (this._presentation.CheckForEncryption(stream))
    {
      Stream stream1 = this.DecryptedPresentationStream(stream);
      this._presentation.IsEncrypted = true;
      this._archive.Open(stream1, false);
    }
    else
      this._archive.Open(stream, false);
  }

  private Stream DecryptedPresentationStream(Stream stream)
  {
    if (stream == null)
      throw new ArgumentException(nameof (stream));
    Stream stream1 = (Stream) new MemoryStream();
    bool flag = false;
    using (ICompoundFile compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.Presentation.Net.CompoundFile(stream))
    {
      ICompoundStorage rootStorage = compoundFile.RootStorage;
      SecurityHelper.EncrytionType encryptionType = new SecurityHelper().GetEncryptionType(rootStorage);
      if (encryptionType != SecurityHelper.EncrytionType.None)
      {
        if (this._presentation.Password == null)
          throw new ArgumentException("Presentation is encrypted, password is needed to open the presentation");
        switch (encryptionType)
        {
          case SecurityHelper.EncrytionType.Standard:
            flag = true;
            StandardDecryptor standardDecryptor = new StandardDecryptor();
            standardDecryptor.Initialize(rootStorage);
            if (!standardDecryptor.CheckPassword(this._presentation.Password))
              throw new Exception($"Specified password \"{this._presentation.Password}\" is incorrect!");
            stream1 = standardDecryptor.Decrypt();
            break;
          case SecurityHelper.EncrytionType.Agile:
            flag = true;
            AgileDecryptor agileDecryptor = new AgileDecryptor();
            agileDecryptor.Initialize(rootStorage);
            if (!agileDecryptor.CheckPassword(this._presentation.Password))
              throw new Exception($"Specified password \"{this._presentation.Password}\" is incorrect!");
            stream1 = agileDecryptor.Decrypt();
            break;
        }
      }
    }
    if (!flag)
      throw new ApplicationException("Wrong Presentation version");
    return stream1;
  }

  internal void RemoveImageStream(string imagePath) => this._archive.RemoveItem(imagePath);

  internal void ParseDocument() => this.Read();

  private void CheckThumbNail()
  {
    ZipArchiveItem zipArchiveItem = this._archive["docProps/thumbnail.jpeg"];
    if (zipArchiveItem == null || zipArchiveItem.DataStream.Length <= 0L)
      return;
    this._presentation.HasThumbnail = true;
  }

  private void ParseContentType()
  {
    ZipArchiveItem zipArchiveItem = this._archive["[Content_Types].xml"];
    if (zipArchiveItem == null)
      throw new NotSupportedException("File cannot be opened - format is not supported");
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseContentTypes(reader, this._presentation);
    this._dictItemsToRemove.Add("[Content_Types].xml");
  }

  private void ParseLayoutRelation(LayoutSlide layoutSlide, string layoutPath)
  {
    string[] strArray = layoutPath.Split('/');
    string itemName = $"ppt/slideLayouts/_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      layoutSlide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseLayoutSlide(LayoutSlide layoutSlide, string layoutPath)
  {
    string[] strArray = layoutPath.Split('/');
    string itemName = "ppt/slideLayouts/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseLayoutSlide(reader, layoutSlide);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseMasterRelation(MasterSlide masterSlide, string masterPath)
  {
    string[] strArray = masterPath.Split('/');
    masterPath = !masterPath.StartsWith("/") ? "ppt/" + masterPath : masterPath.Remove(0, 1);
    string itemName = $"{masterPath.Substring(0, masterPath.IndexOf(strArray[strArray.Length - 1]))}_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      masterSlide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void UpdateMasterRelation(MasterSlide masterSlide)
  {
    RelationCollection topRelation = masterSlide.TopRelation;
    int num = 0;
    RelationCollection relationCollection = new RelationCollection();
    List<Relation> relationList = new List<Relation>();
    for (int index = 0; index < masterSlide.Presentation.Slides.Count; ++index)
    {
      foreach (Relation relation in (masterSlide.Presentation.Slides[index] as Slide).TopRelation.GetRelationList())
        relationList.Add(relation.Clone());
    }
    masterSlide.TopRelation.Clone();
    foreach (Relation relation1 in topRelation.GetRelationList())
    {
      Relation relation2 = new Relation();
      relation2.Type = relation1.Type;
      switch (relation1.Type)
      {
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideLayout":
          string path = $"slideLayouts/slideLayout{(++this._presentation.SlideLayoutCount).ToString()}.xml";
          if (!relation1.Id.StartsWith("rId"))
          {
            string idByTarget = masterSlide.TopRelation.GetIdByTarget(relation1.Target);
            relation2.Id = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(masterSlide.TopRelation);
            if (masterSlide.LayoutList.ContainsKey(relation1.Id) && !masterSlide.LayoutList.ContainsKey(relation2.Id) && relation2.Id == idByTarget)
            {
              string layout = masterSlide.LayoutList[relation1.Id];
              masterSlide.LayoutList.Remove(relation1.Id);
              masterSlide.LayoutList.Add(relation2.Id, layout);
            }
          }
          else
            relation2.Id = relation1.Id;
          this._presentation.RemoveOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation1.Target));
          relation2.Target = "../" + path;
          this._presentation.AddOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(path), "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml");
          for (int index = 0; index < relationList.Count; ++index)
          {
            if (relationList[index].Target == relation1.Target)
            {
              relationList[index].Target = relation2.Target.ToString();
              break;
            }
          }
          ++num;
          break;
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme":
          if (!relation1.Id.StartsWith("rId"))
            relation2.Id = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(masterSlide.TopRelation);
          string target = relation1.Target;
          relation2.Id = relation1.Id;
          this._presentation.RemoveOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(target));
          relation2.Target = relation1.Id.StartsWith("rId") ? $"../theme/theme{(++this._presentation.ThemeCount).ToString()}.xml" : $"/ppt/theme/theme{(++this._presentation.ThemeCount).ToString()}.xml";
          this._presentation.AddOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation2.Target), "application/vnd.openxmlformats-officedocument.theme+xml");
          break;
        default:
          relation2.Target = relation1.Target;
          relation2.Id = relation1.Id;
          break;
      }
      relationCollection.Add(relation2.Id, relation2);
    }
  }

  private void ParseMasterSlide(MasterSlide masterSlide, string masterPath)
  {
    masterPath.Split('/');
    masterPath = !masterPath.StartsWith("/") ? "ppt/" + masterPath : masterPath.Remove(0, 1);
    ZipArchiveItem zipArchiveItem = this._archive[masterPath];
    this.ParseTheme(masterSlide.TopRelation, masterSlide.Theme);
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseMasterSlide(reader, masterSlide);
    this._dictItemsToRemove.Add(masterPath);
  }

  private void ParseLayoutSlides(MasterSlide masterSlide)
  {
    foreach (KeyValuePair<string, string> layout in masterSlide.LayoutList)
    {
      LayoutSlide layoutSlide = new LayoutSlide(this._presentation, masterSlide, layout.Value);
      foreach (Relation relation in masterSlide.TopRelation.GetRelationList())
      {
        if (relation.Target.Contains("slideLayouts") && relation.Id == layout.Key)
        {
          this.ParseLayoutRelation(layoutSlide, relation.Target);
          this.CheckImage(layoutSlide.TopRelation);
          this.ParseLayoutSlide(layoutSlide, relation.Target);
          this.ParseVmlDrawing((BaseSlide) layoutSlide);
          ((LayoutSlides) masterSlide.LayoutSlides).Add(layoutSlide);
          break;
        }
      }
    }
  }

  internal void ParseMasterSlides()
  {
    foreach (KeyValuePair<string, string> master in this._presentation.MasterList)
    {
      string itemPathByRelation = this._presentation.TopRelation.GetItemPathByRelation(master.Key);
      MasterSlide masterSlide = new MasterSlide(this._presentation, master.Value);
      ((MasterSlides) this._presentation.Masters).Add(masterSlide);
      this.ParseMasterRelation(masterSlide, itemPathByRelation);
      this.CheckImage(masterSlide.TopRelation);
      this.ParseMasterSlide(masterSlide, itemPathByRelation);
      this.ParseVmlDrawing((BaseSlide) masterSlide);
      this.ParseLayoutSlides(masterSlide);
    }
  }

  private void CheckImage(RelationCollection relationCollection)
  {
    foreach (Relation relation in relationCollection.GetRelationList())
    {
      if (relation.Type.Contains("image"))
      {
        string extenxtion = this.GetExtenxtion(relation.Target);
        this._presentation.AddDefaultContentType(extenxtion, "image/" + extenxtion);
      }
      else if (relation.Type.Contains("photo"))
        this._presentation.AddDefaultContentType(this.GetExtenxtion(relation.Target), "image/vnd.ms-photo");
    }
  }

  private string GetExtenxtion(string target)
  {
    string extenxtion1;
    if (target.Contains(extenxtion1 = "jpeg"))
      return extenxtion1;
    string extenxtion2;
    if (target.Contains(extenxtion2 = "bmp"))
      return extenxtion2;
    string extenxtion3;
    if (target.Contains(extenxtion3 = "png"))
      return extenxtion3;
    string extenxtion4;
    if (target.Contains(extenxtion4 = "emg"))
      return extenxtion4;
    string extenxtion5;
    if (target.Contains(extenxtion5 = "exif"))
      return extenxtion5;
    string extenxtion6;
    if (target.Contains(extenxtion6 = "gif"))
      return extenxtion6;
    string extenxtion7;
    if (target.Contains(extenxtion7 = "ico"))
      return extenxtion7;
    string extenxtion8;
    if (target.Contains(extenxtion8 = "wdp"))
      return extenxtion8;
    string extenxtion9;
    if (target.Contains(extenxtion9 = "bmp"))
      return extenxtion9;
    string extenxtion10;
    if (target.Contains(extenxtion10 = "tiff"))
      return extenxtion10;
    string extenxtion11;
    if (target.Contains(extenxtion11 = "wmf"))
      return extenxtion11;
    string str;
    return target.Contains(str = "jpg") ? str : (string) null;
  }

  internal void ParsePresentation()
  {
    string itemName = "ppt/presentation.xml";
    if (this._presentation.PresentationName != null)
      itemName = this._presentation.PresentationName;
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParsePresentation(reader, this._presentation);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void ParsePresentationRelation()
  {
    string itemName = "ppt/_rels/presentation.xml.rels";
    if (this._presentation.PresentationName != null)
    {
      int num = this._presentation.PresentationName.LastIndexOf('/');
      string str = this._presentation.PresentationName;
      if (num != -1)
        str = str.Substring(num + 1);
      itemName = $"ppt/_rels/{str}.rels";
    }
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      this._presentation.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void UpdatePresentationRelation(RelationCollection topRelation)
  {
    int num1 = 0;
    this._presentation.SlideList.Clear();
    this._presentation.MasterList.Clear();
    foreach (Relation relation1 in topRelation.GetRelationList())
    {
      switch (relation1.Type)
      {
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster":
          string path = $"slideMasters/slideMaster{(++num1).ToString()}.xml";
          int index1 = -1;
          for (int index2 = 0; index2 < this._presentation.Masters.Count; ++index2)
          {
            foreach (BaseSlide layoutSlide in (IEnumerable<ILayoutSlide>) (this._presentation.Masters[index2] as MasterSlide).LayoutSlides)
            {
              Relation relationByContentType = layoutSlide.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster");
              if (relationByContentType.Target == relation1.Target)
              {
                index1 = index2;
                relationByContentType.Target = !relationByContentType.Id.StartsWith("rId") ? "/ppt/" + path : "../" + path;
              }
            }
          }
          this._presentation.RemoveOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation1.Target));
          relation1.Target = !relation1.Id.StartsWith("rId") ? "/ppt/" + path : path;
          if (index1 != -1)
            this._presentation.MasterList.Add(relation1.Id, ((MasterSlide) this._presentation.Masters[index1]).MasterId);
          else
            this._presentation.MasterList.Add(relation1.Id, ((MasterSlide) this._presentation.Masters[num1 - 1]).MasterId);
          this._presentation.AddOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(path), "application/vnd.openxmlformats-officedocument.presentationml.slideMaster+xml");
          continue;
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide":
          this._presentation.RemoveOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation1.Target));
          int num2 = ++this._presentation.SlideCount;
          relation1.Target = !relation1.Id.StartsWith("rId") ? $"/ppt/slides/slide{num2.ToString()}.xml" : $"slides/slide{num2.ToString()}.xml";
          NotesSlide notesSlide = (NotesSlide) (this._presentation.Slides[num2 - 1] as Slide).NotesSlide;
          if (notesSlide != null)
          {
            foreach (Relation relation2 in notesSlide.TopRelation.GetRelationList())
            {
              switch (relation2.Type)
              {
                case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide":
                  relation2.Target = "../" + relation1.Target;
                  continue;
                default:
                  continue;
              }
            }
          }
          this._presentation.SlideList.Add(relation1.Id, this._presentation.Slides[num2 - 1].SlideID.ToString());
          this._presentation.AddOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation1.Target), "application/vnd.openxmlformats-officedocument.presentationml.slide+xml");
          continue;
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme":
          relation1.Target = !relation1.Id.StartsWith("rId") ? "/ppt/theme/theme1.xml" : "theme/theme1.xml";
          continue;
        default:
          continue;
      }
    }
  }

  internal void ParseVmlDrawingRelation(BaseSlide slide, string vmlRelationPath)
  {
    string[] strArray = vmlRelationPath.Split('/');
    ZipArchiveItem zipArchiveItem = this._archive[$"ppt/drawings/_rels/{strArray[strArray.Length - 1]}.rels"];
    if (zipArchiveItem == null)
      return;
    RelationCollection relationCollection;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      relationCollection = Parser.ParseRelationCollection(reader);
    this.SetVMLShapeImageData(slide.Shapes as Shapes, relationCollection);
  }

  internal void SetVMLShapeImageData(Shapes shapes, RelationCollection vmlDrawingRelations)
  {
    if (vmlDrawingRelations == null || vmlDrawingRelations.Count <= 0)
      return;
    foreach (Shape shape in shapes)
    {
      if (shape.SlideItemType == SlideItemType.OleObject)
      {
        if ((shape as OleObject).VmlShape != null)
        {
          VmlShape vmlShape = (shape as OleObject).VmlShape;
          if (vmlShape.ImageRelationId != null)
          {
            string[] strArray = vmlDrawingRelations.GetItemPathByRelation(vmlShape.ImageRelationId).Split('/');
            if (strArray.Length == 3)
            {
              Stream imageStream = this.GetImageStream($"ppt/{strArray[strArray.Length - 2]}{(object) '/'}{strArray[strArray.Length - 1]}");
              MemoryStream output = new MemoryStream();
              Picture.CopyStream(imageStream, (Stream) output);
              vmlShape.ImageData = output.ToArray();
            }
          }
        }
      }
      else if (shape.SlideItemType == SlideItemType.GroupShape)
        this.SetVMLShapeImageData((shape as GroupShape).Shapes as Shapes, vmlDrawingRelations);
    }
  }

  internal void ParseVmlDrawing(BaseSlide slide)
  {
    if (!slide.HasOLEObject)
      return;
    string str = "ppt" + slide.TopRelation.GetItemByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing").Remove(0, 2);
    ZipArchiveItem zipArchiveItem = this._archive[str];
    if (zipArchiveItem == null)
      return;
    if (UtilityMethods.IsValidXMLDocument(zipArchiveItem.DataStream))
    {
      zipArchiveItem.DataStream.Position = 0L;
      using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream, true))
        Parser.ParseVmlDrawing(reader, slide);
      this.ParseVmlDrawingRelation(slide, str);
      this._dictItemsToRemove.Add(str);
    }
    else
      slide.HasOLEObject = false;
  }

  private void ParseRootRelation()
  {
    ZipArchiveItem zipArchiveItem = this._archive["_rels/.rels"];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      this.TopRelations = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add("_rels/.rels");
  }

  private void ParseSlide(Slide slide, string slidePath)
  {
    string[] strArray = slidePath.Split('/');
    string itemName = "ppt/slides/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseSlide(reader, slide);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void ParseChart(PresentationChart chart)
  {
    if (chart == null)
      return;
    RelationCollection topRelation = chart.BaseSlide.TopRelation;
    string itemPathByRelation = topRelation.GetItemPathByRelation(chart.RelationId);
    topRelation.RemoveRelation(chart.RelationId);
    this.ParseChartRelation(chart, itemPathByRelation);
    this.ParseChartWorkbook(chart);
    this.ParseChartData(chart, itemPathByRelation);
  }

  private void ParseChartData(PresentationChart chart, string chartPath)
  {
    int num = chart.IsChartEx ? 1 : 0;
    string[] strArray1 = chartPath.Split('/');
    string itemName = !chartPath.Contains("slides") ? "ppt/charts/" + strArray1[strArray1.Length - 1] : "ppt/slides/charts/" + strArray1[strArray1.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    chart.Workbook.IsWorkbookOpening = true;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
    {
      Syncfusion.OfficeChart.Implementation.XmlSerialization.RelationCollection relations = new Syncfusion.OfficeChart.Implementation.XmlSerialization.RelationCollection();
      foreach (Relation relation1 in chart.TopRelation.GetRelationList())
      {
        Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation relation2 = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation(relation1.Target, relation1.Type);
        relations.Add(relation2);
      }
      Dictionary<string, string> themeColors = chart.BaseSlide.BaseTheme.ThemeColors;
      chart.Workbook.DataHolder.Parser.m_dicThemeColors = new Dictionary<string, Color>();
      foreach (KeyValuePair<string, string> keyValuePair in themeColors)
      {
        Color color = Color.FromArgb(int.Parse(keyValuePair.Value, NumberStyles.HexNumber));
        chart.Workbook.DataHolder.Parser.m_dicThemeColors.Add(keyValuePair.Key, color);
      }
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      Dictionary<string, string> dictionary2 = (Dictionary<string, string>) null;
      Dictionary<string, string> dictionary3 = (Dictionary<string, string>) null;
      if (chart.BaseSlide is Slide)
      {
        MasterSlide baseSlide = chart.BaseSlide.BaseTheme.BaseSlide as MasterSlide;
        dictionary2 = ((chart.BaseSlide as Slide).LayoutSlide as LayoutSlide).ColorMap;
        dictionary3 = baseSlide.ColorMap;
      }
      else if (chart.BaseSlide is LayoutSlide)
      {
        MasterSlide baseSlide = chart.BaseSlide.BaseTheme.BaseSlide as MasterSlide;
        dictionary2 = (chart.BaseSlide as LayoutSlide).ColorMap;
        dictionary3 = baseSlide.ColorMap;
      }
      else if (chart.BaseSlide is MasterSlide)
        dictionary3 = (chart.BaseSlide.BaseTheme.BaseSlide as MasterSlide).ColorMap;
      else if (chart.BaseSlide is NotesSlide)
      {
        dictionary2 = chart.BaseSlide.ColorMap;
        dictionary3 = chart.BaseSlide.Presentation.NotesMaster.ColorMap;
      }
      Dictionary<string, string> dictionary4 = dictionary2 == null || dictionary2.Count == 0 ? dictionary3 : dictionary2;
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = (string) null;
      string str4 = (string) null;
      foreach (KeyValuePair<string, string> keyValuePair in dictionary4)
      {
        if (keyValuePair.Key == "tx1")
          str1 = keyValuePair.Value;
        if (keyValuePair.Key == "tx2")
          str2 = keyValuePair.Value;
        if (keyValuePair.Key == "bg1")
          str3 = keyValuePair.Value;
        if (keyValuePair.Key == "bg2")
          str4 = keyValuePair.Value;
      }
      string s1 = (string) null;
      string s2 = (string) null;
      string s3 = (string) null;
      string s4 = (string) null;
      foreach (KeyValuePair<string, string> keyValuePair in themeColors)
      {
        if (keyValuePair.Key == str1)
          s1 = keyValuePair.Value;
        if (keyValuePair.Key == str2)
          s2 = keyValuePair.Value;
        if (keyValuePair.Key == str3)
          s3 = keyValuePair.Value;
        if (keyValuePair.Key == str4)
          s4 = keyValuePair.Value;
      }
      chart.Workbook.DataHolder.Parser.m_dicThemeColors.Add("tx1", Color.FromArgb(int.Parse(s1, NumberStyles.HexNumber)));
      chart.Workbook.DataHolder.Parser.m_dicThemeColors.Add("tx2", Color.FromArgb(int.Parse(s2, NumberStyles.HexNumber)));
      chart.Workbook.DataHolder.Parser.m_dicThemeColors.Add("bg1", Color.FromArgb(int.Parse(s3, NumberStyles.HexNumber)));
      chart.Workbook.DataHolder.Parser.m_dicThemeColors.Add("bg2", Color.FromArgb(int.Parse(s4, NumberStyles.HexNumber)));
      chart.GetChartImpl().m_themeColors = new List<Color>();
      foreach (KeyValuePair<string, string> keyValuePair in themeColors)
        chart.GetChartImpl().m_themeColors.Add(Color.FromArgb(int.Parse(keyValuePair.Value, NumberStyles.HexNumber)));
      Dictionary<string, FontImpl> dictionary5 = new Dictionary<string, FontImpl>();
      Dictionary<string, FontImpl> dictionary6 = new Dictionary<string, FontImpl>();
      string[] strArray2 = new string[3]
      {
        "latin",
        "ea",
        "cs"
      };
      for (int index = 0; index < strArray2.Length; ++index)
      {
        switch (strArray2[index])
        {
          case "latin":
            FontImpl font1 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            FontImpl font2 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            font1.Size = 11.0;
            font2.Size = 11.0;
            font1.FontName = chart.BaseSlide.BaseTheme.MajorFont.Latin != null ? chart.BaseSlide.BaseTheme.MajorFont.Latin : "";
            dictionary5.Add(strArray2[index], font1);
            font2.FontName = chart.BaseSlide.BaseTheme.MinorFont.Latin != null ? chart.BaseSlide.BaseTheme.MinorFont.Latin : "";
            dictionary6.Add(strArray2[index], font2);
            break;
          case "ea":
            FontImpl font3 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            FontImpl font4 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            font3.Size = 11.0;
            font4.Size = 11.0;
            font3.FontName = chart.BaseSlide.BaseTheme.MajorFont.Ea != null ? chart.BaseSlide.BaseTheme.MajorFont.Ea : "";
            dictionary5.Add(strArray2[index], font3);
            font4.FontName = chart.BaseSlide.BaseTheme.MinorFont.Ea != null ? chart.BaseSlide.BaseTheme.MinorFont.Ea : "";
            dictionary6.Add(strArray2[index], font4);
            break;
          case "cs":
            FontImpl font5 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            FontImpl font6 = chart.Workbook.CreateFont((IOfficeFont) null, false) as FontImpl;
            font5.Size = 11.0;
            font6.Size = 11.0;
            font5.FontName = chart.BaseSlide.BaseTheme.MajorFont.Cs != null ? chart.BaseSlide.BaseTheme.MajorFont.Cs : "";
            dictionary5.Add(strArray2[index], font5);
            font6.FontName = chart.BaseSlide.BaseTheme.MinorFont.Cs != null ? chart.BaseSlide.BaseTheme.MinorFont.Cs : "";
            dictionary6.Add(strArray2[index], font6);
            break;
        }
      }
      chart.Workbook.MajorFonts = dictionary5;
      chart.Workbook.MinorFonts = dictionary6;
      chart.Workbook.Version = OfficeVersion.Excel2013;
      ChartImpl chartImpl = chart.GetChartImpl();
      chartImpl.Height = chart.Height;
      chartImpl.Width = chart.Width;
      if (chart.IsChartEx)
        new ChartExParser(chart.Workbook).ParseChartEx(reader, chartImpl, relations);
      else
        new ChartParser(chart.Workbook).ParseChart(reader, chartImpl, relations);
      string[] array = new string[chartImpl.RelationPreservedStreamCollection.Keys.Count];
      chartImpl.RelationPreservedStreamCollection.Keys.CopyTo(array, 0);
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index].StartsWith("rId") && chartImpl.RelationPreservedStreamCollection.ContainsKey(array[index]))
          chartImpl.RelationPreservedStreamCollection.Remove(array[index]);
      }
    }
    chart.Workbook.IsWorkbookOpening = false;
    this._dictItemsToRemove.Add(itemName);
    chart.IsParsedChart = true;
  }

  private void SetDataInSheet(ChartImpl chart)
  {
    WorksheetImpl worksheet = chart.Workbook.Worksheets[0] as WorksheetImpl;
    if (chart.CategoryFormula != null)
      this.SetCategortyRange(chart.CategoryFormula, worksheet, chart);
    if (chart.Series == null)
      return;
    for (int index = 1; index <= chart.Series.Count; ++index)
    {
      ChartSerieImpl chartSerieImpl = chart.Series[index - 1] as ChartSerieImpl;
      if (chartSerieImpl.SerieName != null)
        worksheet[chartSerieImpl.NameOrFormula.Split('!')[1]].Text = chartSerieImpl.SerieName;
      int num = 0;
      foreach (IRange range in (IEnumerable) worksheet[chartSerieImpl.ValuesIRange.AddressLocal])
      {
        if (chartSerieImpl.EnteredDirectlyValues != null)
        {
          if (num < chartSerieImpl.EnteredDirectlyValues.Length)
            worksheet[range.AddressLocal].Value2 = chartSerieImpl.EnteredDirectlyValues[num++];
          else
            break;
        }
      }
    }
  }

  private void SetCategortyRange(string categoryRange, WorksheetImpl chartSheet, ChartImpl chart)
  {
    List<string> stringList = new List<string>();
    if (categoryRange.Contains(","))
    {
      string[] strArray = categoryRange.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
        stringList.Add(categoryRange.Split(',')[index]);
    }
    else
      stringList.Add(categoryRange);
    for (int index = 0; index < stringList.Count; ++index)
    {
      if (!stringList[index].Contains("!"))
      {
        stringList.RemoveAt(index);
        --index;
      }
    }
    string[] array = stringList.ToArray();
    if (array == null || array.Length <= 0)
      return;
    int index1 = 0;
    int num = 0;
    for (; index1 < array.Length; ++index1)
    {
      foreach (IRange range in (IEnumerable) chartSheet[array[index1].Split('!')[1]])
      {
        if (chart.CategoryLabelValues == null)
        {
          if ((chart.Series[0] as ChartSerieImpl).EnteredDirectlyCategoryLabels != null)
          {
            if (num < (chart.Series[0] as ChartSerieImpl).EnteredDirectlyCategoryLabels.Length)
              chartSheet[range.AddressLocal].Value2 = (chart.Series[0] as ChartSerieImpl).EnteredDirectlyCategoryLabels[num++];
            else
              break;
          }
          else
            break;
        }
        else if (num < chart.CategoryLabelValues.Length)
          chartSheet[range.AddressLocal].Value2 = chart.CategoryLabelValues[num++];
        else
          break;
      }
    }
  }

  private string GetEmbedWorkBook(PresentationChart chart)
  {
    string str = (string) null;
    ChartImpl chartImpl = (ChartImpl) null;
    foreach (Relation relation in chart.TopRelation.GetRelationList())
    {
      if (!string.Equals(relation.Type, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject") && string.Equals(Path.GetExtension(relation.Target), ".xlsx"))
        str = relation.Target;
      else if (relation.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image")
      {
        if (chartImpl == null)
          chartImpl = chart.GetChartImpl();
        string itemName = relation.Target.Replace("..", "ppt");
        ZipArchiveItem zipArchiveItem = this._archive[itemName];
        if (zipArchiveItem == null && relation.Type != "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes")
          zipArchiveItem = this._archive["ppt/charts/" + itemName];
        if (zipArchiveItem != null)
          chartImpl.RelationPreservedStreamCollection.Add(relation.Id, zipArchiveItem.DataStream);
      }
      else if (relation.Type == "http://schemas.microsoft.com/office/2011/relationships/chartStyle" || relation.Type == "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle" || relation.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes")
      {
        if (chartImpl == null)
          chartImpl = chart.GetChartImpl();
        if (!chartImpl.RelationPreservedStreamCollection.ContainsKey(relation.Type))
        {
          string itemName = relation.Target.Replace("..", "ppt");
          ZipArchiveItem zipArchiveItem = this._archive[itemName];
          if (zipArchiveItem == null && relation.Type != "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes")
            zipArchiveItem = this._archive["ppt/charts/" + itemName];
          if (zipArchiveItem != null)
          {
            if (relation.Type == "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle")
              chartImpl.m_isChartColorStyleSkipped = true;
            else if (relation.Type == "http://schemas.microsoft.com/office/2011/relationships/chartStyle")
              chartImpl.m_isChartStyleSkipped = true;
            Stream output = (Stream) new MemoryStream();
            Picture.CopyStream(zipArchiveItem.DataStream, output);
            output.Position = 0L;
            chartImpl.RelationPreservedStreamCollection.Add(relation.Type, output);
            this._archive.RemoveItem(itemName);
            this._presentation.RemoveOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(relation.Target));
            chart.TopRelation.RemoveRelation(relation.Id);
          }
        }
      }
    }
    if (str == null)
      return (string) null;
    chart.TopRelation.Clear();
    return str.Replace("..", "ppt");
  }

  internal void ParseChartWorkbook(PresentationChart chart)
  {
    string embedWorkBook = this.GetEmbedWorkBook(chart);
    if (embedWorkBook == null)
    {
      (chart.OfficeChart as ChartImpl).HasExternalWorkbook = true;
    }
    else
    {
      ZipArchiveItem zipArchiveItem = this._archive[embedWorkBook];
      Stream stream = (Stream) new MemoryStream();
      Picture.CopyStream(zipArchiveItem.DataStream, stream);
      this.excelFile = stream;
      try
      {
        this.ParseExcelStream(stream, chart.Workbook);
        chart.WorkSheetIndex = chart.Workbook.ActiveSheetIndex;
        chart.EmbeddedWorkbookStream = this.excelFile;
      }
      catch
      {
      }
      this._archive.RemoveItem(embedWorkBook);
    }
  }

  internal void ParseExcelStream(Stream excelStream, WorkbookImpl workbookImpl)
  {
    workbookImpl.DataHolder.ParseDocument(excelStream);
  }

  private void ParseChartRelation(PresentationChart chart, string chartPath)
  {
    string[] strArray = chartPath.Split('/');
    string itemName = !chartPath.Contains("slides") ? $"ppt/charts/_rels/{strArray[strArray.Length - 1]}.rels" : $"ppt/slides/charts/_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    zipArchiveItem.DataStream.Position = 0L;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      chart.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void SerializeSST(XmlWriter writer, WorksheetImpl sheet)
  {
    SSTDictionary innerSst = sheet.ParentWorkbook.InnerSST;
    writer.WriteStartElement("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    int count = innerSst.Count;
    int labelSstCount = innerSst.GetLabelSSTCount();
    writer.WriteAttributeString("uniqueCount", count.ToString());
    writer.WriteAttributeString("count", labelSstCount.ToString());
    for (int index = 0; index < count; ++index)
    {
      object sstContentByIndex = innerSst.GetSSTContentByIndex(index);
      this.SerializeStringItem(writer, sstContentByIndex);
    }
  }

  private void SerializeStringItem(XmlWriter writer, object objTextOrString)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (objTextOrString == null)
      throw new ArgumentNullException("text");
    writer.WriteStartElement("si");
    string text = objTextOrString.ToString();
    writer.WriteStartElement("t");
    writer.WriteString(text);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeWorksheet(XmlWriter writer, WorksheetImpl sheet)
  {
    if (sheet.UsedRange.Row <= 0)
      return;
    writer.WriteStartElement("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteAttributeString("xmlns", "mc", (string) null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
    this.SerializeDimensions(writer, sheet);
    this.SerializeSheetData(writer, sheet, "c");
    writer.WriteEndElement();
  }

  private void SerializeDimensions(XmlWriter writer, WorksheetImpl sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet.FirstRow <= 0 || sheet.FirstColumn <= 0 || sheet.LastColumn > sheet.Workbook.MaxColumnCount)
      return;
    writer.WriteStartElement("dimension");
    writer.WriteAttributeString("ref", sheet.UsedRange.AddressLocal);
    writer.WriteEndElement();
  }

  public void SerializeSheetData(XmlWriter writer, WorksheetImpl sheet, string cellTag)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("sheetData");
    if (sheet.UsedRange.Row > 0)
    {
      int firstRow = sheet.FirstRow;
      for (int lastRow = sheet.LastRow; firstRow <= lastRow; ++firstRow)
        this.SerializeRow(writer, sheet, cellTag, firstRow);
    }
    writer.WriteEndElement();
  }

  private void SerializeRow(XmlWriter writer, WorksheetImpl sheet, string cellTag, int row)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement(nameof (row));
    writer.WriteAttributeString("r", row.ToString());
    string str = $"{sheet.FirstColumn.ToString()}:{sheet.LastColumn.ToString()}";
    writer.WriteAttributeString("spans", str);
    this.SerializeCells(writer, sheet, cellTag, row);
    writer.WriteEndElement();
  }

  private void SerializeCells(XmlWriter writer, WorksheetImpl sheet, string cellTag, int row)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    for (int firstColumn = sheet.FirstColumn; firstColumn <= sheet.LastColumn; ++firstColumn)
      this.SerializeCell(writer, sheet, cellTag, row, firstColumn);
  }

  private void SerializeCell(
    XmlWriter writer,
    WorksheetImpl sheet,
    string cellTag,
    int row,
    int column)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement(cellTag);
    string addressLocal = sheet.Range[row, column].AddressLocal;
    writer.WriteAttributeString("r", addressLocal);
    object obj = sheet[row, column].Value2;
    switch (obj.GetType().Name)
    {
      case "String":
        int num = sheet.ParentWorkbook.InnerSST.AddIncrease((object) obj.ToString());
        writer.WriteAttributeString("t", "s");
        writer.WriteElementString("v", num.ToString());
        break;
      case "DateTime":
        writer.WriteElementString("v", sheet[row, column].Number.ToString());
        break;
      default:
        writer.WriteElementString("v", sheet[row, column].Value2.ToString());
        break;
    }
    writer.WriteEndElement();
  }

  private void ParseSlideRelation(Slide slide, string slidePath)
  {
    string[] strArray = slidePath.Split('/');
    string itemName = $"ppt/slides/_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      slide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseNotesSlideRelation(NotesSlide notesSlide, string notesSlidePath)
  {
    string[] strArray = notesSlidePath.Remove(0, 3).Split('/');
    string str = $"ppt/slides/_rels/{strArray[strArray.Length - 1]}.rels";
    string itemName = $"ppt/{strArray[0]}/_rels/{strArray[1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      notesSlide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void ParseThemeFromDefaultContent(RelationCollection relationCollection, Theme theme)
  {
    Stream streamFromResource = ResourceManager.GetStreamFromResource("defaultContent");
    ZipArchive zipArchive = new ZipArchive();
    zipArchive.Open(streamFromResource, false);
    ZipArchiveItem zipArchiveItem = zipArchive["ppt/theme/theme1.xml"];
    XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream);
    this.ParseThemeRelation(theme, zipArchiveItem.ItemName);
    Parser.ParseTheme(reader, theme);
  }

  private void ParseSlides()
  {
    if (this._presentation.SlideList == null || this._presentation.SlideList.Count <= 0)
      return;
    foreach (KeyValuePair<string, string> slide1 in this._presentation.SlideList)
    {
      string itemPathByRelation = this._presentation.TopRelation.GetItemPathByRelation(slide1.Key);
      string slideId = slide1.Value;
      Slide slide2 = new Slide(this._presentation, slideId);
      string[] strArray = itemPathByRelation.Split('/');
      this._presentation.SlidesFromInputFile.Add(strArray[strArray.Length - 1], (ISlide) slide2);
      this.ParseSlideRelation(slide2, itemPathByRelation);
      this.GetSlideLayout(slide2);
      this.ParseComments(slide2);
      this.ParseSlide(slide2, itemPathByRelation);
      this.ParseVmlDrawing((BaseSlide) slide2);
      this.ParseNotesSlide(slide2);
      foreach (Section section in ((Sections) this._presentation.Sections).GetSectionList())
      {
        if (section.SlideIdList.Contains(slideId))
        {
          ((Slides) section.Slides).AddSlide(slide2);
          slide2.SectionId = section.ID;
          break;
        }
      }
      ((Slides) this._presentation.Slides).AddSlide(slide2);
    }
  }

  private void ParseComments(Slide slide)
  {
    if (!slide.HasComment)
      return;
    string itemPathByRelation = slide.TopRelation.GetItemPathByRelation(slide.CommentRelationId);
    this.ParseSlideCommentList(slide, itemPathByRelation);
    ++slide.Presentation.CommentCount;
  }

  private void ParseSlideCommentList(Slide slide, string commentPath)
  {
    string[] strArray = commentPath.Split('/');
    string itemName = "ppt/comments/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseCommentList(reader, slide);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseNotesSlide(Slide slide)
  {
    if (!slide.HasNotes)
      return;
    string itemPathByRelation = slide.TopRelation.GetItemPathByRelation(slide.NotesRelationId);
    NotesSlide notesSlide = new NotesSlide(slide, ++slide.Presentation.NotesSlideCount);
    this.ParseNotesSlideRelation(notesSlide, itemPathByRelation);
    this.ParseNotes(notesSlide, itemPathByRelation);
    slide.SetNotesSlide(notesSlide);
  }

  private void ParseNotes(NotesSlide notesSlide, string notesSlidePath)
  {
    string[] strArray = notesSlidePath.Split('/');
    string itemName = "ppt/notesSlides/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseNotesSlide(reader, notesSlide);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void GetSlideLayout(Slide slide)
  {
    slide.LayoutIndex = slide.ObtainLayoutIndex();
    if (slide.LayoutIndex == null || this._presentation.GetSlideLayout().ContainsKey(slide.LayoutIndex))
      return;
    MasterSlide master = slide.Presentation.GetMaster(slide.LayoutIndex);
    for (int index = 0; index < master.LayoutList.Count; ++index)
    {
      LayoutSlide layoutSlide = (LayoutSlide) master.LayoutSlides[index];
      if (slide.LayoutIndex == layoutSlide.LayoutId)
      {
        this._presentation.GetSlideLayout().Add(slide.LayoutIndex, layoutSlide);
        break;
      }
    }
  }

  private LayoutSlide ParseSlideLayout(Slide slide)
  {
    string layoutIndex = slide.LayoutIndex;
    MasterSlide master = slide.Presentation.GetMaster(layoutIndex);
    LayoutSlide layoutSlide = new LayoutSlide(this._presentation, master, layoutIndex);
    this.ParseLayoutRelation(layoutSlide, slide.LayoutTarget);
    this.CheckImage(layoutSlide.TopRelation);
    this.ParseLayoutSlide(layoutSlide, slide.LayoutTarget);
    ((LayoutSlides) master.LayoutSlides).Add(layoutSlide);
    return layoutSlide;
  }

  internal void ParseTheme(RelationCollection topRelation, Theme theme)
  {
    string itemByContentType = topRelation.GetItemByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme");
    string str = this._presentation.Created ? "ppt/theme/theme1.xml" : Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(itemByContentType);
    ZipArchiveItem zipArchiveItem = this._archive[str];
    if (zipArchiveItem != null && zipArchiveItem.OriginalSize > 0L)
    {
      using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      {
        this.ParseThemeRelation(theme, str);
        Parser.ParseTheme(reader, theme);
      }
    }
    this._dictItemsToRemove.Add(str);
  }

  private void ParseThemeRelation(Theme theme, string themePath)
  {
    string[] strArray = themePath.Split('/');
    string itemName = $"ppt/{strArray[1]}/_rels/{strArray[2]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      theme.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void Read()
  {
    this.ParseContentType();
    this.ParseRootRelation();
    this.ParsePresentationRelation();
    this.ParsePresentation();
    this.ParseVBAProject();
    this.ParseMaster();
    this.ParseNotesMaster();
    this.ParseHandoutMaster();
    this.ParseSlides();
    this.CheckThumbNail();
    this.ParseDocumentProperties();
    this.ParseViewProperties();
    this.ParseCommentAuthors();
    this.RemoveItemFromArchieve();
    this._presentation.IsStyleChanged = false;
    this.SetPresentationImageCount();
    this.UpdateRelation();
  }

  internal void ParseTableStyle(Table table)
  {
    string itemName = "ppt/tableStyles.xml";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
    {
      if (!reader.IsEmptyElement)
      {
        string localName = reader.LocalName;
        reader.Read();
        TableStyle tableStyleElements;
        while (true)
        {
          if (localName == reader.LocalName)
            goto label_14;
label_4:
          if (reader.NodeType == XmlNodeType.Element)
          {
            if (reader.LocalName == "tblStyle")
            {
              tableStyleElements = Parser.ParseTableStyleElements(reader, this._presentation, table.Id);
              if (tableStyleElements == null)
                reader.MoveToElement();
              else if (table.Id == tableStyleElements.Id)
                break;
              reader.Skip();
              continue;
            }
            reader.Skip();
            continue;
          }
          reader.Skip();
          continue;
label_14:
          if (reader.NodeType != XmlNodeType.EndElement)
            goto label_4;
          goto label_19;
        }
        tableStyleElements.IsCustom = true;
        this._presentation.TableStyleList.Add(table.Id, tableStyleElements);
        return;
      }
    }
label_19:
    this._dictItemsToRemove.Add(itemName);
  }

  private void UpdateRelation()
  {
    this.UpdatePresentationRelation(this._presentation.TopRelation);
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
      this.UpdateMasterRelation(master);
  }

  internal void ParseViewProperties()
  {
    string itemName = "ppt/viewProps.xml";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseViewProperties(reader, this._presentation);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void ParseCommentAuthors()
  {
    string itemName = "ppt/commentAuthors.xml";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseCommentAuthors(reader, this._presentation);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseVBAProject()
  {
    string itemName = "ppt/vbaProject.bin";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    this._presentation.VBAProject = (Stream) new MemoryStream();
    Picture.CopyStream(zipArchiveItem.DataStream, this._presentation.VBAProject);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseNotesMaster() => this.ParseNotesMasterSlides();

  private void ParseNotesMasterSlides()
  {
    if (this._presentation.NotesList == null)
      return;
    foreach (KeyValuePair<string, string> notes in this._presentation.NotesList)
    {
      string itemPathByRelation = this._presentation.TopRelation.GetItemPathByRelation(notes.Key);
      this._presentation.NotesMaster = new NotesMasterSlide(this._presentation, notes.Value);
      this.ParseNotesMasterRelation(this._presentation.NotesMaster, itemPathByRelation);
      this.CheckImage(this._presentation.NotesMaster.TopRelation);
      this.ParseNotesMasterSlide(this._presentation.NotesMaster, itemPathByRelation);
    }
  }

  private void ParseNotesMasterSlide(NotesMasterSlide notesMasterSlide, string notesMasterPath)
  {
    string[] strArray = notesMasterPath.Split('/');
    string itemName = "ppt/notesMasters/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    this.ParseTheme(notesMasterSlide.TopRelation, notesMasterSlide.Theme);
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseNotesMasterSlide(reader, notesMasterSlide);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseNotesMasterRelation(NotesMasterSlide notesMasterSlide, string notesMasterPath)
  {
    string[] strArray = notesMasterPath.Split('/');
    string itemName = $"ppt/notesMasters/_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      notesMasterSlide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  private void SetPresentationImageCount()
  {
    List<string> stringList = new List<string>(11);
    stringList.Add("jpeg");
    stringList.Add("jpg");
    stringList.Add("png");
    stringList.Add("tiff");
    stringList.Add("emf");
    stringList.Add("bmp");
    stringList.Add("bin");
    stringList.Add("gif");
    stringList.Add("icon");
    stringList.Add("wmf");
    stringList.Add("exif");
    stringList.Add("jfif");
    foreach (ZipArchiveItem zipArchiveItem in this._archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains("media"))
      {
        string[] strArray = zipArchiveItem.ItemName.Split('/');
        string str = Syncfusion.Presentation.Drawing.Helper.GetExtension(strArray[strArray.Length - 1]).TrimStart('.');
        if (!string.IsNullOrEmpty(str) && stringList.Contains(str.ToLower()))
          ++this._presentation.ImageCount;
      }
    }
    stringList.Clear();
  }

  private void ParseDocumentProperties()
  {
    this.ParseDocumentPropertiesByContentType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
    this.ParseDocumentPropertiesByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
    this.ParseDocumentPropertiesByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
  }

  private void ParseDocumentPropertiesByContentType(string type)
  {
    string itemByContentType = this.TopRelations.GetItemByContentType(type);
    if (itemByContentType == null)
      return;
    this._presentation.HasDocumentProperties = true;
    ZipArchiveItem zipArchiveItem = this._archive[itemByContentType];
    if (zipArchiveItem == null || zipArchiveItem.OriginalSize <= 0L)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
    {
      switch (type)
      {
        case "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties":
          Parser.ParseDocumentCoreProperties(reader, this._presentation.BuiltInDocumentProperties);
          break;
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties":
          Parser.ParseExtendedProperties(reader, this._presentation.BuiltInDocumentProperties, this._presentation.CustomDocumentProperties);
          break;
        case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties":
          Parser.ParseCustomProperties(reader, this._presentation.CustomDocumentProperties);
          ICustomDocumentProperties documentProperties = this._presentation.CustomDocumentProperties;
          if (documentProperties.Count != 0)
          {
            if (documentProperties.Contains("_MarkAsFinal"))
            {
              if (documentProperties["_MarkAsFinal"].Boolean)
              {
                this._presentation.Final = true;
                break;
              }
              break;
            }
            break;
          }
          break;
      }
    }
    this._dictItemsToRemove.Add(itemByContentType);
  }

  private void ParseMaster() => this.ParseMasterSlides();

  private void ParseHandoutMaster()
  {
    if (this._presentation.HandoutList == null || this._presentation.HandoutList.Count <= 0)
      return;
    foreach (KeyValuePair<string, string> handout in this._presentation.HandoutList)
    {
      string itemPathByRelation = this._presentation.TopRelation.GetItemPathByRelation(handout.Key);
      HandoutMaster handoutMaster = new HandoutMaster(this._presentation);
      this.ParseHandoutRelation(handoutMaster, itemPathByRelation);
      this.ParseHandoutTheme(handoutMaster);
      this.ParseHandoutMaster(handoutMaster, itemPathByRelation);
      this._presentation.SetHandoutMaster(handoutMaster);
    }
  }

  private void ParseHandoutTheme(HandoutMaster handoutMaster)
  {
    string itemName = handoutMaster.TopRelation.GetItemByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme").TrimStart('/');
    if (itemName.StartsWith("../"))
      itemName = "ppt/" + itemName.Remove(0, 3);
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem != null && zipArchiveItem.OriginalSize > 0L)
    {
      using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
        Parser.ParseTheme(reader, handoutMaster.ThemeCollection);
    }
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseHandoutMaster(HandoutMaster handoutMaster, string handoutPath)
  {
    string[] strArray = handoutPath.Split('/');
    string itemName = "ppt/handoutMasters/" + strArray[strArray.Length - 1];
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseHandoutMaster(reader, handoutMaster);
    this._dictItemsToRemove.Add(itemName);
  }

  private void ParseHandoutRelation(HandoutMaster slide, string handoutPath)
  {
    string[] strArray = handoutPath.Split('/');
    string itemName = $"ppt/handoutMasters/_rels/{strArray[strArray.Length - 1]}.rels";
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      slide.TopRelation = Parser.ParseRelationCollection(reader);
    this._dictItemsToRemove.Add(itemName);
  }

  internal void RemoveItemFromArchieve()
  {
    foreach (string itemName in this._dictItemsToRemove)
      this._archive.RemoveItem(itemName);
  }

  internal void WriteDocument(Stream stream)
  {
    this.Write();
    string password = this._presentation.Password;
    if (string.IsNullOrEmpty(password) || !this._presentation.IsEncrypted)
    {
      this._archive.Save(stream, false);
    }
    else
    {
      MemoryStream data = new MemoryStream();
      this._archive.Save((Stream) data, false);
      using (ICompoundFile compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.Presentation.Net.CompoundFile())
      {
        AgileEncryptor agileEncryptor = new AgileEncryptor("SHA512", 256 /*0x0100*/, 64 /*0x40*/);
        data.Position = 0L;
        agileEncryptor.Encrypt((Stream) data, password, compoundFile.RootStorage);
        compoundFile.Save(stream);
      }
      data.Close();
    }
  }

  private void AddItemToZipStream(ZipArchiveItem itemToAdd) => this._archive.AddItem(itemToAdd);

  internal void AddItemToZipStream(string itemPath, Stream stream)
  {
    ZipArchiveItem zipArchiveItem = this._archive[itemPath];
    if (zipArchiveItem != null)
      zipArchiveItem.Update(stream, true);
    else
      this._archive.AddItem(itemPath, stream, true, FileAttributes.Archive);
  }

  private void Write()
  {
    this.WriteRootRelation();
    this.WritePresentationRelation();
    this.WritePresentation();
    this.WriteVBAProject();
    this.WriteMasterSlides();
    this.WriteNotesMasterSlides();
    this.WriteHandoutMaster();
    this.WriteSlides();
    this.WriteContentType();
    this.WriteTableStyles();
    this.WriteDocumentProperties();
    this.WriteViewProperties();
    this.WriteCommentAuthors();
  }

  private void WriteVBAProject()
  {
    if (this._presentation.VBAProject == null)
      return;
    this.AddItemToZipStream("ppt/vbaProject.bin", this._presentation.VBAProject);
  }

  private void WriteViewProperties()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter writer = XmlWriter.Create((Stream) output))
      Serializator.SerializeViewProperties(writer, this._presentation);
    this.AddItemToZipStream("ppt/viewProps.xml", (Stream) output);
  }

  private void WriteCommentAuthors()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter writer = XmlWriter.Create((Stream) output))
      Serializator.SerializeCommentAuthors(writer, this._presentation);
    this.AddItemToZipStream("ppt/commentAuthors.xml", (Stream) output);
  }

  private void WriteNotesMasterSlides()
  {
    NotesMasterSlide notesMaster = this._presentation.NotesMaster;
    if (notesMaster == null)
      return;
    this.WriteTheme(notesMaster.TopRelation.GetItemPathByKeyword("theme"), notesMaster.Theme);
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.SerializeNotesMasterSlide(xmlWriter, notesMaster);
    string itemPathByKeyword = this._presentation.TopRelation.GetItemPathByKeyword("notesMaster");
    this.AddItemToZipStream(Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(itemPathByKeyword), (Stream) output);
    this.WriteTopRelation(itemPathByKeyword, notesMaster.TopRelation);
  }

  private void WriteNotesMasterRelation(RelationCollection relationCollection)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream("ppt/notesMasters/_rels/notesMaster1.xml.rels", (Stream) output);
  }

  private void WriteVmlDrawingRelation(
    RelationCollection relationCollection,
    string vmlRelationPath)
  {
    string[] strArray = vmlRelationPath.Split('/');
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream($"ppt/drawings/_rels/{strArray[strArray.Length - 1]}.rels", (Stream) output);
  }

  internal void WriteVmlDrawing(BaseSlide slide)
  {
    if (!slide.HasOLEObject)
      return;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.SerializeVmlDrawing(xmlWriter, slide);
    string str = "ppt" + slide.TopRelation.GetItemByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing").Remove(0, 2);
    this.AddItemToZipStream(str, (Stream) output);
    RelationCollection vmlRelation = slide.VMLRelation;
    if (vmlRelation != null)
      this.WriteVmlDrawingRelation(vmlRelation, str);
    this._presentation.AddDefaultContentType("vml", "application/vnd.openxmlformats-officedocument.vmlDrawing");
  }

  private void WriteDocumentProperties()
  {
    if (!this._presentation.HasDocumentProperties)
      return;
    MemoryStream stream1 = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream1))
      Serializator.SerializeCoreProperties(writer, this._presentation.BuiltInDocumentProperties);
    this.AddItemToZipStream("docProps/core.xml", (Stream) stream1);
    MemoryStream stream2 = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream2))
      Serializator.SerializeExtendedProperties(writer, this._presentation.BuiltInDocumentProperties, this._presentation.CustomDocumentProperties);
    this.AddItemToZipStream("docProps/app.xml", (Stream) stream2);
    MemoryStream stream3 = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream3))
      Serializator.SerializeCustomProperties(writer, this._presentation.CustomDocumentProperties);
    this.AddItemToZipStream("docProps/custom.xml", (Stream) stream3);
  }

  private void WriteMasterSlides()
  {
    int num = 0;
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
    {
      this.WriteTheme(master.TopRelation.GetItemPathByKeyword("theme"), master.Theme);
      MemoryStream output = new MemoryStream();
      using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
        Serializator.SerializeMasterSlide(xmlWriter, master);
      this.WriteVmlDrawing((BaseSlide) master);
      string slidePath = Syncfusion.Presentation.Drawing.Helper.GetSlidePath(this._presentation.MasterList, this._presentation.TopRelation, master.MasterId);
      this.AddItemToZipStream(Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(slidePath), (Stream) output);
      this.WriteTopRelation(slidePath, master.TopRelation);
      ++num;
      this.WriteLayoutSlides(master);
    }
  }

  private void WriteLayoutSlides(MasterSlide masterSlide)
  {
    int num = 0;
    foreach (LayoutSlide layoutSlide in (IEnumerable<ILayoutSlide>) masterSlide.LayoutSlides)
    {
      MemoryStream output = new MemoryStream();
      using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
        Serializator.SerializeLayoutSlide(xmlWriter, (BaseSlide) layoutSlide);
      this.WriteVmlDrawing((BaseSlide) layoutSlide);
      string layoutIndex = this.GetLayoutIndex(layoutSlide);
      if (layoutIndex != null)
      {
        string str = Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(masterSlide.TopRelation.GetItemPathByRelation(layoutIndex));
        this.AddItemToZipStream(str, (Stream) output);
        this.WriteTopRelation(str, layoutSlide.TopRelation);
      }
      else if (this._presentation.Created)
      {
        this.AddItemToZipStream($"ppt/slideLayouts/slideLayout{(object) (num + 1)}.xml", (Stream) output);
        this.WriteLayoutSlideRelation(num + 1, layoutSlide.TopRelation);
      }
      ++num;
    }
  }

  internal void GetLayoutIndex(Syncfusion.Presentation.Presentation presentation, LayoutSlide layoutSlide)
  {
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) presentation.Masters)
    {
      if (master.MergedMasterId == ((MasterSlide) layoutSlide.MasterSlide).MasterId)
      {
        foreach (KeyValuePair<string, string> layout in master.LayoutList)
        {
          if (layout.Value == layoutSlide.LayoutId)
          {
            layoutSlide.LayoutId = ((MasterSlide) layoutSlide.MasterSlide).LayoutList[layout.Key];
            break;
          }
        }
      }
    }
  }

  internal string GetLayoutIndex(LayoutSlide layoutSlide)
  {
    foreach (KeyValuePair<string, string> layout in ((MasterSlide) layoutSlide.MasterSlide).LayoutList)
    {
      if (layout.Value == layoutSlide.LayoutId)
        return layout.Key;
    }
    return (string) null;
  }

  private void WriteLayoutSlideRelation(int i, RelationCollection relationCollection)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream($"ppt/slideLayouts/_rels/slideLayout{(object) i}.xml.rels", (Stream) output);
  }

  private void ParseAndSerializeLayoutRelation(
    ZipArchiveItem item,
    string savePath,
    int num,
    ZipArchive archive,
    Dictionary<string, string> imagePaths)
  {
    if (item == null)
      return;
    RelationCollection relationCollection = new RelationCollection();
    using (XmlReader reader = UtilityMethods.CreateReader(item.DataStream))
      relationCollection = Parser.ParseRelationCollection(reader);
    foreach (Relation relation in relationCollection.GetRelationList())
    {
      if (relation.Target.Contains("slideMasters"))
        relation.Target = $"../slideMasters/slideMaster{(this._presentation.Masters.Count + 1).ToString()}.xml";
      else if (relation.Target.Contains("media"))
      {
        ZipArchiveItem zipArchiveItem = archive["ppt" + relation.Target.Remove(0, 2)].Clone();
        string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(relation.Target);
        string str = $"/media/image{(object) ++this._presentation.ImageCount}{extension}";
        MemoryStream output = new MemoryStream();
        Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
        if (!imagePaths.ContainsKey(relation.Target))
        {
          imagePaths.Add(relation.Target, relation.Target = ".." + str);
          this.AddItemToZipStream("ppt" + str, (Stream) output);
        }
        else
          relation.Target = imagePaths[relation.Target];
        string lower = extension.Remove(0, 1).ToLower();
        this._presentation.AddDefaultContentType(lower, "image/" + lower);
      }
      this.AddOtherMergableItemToArchive(relation, archive);
    }
    MemoryStream stream = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream))
      Serializator.WriteRelationShip(writer, relationCollection.GetRelationList());
    this.AddItemToZipStream(savePath, (Stream) stream);
  }

  private void WriteMasterRelation(int i, RelationCollection relationCollection)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream($"ppt/slideMasters/_rels/slideMaster{(object) (i + 1)}.xml.rels", (Stream) output);
  }

  private void WriteTheme(string themePath, Theme theme)
  {
    int i = 0;
    themePath = Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(themePath);
    string s = Syncfusion.Presentation.Drawing.Helper.GetFileNameWithoutExtension(themePath).Remove(0, 5);
    if (!string.IsNullOrEmpty(s))
      i = int.Parse(s);
    if (theme.FillFormats.Count == 0)
      return;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
    {
      Serializator.WriteTheme1(xmlWriter, theme);
      this.WriteThemeRelation(i, theme.TopRelation);
      xmlWriter.Flush();
    }
    this.AddItemToZipStream(themePath, (Stream) output);
  }

  private void WriteThemeRelation(int i, RelationCollection relationCollection)
  {
    if (relationCollection == null)
      return;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream($"ppt/theme/_rels/theme{i.ToString()}.xml.rels", (Stream) output);
  }

  private void WriteHandoutMaster()
  {
    HandoutMaster handoutMaster = this._presentation.GetHandoutMaster();
    if (handoutMaster == null)
      return;
    this.WriteTheme(handoutMaster.TopRelation.GetItemPathByKeyword("theme"), handoutMaster.ThemeCollection);
    Stream stream = (Stream) new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create(stream))
      Serializator.SerializeHandoutMasterSlide(xmlWriter, (BaseSlide) handoutMaster);
    string itemPathByKeyword = handoutMaster.Presentation.TopRelation.GetItemPathByKeyword("handoutMaster");
    this.AddItemToZipStream(Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(itemPathByKeyword), stream);
    this.WriteTopRelation(itemPathByKeyword, handoutMaster.TopRelation);
    string itemName = handoutMaster.TopRelation.GetItemPathByKeyword("theme").TrimStart('/');
    if (itemName.StartsWith("../"))
      itemName = "ppt" + itemName.Remove(0, 2);
    if (this._archive[itemName] != null)
      return;
    Stream streamFromResource = ResourceManager.GetStreamFromResource("defaultContent");
    using (ZipArchive zipArchive = new ZipArchive())
    {
      zipArchive.Open(streamFromResource, true);
      this.AddItemToZipStream(zipArchive["ppt/theme/theme2.xml"].Clone());
    }
    streamFromResource.Close();
  }

  private void WriteHandoutRelation(RelationCollection relationCollection)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    this.AddItemToZipStream("ppt/handoutMasters/_rels/handoutMaster1.xml.rels", (Stream) output);
  }

  private void WriteContentType()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
    {
      Serializator.WriteContentType(xmlWriter, this._presentation);
      xmlWriter.Flush();
    }
    this.AddItemToZipStream("[Content_Types].xml", (Stream) output);
  }

  private void WritePresentation()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
    {
      Serializator.WritePresentation(xmlWriter, this._presentation);
      xmlWriter.Flush();
    }
    this.AddItemToZipStream("ppt/presentation.xml", (Stream) output);
  }

  private void WritePresentationRelation()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
    {
      Serializator.WriteRelationShip(xmlWriter, this._presentation.TopRelation.GetRelationList());
      xmlWriter.Flush();
    }
    this.AddItemToZipStream("ppt/_rels/presentation.xml.rels", (Stream) output);
  }

  private void WriteRawData(string styleId, Stream stream, XmlWriter xmlWriter)
  {
    if (stream == null || stream.Length <= 0L)
      return;
    stream.Position = 0L;
    using (XmlReader reader = XmlReader.Create(stream))
    {
      while (reader.LocalName != "tblStyle")
        reader.Read();
      if (reader.IsEmptyElement)
        return;
      while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "tblStyle" && reader.GetAttribute(nameof (styleId)) == styleId)
        {
          xmlWriter.WriteNode(reader, false);
          break;
        }
        reader.Skip();
      }
    }
  }

  private void WriteRootRelation()
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
    {
      Serializator.WriteRelationShip(xmlWriter, this.TopRelations.GetRelationList());
      xmlWriter.Flush();
    }
    this.AddItemToZipStream("_rels/.rels", (Stream) output);
  }

  private void WriteTopRelation(string path, RelationCollection relationCollection)
  {
    if (relationCollection == null)
      return;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    string[] strArray = path.TrimStart('/').Split('/');
    if (!path.Contains("ppt"))
      this.AddItemToZipStream($"ppt/{strArray[0]}/_rels/{strArray[1]}.rels", (Stream) output);
    else
      this.AddItemToZipStream($"ppt/{strArray[1]}/_rels/{strArray[2]}.rels", (Stream) output);
  }

  private void WriteSlides()
  {
    foreach (Slide slide in (IEnumerable<ISlide>) this._presentation.Slides)
    {
      MemoryStream output = new MemoryStream();
      using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
        Serializator.SerializeSlide(xmlWriter, slide);
      this.WriteVmlDrawing((BaseSlide) slide);
      this.WriteNotesSlide(slide);
      this.WriteComments(slide);
      string slidePath = Syncfusion.Presentation.Drawing.Helper.GetSlidePath(slide.Presentation.SlideList, slide.Presentation.TopRelation, slide.SlideID.ToString());
      this.AddItemToZipStream(Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(slidePath), (Stream) output);
      this.WriteTopRelation(slidePath, slide.TopRelation);
    }
  }

  private void WriteComments(Slide slide)
  {
    if (!slide.HasComment)
      return;
    Comments comments = (Comments) slide.Comments;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.SerializeComments(xmlWriter, comments);
    string str;
    if (!slide.IsCloned)
    {
      str = slide.TopRelation.GetItemPathByKeyword("comments");
    }
    else
    {
      slide.TopRelation.GetTargetByRelationId(slide.CommentRelationId);
      str = "/ppt/comments/" + $"comments{(object) (slide.Presentation.CommentCount + 1)}.xml";
      slide.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments").Target = str;
      this._presentation.AddOverrideContentType(str, "application/vnd.openxmlformats-officedocument.presentationml.comments+xml");
    }
    this.AddItemToZipStream(Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(str), (Stream) output);
  }

  private void WriteNotesSlide(Slide slide)
  {
    if (!slide.HasNotes)
      return;
    NotesSlide notesSlide = slide.NotesSlide as NotesSlide;
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.SerializeNotesSlide(xmlWriter, notesSlide);
    string str = Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(slide.TopRelation.GetItemPathByKeyword("notesSlide"));
    this.AddItemToZipStream(str, (Stream) output);
    this.WriteTopRelation(str, notesSlide.TopRelation);
  }

  internal void WriteChart(PresentationChart chart)
  {
    this.EmbedChartData(chart);
    MemoryStream output = new MemoryStream();
    bool isChartEX = FileDataHolder.IsChartExSerieType(chart.ChartType);
    using (XmlWriter writer = XmlWriter.Create((Stream) output))
    {
      if (isChartEX)
        new ChartExSerializator().SerializeChartEx(writer, chart.GetChartImpl());
      else
        new ChartSerializator().SerializeChart(writer, chart.GetChartImpl(), string.Empty);
    }
    int itemsCount = 0;
    string str1 = (string) null;
    string str2 = (string) null;
    string itemName = Syncfusion.Presentation.Drawing.Helper.GenerateItemName(out itemsCount, $"ppt/charts/chart{(isChartEX ? "Ex" : "")}{{0}}.xml", this._archive);
    int excelCount = chart.BaseSlide.Presentation.ExcelCount;
    if (chart.ShapeName == null)
      chart.ShapeName = "Chart " + (object) itemsCount;
    this.AddItemToZipStream(itemName, (Stream) output);
    chart.RelationId = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(chart.BaseSlide.TopRelation);
    Relation relation1 = !isChartEX ? new Relation(chart.RelationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart", itemName.Replace("ppt", ".."), (string) null) : new Relation(chart.RelationId, "http://schemas.microsoft.com/office/2014/relationships/chartEx", itemName.Replace("ppt", ".."), (string) null);
    chart.BaseSlide.TopRelation.Add(relation1.Id, relation1);
    ChartImpl chartImpl = chart.GetChartImpl();
    if (chart.TopRelation.Count > 0)
    {
      Relation relation2 = chart.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject") ?? chart.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/package");
      Relation relation3 = new Relation("rId1", relation2.Type, relation2.Target, relation2.TargetMode);
      chart.TopRelation.Clear();
      chart.TopRelation.Add(relation3.Id, relation3);
    }
    else
    {
      string relationId = "rId1";
      Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation relation4 = (Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation) null;
      if (chartImpl.Relations.Count > 0)
        relation4 = chartImpl.Relations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/package", out relationId);
      if (relation4 == null)
        chartImpl.Relations[relationId] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package");
      Relation relation5 = new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package", $"../embeddings/Microsoft_Excel_Worksheet{(object) excelCount}.xlsx", (string) null);
      chart.TopRelation.Add(relation5.Id, relation5);
      Stream workbookStream = this.GetWorkbookStream(chart);
      this.AddItemToZipStream($"ppt/embeddings/Microsoft_Excel_Worksheet{(object) excelCount}.xlsx", workbookStream);
      ++chart.BaseSlide.Presentation.ExcelCount;
      if (isChartEX)
      {
        string id1 = "rId2";
        if (chartImpl.Relations.Count > 0)
          id1 = chartImpl.Relations.GenerateRelationId();
        Relation relation6 = new Relation(id1, "http://schemas.microsoft.com/office/2011/relationships/chartStyle", $"style{(object) itemsCount}.xml", (string) null);
        chartImpl.Relations[id1] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", "http://schemas.microsoft.com/office/2011/relationships/chartStyle");
        chart.TopRelation.Add(relation6.Id, relation6);
        str1 = $"ppt/charts/style{(object) itemsCount}.xml";
        string id2 = "rId3";
        if (chartImpl.Relations.Count > 0)
          id2 = chartImpl.Relations.GenerateRelationId();
        Relation relation7 = new Relation(id2, "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle", $"colors{(object) itemsCount}.xml", (string) null);
        chartImpl.Relations[id2] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle");
        str2 = $"ppt/charts/colors{(object) itemsCount}.xml";
        chart.TopRelation.Add(relation7.Id, relation7);
        this.SerializeChartExStyles((WorksheetBaseImpl) chartImpl, itemsCount);
      }
    }
    this.WriteChartPreservedStreams(chart, itemsCount);
    this.WriteChartRelation(itemsCount, chart.TopRelation, isChartEX);
    this._presentation.AddDefaultContentType("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    if (isChartEX)
    {
      this._presentation.AddOverrideContentType("/" + itemName, "application/vnd.ms-office.chartex+xml");
      if (!this._presentation.Created)
        return;
      this._presentation.AddOverrideContentType("/" + str1, "application/vnd.ms-office.chartstyle+xml");
      this._presentation.AddOverrideContentType("/" + str2, "application/vnd.ms-office.chartcolorstyle+xml");
    }
    else
      this._presentation.AddOverrideContentType("/" + itemName, "application/vnd.openxmlformats-officedocument.drawingml.chart+xml");
  }

  private void WriteChartPreservedStreams(PresentationChart chart, int itemsCount)
  {
    ChartImpl chartImpl = chart.GetChartImpl();
    foreach (KeyValuePair<string, Stream> relationPreservedStream in chartImpl.RelationPreservedStreamCollection)
    {
      if (relationPreservedStream.Value.Length > 0L)
      {
        string target = (string) null;
        string itemPath = (string) null;
        string contentType = (string) null;
        string relationId = (string) null;
        Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation relation1 = (Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation) null;
        string type;
        if (relationPreservedStream.Key.StartsWith("rId"))
        {
          type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image";
          string strExtension;
          contentType = Syncfusion.OfficeChart.Implementation.XmlSerialization.FileDataHolder.GetPictureContentType(ApplicationImpl.CreateImage(relationPreservedStream.Value).RawFormat, out strExtension);
          int num = ++this._presentation.ImageCount;
          target = $"../media/image{num.ToString()}.{strExtension}";
          itemPath = $"ppt/media/image{num.ToString()}.{strExtension}";
          relationId = relationPreservedStream.Key;
        }
        else
        {
          type = relationPreservedStream.Key;
          if (chartImpl.Relations.Count > 0)
            relation1 = chartImpl.Relations.FindRelationByContentType(relationPreservedStream.Key, out relationId);
          if (relation1 == null)
          {
            relationId = chartImpl.Relations.GenerateRelationId();
            chartImpl.Relations[relationId] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", relationPreservedStream.Key);
          }
          if (relationPreservedStream.Key == "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle")
          {
            target = $"colors{itemsCount.ToString()}.xml";
            itemPath = $"ppt/charts/colors{(object) itemsCount}.xml";
            contentType = "application/vnd.ms-office.chartcolorstyle+xml";
          }
          else if (relationPreservedStream.Key == "http://schemas.microsoft.com/office/2011/relationships/chartStyle")
          {
            target = $"style{itemsCount.ToString()}.xml";
            itemPath = $"ppt/charts/style{(object) itemsCount}.xml";
            contentType = "application/vnd.ms-office.chartstyle+xml";
          }
          else if (relationPreservedStream.Key == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartUserShapes")
          {
            target = $"../drawings/drawing{itemsCount.ToString()}.xml";
            itemPath = $"ppt/drawings/drawing{(object) itemsCount}.xml";
            contentType = "application/vnd.openxmlformats-officedocument.drawingml.chartshapes+xml";
          }
        }
        if (relationId != null)
        {
          Relation relation2 = new Relation(relationId, type, target, (string) null);
          chart.TopRelation.Add(relationId, relation2);
          this.AddItemToZipStream(itemPath, relationPreservedStream.Value);
          this._presentation.AddOverrideContentType("/" + itemPath, contentType);
        }
      }
    }
  }

  internal void SerializeChartExStyles(WorksheetBaseImpl chart, int itemCount)
  {
    if (!(chart is ChartImpl chart1) || !ChartImpl.IsChartExSerieType(chart1.ChartType))
      return;
    if (!chart1.m_isChartStyleSkipped)
    {
      string relationId = "rId2";
      Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation relation1 = (Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation) null;
      if (chart1.Relations.Count > 0)
        relation1 = chart1.Relations.FindRelationByContentType("http://schemas.microsoft.com/office/2011/relationships/chartStyle", out relationId);
      if (relation1 == null)
        chart1.Relations[relationId] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", "http://schemas.microsoft.com/office/2011/relationships/chartStyle");
      Relation relation2 = new Relation(relationId, "http://schemas.microsoft.com/office/2011/relationships/chartStyle", $"style{(object) itemCount}.xml", (string) null);
      MemoryStream memoryStream = new MemoryStream();
      StreamWriter data = new StreamWriter((Stream) memoryStream);
      string itemName = Syncfusion.Presentation.Drawing.Helper.GenerateItemName(out itemCount, "ppt/charts/style{0}.xml", this._archive);
      this.SerializeDefaultChartStyles(UtilityMethods.CreateWriter((TextWriter) data), chart1, chart1.AppImplementation);
      data.Flush();
      memoryStream.Flush();
      this.AddItemToZipStream(itemName, (Stream) memoryStream);
    }
    if (chart1.m_isChartColorStyleSkipped)
      return;
    Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation relation3 = (Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation) null;
    string relationId1 = "rId3";
    if (chart1.Relations.Count > 0)
      relation3 = chart1.Relations.FindRelationByContentType("http://schemas.microsoft.com/office/2011/relationships/chartColorStyle", out relationId1);
    if (relation3 == null)
      chart1.Relations[relationId1] = new Syncfusion.OfficeChart.Implementation.XmlSerialization.Relation("", "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle");
    Relation relation4 = new Relation(relationId1, "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle", $"colors{(object) itemCount}.xml", (string) null);
    string itemName1 = Syncfusion.Presentation.Drawing.Helper.GenerateItemName(out itemCount, "ppt/charts/colors{0}.xml", this._archive);
    MemoryStream memoryStream1 = new MemoryStream();
    StreamWriter data1 = new StreamWriter((Stream) memoryStream1);
    this.SerializeDefaultChartColorStyles(UtilityMethods.CreateWriter((TextWriter) data1), chart1.AppImplementation);
    data1.Flush();
    memoryStream1.Flush();
    this.AddItemToZipStream(itemName1, (Stream) memoryStream1);
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
          if (chart.ChartType == OfficeChartType.Funnel && key == 1)
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
    shapeStyle4.ShapeProperties.ShapeFillType = OfficeFillType.SolidColor;
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
    shapeStyle6.TextBodyProperties.TextDirection = Syncfusion.OfficeChart.TextDirection.Horizontal;
    shapeStyle6.TextBodyProperties.VerticalAlignment = OfficeVerticalAlignment.Middle;
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
    shapeStyle7.ShapeProperties.ShapeFillType = OfficeFillType.SolidColor;
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
    shapeStyle7.TextBodyProperties.TextDirection = Syncfusion.OfficeChart.TextDirection.Horizontal;
    shapeStyle7.TextBodyProperties.VerticalAlignment = OfficeVerticalAlignment.MiddleCentered;
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

  internal static bool IsChartExSerieType(OfficeChartType type)
  {
    switch (type)
    {
      case OfficeChartType.Pareto:
      case OfficeChartType.Funnel:
      case OfficeChartType.Histogram:
      case OfficeChartType.WaterFall:
      case OfficeChartType.TreeMap:
      case OfficeChartType.SunBurst:
      case OfficeChartType.BoxAndWhisker:
        return true;
      default:
        return false;
    }
  }

  internal void EmbedChartData(PresentationChart chart)
  {
    if (!chart.UseExcelDataRange)
    {
      string embedWorkBook = this.GetEmbedWorkBook(chart);
      if (embedWorkBook != null)
        this._archive.RemoveItem(embedWorkBook);
    }
    if (chart.ChartDataRangeSet)
      return;
    chart.GetChartImpl().DataIRange = chart.Workbook.ActiveSheet.UsedRange;
    chart.ChartDataRangeSet = true;
  }

  private Stream GetWorkbookStream(PresentationChart chart)
  {
    Stream stream1 = (Stream) null;
    bool defaultExcelFile = true;
    if (chart.EmbeddedWorkbookStream != null && chart.EmbeddedWorkbookStream.Length != 0L)
    {
      chart.EmbeddedWorkbookStream.Position = 0L;
      stream1 = chart.EmbeddedWorkbookStream;
      defaultExcelFile = false;
    }
    else if (!chart.IsParsedChart || this.excelFile != null && !this.excelFile.CanRead)
      stream1 = ResourceManager.GetStreamFromResource("ExcelTemplate", ".xlsx");
    ZipArchive archive = new ZipArchive();
    if (stream1 != null && stream1.Length != 0L)
    {
      archive.Open(stream1, true);
      archive.RemoveItem("xl/sharedStrings.xml");
      archive.RemoveItem("xl/styles.xml");
      MemoryStream stream2 = new MemoryStream();
      Dictionary<int, int> styleIndex = chart.Workbook.DataHolder.SaveStyles(archive, stream2);
      chart.Workbook.DataHolder.Serializator.SerializeWorksheets(archive, (IWorkbook) chart.Workbook, chart.GetChartImpl(), chart.WorkSheetIndex, styleIndex, defaultExcelFile);
      MemoryStream memoryStream = new MemoryStream();
      using (XmlWriter writer = XmlWriter.Create((Stream) memoryStream))
        chart.Workbook.DataHolder.Serializator.SerializeSST(writer);
      archive.AddItem("xl/sharedStrings.xml", (Stream) memoryStream, true, FileAttributes.Archive);
    }
    MemoryStream workbookStream = new MemoryStream();
    archive.Save((Stream) workbookStream, false);
    return (Stream) workbookStream;
  }

  private void WriteChartRelation(int i, RelationCollection relationCollection, bool isChartEX)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.WriteRelationShip(xmlWriter, relationCollection.GetRelationList());
    if (isChartEX)
      this.AddItemToZipStream($"ppt/charts/_rels/chartEx{(object) i}.xml.rels", (Stream) output);
    else
      this.AddItemToZipStream($"ppt/charts/_rels/chart{(object) i}.xml.rels", (Stream) output);
  }

  private void WriteTableStyles()
  {
    MemoryStream stream = new MemoryStream();
    using (XmlWriter writer = UtilityMethods.CreateWriter(stream))
    {
      Serializator.SerializeTableStyles(writer, this._presentation);
      writer.Flush();
    }
    this.AddItemToZipStream("ppt/tableStyles.xml", (Stream) stream);
  }

  public void Dispose() => this.Close();

  internal void Close()
  {
    if (this._isDisposed)
      return;
    if (this._topRelations != null)
    {
      this._topRelations.Close();
      this._topRelations = (RelationCollection) null;
    }
    if (this._archive != null)
    {
      this._archive.Dispose();
      this._archive = (ZipArchive) null;
    }
    if (this._dictItemsToRemove != null)
    {
      this._dictItemsToRemove.Clear();
      this._dictItemsToRemove = (List<string>) null;
    }
    if (this._items != null)
    {
      this._items.Clear();
      this._items = (List<string>) null;
    }
    if (this._mediaItems != null)
    {
      this._mediaItems.Clear();
      this._mediaItems = (Dictionary<string, string>) null;
    }
    this.excelFile.Dispose();
    this._presentation = (Syncfusion.Presentation.Presentation) null;
    this._isDisposed = true;
  }

  public FileDataHolder Clone()
  {
    FileDataHolder fileDataHolder = (FileDataHolder) this.MemberwiseClone();
    fileDataHolder._archive = this._archive.Clone();
    fileDataHolder._dictItemsToRemove = Syncfusion.Presentation.Drawing.Helper.CloneList(this._dictItemsToRemove);
    if (this._items != null)
      fileDataHolder._items = Syncfusion.Presentation.Drawing.Helper.CloneList(this._items);
    fileDataHolder._topRelations = this._topRelations.Clone();
    Stream stream = (Stream) new MemoryStream();
    this.excelFile.Position = 0L;
    byte[] buffer = new byte[this.excelFile.Length];
    int count;
    while ((count = this.excelFile.Read(buffer, 0, buffer.Length)) > 0)
      stream.Write(buffer, 0, count);
    stream.Position = 0L;
    fileDataHolder.excelFile = stream;
    return fileDataHolder;
  }

  internal void SetBackgroundColor(Slide slide)
  {
    slide.LayoutIndex = slide.ObtainLayoutIndex();
    if (slide.LayoutIndex == null || !this._presentation.GetSlideLayout().ContainsKey(slide.LayoutIndex))
      return;
    MasterSlide master = slide.Presentation.GetMaster(slide.LayoutIndex);
    for (int index = 0; index < master.LayoutList.Count; ++index)
    {
      LayoutSlide layoutSlide = (LayoutSlide) master.LayoutSlides[index];
      if (slide.LayoutIndex == layoutSlide.LayoutId)
      {
        slide.Background = layoutSlide.Background;
        break;
      }
    }
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }

  internal void AddLayoutsToArchive(
    Syncfusion.Presentation.Presentation sourcePresentation,
    MasterSlide sourceMaster,
    Slide slideClone,
    bool isMerged)
  {
    ZipArchive archive = sourcePresentation.DataHolder._archive;
    int layoutCount = 0;
    bool flag = false;
    if (isMerged)
      flag = this.CheckIsLayoutContains(slideClone, ref layoutCount);
    if (!flag)
      layoutCount = this.GenerateLayoutCountFromMasterSlide();
    string[] fromSourceMaster = this.GetPathArrayFromSourceMaster(sourceMaster, archive);
    Dictionary<string, string> imagePaths = new Dictionary<string, string>();
    if (!flag)
    {
      this.AddBackgroundImageToArchive((Background) sourceMaster.Background, imagePaths, archive);
      this.SetExternalImageToArchive(sourceMaster.Shapes, imagePaths, archive);
    }
    this.AddBackgroundImageToArchive((Background) slideClone.Background, imagePaths, archive);
    this.SetExternalImageToArchive(slideClone.Shapes, imagePaths, archive);
    if (!flag)
    {
      this.MergeThemeElementsFromMaster(sourceMaster, imagePaths, archive);
      this.MergeParsedLayoutSlides(sourceMaster, archive, imagePaths);
    }
    this.SortPathArray(ref fromSourceMaster);
    this.MergeSlideCloneItems(slideClone, fromSourceMaster, layoutCount, archive, isMerged);
    if (flag)
      return;
    this.MergeUnParsedElements(fromSourceMaster, archive, layoutCount, imagePaths);
  }

  private bool CheckIsLayoutContains(Slide slideClone, ref int layoutCount)
  {
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
    {
      if (((MasterSlide) slideClone.LayoutSlide.MasterSlide).MergedMasterId == master.MergedMasterId)
      {
        LayoutSlide layoutSlide = slideClone.LayoutSlide as LayoutSlide;
        if (master.LayoutList.ContainsValue(layoutSlide.LayoutId))
          return true;
        if (master.OldLayoutList != null && master.OldLayoutList.ContainsValue(layoutSlide.LayoutId))
        {
          foreach (KeyValuePair<string, string> oldLayout in master.OldLayoutList)
          {
            if (oldLayout.Value == layoutSlide.LayoutId)
            {
              layoutSlide.LayoutId = master.LayoutList[oldLayout.Key];
              return true;
            }
          }
        }
      }
      layoutCount += master.LayoutList.Count;
    }
    return false;
  }

  private void MergeUnParsedElements(
    string[] pathArray,
    ZipArchive zipArchive,
    int layoutCount,
    Dictionary<string, string> imagePaths)
  {
    foreach (string path in pathArray)
    {
      ++layoutCount;
      foreach (ZipArchiveItem zipArchiveItem1 in zipArchive.Items)
      {
        if (zipArchiveItem1.ItemName.Contains(path + ".xml"))
        {
          string[] strArray = path.Split('t');
          int.Parse(strArray[1]);
          ZipArchiveItem zipArchiveItem2 = zipArchiveItem1.Clone();
          if (zipArchiveItem2.ItemName.Contains("rels"))
          {
            string savePath = $"ppt/slideLayouts/_rels/{strArray[0]}t{layoutCount.ToString()}.xml.rels";
            this.ParseAndSerializeLayoutRelation(zipArchiveItem2, savePath, layoutCount, zipArchive, imagePaths);
          }
          else
          {
            zipArchiveItem2.ItemName = $"ppt/slideLayouts/{strArray[0]}t{layoutCount.ToString()}.xml";
            this._presentation.AddOverrideContentType("/" + zipArchiveItem2.ItemName, "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml");
            Stream stream = (Stream) new MemoryStream();
            Picture.CopyStream(zipArchiveItem2.DataStream, stream);
            this.AddItemToZipStream(zipArchiveItem2.ItemName, stream);
          }
        }
      }
    }
  }

  private void MergeSlideCloneItems(
    Slide slideClone,
    string[] pathArray,
    int layoutCount,
    ZipArchive zipArchive,
    bool isMerged)
  {
    foreach (Relation relation in slideClone.TopRelation.GetRelationList())
    {
      if (relation.Target.Contains("slideLayout"))
      {
        int num1 = int.Parse(Syncfusion.Presentation.Drawing.Helper.GetFileNameWithoutExtension(relation.Target).Remove(0, 11));
        int pathPosition1;
        if (isMerged && this._presentation.MasterList.Count > 1)
        {
          string[] fromSourceMaster = this.GetPathArrayFromSourceMaster(slideClone.LayoutSlide.MasterSlide as MasterSlide);
          this.SortPathArray(ref fromSourceMaster);
          int pathPosition2 = this.GetPathPosition(fromSourceMaster, num1);
          pathPosition1 = this.GetPathPosition(pathArray, pathPosition2 + layoutCount);
        }
        else
          pathPosition1 = this.GetPathPosition(pathArray, num1);
        int num2 = pathPosition1 + layoutCount;
        relation.Target = $"../slideLayouts/slideLayout{num2.ToString()}.xml";
      }
      this.AddOtherMergableItemToArchive(relation, zipArchive);
    }
  }

  private void MergeParsedLayoutSlides(
    MasterSlide sourceMaster,
    ZipArchive zipArchive,
    Dictionary<string, string> imagePaths)
  {
    foreach (ILayoutSlide layoutSlide in (IEnumerable<ILayoutSlide>) sourceMaster.LayoutSlides)
    {
      this.SetExternalImageToArchive(layoutSlide.Shapes, imagePaths, zipArchive);
      this.AddBackgroundImageToArchive((Background) layoutSlide.Background, imagePaths, zipArchive);
      foreach (Relation relation in ((BaseSlide) layoutSlide).TopRelation.GetRelationList())
        this.AddOtherMergableItemToArchive(relation, zipArchive);
    }
  }

  private void MergeThemeElementsFromMaster(
    MasterSlide sourceMaster,
    Dictionary<string, string> imagePaths,
    ZipArchive zipArchive)
  {
    if (sourceMaster.Theme.TopRelation == null)
      return;
    foreach (Relation relation in sourceMaster.Theme.TopRelation.GetRelationList())
    {
      if (relation.Target.Contains("media"))
      {
        string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(relation.Target);
        ZipArchiveItem zipArchiveItem = zipArchive["ppt" + relation.Target.Remove(0, 2)];
        string str = $"/media/image{(object) ++this._presentation.ImageCount}{extension}";
        MemoryStream output = new MemoryStream();
        Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
        if (!imagePaths.ContainsKey(relation.Target))
        {
          imagePaths.Add(relation.Target, relation.Target = ".." + str);
          this.AddItemToZipStream("ppt" + str, (Stream) output);
        }
        else
          relation.Target = imagePaths[relation.Target];
        sourceMaster.Theme.MergedImagePathList.Add(relation.Id, relation.Target);
        string lower = extension.Remove(0, 1).ToLower();
        this._presentation.AddDefaultContentType(lower, "image/" + lower);
      }
    }
  }

  private string[] GetPathArrayFromSourceMaster(MasterSlide sourceMaster, ZipArchive zipArchive)
  {
    RelationCollection topRelation = sourceMaster.TopRelation;
    string[] fromSourceMaster = new string[sourceMaster.LayoutList.Count];
    int index = 0;
    foreach (Relation relation in topRelation.GetRelationList())
    {
      if (relation.Target.Contains("slideLayout"))
      {
        fromSourceMaster[index] = Syncfusion.Presentation.Drawing.Helper.GetFileNameWithoutExtension(relation.Target.Remove(0, 16 /*0x10*/));
        ++index;
      }
      this.AddOtherMergableItemToArchive(relation, zipArchive);
    }
    return fromSourceMaster;
  }

  private string[] GetPathArrayFromSourceMaster(MasterSlide sourceMaster)
  {
    RelationCollection topRelation = sourceMaster.TopRelation;
    string[] fromSourceMaster = new string[sourceMaster.LayoutList.Count];
    int index = 0;
    foreach (Relation relation in topRelation.GetRelationList())
    {
      if (relation.Target.Contains("slideLayout"))
      {
        fromSourceMaster[index] = Syncfusion.Presentation.Drawing.Helper.GetFileNameWithoutExtension(relation.Target.Remove(0, 16 /*0x10*/));
        ++index;
      }
    }
    return fromSourceMaster;
  }

  private int GenerateLayoutCountFromMasterSlide()
  {
    int countFromMasterSlide = 0;
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
    {
      if (master.LayoutList != null)
        countFromMasterSlide += master.LayoutList.Count;
    }
    return countFromMasterSlide;
  }

  internal void AddCommentArchiveItem(Relation relation, ZipArchive archive)
  {
    string str = "ppt" + relation.Target.Remove(0, 2);
    ZipArchiveItem zipArchiveItem = archive[str];
    if (zipArchiveItem == null)
      return;
    MemoryStream output = new MemoryStream();
    Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
    if (!str.Contains("comments/comment"))
      return;
    if (this._archive[zipArchiveItem.ItemName] == null)
    {
      this.AddItemToZipStream(zipArchiveItem.ItemName, (Stream) output);
    }
    else
    {
      int count = 0;
      this.SetItemCount(ref count, "ppt/comments/comment");
      int num;
      relation.Target = $"../comments/comment{(object) (num = count + 1)}.xml";
      str = "ppt" + relation.Target.Remove(0, 2);
      this.AddItemToZipStream(str, (Stream) output);
    }
    this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.presentationml.comments+xml");
  }

  private void AddOtherMergableItemToArchive(Relation relation, ZipArchive archive)
  {
    if (relation.Target.Contains("themeOverride") || relation.Target.Contains("ink") || relation.Target.Contains("oleObject") || relation.Target.Contains("vmlDrawing") || relation.Target.Contains("printerSettings") || relation.Target.Contains("tag"))
    {
      string itemName = "ppt" + relation.Target.Remove(0, 2);
      string contentType = (string) null;
      string itemPath;
      if (relation.Target.Contains("themeOverride"))
      {
        string str1 = itemName.Substring(0, itemName.LastIndexOf('e') + 1);
        int num = 1;
        string str2 = relation.Target.Substring(0, itemName.LastIndexOf('e'));
        foreach (ZipArchiveItem zipArchiveItem in this._archive.Items)
        {
          if (zipArchiveItem.ItemName.Contains(str1))
            ++num;
        }
        itemPath = $"{str1}{(object) num}.xml";
        string str3 = $"{str2}{(object) num}.xml";
        relation.Target = str3;
      }
      else
        itemPath = itemName;
      ZipArchiveItem zipArchiveItem1 = archive[itemName];
      if (zipArchiveItem1 == null)
        return;
      MemoryStream output = new MemoryStream();
      Picture.CopyStream(zipArchiveItem1.DataStream, (Stream) output);
      if (!itemName.Contains("oleObject") && !itemName.Contains("vmlDrawing"))
        this.AddItemToZipStream(itemPath, (Stream) output);
      if (itemName.Contains("oleObject"))
      {
        if (this._archive[zipArchiveItem1.ItemName] == null)
        {
          this.AddItemToZipStream(zipArchiveItem1.ItemName, (Stream) output);
        }
        else
        {
          int count = 0;
          this.SetItemCount(ref count, "ppt/embeddings/oleObject");
          int num;
          relation.Target = $"../embeddings/oleObject{(object) (num = count + 1)}.bin";
          this.AddItemToZipStream("ppt" + relation.Target.Remove(0, 2), (Stream) output);
        }
        this._presentation.AddDefaultContentType("bin", "application/vnd.openxmlformats-officedocument.oleObject");
      }
      else if (itemName.Contains("vmlDrawing"))
      {
        if (this._archive[zipArchiveItem1.ItemName] == null)
        {
          this.AddItemToZipStream(zipArchiveItem1.ItemName, (Stream) output);
        }
        else
        {
          int count = 0;
          this.SetItemCount(ref count, "ppt/drawings/vmlDrawing");
          int num;
          relation.Target = $"../drawings/vmlDrawing{(object) (num = count + 1)}.vml";
          this.AddItemToZipStream("ppt" + relation.Target.Remove(0, 2), (Stream) output);
        }
        this._presentation.AddDefaultContentType("vml", "application/vnd.openxmlformats-officedocument.vmlDrawing");
      }
      else if (itemName.Contains("ink"))
        contentType = "application/inkml+xml";
      else if (itemName.Contains("themeOverride"))
        contentType = "application/vnd.openxmlformats-officedocument.themeOverride+xml";
      else if (itemName.Contains("printerSettings"))
        this._presentation.AddDefaultContentType("bin", "application/vnd.openxmlformats-officedocument.presentationml.printerSettings");
      else if (itemName.Contains("tag"))
        contentType = "application/vnd.openxmlformats-officedocument.presentationml.tags+xml";
      if (contentType == null)
        return;
      this._presentation.AddOverrideContentType("/" + itemPath, contentType);
    }
    else if (relation.Target.Contains("diagrams"))
    {
      string str = "ppt" + relation.Target.Remove(0, 2);
      if (str.Contains("diagrams/data"))
        this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.drawingml.diagramData+xml");
      ZipArchiveItem zipArchiveItem = archive[str];
      if (zipArchiveItem == null)
        return;
      MemoryStream output = new MemoryStream();
      Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
      if (str.Contains("diagrams/layout") || str.Contains("diagrams/drawing"))
      {
        if (this._archive[zipArchiveItem.ItemName] == null)
        {
          this.AddItemToZipStream(zipArchiveItem.ItemName, (Stream) output);
        }
        else
        {
          int count = 0;
          this.SetItemCount(ref count, "ppt/diagrams/layout");
          int num;
          relation.Target = $"../diagrams/layout{(object) (num = count + 1)}.xml";
          str = "ppt" + relation.Target.Remove(0, 2);
          this.AddItemToZipStream(str, (Stream) output);
        }
        this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.drawingml.diagramLayout+xml");
      }
      else if (str.Contains("diagrams/quickStyle"))
      {
        if (this._archive[zipArchiveItem.ItemName] == null)
        {
          this.AddItemToZipStream(zipArchiveItem.ItemName, (Stream) output);
        }
        else
        {
          int count = 0;
          this.SetItemCount(ref count, "ppt/diagrams/quickStyle");
          int num;
          relation.Target = $"../diagrams/quickStyle{(object) (num = count + 1)}.xml";
          str = "ppt" + relation.Target.Remove(0, 2);
          this.AddItemToZipStream(str, (Stream) output);
        }
        this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.drawingml.diagramStyle+xml");
      }
      else
      {
        if (!str.Contains("diagrams/colors"))
          return;
        if (this._archive[zipArchiveItem.ItemName] == null)
        {
          this.AddItemToZipStream(zipArchiveItem.ItemName, (Stream) output);
        }
        else
        {
          int count = 0;
          this.SetItemCount(ref count, "ppt/diagrams/colors");
          int num;
          relation.Target = $"../diagrams/colors{(object) (num = count + 1)}.xml";
          str = "ppt" + relation.Target.Remove(0, 2);
          this.AddItemToZipStream(str, (Stream) output);
        }
        this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.drawingml.diagramColors+xml");
      }
    }
    else
    {
      if (!relation.Target.Contains("comment"))
        return;
      string str = "ppt" + relation.Target.Remove(0, 2);
      ZipArchiveItem zipArchiveItem = archive[str];
      if (zipArchiveItem == null)
        return;
      MemoryStream output = new MemoryStream();
      Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
      if (!str.Contains("comments/comment"))
        return;
      if (this._archive[zipArchiveItem.ItemName] == null)
      {
        this.AddItemToZipStream(zipArchiveItem.ItemName, (Stream) output);
      }
      else
      {
        int count = 0;
        this.SetItemCount(ref count, "ppt/comments/comment");
        int num;
        relation.Target = $"../comments/comment{(object) (num = count + 1)}.xml";
        str = "ppt" + relation.Target.Remove(0, 2);
        this.AddItemToZipStream(str, (Stream) output);
      }
      this._presentation.AddOverrideContentType("/" + str, "application/vnd.openxmlformats-officedocument.presentationml.comments+xml");
    }
  }

  private void SetItemCount(ref int count, string itemName)
  {
    int num = 0;
    foreach (ZipArchiveItem zipArchiveItem in this._archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains(itemName))
        ++num;
    }
    if (num == 0)
      return;
    count = num;
  }

  private int GetPathPosition(string[] pathArray, int num)
  {
    int pathPosition = 1;
    string[] strArray = pathArray;
    for (int index = 0; index < strArray.Length && !strArray[index].Remove(0, 11).Equals(num.ToString()); ++index)
      ++pathPosition;
    return pathPosition;
  }

  private void SortPathArray(ref string[] pathArray)
  {
    int[] array = new int[pathArray.Length];
    int index1 = 0;
    foreach (string str in pathArray)
    {
      array[index1] = int.Parse(str.Remove(0, 11));
      ++index1;
    }
    Array.Sort<int>(array);
    int index2 = 0;
    pathArray = new string[array.Length];
    for (; index2 < array.Length; ++index2)
      pathArray[index2] = "slideLayout" + (object) array[index2];
  }

  private void AddBackgroundImageToArchive(
    Background background,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    if (background.Type != BackgroundType.OwnBackground || background.Fill.FillType != FillType.Picture)
      return;
    TextureFill pictureFill = (TextureFill) background.Fill.PictureFill;
    string imagePath = pictureFill.GetImagePath();
    if (imagePath == null || archive["ppt" + imagePath.Remove(0, 2)] == null)
      return;
    ZipArchiveItem zipArchiveItem = archive["ppt" + imagePath.Remove(0, 2)];
    if (zipArchiveItem == null)
      return;
    string path;
    if (!imagePaths.ContainsKey(imagePath))
    {
      Dictionary<string, string> dictionary = imagePaths;
      string key = imagePath;
      string str1 = (++this._presentation.ImageCount).ToString();
      string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(imagePath);
      string str2;
      path = str2 = $"../media/image{str1}{extension}";
      dictionary.Add(key, str2);
      zipArchiveItem.ItemName = "ppt" + path.Remove(0, 2);
      this.AddItemToZipStream(zipArchiveItem.Clone());
    }
    else
      path = imagePaths[imagePath];
    pictureFill.SetImagePath(path);
  }

  internal void SetExternalImageToArchive(
    IShapes shapes,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    this.GetBulletPictureFromDefaultTextStyle(((Shapes) shapes).BaseSlide, imagePaths, archive);
    foreach (IShape shape in (IEnumerable<ISlideItem>) shapes)
    {
      bool flag = false;
      this.PreserveBulletFromSource((Syncfusion.Presentation.RichText.TextBody) shape.TextBody, imagePaths, archive);
      if (((Shape) shape).ShapeType == ShapeType.AlternateContent && !flag)
      {
        this.AddAlternateContentToArchive((Shape) shape, imagePaths, archive);
        flag = true;
      }
      else if (((Shape) shape).ShapeType == ShapeType.GraphicFrame && !flag)
      {
        if (shape.SlideItemType == SlideItemType.SmartArt)
        {
          foreach (SmartArtPoint point in (shape as SmartArt).DataModel.PointCollection.GetPointList())
          {
            SmartArtShape pointShapeProperties = point.PointShapeProperties;
            if (pointShapeProperties.Fill.FillType == FillType.Picture)
            {
              TextureFill pictureFill = (TextureFill) pointShapeProperties.Fill.PictureFill;
              string imagePath = pictureFill.GetImagePath();
              if (imagePath != null && archive["ppt" + imagePath.Remove(0, 2)] != null)
              {
                ZipArchiveItem itemToAdd = archive["ppt" + imagePath.Remove(0, 2)].Clone();
                string path;
                if (!imagePaths.ContainsKey(imagePath))
                {
                  string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(imagePath);
                  Dictionary<string, string> dictionary = imagePaths;
                  string key = imagePath;
                  string str1 = (++this._presentation.ImageCount).ToString();
                  string str2 = extension;
                  string str3;
                  path = str3 = $"../media/image{str1}{str2}";
                  dictionary.Add(key, str3);
                  itemToAdd.ItemName = "ppt" + path.Remove(0, 2);
                  this.AddItemToZipStream(itemToAdd);
                  string lower = extension.Remove(0, 1).ToLower();
                  this._presentation.AddDefaultContentType(lower, "image/" + lower);
                }
                else
                  path = imagePaths[imagePath];
                pictureFill.SetImagePath(path);
              }
            }
          }
        }
        else if (shape.SlideItemType == SlideItemType.OleObject)
        {
          OleObject oleObject = (OleObject) shape;
          if (oleObject.OlePicture != null)
          {
            Picture olePicture = oleObject.OlePicture;
            string imagePath = olePicture.GetImagePath();
            if (imagePath != null && archive["ppt" + imagePath.Remove(0, 2)] != null)
            {
              ZipArchiveItem itemToAdd = archive["ppt" + imagePath.Remove(0, 2)].Clone();
              string path;
              if (!imagePaths.ContainsKey(imagePath))
              {
                string extension = Path.GetExtension(imagePath);
                Dictionary<string, string> dictionary = imagePaths;
                string key = imagePath;
                string str4 = (++this._presentation.ImageCount).ToString();
                string str5 = extension;
                string str6;
                path = str6 = $"../media/image{str4}{str5}";
                dictionary.Add(key, str6);
                itemToAdd.ItemName = "ppt" + path.Remove(0, 2);
                this.AddItemToZipStream(itemToAdd);
                string lower = extension.Remove(0, 1).ToLower();
                this._presentation.AddDefaultContentType(lower, "image/" + lower);
              }
              else
                path = imagePaths[imagePath];
              olePicture.SetImagePath(path);
            }
            else
              continue;
          }
        }
        this.AddAlternateContentToArchive((Shape) shape, imagePaths, archive);
        flag = true;
      }
      if ((Shape) shape is Picture)
      {
        Picture picture = (Picture) shape;
        if (picture.PreservedElements.ContainsKey("AlternateContent") && !flag)
          this.AddAlternateContentToArchive((Shape) shape, imagePaths, archive);
        string imagePath = picture.GetImagePath();
        if (imagePath != null && archive["ppt" + imagePath.Remove(0, 2)] != null)
        {
          ZipArchiveItem itemToAdd = archive["ppt" + imagePath.Remove(0, 2)].Clone();
          string path;
          if (!imagePaths.ContainsKey(imagePath))
          {
            string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(imagePath);
            Dictionary<string, string> dictionary = imagePaths;
            string key = imagePath;
            string str7 = (++this._presentation.ImageCount).ToString();
            string str8 = extension;
            string str9;
            path = str9 = $"../media/image{str7}{str8}";
            dictionary.Add(key, str9);
            itemToAdd.ItemName = "ppt" + path.Remove(0, 2);
            if (this._archive[itemToAdd.ItemName] == null)
              this.AddItemToZipStream(itemToAdd);
            string lower = extension.Remove(0, 1).ToLower();
            this._presentation.AddDefaultContentType(lower, "image/" + lower);
            string videoPath = (shape as Shape).GetVideoPath();
            if (videoPath != null && archive["ppt" + videoPath.Remove(0, 2)] != null)
              this.SetVideoContentToArchive(archive, videoPath);
          }
          else
            path = imagePaths[imagePath];
          picture.SetImagePath(path);
        }
        else
          continue;
      }
      else if ((Shape) shape is GroupShape)
        this.SetExternalImageToArchive(((GroupShape) shape).Shapes, imagePaths, archive);
      if (shape.Fill.FillType == FillType.Picture)
      {
        TextureFill pictureFill = (TextureFill) shape.Fill.PictureFill;
        string imagePath = pictureFill.GetImagePath();
        if (imagePath != null && archive["ppt" + imagePath.Remove(0, 2)] != null)
        {
          string path;
          if (!imagePaths.ContainsKey(imagePath))
          {
            string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(imagePath);
            Dictionary<string, string> dictionary = imagePaths;
            string key = imagePath;
            string str10 = (++this._presentation.ImageCount).ToString();
            string str11 = extension;
            string str12;
            path = str12 = $"../media/image{str10}{str11}";
            dictionary.Add(key, str12);
            ZipArchiveItem zipArchiveItem = archive["ppt" + path.Remove(0, 2)];
            if (zipArchiveItem != null)
            {
              ZipArchiveItem itemToAdd = zipArchiveItem.Clone();
              itemToAdd.ItemName = "ppt" + path.Remove(0, 2);
              this.AddItemToZipStream(itemToAdd);
            }
            string lower = extension.Remove(0, 1).ToLower();
            this._presentation.AddDefaultContentType(lower, "image/" + lower);
          }
          else
            path = imagePaths[imagePath];
          pictureFill.SetImagePath(path);
        }
      }
    }
  }

  private void SetVideoContentToArchive(ZipArchive archive, string videoPath)
  {
    ZipArchiveItem itemToAdd = archive["ppt" + videoPath.Remove(0, 2)].Clone();
    itemToAdd.ItemName = "ppt" + videoPath.Remove(0, 2);
    if (this._archive[itemToAdd.ItemName] != null)
      return;
    this.AddItemToZipStream(itemToAdd);
    string lower = Syncfusion.Presentation.Drawing.Helper.GetExtension(videoPath).Remove(0, 1).ToLower();
    this._presentation.AddDefaultContentType(lower, "video/" + lower);
  }

  private void GetBulletPictureFromDefaultTextStyle(
    BaseSlide baseSlide,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    if (!(baseSlide is MasterSlide))
      return;
    MasterSlide masterSlide = (MasterSlide) baseSlide;
    this.PreserveBulletPictureFromStyleList(masterSlide.BodyStyle.StyleList, imagePaths, archive);
    this.PreserveBulletPictureFromStyleList(masterSlide.OtherStyle.StyleList, imagePaths, archive);
    this.PreserveBulletPictureFromStyleList(masterSlide.TitleStyle.StyleList, imagePaths, archive);
  }

  private void PreserveBulletFromSource(
    Syncfusion.Presentation.RichText.TextBody textBody,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    this.PreserveBulletPictureFromParagraphs(textBody.Paragraphs, imagePaths, archive);
    this.PreserveBulletPictureFromStyleList(textBody.StyleList, imagePaths, archive);
  }

  private void PreserveBulletPictureFromStyleList(
    Dictionary<string, Paragraph> styleList,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    foreach (KeyValuePair<string, Paragraph> style in styleList)
      this.PreserveBulletPictureFromParagraph(style.Value, imagePaths, archive);
  }

  private void PreserveBulletPictureFromParagraphs(
    IParagraphs paragraphs,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    foreach (Paragraph paragraph in (IEnumerable<IParagraph>) paragraphs)
      this.PreserveBulletPictureFromParagraph(paragraph, imagePaths, archive);
  }

  private void PreserveBulletPictureFromParagraph(
    Paragraph paragraph,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    if (paragraph.ListFormat.Type != ListType.Picture)
      return;
    ListFormat listFormat = paragraph.ListFormat as ListFormat;
    string picturePath = listFormat.PicturePath;
    if (picturePath == null || archive["ppt" + picturePath.Remove(0, 2)] == null)
      return;
    ZipArchiveItem itemToAdd = archive["ppt" + picturePath.Remove(0, 2)].Clone();
    string str1;
    if (!imagePaths.ContainsKey(picturePath))
    {
      string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(picturePath);
      Dictionary<string, string> dictionary = imagePaths;
      string key = picturePath;
      string str2 = (++this._presentation.ImageCount).ToString();
      string str3 = extension;
      string str4;
      str1 = str4 = $"../media/image{str2}{str3}";
      dictionary.Add(key, str4);
      itemToAdd.ItemName = "ppt" + str1.Remove(0, 2);
      this.AddItemToZipStream(itemToAdd);
      string lower = extension.Remove(0, 1).ToLower();
      this._presentation.AddDefaultContentType(lower, "image/" + lower);
    }
    else
      str1 = imagePaths[picturePath];
    listFormat.PicturePath = str1;
  }

  private void AddAlternateContentToArchive(
    Shape alternateContentShape,
    Dictionary<string, string> imagePaths,
    ZipArchive archive)
  {
    foreach (Relation relation in alternateContentShape.BaseSlide.TopRelation.GetRelationList())
    {
      if (relation.Target.Contains("media"))
      {
        string extension = Syncfusion.Presentation.Drawing.Helper.GetExtension(relation.Target);
        ZipArchiveItem zipArchiveItem = archive["ppt" + relation.Target.Remove(0, 2)];
        if (zipArchiveItem == null)
          break;
        string str = $"/media/image{(object) ++this._presentation.ImageCount}{extension}";
        MemoryStream output = new MemoryStream();
        Picture.CopyStream(zipArchiveItem.DataStream, (Stream) output);
        if (!imagePaths.ContainsKey(relation.Target))
        {
          imagePaths.Add(relation.Target, relation.Target = ".." + str);
          this.AddItemToZipStream("ppt" + str, (Stream) output);
        }
        else
          relation.Target = imagePaths[relation.Target];
        string lower = extension.Remove(0, 1).ToLower();
        this._presentation.AddDefaultContentType(lower, "image/" + lower);
      }
    }
  }

  internal int GetUnParsedThemeCount()
  {
    int parsedThemeCount = 0;
    foreach (ZipArchiveItem zipArchiveItem in this._archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains("theme"))
        ++parsedThemeCount;
    }
    return parsedThemeCount;
  }

  internal Dictionary<string, string> MediaList => this._mediaItems;

  internal void AddToZipArchive(string target, Syncfusion.Presentation.Presentation presentation)
  {
    string str = target.Split('/')[2];
    foreach (ZipArchiveItem zipArchiveItem in presentation.DataHolder._archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains("path"))
        this._archive.AddItem(zipArchiveItem);
    }
  }

  internal void AddSlideContentToArchive(Slide slideClone, Syncfusion.Presentation.Presentation presentation)
  {
    Dictionary<string, string> imagePaths = new Dictionary<string, string>();
    ZipArchive archive = presentation.DataHolder._archive;
    this.AddBackgroundImageToArchive((Background) slideClone.Background, imagePaths, archive);
    this.SetExternalImageToArchive(slideClone.Shapes, imagePaths, archive);
    foreach (Relation relation in slideClone.TopRelation.GetRelationList())
      this.AddOtherMergableItemToArchive(relation, archive);
  }

  internal void ParseSmartArt(DataModel dataModel, string path)
  {
    path = "ppt" + path.Remove(0, 2);
    ZipArchiveItem zipArchiveItem = this._archive[path];
    if (zipArchiveItem == null)
      return;
    this.ParseDataModelRelation(dataModel, path.Remove(0, 4));
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseDataModel(reader, dataModel);
    this._dictItemsToRemove.Add(path);
  }

  internal void ParseSmartArt(SmartArtLayout layoutDefinition, string path)
  {
    path = "ppt" + path.Remove(0, 2);
    ZipArchiveItem zipArchiveItem = this._archive[path];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseLayoutDefinition(reader, layoutDefinition);
  }

  private void ParseDataModelRelation(DataModel dataModel, string path)
  {
    string[] strArray = path.Split('/');
    string itemName = $"ppt/{strArray[0]}/_rels/{strArray[1]}.rels";
    bool flag = strArray[1].Contains("drawing");
    ZipArchiveItem zipArchiveItem = this._archive[itemName];
    if (zipArchiveItem == null)
      return;
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
    {
      RelationCollection relationCollection = Parser.ParseRelationCollection(reader);
      if (flag)
        dataModel.DrawingRelation = relationCollection;
      else
        dataModel.TopRelation = relationCollection;
    }
    if (flag)
      return;
    this._dictItemsToRemove.Add(itemName);
  }

  internal void ParseSmartArtDrawing(DataModel dataModel, string path)
  {
    if (path == null)
      return;
    path = "ppt" + path.Remove(0, 2);
    ZipArchiveItem zipArchiveItem = this._archive[path];
    if (zipArchiveItem == null)
      return;
    this.ParseDataModelRelation(dataModel, path.Remove(0, 4));
    using (XmlReader reader = UtilityMethods.CreateReader(zipArchiveItem.DataStream))
      Parser.ParseSmartArtDrawing(reader, dataModel);
  }

  internal void WriteSmartArt(SmartArt smartArt)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlWriter = XmlWriter.Create((Stream) output))
      Serializator.SerializeDataModel(xmlWriter, smartArt.DataModel);
    if (smartArt.DataModel.RelationId == null)
      (smartArt.Nodes as SmartArtNodes).AddSmartArtRelation();
    string str = smartArt.BaseSlide.TopRelation.GetItemPathByRelation(smartArt.DataModel.RelationId).Remove(0, 2);
    this.AddItemToZipStream("ppt" + str, (Stream) output);
    this.WriteTopRelation(str.Remove(0, 1), smartArt.DataModel.TopRelation);
  }

  internal void ParseSmartArtDrawing(SmartArt smartArt)
  {
    if (!this._presentation.Created)
      return;
    Stream streamFromResource = ResourceManager.GetStreamFromResource("SmartArtResources");
    using (ZipArchive zipArchive = new ZipArchive())
    {
      zipArchive.Open(streamFromResource, false);
      using (XmlReader reader = UtilityMethods.CreateReader(zipArchive[smartArt.Layout.ToString() + ".xml"].DataStream))
        Parser.ParseSmartArtDrawing(reader, smartArt.DataModel);
    }
  }
}
