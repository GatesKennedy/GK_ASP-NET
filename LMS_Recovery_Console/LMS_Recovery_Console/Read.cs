using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMS_Recovery_Console
{
    public class Read : Sort
    {
        //==========================================
        //  PROPERTIES  
        //==========================================

        //  Input Document
        public string Txt_FilePath { get; set; }
        public string[] TextArray { get; set; }

        //  Sub Properties
        public List<string> LinesList { get; set; }
        public Dictionary<int, string> PageBreaks { get; set; }
        public List<string> FullFlagList { get; set; }
        public List<string> FormatList { get; set; }

        //  Method Properties
        List<string> TagList = new List<string>()
            {
                "<li>","</li>","<ul>","</ul>","<ol>","</ol>","*","<br>","</br>","<strong>","</strong>"//,"<p>","</p>"
            };

        List<string> FlagList = new List<string>()
        {
            "^G^","$$$","Learning","^^^","***","###"
        };

        //==========================================
        //  PROTECTED SUB-METHODS
        //==========================================

        //___________________
        //=====  CLEAN  =====

        //  FXN: Trim List Elements
        protected void TrimList()
        {
            List<string> tempList = new List<string>();
            foreach (var line in this.LinesList)
            {
                tempList.Add(line.Trim());
            }
            this.LinesList = tempList;
        }
        protected void TrimList(List<string> _readList, out List<string> ResultList)
        {
            List<string> _sortedList = new List<string>();

            foreach (var line in _readList)
            {
                //Display.RouteSiphon(line);
                _sortedList.Add(line.Trim());
            }
            ResultList = _sortedList;
        }
        static void TrimList2(List<string> _readList, out List<string> ResultList)
        {
            List<string> _sortedList = new List<string>();

            foreach (var line in _readList)
            {
                //Display.RouteSiphon(line);
                _sortedList.Add(line.Trim());
            }
            ResultList = _sortedList;
        }
        protected List<string> TrimList(List<string> _readList)
        {
            List<string> _sortedList = new List<string>();

            foreach (var line in _readList)
            {
                _sortedList.Add(line.Trim());
            }
            return _sortedList;
        }

        //  FXN: Remove Empty Lines
        public void RemoveEmptyLines()
        {
            List<string> tempList = new List<string>();

            foreach (var line in LinesList)
            {
                if (line.Trim() == "") continue;
                else if (line.Trim() == "<p>") continue;
                else if (line.Trim() == "</p>") continue;
                else if (line.Trim() == "<ol>") continue;
                else if (line.Trim() == "</li> </ol>") continue;
                else if (line.Trim() == "</li><ol>") continue;
                else if (line.Trim() == "</li><ul>") continue;
                else if (line.Trim() == "</p><ul>") continue;
                else if (line.Trim() == "</p><ol>") continue;
                else tempList.Add(line);
            }
            this.LinesList = tempList;
        }
        public List<string> ReduceEmptyLines(List<string> _listToModify)
        {
            List<string> tempList = _listToModify;

            foreach (var item in tempList)
            {
                if (item.Trim() == "<p>") continue;
                else tempList.Add(item);
            }
            return tempList;
        }
        public List<string> RemoveEmptyLines(List<string> _listToModify)
        {
            List<string> tempList = _listToModify ;

            foreach (var line in tempList)
            {
                if (line.Trim() == "") continue;
                else if (line.Trim() == "<p>") continue;
                else if (line.Trim() == "</p>") continue;
                else if (line.Trim() == "<ol>") continue;
                else if (line.Trim() == "</li> </ol>") continue;
                else if (line.Trim() == "</li><ol>") continue;
                else if (line.Trim() == "</li><ul>") continue;
                else if (line.Trim() == "</p><ul>") continue;
                else if (line.Trim() == "</p><ol>") continue;
                else tempList.Add(line);
            }

            return tempList;
        }
        public List<string> RemoveEmptyLines(List<string> _readList, out List<string> _OutList)
        {
            List<string> tempList = new List<string>();
            foreach (var line in _readList)
            {
                if (line.Trim() == "") continue;
                else if (line.Trim() == "~~") continue;
                else if (line.Trim() == "<p>") continue;
                else if (line.Trim() == "</p>") continue;
                else if (line.Trim() == "<ol>") continue;
                else if (line.Trim() == "</li> </ol>") continue;
                else if (line.Trim() == "</li><ol>") continue;
                else if (line.Trim() == "</li><ul>") continue;
                else if (line.Trim() == "</p><ul>") continue;
                else if (line.Trim() == "</p><ol>") continue;
                else tempList.Add(line);
            }
            _OutList = tempList;
            return tempList;
        }
        
        //  FXN: Removes Sub-Strings from 'textLine' SuperString
        protected string TrimTag(string textLine, string tagHtmlUp, string tagHtmlDwn)
        {
            textLine = textLine.Replace(tagHtmlUp, "");
            textLine = textLine.Replace(tagHtmlDwn, "");
            return textLine;
        }
        protected string TrimTag(string textLine, string tag)
        {
            textLine = textLine.Replace(tag, "");
            return textLine;
        }

        //  Remove HTML Tags << string
        public void TrimHtmlTags()
        {
            List<string> tempList = LinesList;

            for (int i = 0; i < tempList.Count(); i++)
            {
                foreach (var tag in TagList)
                {
                    if (tempList[i].Contains(tag))
                        tempList[i] = TrimTag(tempList[i],tag).ToString().Replace("\\\"","");
                }
                string htmlBS = @"\";
                htmlBS = Regex.Escape(htmlBS);
                string htmlLine = tempList[i];
                string htmlClean = Regex.Replace(htmlLine, htmlBS, "");
                tempList[i] = htmlClean;
            }
            
            this.LinesList = tempList.ToList();
        }
        public void TrimHtmlTags(List<string> _readList, out List<string> ResultList)
        {
            List<string> tempList = _readList;
            for (int i = 0; i < tempList.Count(); i++)
            {
                foreach (var tag in TagList)
                {
                    if (tempList[i].Contains(tag))
                        tempList[i] = TrimTag(tempList[i], tag).Trim().ToString().Replace("\\\"","");
                }
                string htmlBS = @"\";
                htmlBS = Regex.Escape(htmlBS);
                string htmlLine = tempList[i];
                string htmlClean = Regex.Replace(htmlLine, htmlBS, "");
                tempList[i] = htmlClean;
            }
            ResultList = tempList.ToList();
        }
        public string TrimHtmlTags(string _readString)
        {
            foreach (var tag in TagList)
            {
                if (_readString.Contains(tag))
                    _readString = TrimTag(_readString, tag);
            }
            return _readString;
        }
        public List<string> TrimHtmlTags(List<string> _readList)
        {
            foreach (var line in _readList) TrimHtmlTags(line);
            return _readList;
        }

        //  Trim Flags (FormattedList)
        public void TrimFlags()
        {
            foreach (var page in FormattedList)
            {
                foreach (var line in page)
                {
                    string tempString = line;
                    foreach (var flag in FlagList)
                    {
                        tempString = tempString.Replace(flag, "");
                    }
                    FormatList.Add(tempString);
                }
            }
        }
        //  Trim Flags (Input: List<string>)
        public void TrimFlagsIsoList()
        {
            List<string> tempList = new List<string>();
            foreach (var line in IsolatedList)
            {
                string tempString = line;
                foreach (var flag in FlagList)
                {
                    tempString = tempString.Replace(flag, "");
                }
                tempList.Add(tempString);
            }
            IsolatedList = tempList;
        }
        //  Trim Flags (Input: List<List<string>>)
        public void TrimFlags(List<List<string>> _readPages)
        {
            foreach (var page in _readPages)
            {
                foreach (var line in page)
                {
                    string tempString = line;
                    foreach (var flag in FlagList)
                    {
                        tempString = tempString.Replace(flag, "");
                    }
                    FormatList.Add(tempString);
                }
            }
        }

        //  Handle Flags
        public void HandleFlags(List<string> _ReadList, out List<string> FlaglessList)
        {
            List<string> TempList = new List<string>();
            int cntLines = 0;
            foreach (var line in _ReadList)
            {
                cntLines++;
                StringBuilder tempStrBld = new StringBuilder();

                if (line.StartsWith("$$$"))
                {
                    TempList.Add("$$$");
                }
                else if (line.Contains("^G^") || line.Contains("SUBMODULE") || line.Contains("ESTIMATED COURSE"))
                {
                    TempList.Add(CleanGreen(line, _ReadList, cntLines));
                }
                else if (line.ToLower().Contains("hours of study")) continue;
                else TempList.Add(line);
            }
            FlaglessList = TempList;
        }

        public string CleanGreen(string _readString, List<string> _ReadList, int _cntLines)
        {
            string _line = _readString.ToLower();
            int j = _cntLines;
            StringBuilder tempStrBld = new StringBuilder();
            _readString = _readString.Replace("<p>", "");
            _readString = _readString.Replace("</p>", "");
            _readString = _readString.Replace("^G^", "");
            _readString = _readString.Replace(".", "");
            _readString = _readString.Trim();

            if (_readString.Contains("MODULE"))
            {
                string[] arrayWords = _readString.Split();
                int cntWords = arrayWords.Length;

                if (cntWords < 3)
                {
                    return arrayWords[0] + "~~" + arrayWords[1] + "~~na";
                }
                else
                {
                    tempStrBld.Append(arrayWords[0] + "~~" + arrayWords[1] + "~~");
                    for (int i = 2; i < arrayWords.Length; i++)
                    {
                        tempStrBld.Append(arrayWords[i] + " ");
                    }
                    return tempStrBld.ToString();
                }
            }
            else if (_line.Contains("learning objectives"))
            {
                tempStrBld.Append("OBJECTIVES~~Learning Objectives");
                
                for (int i = _cntLines; i < _cntLines+5; i++)
                {
                    if (_ReadList[i].StartsWith("~~"))
                    {
                        tempStrBld.Append(_ReadList[i].Replace("^G^ ","").Replace("##EndBull",""));
                    }
                    else if (_ReadList[i].Contains("?")) break;
                }

            }
            else if (_line.Contains("estimated course"))
            {
                string strPrev = _ReadList[j - 1];
                string strNow = _ReadList[j];
                string strNext = _ReadList[j + 1];
                tempStrBld.Append("LESSON~~Estimated Course Completion Time");
                if (_ReadList[j].Contains("hours"))
                {
                    _line = _ReadList[j].Replace("<p>", "~~").Replace("</p>", "");
                    tempStrBld.Append(_line);
                }
                else if (_ReadList[j+1].Contains("hours"))
                {
                    _line = _ReadList[j + 1].Replace("<p>", "~~").Replace("</p>", "");
                    _ReadList[j+1] = "";
                }
            }
            return tempStrBld.ToString();
        }

        //  FXN: Clean Trusted List 
        protected List<string> cleanKeyList(List<string> _readList)
        {
            List<string> tempList = new List<string>();
            foreach (var key in _readList)
            {
                tempList.Add(key.Remove(0, 2).Trim());
            }
            return tempList;
        }

        //___________________
        //=====  FIND  =====

        //  GET: Populate Lines
        protected void PopulateLines(Parse_Pop _readObject, out List<string> _linesList)
        {
            //  Populate 'textLines' Array from .txt document
            string[] textArray = System.IO.File.ReadAllLines(_readObject.Txt_FilePath);
            _linesList = textArray.ToList();
            _readObject.TextArray = textArray;
        }

        //  GET: Find Page Conditional
        protected bool isPage(string _lineText)
        {
            bool resultBool;
            _lineText.Trim();
            // check if line is tag
            resultBool = (_lineText.StartsWith("<") && _lineText.EndsWith(">"));
            resultBool = _lineText.Contains("<li><") && _lineText.Contains("></li>");
            resultBool = _lineText.Contains("></li></ol>");

            return resultBool;
        }

        public void FindPageBreaksOSD()
        {
            int pgNum = 269;
            for (int i = 0; i < LinesList.Count; i++)
            {
                
                if(LinesList[i].Replace("<p>","").Trim().StartsWith(pgNum + "."))
                {
                    LinesList[i] = "$$$";
                    pgNum++;
                }
            }
        }

        //____________________
        //=====  MODIFY  =====

        //  MOD: Number Lines of a List
        protected List<string> prependNumberList(List<string> _readList)
        {
            List<string> tempList = new List<string>();
            int i = 0;
            foreach (var line in _readList)
            {
                i++;
                string NumTag = "Page " + i.ToString() + ": ";
                tempList.Add(line.Insert(0, NumTag));
            }
            return tempList;
        }

        //  MOD: FlagEmptyLines
        public void FlagEmptyLines(Parse_Pop _readObject, List<string> _readLines, out List<string> _outList)
        {
            //  Local var
            List<string> _sortedLines = new List<string>();

            //  Reduce Lines, Add FieldBreaks
            for (int i = 0; i < _readLines.Count(); i++)
            {
                //  add first line
                if (i == 0) _sortedLines.Add(_readLines[i]);
                //  add Non-Empty Lines
                else if (_readLines[i].Trim() != "") _sortedLines.Add(_readLines[i].Trim());
                //  add field break Lines
                else if (_readLines[i].Trim() == "" && _readLines[i - 1].Trim() != "#")
                {
                    _readLines[i] = "#";
                    _sortedLines.Add(_readLines[i]);
                }
            }
            _outList = _sortedLines;
        }

        //  SET: Convert List >> Dictionary (Indexed)
        protected Dictionary<int, string> ToDictIndex(List<string> _readList, out Dictionary<int, string> _DictOut)
        {
            Dictionary<int, string> tempDict = new Dictionary<int, string>();
            int cntKey = 0;
            foreach (var value in _readList)
            {
                tempDict[cntKey] = value;
                cntKey++;
            }
            _DictOut = tempDict;
            return tempDict;
        }


        //=======================================================
        //  SubRead: Reduce Elements of 'textLines' to 'linesList'

        public void ReduceLines(Parse_Pop _readObject, string[] textLines, out List<string> LinesList)
        {
            int pageCount = 0;
            for (int i = 1; i < textLines.Length; i++)

                //  Line is not empty
                if (textLines[i] != "")
                {
                    //  Line is the Start of a new Page
                    //if (textLines[i].Contains("1.") && textLines[i].Contains("<") && textLines[i].Contains(">"))

                    if (isPage(textLines[i]))
                    {
                        pageCount++;
                        textLines[i].Insert(0, "$$$");
                        _readObject.LinesList.Add("$$$ Page: " + pageCount.ToString() + " Line: " + i.ToString());  // New Page Symbol ($$$ pg#)
                    }

                    //  Submodules or Titleless Pages
                    if (textLines[i].Contains("SUBMODULE") || textLines[i].Contains("MODULE") || textLines[i].Trim() == "1.")
                    {
                        pageCount++;
                        textLines[i].Insert(0, "$$$ "); //  Prepend PageFlag ($$$) to line
                        _readObject.LinesList.Add(textLines[i] + " Page: " + pageCount.ToString());
                        //_readObject.LinesList.Add("$$$ Page: " + pageCount.ToString() + " Line: " + i.ToString());  // New Page Symbol ($$$ pg#)
                    }
                    else if ((textLines[i].Trim() == "<p>" || textLines[i].Trim() == "</p>") && textLines[i - 1] != "#")
                    {
                        textLines[i] = "#";
                        _readObject.LinesList.Add("#");  // New Field Symbol (#)
                    }
                    // Valid Text Content
                    else
                    {
                        //  Clean lines from html tags i.e. '</li>'
                        char[] webTag_1 = "<li>".ToCharArray();
                        char[] webTag_2 = "</li>".ToCharArray();
                        textLines[i].Trim(webTag_1);
                        textLines[i].Trim(webTag_2);

                        //  Add Clean Line to 'Object.LinesList' prop
                        _readObject.LinesList.Add(textLines[i].Trim()); // Text Content
                    }
                }
                //  Line is Empty
                else if (textLines[i] == "" && textLines[i - 1] != "#")
                {
                    textLines[i] = "#";
                    _readObject.LinesList.Add("#");  // New Field Symbol (#)
                }
                //  Redundant Line is Empty
                else if (textLines[i] == "" && textLines[i - 1] == "#")
                {
                    // Do Nothing
                }

            //  Assign out-param Value
            LinesList = _readObject.LinesList;
        }

        //==========================================================================================
        //========  Main Methods  ==================================================================
        //==========================================================================================

        //================================
        //  Routine: COMPARE Page Breaks
        //================================
        
        //  CREATE List of PageBreaks
        public static void CompareTxtDocs(Parse_Pop _txtObjNum, Parse_Pop _txtObjBull, out Dictionary<int, string> _PageBreaks)
        {
            Dictionary<int, string> tempDict = new Dictionary<int, string>();
            if (_txtObjBull.LineCount == _txtObjNum.LineCount)
            {
                for (int i = 0; i < _txtObjNum.LinesList.Count(); i++)
                {
                    if (_txtObjBull.LinesList[i] != _txtObjNum.LinesList[i])
                    {
                        tempDict.Add(i, _txtObjBull.LinesList[i]);
                    }
                }
            }
            else Console.WriteLine("ERROR: Lines of Txt Docs are NOT the same.");
            _PageBreaks = tempDict;

            Console.WriteLine("PageBreaks: " + tempDict.Count());
        }
        
        //  Get Flagless Version of Line (live)
        public string GetFlaglessLine(int _lineIndex)
        {
            int i = _lineIndex;
            string line = LinesList.ElementAtOrDefault(i);
            bool isPageBreak = false;
            bool isModule = false;
            bool isSubmodule = false;
            bool isGreen = false;
            bool isVideo = false;

            if      (line.StartsWith("$$$")) isPageBreak = true;
            else if (line.StartsWith("!!! Video:")) isVideo = true;
            else if (line.StartsWith("***"))
            {
                isModule = true;
                line = line.Remove(0, 3);
            }
            else if (line.StartsWith("###"))
            {
                isSubmodule = true;
                line = line.Remove(0, 3);
            }
            else if (line.Trim().StartsWith("^GREEN^"))
            {
                char[] tagGreen = "^GREEN^".ToCharArray();
                char[] tagStrong = "<strong>".ToCharArray();
                char[] tagStrongEnd = @"</strong>".ToCharArray();
                

                line = line.Trim(tagGreen);
                if (line.Contains(tagStrong.ToString())) line = line.TrimStart(tagStrong);
                if (line.Contains(tagStrongEnd.ToString())) line = line.TrimEnd(tagStrongEnd);
                isGreen = true;
            }

            return line;
        }

        //  CLEAN Reduce Lines of List<string>
        public static void CompareLinesList(Parse_Pop _ObjTarget, Parse_Pop _ObjTxt, out List<string> _isolatedList)
        {
            List<string> TarList = _ObjTarget.LinesList;
            List<string> TxtList = _ObjTxt.LinesList;
            List<string> tempList = new List<string>();
            int offset = 0;
            int j = 0;

            for (int i = 0; i < TxtList.Count(); i++)
            {
                
                Console.WriteLine("lineList" + i);
                string TxtLine = _ObjTxt.LinesList[i];
                string TarLine = _ObjTarget.LinesList[j + offset];

                if ((TarLine.StartsWith("<a") || TarLine.StartsWith("<p") || TarLine.StartsWith("<img")) && TarList[j + 1] == "")
                {
                    tempList.Add(TarLine);
                }
                else if (TarLine.StartsWith("<a") || TarLine.StartsWith("<p") || TarLine.StartsWith("<img"))
                {
                    if (TxtLine.Contains("Template")) TarList.Insert(i, "^^^");
                    else if (TxtLine == "^^^") tempList.Add(TarLine);
                    else if (!TarLine.Contains(TxtLine) && TxtLine != "") TxtList.Insert(i + 1, "^^^");
                    else tempList.Add(TarLine);
                }
                //else if (TarLine == "") TxtList.Insert(i, "^^^");
                else if (TxtLine == "" )
                {
                    TarList.Insert(i, "^^^");
                }
                else if (TarLine != "" && TxtLine != "" && TarLine.Split(' ').FirstOrDefault() == TxtLine.Split(' ').FirstOrDefault())
                    tempList.Add(TarLine);
                else if (TarLine.ToLower() == TxtLine.ToLower())
                    tempList.Add(TarLine);
                else
                {

                    for (j = i; j < TarList.Count(); j++)
                    {
                        TarLine = _ObjTarget.LinesList[j + offset].ToString();
                        Console.Clear();
                        Display.FormatLinesCheck_Scroll(i, j + offset, TarList, TxtList);
                        Console.WriteLine("Do you want to Add the 'HTML " + j + "' to the Iso List?\n'F': Yes\n" +
                            "'D': No\n'S': Back\n'A': Back 20 Lines\n'G': Decrement 'Offset'");
                        Console.WriteLine();
                        var userChoice = Console.ReadKey();

                        try
                        {
                            if (ConsoleKey.F == userChoice.Key)
                            {
                                //STORE Line
                                Console.WriteLine(" >> *Added*\n");
                                tempList.Add(_ObjTarget.LinesList[j]);
                                break;
                            }
                            else if (ConsoleKey.D == userChoice.Key)
                            {
                                //ADVANCE Offset +1
                                Console.WriteLine("*Ignored*");
                                j = j - 1;
                                offset++;
                            }
                            else if (ConsoleKey.S == userChoice.Key)
                            {
                                //RETARD HTML -1

                                j = j - 2;
                            }
                            else if (ConsoleKey.A == userChoice.Key)
                            {
                                //RETARD HTML -1
                                //RETARD TXT  -1
                                j = j - 2;
                                i = i - 1;
                            }
                            else if (ConsoleKey.G == userChoice.Key)
                            {
                                i = i - 20;
                                break;
                            }
                            else
                            {
                                j = j - 1;
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            i--;
                            j--;
                            Console.WriteLine("~ Unrecognized Key ~");
                            throw;
                        }
                    }
                }
                j++;
            }
            _isolatedList = tempList;
        }
        public void CompareLinesList( Parse_Pop _ObjTxt, out List<string> _isolatedList)
        {
            // Target Object
            List<string> TarList = this.LinesList;
            //  Input Compare Object
            List<string> TxtList = _ObjTxt.LinesList;
            List<string> tempList = new List<string>();

            int j = 0;

            for (int i = 0; i < TxtList.Count(); i++)
            {
                Console.WriteLine("lineList: " + i);
                string TxtLine = _ObjTxt.LinesList[i];
                string TarLine = this.LinesList[i];

                string fLineTxt = _ObjTxt.GetFlaglessLine(i);
                string fLineTar = this.GetFlaglessLine(i);

                string fLastTxt = "";
                string fLastTar = "";
                if (i != 0)
                {
                    fLastTxt = TxtList[i - 1];
                    fLastTar = TarList[i - 1];
                }

                string fNextTxt = "";
                string fNextTar = "";
                if (i<tempList.Count-1)
                {
                     fNextTxt= TxtList[i + 1];
                     fNextTar = TarList[i + 1];
                }

                //  ADD Line: First
                if (i == 0) tempList.Add(TxtLine);
                //  ADD Line: Lines are equivalent
                else if (fLineTar.ToLower() == fLineTxt.ToLower() || String.Equals(TarLine, TxtLine))
                    tempList.Add(TarLine);
                //  ADD Line: First words are equivalent
                else if (TarLine != "" && TxtLine != "" && TarLine.Split(' ').FirstOrDefault() == TxtLine.Split(' ').FirstOrDefault())
                    tempList.Add(TarLine);
                else if (fLineTxt.ToLower() == fLineTar.ToLower()) tempList.Add(TarLine);
                //  Txt Overrides
                else if (fLineTxt != fLineTar)
                {
                    //  Videos
                    if (TarLine.StartsWith("!!!"))
                    {
                        char[] separator = "!!! ".ToCharArray();
                        string urlStripped = TarLine.Substring(11).Split(separator).FirstOrDefault();
                        
                        tempList.Add(String.Format("VIDEO~{0}~{1}", TxtLine, urlStripped));
                    }
                    //  PageBreaks
                    if ((fLineTxt.Trim() == "$$$" && fLastTxt == fLastTar) || fNextTar == fNextTxt) tempList.Add(TxtLine);
                    //  Quizzes 
                    else if (fLineTxt.Contains(fLineTar))
                    {
                        string firstChar = fLineTxt.ElementAtOrDefault(0).ToString();
                        bool isInt = Int32.TryParse(firstChar, out int listNum);

                        if (isInt) tempList.Add(TxtLine);
                        else
                        {
                            if (fLineTxt.Count() > fLineTar.Count()) tempList.Add(fLineTxt);
                            else tempList.Add(fLineTar);
                        }
                    }

                }

                //  NOT EQUIVALENT LINES
                else
                {
                    for (j = i; j < TarList.Count(); j++)
                    {
                        TarLine = this.LinesList[j].ToString();
                        Console.Clear();
                        Display.FormatLinesCheck_Add(i, j, TarList, TxtList);
                        Console.WriteLine("Do you these Lines match?\n" +
                            "'F': Yes, Next\n" +
                            "'D': No, Insert Line: TxtList\n" +
                            "'S': No, Insert Line: TarList\n" +
                            "'A': Back 2 Lines\n" +
                            "'G': Decrement 'Offset'");
                        Console.WriteLine();
                        var userChoice = Console.ReadKey();

                        try
                        {
                            if (ConsoleKey.F == userChoice.Key)
                            {
                                //NEXT
                                tempList.Add(this.LinesList[j]);
                                Console.WriteLine(" >> *Added*\n");
                                break;
                            }
                            else if (ConsoleKey.D == userChoice.Key)
                            {
                                //ADD Line: TxtList
                                
                                TxtList.Insert(i, "^^^");
                                Console.WriteLine("*ADD to TXT* \n");
                            }
                            else if (ConsoleKey.S == userChoice.Key)
                            {
                                //ADD Line: TarList
                                TarList.Insert(j, "^^^");
                                Console.WriteLine("*ADD to TAR* \n");
                                //j = j - 2;
                            }
                            else if (ConsoleKey.A == userChoice.Key)
                            {
                                //RETARD HTML -1
                                //RETARD TXT  -1
                                j = j - 2;
                                i = i - 1;
                                Console.WriteLine("*BACK* \n");
                            }
                            else if (ConsoleKey.G == userChoice.Key)
                            {
                                i = i - 20;
                                break;
                            }
                            else
                            {
                                j = j - 1;
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            i--;
                            j--;
                            Console.WriteLine("~ Unrecognized Key ~\n");
                            throw;
                        }
                    }
                }
                j++;
            }
            _isolatedList = tempList;
            this.IsolatedList = tempList;
        }

        //___________________________
        //  RECORD Functions (FXN)

        
    }
}

