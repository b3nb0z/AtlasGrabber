# AtlasGrabber

A software to enable fast and large scale analysis of the the Human Protein Atlas online database. 

## Table of Contents

## Installation


## Usage 

To start using the AtlasGrabber, an initial text file (.txt) is needed that contains a list of ENSG Id’s, that one intends to analyze. Such lists can be generated from the downloadable files from the HPA website (http://www.proteinatlas.org/about/download) or by searching key words in the HPA search field and exporting the file. 
The software executable can be downloaded directly (github link) or compiled from the source code. No additional setup or installations are required and it should run without issues in windows 7, 8 and 10 . We recommend using a high definition, large screen monitor (above 20 inches) for the best experience. 

![shot1](https://cloud.githubusercontent.com/assets/17572110/22786755/23ccbf8a-eeda-11e6-9034-58a92b146569.jpg)
Format: ![Alt Text](url)

Figure 2. Gene selection window of the Atlas Grabber
The program initially opens to the "Gene list" window. Here one can load the gene list from the text file (Fig1 A); specify to look at all antibodies, only commercial ones, in-house ones, or both (B) and specify to look at all the image samples, just the first one, or a random one. One can also  chose to filter out additional images from the same patient for an antibody (C) (typically there will be 2 images per patient). 
Here it also also possible to name different lists that that selected genes will be added to. Each list already has an assigned key: from 0 to 9. While in the Analysis window, looking though the atlas the current gene Id can copied over to any of the up to 10 lists. The lists will be created in the same folder where the program is located when the first gene ID is added to it. If the file already exists, from a previous analysis for instance, the new gene names will be added to the old one. 
Next, at the top of the page you can select which tissues you would like to analyze. You can select any normal or cancerous tissue from the dropdown menu in any of the four windows. When a new window is assigned a tissue, a new window will be added. 
You are now ready to start the analysis. To start looking at the images simple select Images tab (Figure 1 X). Use you mouse pointer to look around a tissue, use the scrolling wheel up and down to move through the images. The keyboard keys are used to move to the next antibody for the gene ID (A and D) and to the next of previous gene ID (N and P). Pressing any of the keys 0-9 will assign the gene ID to that particular list. Going back to the Gene list window, you can see which gene ID you are looking on the left panel and what genes IDs you have assigned to the different lists. 
If you are interested in the particular Gene you looking at, you also have the option to go to it's website location in the built-in browser by selecting the browser window. This can be useful if you wish to read a quick gene summary etc. 
During the analysis, the progress bar will show how far along you have gotten. 
Once finished, you can go back to the gene list window, save the gene lists and either save the rest of the gene list, in case you haven't yet finished it, or even save all of them. Clicking any of them will not overwrite the current file but create a new one. Next time you wish to continue the analysis, you can load the file containing the reaming genes.  
If you are unsure at any time about how to use the software, the help button will bring up a document that contains a detailed tutorial on how to use it. 

[how does the functionality compare and improve the functionality of similar existing software]
Similar software has been designed previously, for example the HPASubC. It can also be used to analyze the protein atlas. It is a package of several python scripts: one to download images, one to view, one to score them and one to download the protein data. In addition, it also relies on a number of dependencies to run. 
The HPASubC differs from the AtlasGrabber in several ways. Because AtlasGrabber is a windows executable, it will be easier and faster to run even for technically non-skilled users although it limits the use to the windows platform. In comparison, to run HPASubC, users must be able to install Python 2.x, install dependencies, and run python scripts form the terminal. It also requires users to download all the images one plans to look through to the hard drive, while the AtlasGrabber doesn’t. The AtlasGrabber will only display the images and not download them. The only thing it saves will be the text file lists. 

Contributing: Larger projects often have sections on contributing to their project, in which contribution instructions are outlined. Sometimes, this is a separate file. If you have specific contribution preferences, explain them so that other developers know how to best contribute to your work. To learn more about how to help others contribute, check out the guide for (setting guidelines for repository contributors)[https://help.github.com/articles/setting-guidelines-for-repository-contributors/].

## Credits:
Andrii Savchenko
Benedek Bozoky

## License
Gnu Public License v3
