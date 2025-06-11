// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.AddInFunctionsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class AddInFunctionsCollection : 
  CollectionBaseEx<AddInFunctionImpl>,
  IAddInFunctions,
  IParentApplication
{
  private const string DEF_FILE_NAME_START = "\u0001";
  public const int DEF_LOCAL_BOOK_INDEX = -1;
  private Dictionary<string, int> m_hashFileNames = new Dictionary<string, int>();
  private WorkbookImpl m_book;
  private ExternWorkbookImpl m_unknownBook;

  public AddInFunctionsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_book.ExternWorkbooks.Inserted += new CollectionBaseEx<ExternWorkbookImpl>.CollectionChange(this.ExternWorkbooks_Inserted);
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("m_book");
  }

  public IAddInFunction this[int index] => (IAddInFunction) this.List[index];

  public int Add(string fileName, string functionName)
  {
    switch (functionName)
    {
      case null:
        throw new ArgumentNullException(nameof (functionName));
      case "":
        throw new ArgumentException("functionName - string cannot be empty");
      default:
        if (fileName != null)
          fileName = Path.GetFullPath(fileName);
        ExternBookCollection externWorkbooks = this.m_book.ExternWorkbooks;
        int num = !this.Contains(fileName) ? externWorkbooks.Add(fileName, true) : (fileName != null ? this.m_hashFileNames[fileName] : this.m_unknownBook.Index);
        ExternWorkbookImpl externWorkbookImpl = externWorkbooks[num];
        if (fileName == null)
          externWorkbookImpl.IsAddInFunctions = true;
        if (externWorkbookImpl.ExternNames.Contains(functionName))
          throw new ApplicationException("Already contains same function");
        int iNameIndex = externWorkbookImpl.ExternNames.Add(functionName, true);
        this.Add(new AddInFunctionImpl(this.Application, (object) this, num, iNameIndex));
        return this.Count - 1;
    }
  }

  public int Add(string strFunctionName)
  {
    switch (strFunctionName)
    {
      case null:
        throw new ArgumentNullException(nameof (strFunctionName));
      case "":
        throw new ArgumentException("strFunctionName - string cannot be empty");
      default:
        this.Add(new AddInFunctionImpl(this.Application, (object) this, -1, this.m_book.InnerNamesColection.AddFunctions(strFunctionName)));
        return this.Count - 1;
    }
  }

  public void Add(int iExternBookIndex, int iNameIndex)
  {
    this.Add(new AddInFunctionImpl(this.Application, (object) this, iExternBookIndex, iNameIndex));
  }

  public new void RemoveAt(int index)
  {
    AddInFunctionImpl addInFunctionImpl = index >= 0 && index <= this.Count - 1 ? this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1");
    this.m_book.ExternWorkbooks[addInFunctionImpl.BookIndex].ExternNames.RemoveAt(addInFunctionImpl.NameIndex);
  }

  public bool Contains(string strBookName)
  {
    return strBookName == null ? this.m_unknownBook != null : this.m_hashFileNames.ContainsKey(strBookName);
  }

  public void CopyFrom(AddInFunctionsCollection addinFunctions)
  {
    if (addinFunctions.m_unknownBook != null)
      this.m_unknownBook = this.m_book.ExternWorkbooks[addinFunctions.m_unknownBook.Index];
    System.Collections.Generic.List<AddInFunctionImpl> innerList1 = addinFunctions.InnerList;
    System.Collections.Generic.List<AddInFunctionImpl> innerList2 = this.InnerList;
    int index = 0;
    for (int count = innerList1.Count; index < count; ++index)
    {
      AddInFunctionImpl addInFunctionImpl = (AddInFunctionImpl) innerList1[index].Clone((object) this);
      innerList2.Add(addInFunctionImpl);
    }
  }

  private void ExternWorkbooks_Inserted(
    object sender,
    CollectionChangeEventArgs<ExternWorkbookImpl> args)
  {
    ExternWorkbookImpl externWorkbookImpl = args.Value;
    if (externWorkbookImpl.IsInternalReference || !externWorkbookImpl.IsAddInFunctions)
      return;
    string url = externWorkbookImpl.URL;
    if (url == null)
    {
      this.m_unknownBook = externWorkbookImpl;
    }
    else
    {
      if (!(url != " "))
        return;
      this.m_hashFileNames.Add(url, externWorkbookImpl.Index);
    }
  }
}
