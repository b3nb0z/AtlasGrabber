# AtlasGrabber

A software to enable fast and large scale analysis of the the Human Protein Atlas online database. 

## Table of Contents
1. Installation
2. Intended use 
3. Usage
4. Credits
5. Licence 

## Installation
No installation of the executable is required. Some additional **packages** could be required at first start. Windows should automatically identify these and promt their installation. 

## Intended use
To preform a fast analysis of a gene list in the Human Protein Atlas online database.

## Usage 
An initial text file (.txt) is needed that contains the list of ENSG Id’s that is to be analyzed in the Protein Atlas. Such lists can be generated from the downloadable files from the HPA website (http://www.proteinatlas.org/about/download) or by searching key words in the HPA search field and exporting the file. 

### Setup window
The software executable can be downloaded directly (github link) or compiled from the source code. No additional setup or installations are required and it should run without issues in windows 7, 8 and 10. On the first run, some **packages** might be needed to be installed, but windows will automatically detect which ones. We recommend using a high definition, large screen monitor (above 20 inches) for the best experience. 

The program initially opens to the Setup window. In the setup window the following setting can be done: 

1. Load a gene list (ENSG ID's) from a text file (Fig 1. A)
2. Specify to include only commercial antibodies, in-house ones, or  both (Fig 1. B) 
  Note: typically there will be a commercial and an in-house antibody for each protein
3. Specify to inlcude all image samples, just the first one, or a random one (Fig 1. C)
4. Filter out additional images from the same patient for an antibody (Fig 1. D)
  Note: typically there will be 2 images per patient). 
5. Name different lists that that selected genes will be added to. Each list already has an assigned key: from 0 to 9. While in the analysis window, pressing the assigned key will copy the ENSG ID to the list. The lists will be created in the same folder where the program is located when the first gene ID is added to it. If the file already exists, from a previous analysis for instance, the new gene names will be added to the old one. 

![shot1](https://cloud.githubusercontent.com/assets/17572110/22786755/23ccbf8a-eeda-11e6-9034-58a92b146569.jpg)
Figure 1.  Setup window of the AtlasGrabber

### Analysis window
At the top of the page select which tissues to look at. Any normal or cancerous tissue can be selected from the dropdown menu in any of the four windows. When a new window is assigned a tissue, the new window will be displayed. Up to four windows can be looked at the same time (Figure 2. A). 

![shot2](https://cloud.githubusercontent.com/assets/17572110/22879335/d49d9d88-f1dd-11e6-8927-01204d0f748b.jpg)
Figure 2.  Analysis window of the AtlasGrabber

The Human Protein Atlas is organized in the following hierarchial structure (Figure 3): For each gene ID, there are several antibodies that target it, for each antibody there will be several images (= tissue sections) from different patients. Typically there will be 1-2 images from one patient (not shown in figure). Note that for each antibody there will be a varying number of antibodies (between 1-4) and images for each antibody. 

![hpa_organization 2](https://cloud.githubusercontent.com/assets/17572110/22879557/e1b25cce-f1de-11e6-8e52-b0ca0c7f641f.png)
Figure 3. The hiarchial organization of the Human Protein Atlas Database

Thus, o preform the analysis, use the following keys to
1. Go back and forth the gene list - scroll up and down with mouse
2. Go back and forth the antibodies for a particular gene
  **Right arrow**: next
  **Left arrow**: previous
3. Go back and forth the images
  **Down arrow**: next
  **Up arrow**: previous

In case of looking at multiple windows (and different tissues/tumors), the keys will be applied to all of them. Pressing any of the numbers, 0-9, will assign the current gene ID to the corresponding list. 

In the Gene list window, the current gene ID can be seen on the left, and the ones assigned to the lists on the right. 

If you are interested in the particular Gene you looking at, you also have the option to go to it's website location in the built-in browser by selecting the browser window. This can be useful if you wish to read a quick gene summary etc. 

There is a progress bar at the top of the window that shows how much of the imported genes have been looked at.  

Once finished, back at the gene list window, it is possible to save the gene lists and either save the rest of the gene list, in case you haven't yet finished it, or even save all of them. Clicking any of them will not overwrite the current file but create a new one. 

Next time you wish to continue the analysis, you can load the file containing the reaming genes.  
If you are unsure at any time about how to use the software, the help button will bring up a document that contains a detailed tutorial on how to use it. 

Note! The AtlasGrabber will not save the images to the harddrive. It will simply save the text files. 

## Credits:
Andrii Savchenko
Benedek Bozoky

## License
Gnu Public License v3. For more infromation see the 
