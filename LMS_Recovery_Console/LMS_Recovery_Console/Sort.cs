using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LMS_Recovery_Console
{
    public class Sort
    {
        //  Class Properties
        public int cntKC { get; set; }

        public List<string> IsolatedList { get; set; }
        public List<string> TestsList { get; set;  }
        public List<string> FormatAOLCC { get; set; }

        public List<List<string>>           PageList { get; set; }
        public List<List<List<string>>>     ModuleList { get; set; }
        public List<List<StringBuilder>>    CompiledPages { get; set; }
        public List<List<StringBuilder>>    SortedPages { get; set; }
        public List<List<Tuple<string, string>>> IdentfiedPages { get; set; }
        public List<List<string>>           FormattedList { get; set; }
    
        List<string> FlagList = new List<string>()
        {
            "$$$","!!!","^GREEN^","***","^^^","###"
        };

        //  Format List: AOLCC
        public List<string> SortFormatAOLCC()
        {
            List<string> _ReadList = this.IsolatedList;
            List<string> tempList = new List<string>();

            foreach (var line in _ReadList)
            {
                char[] separator = " ".ToCharArray();
                string[] wordsArray = line.Split(separator);
                StringBuilder tempString = new StringBuilder();

                //  Page Break
                if (line.StartsWith("$$$"))
                {

                }
                //  Module Break
                if (line.StartsWith("***"))
                {
                    tempString.Append("Module~");
                    for (int i = 0; i < wordsArray.Count(); i++)
                    {
                        if (i == 1) tempString = tempString.Append(wordsArray[i]+"~");
                        else if (i > 1) tempString.Append(wordsArray[i]+" ");
                    }
                }
                //  Submodule Break
                if (line.StartsWith("###"))
                {
                    tempString.Append("Submodule~");
                    for (int i = 0; i < wordsArray.Count(); i++)
                    {
                        if (i == 1) tempString = tempString.Append(wordsArray[i] + "~");
                        else if(i > 1) tempString.Append(wordsArray[i] + " ");
                    }
                }
                //  Video Element
                if (line.StartsWith("!!!"))
                {

                }
                //  Green Font Element
                if (line.Trim().StartsWith("^GREEN^")) tempString = tempString.Append(line.Substring(7));
            }

            return tempList;
        }
        public void SortPagesFormatAOLCC()
        {
            List<string> tempPage = new List<string>();
            StringBuilder tempString = new StringBuilder();
            foreach (var page in PageList)
            {
                string _readLine = "";
                for (int i = 0; i < page.Count; i++)
                {
                    _readLine = page[i];

                    if (_readLine != "$$$")
                    foreach (var flag in FlagList)
                    {
                        var flagId = flag.ToCharArray();
                        _readLine = _readLine.Trim(flagId).Trim();
                    }

                    
                    
                    //if (_readLine.StartsWith("^GR"))
                    //{
                    //    var tagGreen = "^GREEN^".ToCharArray();
                    //    _readLine = _readLine.Trim(tagGreen);
                    //}

                    if (_readLine == "$$$") continue;
                    else if (_readLine.Contains("ESTIMATED COURSE"))
                    {
                        _readLine = "LESSON~" + _readLine + "~" + page[i + 1];
                        tempPage.Add(_readLine);

                        i++;
                    }
                    else if (_readLine.ToUpper().Contains("MODULE"))
                    {
                        var stringArray = _readLine.Split();
                        string title = "";
                        if (stringArray.Length > 2)
                        {
                            for (int j = 2; j < stringArray.Length; j++)
                            {
                                title = title + " " + stringArray[j];
                            }
                        }

                        string cleanString = String.Format("{0}~{1}~{2}", stringArray[0], stringArray[1], title);
                        tempPage.Add(cleanString);
                    }
                    else tempPage.Add(_readLine);
                }
                FormattedList.Add(tempPage);
            }

        }

        //  Separate Pages
        public void SeparatePages()
        {
            List<string> tempPage = new List<string>();
            int PageCount = 0;

            for (int i = 0; i < this.IsolatedList.Count; i++)
            {
                string line = IsolatedList[i];
                bool nowGreen = false;
                bool wasGreen = false;

                //  NOW Green?
                if (IsolatedList[i].Contains("^G^")) nowGreen = true;
                else nowGreen = false;
                //  WAS Green?
                if (i > 0 && IsolatedList[i - 1].Contains("^G^")) wasGreen = true;
                else wasGreen = false;

                //  if PageBreak
                if (line.StartsWith("$$$"))
                {
                    // Add Page to PageList and Restart
                    PageCount++;
                    PageList.Add(tempPage);
                    tempPage = new List<string>();

                    Console.WriteLine("NEW PAGE: "+PageCount);
                }
                //  Green Changed
                else if (wasGreen && !nowGreen)
                {
                    // Add Page to PageList and Restart
                    PageCount++;
                    PageList.Add(tempPage);
                    tempPage = new List<string>() { line };

                    Console.WriteLine("NEW PAGE: " + PageCount);
                }
                //  if Last line in IsolatedList
                else if (i == IsolatedList.Count - 2)
                {
                    tempPage.Add(line);
                    PageList.Add(tempPage);
                    Console.WriteLine("END of SortPages()");
                }
                else
                {
                    tempPage.Add(line);
                    Console.WriteLine("Add Line " + i);
                }
            }

        }

        //  Identify Pages
        public void IdentifyPages()
        {
            foreach (var page in PageList)
            {
                
            }
        }
        // Separate Modules
        public void SeparateModules()
        {
            List<List<string>> tempModule = new List<List<string>>();
            int cntModule = 0;
            foreach (var page in this.PageList)
            {
                foreach (var line in page)
                {
                    if (line.Contains("MODULE") && !line.Contains("SUBMODULE"))
                    {
                        
                        if (cntModule > 0) ModuleList.Add(tempModule);
                        tempModule = new List<List<string>>();
                        List<string> moduleTitle = new List<string>() { "$$$$$$$  MODULE " + cntModule + "  $$$$$$$" };
                        tempModule.Add(moduleTitle);
                        cntModule++;
                    }
                 
                }
                if(page.Count > 0 && page[0].Contains("FINAL EXAM"))
                {
                    if (cntModule > 0) ModuleList.Add(tempModule);
                    tempModule = new List<List<string>>();
                    List<string> moduleTitle = new List<string>() { "$$$$$$$  MODULE " + cntModule + " FINAL EXAM  $$$$$$$" };
                    tempModule.Add(moduleTitle);
                    cntModule++;
                }
                tempModule.Add(page);
            }
            ModuleList.Add(tempModule);
        }

        //==================================================================
        // Route Page Types 
        //==================================================================
        public void RoutePageTypes()
        {
            List<List<string>> tempPageList = new List<List<string>>();
            this.cntKC = 0;

            foreach (var page in this.PageList)
            {
                var pageLine = HandlePageTypes(page, cntKC);
                tempPageList.Add(pageLine);
                Console.WriteLine("KC count: " + this.cntKC);
            }
            this.PageList = tempPageList;
        }

        //====================================================================
        //  IDENTIFY PAGE TYPES
        //====================================================================
        public List<string> HandlePageTypes(List<string> _readPage, int _cntKC)
        {
            var tempPage = new List<string>();
            bool isMatch = false;
            bool userCheck = false;

            //  IDENTIFY Page Type
            //  LOOP thr Lines of page
            for (int j = 0; j < _readPage.Count; j++)
            {
                userCheck = false;
                while (!userCheck)
                {
                    var line = _readPage[j];

                    //  VIDEO
                    if (line.ToLower().StartsWith("VIDEO"))
                    {
                        tempPage = HandleVideo(_readPage);
                        isMatch = true;
                    }
                    //  QUESTION
                    else if (line.ToLower().StartsWith("knowledge"))
                    {
                        bool noTitle = true;
                        //  Handle Tests Fxn
                        tempPage = HandleTests(_readPage, cntKC);
                        isMatch = true;
                        break;
                    }
                    //  GREEN
                    else if (line.StartsWith("^G^") && line.Contains("MODULE"))
                    {
                        tempPage = HandleGreen(_readPage, cntKC);
                        isMatch = true;
                    }
                    //  DRILL
                    else if (line.ToLower().Trim() == "assignment")
                    {
                        tempPage = HandleDrill(_readPage);
                        isMatch = true;
                    }

                    //  LESSON
                    if (!isMatch)
                    {
                        tempPage = HandleLesson(_readPage);
                    }
                    Console.WriteLine("np==========================================");
                    Display.ToConsole(tempPage);
                    string input = Console.ReadKey().Key.ToString();
                    if (input == "F")
                    {
                        userCheck = true;
                        break;
                    }
                }
            }
            return tempPage;
        }

        //==================================
        //========== HANDLE PAGES TYPES ==========

        //  VIDEO
        public List<string> HandleVideo(List<string> _readPage)
        {
            var _tempList = new List<string>();
            foreach (var line in _readPage)
            {
                if (line.StartsWith("VIDEO~~")) _tempList.Add(line);
                else if (line.Contains("video in full")) continue;
            }

            return _tempList;
        }

        //  QUESTION
        public List<string> HandleTests(List<string> _readPage, int _cntKC)
        {
            //  Local Variables
            string fLine = _readPage[0];
            var _outList = new List<string>();
            var tempStrBldr = new StringBuilder();
            bool isFinalExam = false;
            int qCount = 0;

            // Exam? or KnowCheck?
            //  IS Exam?
            for (int i = 0; i < 3; i++)
            {
                if (_readPage[i].ToLower().Trim().Contains("final exam")) isFinalExam = true;
            }

            //  Knowledge Check
            if (!isFinalExam) _outList = HandleKC(_readPage);

            //  Final Exam
            else if (isFinalExam) _outList = HandleExam(_readPage);
  
            //  Return OUT LIST
            return _outList;
        }
        //  QUESTION: Knowledge Checks
        public List<string> HandleKC(List<string> _readPage)
        {
            var _outList = new List<string>();
            var tempStrBldr = new StringBuilder();
            int qCount = 0;

            this.cntKC++;
            //  Compile Lines to Questions
            for (int i = 0; i < _readPage.Count; i++)
            {
                string line = _readPage[i];
                //  QUESTION
                if (line.Trim().StartsWith("^^"))
                {
                    //  Additional Q
                    if (qCount > 0)
                    {
                        //  Add and Clear
                        _outList.Add(tempStrBldr.ToString());
                        tempStrBldr = new StringBuilder();
                        this.cntKC++;
                    }
                    //  New Question
                    tempStrBldr.Append("QUESTION~~Knowledge Check #" + this.cntKC + "~~" + line.Trim("^^".ToCharArray()).Trim());
                    qCount++;
                }
                //  ANSWER
                else if (line.Trim().StartsWith("##"))
                {
                    string tString = line.Trim("##".ToCharArray());
                    tString = tString.Replace("CORRECT", "").Trim();
                    tempStrBldr.Append("~~" + tString);
                }
                //  LAST line
                if (i == _readPage.Count - 1) _outList.Add(tempStrBldr.ToString());
                else continue;
            }
            return _outList;
        }
        //  QUESTION: Exams
        public List<string> HandleExam(List<string> _readPage)
        {
            var _outList = new List<string>();
            var tempStrBldr = new StringBuilder();
            int qCount = 0;

            _outList.Add("EXAM~~Final Exam");
            for (int i = 0; i < _readPage.Count; i++)
            {
                string line = _readPage[i];
                //  QUESTION
                if (line.Trim().StartsWith("^^"))
                {
                    //  Additional Q
                    if (qCount > 0)
                    {
                        //  Add and Clear
                        _outList.Add(tempStrBldr.ToString());
                        tempStrBldr = new StringBuilder();
                    }
                    //  All Question
                    tempStrBldr.Append("EXAM~~" + qCount + "~~" + line.Replace("^^", "").Trim());
                    qCount++;
                }
                //  ANSWER
                else if (line.Trim().StartsWith("##"))
                {
                    string tString = line.Replace("##", "").Trim();
                    tString = tString.Replace("CORRECT", "").Trim();
                    tempStrBldr.Append("~~" + tString);
                }
                //  LAST line
                if (i == _readPage.Count - 1) _outList.Add(tempStrBldr.ToString());
                else continue;
            }
            return _outList;
        }

        //  GREEN
        public List<string> HandleGreen(List<string> _readPage, int _cntKC)
        {
            var _outList = new List<string>();
            var tempStrBldr = new StringBuilder();

            // LOOP >> LINES
            for (int i = 0; i < _readPage.Count; i++)
            {
                //  Local Variables
                tempStrBldr = new StringBuilder();
                string line = _readPage[i].Replace("^G^","").Replace(".","").Trim();
                //_______________________
                // IS MODULE / SUBMODULE
                if (line.ToLower().Contains("module"))
                {
                    //  IS Module
                    if (!line.ToLower().Contains("submodule"))
                    {
                        this.cntKC = 0;
                        tempStrBldr.Append("MODULE~~");
                    }
                    //  IS Submodule
                    else tempStrBldr.Append("SUBMODULE~~");
                    //  Separate Words
                    string[] tempWords = line.Split(" ".ToCharArray());
                    //  Compile words with Separations
                    for (int j = 0; j < tempWords.Length; j++)
                    {
                        string word = tempWords[j];
                        if (j == 0) continue;
                        else if (j == 1) tempStrBldr.Append(word.Trim() + "~~");
                        else tempStrBldr.Append(word.Trim() + " ");
                    }
                    _outList.Add(tempStrBldr.ToString().Trim());
                }
                //_______________________
                //  Learning Objectives
                else if (line.ToLower().Contains("learning objectives"))
                {
                    tempStrBldr.Append("OBJECTIVE~~Learning Objectives");
                    //  ADD Objectives
                    foreach (var lines in _readPage)
                    {
                        //  ADD Objectives
                        if (lines.Contains("%%"))
                            tempStrBldr.Append("~~"+lines.Replace("%%","").Replace("^G^","").Trim());
                    }
                    _outList.Add(tempStrBldr.ToString());
                }
                //_______________________
                //  Estimated Study
                else if (line.ToLower().Contains("estimated course"))
                {
                    tempStrBldr.Append("ESTIMATE~~"+line.Replace("^G^","").Replace(":","").Trim());
                    tempStrBldr.Append("~~" + _readPage[i + 1].Replace("^G^ ","").Trim());
                    _outList.Add(tempStrBldr.ToString().Trim());
                }
            }
            return _outList;
        }

        //  OBJECTIVES
        public List<string> HandleObjective(List<string> _readPage)
        {
            var _tempList = new List<string>();



            return _tempList;
        }

        //  DRILL
        public List<string> HandleDrill(List<string> _readPage)
        {
            var _tempList = new List<string>();

            return _tempList;
        }

        //  LESSON
        public List<string> HandleLesson(List<string> _readPage)
        {
            var tempStrBldr = new StringBuilder();
            var _tempList = new List<string>();

            tempStrBldr.Append("LESSON");
            for (int i = 0; i < _readPage.Count; i++)
            {
                string line = _readPage[i];
                if (i == 0)
                {
                    string[] wordsApart = line.Split(":".ToCharArray());
                    if (wordsApart[0].Length < 20)
                    {
                        line = line.Replace(wordsApart[0], "").Trim();
                        //  ADD Title
                        tempStrBldr.Append("~~" + wordsApart[0].Replace(":", ""));
                        //  ADD Definition
                        tempStrBldr.Append("~~" + line.Replace(":","").Trim());
                    }
                    else if (_readPage[i].Length > 41) _readPage.Insert(0, "[Title]");

                }
                if(i<=1) tempStrBldr.Append("~~" + line);
                else if (i>1)tempStrBldr.Append("<br>" + line);
            }
            _tempList.Add(tempStrBldr.ToString());
            return _tempList;
        }

        //===================================
        //===================================

        //  Separate Tests
        public void SeparateTests()
        {
            List<List<List<string>>> tempModuleList = new List<List<List<string>>>();

            // Loop through Modules
            foreach (var Module in ModuleList)
            {
                List<List<string>> tempModule = new List<List<string>>();

                // Loop through Pages
                for (int p = 0; p < Module.Count; p++)
                {
                    var  _readPage = Module[p];
                    List<string> tempPage = new List<string>();

                    //  Check if Test
                    bool isTest = false;
                    bool isExam = false;
                    foreach (var line in _readPage)
                    {
                        if (line.Contains("FINAL EXAM"))
                        {
                            isExam = true;
                            break;
                        }
                        else if (line.Contains("CORRECT"))
                        {
                            isTest = true;
                            break;
                        }
                    }

                    //  Check if Exam (Final Quiz)

                    //  Page IS Exam
                    if (isExam) tempModule.Add(FormatExam(_readPage));
                    //  Page IS Test
                    else if (isTest) tempModule.Add(FormatTest(_readPage));
                    // Page NOT Test
                    else tempModule.Add(_readPage);
                }
                tempModuleList.Add(tempModule);
            }
            this.ModuleList = tempModuleList;

            GroupTests();
        }

        //  Format Test Page
        public List<string> FormatTest(List<string> _readPage)
        {
            // Method Variables
            List<string> tempList = new List<string>();
            List<string> _outList = new List<string>();
            StringBuilder tempStrBld = new StringBuilder();
            List<string> qList = new List<string>() { ""};

            var page = new List<string>();
            string[] alphaArray = new string[5] { "a", "b", "c", "d", "a" };
            char[] tagsBad = ") ".ToCharArray();
            char[] tagsBad2 = ". ".ToCharArray();


            //  Trim Lines
            foreach (var line in _readPage)
            {
                string tempLine;
                tempLine = line.Replace("<p>", "");
                tempLine = tempLine.Replace("</p>", "");
                tempLine = tempLine.Replace("~~", "");
                tempLine = tempLine.Trim().Replace(". ", "");
                tempLine = tempLine.Trim().Replace(") ", "");
                tempLine = tempLine.Replace("##EndNum", "");
                if (tempLine.Length > 1)
                {
                    if (Int32.TryParse(tempLine[0].ToString(), out int blah)
                        && !Int32.TryParse(tempLine[1].ToString(), out int blahh)) tempLine = tempLine.Substring(1);
                }

                page.Add(tempLine);
            }

            for (int j = 0; j < page.Count; j++)
            {
                var line = page[j];
                var charArray = line.ToArray();
                int spacesHigh = 0;
                int space = 0;
                for (int k = 0; k < charArray.Length; k++)
                {
                    var ch = charArray[k];
                    if (ch.ToString() == " ") space++;
                    else if (ch.ToString() != " ")
                    {
                        spacesHigh = space;
                        if (spacesHigh > 2)
                        {
                            page[j] = line.Substring(k-1).Trim();
                        }
                        space = 0;
                    }
                }
            }

            //  Loop through page
            for (int i = 0; i < page.Count; i++)
            {
                //  Variables
                var line = page[i];

                if (page[i].Contains("?") || page[i].Contains("."))
                {
                    foreach (var item in qList) tempStrBld.Append(item);
                    tempList.Add(tempStrBld.ToString());
                    qList = new List<string>() { ""};
                    tempStrBld = new StringBuilder();

                    qList[0] = line.Trim().Insert(0, "QUESTION~~");
                }
                else if (line.Contains("QUIZ")) tempList.Add(line.Replace(":", ""));
                else if (line.Contains("CORRECT")) qList.Insert(1, line.Insert(0, "~~").Replace(" CORRECT",""));
                else if (line.Contains("INCORRECT")) qList.Add(line.Insert(0, "~~").Replace(" INCORRECT", ""));
                else if (page.Count - i == 1)
                {
                    qList.Add(line.Insert(0, "~~"));
                    foreach (var item in qList) tempStrBld.Append(item);
                    tempList.Add(tempStrBld.ToString());
                    qList = new List<string>();
                    tempStrBld = new StringBuilder();
                }
                else if (qList.Count > 0 && qList.Count < 6) qList.Add(line.Insert(0,"~~"));
                else tempList.Add(line.Insert(0, "~~"));
            }

            foreach (var line in tempList)
            {
                if (line != "") _outList.Add(line);
            }
            return _outList;
        }

        //  Format Exam Page
        public List<string> FormatExam(List<string> _readPage)
        {
            // Method Variables
            List<string> oddList = new List<string>();
            List<string> _outList = new List<string>();
            StringBuilder tempStrBld = new StringBuilder();
            List<string> qList = new List<string>() { "" };
            List<string> tempPage = new List<string>();

            string[] alphaArray = new string[5] { "a", "b", "c", "d", "a" };
            char[] tagsBad = ") ".ToCharArray();
            char[] tagsBad2 = ". ".ToCharArray();

            //  Trim Lines
            foreach (var line in _readPage)
            {
                string tempLine;
                tempLine = line.Replace("<p>", "");
                tempLine = tempLine.Replace("</p>", "");
                tempLine = tempLine.Replace("~~", "");
                tempLine = tempLine.Trim().Replace(". ", "");
                tempLine = tempLine.Trim().Replace(") ", "");
                tempLine = tempLine.Replace("##EndNum", "");
                if (tempLine.Length > 1)
                {
                    if (Int32.TryParse(tempLine[0].ToString(), out int blah)
                        && !Int32.TryParse(tempLine[1].ToString(), out int blahh)) tempLine = tempLine.Substring(1);
                }
                tempPage.Add(tempLine);
            }

            //  Compile Question Lines
            for (int i = 0; i < _readPage.Count; i++)
            {
                var line = _readPage[i];
                string fChar = line.FirstOrDefault().ToString();

                if (line.Contains("FINAL EXAM")) tempPage.Add("MODULE~~FINAL EXAM");
                else if (line[1] == tagsBad2.FirstOrDefault() || line[1] == tagsBad.FirstOrDefault())
                {
                    // Line is Question (Numbered)
                    if (Int32.TryParse(fChar, out int qNum))
                    {
                        if (qList.Count > 1) tempPage.Add(CompileQuestions_AOLCC(qList));
                        qList = new List<string>() { "QUESTION~~", "" };
                        qList.Insert(1, "~~" + line.Substring(3).Trim());
                    }
                    //  Line is Answer (Lettered)
                    //      Correct
                    else if (line.ToLower().EndsWith("Correct")) qList.Insert(2, "~~" + line.Substring(3).Trim());
                    //      Incorrect
                    else qList.Add("~~" + line.Substring(3).Trim());
                }
            }

            return tempPage;
        }

        //  Group Tests
        public void GroupTests()
        {
            var tempModuleList = new List<List<List<string>>>();

            foreach (var Module in this.ModuleList)
            {
                var tempModule = new List<List<string>>();
                var endTests = new List<string>();
                
                foreach (var Page in Module)
                {
                    int qCount = 0;
                    var tempPage = new List<string>();

                    foreach (var line in Page)
                    {
                        if (line.StartsWith("QUESTION"))
                        {
                            if (qCount == 0) tempPage.Add(line);
                            else endTests.Add(line);
                            qCount++;
                        }
                        else tempPage.Add(line);
                    }
                    tempModule.Add(tempPage);
                }
                tempModule.Add(endTests);
                tempModuleList.Add(tempModule);
            }
            this.ModuleList = tempModuleList;
        }

        //  Group Lessons
        public void GroupLessons()
        {
            List<string> flagList = new List<string>()
                {
                    "QUESTION",
                    "VIDEO",
                    "MODULE",
                    "SUBMODULE",
                    "OBJECTIVE",
                    "LESSON"
                };

            List<string> _outList = new List<string>();
            List<string> _outList2 = new List<string>();

            foreach (var module in ModuleList)
            {
                foreach (var page in module)
                {
                    var tempPage = new List<string>();
                    var tempStrBld = new StringBuilder();
                    tempStrBld.Append("LESSON");
                    foreach (var line in page)
                    {
                        string tempString = line.Replace("\"", "\"");
                        bool flagCheck = false;

                        foreach (var flag in flagList)
                        {
                            if (line.Replace("<p>", "").Trim().StartsWith(flag)) flagCheck = true;
                        }

                        if (flagCheck)
                        {
                            _outList.Add(tempStrBld.ToString());
                            _outList.Add(line.Replace("<p>", "").Replace("</p>", ""));
                            tempStrBld = new StringBuilder();
                            tempStrBld.Append("LESSON");
                        }
                        else if (tempString.Contains("Watch this video in full") || tempString.Contains("$$$$")) continue;
                        else
                        {
                            tempStrBld.Append(tempString.Insert(0, "~~"));
                        }
                    }
                    _outList.Add(tempStrBld.ToString());
                }
            }

            foreach (var line in _outList)
            {
                if (line == "LESSON") continue;
                else _outList2.Add(line);
            }
            this.FormatAOLCC = _outList2;
        }

        //  Compile Question Strings
        public void CompileQuestions_AOLCC()
        {
            bool isTest = false;
            List<string> extraList = new List<string>();

            //  Loop thru PAGES
            for (int n = 0; n < PageList.Count; n++)
            {
                List<string> tempList = new List<string>();
                List<string> page = PageList[n];
                int qCount = 0;

                //  Is Page a Test??
                for (int i = 0; i < page.Count; i++)
                {
                    if (page[i].Contains("CORRECT"))
                    {
                        isTest = true;
                        break;
                    }
                }

                //  Compile Question Strings
                if (isTest)
                {
                    StringBuilder questionString = new StringBuilder();
                    StringBuilder answerString = new StringBuilder();

                    //  LOOP thru Lines
                    foreach (var line in page)
                    {
                        if (line.StartsWith("QUESTION~"))
                        {
                            qCount++;
                            if (qCount > 1) questionString.Append(answerString);
                            if (qCount == 2) tempList.Add(questionString.ToString());
                            else if (qCount > 2) TestsList.Add(questionString.ToString());
                            questionString = new StringBuilder();
                            answerString = new StringBuilder();
                            questionString.Append(line);
                        }
                        else if (line.StartsWith("~") && line.Contains("CORRECT")) answerString.Insert(0, line.Replace(" CORRECT",""));
                        else if (line.StartsWith("~") && !line.Contains("CORRECT")) answerString.Append(line);
                    }
                    if (qCount > 0) questionString.Append(answerString);
                    if (qCount == 1) tempList.Add(questionString.ToString());
                    else if (qCount > 1) TestsList.Add(questionString.ToString());
                    //  Replace Page with new Format
                    PageList[n] = tempList;
                    isTest = false;
                }
            }
        }
        public string CompileQuestions_AOLCC(List<string> _readList)
        {
            StringBuilder tempStrBldr = new StringBuilder();

            foreach (var item in _readList)
            {
                if (item != "") tempStrBldr.Append(item);
            }

            return tempStrBldr.ToString();
        }

        //  Compile Text Content
        public void CompileText_AOLCC()
        {
            List<string> tempList = new List<string>();

            for (int n = 0; n < PageList.Count; n++)
            {
                List<string> page = PageList[n];
                for (int i = 0; i < page.Count; i++)
                {
                    string line = page[i];

                    if (line.ToUpper().StartsWith("MODULE")) page[i] = CompText_Module(page, i);

                    else if (line.ToUpper().StartsWith("SUBMODULE")) page[i] = CompText_Submodule(page, i);

                    else if (line.ToUpper().StartsWith("LEARNING OBJECTIVES")) page[i] = CompText_Objectives(page, i);
   
                    else if (line.ToUpper().StartsWith("ESTIMATED")) page[i] = CompText_Estimated(page, i);

                    else continue;
                }
            }
        }

        //  Compile Module Format
        public string CompText_Module(List<string> _readList, int index)
        {
            string formatString = "";
            var sep = " ".ToCharArray();
            StringBuilder title = new StringBuilder();

            var wordArray =_readList[index].Split(sep);
            for (int i = 2; i < wordArray.Length; i++) title.Append(wordArray[i] + " ");
            if (wordArray.Length >= 2)
            {
                if (title.Length > 0) formatString = String.Format("Module~{0}~{1}", wordArray[1].Trim(".".ToCharArray()), title.ToString().Trim().Insert(title.Length - 1, "."));
                else if (title.Length == 0) formatString = String.Format("Module~{0}~{1}", wordArray[1].Trim(".".ToCharArray()), @"n/a");
            }
            else formatString = @"Module~n/a~n/a";
            Console.WriteLine("tile string length: "+ title.Length.ToString());
            
            return formatString;
        }
        //  Compile Submodule Format
        public string CompText_Submodule(List<string> _readList, int index)
        {
            string formatString = "";
            var sep = " ".ToCharArray();
            StringBuilder title = new StringBuilder();

            var wordArray = _readList[index].Split(sep);
            for (int i = 2; i < wordArray.Length; i++) title.Append(wordArray[i] + " ");
            if (wordArray.Length >= 2)
            {
                if (title.Length > 0) formatString = String.Format("Submodule~{0}~{1}", wordArray[1].Trim(".".ToCharArray()), title.ToString().Trim().Insert(title.Length - 1, "."));
                else if (title.Length == 0) formatString = String.Format("Submodule~{0}~{1}", wordArray[1].Trim(".".ToCharArray()), @"n/a");
            }
            else formatString = @"Submodule~n/a~n/a";
            Console.WriteLine("tile string length: " + title.Length.ToString());

            return formatString;
        }
        //  Compile Learning Objectives Format
        public string CompText_Objectives(List<string> _readList, int index)
        {
            var page = _readList;
            StringBuilder formatString = new StringBuilder();
            formatString.Append("OBJECTIVE~Learning Objectives:");

            for (int i = index+1; i < _readList.Count; i++)
            {
                var line = page[i];
                if (!line.ToLower().Contains("module") && !line.ToLower().Contains("estimated course")) formatString.Append("~"+line);
                else break;
            }

            return formatString.ToString();
        }
        //  Compile Estimated Time Format
        public string CompText_Estimated(List<string> _readList, int index)
        {
            StringBuilder tempString = new StringBuilder();
            tempString.Append("LESSON~Estimated Course Completion Time:~"+_readList[index+1]);

            return tempString.ToString();
        }

        //  Clean Compilor 
        public void CompTextClean()
        {
            for (int j = 0; j < PageList.Count; j++)
            {
                List<string> page = PageList[j];
                List<string> pageClean = new List<string>();
                bool isGreen = false;
                for (int k = 0; k < page.Count; k++) if (page[k].ToLower().Contains("learning objectives:")) isGreen = true;

                if (isGreen)
                {
                    foreach (var line in page)
                    {
                        string loLine = line.ToLower();
                        if (line.Contains("~")) pageClean.Add(line);
                    }
                    PageList[j] = pageClean;
                }
                else
                {
                    StringBuilder formatString = new StringBuilder();
                    formatString.Append("LESSON~");
                    foreach (var line in page) formatString.Append("~"+line);
                    pageClean.Add(formatString.ToString());
                }
            }
        }

        public void compText_LessonsLeft()
        {
            for (int j = 0; j < PageList.Count; j++)
            {
                List<string> page = PageList[j];
                List<string> tempPage = new List<string>();
                StringBuilder formatString = new StringBuilder();
                int vidInx = 1;
                bool vidChx = false;

                formatString.Append("LESSON");
                // Check for Video
                //foreach (var line in page) 
                //{
                //    if (line.StartsWith("VIDEO~"))
                //    {
                //        vidChx = true;
                //        break;
                //    }
                //}

                for(int i = 0; i < page.Count; i++)
                {
                    var line = page[i];
                    if (line.StartsWith("VIDEO~"))
                    {
                        vidChx = true;
                        tempPage.Add(line);
                    }
                    else if (line.ToLower().Contains("~")) tempPage.Add(line);
                    else
                    {
                        formatString.Append("~" + line);
                        if(!vidChx) vidInx--;
                    }
                }
                // Find Video Index
                if (vidChx)
                {
                    for (int i = 0; i < tempPage.Count; i++)
                    {
                        if (tempPage[i].StartsWith("VIDEO~")) vidInx = i;
                    }
                }
                if (formatString.Length > 6)
                {
                    if (vidChx) tempPage.Insert(vidInx, formatString.ToString());
                    else tempPage.Add(formatString.ToString());
                }
                PageList[j] = tempPage;
            }
        }

        //  Count and Add 'Knowledge Check' Title
        public void CountAddKnowledge()
        {
            int qCount = 0;
            bool inSubMod = false;
            for (int i = 0; i < PageList.Count; i++)
            {
                var page = PageList[i];
                for (int j = 0; j < page.Count; j++)
                {
                    var line = page[j];
                    if (line.ToLower().StartsWith("submodule")) qCount = 0;
                    else if (line.StartsWith("QUESTION~"))
                    {
                        qCount++;
                        string kCString = "Knowledge Check #" + qCount.ToString()+"~";
                        page[j]= line.Insert(9, kCString);
                    }
                }
                PageList[i] = page;
            }
        }

        public void SortTests()
        {
            bool isTest = false;
            List<List<string>> outPagesList = new List<List<string>>();
            List<string> FinalTest = new List<string>();
            // Loop Pages
            foreach (var page in PageList)
            {
                //  Is Page a Test??
                for (int i = 0; i < page.Count; i++)
                {
                    if (page[i].Contains("CORRECT")) isTest = true;
                }

                //  Seperate Extra Tests
                if (isTest)
                {
                    List<string> tempTestPage = new List<string>();
                    FormatTest(page, out List<string> _tempTestPage);

                    //  Add First Question to tempTestPage
                    tempTestPage.Add(_tempTestPage[0].ToString());
                    outPagesList.Add(tempTestPage);

                    //  Add Remaing Questions to FinalTest
                    if (_tempTestPage.Count > 1)
                    {
                        for (int i = 1; i < _tempTestPage.Count; i++)
                        {
                            FinalTest.Add(_tempTestPage[i]);
                        }
                    }
                    isTest = false;
                }
                else
                {
                    outPagesList.Add(page);
                }
            }
            
            outPagesList.Add(FinalTest);
            Display.ExportCollection(FinalTest, "Final_Test");
            PageList = outPagesList;
        }

        public void FormatTest(List<string> _readPage, out List<string> _tempTestPage)
        {
            string _readLine;
            List<string> elementList = new List<string>();
            StringBuilder questionString = new StringBuilder();
            List<string> testList = new List<string>();
            int questionCount = 0;
            char separator = " ".ToCharArray().FirstOrDefault();
            char[] charToTrim = "1 2 3 4 a b c d A B C D . )".ToCharArray();



            //  Order Test Elements
            for (int i = 0; i < _readPage.Count; i++)
            {
                _readLine = _readPage[i];
                foreach (var item in charToTrim)
                {
                    _readLine = _readLine.Trim(item);
                }

                if (_readLine.Contains("QUIZ") || _readLine == "$$$" || _readLine.Contains("Knowledge Check") || _readLine.Count() < 2) continue;
                else{
                    //_readLine = _readLine.Substring(2).Trim();
                    if (_readLine.Contains("?"))
                    {
                        questionCount++;
                        if (questionCount > 1)
                        {
                            foreach (var element in elementList)
                            {
                                questionString.Append(element);
                            }
                            testList.Add(questionString.ToString());
                            elementList = new List<string>();
                        }
                        _readLine = String.Format("QUESTION~Knowledge Check #{0}~{1}", questionCount, _readLine);
                        elementList.Insert(0, _readLine);
                    }
                    //  Add Answer (CORRECT)
                    else if (_readLine.Contains("CORRECT"))
                    {
                        string tempString = _readLine;
                        //if (_readLine.Count() > 3)
                        //{
                        //    //tempString = _readLine.Substring(2).Trim();
                        //}
                        elementList.Insert(1, "~" + _readLine);
                    }
                    //  Add Anser (WRONG)
                    else if (_readLine.ElementAt(1).ToString() == "." || _readLine.ElementAt(1).ToString() == ")")
                    {
                        string tempString = _readLine;
                        //if (_readLine.Count() > 3)
                        //{
                        //     //tempString = _readLine.Substring(2).Trim();
                        //}
                        elementList.Add("~" + tempString);
                    }
                    else if (questionCount == 0) elementList.Insert(0, _readLine);
                    //  cCeck if last
                    if (i == _readPage.Count - 1)
                    {
                        foreach (var element in elementList)
                        {
                            questionString.Append(element);
                        }
                        testList.Add(questionString.ToString());
                        elementList = new List<string>();
                    }
                    if (_readPage.Count - i < 2)
                    {
                        foreach (var element in elementList)
                        {
                            questionString.Append(element);
                        }

                        testList.Add(questionString.ToString());
                        elementList = new List<string>();
                    }
                }
            }
            _tempTestPage = testList;
        }

        //===========================================
        //  SubSort: Separate 'linesList' into Pages
        //  Add 'pageFields' to 'pageList'
        //===========================================

        //public void SortSeparatePages(Parse_Pop _parseObject)
        //{
        //    //List<string> pageFields = new List<string>();
        //    // Separate Pages
        //    for (int j = 0; j < _parseObject.LinesList.Count; j++)
        //    {
        //        //  No Page Flag Detected
        //        if (!_parseObject.LinesList.ElementAt(j).Contains("$$$"))
        //        {
        //            _parseObject.PageFields.Add(_parseObject.LinesList.ElementAt(j));
        //        }
        //        // Page Flag Detected
        //        else if (_parseObject.LinesList.ElementAt(j).Contains("$$$"))
        //        {
        //            //  Add Collected 'pageFields'
        //            _parseObject.PageList.Add(_parseObject.PageFields);
        //            //  Reset 'pageFields' Collection
        //            _parseObject.PageFields = new List<string>();
        //            //  Increment Page Count
        //            _parseObject.PageCount2++;
        //            _parseObject.PageFields.Add("Page Number: " + _parseObject.PageCount2);
        //            _parseObject.PageFields.Add("#");
        //        }
        //    }
        //    // Count Pages
        //    _parseObject.PageCount = _parseObject.PageList.Count();
        //}

        //=================================================
        //  SubSort: Compile Fields of 'pageList' Elements
        //  Add 'compiledFields' to 'compiledPages'
        //=================================================
        public void SortCompileFields(Parse_Pop _parseObject)
        {
            //  Local Variables
            int fieldNumber = -1;
            int pageNumber = 0;
            StringBuilder newField = new StringBuilder();
            List<StringBuilder> compiledFields = new List<StringBuilder>();
            List<StringBuilder> cleanFields = new List<StringBuilder>();

            // Loop Through Pages
            foreach (var page in _parseObject.PageList)
            {
                //  Loop Through Fields of 'page'
                for (int i = 0; i < page.Count(); i++)
                {
                    //  New Field Detected
                    if (page.ElementAt(i) == "#")
                    {
                        newField = new StringBuilder();
                        fieldNumber++;
                    }
                    //  New Field Detected
                    else if (page.ElementAt(i) != "#" && page.ElementAt(i) == page.First())
                    {
                        newField = new StringBuilder();
                        compiledFields.Add(newField.Append(page.ElementAt(i)));
                        fieldNumber++;
                    }
                    //  Start Field
                    else if (page.ElementAt(i) != "#" && page.ElementAt(i - 1) == "#")
                    {
                        compiledFields.Add(newField.Append(page.ElementAt(i)));
                    }
                    //  Cont. Field Detected
                    else if (page.ElementAt(i) != "#" && page.ElementAt(i - 1) != "#" && !page.ElementAt(i).StartsWith("*"))
                    {
                        compiledFields.LastOrDefault().Append(" " + page.ElementAt(i));
                    }
                    //  Cont. Field Detected
                    else if (page.ElementAt(i) != "#" && page.ElementAt(i).StartsWith("*"))
                    {
                        newField = new StringBuilder();
                        compiledFields.Add(newField.Append(page.ElementAt(i)));
                        fieldNumber++;
                    }
                }
                //  Clean Empty Fields/Pages
                if (compiledFields.Count > 1)
                {
                    pageNumber++;
                    StringBuilder pageNumElement = new StringBuilder();
                    pageNumElement.Append("Page Number: " + pageNumber.ToString());
                    compiledFields[0] = pageNumElement;
                    _parseObject.CompiledPages.Add(compiledFields);
                }

                //  Reset Local Variables
                compiledFields = new List<StringBuilder>();
                newField = new StringBuilder();
                fieldNumber = -1;
            }
        }

        //=======================================================
        //  SubSort: Clean Up Sort (empty pages, fields, etc..)
        //=======================================================
        public void SortCleanUp(Parse_Pop _parseObject)
        {
            foreach (var page in _parseObject.CompiledPages)
            {
                if (page.Count > 0)
                {
                    _parseObject.SortedPages.Add(page);
                }
            }
        }
    }
}
