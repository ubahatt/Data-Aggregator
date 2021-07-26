NPI DATA PIPELINE VERSION 0

README 
This file contains instructions on how to operate the current version of the NPI Data Pipeline Tool.

TO RUN
1. Download repository and open it in VS.
2. Click Start Button to run Tool.

PREREQUISITES
1. In order to preserve integrity of original data, create a holding cell for original files you wish to combine.
2. After storing original data in a holding cell (seperate folder), create a new folder and copy the data to it.
3. Now you're ready to use the tool :) !

USAGE
1. Ensure that your original data is stored in a holding cell prior to usage of the tool.
2. Open the tool.
3. Designate a Parent Folder - Folder that the data from the holding cell is copied into. This is the folder holding the copy of the original data and will be the files that we are formatting and then combining.
4. Click "Display Folder Contents" button - This button will display the contents of the Parent Folder on the box to the right side of the screen. Use this to confirm you have selected the correct folder.
5. Designate a Child Folder - This is the folder that will contain the combined data of all the files within the Parent Folder.
6. Enter a name for the new file in the "Enter Name for Master File" bar below.
7. After ensuring all fields are filled out - Parent Folder, Child Folder, Master File Name - Click the format data button. This will format data and then combine it into a master file containing the data from the Parent Folder.
8. Let me know if anything breaks :) .

CHANGELOG
7/23/2021 - Release of beta version for testing.
7/26/2021 - Added line for header line input. This is used to delete header data post process due to the presence of Metadata on each of the file itself.
7/26/2021 - Added file creation time functionality.
