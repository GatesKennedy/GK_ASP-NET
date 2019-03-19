using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace LMS_Recovery_Console
{
    public static class Display
    {
        //  Isolate First Word In Line
        public static string IsoFirstWords(string _readString, int wordIndex)
        {
            if (_readString != "")
            {
                try
                {
                    char space = " ".ToCharArray().FirstOrDefault();
                    string[] wordArray = _readString.Split(space);
                    int wordCount = wordArray.Count();
                    int indexLimit;
                    string tempString = wordArray[0];

                    if (wordIndex < wordCount) indexLimit = wordIndex;
                    else indexLimit = wordCount;
                    
                    for (int i = 1; i < indexLimit; i++)
                    {
                        string word = wordArray[i];
                        tempString = tempString + word;
                    }
                    
                    return _readString;
                }
                catch (Exception)
                {
                    _readString = _readString.Split(' ').FirstOrDefault();
                    throw;
                }

            }
            else return "^^^";
        }

        //  Displays Console: Full List
        public static void ToConsole(List<string> _targetList)
        {
            foreach (var line in _targetList) Console.WriteLine(line);
        }
        public static void ToConsole(List<int> _targetList)
        {
            foreach (var line in _targetList)
                Console.WriteLine(line);
        }

        //  Displays Console: Object Properties 
        public static void ObjPropToConsole(Parse_Pop _targetObj)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(_targetObj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(_targetObj);

                Console.WriteLine("{0} = {1}", name, value);
            }
        }

        //  Displays Console: Object List Metrics

        public static void MetricsToConsole(Parse_Pop _targetObj)
        {
            List<int> tempList = new List<int>
            {
                _targetObj.LinesList.Count(),
                _targetObj.PageList.Count(),
                _targetObj.LinesList.Count(),
                _targetObj.PageBreaks.Count()
            };

            ToConsole(tempList);
        }
        public static void MetricsToConsole(List<string> _targetList, string _listName)
        {

                Console.WriteLine(String.Format("{0} = {1}", _listName, _targetList.Count()));

            
        }

        // Display Console: Format Line Check
        public static void FormatLinesCheck_Scroll(int i, int j, List<string> TarList, List<string> TxtList)
        {
            // Number of words to be display from each Line 
            int wordsDisplayed = 5;

            try
            {
                for (int k = -2; k < 3; k++)
                {
                    if (k == 0)
                    {
                        Console.WriteLine(String.Format(
                            "````````````````````\n" +
                            "HTML {0}: {1}\n" +
                            " TXT {2}: {3}\n\n" +
                            "````````````````````", j + k, IsoFirstWords(TarList[j + k], wordsDisplayed), i + k, IsoFirstWords(TxtList[i + k], wordsDisplayed)));
                    }
                    else
                    {
                        Console.WriteLine(String.Format(
                            "HTML {0}: {1}\n" +
                            " TXT {2}: {3}\n" +
                            "", j + k, IsoFirstWords(TarList[j + k], wordsDisplayed), i + k, IsoFirstWords(TxtList[i + k], wordsDisplayed)));
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine(String.Format(
                    "HTML {0}: {1}\n" +
                    " TXT {2}: {3}\n" +
                    "", j, TarList[j], i, TxtList[i]));
            }
        }
        public static void FormatLinesCheck_Add(int i, int j, List<string> TarList, List<string> TxtList)
        {
            // Number of words to be display from each Line 
            int wordsDisplayed = 5;

            for (int k = -2; k < 3; k++)
            {
                try
                {

                    if (k == 0)
                    {
                        Console.WriteLine(String.Format(
                            "````````````````````\n" +
                            "HTML {0}: {1}\n" +
                            " TXT {2}: {3}\n\n" +
                            "````````````````````", j + k, IsoFirstWords(TarList[j + k], wordsDisplayed), i + k, IsoFirstWords(TxtList[i + k], wordsDisplayed)));
                    }
                    else
                    {
                        Console.WriteLine(String.Format(
                            "HTML {0}: {1}\n" +
                            " TXT {2}: {3}\n" +
                            "", j + k, IsoFirstWords(TarList[j + k], wordsDisplayed), i + k, IsoFirstWords(TxtList[i + k], wordsDisplayed)));
                    }


                }
                catch (Exception)
                {
                    Console.WriteLine(String.Format(
                        "HTML {0}: {1}\n" +
                        " TXT {2}: {3}\n" +
                        "", j , IsoFirstWords(TarList[j], wordsDisplayed), i , IsoFirstWords(TxtList[i], wordsDisplayed)));
                }
            }
        }


        //=====================================================

        //  Compare List Lengths
        public static void CompareLists(List<string> _targetList, List<string> _compareList)
        {
            Console.WriteLine("COMPARE");
            Console.WriteLine(String.Format(" Target: {0} \n Compare: {1}",_targetList.Count(), _compareList.Count() ));
        }

        //  Displays Output Window and returns 
        public static List<List<StringBuilder>> Display_ToOutput(Parse_Pop _parseObj)
        {
            System.Diagnostics.Debug.WriteLine("Compiled PAGE COUNT: " + _parseObj.CompiledPages.Count());
            System.Diagnostics.Debug.WriteLine("List PAGE COUNT2: " + _parseObj.PageCount2);

            bool printCompare = false;
            bool printSection = false;
            bool printCompiled = true;

            //=========================================================
            //      Compare Single Pages (A and B)
            //=========================================================
            if (printCompare)
            {
                //  Print Page A
                System.Diagnostics.Debug.WriteLine("$$$$$$$$  PAGE A  $$$$$$$$$");
                int pageA = 786;
                int pageB = 787;

                //  Uncompiled Page A
                foreach (var line in _parseObj.PageList.ElementAt(pageA))
                {
                    System.Diagnostics.Debug.WriteLine(line);
                }
                //  Compiled Page A
                foreach (var field in _parseObj.CompiledPages.ElementAt(pageA))
                {
                    System.Diagnostics.Debug.WriteLine(field);
                }
                // Print Page B
                System.Diagnostics.Debug.WriteLine("$$$$$$$$  PAGE B  $$$$$$%$%$%$$");
                // Uncompiled Page B
                foreach (var line in _parseObj.PageList.ElementAt(pageB))
                {
                    System.Diagnostics.Debug.WriteLine(line);
                }
                //  Compiled Page B
                foreach (var field in _parseObj.CompiledPages.ElementAt(pageB))
                {
                    System.Diagnostics.Debug.WriteLine(field);
                }
                System.Diagnostics.Debug.WriteLine("$$$$$$$$$$  END SINGLE PAGE PRINT  $$$$$$$$$$");
            }

            //=========================================================
            //      Print Uncompiled Section of Pages (A-B)
            //=========================================================
            if (printSection)
            {
                int pageA = 783;
                int pageB = 788;
                int pageN = pageA;

                while (pageN <= pageB)
                {
                    System.Diagnostics.Debug.WriteLine("===============================");
                    foreach (var field in _parseObj.CompiledPages.ElementAt(pageN))
                    {
                        System.Diagnostics.Debug.WriteLine(field);
                    }
                    pageN++;
                }
                System.Diagnostics.Debug.WriteLine("$$$$$$$$$$  END SECTION PRINT  $$$$$$$$$$");
            }

            //=========================================================
            //      Print All Compiled Pages
            //=========================================================
            if (printCompiled)
            {
                //System.IO.File.Create(@"C:\Users\gates\OneDrive\Desktop");
                using (StreamWriter resultFile = new StreamWriter(@"C:\Users\gates\OneDrive\Desktop\CoolResult."))
                foreach (var page in _parseObj.SortedPages)
                {
                    System.Diagnostics.Debug.WriteLine("=============================");
                    foreach (var field in page)
                    {
                            resultFile.WriteLine(field.ToString());
                            System.Diagnostics.Debug.WriteLine(field);
                    }
                }
                System.Diagnostics.Debug.WriteLine("$$$$$$$$ END OF DISPLAY() $$$$$$%$%$%$$");
            }
            return _parseObj.SortedPages;
        }
        

        //=========================================================
        //      EXPORT
        //=========================================================
        //  Creates .txt document named "docName"

        public static void ExportCollection(List<List<StringBuilder>> _parsedCollection, string docName)
        {
            using (StreamWriter resultFile = new StreamWriter(@"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\Results\" + docName + ".txt", false))
                
                foreach (var page in _parsedCollection)
                {
                    System.Diagnostics.Debug.WriteLine("=============================");
                    foreach (var field in page)
                    {
                        resultFile.WriteLine(field.ToString());
                        System.Diagnostics.Debug.WriteLine(field);
                    }
                }
            
            System.Diagnostics.Debug.WriteLine("%%%%%%% END OF DISPLAY() %%%%%%%");
        }

        //export with pageCount
        public static void ExportCollection(List<List<List<string>>> _parsedCollection, string docName)
        {
            int moduleCount = 0;
            int pageCount = 0;
            using (StreamWriter resultFile = new StreamWriter(@"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\Results\" + docName + ".txt", false))

                foreach (var module in _parsedCollection)
                {
                    moduleCount++;
                    resultFile.WriteLine("======  MODULE " + moduleCount + "  ======");
                    foreach (var page in module)
                    {
                        pageCount++;
                        //System.Diagnostics.Debug.WriteLine("=============================");
                        resultFile.WriteLine("======  page " + pageCount + "  ======");
                        foreach (var field in page)
                        {
                            resultFile.WriteLine(field.ToString());
                            System.Diagnostics.Debug.WriteLine(field);
                        }
                    }

                }
            System.Diagnostics.Debug.WriteLine("%%%%%%% END OF DISPLAY() %%%%%%%");
        }
        public static void ExportCollection(List<List<string>> _parsedCollection, string docName)
        {
            int pageCount = 0;
            using (StreamWriter resultFile = new StreamWriter(@"C:\Programming\GK_ASP-NET\LMS_Recovery_Console\Results\" + docName + ".txt", false))

                foreach (var page in _parsedCollection)
                {
                    pageCount++;
                    //System.Diagnostics.Debug.WriteLine("=============================");
                    resultFile.WriteLine("======  page " + pageCount + "  ======");
                    foreach (var field in page)
                    {
                        resultFile.WriteLine(field.ToString());
                        System.Diagnostics.Debug.WriteLine(field);
                    }
                }
            System.Diagnostics.Debug.WriteLine("%%%%%%% END OF DISPLAY() %%%%%%%");
        }
        public static void ExportCollection(List<string> _parsedCollection, string docName)
        {
            string writeString = @"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\Results\" + docName + ".txt";
            using (StreamWriter resultFile = new StreamWriter(writeString, false))
            {
                foreach (var field in _parsedCollection)
                {
                    resultFile.WriteLine(field.ToString());
                    System.Diagnostics.Debug.WriteLine(field);
                }
            }
            System.Diagnostics.Debug.WriteLine("%%%%%%% END OF DISPLAY() %%%%%%%");
        }

        //  Export  DICTIONARY   Creates .txt doc named "docName"
        public static void ExportCollection(Dictionary<int,string> _readDictionary, string docName)
        {
            string writeString = String.Format(@"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\{0}.txt", docName);
            using(StreamWriter resultFile = new StreamWriter(writeString, false))
            {
                foreach (var field in _readDictionary)
                {
                    int key = field.Key;
                    string value = field.Value;
                    string comboString = String.Format("KeyLine: {0} | {1}", key, value);
                    resultFile.WriteLine(comboString);
                }
            }
        }

        //  LIVE Export
        //  LiveSplit
        public static string RouteSplit(string _readString, out string siphonString)
        {
            siphonString = _readString;
            return _readString;
        }
        public static void RouteSiphon(string siphonString)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\aolcc_versioncontrol_html\LiveExport1.txt")) sw.WriteLine(siphonString);
        }
        public static void RouteBinClear()
        {
            string path = @"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\results\LiveExport1.txt";
            File.WriteAllText(path, String.Empty);
            File.Create(path).Close();
        }

        //  COMPARE Collection Count
        public static void CompareCollection(Dictionary<int,string> _trustDict, Dictionary<int,string> _untrustDict)
        {
            string compareString = String.Format("Trusted Dictionary Count: {0} /n Untrusted Dictionary Count: {1}", _trustDict.Count(), _untrustDict.Count());
            Console.WriteLine(compareString);
            System.Diagnostics.Debug.WriteLine(compareString);
        }

    }
}
