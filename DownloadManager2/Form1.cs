using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DownloadManager2
{
    public partial class Form1 : Form
      
      {


        //Suffixes
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        int bytesIn,totalBytes;
        int lastDownload = 0;
        string  EndPathFileNam;
        
        

       public Form1()
        {
            InitializeComponent();
            
        }
       //function to get raw bytes and converts them to suffixed-bytes
       static string SizeSuffix(Int64 value)
       {
           if (value < 0) { return "-" + SizeSuffix(-value); }
           if (value == 0) { return "0.0 bytes"; }

           int mag = (int)Math.Log(value, 1024);
           decimal adjustedSize = (decimal)value / (1L << (mag * 10));

           return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
       }

        //Load the browse option on demand
       private void loadbtn_Click(object sender, EventArgs e)
       {
           savePath.HideSelection = true;

           LoadFolder();
       }
        
       //Method to load folder on browse click
        private void LoadFolder()
        {

            // Folder loader

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
              
                savePath.Text = folderBrowserDialog1.SelectedPath;
                
              
            }
           

        }
        
        // Download button gets us to downloading process and monitors progress
        private void downloadbtn_Click(object sender, EventArgs e)
        {
              WebClient myWebClient = new WebClient();
             //Declarations for string objects
             string downloadURL, path;
             //raw URL taken from user
             downloadURL =  this.downloadURL.Text;
             path = savePath.Text;
             Uri tmp = new Uri(downloadURL);
             var segments=tmp.Segments;
             Console.WriteLine(segments);

            //get the name of file to append it to location on drive
              HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tmp);
              request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
             request.Credentials = CredentialCache.DefaultCredentials;
             HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
             Uri uri = (Uri)response.ResponseUri;
             EndPathFileNam = uri.Segments.Last();
             path = path + @"\" + EndPathFileNam;
            // string EndPathFileName = tmp.Segments.Last();
             
           
            //downloads file using async method
         //  MessageBox.Show(downloadURL);
         //  MessageBox.Show(savePath.Text);
           myWebClient.DownloadFileAsync(tmp, path);

           downloadbtn.Text = "Download In Process";
           loadbtn.Enabled = false;
           downloadbtn.Enabled = false;
           myWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
           myWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

          
           // Set some reasonable limits on resources used by this request
         //  request.MaximumAutomaticRedirections = 4;
         //  request.MaximumResponseHeadersLength = 4;
           // Set credentials to use for this request.
       

           Console.WriteLine("Content length is {0}", response.ContentLength);
           Console.WriteLine("Content type is {0}", response.ContentType);
           Console.WriteLine("Get type is {0}", response.ResponseUri);
           
           Console.WriteLine("File Name: {0}", EndPathFileNam); 











           

        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            bytesIn = int.Parse(e.BytesReceived.ToString());
            string bytesIn_proc,totalBytes_proc;
            bytesIn_proc = SizeSuffix(bytesIn);
             totalBytes = int.Parse(e.TotalBytesToReceive.ToString());
            totalBytes_proc = SizeSuffix(totalBytes);
            double percentage = ((double)bytesIn /(double) totalBytes )* 100;
            lbl.Text = "Downloaded " + bytesIn_proc+ " out of " + totalBytes_proc+ " (" + (int)percentage + "%)";
            progressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString());
           

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
             //Figure out how many bytes where downloaded in the last second.
             var bytesDownloadedLastSecond = bytesIn - lastDownload;
             string bytespersecond;
           //Copy the number over for the next firing of TimerTick.
            lastDownload = bytesIn;

               bytespersecond  =SizeSuffix(bytesDownloadedLastSecond);

               label4.Text = "Download speed:" + bytespersecond + "/s";

          
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        { MessageBox.Show("Download Completed");  }
        
        // Load the form
         private void Form1_Load(object sender, EventArgs e)
        {

            
         }

         
       
            
            















            
    
	}
}
































      


       
   

          