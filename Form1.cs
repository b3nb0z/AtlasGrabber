using System;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        ImageCodecInfo jgpEncoder ;
        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
        System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);

        const int upperShift = 40;
        
        Bitmap b = new Bitmap(3000, 3000);
        
        bool rn = false;
        bool uploadInProcess = false;
        bool wheelProcessing = false;
        const string programName = "Atlas Grabber   ";
            
        string[] ensa = new string[54000];
        string[] gene = new string[54000];
        int[] pid_cat = new int[10000];
      
        int ia=0, ib=0, ic=0, id=0;
        int sa=1, sb=3, sc=3, sd=3;

        int[] abnumber = new int[100];
        int abcount = 0;
        int[] pid = new int[10000];
      
        int[,] ablist = new int[5, 25];
        int[,] curMaxAb = new int[5, 2];
        int[,] curMaxImg = new int[5, 2];
        Keys[] keysList = new Keys[15];
        TextBox[] namesList = new TextBox[15];
        ListBox[] listsList = new ListBox[15];
        Label[] labelsList = new Label[15];

      
        List<string> imagesList1 = new List<string>();
        List<string> imagesList2 = new List<string>(); 
        List<string> imagesList3 = new List<string>();
        List<string> imagesList4 = new List<string>();

        List<int> pIDList1 = new List<int>();
        List<int> pIDList2 = new List<int>();
        List<int> pIDList3 = new List<int>();
        List<int> pIDList4 = new List<int>();

        List<imageDescr> imagesToGet = new List<imageDescr>();
        List<abDescr> abToGet = new List<abDescr>();
        List<abDescr> abToGetS = new List<abDescr>();


        Point _mousePt = new Point();
        bool _tracking = false;

        int windowNumber = 0;
        int exc = 0;
        int geneNumber = 0;
        StreamReader genesr;
        StreamWriter geneSave;
        string lastSaved="";
        string current="";
        bool opened = false;
        int genesProc=0;
        DateTime lastclick;


        private class imageDescr
        {
            public string ensg { get; set; }
            public string ABMark { get; set; }
            public int ABCode { get; set; }
            public int PID { get; set; }
            public string imageAddress { get; set; }
        }

        private class abDescr
        {
            public string ABMark { get; set; }
            public int ABCode { get; set; }
            public int ABPos { get; set; } 
        }

        

        private void GetIt2(string fl, int window)
        {
            uploadInProcess = true;
            Image _Image = null;
            _Image = DownloadImage("http://www.proteinatlas.org" + fl);

            string fn = fl.Substring(8, fl.Length - 8);
            int ast;
            string fn1;

            ast = fn.IndexOf("/");
            fn1 = fn.Substring(0, ast) + "_" + fn.Substring(ast + 1, fn.Length - ast - 1);
          
            if (_Image != null)
            {
                // show image in picturebox . 
                switch (window)
                {
                    case 1: pictureBox1.Image = _Image;
                            panel1.AutoScrollPosition = new Point((pictureBox1.Width - panel1.Width) / 2, (pictureBox1.Height - panel1.Height) / 2);
                            labelAb1.Text = "Ab_" + ablist[1, curMaxAb[1, 0]] + "(" + (curMaxAb[1, 0] ) + @"/" + (curMaxAb[1, 1] ) + "), Sample(" + (ia + 1) + @"/" + imagesList1.Count() + ")";
                            labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
                            labelAb1.Left = -panel1.AutoScrollPosition.X + 5;
                            labelAb1.Visible = true;
                            break;
                    
                    case 2: pictureBox2.Image = _Image; 
                            panel2.AutoScrollPosition = new Point((pictureBox2.Width - panel2.Width) / 2, (pictureBox2.Height - panel2.Height) / 2); 
                            labelAb2.Text = "Ab_" + ablist[2, curMaxAb[2, 0]] + "(" + (curMaxAb[2, 0] ) + @"/" + (curMaxAb[2, 1] ) + "), Sample(" + (ib + 1) + @"/" + imagesList2.Count() + ")";
                            labelAb2.Top = -panel2.AutoScrollPosition.Y + 5;
                            labelAb2.Left = -panel2.AutoScrollPosition.X + 5;
                            labelAb2.Visible = true;
                            break;
                    
                    case 3: pictureBox3.Image = _Image;
                            panel3.AutoScrollPosition = new Point((pictureBox3.Width - panel3.Width) / 2, (pictureBox3.Height - panel3.Height) / 2); 
                            labelAb3.Text = "Ab_" + ablist[3, curMaxAb[3, 0]] + "(" + (curMaxAb[3, 0]) + @"/" + (curMaxAb[3, 1] ) + "), Sample(" + (ic + 1) + @"/" + imagesList3.Count() + ")";
                            labelAb3.Top = -panel3.AutoScrollPosition.Y + 5;
                            labelAb3.Left = -panel3.AutoScrollPosition.X + 5;
                            labelAb3.Visible = true;
                            break;
                    
                    case 4: pictureBox4.Image = _Image;
                            panel4.AutoScrollPosition = new Point((pictureBox4.Width - panel4.Width) / 2, (pictureBox4.Height - panel4.Height) / 2);
                            labelAb4.Text = "Ab_" + ablist[4, curMaxAb[4, 0]] + "(" + (curMaxAb[4, 0]) + @"/" + (curMaxAb[4, 1] ) + "), Sample(" + (id + 1) + @"/" + imagesList4.Count() + ")";
                            labelAb4.Top = -panel4.AutoScrollPosition.Y + 5;
                            labelAb4.Left = -panel4.AutoScrollPosition.X + 5;
                            labelAb4.Visible = true;
                            break;
                    default: break;

                }
                Application.DoEvents();
                pictureBox1.Focus();
                uploadInProcess = false;
            }

        }

        public Image DownloadImage(string _URL)
        {

            Image _tmpImage = null;  
            try {

                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL); 
                _HttpWebRequest.AllowWriteStreamBuffering = true;
                _HttpWebRequest.MediaType = "image/jpeg";
                _HttpWebRequest.Timeout = 8000000;  
 
                // Request response: . 
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();  
 
                // Open data stream: . 
                System.IO.Stream _WebStream =  _WebResponse.GetResponseStream();  

                // convert webstream to image . 
                _tmpImage = Image.FromStream(_WebStream);  
 
                // Cleanup . 
                _WebResponse.Close(); 
                _WebResponse.Close();  
            }  

            catch (Exception _Exception)  
            {  
                MessageBox.Show("Exception caught in process: "+_Exception.ToString());  
                return null;  
            }  
            return _tmpImage;  

        }


        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
            if (lastclick.AddMilliseconds(300) < DateTime.Now)
            {
                if (opened) // & !uploadInProcess
                {
                    if ((e.KeyCode == Keys.S) || (e.KeyCode == Keys.Space)) //next gene
                    {
                        //label1.Text = "Loaded " + geneNumber.ToString() + " genes";
                        nextGene();
                    }
                  
                    if ((e.KeyCode == Keys.A)) //next antibody
                    {
                        this.Enabled = false;
                        Application.DoEvents();
                        this.Enabled = true; 
                        nextAb(1);
                    }

                    
                    if ((e.KeyCode == Keys.Q)) //previous antiboy
                    {
                        this.Enabled = false;
                        Application.DoEvents();
                        this.Enabled = true;
                        previousAb(1);
                    }

                    if ((e.KeyCode == Keys.D)) //next image, the same as wheel
                    {
                        this.Enabled = false;
                        Application.DoEvents();
                        this.Enabled = true;
                        nextImg(1);
                    }


                    if ((e.KeyCode == Keys.E)) //previous image, the same as wheel
                    {
                        this.Enabled = false;
                        Application.DoEvents();
                        this.Enabled = true;
                        previousImg(1);
                    }

                    if ((e.KeyCode == Keys.W))
                    {
                        this.Enabled = false;
                        Application.DoEvents();
                        this.Enabled = true;
                        if (opened)
                        {
                            previousGene();
                        }
                    }

                    // process 10 lists
                    for (int i = 0; i < 10; i++ )
                    {
                        if (e.KeyCode == keysList[i])
                        {
                            this.Enabled = false;
                            Application.DoEvents();
                            this.Enabled = true;

                            // label1.Text = "Saved";
                            int itemCount=listsList[i].Items.Count;
                            if (itemCount != 0)
                            {
                                if ((string)listsList[i].Items[itemCount-1] != current) //skip duplicates
                                {
                                    listsList[i].Items.Add(current);
                                }
                            }
                            else
                            {
                                listsList[i].Items.Add(current);
                            }
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please, open the gene list first!", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                lastclick = DateTime.Now;
            }
        }
        
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void SaveGene()
        {
            if (lastSaved != current)
            {
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder =    System.Drawing.Imaging.Encoder.Quality;
                System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 95L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                //   pb.Image.Save(fn + ".jpg", jgpEncoder, myEncoderParameters);
                geneSave = File.AppendText(textBox33.Text);
                geneSave.WriteLine(current);
                lastSaved = current;
                geneSave.Close();

                listView2.Items.Add(current);

                if (checkBox2.Checked)
                {
                    if (windowNumber > 0) pictureBox1.Image.Save(ensa[genesProc] + "_" + ablist[1, curMaxAb[1, 0]] + "_" + (ia + 1) + "_1.jpg", jgpEncoder, myEncoderParameters);
                    if (windowNumber > 1) pictureBox2.Image.Save(ensa[genesProc] + "_" + ablist[2, curMaxAb[2, 0]] + "_" + (ib + 1) + "_2.jpg", jgpEncoder, myEncoderParameters);
                    if (windowNumber > 2) pictureBox3.Image.Save(ensa[genesProc] + "_" + ablist[3, curMaxAb[3, 0]] + "_" + (ic + 1) + "_3.jpg", jgpEncoder, myEncoderParameters);
                    if (windowNumber > 3) pictureBox4.Image.Save(ensa[genesProc] + "_" + ablist[4, curMaxAb[4, 0]] + "_" + (id + 1) + "_4.jpg", jgpEncoder, myEncoderParameters);
                }
            }
            
        }



        private void LoadGeneListBtn_Click(object sender, EventArgs e)
        {
            string inp,fname;

            fname = "";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "TXT Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog1.ShowDialog() == DialogResult.OK) fname = openFileDialog1.FileName;
            if (fname == String.Empty) return;

            genesr = File.OpenText(fname);
            

            if (genesr != null)
            {
                opened = true;
                geneNumber=0;
                while ((inp = genesr.ReadLine()) != null)
                {   ensa[geneNumber] = inp;
                    geneNumber += 1;
                    listView1.Items.Add(inp);
                    if (geneNumber % 1000 == 0) 
                    { 
                        this.Text = programName + "Loaded " + geneNumber.ToString() + " genes"; Application.DoEvents(); 
                    }
                }
                if (geneNumber > 0)
                {
                    geneNumber -= 1;
                    download_page(ensa[0]); genesProc = 0;
                    this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);
                    progressBar1.Value = 0;
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = geneNumber;
                    progressBar1.Step = 1;

                    radioButton1.Enabled = true;
                    radioButton3.Enabled = true;
                    ClearGeneListBtn.Enabled = true;
                    SaveRestBtn.Enabled = true;
                    button2.Enabled = true;

                }
            }
            genesProc = 0;
            groupBox7.Text = String.Format("Genes to analyze ({0}/{1})", genesProc, geneNumber);
        }

        private void ClearGeneListBtn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            geneNumber = 0; genesProc = 0;
            this.Text = programName;

            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = geneNumber;
            progressBar1.Step = 1;

            radioButton1.Enabled = false;
            radioButton3.Enabled = false;
            ClearGeneListBtn.Enabled = false;
            SaveRestBtn.Enabled = false;
            button2.Enabled = false;
            
            groupBox7.Text = String.Format("Genes to analyze ({0}/{1})", genesProc, geneNumber);
        }

        private void download_page(string en)
        {
            uploadInProcess = true;

            textBox34.Text = en;
            current = en;

            string tissueType = "";

            Application.DoEvents();
            Uri myuri1, myuri2, myuri3, myuri4;
            if (comboBox1.SelectedIndex != 0 & comboBox1.Enabled)
            {
                if (comboBox1.SelectedIndex <= 20) tissueType = @"/pathology/"; else tissueType = @"/";
                myuri1 = new Uri("http://proteinatlas.org/" + en + tissueType + @"tissue/" + (string)comboBox1.SelectedItem);
                webBrowser1.Navigate(myuri1);
            }

            if (comboBox2.SelectedIndex != 0 & comboBox2.Enabled)
            {
                if (comboBox2.SelectedIndex <= 20) tissueType = @"/pathology/"; else tissueType = @"/";
                myuri2 = new Uri("http://proteinatlas.org/" + en + tissueType + @"tissue/" + (string)comboBox2.SelectedItem);
                webBrowser2.Navigate(myuri2);
            }

            if (comboBox3.SelectedIndex != 0 & comboBox3.Enabled)
            {
                if (comboBox3.SelectedIndex <= 20) tissueType = @"/pathology/"; else tissueType = @"/";
                myuri3 = new Uri("http://proteinatlas.org/" + en + tissueType + @"tissue/" + (string)comboBox3.SelectedItem);
                webBrowser3.Navigate(myuri3);
            }

            if (comboBox4.SelectedIndex != 0 & comboBox4.Enabled)
            {
                if (comboBox4.SelectedIndex <= 20) tissueType = @"/pathology/"; else tissueType = @"/";
                myuri4 = new Uri("http://proteinatlas.org/" + en + tissueType + @"tissue/" + (string)comboBox4.SelectedItem);
                webBrowser4.Navigate(myuri4);
            }

            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            
            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
            {
                 Application.DoEvents();
            }

            while (webBrowser3.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            while (webBrowser4.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            initImageCountersAndSteps();
                        
            if (windowNumber > 0) 
            {
                getAbList(webBrowser1.DocumentText, 1); 
                if (curMaxAb[1, 1]>0) getImageList(webBrowser1.DocumentText, 1, curMaxAb[1, 0]);
                if (imagesList1.Count > 0) GetIt2(imagesList1[0], 1); else showEmpty(1);
            }
            if (windowNumber > 1) 
            { 
                getAbList(webBrowser2.DocumentText, 2);
                if (curMaxAb[2, 1] > 0) getImageList(webBrowser2.DocumentText, 2, curMaxAb[2, 0]);
                if (imagesList2.Count >= ib & imagesList2.Count > 0 ) GetIt2(imagesList2[ib], 2);  else showEmpty(2);
            }


            if (windowNumber > 2) 
            { 
                getAbList(webBrowser3.DocumentText, 3);
                if (curMaxAb[3, 1] > 0) getImageList(webBrowser3.DocumentText, 3, curMaxAb[3, 0]);
                if (imagesList3.Count >= ic & imagesList3.Count > 0) GetIt2(imagesList3[ic], 3); else showEmpty(3);
            }

            if (windowNumber > 3) 
            { 
                getAbList(webBrowser4.DocumentText, 4);
                if (curMaxAb[4, 1] > 0) getImageList(webBrowser4.DocumentText, 4, curMaxAb[4, 0]);
                if (imagesList4.Count >= id & imagesList4.Count > 0) GetIt2(imagesList4[id], 4); else showEmpty(4);
            }
            webBrowser1.Focus();

            uploadInProcess = false;
        }

        private void initImageCountersAndSteps() // it allows to compare stainings for the same gene. If two windows of the same gene is used then:
                                           //window 1 will have image0, window 2 will have image 1 and image step for window 1 and 2 will be 2. 
        {
            int a = comboBox1.SelectedIndex, b = comboBox2.SelectedIndex, c = comboBox3.SelectedIndex, d = comboBox4.SelectedIndex;

            ia = 0; ib = 0; ic = 0; id = 0;
            if (b == a) { ib = ia + 1; }
            if (c == a) { ic = ia + 1; }
            if (c == b) { ic = ib + 1; }
            if (d == a) { id = ia + 1; }
            if (d == b) { id = ib + 1; }
            if (d == c) { id = ic + 1; }

            sa = 1; sb = 1; sc = 1; sd = 1;

            if (a == b) { sa = 2; sb = 2; }
            if (a == c) { sa = 2; sc = 2; }
            if (a == d) { sa = 2; sd = 2; }
            if (b == c) { sb = 2; sc = 2; }
            if (b == d) { sb = 2; sd = 2; }
            if (c == d) { sc = 2; sd = 2; }

            if (a == b & a == c) { sa = 3; sb = 3; sc = 3; }
            if (a == b & a == d) { sa = 3; sb = 3; sd = 3; }
            if (b == c & b == d) { sb = 3; sc = 3; sd = 3; }

            if (a == b & a == c & a == d) { sa = 4; sb = 4; sc = 4; sd = 4; }
        }


        private void showEmpty(int wnd)
        {
            switch (wnd)
            {
                case 1: pictureBox1.Image = b; labelAb1.Text = "No image"; break;
                case 2: pictureBox2.Image = b; labelAb2.Text = "No image"; break;
                case 3: pictureBox3.Image = b; labelAb3.Text = "No image"; break;
                case 4: pictureBox4.Image = b; labelAb4.Text = "No image"; break;
                default: break;
            }

        }


        private void getImageList(string s1, int window, int abNumber)
        {
            string s2, as3, s3;

            switch (window)
            {
                case 1: imagesList1.Clear(); pIDList1.Clear(); listBoxIm1.Items.Clear(); break;
                case 2: imagesList2.Clear(); pIDList2.Clear(); listBoxIm2.Items.Clear(); break;
                case 3: imagesList3.Clear(); pIDList3.Clear(); listBoxIm3.Items.Clear(); break;
                case 4: imagesList4.Clear(); pIDList4.Clear(); listBoxIm4.Items.Clear(); break;
                default: break;
            }
            
            int firstCharacter = 0;
            int secondCharacter = 0;
            int imagesCount = 0;

            int pID=0;
            int med2;
            as3 = "\"\""; as3 = as3.Substring(1);

            do
            {
                firstCharacter = s1.IndexOf(@"/images/" + ablist[window, abNumber], firstCharacter + 4);
                if (firstCharacter != -1)
                {

                    s2 = s1.Substring(firstCharacter, 150);
                    secondCharacter = s2.IndexOf(as3);

                    s2 = s2.Substring(0, secondCharacter);

                    secondCharacter = s2.IndexOf("thumb");
                    if (secondCharacter == -1) //full res, not thumbnails
                    {
                        secondCharacter = s2.IndexOf("medium");
                        if (secondCharacter == -1) //full res, not thumbnails
                        {
                            secondCharacter = s2.IndexOf("_selected_60x60");
                            if (secondCharacter == -1) //it is real image, not some background stuff
                            {
                                s3 = s1.Substring(firstCharacter, 540);
                                med2 = s3.IndexOf("Patient id");

                                if (med2 != -1)
                                {
                                    string s4 = s3.Substring(med2 + 15, 6);
                                    med2 = s4.IndexOf("<");
                                    pID = Convert.ToInt16(s4.Substring(0, med2));
                                }
                                switch (window)
                                {
                                    case 1:
                                        if (checkBox1.Checked)
                                        {
                                            if (!pIDList1.Contains(pID)) { pIDList1.Add(pID); imagesList1.Add(s2); listBoxIm1.Items.Add(s2); imagesCount += 1; }
                                        }
                                        else
                                        {
                                            pIDList1.Add(pID); imagesList1.Add(s2); listBoxIm1.Items.Add(s2); imagesCount += 1;
                                        }
                                        break;
                                    case 2:
                                        if (checkBox1.Checked)
                                        {
                                            if (!pIDList2.Contains(pID)) { pIDList2.Add(pID); imagesList2.Add(s2); listBoxIm2.Items.Add(s2); imagesCount += 1; }
                                        }
                                        else
                                        {
                                            pIDList2.Add(pID); imagesList2.Add(s2); listBoxIm2.Items.Add(s2); imagesCount += 1;
                                        }
                                        break;
                                    case 3:
                                        if (checkBox1.Checked)
                                        {
                                            if (!pIDList3.Contains(pID)) { pIDList3.Add(pID); imagesList3.Add(s2); listBoxIm3.Items.Add(s2); imagesCount += 1; }
                                        }
                                        else
                                        {
                                            pIDList3.Add(pID); imagesList3.Add(s2); listBoxIm3.Items.Add(s2); imagesCount += 1;
                                        }
                                        break;
                                    case 4:
                                        if (checkBox1.Checked)
                                        {
                                            if (!pIDList4.Contains(pID)) { pIDList4.Add(pID); imagesList4.Add(s2); listBoxIm4.Items.Add(s2); imagesCount += 1; }
                                        }
                                        else
                                        {
                                            pIDList4.Add(pID); imagesList4.Add(s2); listBoxIm4.Items.Add(s2); imagesCount += 1; ;
                                        }

                                        break;
                                    default: break;
                                }

                            }
                        }
                    }
                    
                }

            } while (firstCharacter != -1);

            switch (window)
            {
                case 1: listBoxIm1.Items.Add(imagesCount); curMaxImg[1, 0] = 0; curMaxImg[1, 1] = imagesCount; break;
                case 2: listBoxIm2.Items.Add(imagesCount); curMaxImg[2, 0] = 0; curMaxImg[2, 1] = imagesCount; break;
                case 3: listBoxIm3.Items.Add(imagesCount); curMaxImg[3, 0] = 0; curMaxImg[3, 1] = imagesCount; break;
                case 4: listBoxIm4.Items.Add(imagesCount); curMaxImg[4, 0] = 0; curMaxImg[4, 1] = imagesCount; break;
                default: break;
            }
            initImageCountersAndSteps();
        }



        private void getAbList(string s1, int windowNumber)
        {
            int abn;
            int firstCharacter = 0; abcount = 0;
            string s2;

            switch (windowNumber)
            {
                case 1: listBoxAb1.Items.Clear(); break;
                case 2: listBoxAb2.Items.Clear(); break;
                case 3: listBoxAb3.Items.Clear(); break;
                case 4: listBoxAb4.Items.Clear(); break;
                default: break;
            }

            switch (windowNumber)
            {
                case 1: imagesList1.Clear(); pIDList1.Clear(); listBoxIm1.Items.Clear(); break;
                case 2: imagesList2.Clear(); pIDList2.Clear(); listBoxIm2.Items.Clear(); break;
                case 3: imagesList3.Clear(); pIDList3.Clear(); listBoxIm3.Items.Clear(); break;
                case 4: imagesList4.Clear(); pIDList4.Clear(); listBoxIm4.Items.Clear(); break;
                default: break;
            }

            do
            {
                firstCharacter = s1.IndexOf("CAB", firstCharacter + 4);
                if (firstCharacter != -1)
                {
                    s2 = s1.Substring(firstCharacter + 3, 6);//copy ab number

                    try
                    {
                        abn = Convert.ToInt32(s2);
                        abcount += 1;
                        ablist[windowNumber, abcount]=abn;
                        
                        switch (windowNumber)
                        {
                            case 1: listBoxAb1.Items.Add(abn); break;
                            case 2: listBoxAb2.Items.Add(abn); break;
                            case 3: listBoxAb3.Items.Add(abn); break;
                            case 4: listBoxAb4.Items.Add(abn); break;
                            default: break;
                        }
                    }
                    catch (FormatException)
                    {
                        exc += 1; 
                    }

                }
            } while (firstCharacter != -1);

            firstCharacter = 0;
            do
            {
                firstCharacter = s1.IndexOf("HPA", firstCharacter + 4);
                if (firstCharacter != -1)
                {
                    s2 = s1.Substring(firstCharacter, 15);
                    char c=s1[firstCharacter+3];
                    if (c == '0' | c == '1' | c == '2' | c == '3' | c == '4' | c == '5' | c == '6' | c == '7' | c == '8' | c == '9')
                    {
                        s2 = s1.Substring(firstCharacter + 3, 6);//copy ab number

                        try
                        {
                            abn = Convert.ToInt32(s2);
                            abcount += 1;
                            ablist[windowNumber, abcount] = abn;
                            switch (windowNumber)
                            {
                                case 1: listBoxAb1.Items.Add(abn); break;
                                case 2: listBoxAb2.Items.Add(abn); break;
                                case 3: listBoxAb3.Items.Add(abn); break;
                                case 4: listBoxAb4.Items.Add(abn); break;
                                default: break;
                            }
                        }
                        catch (FormatException)
                        {
                            exc += 1; 
                        }
                    }
                }
            } while (firstCharacter != -1);

            if (abcount > 0) curMaxAb[windowNumber, 0] = 1; else curMaxAb[windowNumber, 0] = 0;
            curMaxAb[windowNumber, 1] = abcount;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resize_upperStrip();
            if (radioButton1.Checked||radioButton2.Checked)  resize_browsers();
            if (radioButton3.Checked) resize_panels();
            resize_reqLists();
        }

        private void resize_universal()
        {
            if (radioButton1.Checked || radioButton2.Checked) resize_browsers();
            if (radioButton3.Checked) resize_panels();
            resize_reqLists();
        }

        private void resize_reqLists()
        {
            int he = this.Height - upperShift - 28;
            groupBox2.Height = he;
            groupBox6.Height = he;

            groupBox7.Height = groupBox2.Height - 20 - groupBox8.Height;
            listView1.Height = groupBox7.Height - 76;

            groupBox6.Width = this.Width - 15 - groupBox2.Width;

            int llw= (int)Math.Round((double)(groupBox6.Width-6*8)/5);
            int llh = (int)Math.Round((double)(groupBox6.Height - 100) / 2);


            for (int i = 0; i<10; i++)
            {
                listsList[i].Width = llw;
            }

            listsList[0].Left = 8; namesList[0].Left = 8;
            listsList[5].Left = 8; namesList[5].Left = 8;

            for (int i = 1; i < 5; i++)
            {
                listsList[i].Left = listsList[i-1].Right+8;
                namesList[i].Left = listsList[i - 1].Right + 8;
                labelsList[i].Left = namesList[i].Right + 2;

                listsList[i+5].Left = listsList[i + 4].Right + 8;
                namesList[i+5].Left = listsList[i + 4].Right + 8;
                labelsList[i+5].Left = namesList[i + 5].Right + 2; 

            }

            for (int i = 0; i < 10; i++)
            {
                listsList[i].Height = llh;
             
            }

            for (int i = 5; i < 10; i++)
            {
                listsList[i].Top = llh + 70;
                namesList[i].Top = llh + 48;
                labelsList[i].Top = llh + 50;
            }
        }

        private void resize_browsers()
        {
            int half = 0;
            half = (int)Math.Round((double)(this.Width - 10) / 2);
            int tred = (int)Math.Round((double)(this.Width - 10) / 3);
            int he = this.Height - upperShift-28;
            int halfHe = (int)Math.Round((double)he/2);

          //  progressBar1.Width = this.Width - 24;
        
            switch (windowNumber)
            {
                case 0:
                    webBrowser1.Visible = false;
                    webBrowser2.Visible = false;
                    webBrowser3.Visible = false;
                    webBrowser4.Visible = false;

                    break;
                case 1:
                    webBrowser1.Visible = true;
                    webBrowser2.Visible = false;
                    webBrowser3.Visible = false;
                    webBrowser4.Visible = false;
                   
                    webBrowser1.Width = this.Width - 10;
                    webBrowser1.Height = he;
                    webBrowser1.Top = upperShift;
                    webBrowser1.Left = 0;
                    
                    break;
                case 2:
                    webBrowser1.Visible = true;
                    webBrowser2.Visible = true;
                    webBrowser3.Visible = false;
                    webBrowser4.Visible = false;
                    
                    webBrowser1.Width = half;
                    webBrowser1.Height = he;
                    webBrowser1.Top = upperShift;
                    webBrowser1.Left = 0;

                    webBrowser2.Width = half;
                    webBrowser2.Height = he;
                    webBrowser2.Top = upperShift;
                    webBrowser2.Left = half;
                    break;
                
                case 3:
                    webBrowser1.Visible = true;
                    webBrowser2.Visible = true;
                    webBrowser3.Visible = true;
                    webBrowser4.Visible = false;
                    
                    webBrowser1.Width = tred;
                    webBrowser1.Height = he;
                    webBrowser1.Top = upperShift;
                    webBrowser1.Left = 0;

                    webBrowser2.Width = tred;
                    webBrowser2.Height = he;
                    webBrowser2.Top = upperShift;
                    webBrowser2.Left = tred;
                
                    webBrowser3.Width = tred;
                    webBrowser3.Height = he;
                    webBrowser3.Top = upperShift;
                    webBrowser3.Left = 2*tred;
                
                    break;

                case 4:
                    webBrowser1.Visible = true;
                    webBrowser2.Visible = true;
                    webBrowser3.Visible = true;
                    webBrowser4.Visible = true;

                    webBrowser1.Width = half;
                    webBrowser1.Height = halfHe;
                    webBrowser1.Top = upperShift;
                    webBrowser1.Left = 0;

                    webBrowser2.Width = half;
                    webBrowser2.Height = halfHe;
                    webBrowser2.Top = upperShift;
                    webBrowser2.Left = half;

                    webBrowser3.Width = half;
                    webBrowser3.Height = halfHe;
                    webBrowser3.Top = upperShift + halfHe + 2;
                    webBrowser3.Left = 0;

                    webBrowser4.Width = half;
                    webBrowser4.Height = halfHe;
                    webBrowser4.Top = upperShift + halfHe + 2;
                    webBrowser4.Left = half;

                    break;

                default:
                    break;
            }
            if (textBox34.Text!="") download_page(textBox34.Text);
                
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRectangle(new SolidBrush(Color.Beige), 0, 0, 3000, 3000);
            }
            putImagesToBrowsers();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
     
            labelAb1.Parent = pictureBox1;
            labelAb1.Top = 5;
            labelAb1.Left = 5;
            labelAb1.BackColor = Color.White;//Transparent;
            labelAb1.ForeColor = Color.Black;//DarkGreen;
            labelAb1.Text = ".";

            labelAb2.Parent = pictureBox2;
            labelAb2.Top = 5;
            labelAb2.Left = 5;
            labelAb2.BackColor = Color.White;//.Transparent;
            labelAb2.ForeColor = Color.Black;//.DarkGreen;
            labelAb2.Text = ".";

            labelAb3.Parent = pictureBox3;
            labelAb3.Top = 5;
            labelAb3.Left = 5;
            labelAb3.BackColor = Color.White;//.Transparent;
            labelAb3.ForeColor = Color.Black;//.DarkGreen;
            labelAb3.Text = ".";

            labelAb4.Parent = pictureBox4;
            labelAb4.Top = 5;
            labelAb4.Left = 5;
            labelAb4.BackColor = Color.White;//.Transparent;
            labelAb4.ForeColor = Color.Black;//.DarkGreen;
            labelAb4.Text = ".";


            windowNumber = 0;
            rn = true;

            this.MouseWheel += new MouseEventHandler(wb1_MouseWheel);

            resize_browsers();
            resize_upperStrip();

            columnHeader1.Width = listView1.Size.Width - 20;//
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            columnHeader2.Width = listView2.Size.Width - 20;//
            listView2.HeaderStyle = ColumnHeaderStyle.None;

            columnHeader3.Width = listView3.Size.Width - 20;//
            listView3.HeaderStyle = ColumnHeaderStyle.None;

            string fname = "";
            if (File.Exists("xmlPth.txt"))
            {
                StreamReader sr = new StreamReader("xmlPth.txt");
                fname=   sr.ReadLine(); sr.Close();
                if (File.Exists(fname))
                {
                    label27.Text = fname;
                }
                else
                {
                    label27.Text = "none selected";
                }
            }
            contextMenuStrip1.Items[0].Click += new EventHandler(menuItmShowGene);
            contextMenuStrip1.Items[1].Click += new EventHandler(menuItmDelete);

            contextMenuStrip2.Items[0].Click += new EventHandler(menuItmShowGene2);
            contextMenuStrip2.Items[1].Click += new EventHandler(menuItmDelete2);

            contextMenuStrip3.Items[0].Click += new EventHandler(menuItmShowGene3);
            contextMenuStrip3.Items[1].Click += new EventHandler(menuItmDelete3);

            groupBox2.Top = upperShift;
            groupBox2.Left = 5;
            groupBox2.Height = this.Height - upperShift - 30;
            ///groupBox2.Height=

            groupBox1.Top = 60 + groupBox2.Height;
            groupBox1.Left = 5;

            groupBox6.Top = upperShift;
            groupBox6.Left = 5 + groupBox2.Width;
            groupBox6.Height = this.Height -upperShift-30;
            
            jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 95L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            listsList[0] = listBox1; namesList[0] = textBox1; keysList[0] = Keys.D1; labelsList[0] = label1;
            listsList[1] = listBox2; namesList[1] = textBox2; keysList[1] = Keys.D2; labelsList[1] = label2;
            listsList[2] = listBox3; namesList[2] = textBox3; keysList[2] = Keys.D3; labelsList[2] = label3;
            listsList[3] = listBox4; namesList[3] = textBox4; keysList[3] = Keys.D4; labelsList[3] = label4;
            listsList[4] = listBox5; namesList[4] = textBox5; keysList[4] = Keys.D5; labelsList[4] = label5;
            listsList[5] = listBox6; namesList[5] = textBox6; keysList[5] = Keys.D6; labelsList[5] = label6;
            listsList[6] = listBox7; namesList[6] = textBox7; keysList[6] = Keys.D7; labelsList[6] = label7;
            listsList[7] = listBox8; namesList[7] = textBox8; keysList[7] = Keys.D8; labelsList[7] = label8;
            listsList[8] = listBox9; namesList[8] = textBox9; keysList[8] = Keys.D9; labelsList[8] = label9;
            listsList[9] = listBox0; namesList[9] = textBox0; keysList[9] = Keys.D0; labelsList[9] = label0;

            for (int i = 0; i < 10; i++)
            {
                namesList[i].Text = "unnamed" + (i+1).ToString("X");
            }
            namesList[9].Text = "unnamed0";
            resize_reqLists();

        }

        private void putImagesToBrowsers()
        {
            
            //////////////// listbox 1
            listBoxAb1.Parent = webBrowser1;
            listBoxAb1.Top = 5;
            listBoxAb1.Left = 5;
            listBoxIm1.Parent = webBrowser1;
            listBoxIm1.Top = 10 + listBoxAb1.Height;
            listBoxIm1.Left = 5;

            //////////////// listbox 2
            listBoxAb2.Parent = webBrowser2;
            listBoxAb2.Top = 5;
            listBoxAb2.Left = 5;
            listBoxIm2.Parent = webBrowser2;
            listBoxIm2.Top = 10 + listBoxAb2.Height;
            listBoxIm2.Left = 5;

            //////////////// listbox 3
            listBoxAb3.Parent = webBrowser3;
            listBoxAb3.Top = 5;
            listBoxAb3.Left = 5;
            listBoxIm3.Parent = webBrowser3;
            listBoxIm3.Top = 10 + listBoxAb3.Height;
            listBoxIm3.Left = 5;

            //////////////// listbox 4
            listBoxAb4.Parent = webBrowser4;
            listBoxAb4.Top = 5;
            listBoxAb4.Left = 5;
            listBoxIm4.Parent = webBrowser4;
            listBoxIm4.Top = 10 + listBoxAb4.Height;
            listBoxIm4.Left = 5;


            /////////  panel 1
            panel1.Parent = webBrowser1;
            panel1.Width = 300;
            panel1.Height = 300;
            panel1.Left = webBrowser1.Width - 300 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            panel1.Top = 5;
            panel1.AutoScroll = false;

            /////////  panel 2
            panel2.Parent = webBrowser2;
            panel2.Width = 300;
            panel2.Height = 300;
            panel2.Left = webBrowser2.Width - 300 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            panel2.Top = 5;
            panel2.AutoScroll = false;

            /////////  panel 3
            panel3.Parent = webBrowser3;
            panel3.Width = 300;
            panel3.Height = 300;
            panel3.Left = webBrowser3.Width - 300 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            panel3.Top = 5;
            panel3.AutoScroll = false;


            /////////  panel 4
            panel4.Parent = webBrowser4;
            panel4.Width = 300;
            panel4.Height = 300;
            panel4.Left = webBrowser4.Width - 300 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
            panel4.Top = 5;
            panel4.AutoScroll = false;
        }


        private void Sort53KBtn_Click(object sender, EventArgs e)
        {
            string cc;
           
            Uri myuri;
            string s1; 
           
            int geneNumber = 0;
            int geneOk = 0;
            int p1=0;

            webBrowser1.Visible = true;
            webBrowser1.Width = this.Width;
            webBrowser1.Top = 50;
            webBrowser1.Height = this.Height - 50;
            webBrowser1.Left = 5;

            StreamWriter sw = new StreamWriter("FoundInAtlas.txt");

            using (StreamReader sr = File.OpenText("53KGenes.txt"))
            {
                cc = sr.ReadLine();

                {

                     myuri = new Uri(@"http://www.proteinatlas.org/" + cc + @"/cancer/breast+cancer");
                     webBrowser1.Navigate(myuri);
                     while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                     {
                            Application.DoEvents();
                     }

                     s1 = webBrowser1.DocumentText;
                     
                     p1 = s1.IndexOf("Antibody");
                     if (p1!=-1)
                     {
                         myuri = new Uri(@"http://www.proteinatlas.org/" + cc + @"/normal/breast");
                         webBrowser1.Navigate(myuri);
                         while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                         {
                             Application.DoEvents();
                         }

                         s1 = webBrowser1.DocumentText;

                         p1 = s1.IndexOf("Antibody");
                         if (p1 != -1)
                         {
                             // ok, there is data about gene in both normal and cancer so save it.
                             sw.WriteLine(cc);
                             geneOk += 1;
                         }

                     }
                     geneNumber += 1;
                   
                }
            }

            sw.Close();
        }



        private void SaveRestBtn_Click(object sender, EventArgs e)
        {
            if (geneNumber == 0) { MessageBox.Show("The gene list is empty. There is nothing to save. Sorry! :)", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }
            if (genesProc == geneNumber) { MessageBox.Show("You have checked all genes in the list. There is nothing to save. Sorry! :)", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            string fname = "";
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) fname = saveFileDialog1.FileName;
            if (fname == String.Empty) return;

            if (string.IsNullOrEmpty(Path.GetExtension(fname))) { fname += ".txt"; }
            StreamWriter sw = new StreamWriter(fname);
            if (genesProc<geneNumber) for (int i=genesProc+1; i<=geneNumber; i++) sw.WriteLine(ensa[i]);
            sw.Close();


        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rn)
            {
                if (comboBox1.SelectedIndex != 0)
                {
                    if (windowNumber == 0)
                    {
                        windowNumber = 1;
                        comboBox2.Enabled = true;
                        resize_universal();
                    }
                    else download_page(textBox34.Text);


                }
                else
                {
                   // textBox5.Text = (string)comboBox1.SelectedItem;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    windowNumber = 0;
                    resize_universal();
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rn)
            {
                if (comboBox2.SelectedIndex != 0)
                {
                    if (windowNumber == 1)
                    {
                        windowNumber = 2;
                        comboBox3.Enabled = true;
                        resize_universal();
                    }
                    download_page(textBox34.Text);


                }
                else
                {
                   // textBox5.Text = (string)comboBox2.SelectedItem;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    windowNumber = 1;
                    resize_universal();
                }
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rn)
            {
                if (comboBox3.SelectedIndex != 0)
                {
                    if (windowNumber == 2)
                    {
                        windowNumber = 3;
                        comboBox4.Enabled = true;
                        resize_universal();
                    }
                    download_page(textBox34.Text);

                }
                else
                {
                  //  textBox5.Text = (string)comboBox3.SelectedItem;
                    comboBox4.Enabled = false;
                    windowNumber = 2;
                    resize_universal();
                }
            }
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rn)
            {
                if (comboBox4.SelectedIndex != 0)
                {
                    if (windowNumber == 3)
                    {
                        windowNumber = 4;
                        resize_universal();
                    }
                    download_page(textBox34.Text);

                }
                else
                {
                  //  textBox5.Text = (string)comboBox4.SelectedItem;
                    windowNumber = 3;
                    resize_universal();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                labelAb1.Visible = false;
                _mousePt = e.Location;
                _tracking = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
            labelAb1.Left = -panel1.AutoScrollPosition.X + 5;
            labelAb1.Visible = true;
            _tracking = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
           if (_tracking &&
                 (pictureBox1.Image.Width > panel1.ClientSize.Width ||
                 pictureBox1.Image.Height > panel1.ClientSize.Height))
            {
                panel1.AutoScrollPosition = new Point(-panel1.AutoScrollPosition.X + (_mousePt.X - e.X),
                 -panel1.AutoScrollPosition.Y + (_mousePt.Y - e.Y));
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                labelAb2.Visible = false;
                _mousePt = e.Location;
                _tracking = true;
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            labelAb2.Top = -panel2.AutoScrollPosition.Y + 5;
            labelAb2.Left = -panel2.AutoScrollPosition.X + 5;
            labelAb2.Visible = true;
     
            _tracking = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tracking &&
                  (pictureBox2.Image.Width > panel2.ClientSize.Width ||
                  pictureBox2.Image.Height > panel2.ClientSize.Height))
            {
                panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X + (_mousePt.X - e.X),
                 -panel2.AutoScrollPosition.Y + (_mousePt.Y - e.Y));
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                labelAb3.Visible = false;
                _mousePt = e.Location;
                _tracking = true;
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
           
            labelAb3.Top = -panel3.AutoScrollPosition.Y + 5;
            labelAb3.Left = -panel3.AutoScrollPosition.X + 5;
            labelAb3.Visible = true;
     
            _tracking = false;

        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tracking &&
                  (pictureBox3.Image.Width > panel3.ClientSize.Width ||
                  pictureBox3.Image.Height > panel3.ClientSize.Height))
            {
                panel3.AutoScrollPosition = new Point(-panel3.AutoScrollPosition.X + (_mousePt.X - e.X),
                 -panel3.AutoScrollPosition.Y + (_mousePt.Y - e.Y));
            }

        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                labelAb4.Visible = false;
                _mousePt = e.Location;
                _tracking = true;
            }
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {

          //  labelAb4.Text = "Ab_" + ablist[4, curMaxAb[4, 0]] + "(" + (curMaxAb[4, 0]) + @"/" + (curMaxAb[4, 1]) + "), Sample(" + (id+1) + @"/" + imagesList4.Count() + ")";
            labelAb4.Top = -panel4.AutoScrollPosition.Y + 5;
            labelAb4.Left = -panel4.AutoScrollPosition.X + 5;
            labelAb4.Visible = true;
     
            _tracking = false;

        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tracking &&
                  (pictureBox4.Image.Width > panel4.ClientSize.Width ||
                  pictureBox4.Image.Height > panel4.ClientSize.Height))
            {
                panel4.AutoScrollPosition = new Point(-panel4.AutoScrollPosition.X + (_mousePt.X - e.X),
                 -panel4.AutoScrollPosition.Y + (_mousePt.Y - e.Y));
               
            }

        }

        private void nextGene()
        {
            if (genesProc < geneNumber)
            {
                genesProc += 1;
                download_page(ensa[genesProc]);

                this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);

                webBrowser1.Focus();
                progressBar1.Value = genesProc;
            }
            else
            {
                MessageBox.Show("You did it! The last gene was reached!", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        private void previousGene()
        {
            if (genesProc > 0)
            {
                genesProc -= 1;
                download_page(ensa[genesProc]);
                this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);
                progressBar1.Value = genesProc;
            }
            else
            {
                MessageBox.Show("First gene in the list is reached!", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }

        }


        private void nextAb(int wndNr)
        {
            if (  !(curMaxAb[1, 1] > 0 & curMaxAb[1, 0] < curMaxAb[1, 1])
                & !(curMaxAb[2, 1] > 0 & curMaxAb[2, 0] < curMaxAb[2, 1])
                & !(curMaxAb[3, 1] > 0 & curMaxAb[3, 0] < curMaxAb[3, 1])
                & !(curMaxAb[4, 1] > 0 & curMaxAb[4, 0] < curMaxAb[4, 1]))
            {
                //all antibodies were shown, process next gene 
                nextGene();
            }
            else
            {
                
                if (curMaxAb[1, 1] > 0 & curMaxAb[1, 0] < curMaxAb[1, 1])
                {
                    curMaxAb[1, 0] += 1;
                    getImageList(webBrowser1.DocumentText, 1, curMaxAb[1, 0]);
                    if (imagesList1.Count > 0) GetIt2(imagesList1[0], 1);
                }

                if (curMaxAb[2, 1] > 0 & curMaxAb[2, 0] < curMaxAb[2, 1])
                {
                    curMaxAb[2, 0] += 1;
                    getImageList(webBrowser2.DocumentText, 2, curMaxAb[2, 0]);
                    if (imagesList2.Count > 0) GetIt2(imagesList2[0], 2);
                }

                if (curMaxAb[3, 1] > 0 & curMaxAb[3, 0] < curMaxAb[3, 1])
                {
                    curMaxAb[3, 0] += 1;
                    getImageList(webBrowser3.DocumentText, 3, curMaxAb[3, 0]);
                    if (imagesList3.Count > 0) GetIt2(imagesList3[0], 3);
                }

                if (curMaxAb[4, 1] > 0 & curMaxAb[4, 0] < curMaxAb[4, 1])
                {
                    curMaxAb[4, 0] += 1;
                    getImageList(webBrowser4.DocumentText, 4, curMaxAb[4, 0]);
                    if (imagesList4.Count > 0) GetIt2(imagesList4[0], 4);
                }

            }
        }

        private void previousAb(int wndNr)
        {
            if (!(curMaxAb[1, 1] > 0 & curMaxAb[1, 0] > 0)
                & !(curMaxAb[2, 1] > 0 & curMaxAb[2, 0] > 0)
                & !(curMaxAb[3, 1] > 0 & curMaxAb[3, 0] > 0)
                & !(curMaxAb[4, 1] > 0 & curMaxAb[4, 0] > 0))
            {  //Antibody list is finished, return to previous gene
                previousGene();
            }
            else
            {

                if (curMaxAb[1, 1] > 0 & curMaxAb[1, 0] > 0)
                {
                    curMaxAb[1, 0] -= 1;
                    getImageList(webBrowser1.DocumentText, 1, curMaxAb[1, 0]);
                    if (imagesList1.Count > 0) GetIt2(imagesList1[0], 1);
                }

                if (curMaxAb[2, 1] > 0 & curMaxAb[2, 0] > 0)
                {
                    curMaxAb[2, 0] -= 1;
                    getImageList(webBrowser2.DocumentText, 2, curMaxAb[2, 0]);
                    if (imagesList2.Count > 0) GetIt2(imagesList2[0], 2);
                }

                if (curMaxAb[3, 1] > 0 & curMaxAb[3, 0] > 0)
                {
                    curMaxAb[3, 0] -= 1;
                    getImageList(webBrowser3.DocumentText, 3, curMaxAb[3, 0]);
                    if (imagesList3.Count > 0) GetIt2(imagesList3[0], 3);
                }

                if (curMaxAb[4, 1] > 0 & curMaxAb[4, 0] > 0)
                {
                    curMaxAb[4, 0] -= 1;
                    getImageList(webBrowser4.DocumentText, 4, curMaxAb[4, 0]);
                    if (imagesList4.Count > 0) GetIt2(imagesList4[0], 4);
                }
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            nextImg(1);
        }


        private void nextImg(int wndNr) 
        {
            if (!(imagesList1.Count > ia + sa) & !(imagesList2.Count > ib + sb) & !(imagesList3.Count > ic + sc) & !(imagesList4.Count > id + sd))//end of image list for this Ab reached 
            {                                                                                                                                 //next Ab needed
                nextAb(wndNr);
            }
            else// change image
            {

                if (imagesList1.Count > ia + sa) { ia += sa; GetIt2(imagesList1[ia], 1); };
                if (imagesList2.Count > ib + sb) { ib += sb; GetIt2(imagesList2[ib], 2); };
                if (imagesList3.Count > ic + sc) { ic += sc; GetIt2(imagesList3[ic], 3); };
                if (imagesList4.Count > id + sd) { id += sd; GetIt2(imagesList4[id], 4); };
            }
            
            wheelProcessing = false;
        }

        private void previousImg(int wndNr) 
        {
            if (!(ia - sa >= 0) & !(ib - sb >= 0) & !(ic - sc >= 0) & !(id - sd >= 0))
            {
                previousAb(wndNr);
            }
            else
            {
                if (ia - sa >= 0) { ia -= sa; GetIt2(imagesList1[ia], 1); };
                if (ib - sb >= 0) { ib -= sb; GetIt2(imagesList2[ib], 2); };
                if (ic - sc >= 0) { ic -= sc; GetIt2(imagesList3[ic], 3); };
                if (id - sd >= 0) { id -= sd; GetIt2(imagesList4[id], 4); };
            }
            wheelProcessing = false;

        }
  

        private void wb1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!wheelProcessing)
            {
                wheelProcessing = true;
                if (e.Delta > 0) { nextImg(1); }
                else { previousImg(1); }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                groupBox6.SendToBack();
                groupBox1.SendToBack();
                groupBox2.SendToBack();

                listBoxAb1.Visible = false;
                listBoxAb2.Visible = false;
                listBoxAb3.Visible = false;
                listBoxAb4.Visible = false;

                listBoxIm1.Visible = false;
                listBoxIm2.Visible = false;
                listBoxIm3.Visible = false;
                listBoxIm4.Visible = false;

                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;

                switch (windowNumber)
                {
                    case 0:
                        webBrowser1.Visible = false;
                        webBrowser2.Visible = false;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;
                    case 1:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = false;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;
                    case 2:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;

                    case 3:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = true;
                        webBrowser4.Visible = false;
                        break;

                    case 4:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = true;
                        webBrowser4.Visible = true;
                        break;

                    default:
                        break;
                }
                resize_browsers();
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                groupBox6.SendToBack();
                groupBox1.SendToBack();
                groupBox2.SendToBack();

                putImagesToBrowsers();

                listBoxAb1.Visible = true;
                listBoxAb2.Visible = true;
                listBoxAb3.Visible = true;
                listBoxAb4.Visible = true;

                listBoxIm1.Visible = true;
                listBoxIm2.Visible = true;
                listBoxIm3.Visible = true;
                listBoxIm4.Visible = true;

                panel1.Visible = true;
                panel2.Visible = true;
                panel3.Visible = true;
                panel4.Visible = true;

                switch (windowNumber)
                {
                    case 0:
                        webBrowser1.Visible = false;
                        webBrowser2.Visible = false;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;
                    case 1:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = false;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;
                    case 2:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = false;
                        webBrowser4.Visible = false;
                        break;

                    case 3:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = true;
                        webBrowser4.Visible = false;
                        break;

                    case 4:
                        webBrowser1.Visible = true;
                        webBrowser2.Visible = true;
                        webBrowser3.Visible = true;
                        webBrowser4.Visible = true;
                        break;

                    default:
                        break;
                }
                resize_browsers();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
               

                listBoxAb1.Visible = false;
                listBoxAb2.Visible = false;
                listBoxAb3.Visible = false;
                listBoxAb4.Visible = false;

                listBoxIm1.Visible = false;
                listBoxIm2.Visible = false;
                listBoxIm3.Visible = false;
                listBoxIm4.Visible = false;

                panel1.Parent = this;
                panel2.Parent = this;
                panel3.Parent = this;
                panel4.Parent = this;

                panel1.Visible = true;
                panel2.Visible = true;
                panel3.Visible = true;
                panel4.Visible = true;

                groupBox6.SendToBack();
                groupBox1.SendToBack();
                groupBox2.SendToBack();

                if (textBox34.Text != "") download_page(textBox34.Text);
                resize_panels();
                radioButton3.Focus();

                webBrowser1.Visible = false;
                webBrowser2.Visible = false;
                webBrowser3.Visible = false;
                webBrowser4.Visible = false;
            }
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            listBoxAb1.Visible = false;
            listBoxAb2.Visible = false;
            listBoxAb3.Visible = false;
            listBoxAb4.Visible = false;

            listBoxIm1.Visible = false;
            listBoxIm2.Visible = false;
            listBoxIm3.Visible = false;
            listBoxIm4.Visible = false;

            webBrowser1.Visible = false;
            webBrowser2.Visible = false;
            webBrowser3.Visible = false;
            webBrowser4.Visible = false;

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }
       

        private void resize_panels()
        {
            const int upperShift = 40;

            int half = 0;
            half = (int)Math.Round((double)(this.Width - 10) / 2);
            int tred = (int)Math.Round((double)(this.Width - 10) / 3);
            int he = this.Height - upperShift - 28;
            int halfHe = (int)Math.Round((double)he / 2);

            //  progressBar1.Width = this.Width - 24;

            switch (windowNumber)
            {
                case 0:
                    panel1.Visible = false;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = false;

                    break;
                case 1:
                    panel1.Visible = true;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = false;

                    panel1.Width = this.Width - 10;
                    panel1.Height = he;
                    panel1.Top = upperShift;
                    panel1.Left = 0;
                    panel1.AutoScrollPosition = new Point((pictureBox1.Width - panel1.Width) / 2, (pictureBox1.Height - panel1.Height) / 2);
                    labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
                    labelAb1.Left = -panel1.AutoScrollPosition.X + 5;

                    break;
                case 2:
                    panel1.Visible = true;
                    panel2.Visible = true;
                    panel3.Visible = false;
                    panel4.Visible = false;

                    panel1.Width = half;
                    panel1.Height = he;
                    panel1.Top = upperShift;
                    panel1.Left = 0;

                    panel2.Width = half;
                    panel2.Height = he;
                    panel2.Top = upperShift;
                    panel2.Left = half;
                    
                    panel1.AutoScrollPosition = new Point((pictureBox1.Width - panel1.Width) / 2, (pictureBox1.Height - panel1.Height) / 2);
                    panel2.AutoScrollPosition = new Point((pictureBox2.Width - panel2.Width) / 2, (pictureBox2.Height - panel2.Height) / 2);
                    labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
                    labelAb1.Left = -panel1.AutoScrollPosition.X + 5;
                    labelAb2.Top = -panel2.AutoScrollPosition.Y + 5;
                    labelAb2.Left = -panel2.AutoScrollPosition.X + 5;
                    
                    break;

                case 3:
                    panel1.Visible = true;
                    panel2.Visible = true;
                    panel3.Visible = true;
                    panel4.Visible = false;

                    panel1.Width = tred;
                    panel1.Height = he;
                    panel1.Top = upperShift;
                    panel1.Left = 0;

                    panel2.Width = tred;
                    panel2.Height = he;
                    panel2.Top = upperShift;
                    panel2.Left = tred;

                    panel3.Width = tred;
                    panel3.Height = he;
                    panel3.Top = upperShift;
                    panel3.Left = 2 * tred;
                    
                    panel1.AutoScrollPosition = new Point((pictureBox1.Width - panel1.Width) / 2, (pictureBox1.Height - panel1.Height) / 2);
                    panel2.AutoScrollPosition = new Point((pictureBox2.Width - panel2.Width) / 2, (pictureBox2.Height - panel2.Height) / 2);
                    panel3.AutoScrollPosition = new Point((pictureBox3.Width - panel3.Width) / 2, (pictureBox3.Height - panel3.Height) / 2);
                    labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
                    labelAb1.Left = -panel1.AutoScrollPosition.X + 5;
                    labelAb2.Top = -panel2.AutoScrollPosition.Y + 5;
                    labelAb2.Left = -panel2.AutoScrollPosition.X + 5;
                    labelAb3.Top = -panel3.AutoScrollPosition.Y + 5;
                    labelAb3.Left = -panel3.AutoScrollPosition.X + 5;
                    
                    
                    break;

                case 4:
                    panel1.Visible = true;
                    panel2.Visible = true;
                    panel3.Visible = true;
                    panel4.Visible = true;

                    panel1.Width = half;
                    panel1.Height = halfHe;
                    panel1.Top = upperShift;
                    panel1.Left = 0;

                    panel2.Width = half;
                    panel2.Height = halfHe;
                    panel2.Top = upperShift;
                    panel2.Left = half;

                    panel3.Width = half;
                    panel3.Height = halfHe;
                    panel3.Top = upperShift + halfHe + 2;
                    panel3.Left = 0;

                    panel4.Width = half;
                    panel4.Height = halfHe;
                    panel4.Top = upperShift + halfHe + 2;
                    panel4.Left = half;

                    panel1.AutoScrollPosition = new Point((pictureBox1.Width - panel1.Width) / 2, (pictureBox1.Height - panel1.Height) / 2);
                    panel2.AutoScrollPosition = new Point((pictureBox2.Width - panel2.Width) / 2, (pictureBox2.Height - panel2.Height) / 2);
                    panel3.AutoScrollPosition = new Point((pictureBox3.Width - panel3.Width) / 2, (pictureBox3.Height - panel3.Height) / 2);
                    panel4.AutoScrollPosition = new Point((pictureBox4.Width - panel4.Width) / 2, (pictureBox4.Height - panel4.Height) / 2);
                    labelAb1.Top = -panel1.AutoScrollPosition.Y + 5;
                    labelAb1.Left = -panel1.AutoScrollPosition.X + 5;
                    labelAb2.Top = -panel2.AutoScrollPosition.Y + 5;
                    labelAb2.Left = -panel2.AutoScrollPosition.X + 5;
                    labelAb3.Top = -panel3.AutoScrollPosition.Y + 5;
                    labelAb3.Left = -panel3.AutoScrollPosition.X + 5;
                    labelAb4.Top = -panel4.AutoScrollPosition.Y + 5;
                    labelAb4.Left = -panel4.AutoScrollPosition.X + 5;
                    
                    
                    break;

                default:
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox34.Text != "") download_page(textBox34.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (geneNumber == 0) { MessageBox.Show("The gene list is empty. Nothing to save. Sorry! :)", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            string fname="";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) fname = saveFileDialog1.FileName;
            if (fname == String.Empty) return;

            if (string.IsNullOrEmpty(Path.GetExtension(fname))) { fname += ".txt"; }

            StreamWriter sw = new StreamWriter(fname);
            foreach (ListViewItem item in listView1.Items)
            {
                sw.WriteLine(item.Text);
            }
            sw.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            label26.Text = String.Format("Selected genes list two ({0})", listView3.Items.Count);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0) { MessageBox.Show("The gene list is empty. Nothing to save. Sorry! :)", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            string fname = "";
            saveFileDialog1.FileName = "SelectedGenesOne.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) fname = saveFileDialog1.FileName;
            if (fname == String.Empty) return;

            if(string.IsNullOrEmpty(Path.GetExtension(fname))) { fname += ".txt";}

            StreamWriter sw = new StreamWriter(fname);
            foreach (ListViewItem item in listView2.Items)
            {
                sw.WriteLine(item.Text);
            }
            sw.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView3.Items.Count == 0) { MessageBox.Show("The gene list is empty. Nothing to save. Sorry! :)", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            string fname = "";
            saveFileDialog1.FileName = "SelectedGenesTwo.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) fname = saveFileDialog1.FileName;
            if (fname == String.Empty) return;

            if (string.IsNullOrEmpty(Path.GetExtension(fname))) { fname += ".txt"; }
            StreamWriter sw = new StreamWriter(fname);
            foreach (ListViewItem item in listView3.Items)
            {
                sw.WriteLine(item.Text);
            }
            sw.Close();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.proteinatlas.org/about/download");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fname="";
            openFileDialog1.DefaultExt = "XML";
            openFileDialog1.FileName = "proteinatals.xml";
            openFileDialog1.Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog1.ShowDialog() == DialogResult.OK) fname = openFileDialog1.FileName;
            if (fname == String.Empty) return;
            if (File.Exists(fname))
            {
                StreamWriter sw = new StreamWriter("xmlPth.txt");
                sw.WriteLine(fname); sw.Close();
                label27.Text = fname;
            }
            openFileDialog1.Filter = null;


        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    genesProc = listView1.FocusedItem.Index;
                    groupBox7.Text = String.Format("Genes to analyze ({0}/{1})", genesProc+1, geneNumber);
                   // label3.Text = String.Format("Genes ({0}/{1})", genesProc + 1 , geneNumber);
                    this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);
                    progressBar1.Value = genesProc;

                }
            }
        }

        private void menuItmShowGene(object sender, EventArgs e)
        {
            Process.Start(@"http://www.proteinatlas.org/"+listView1.FocusedItem.Text);
          
        }

        private void menuItmDelete(object sender, EventArgs e)
        {

            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                listView1.Items.Remove(lvi);
            }


            geneNumber = listView1.Items.Count;
            if (genesProc > geneNumber) genesProc = geneNumber;
            listView1.FocusedItem = listView1.Items[genesProc];

            groupBox7.Text = String.Format("Genes to analyze ({0}/{1})", genesProc + 1, geneNumber);
           // label3.Text = String.Format("Genes ({0}/{1})", genesProc +1, geneNumber);
            this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);
            progressBar1.Maximum = geneNumber-1;

            progressBar1.Value = genesProc;
            

            //Process.Start(@"http://www.proteinatlas.org/" + listView1.FocusedItem.Text);

        }

        private void menuItmShowGene2(object sender, EventArgs e)
        {
            Process.Start(@"http://www.proteinatlas.org/" + listView2.FocusedItem.Text);

        }

        private void menuItmDelete2(object sender, EventArgs e)
        {

            foreach (ListViewItem lvi in listView2.SelectedItems)
            {
                listView2.Items.Remove(lvi);
            }

            label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);

        }

        private void menuItmShowGene3(object sender, EventArgs e)
        {
            Process.Start(@"http://www.proteinatlas.org/" + listView3.FocusedItem.Text);

        }

        private void menuItmDelete3(object sender, EventArgs e)
        {

            foreach (ListViewItem lvi in listView3.SelectedItems)
            {
                listView3.Items.Remove(lvi);
            }

            label26.Text = String.Format("Selected genes list two ({0})", listView3.Items.Count);

        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.FocusedItem.Index != -1)
            {
                genesProc = listView1.FocusedItem.Index;
                groupBox7.Text = String.Format("Genes to analyze ({0}/{1})", genesProc +1, geneNumber);
                //label3.Text = String.Format("Genes ({0}/{1})", genesProc+1, geneNumber);
                this.Text = String.Format(programName + "Genes ({0}/{1})", genesProc + 1, geneNumber);
                progressBar1.Value = genesProc;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
             string inp,fname;

            fname = "";

            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "TXT Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog1.ShowDialog() == DialogResult.OK) fname = openFileDialog1.FileName;
            if (fname == String.Empty) return;

            genesr = File.OpenText(fname);
            

            if (genesr != null)
            {
                opened = true;
                geneNumber=0;
                while ((inp = genesr.ReadLine()) != null)
                {
                    if (inp.IndexOf("ENSG") == -1) 
                    { 
                        MessageBox.Show(String.Format("file contains non ENSG coded gene at line {0}.", listView2.Items.Count+1), "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);
                        return; 
                    }
                    listView2.Items.Add(inp);
                    if (listView2.Items.Count % 1000 == 0) {  label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);; Application.DoEvents(); }
                }
                
            }

            label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView2.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip2.Show(Cursor.Position);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label25.Text = label25.Text = String.Format("Downloading images, genes done ({0}/{1})", 0, listView2.Items.Count);
         label25.Font = new Font(label25.Font, FontStyle.Bold);
            Random rnd1 = new Random();
            if (listView2.Items.Count == 0) { MessageBox.Show("The gene list is empty!", "AtlasGrabber", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }

            int typeOfAbCapt=0;
            if (radioButton4.Checked) typeOfAbCapt=1;
            if (radioButton5.Checked) typeOfAbCapt=2;
            if (radioButton6.Checked) typeOfAbCapt=3;
            if (radioButton7.Checked) typeOfAbCapt=4;
            if (radioButton8.Checked) typeOfAbCapt=5;
           
            StreamWriter sw = new StreamWriter("testAb2.txt");
            string pth = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) pth = folderBrowserDialog1.SelectedPath;
            if (pth == String.Empty) return;

            string ensg;
            for (int i = 0; i < listView2.Items.Count; i++)//listView2.Items.Count;
            {
                ensg = listView2.Items[i].Text;



                label25.Text = label25.Text = String.Format("Downloading images, genes done ({0}/{1})", i+1, listView2.Items.Count);
                Application.DoEvents();
                webBrowser1.Top = upperShift;
                webBrowser1.Left = 12 + groupBox2.Width + 12;
                webBrowser1.Width = (int)Math.Round((double)(this.Width - 24 - groupBox2.Width));
                webBrowser1.Visible = true;

              //  if (comboBox1.SelectedIndex != 0 & comboBox1.Enabled)
                {

                    webBrowser1.Navigate(new Uri("http://proteinatlas.org/" + ensg + @"/" + (string)comboBox1.SelectedItem));
                    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                    {
                      Application.DoEvents();
                    }

                    getAbListExt(webBrowser1.DocumentText);

                    abToGetS = abToGet.OrderBy(o => o.ABPos).ToList();

                  //  for (int j = 0; j < abToGet.Count; j++)
               //     { // get image list for current AB and get images according to the image rule.
                       
               //         sw.WriteLine("{0} {1} {2} {3}",ensg, abToGetS[j].ABMark, abToGetS[j].ABCode, abToGetS[j].ABPos);


                        
             //       }
                    int k;
                    if (abToGetS.Count > 0)
                    {
                        switch (typeOfAbCapt)
                        {
                            case 1: // sort AB list to get the firs one
                                k = 0;
                                sw.WriteLine("{0} {1} {2} {3}", ensg, abToGetS[k].ABMark, abToGetS[k].ABCode, abToGetS[k].ABPos);
                                processImageList(pth);
                                saveList();
                                break;

                            case 2: // get random out of number 

                                k = rnd1.Next(abToGetS.Count);
                               
                                if (k >= abToGetS.Count) k = abToGetS.Count - 1;
                                if (k < 0) k = 0;

                                sw.WriteLine("{0} {1} {2} {3}", ensg, abToGetS[k].ABMark, abToGetS[k].ABCode, abToGetS[k].ABPos);
                                processImageList(pth);
                                saveList();
                            //        sw.Flush();
                                break;

                            case 3: // All CABs should be loaded
                                for (k = 0; k < abToGetS.Count; k++)
                                {
                                    if (abToGetS[k].ABMark == "CAB")
                                    {
                                        sw.WriteLine("{0} {1} {2} {3}", ensg, abToGetS[k].ABMark, abToGetS[k].ABCode, abToGetS[k].ABPos);
                                        processImageList(pth);
                                        saveList();
                                    }
                                }

                                break;
                            case 4: // All HPAs should be loaded
                                for (k = 0; k < abToGetS.Count; k++)
                                {

                                    if (abToGetS[k].ABMark == "HPA")
                                    {
                                        sw.WriteLine("{0} {1} {2} {3}", ensg, abToGetS[k].ABMark, abToGetS[k].ABCode, abToGetS[k].ABPos);
                                        processImageList(pth);
                                        saveList();
                                    }
                                }
                                break;
                            case 5: // Just everythig 
                                for (k = 0; k < abToGetS.Count; k++)
                                {
                                    sw.WriteLine("{0} {1} {2} {3}", ensg, abToGetS[k].ABMark, abToGetS[k].ABCode, abToGetS[k].ABPos);
                                    getImageListExt(webBrowser1.DocumentText, abToGetS[k].ABCode, abToGetS[k].ABMark, ensg);
                                    processImageList(pth);
                                    saveList();
                                }
                                break;
                        }

                        //if (radioButton4.Checked)

                        //getImageList(webBrowser1.DocumentText, 1, curMaxAb[1, 0]);
                        //if (curMaxAb[1, 1]>0) 
                        //if (imagesList1.Count > 0) GetIt2(imagesList1[0], 1); else showEmpty(1);
                    }
                }

               
                 //GetIt2(imagesList1[0], 1);
            }

            sw.Close();
            label25.Text = String.Format("Selected genes list one ({0})", listView2.Items.Count);
            label25.Font = new Font(label25.Font, FontStyle.Regular);
        }

        private void processImageList(string pth)
        {
            Random rnd1 = new Random();
            int typeOfImCapt = 0;
            if (radioButton9.Checked) typeOfImCapt = 1;
            if (radioButton10.Checked) typeOfImCapt = 2;
            if (radioButton11.Checked) typeOfImCapt = 3;
            int k;
           // Image im;
            string fn;
            string url;
            WebClient webClient;
            if (imagesToGet.Count > 0)
            {
                switch (typeOfImCapt)
                {
                    case 1: // first one
                        k = 0;
                        //im = DownloadImage(@"http://www.proteinatlas.org" + imagesToGet[k].imageAddress);
                        url = @"http://www.proteinatlas.org" + imagesToGet[k].imageAddress;
                        fn = @"\" + imagesToGet[k].ensg + "_" + imagesToGet[k].ABCode.ToString("D5") + "_" + imagesToGet[k].PID.ToString("D4") + "_" + k.ToString("D2") + ".jpg";
                        //im.Save(pth+fn, jgpEncoder, myEncoderParameters);
                        webClient = new WebClient();
                        webClient.DownloadFile(url, pth + fn);
                        
                        break;
                    case 2: // one but random
                        k = rnd1.Next(imagesToGet.Count);
                        if (k >= imagesToGet.Count) k = imagesToGet.Count - 1;
                        if (k < 0) k = 0;
                        //im = DownloadImage(@"http://www.proteinatlas.org" + imagesToGet[k].imageAddress);
                        url = @"http://www.proteinatlas.org" + imagesToGet[k].imageAddress;
                        fn = @"\" + imagesToGet[k].ensg + "_" + imagesToGet[k].ABCode.ToString("D5") + "_" + imagesToGet[k].PID.ToString("D4") + "_" + k.ToString("D2") + ".jpg";
                        //im.Save(pth+fn, jgpEncoder, myEncoderParameters);
                        webClient = new WebClient();
                        webClient.DownloadFile(url, pth + fn);
                        
                        
                        break;
                    case 3: // all of them
                        for (k = 0; k < imagesToGet.Count; k++)
                        {
                         //   im = DownloadImage(@"http://www.proteinatlas.org" + imagesToGet[k].imageAddress);
                            url = @"http://www.proteinatlas.org" + imagesToGet[k].imageAddress;
                            fn = @"\" + imagesToGet[k].ensg + "_" + imagesToGet[k].ABCode.ToString("D5") + "_" + imagesToGet[k].PID.ToString("D4") + "_" + k.ToString("D2") + ".jpg";
                            //im.Save(pth + fn, jgpEncoder, myEncoderParameters);

                            webClient = new WebClient();
                            webClient.DownloadFile(url, pth + fn);
                        
                        }
                        
                        break;
                }
            }

        }

        private void saveList()
        {
            StreamWriter sw = File.AppendText("imageList.txt");
            
            for (int i = 0; i < imagesToGet.Count; i++)
            {
                sw.WriteLine("{0} {1} {2} {3}", imagesToGet[i].ensg, imagesToGet[i].ABMark, imagesToGet[i].PID, imagesToGet[i].imageAddress);
            }
            sw.WriteLine();
            sw.Close();
        }


        private void getAbListExt(string s1)
        {
            int abn;
            int firstCharacter = 0; abcount = 0;
            string s2;

            abToGet.Clear();

            do
            {
                firstCharacter = s1.IndexOf("CAB", firstCharacter + 4);
                if (firstCharacter != -1)
                {
                    s2 = s1.Substring(firstCharacter + 3, 6);//copy ab number

                    try
                    {
                        abn = Convert.ToInt32(s2);
                        abToGet.Add(new abDescr{ABCode=abn,ABMark="CAB",ABPos=firstCharacter});
                        //abcount += 1;
                        //ablist[windowNumber, abcount] = abn;

                       
                    }
                    catch (FormatException)
                    {
                        exc += 1; //label10.Text = "E:" + exc;
                    }

                }
            } while (firstCharacter != -1);

            firstCharacter = 0;
            do
            {
                firstCharacter = s1.IndexOf("HPA", firstCharacter + 4);
                if (firstCharacter != -1)
                {
                    s2 = s1.Substring(firstCharacter, 15);
                    char c = s1[firstCharacter + 3];
                    if (c == '0' | c == '1' | c == '2' | c == '3' | c == '4' | c == '5' | c == '6' | c == '7' | c == '8' | c == '9')
                    {
                        s2 = s1.Substring(firstCharacter + 3, 6);//copy ab number

                        try
                        {
                            abn = Convert.ToInt32(s2);
                            abToGet.Add(new abDescr{ABCode=abn,ABMark="HPA",ABPos=firstCharacter});
                        
                            abcount += 1;
                            ablist[windowNumber, abcount] = abn;
                        }
                        catch (FormatException)
                        {
                            exc += 1; 
                        }
                    }
                }
            } while (firstCharacter != -1);


        }

        private void getImageListExt(string s1, int abCode,  string abMark, string ensg)
        {
            string s2, as3, s3;

            imagesToGet.Clear();
            pIDList1.Clear();


            int firstCharacter = 0;
            int secondCharacter = 0;
            //int imagesCount = 0;

            int pID = 0;
            int med2;
            as3 = "\"\""; as3 = as3.Substring(1);

            do
            {
                firstCharacter = s1.IndexOf(@"/images/" + abCode, firstCharacter + 4);
                if (firstCharacter != -1)
                {

                    s2 = s1.Substring(firstCharacter, 150);
                    secondCharacter = s2.IndexOf(as3);

                    s2 = s2.Substring(0, secondCharacter);

                    secondCharacter = s2.IndexOf("thumb");
                    if (secondCharacter == -1) //full res, not thumbnails
                    {
                        secondCharacter = s2.IndexOf("medium");
                        if (secondCharacter == -1) //full res, not thumbnails
                        {
                            secondCharacter = s2.IndexOf("_selected_60x60");
                            if (secondCharacter == -1) //it is real image, not some background stuff
                            {
                                s3 = s1.Substring(firstCharacter, 540);
                                med2 = s3.IndexOf("Patient id");

                                if (med2 != -1)
                                {
                                    string s4 = s3.Substring(med2 + 15, 6);
                                    med2 = s4.IndexOf("<");
                                    pID = Convert.ToInt16(s4.Substring(0, med2));
                                    //}

                                    if (checkBox1.Checked) // filter out samples with same PID
                                    {
                                        if (!pIDList1.Contains(pID))
                                        {
                                            pIDList1.Add(pID);

                                            imagesToGet.Add(new imageDescr { ABCode = abCode, ABMark = abMark, ensg = ensg, PID = pID, imageAddress = s2 });
                                            //imagesList1.Add(s2); 
                                            //listBoxIm1.Items.Add(s2); 
                                            //imagesCount += 1; 
                                        }
                                    }
                                    else
                                    {
                                        pIDList1.Add(pID);
                                        imagesToGet.Add(new imageDescr { ABCode = abCode, ABMark = abMark, ensg = ensg, PID = pID, imageAddress = s2 });
                                        //imagesList1.Add(s2); 
                                        //listBoxIm1.Items.Add(s2); 
                                        //imagesCount += 1;
                                    }
                                    //      break;

                                }
                            }
                        }
                    }

                }

            } while (firstCharacter != -1);

            //switch (window)
            //{
            //    case 1: listBoxIm1.Items.Add(imagesCount); curMaxImg[1, 0] = 0; curMaxImg[1, 1] = imagesCount; break;
            //    case 2: listBoxIm2.Items.Add(imagesCount); curMaxImg[2, 0] = 0; curMaxImg[2, 1] = imagesCount; break;
            //    case 3: listBoxIm3.Items.Add(imagesCount); curMaxImg[3, 0] = 0; curMaxImg[3, 1] = imagesCount; break;
            //    case 4: listBoxIm4.Items.Add(imagesCount); curMaxImg[4, 0] = 0; curMaxImg[4, 1] = imagesCount; break;
            //    default: break;
            //}
        }

        private void button9_Click(object sender, EventArgs e)
        {
            StreamWriter sw;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string pth = folderBrowserDialog1.SelectedPath;

                for (int i = 0; i < 10; i++)
                {
                    int itemCount = listsList[i].Items.Count;
                    if (itemCount != 0)
                    {
                        sw = new StreamWriter(pth + @"\" + namesList[i].Text + ".txt");

                        for (int j = 0; j < itemCount; j++)
                        {
                            sw.WriteLine((string)listsList[i].Items[j]);
                            
                        }
                        sw.Close();
                    }
                    else
                    {
                        if (checkBox3.Checked)
                        {

                            sw = new StreamWriter(pth + @"\" + namesList[i].Text + ".txt");
                            sw.Close();
                        }
                    }

                }
            }

        }  

        private void button10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://github.com/b3nb0z/AtlasGrabber/blob/master/README.md");
        }

        private void resize_upperStrip()
        {
            int wi = this.Width - 152- 33;
            progressBar1.Width = wi-33;

            int cmbw = wi / 4 - 33;
            comboBox1.Width = cmbw;

            comboBox2.Width = cmbw;
            comboBox3.Width = cmbw;
            comboBox4.Width = cmbw;

            label13.Left = comboBox1.Right + 4;
            comboBox2.Left = label13.Right - 1;

            label14.Left = comboBox2.Right + 4;
            comboBox3.Left = label14.Right - 1;

            label15.Left = comboBox3.Right + 4;
            comboBox4.Left = label15.Right - 1;



        }

        private void parser(bool shortRun, string tissueType)
        {
            string XMLpth; string outputpth;
            openFileDialog1.FileName = "proteinatlas.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XMLpth = openFileDialog1.FileName;
                string validName = tissueType.Replace(@"/", "_");

                outputpth = Path.GetDirectoryName(XMLpth) + @"\" + validName + ".txt";
                //label2.Text = "Output:" + outputpth;
                this.Width = label2.Width + 30;

                int i = 0; int j = 0;
                string ensg = "";
                string abId = "";
                string tissue = "";
                string tissueDescr = "";
                string iurl = "";
                string pid = "";
                int ensn = 0;
                {
                    StreamWriter sw = new StreamWriter(outputpth);
                    XmlTextReader reader = new XmlTextReader(XMLpth);
                    while (reader.Read() && j < 100000) // j wil be incremenrted in short run only
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "identifier")        //doing nothing for the moment, just counting ENSG's
                                {
                                    ensg = reader.GetAttribute("id");
                                    //sw.WriteLine(ensg); 
                                    ensn++;
                                }

                                if (reader.Name == "antibody")          //doing nothing for the moment, could be used to create dictionaries.
                                {
                                    abId = reader.GetAttribute("id");
                                    //  sw.WriteLine(ensg + " " + abId);    
                                }

                                if (reader.Name == "tissue")            //doing nothing for the moment
                                {
                                    tissue = reader.ReadElementContentAsString();
                                    // sw.WriteLine(ensg + " " + abId+" "+tissue);
                                }

                                if ((reader.Name == "patient") && (tissue == tissueType))
                                {
                                    reader.ReadToFollowing("patientId");
                                    pid = reader.ReadElementContentAsString();

                                    reader.ReadToFollowing("snomed");
                                    tissueDescr = reader.GetAttribute("tissueDescription");
                                    tissueDescr += ", ";
                                    reader.ReadToFollowing("snomed");
                                    tissueDescr += reader.GetAttribute("tissueDescription");
                                    reader.ReadToFollowing("imageUrl");
                                    iurl = reader.ReadElementContentAsString();
                                    sw.WriteLine(ensg + ";" + abId + ";" + tissue + ";" + tissueDescr + ";" + pid + ";" + iurl);
                                }
                                break;
                        }
                        i++;
                        if (i % 10000 == 0) { groupBox8.Text = "XML parser " + (i / 1000).ToString() + "k lines analysed " + ensn.ToString() + " ENSG's found"; Application.DoEvents(); }
                        if (shortRun) j++;

                    }

                    sw.Close();
                    if (shortRun) System.Diagnostics.Process.Start(outputpth);

                }
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            parser(true, (string)comboBox5.SelectedItem);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            parser(false, (string)comboBox5.SelectedItem);
        }
     
    }
}



// tempelroy dubometer
//Sounds From The Ground - Loaf