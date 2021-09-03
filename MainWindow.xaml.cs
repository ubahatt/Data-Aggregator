// DATA PIPELINE TOOL @ GENER8

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataAggregator
{
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     All code submitted prior to September 9th is written by Udi Bahatt
        ///
        ///     The purpose of the tool is to allow NPI Engineers within Gener8 to easily aggregate test data in the form of csv files in a large master file alongside important
        ///     metadata gathered from the files themselves. Metadata can include things such as the date the file was created, the date the test was performed, or other things
        ///     present in the files being combined such as firmware versions of the GUI used for testing or the name/designation of a certain machine the test was performed on.
        ///
        ///     Currently the tool does not search for any metadat when formatting the files, but in the future this can be addressed post test file format standardization.
        ///     Please see documentation below on the parsing for metadata in order to address this, perhaps copying and slightly editing the parse code I've written for the date
        ///     and such can be the best way to accomplish this.
        ///
        ///     On the current iteration of this code we are using Button_Format_Better_2, as it is the second version of the Button_Format_Better function. In the future this would
        ///     be best to rename into something more professional prior to widespread release and possibly future editing as well.
        /// </summary>

        // Variables for use within the majority of the tool
        private MyModel myModel;

        private string parent = "D:\\Testfolder";
        private string child = "D:\\Testfolder2";
        private ObservableCollection<MyModel> lt = new ObservableCollection<MyModel>();

        // Instantiating the window for the app to run in
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Button_Open_Input_Folder
        ///
        ///     This function is responsible for designating a folder to be used as an Input Folder withing the tool.
        ///
        ///     Use:
        ///     User clicks the button "Open Input Folder" to which a file explorer is opened. The user is then prompted to select a folder using
        ///     said file explorer and once one is selected, the field to the left displaying the folder path is shown.
        /// </summary>
        private void Button_Open_Input_Folder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                file_input.Text = openFileDlg.SelectedPath;
            }
            parent = file_input.Text;
        }

        /// <summary>
        ///     Button_Open_Output_Folder
        ///
        ///     This function is identical to Button_Open_InputFolder, but rather than designating an Input Folder is designates an Output Folder
        ///
        ///     Use:
        ///     Identical to Button_Open_InputFolder, just that the selected folder is assigned as the output location for the tool.
        /// </summary>
        private void Button_Open_Output_Folder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                file_output.Text = openFileDlg.SelectedPath;
            }
            child = file_output.Text;
        }

        /// <summary>
        ///     Button_Display_Folder_Content
        ///
        ///     This function displays the contents of the Input Folder in the textbox on the right side of the tool. Currently the tool shows you
        ///     all files within the folder, but you can change that within the function by editing a line of code, causing the function to only
        ///     display csv, txt, etc. (whatever you want). This is to verify that both Input Folder is populated, and populated with the correct
        ///     files.
        ///
        ///     Use:
        ///     Once clicked, displays the contents of Input Folder on the right side of the screen for users to see them.
        /// </summary>
        private void Button_Display_Folder_Content(object sender, RoutedEventArgs e)
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

        /// <summary>
        ///     Textbox_TextChanges
        ///
        ///     I don't know what this does except that I need to keep it for the tool to work.
        ///
        ///     Use:
        ///     N/A
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        /// <summary>
        ///     Button_Format_Better_2 - Currently in use within the tool.
        ///
        ///     This function handles the formatting and the combination of the data within the Input Folder. It creates a new folder within the Input
        ///     Folder, copies the files present within the Input Folder into the new folder, formats the files within the new folder by adding file
        ///     names, file creation dates, and relevant metadata to them, then combining the files post formatting into one master file (which is
        ///     named by the user), and finally deleting both the formatted files and the previously created folder within the Input Folder. These are
        ///     all independant parts of the function and do not rely on anything else such that they can each run independantly allowing for easy
        ///     changes in the future.
        ///
        ///     Use:
        ///     User clicks the Format and Combine Button in the tool. Files are moved around, formatted, and combined (the user is updated at each step's
        ///     completion) and after all the steps the master file can be found in the Output Folder.
        /// </summary>
        private void Button_Format_Better_2(object sender, RoutedEventArgs e)
        {
            // Variables for use within format function
            int lineSkip = 0;
            var newFileName = "";
            newFileName = fileNameTextBox.Text;

            // Main loop for the function
            // If else below handles if all fields are currently not present/filled out in the app
            if (parent == "D:\\Testfolder" || child == "D:\\Testfolder" || newFileName == "")
            {
                MessageBox.Show("Please select a Parent Folder AND enter a name for your master file prior to formatting data.", "Notification");
            }
            else
            {
                // More variables for use within format function
                int lineSkipNum = 0;
                string path = parent;
                int[] lineSkipArray = new int[99];
                CultureInfo provider = CultureInfo.InvariantCulture;

                //// Chunk below handles creating the new folder and naming it accordingly within parent then transferring parent files into the folder
                // Date grabbing for folder name
                DateTime currentDate = DateTime.Now;
                var fileDate = currentDate.ToString();
                fileDate = fileDate.Replace("/", "-");
                fileDate = fileDate.Replace(":", "-");

                // Assigning a destination for our new folder, but in our current case we assign parent as we want it to appear there but theoretically we can change it accordingly
                // or even allow for user input in the future
                string folderName = @parent;

                // Creating a string with both the new filename and it's date data and then appending it to the file path that we want it to be under
                var pathString = System.IO.Path.Combine(folderName, $"Edited Data {fileDate}");
                var newPath = pathString;

                // Create the subfolder. You can verify in File Explorer that you have this
                // structure in the C: drive (or whatever drive for that matter).
                //    Local Disk (C:)
                //        Top-Level Folder
                //            SubFolder
                System.IO.Directory.CreateDirectory(newPath);

                // Copying data from parent into newly created folder
                string sourcePath = parent;
                string targetPath = newPath;
                string copyFileName = string.Empty;
                string destFile = string.Empty;

                // Recursively copy files from parent into the new folder
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

                // This loop counts the amount of lines of metadata present in every file, assigning that number to an array containging all the data for every file
                // current we instantiate lineArray as having 99 cells, but in the future we can do more such that it becomes relevant
                foreach (string s in files)
                {
                    bool tf;
                    string[] lineArray = File.ReadAllLines(s);
                    int arrayNum = 0;
                    int t = 0;

                    // Iterate through the csv's lines parsing for "Time (sec)" as that is what data always begins with, and count lines by incrementing lineskip for each
                    // false output.
                    //
                    // In the future maybe change "Time (sec)" to just "Time" as some copies of test software vary in their naming scheme for time, essentially make uniform and
                    // more broad in scope
                    do
                    {
                        // More specific search term, but not all GUIs return us Time (sec), just Time will be more consistent
                        //tf = String.Equals(lineArray[t].Substring(0, 10), "Time (sec)");
                        tf = String.Equals(lineArray[t].Substring(0, 4), "Time");
                        lineSkip++;
                        t++;
                    } while (tf == false);

                    // We need to subtract 1 from lineSkip to ensure things work correctly, then assign lineskip to it's relevant lineArray entry to ensure we can use it later in data formatting.
                    lineSkip = lineSkip - 1;
                    lineSkipArray[arrayNum] = lineSkip;

                    // TESTING MESSAGE BOX
                    //MessageBox.Show(lineSkipArray[arrayNum].ToString(), "Notification");

                    // Reset lineSkip to 0 before going back in the loop such that we get correct data for every file present
                    lineSkip = 0;
                    arrayNum++;
                }

                foreach (string s in files)
                {
                    //int[] lineSkipNum = new int[99];

                    // Variables for use within data assignment for header data
                    // Certain variables are commented out as they aren't needed or used at the moment
                    string fileName = Path.GetFileName(s);
                    //string date_default = "N/A";
                    var date_input = "N/A";
                    var date_default = "N/A";
                    var firm_A_input = "N/A";
                    var firm_B_input = "N/A";
                    var firm_C_input = "N/A";

                    // The following variables are included in the case that you wish to be able to assign metadata to them,
                    // currently they are not in use.
                    //var firm_A_default = "N/A";
                    //var firm_B_default = "N/A";
                    //var firm_C_default = "N/A";

                    //// Grabbing the metadata from the file below
                    var line1 = File.ReadLines(s).First();
                    string[] line_number = File.ReadAllLines(s);
                    string fileCreationTime = File.GetCreationTime(s).ToString();

                    // Handles if GUI Compile date exists or not, can probably rework this as all data is different format wise
                    //
                    // Weirdly this begins at 1 and not 0? Anyways just extended Substring(0, 9) to (0, 10)
                    if (String.Equals(line1.Substring(0, 4), "Time"))
                    {
                        date_input = date_default;
                    }
                    else
                    {
                        line1 = line1.Replace("GUI Compile Date: ", "");
                        DateTime date = DateTime.Parse(line1);
                        date_input = date.ToString();
                    }

                    var csv = File.ReadLines(s)
                    .Select((line, index) => index == lineSkipArray[lineSkipNum]
                        // Order can be changed by subbing out the data labels below and by changing the variable name below that
                        ? line + "File_Name" + ",File_Creation_Date" + ",GUI_Compile_Date" + ",Firmware_Header_A" + ",Firmware_Header_B" + ",Firmware_Header_C" + ","
                        : line + fileName + "," + fileCreationTime + "," + date_input + "," + firm_A_input + ","
                               + firm_B_input + "," + firm_C_input + ",")
                    .ToList(); // we should write into the same file, that´s why we materialize

                    // Write all lines to csv such that it reflects the newly edited data
                    File.WriteAllLines(s, csv);

                    // Increment lineSkipNum in order to make sure that each file gets it's correct lineSkipArray value
                    lineSkipNum++;
                }

                // Notification showing that program has completed the data formatting portion
                MessageBox.Show("Data Formatting Complete!", "Notification");

                //// Chunk below handles combining data into a master
                // We have a commented chunk at the bottom, this will change the location of the combined master into the new generated folder if uncommented, currently
                // it saves that file just to parent.
                string sourceFolder = newPath;
                string destinationFile = child + "\\" + newFileName + ".csv";
                //string destinationFile = newPath + "\\" + newFileName + ".csv";

                // Specify wildcard search to match CSV files that will be combined
                //
                // Creating some more variables for use in this chunk
                string[] filePaths = Directory.GetFiles(sourceFolder);
                StreamWriter fileDest = new StreamWriter(destinationFile, true);

                // We can use this line below to save the csv to the original folder rather than putting that in the new edited data folder
                //StreamWriter fileDest = new StreamWriter(parent, true);

                int i;
                for (i = 0; i < filePaths.Length; i++)
                {
                    string file = filePaths[i];
                    string[] lines = File.ReadAllLines(file);

                    // Removing metadata labels for first file
                    if (i == 0)
                    {
                        lines = lines.Skip(lineSkipArray[i]).ToArray(); // Skip header row for first file
                    }

                    // Removes metadata lines in all other files except the first
                    if (i > 0)
                    {
                        lines = lines.Skip(lineSkipArray[i] + 1).ToArray(); // Skip header row for all but first file
                    }

                    foreach (string line in lines)
                    {
                        fileDest.WriteLine(line);
                    }
                }
                fileDest.Close();

                MessageBox.Show("Files have been combined!", "Notification");

                // File Cleanup
                // Deletes the "Edited Data {date}" file that gets created
                string[] fileDelete = Directory.GetFiles(newPath);
                foreach (string file in fileDelete)
                {
                    File.Delete(file);
                }
                Directory.Delete(newPath);

                MessageBox.Show("Files have been cleaned up!", "Notification");
            }
        }

        // This function is currently being used to test new functionality if we want to include it within the final version of the tool, but in the meantime is
        // not in use.
        //
        // Can honestly move this to the deprecated functions at this point, cant seem to get anywhere with it.
        private void Button_Format_Better_3(object sender, RoutedEventArgs e)
        {
            // VARIABLES ALLOWING FOR USER DEFINED FILENAMES
            var newFileName = "";
            newFileName = fileNameTextBox.Text;
            int lineSkip = 0;
            int easterEgg = 0;

            // MAIN LOOP FOR THE FUNCTION
            // THROWS USERS A MESSAGE INCASE CERTAIN FIELDS ARE MISSING IN WINDOW
            if (parent == "D:\\Testfolder" || newFileName == "")
            {
                easterEgg++;
                if (easterEgg >= 15)
                {
                    MessageBox.Show("Come on dude, you've incorrectly used the tool 15 times.", "Notification");
                }
                else
                {
                    MessageBox.Show("Please select a Parent Folder AND enter a name for your master file prior to formatting data.", "Notification");
                }
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
                DateTime currentDate = DateTime.Now;
                var fileDate = currentDate.ToString();
                fileDate = fileDate.Replace("/", "-");
                fileDate = fileDate.Replace(":", "-");

                // Specify a name for your top-level folder.
                string folderName = @parent;

                // To create a string that specifies the path to a subfolder under your
                // top-level folder, add a name for the subfolder to folderName.
                var pathString = System.IO.Path.Combine(folderName, $"Edited Data {fileDate}");
                var newPath = pathString;

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

                    // WEIRDLY THIS BEGINS AT 1 AND NOT 0
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

        //
        //
        //
        //
        // DEPRECATED FUNCTIONS BELOW
        //
        //
        //
        //

        // First tranfer function
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

        // 2nd Format function, written kinda badly
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

        // Function just moves files from one point to another
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

        // evolution on Button_Transfer2
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
    }

    public class MyModel
    {
        public string Name { get; set; }
        public bool StatusForCheckBox { get; set; }
        public bool Checked { get; set; }
    }

    // CLASS FOR HANDLING MISMSATCHED STUFF, NEED TO MESH THIS STUFF IN
    /*
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

    internal class Program
    {
        private const string FOLDER = @"c:\temp\test";

        private static void Main(string[] args)
        {
            CSVReader reader = new CSVReader();

            //table containing merged csv files
            DataTable dt = new DataTable();

            //get csv files one at a time
            foreach (string file in Directory.GetFiles(FOLDER, "*.csv"))
            {
                //read csv file into a new dataset
                DataSet ds = reader.ReadCSVFile(file, true);
                //datatable containing new csv file
                DataTable dt1 = ds.Tables[0];

                //add new columns to datatable dt if doesn't exist
                foreach (DataColumn col in dt1.Columns.Cast<DataColumn>())
                {
                    //test if column exists and add if it doesn't
                    if (!dt.Columns.Contains(col.ColumnName))
                    {
                        dt.Columns.Add(col.ColumnName, typeof(string));
                    }
                }

                //array of column names in new table
                string[] columnNames = dt1.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

                //copy row from dt1 into dt
                foreach (DataRow row in dt1.AsEnumerable())
                {
                    //add new row to table dt
                    DataRow newRow = dt.Rows.Add();

                    //add data from dt1 into dt
                    for (int i = 0; i < columnNames.Count(); i++)
                    {
                        newRow[columnNames[i]] = row[columnNames[i]];
                    }
                }
            }
        }
    }

    public class CSVReader
    {
        public DataSet ReadCSVFile(string fullPath, bool headerRow)
        {
            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\") + 1);
            string filename = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
            DataSet ds = new DataSet();

            try
            {
                //read csv file using OLEDB Net Library
                if (File.Exists(fullPath))
                {
                    string ConStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}" + ";Extended Properties=\"Text;HDR={1};FMT=Delimited\\\"", path, headerRow ? "Yes" : "No");
                    string SQL = string.Format("SELECT * FROM {0}", filename);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(SQL, ConStr);
                    adapter.Fill(ds, "TextFile");
                    ds.Tables[0].TableName = "Table1";
                }

                //replace spaces in column names with underscore
                foreach (DataColumn col in ds.Tables["Table1"].Columns)
                {
                    col.ColumnName = col.ColumnName.Replace(" ", "_");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ds;
        }
    }
    */
}