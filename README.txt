NPI DATA PIPELINE VERSION 0

README 
This file contains instructions on how to operate the current version of the NPI Data Pipeline Tool.

TO RUN
1. Download the repository then run the executable "Data_Pipeline.exe".

PREREQUISITES
1. In order to preserve integrity of original data, create 3 differing folders; A holding cell, parent folder, and child folder. The holding cell is responsible for holding the original copy of the data you are working with, and both parent and child folder are there to be used by the tool.
2. Ensure that the holding cell has the correct data, then copy any data from the holding cell into the parent folder.
3. Now you're ready to use the tool :) !

USAGE
1. Ensure that your original data is stored in a holding cell prior to usage of the tool. THIS STEP IS VERY IMPORTANT!!!
2. Open the tool.
3. Designate a Parent Folder - Folder that the data from the holding cell is copied into. This is the folder holding the copy of the original data and will be the files that we are formatting and then combining.
4. Click "Display Folder Contents" button - This button will display the contents of the Parent Folder on the box to the right side of the screen. Use this to confirm you have selected the correct folder.
5. Designate a Child Folder - This is the folder that will contain the combined data of all the files within the Parent Folder.
6. Enter the amount of Metadata lines present in your files (Currently the tool will only accept files that all have the same amount of metadata lines)
7. Enter a name for the new file in the "Enter Name for Master File" bar below.
8. After ensuring all fields are filled out - Parent Folder, Child Folder, Metadata Lines, and Master File Name - Click the format data button. This will format data and then combine it into a master file containing the data from the Parent Folder.
9. Let me know if anything breaks :) .

CHANGELOG
7/23/2021 - Release of beta version for testing.
7/26/2021 - Added line for header line input. This is used to delete header data post process due to the presence of Metadata on each of the file itself.
7/26/2021 - Added file creation time functionality.
