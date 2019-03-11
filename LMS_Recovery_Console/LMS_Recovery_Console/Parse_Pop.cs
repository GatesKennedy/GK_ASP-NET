using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_Recovery_Console
{
    public class Parse_Pop : Read
    {
        //public int MyProperty { get; set; }

        //====  PROPERTIES  ====
        public List<string>     TypeList { get; set; }
        public List<string>     FieldsList { get; set; }

        //  Report Properties
        public int              LineCount { get; set; }
        public int              PageCount { get; set; }
        public int              PageCount2 { get; set; }

        public List<string>     RecordList1 { get; set; }
        public List<string>     RecordList2 { get; set; }
        public List<string>     RecordList3 { get; set; }  

        //====  CONSTRUCTORS  ====
        public Parse_Pop()
        {
            LinesList = new List<string>();
            PageBreaks = new Dictionary<int, string>();
            FullFlagList = new List<string>();
            IsolatedList = new List<string>();
            TestsList = new List<string>();
            FormatList = new List<string>();

            PageList = new List<List<string>>();
            FormattedList = new List<List<string>>();
            ModuleList = new List<List<List<string>>>();

            TypeList = new List<string>();
            FieldsList = new List<string>();

            RecordList1 = new List<string>();
            RecordList2 = new List<string>();
            RecordList3 = new List<string>();
        }

        //==== READ Methods ====

        //  Read: InputDoc
        //  "Initialize Object Properties"
        public void Read_InputDoc(string _inputFilePath)
        {
            this.Txt_FilePath = _inputFilePath;

            //  Populate 'textLines' Array from .txt document
            this.TextArray = System.IO.File.ReadAllLines(this.Txt_FilePath);
            this.LinesList = TextArray.ToList();
            this.LineCount = LinesList.Count();

            Console.WriteLine("LineCount: " + LineCount);
        }

        //  Read: Clean
        //  "Isolate Content"
        public void Read_Clean()
        {
            List<string> tempList = LinesList;

            TrimList(tempList, out List<string> trimmedList);
                Display.MetricsToConsole(trimmedList, "trimList");
             
            RemoveEmptyLines(trimmedList, out List<string> ReducedList);
                Display.MetricsToConsole(ReducedList, "reduceList");
                Display.ExportCollection(ReducedList, "Clean_ReducedList");

            TrimHtmlTags(ReducedList, out List<string> taglessList);
                Display.MetricsToConsole(taglessList, "taglessList");
                Display.ExportCollection(taglessList, "Clean_TaglessList");

            //HandleFlags(taglessList, out List<string> flaglessList);
            //Display.ToConsole(flaglessList);
            //Display.ExportCollection(flaglessList, "Clean_FlaglessList");

            RemoveEmptyLines(taglessList, out List<string> ReducedList2);
            TrimList(ReducedList2, out List<string> resultList);

            //  For OSD Course
            

            LinesList = resultList;
            IsolatedList = resultList;
            this.FindPageBreaksOSD();

            TrimHtmlTags();
        }
        public void Read_Clean(Dictionary<int,string> _listToModify)
        {
            List<string> tempList = _listToModify.Values.ToList();
            Display.ToConsole(tempList);

            this.TrimList(tempList);
            this.RemoveEmptyLines(tempList);
            this.TrimHtmlTags(tempList);

            var tempDict = tempList.Select((value, key) => new { value, key }).ToDictionary(x => x.key, x => x.value);
            _listToModify = tempDict;
        }

        //  Flag Page Breaks

        public void FlagPageBreaks()
        {
            foreach (var line in this.PageBreaks)
            {
                
                this.LinesList[line.Key] = "$$$";
            }
        }

        // Flag Module Breaks
        public void FlagModuleBreaks()
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            for (int i = 0; i < LinesList.Count(); i++)
            {
                string line = LinesList[i];
                if (LinesList[i].ToUpper().Contains("SUBMODULE"))
                {
                    LinesList[i] = LinesList[i].Replace("SUBMODULE", "Submodule");
                    LinesList[i] = textInfo.ToTitleCase(LinesList[i]);
                    LinesList[i] = LinesList[i].Insert(0, "###");
                }
                else if (LinesList[i].ToUpper().Contains("MODULE"))
                {
                    LinesList[i] = LinesList[i].Replace("MODULE", "Module");
                    LinesList[i] = textInfo.ToTitleCase(LinesList[i]);
                    LinesList[i] = LinesList[i].Insert(0, "***");
                }
            }
        }


        //  Mod Line to TitleCasing
        public void ToTitleCase()
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            for (int i = 0; i < LinesList.Count(); i++)
            {
                if (LinesList[i].Contains("MODULE") || LinesList[i].ToUpper().Contains("KNOWLEDGE CHECK"))
                {
                    LinesList[i] = textInfo.ToTitleCase(LinesList[i]);
                }
            }
        }

        //  Object Metrics to Console
        public void MetricsToConsole()
        {
            List<int> tempList = new List<int>
            {
                this.LinesList.Count(),
                this.PageList.Count(),
                this.LinesList.Count(),
                this.PageBreaks.Count()
            };
            Display.ToConsole(tempList);
        }
        //================================== 
        //  Routine: FULL READ
        //==================================
        public void FullRead(Parse_Pop _readObject, out List<string> reducedLinesList)
        {
            //  initialize linesArray
            PopulateLines(_readObject, out List<string> linesList);
            //  Remove/Reduce Empty Lines
            reducedLinesList = linesList;
        }

        //================================
        //  Routine: HALF READ
        //================================
        public void HalfRead(Parse_Pop _readObject)
        {

            PopulateLines(_readObject, out List<string> _linesList);
            FlagEmptyLines(_readObject, _linesList, out List<string> _outList);
            List<string> PageBreakPRE = new List<string>();
            PageBreakPRE = prependNumberList(_outList);
            Display.ExportCollection(PageBreakPRE, "PageBreakPRE");
        }

        //================================
        //  Routine: COMPARE Page Breaks
        //================================
       
        //      in: _trustedList is ONLY key string to be searched for
        //      in: _untrustedList can be 'dirty'
        //public static void ComparePageBreaks(List<string> _trustedSource, List<string> _untrustedSource, out List<string> _PageBreakList)
        //{
        //    RemoveEmptyLines(_trustedSource, out List<string> _ReducedTrusted);
        //    RemoveEmptyLines(_untrustedSource, out List<string> _ReducedUntrusted);

        //    ToDictIndex(RemoveEmptyLines(_trustedSource), out Dictionary<int, string> trustedDict);
        //    ToDictIndex(RemoveEmptyLines(_untrustedSource), out Dictionary<int, string> untrustedDict);

        //    Display.exportCollection(trustedDict, "CompDictTrust");
        //    Display.exportCollection(untrustedDict, "CompDictUntrusted");

        //    FindKindaPgBrk_txt(_trustedSource, out List<string> _trustedList);
        //    FindKindaPgBrk_html(_untrustedSource, out List<string> _untrustedList);

        //    FindKindaPgBrk_txt(trustedDict, out Dictionary<int, string> _KindaTrustDict);
        //    FindKindaPgBrk_html(untrustedDict, out Dictionary<int, string> _KindaSketchDict);

        //    Display.exportCollection(_KindaTrustDict, "KindaTrustDict");
        //    Display.exportCollection(_KindaSketchDict, "KindaSketchDict");

        //    Display.CompareCollection(_KindaTrustDict, _KindaSketchDict);

        //    //Display.exportCollection(prependNumberList(_trustedSource), "CompTrustSrc");
        //    //Display.exportCollection(prependNumberList(_untrustedSource), "CompUntrustSrc");
        //    //Display.exportCollection(_trustedList, "CompTrusted");
        //    //Display.exportCollection(_untrustedList, "CompUntrusted");

        //    List<string> tempList = new List<string>();
        //    _trustedList = cleanKeyList(_trustedList);
        //    int countTrust = 0;
        //    int countUntrust = 0;
        //    for (int i = 0; i < _trustedList.Count(); i++)
        //    {
        //        int countError = 0;
        //        if (i <= countTrust) continue;
        //        for (int j = 0; j < _untrustedList.Count(); j++)
        //        {
        //            if (j < countUntrust) continue;
        //            else if (_untrustedList[j].ToLower().Contains(_trustedList[i].ToLower()))
        //            {
        //                countTrust = i;
        //                countUntrust = j;
        //                i++;

        //                tempList.Add(_untrustedList[j]);
        //            }
        //            else if (countError > 100) break;
        //            countError++;
        //        }
        //    }
        //    _PageBreakList = prependNumberList(tempList);
        //}

        //public static void ComparePageBreaks(List<string> _trustedSource, List<string> _untrustedSource)
        //{
        //    // Clean
        //    RemoveEmptyLines(_trustedSource, out List<string> _ReducedTrusted);
        //    RemoveEmptyLines(_untrustedSource, out List<string> _ReducedUntrusted);

        //    // To Dictionary
        //    ToDictIndex(RemoveEmptyLines(_trustedSource), out Dictionary<int, string> trustedDict);
        //    ToDictIndex(RemoveEmptyLines(_untrustedSource), out Dictionary<int, string> untrustedDict);

        //    // Get PageBreaks
        //    FindKindaPgBrk_txt(trustedDict, out Dictionary<int, string> _KindaTrustDict);
        //    FindKindaPgBrk_html(untrustedDict, out Dictionary<int, string> _KindaSketchDict);

        //    Display.exportCollection(_KindaTrustDict, "KindaTrustDict");
        //    Display.exportCollection(_KindaSketchDict, "KindaSketchDict");
        //    Display.CompareCollection(_KindaTrustDict, _KindaSketchDict);

        //    // Set PageBreaks


        //}

        //public static void CompareTxtDocs(Parse_Pop _txtObjNum, Parse_Pop _txtObjBull, out Dictionary<int,string> _PageBreaks)
        //{
        //    Dictionary<int, string> tempDict = new Dictionary<int, string>();
        //    if (_txtObjBull.LineCount == _txtObjNum.LineCount)
        //    {
        //        for (int i = 0; i < _txtObjNum.LinesList.Count(); i++)
        //        {
        //            if (_txtObjBull.LinesList[i] != _txtObjNum.LinesList[i])
        //            {
        //                tempDict.Add(i, _txtObjBull.LinesList[i]);
        //            }
        //        }
        //    }
        //    else Console.WriteLine("ERROR: Lines of Txt Docs are NOT the same.");
        //    _PageBreaks = tempDict;

        //    Console.WriteLine("PageBreaks: " + tempDict.Count());
        //}


        //List<string> tempList = new List<string>();
        //_trustedList = cleanKeyList(_trustedList);
        //int countTrust = 0;
        //int countUntrust = 0;
        //for (int i = 0; i < _trustedList.Count(); i++)
        //{
        //    int countError = 0;
        //    if (i <= countTrust) continue;
        //    for (int j = 0; j < _untrustedList.Count(); j++)
        //    {
        //        if (j < countUntrust) continue;
        //        else if (_untrustedList[j].ToLower().Contains(_trustedList[i].ToLower()))
        //        {
        //            countTrust = i;
        //            countUntrust = j;
        //            i++;

        //            tempList.Add(_untrustedList[j]);
        //        }
        //        else if (countError > 100) break;
        //        countError++;
        //    }
        //}
        //_PageBreakList = prependNumberList(tempList);


        //=============================================================
        //  Sort > ..SeperatedPages/..CompileFields/..SortCleanUp()
        //=============================================================  

        //public void Completely(Parse_Pop _parseObject)
        //{
        //    SortSeparatePages(_parseObject);
        //    SortCompileFields(_parseObject);
        //    SortCleanUp(_parseObject);
        //}

    }
}

