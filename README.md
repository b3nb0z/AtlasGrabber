# AtlasGrabber

Software to enable fast and large-scale analysis of the Human Protein Atlas (HPA) online database. 

### Features summary:

- Side by side view of up to 4 images for easy comparison
- Manual sorting of genes into separate lists
- Extract of all the genes/images for a particular tissue or cancer from the HPA XML file. 

## Table of Contents
- Installation
- Intended use
- Features
- Usage
    -  **Settings window**
        - Load gene list
        - Adjust settings
        - XML parser
    - **Analysis window**
        - Key bindings
    - **Browsers window**
    - **Progress bar**
    - **Finishing up / Saving progress**
- Help
- Credits
- License

## Setup
The software has been tested to run on Windows 7, 8, and 10. No installation of the executable is required. Some additional packages could be required when first launched. Windows should automatically identify these and prompt their installation.

You can also download the sourcecode directly and compile it yourself. 

## Intended use
To perform a fast, high throughput analysis of a gene list in the Human Protein Atlas (HPA) online database (https://www.proteinatlas.org/). 

The HPA is an online database that contains both a normal tissue atlas and a cancer atlas. Both of these include information on gene expression and localization of human proteins at the cellular level, either in normal or cancerous tissues, by immunohistochemistry. The normal tissue atlas is made up of 44 different normal tissues. The cancer atlas contains over 5 million images of immunohistochemically stained tissue sections and represents the 17 most common cancer types. 

The database is open access and is free to use. It can be accessed via the website (https://www.proteinatlas.org/). However, a large-scale analysis is difficult to perform through this website. 

The Atlas Grabber aims to help scientists quickly analyze a large number of genes for their expression in the HPA. 

## Features
- Viewing up to 4 images side by side from the same or different tissues: 
      - possibility to filter antibodies for
      - only HPA antibodies
      - only commercial antibodies
      - only looking at first antibody for a gene
      - random selection of antibody
    - Filter image samples to only view:
      - only first image
      - a random image
      - all images
      - exclude on include same patient ID images
    - Sort genes into up to 10 different lists
- Generate a file of all the genes available in the HPA from their XML database file.

## Usage
The software executable can be downloaded directly (GitHub link) or compiled from the source code. No additional setup or installations are required, and it should run without issues in windows 7, 8 and 10. On the first run, some packages might be needed to be installed, but windows will automatically detect these and prompt their installation. We recommend using a high definition, large screen monitor (above 20 inches) for the best experience.

An initial text file (.txt) is needed that contains the list of ENSG ID’s that you wish to analyze in the HPA. Such lists can be created for example by searching keywords in the HPA search field and exporting the file. We also provide a tool to generate a list of all ENSG ID’s from a particular tissue (see XML parser).  

To generate a gene list using keywords from the HPA website (Figure 1.), search for keywords in the search bar, then export the gene list by clicking "TSV". Download the file and extract it with a software like 7Zip (https://www.7-zip.org/download.html). Use excel to import and open the datafile (Figure 2.). Select the ENSG row and save it in a separate .txt file as a single column.   

![fig1 list_gen](https://user-images.githubusercontent.com/17572110/39356936-9b565104-4a11-11e8-8d4e-32eadd8d5282.gif)

Figure1. Download gene list using keywords from the HPA website (https://www.proteinatlas.org)

![fig2 excel](https://user-images.githubusercontent.com/17572110/39356967-adf22950-4a11-11e8-8a4a-4dd2e2ddeaa5.gif)

**Figure 2. Open downloaded file in excel.** 

### Settings window
#### Load gene list
The program initially opens in the Settings window. Here you load the genes you wish to analyze and set additional settings. 

Start by loading a gene list (ENSG ID's) from a text file that you wish to analyze in the HPA (Figure 3.). It should be a text file (.txt) made of a single column of ENSG ID’s. When done with the session, you can choose “Save rest” which will create a new file and save the remaining ENSG ID’s, then you can load that file if you wish to continue where you left off. 

![fig3 load_genelist](https://user-images.githubusercontent.com/17572110/39357172-4d32d4a6-4a12-11e8-8fd6-de47aecda73d.gif)

**Figure 3. Loading a text file.** 

#### Adjust settings
For each gene, the HPA may have several commercial or in-house made antibodies. You can specify which antibodies you wish to view: 
-	First - only first antibody
-	Random – random selection
-	Only commercial 
-	Only in-house (Atlas) 
-	All – view all the antibodies

You can then specify if you wish to see just the first image per antibody, a random image or all of them. 

The HPA database will typically have 1-2 images from a patient. If you only wish to view one image per patient, you can check the “Filter out same PID” checkbox. 

Specify to include only commercial antibodies, in-house ones, or both. Note: typically, there will be a commercial and an in-house antibody for each protein

Specify to include all image samples, just the first one, or a random one.

#### Selected gene lists
During the analysis, you can sort the genes into up to 10 lists. In the Settings window, you can name the different lists (Figure 4.). Each list has an assigned key (numbers 0-9) that when pressed in the analysis window will add the ENSG ID to the specified list. A gene can be added to multiple lists. The lists are .txt files, with a column of ENSG ID’s and can also be loaded to be analyzed.  
When finished with the analysis you have to save to lists, they are not saved automatically. We recommend saving it frequently not to lose data. Please note that saving the lists with the same name will replace the file. 

![fig4 listname_save](https://user-images.githubusercontent.com/17572110/39358034-ff9cfe12-4a14-11e8-9a28-3621f28006d9.gif)

**Figure 4. Naming and saving gene lists.** 

#### XML parser

With the XML parser you can extract all of the genes, antibodies, and image links for a specific tissue or cancer from the HPA XML database file (Figure 4).

To use it: 
1.	Download the HPA database file in XML format (https://www.proteinatlas.org/download/proteinatlas.xml.gz) 
2.	Extract it on your computer using a software like 7zip (https://www.7-zip.org/download.html) 
3.	In the “Settings window,” in the “XML parser” section, select the tissue or cancer type
4.	TestRun 100K (will test run the parser on the first 100 000 lines). Clicking the button will prompt you to open the extracted database file. You might have to select “all file types” to make it visible. Running it should only take a few seconds. 
5.	Parsing the whole XML will take a couple of minutes.
6.	As an output file, you will get a semicolon separated .text file like this. It includes ENSG ID, Antibody name, tissue, and link to the image. 
7.	Save the .text file. 

![fig4 xmlpars](https://user-images.githubusercontent.com/17572110/39357234-8348e5ee-4a12-11e8-8b50-496eadc35a18.gif)

**Figure 5. Use the XML parser to generate lists of all the avialable genes for a particular tissue or cancer.** 
 
### Analysis window

At the top of the page select which tissues to view. Any normal or cancerous tissue can be selected from the drop-down menu in any of the four windows. When a new window is assigned a tissue, the new window will be displayed. Up to four windows can be viewed simultaneously (Figure 5.).

The Human Protein Atlas is organized in the following hierarchical structure (Figure XX): For each gene ID, there are several antibodies, for each antibody there will be several images (= tissue sections) from different patients. Typically, there will be 1-2 images from one patient (not shown in the figure). Note that for each antibody there will be a varying number of antibodies (between 1-4) and images for each antibody.

![fig5 viewer](https://user-images.githubusercontent.com/17572110/39357874-6b55108c-4a14-11e8-8eb6-92cdd7273b75.gif)

**Figure 6. Analysis window for viewing images.** 

#### Key bindings: 

| Key           | Command |
| ------------- | ------------- |
| **A**         | next gene |
| **S**         | next antibody |
| **D**         | next image |
| **Q**         | previous gene |
| **W**         | previous antibody |
| **S**         | previous image |
| **0-9**       | save to list 0-9|
| **Space**     | previous gene |
| **Scroll wheel up**  | next image, if all images have been viewed it will jump to the next antibody, and eventually next gene.  |
| **Scroll wheel down**| opposite to wheel up |

In case of looking at multiple windows (and different tissues/tumors), the keys will be applied to all of them. 

![key bindings](https://user-images.githubusercontent.com/17572110/39312507-0b185016-4970-11e8-851e-859783bc5ec1.jpg)

**Figure 7. Key bindings**

### Browsers window

The browser option allows you to use the AtlasGrabber as a web browser. Entering browser mode will take you to the genes HPA page. Here you can view the gene summary and info (Figure 7.).

![fig8 listname_save_2](https://user-images.githubusercontent.com/17572110/39358138-732974c8-4a15-11e8-8d3c-df86ec5072b2.gif)

**Figure 8. Browser window can be used to directly access the HPA website.**

### Progress
To help track your progress, a progress bar at the top that tracks how far along the gene list you are. The Window name will also indicate how many genes you have viewed of the total. 

### Finishing up
Once finished, back at the gene list window, you should save your selected gene lists. In case you have not finished the whole gene list, and wish to continue another time, you can save the rest of the genes. This way the next time you load the list you can continue where you left off.

## Help
If you are unsure at any time about how to use the software, the help button will take you to this Readme page. 
Note! The AtlasGrabber will not save the images to the hard drive. It will simply save the text files.

## Credits:
Andrii Savchenko
Benedek Bozoky

## License
Gnu Public License v3.  For more info, please visit https://choosealicense.com/licenses/gpl-3.0/.
