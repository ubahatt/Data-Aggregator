NPI DATA PIPELINE VERSION 1

README 
This file contains instructions on how to operate the current version of the NPI Data Pipeline Tool.

TO RUN
1. Download the repository then run the executable "Data_Pipeline.exe".

PREREQUISITES
1. In order to preserve integrity of original data create 2 folder, a holding cell for original data (currently we may not need this, but keep it just in case until further testing) and a parent folder.
2. Ensure that the holding cell has the correct data, then copy any data from the holding cell into the parent folder.
3. Now you're ready to use the tool :) !

USAGE
1. Ensure that your original data is stored in a holding cell prior to usage of the tool. THIS STEP IS VERY IMPORTANT AS IT ENSURES THE INTEGRITY AND PURITY OF ORIGINAL DATA!!!
2. Open the tool through the Data_Pipeline.exe.
3. Designate a Parent Folder - Folder that the data from the holding cell is copied into.
4. Click "Display Folder Contents" button - This button will display the contents of the Parent Folder on the box to the right side of the screen. Use this to confirm you have selected the correct folder.
5. Enter a filename for the master file you are creating - file containing the edited data you wish to combine.
8. After ensuring all fields are filled out - Parent Folder and Master File Name - Click the format data button. This will format data and then combine it into a master file containing the data from the Parent Folder. The file will be placed into a new folder that is created named "Edited Data {Today's Date}".
9. Let me know if anything breaks :) .

CHANGELOG
7/23/2021 - Release of beta version for testing.
7/26/2021 - Added line for header line input. This is used to delete header data post process due to the presence of Metadata on each of the file itself.
7/26/2021 - Added file creation time functionality.
7/30/2021 - Version 1 Release.
8/05/2021 - Automatic metadata line calculation.
8/09/2021 - Child folder is deprecated, Tool creates new file with the date and time attached and stores edited data copy there and master file.
