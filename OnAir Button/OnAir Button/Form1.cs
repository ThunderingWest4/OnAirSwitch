using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace OnAir_Button
{
    public partial class Form1 : Form
    {   
        private static readonly HttpClient client = new HttpClient();
        private static string boardID = System.IO.File.ReadAllText(@"S:\Coding\OnAirSwitch\OnAir Button\OnAir Button\secretID.txt");
        private static string token = System.IO.File.ReadAllText(@"S:\Coding\OnAirSwitch\OnAir Button\OnAir Button\secretToken.txt");
        private static Dictionary<string, string> arg = new Dictionary<string, string>
        {
            {"args", "go" },
            {"access_token", token }

        };
        private string url = "https://api.particle.io/v1/devices/"+boardID+"/";
        private string stat = "";

        public Form1()
        {
            InitializeComponent();
            setup();
        }

        private async void setup()
        {

            var response = await client.PostAsync(url+"Status", new FormUrlEncodedContent(arg));
            var responseString = await response.Content.ReadAsStringAsync();
            updateStat(responseString);
            label1.Text = "Status: " + stat;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var response = await client.PostAsync(url+"Toggle", new FormUrlEncodedContent(arg));
            var rtoo = await client.PostAsync(url + "Status", new FormUrlEncodedContent(arg));
            var responseString = await rtoo.Content.ReadAsStringAsync();
            updateStat(responseString);
            label1.Text = "Status: " + stat;
        }
        
        private void updateStat(string responseString)
        {
            if (responseString[responseString.Length - 2] == '0' || responseString[responseString.Length - 2] == 0)
            {
                stat = "Off";
            }
            else if (responseString[responseString.Length - 2] == '1' || responseString[responseString.Length - 2] == 1)
            {
                stat = "On";
            }
            else
            {

                string[] split = responseString.Split(':');
                if(split[split.Length-1].Substring(1, 9) == "Timed out") { stat = "Lost Connection. Is the Board on?";  }
                else { throw new Exception(responseString + split[split.Length - 1].Substring(1, 9)); }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setup();
        }
    }
}
