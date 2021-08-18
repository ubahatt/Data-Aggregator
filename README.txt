NPI DATA PIPELINE VERSION 1.5

README 
This file contains instructions on how to operate the current version of the NPI Data Pipeline Tool.

TO RUN
1. Download the repository then run the executable "Data_Aggregator.exe".

PREREQUISITES
1. In order to preserve integrity of original data create 2 folder, a holding cell for original data (currently we may not need this, but keep it just in case until further testing) and an Input folder.
2. Ensure that the holding cell has the correct data, then copy any data from the holding cell into the parent folder.
3. Now you're ready to use the tool :) !

USAGE
Pre-use Nots: Make sure that you only have the files you want to combine in the folder, as the tool takes every present csv stored in the parent folder and combines them.

1. Ensure that you have a copy of your data saved elsewhere apart from the folder you plan on designating as the "Input Folder".
2. Open the tool.
3. Designate an input folder using the "Open Input Folder" button and selecting a folder present within the file browser pop-up.
4. After selecting an Input Folder, you can click the "Display Folder Contents" button to view the files present within the designated folder.
5. Designate an output location for your combined file using the "Select Output Folder" button.
5. Enter a name for your master file in the textbox under "Enter Name for Master File".
6. Click the "Format and Combine Data" button, and upon formatting success and data combination success you will receive a unique message for each portion.
-> After clicking the format and combine button, a new folder will be generated within the parent folder with the current date and time present in the filename,
   The files from parent folder will be copied into the new folder, to which they will then be edited to reflect their metadata, then they will be combined and made into a new
   csv file named after the user inputted master file name.

CHANGELOG
7/23/2021 - Release of beta version for testing.
7/26/2021 - Added line for header line input. This is used to delete header data post process due to the presence of Metadata on each of the file itself.
7/26/2021 - Added file creation time functionality.
7/30/2021 - Version 1 Release.
8/05/2021 - Automatic metadata line calculation.
8/09/2021 - Child folder is deprecated, Tool creates new file with the date and time attached and stores edited data copy there and master file.
8/18/2021 - Readded Child Folder Functionality - now known as "Output Folder" - due to Engineer Input.
