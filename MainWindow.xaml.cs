// DATA PIPELINE TOOL @ GENER8
// UDI BAHATT

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace DataPipeline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    ///
    /// Multitude of commands used to interact with the Data Transfer Tool.
    /// </summary>

    //                                                  |>>>
    //                                                  |
    //                                               _ _|_ _
    //                                             |;|_|;|_|;|
    //                                             \\.    .  /
    //                                              \\:  .  /
    //                                               ||:   |
    //                                               ||:.  |
    //                                               ||:  .|
    //                                               ||:   |       \,/
    //                                               ||: , |            /`\
    //                                               ||:   |
    //                                               ||: . |
    //                __                            _||_   |
    //       ____--`~    '--~~__            __ ----~    ~`---,              ___
    //  -~--~                   ~---__ ,--~'                  ~~----_____-~'   `~----~~

    public partial class MainWindow : Window
    {
        private string parent = "D:\\Testfolder";
        private string child = "D:\\Testfolder2";
        private ObservableCollection<MyModel> lt = new ObservableCollection<MyModel>();
        private MyModel myModel;

        // THIS JUST MAKES THE WINDOW EXIST, IT'S PRETTY RAD NGL
        public MainWindow()
        {
            InitializeComponent();
        }

        // SELECT FILE PATH USES THIS
        // CHOOSES A CHILD FOLDER SUCH THAT WE CAN TEST THE TRANSFER OF FILES FROM ONE FOLDER TO THE OTHER
        private void Button_Click_ChildFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                file_receiver.Text = openFileDlg.SelectedPath;
            }
            child = file_receiver.Text;
        }

        // OPEN PARENT FOLDER USES THIS
        // CHOOSES A FILE TO OPEN AND SEE FILES WITHIN THAT ONE
        private void Button_Open_ParentFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                txtPath.Text = openFileDlg.SelectedPath;
            }
            parent = txtPath.Text;
        }

        // DISPLAYS FOLDER CONTENTS ON THE TEXTBOX RIGHTSIDE OF SCREEN
        // YOU CAN SPECIFY FILE TYPE, SEE COMMENT BLOCK BELOW
        private void Button_DisplayFolderContent(object sender, RoutedEventArgs e)
        {
            if (parent == "D:\\Testfolder")
            {
                MessageBox.Show("Please select a Parent Folder prior to displaying files.", "Notification");
            }
            else
            {
                lt = new ObservableCollection<MyModel>();

                /*
                    ATTENTION, IN ORDER TO DETERMINE WHAT KIND OF FILE YOU WANT TO LOOK FOR, JUST CHANGE THE EXTENSION YOU SEARCH FOR
                    IN THE LINE BELOW THE BLOCK COMMENT. FOR EXAMPLE, WE ARE LOOKING FOR .CSV SO WE ENTER "*.CSV" BUT IF WE WANTED
                    TO LOOK FOR .TXT WE ENTER "*.TXT" RATHER THAN "*.CSV".

                    ALTERNATIVELY, JUST UNCOMMENT LINE THE COMMENTED LINE BELOW THE STRING[] AND COMMENT OUT THE ONE ON TOP
                */

                // This lets us see all the files within the folder rather than a certain type of file
                // This one below lets you only display files of a certain type, and if you want to change that type just change .txt to .csv or whatever
                // you need to be able to see.
                // string[] dicFileList = Directory.GetFiles(parent, "*.txt", SearchOption.AllDirectories);
                string[] dicFileList = Directory.GetFiles(parent);

                foreach (string element in dicFileList)
                {
                    myModel = new MyModel();

                    // These 2 lines determine whether you want to include the file type extension in the display window.
                    // Just comment out one or the other in order to display or not display extensions.
                    myModel.Name = System.IO.Path.GetFileName(element);
                    // myModel.Name = System.IO.Path.GetFileNameWithoutExtension(element);
                    myModel.StatusForCheckBox = false;

                    lt.Add(myModel);
                }
                myList.ItemsSource = lt;
            }
        }

        // IM NOT SURE WHAT THIS FUNCTION IS DOING, BUT THE APP DOES NOT RUN WITHOUT IT
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        // FORMATTING FUNCTION
        // TAKES IN FILES FROM PARENT FOLDER, FORMATS THEM ACCORDINGLY WITH METADATA, THEN DUMPS INTO ONE MASTER FILE INTO CHILD FOLDER
        private void Button_FormatAndTransfer(object sender, RoutedEventArgs e)
        {
            /*
            // VARIABLES ALLOWING FOR USER DEFINED FILENAMES
            var newFileName = "";
            newFileName = fileNameTextBox.Text;
            int lineSkip = 0;

            // MAIN LOOP FOR THE FUNCTION

            // THROWS USERS A MESSAGE INCASE CERTAIN FIELDS ARE MISSING IN WINDOW
            if (parent == "D:\\Testfolder" || child == "D:\\Testfolder2" || newFileName == "" || String.IsNullOrEmpty(headerLinesTextBox.Text))
            {
                MessageBox.Show("Please select both a Parent and Child Folder AND enter both the number of header lines a name for your master file prior to formatting data.", "Notification");
            }
            // MAIN LOOP ITSELF
            else
            {
                // VARIABLES - ASSIGN FILE PATH FROM INPUT BASED ON PRIOR FUNCTIONS
                string path = parent;
                string[] files = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories);
                CultureInfo provider = CultureInfo.InvariantCulture;

                // MOVED THIS LINE FROM 269 TO HERE DUE TO FORMATTING
                // LINE TAKES IN INPUT FROM HEADER LINE NUMBER LINE AND TURNS IT INTO INT TO BE USED LATER
                lineSkip = Convert.ToInt32(headerLinesTextBox.Text);

                foreach (string s in files)
                {
                    // EXTRACTING DATA FROM FILES
                    string fileName = Path.GetFileName(s);
                    string date_default = "N/A";
                    var date_input = "N/A";
                    var firm_A_default = "N/A";
                    var firm_A_input = "N/A";
                    var firm_B_default = "N/A";
                    var firm_B_input = "N/A";
                    var firm_C_default = "N/A";
                    var firm_C_input = "N/A";

                    // AUTO HEADER CALCULATION WORKS !
                    //string[] lineArray = File.ReadAllLines(s);
                    //int t = 0;
                    //bool tf;
                    //do
                    //{
                    //    tf = String.Equals(lineArray[t].Substring(0, 10), "Time (sec)");
                    //    lineSkip++;
                    //    t++;
                    //} while (tf == false);
                    //lineSkip = lineSkip - 1;
                    //MessageBox.Show(lineSkip.ToString(), "Notification");

                    // GRABBING DATE DATA FROM THE FILE
                    var line1 = File.ReadLines(s).First();
                    string[] line_number = File.ReadAllLines(s);
                    string fileCreationTime = File.GetCreationTime(s).ToString();

                    // INCREMENTS AT 1 NOT 0 ?????
                    // HANDLES IF COMPILE DATE ACTUALLY EXISTS OR NOT
                    if (String.Equals(line1.Substring(0, 10), "Time (sec)"))
                    {
                        date_input = date_default;
                    }
                    else
                    {
                        line1 = line1.Replace("GUI Compile Date: ", "");
                        DateTime date = DateTime.Parse(line1);
                        date_input = date.ToString();
                    }

                    // MAKE IT SO METADATA IS NOT APPENDED TO HEADER LINES
                    var csv = File.ReadLines(s) // not AllLines
                    .Select((line, index) => index == lineSkip
                        // ONCE ALL METADATA IS IMPLEMENTED ORDER WITHIN THE CSV CAN BE CHANGED BY CHANGING THE ORDER IN WHICH THEY APPEAR BELOW
                        ? line + "File_Name" + ",File_Creation_Date" + ",GUI_Compile_Date" + ",Firmware_Header_A" + ",Firmware_Header_B" + ",Firmware_Header_C" + ","
                        : line + fileName + "," + fileCreationTime + "," + date_input + "," + firm_A_input + ","
                               + firm_B_input + "," + firm_C_input + ",") // REMOVE BLOCK COMMENT ONCE METADATA IS SECURED
                    .ToList(); // we should write into the same file, that´s why we materialize

                    File.WriteAllLines(s, csv);

                    // CAN COMMENT THIS LINE BELOW OUT IF NEED BE
                    //lineSkip = 0;
                }

                // NOTIFICATION ALLOWING USER TO SEE IF SOMETHING WAS DONE WITHOUT THE PROGRAM BREAKING
                MessageBox.Show("Data Formatting Complete!", "Notification");

                // FILE COMBINATION CODE
                string sourceFolder = parent;
                string destinationFile = child + "\\" + newFileName + ".csv";

                // Specify wildcard search to match CSV files that will be combined
                string[] filePaths = Directory.GetFiles(sourceFolder);
                StreamWriter fileDest = new StreamWriter(destinationFile, true);

                int i;
                for (i = 0; i < filePaths.Length; i++)
                {
                    string file = filePaths[i];
                    string[] lines = File.ReadAllLines(file);

                    // REMOVES HEADER DATA POST PROCESS FOR FIRST FILE
                    if (i == 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkip).ToArray(); // Skip header row for first file
                    }

                    // REMOVES HEADER DATA POST PROCESS FOR ALL FILES AFTER THE FIRST ONE
                    if (i > 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkip + 1).ToArray(); // Skip header row for all but first file
                    }

                    foreach (string line in lines)
                    {
                        fileDest.WriteLine(line);
                    }
                }
                fileDest.Close();

                MessageBox.Show("Files have been combined!", "Notification");
            }
            */
        }

        // TESTING FUNCTION - SOMEWHAT REFINED? GONNA ASK ABOUT THIS LATER
        private void Button_Format_Better(object sender, RoutedEventArgs e)
        {
            // VARIABLES ALLOWING FOR USER DEFINED FILENAMES
            var newFileName = "";
            newFileName = fileNameTextBox.Text;
            int lineSkip = 0;

            // MAIN LOOP FOR THE FUNCTION
            // THROWS USERS A MESSAGE INCASE CERTAIN FIELDS ARE MISSING IN WINDOW
            if (parent == "D:\\Testfolder" || child == "D:\\Testfolder2" || newFileName == "")
            {
                MessageBox.Show("Please select both a Parent and Child Folder AND enter both a name for your master file prior to formatting data.", "Notification");
            }
            // MAIN LOOP ITSELF
            else
            {
                // VARIABLES - ASSIGN FILE PATH FROM INPUT BASED ON PRIOR FUNCTIONS
                int lineSkipNum = 0;
                string path = parent;
                int[] lineSkipArray = new int[99];
                CultureInfo provider = CultureInfo.InvariantCulture;
                string[] files = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories);

                // THIS LOOP SHOULD COUNT THE AMOUNT OF LINES, ASSIGN IT TO LINESKIPARRAY PER T, THEN RESET LINESKIP TO 0 FOR THE NEXT FILE
                foreach (string s in files)
                {
                    bool tf;
                    string[] lineArray = File.ReadAllLines(s);
                    int arrayNum = 0;
                    int t = 0;

                    // MAIN LOOP FOR CALCULATING LINESKIP
                    do
                    {
                        tf = String.Equals(lineArray[t].Substring(0, 10), "Time (sec)");
                        lineSkip++;
                        t++;
                    } while (tf == false);

                    // INCREMENTING LINESKIP BY -1 TO SET IT UP CORRECTLY
                    lineSkip = lineSkip - 1;
                    lineSkipArray[arrayNum] = lineSkip;

                    // TESTING MESSAGE BOX
                    //MessageBox.Show(lineSkipArray[arrayNum].ToString(), "Notification");

                    // RESET LINESKIP TO 0 FOR EACH FILE IN FILES (AN ARRAY OF STRINGS INCLUDING THE NAME OF EACH FILE IN PARENT)
                    lineSkip = 0;
                    arrayNum++;
                }

                // NEW FODLER CREATION FOR HOLDING TEST DATA
                //DateTime currentDate = DateTime.Today;
                //var fileDate = currentDate.ToString();

                //string folderName = parent;
                //string pathstring = System.IO.Path.Combine(folderName, $"Formatted Data {fileDate}");
                //System.IO.Directory.CreateDirectory()

                foreach (string s in files)
                {
                    //int[] lineSkipNum = new int[99];

                    // EXTRACTING DATA FROM FILES
                    string fileName = Path.GetFileName(s);
                    string date_default = "N/A";
                    var date_input = "N/A";
                    //var firm_A_default = "N/A";
                    var firm_A_input = "N/A";
                    //var firm_B_default = "N/A";
                    var firm_B_input = "N/A";
                    //var firm_C_default = "N/A";
                    var firm_C_input = "N/A";

                    // GRABBING DATE DATA FROM THE FILE
                    var line1 = File.ReadLines(s).First();
                    string[] line_number = File.ReadAllLines(s);
                    string fileCreationTime = File.GetCreationTime(s).ToString();

                    // INCREMENTS AT 1 NOT 0 ?????
                    // HANDLES IF COMPILE DATE ACTUALLY EXISTS OR NOT
                    if (String.Equals(line1.Substring(0, 10), "Time (sec)"))
                    {
                        date_input = date_default;
                    }
                    else
                    {
                        line1 = line1.Replace("GUI Compile Date: ", "");
                        DateTime date = DateTime.Parse(line1);
                        date_input = date.ToString();
                    }

                    // MAKE IT SO METADATA IS NOT APPENDED TO HEADER LINES
                    var csv = File.ReadLines(s) // not AllLines
                    .Select((line, index) => index == lineSkipArray[lineSkipNum]
                        // ONCE ALL METADATA IS IMPLEMENTED ORDER WITHIN THE CSV CAN BE CHANGED BY CHANGING THE ORDER IN WHICH THEY APPEAR BELOW
                        ? line + "File_Name" + ",File_Creation_Date" + ",GUI_Compile_Date" + ",Firmware_Header_A" + ",Firmware_Header_B" + ",Firmware_Header_C" + ","
                        : line + fileName + "," + fileCreationTime + "," + date_input + "," + firm_A_input + ","
                               + firm_B_input + "," + firm_C_input + ",") // REMOVE BLOCK COMMENT ONCE METADATA IS SECURED
                    .ToList(); // we should write into the same file, that´s why we materialize

                    // EDITING FUNCTION LOCATION GOES HERE, FIX THIS SOON FOR UPCOMING TESTING
                    File.WriteAllLines(s, csv);

                    // CAN COMMENT THIS LINE BELOW OUT IF NEED BE
                    //lineSkip = 0;
                    lineSkipNum++;
                }

                // NOTIFICATION ALLOWING USER TO SEE IF SOMETHING WAS DONE WITHOUT THE PROGRAM BREAKING
                MessageBox.Show("Data Formatting Complete!", "Notification");

                // FILE COMBINATION CODE
                string sourceFolder = parent;
                string destinationFile = child + "\\" + newFileName + ".csv";

                // Specify wildcard search to match CSV files that will be combined
                string[] filePaths = Directory.GetFiles(sourceFolder);
                StreamWriter fileDest = new StreamWriter(destinationFile, true);
                //int lineSkipHeader = 0;
                //lineSkipArray[0] = lineSkipHeader;
                int i;
                for (i = 0; i < filePaths.Length; i++)
                {
                    string file = filePaths[i];
                    string[] lines = File.ReadAllLines(file);

                    // REMOVES METADATA LINES POST PROCESS FOR FIRST FILE
                    if (i == 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkipArray[i]).ToArray(); // Skip header row for first file
                    }

                    // REMOVES METADATA LINES AND HEADER DATA POST PROCESS FOR ALL FILES EXCLUDING THE FIRST
                    if (i > 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkipArray[i] + 1).ToArray(); // Skip header row for all but first file
                    }

                    foreach (string line in lines)
                    {
                        fileDest.WriteLine(line);
                    }
                }
                fileDest.Close();

                MessageBox.Show("Files have been combined!", "Notification");
            }
        }

        // TESTING FUNCTION - SOMEWHAT REFINED? GONNA ASK ABOUT THIS LATER
        private void Button_Format_Better_2(object sender, RoutedEventArgs e)
        {
            // VARIABLES ALLOWING FOR USER DEFINED FILENAMES
            var newFileName = "";
            newFileName = fileNameTextBox.Text;
            int lineSkip = 0;

            // MAIN LOOP FOR THE FUNCTION
            // THROWS USERS A MESSAGE INCASE CERTAIN FIELDS ARE MISSING IN WINDOW
            if (parent == "D:\\Testfolder" /*|| child == "D:\\Testfolder2"*/ || newFileName == "")
            {
                MessageBox.Show("Please select a Parent Folder AND enter a name for your master file prior to formatting data.", "Notification");
            }
            // MAIN LOOP ITSELF
            else
            {
                // VARIABLES - ASSIGN FILE PATH FROM INPUT BASED ON PRIOR FUNCTIONS
                int lineSkipNum = 0;
                string path = parent;
                int[] lineSkipArray = new int[99];
                CultureInfo provider = CultureInfo.InvariantCulture;

                // NEW FODLER CREATION FOR HOLDING TEST DATA
                DateTime currentDate = DateTime.Today;
                var fileDate = currentDate.ToString();

                // Specify a name for your top-level folder.
                string folderName = @parent;

                // To create a string that specifies the path to a subfolder under your
                // top-level folder, add a name for the subfolder to folderName.
                var pathString = System.IO.Path.Combine(folderName, $"Edited Data {currentDate}");

                // You can extend the depth of your path if you want to.
                //pathString = System.IO.Path.Combine(pathString, "SubSubFolder");
                //MessageBox.Show(pathString, "Notification");
                var newPath = pathString.Replace("/", "-");
                newPath = newPath.Replace(" ", "_");
                newPath = newPath.Replace("_12:00:00_AM", "");
                //MessageBox.Show(newPath, "Notification");

                // Create the subfolder. You can verify in File Explorer that you have this
                // structure in the C: drive.
                //    Local Disk (C:)
                //        Top-Level Folder
                //            SubFolder
                System.IO.Directory.CreateDirectory(newPath);

                // COPYING FILES FROM PARENT TO FORMATTED DATA DATE FOLDER
                string sourcePath = parent;
                string targetPath = newPath;
                string copyFileName = string.Empty;
                string destFile = string.Empty;

                // To copy all the files in one directory to another directory. 
                // Get the files in the source folder. (To recursively iterate through 
                // all subfolders under the current directory, see 
                // "How to: Iterate Through a Directory Tree.")
                // Note: Check for target path was performed previously 
                //       in this code example. 
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] copyFiles = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in copyFiles)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        copyFileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, copyFileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }

                string[] files = Directory.GetFiles(newPath, "*.csv", SearchOption.AllDirectories);

                // THIS LOOP SHOULD COUNT THE AMOUNT OF LINES, ASSIGN IT TO LINESKIPARRAY PER T, THEN RESET LINESKIP TO 0 FOR THE NEXT FILE
                foreach (string s in files)
                {
                    bool tf;
                    string[] lineArray = File.ReadAllLines(s);
                    int arrayNum = 0;
                    int t = 0;

                    // MAIN LOOP FOR CALCULATING LINESKIP
                    do
                    {
                        tf = String.Equals(lineArray[t].Substring(0, 10), "Time (sec)");
                        lineSkip++;
                        t++;
                    } while (tf == false);

                    // INCREMENTING LINESKIP BY -1 TO SET IT UP CORRECTLY
                    lineSkip = lineSkip - 1;
                    lineSkipArray[arrayNum] = lineSkip;

                    // TESTING MESSAGE BOX
                    //MessageBox.Show(lineSkipArray[arrayNum].ToString(), "Notification");

                    // RESET LINESKIP TO 0 FOR EACH FILE IN FILES (AN ARRAY OF STRINGS INCLUDING THE NAME OF EACH FILE IN PARENT)
                    lineSkip = 0;
                    arrayNum++;
                }

                foreach (string s in files)
                {
                    //int[] lineSkipNum = new int[99];

                    // EXTRACTING DATA FROM FILES
                    string fileName = Path.GetFileName(s);
                    string date_default = "N/A";
                    var date_input = "N/A";
                    //var firm_A_default = "N/A";
                    var firm_A_input = "N/A";
                    //var firm_B_default = "N/A";
                    var firm_B_input = "N/A";
                    //var firm_C_default = "N/A";
                    var firm_C_input = "N/A";

                    // GRABBING DATE DATA FROM THE FILE
                    var line1 = File.ReadLines(s).First();
                    string[] line_number = File.ReadAllLines(s);
                    string fileCreationTime = File.GetCreationTime(s).ToString();

                    // INCREMENTS AT 1 NOT 0 ?????
                    // HANDLES IF COMPILE DATE ACTUALLY EXISTS OR NOT
                    if (String.Equals(line1.Substring(0, 10), "Time (sec)"))
                    {
                        date_input = date_default;
                    }
                    else
                    {
                        line1 = line1.Replace("GUI Compile Date: ", "");
                        DateTime date = DateTime.Parse(line1);
                        date_input = date.ToString();
                    }

                    // MAKE IT SO METADATA IS NOT APPENDED TO HEADER LINES
                    var csv = File.ReadLines(s) // not AllLines
                    .Select((line, index) => index == lineSkipArray[lineSkipNum]
                        // ONCE ALL METADATA IS IMPLEMENTED ORDER WITHIN THE CSV CAN BE CHANGED BY CHANGING THE ORDER IN WHICH THEY APPEAR BELOW
                        ? line + "File_Name" + ",File_Creation_Date" + ",GUI_Compile_Date" + ",Firmware_Header_A" + ",Firmware_Header_B" + ",Firmware_Header_C" + ","
                        : line + fileName + "," + fileCreationTime + "," + date_input + "," + firm_A_input + ","
                               + firm_B_input + "," + firm_C_input + ",") // REMOVE BLOCK COMMENT ONCE METADATA IS SECURED
                    .ToList(); // we should write into the same file, that´s why we materialize

                    // EDITING FUNCTION LOCATION GOES HERE, FIX THIS SOON FOR UPCOMING TESTING
                    File.WriteAllLines(s, csv);

                    // CAN COMMENT THIS LINE BELOW OUT IF NEED BE
                    //lineSkip = 0;
                    lineSkipNum++;
                }

                // NOTIFICATION ALLOWING USER TO SEE IF SOMETHING WAS DONE WITHOUT THE PROGRAM BREAKING
                MessageBox.Show("Data Formatting Complete!", "Notification");

                // FILE COMBINATION CODE
                string sourceFolder = newPath;
                string destinationFile = newPath + "\\" + newFileName + ".csv";

                // Specify wildcard search to match CSV files that will be combined
                string[] filePaths = Directory.GetFiles(sourceFolder);
                StreamWriter fileDest = new StreamWriter(destinationFile, true);
                //int lineSkipHeader = 0;
                //lineSkipArray[0] = lineSkipHeader;
                int i;
                for (i = 0; i < filePaths.Length; i++)
                {
                    string file = filePaths[i];
                    string[] lines = File.ReadAllLines(file);

                    // REMOVES METADATA LINES POST PROCESS FOR FIRST FILE
                    if (i == 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkipArray[i]).ToArray(); // Skip header row for first file
                    }

                    // REMOVES METADATA LINES AND HEADER DATA POST PROCESS FOR ALL FILES EXCLUDING THE FIRST
                    if (i > 0)
                    {
                        // STANDARD
                        lines = lines.Skip(lineSkipArray[i] + 1).ToArray(); // Skip header row for all but first file
                    }

                    foreach (string line in lines)
                    {
                        fileDest.WriteLine(line);
                    }
                }
                fileDest.Close();

                MessageBox.Show("Files have been combined!", "Notification");
            }
        }

        // DEPRECATED FUNCTIONS BELOW

        private void Button_Transfer(object sender, RoutedEventArgs e)
        {
            string fileName = "test.txt";
            string sourcePath = parent;
            string targetPath = child;
            int file_count = 0;

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                    file_count++;
                }

                // Change all filenames in the child folder to their lowercase variants
                // NEED TO FIGURE OUT HOW TO CHANGE SPACES TO UNDERSCORE, MAYBE USE .REPLACE?
                foreach (var file in Directory.GetFiles(targetPath))
                {
                    //string newName = file.Name.Replace
                    File.Move(file, file.ToLowerInvariant());
                    //Path.Combine(targetPath, file.fileName.Replace(" ", "_"));
                }
            }

            // Error message incase invalid input for sourcePath
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

            MessageBox.Show($"{file_count} files have been transferred.", "Notification");
        }

        private void Button_Transfer2(object sender, RoutedEventArgs e)
        {
            // FILE COMBINATION CODE
            string sourceFolder = parent;
            string destinationFile = @"C:\Users\ubahatt\Desktop\test_data_folder\test_csv_combo.csv";

            // Specify wildcard search to match CSV files that will be combined
            string[] filePaths = Directory.GetFiles(sourceFolder);
            StreamWriter fileDest = new StreamWriter(destinationFile, true);

            int i;
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                string[] lines = File.ReadAllLines(file);

                if (i > 0)
                {
                    lines = lines.Skip(1).ToArray(); // Skip header row for all but first file
                }

                foreach (string line in lines)
                {
                    fileDest.WriteLine(line);
                }
            }
            fileDest.Close();

            MessageBox.Show("Files Combined!", "Notification");
        }

        /*
     // DEPRECATED
     // DELETE THIS IN THE FUTURE
     private void Save_Click(object sender, RoutedEventArgs e)
     {
         List<string> ltForSave = new List<string>();
         foreach (MyModel obj in myList.ItemsSource)
         {
             if (obj.StatusForCheckBox == false)
             {
                 ltForSave.Add(";" + obj.Name);
             }
             else
             {
                 ltForSave.Add(obj.Name);
             }
         }
         //Save ltForSave for ini file.
     }

     // DEPRECATED
     // DELETE LATER
     private void Button_Click_1(object sender, RoutedEventArgs e)
     {
         // Create OpenFileDialog
         Microsoft.Win32.OpenFileDialog Search = new Microsoft.Win32.OpenFileDialog();

         // Set filter for file extension and default file extension
         Search.DefaultExt = ".ini";
         Search.Filter = "File ini (*.ini)|*.ini";

         // Display OpenFileDialog by calling ShowDialog method
         Nullable<bool> result = Search.ShowDialog();

         // Get the selected file name and display in a TextBox
         if (result == true)
         {
             // Open document
             string filename = Search.FileName;
             file_receiver.Text = filename;
         }
     }
     */
    }

    public class MyModel
    {
        public string Name { get; set; }
        public bool StatusForCheckBox { get; set; }
        public bool Checked { get; set; }
    }

    // CLASS HANDLING MISMSATCHED STUFF, NEED TO MESH THIS STUFF IN
    public static class CSVHelpers
    {
        public static void CombineCsvFiles(string sourceFolder, string destinationFile, string searchPattern = "*.csv", bool isMismatched = false)
        {
            // Specify wildcard search to match CSV files that will be combined

            string[] filePaths = Directory.GetFiles(sourceFolder, searchPattern);
            if (isMismatched)
                CombineMisMatchedCsvFiles(filePaths, destinationFile);
            else
                CombineCsvFiles(filePaths, destinationFile);
        }

        public static void CombineCsvFiles(string[] filePaths, string destinationFile)
        {
            StreamWriter fileDest = new StreamWriter(destinationFile, true);

            int i;
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];

                string[] lines = File.ReadAllLines(file);

                if (i > 0)
                {
                    lines = lines.Skip(1).ToArray(); // Skip header row for all but first file
                }

                foreach (string line in lines)
                {
                    fileDest.WriteLine(line);
                }
            }

            fileDest.Close();
        }

        public static void CombineMisMatchedCsvFiles(string[] filePaths, string destinationFile, char splitter = ',')
        {
            HashSet<string> combinedheaders = new HashSet<string>();
            int i;
            // aggregate headers
            for (i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                combinedheaders.UnionWith(File.ReadLines(file).First().Split(splitter));
            }
            var hdict = combinedheaders.ToDictionary(y => y, y => new List<object>());

            string[] combinedHeadersArray = combinedheaders.ToArray();
            for (i = 0; i < filePaths.Length; i++)
            {
                var fileheaders = File.ReadLines(filePaths[i]).First().Split(splitter);
                var notfileheaders = combinedheaders.Except(fileheaders);

                File.ReadLines(filePaths[i]).Skip(1).Select(line => line.Split(splitter)).ToList().ForEach(spline =>
                {
                    for (int j = 0; j < fileheaders.Length; j++)
                    {
                        hdict[fileheaders[j]].Add(spline[j]);
                    }
                    foreach (string header in notfileheaders)
                    {
                        hdict[header].Add(null);
                    }
                });
            }

            DataTable dt = hdict.ToDataTable();

            dt.ToCSV(destinationFile);
        }
    }

    public static class DataTableHelper
    {
        public static DataTable ToDataTable(this Dictionary<string, List<object>> dict)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.AddRange(dict.Keys.Select(c => new DataColumn(c)).ToArray());

            for (int i = 0; i < dict.Values.Max(item => item.Count()); i++)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (var key in dict.Keys)
                {
                    if (dict[key].Count > i)
                        dataRow[key] = dict[key][i];
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static void ToCSV(this DataTable dt, string destinationfile)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(destinationfile, sb.ToString());
        }
    }
}