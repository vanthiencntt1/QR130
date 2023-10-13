using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace DM
{
    public class dmbs
    {
        public string ID { get; set; }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public static readonly string baseURL = "http://localhost:8083/Dmbs";
        // Get All
        public static async Task<string> GetALL()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseURL + "/Dm"))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        return data;
                    }
                }
            }

        }

        // Get By ID 

        public static async Task<string> GetID(string Id)
        {
            using (HttpClient client = new HttpClient()) //DmbyID? Id = 0014
            {
                using (HttpResponseMessage res = await client.GetAsync(baseURL + "/DmbyID?ID=" + Id + ""))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        return data;
                    }
                }
            }

        }
        // Post
        public static async Task<string> PosstID(string Id, string hoten)
        {
            string inputdata = $"{{ \"MA\":\"{Id}\", \"HOTEN\":\"{hoten}\" }}";
            StringContent input = new StringContent(inputdata, Encoding.UTF8, "application/json");


            using (HttpClient client = new HttpClient()) //DmbyID? Id = 0014
            {
                using (HttpResponseMessage res = await client.PostAsync(baseURL + "/PostBS", input))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        return data;
                    }
                }
            }

        }

        //DeleteID
        //public static async Task<string> DeleteID(string Id)
        //{
        //    string inputdata = $"{{ \"MA\":\"{Id}\"}}";
        //     StringContent input = new StringContent(inputdata, Encoding.UTF8, "application/json");
        //    using (HttpClient client = new HttpClient()) //DmbyID? Id = 0014
        //    {

        //        using (HttpResponseMessage res = await client.DeleteAsync(baseURL + "/Delete"))
        //        {
        //            using (HttpContent content = res.Content)
        //            {
        //                string data = await content.ReadAsStringAsync();
        //                return data;
        //            }
        //        }
        //    }

        //}


        public async Task<string> DeleteData(string MA)
        {
            HttpClient client = new HttpClient();
            try
            {
                string inputdata = $"{{ \"MA\":\"{MA}\" }}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(baseURL + "/Delete"),
                    // doi sang kiểu json  JsonConvert.SerializeObject(inputdata)
                    //   Content = new StringContent(JsonConvert.SerializeObject(inputdata), Encoding.UTF8, "application/json")
                    Content = new StringContent(inputdata, Encoding.UTF8, "application/json")
                };

                using (HttpResponseMessage response = await client.SendAsync(request))

                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    response.EnsureSuccessStatusCode(); // Kiểm tra xem có lỗi HTTP không

                    string responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý các lỗi HTTP, mạng và khác nhau có thể xảy ra ở đây
                Console.WriteLine($"Lỗi HTTP: {ex.Message}");
                throw;
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var responce = await GetALL();
            //  dataGridView1 =responce;
            richTextBox1.Text = responce.ToString();


            //Tải dữ liệu lên dataGridView
            //   dataGridView1.DataSource = responce;
            ;
        }

        private async void button2_Click(object sender, EventArgs e)

        {
            string text = txtid.Text.Trim();
            if (String.IsNullOrEmpty(text))

            {

                MessageBox.Show("Vui lòng nhập Id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                var responce = await GetID(text);
                //  dataGridView1 =responce;
                richTextBox1.Text = responce.ToString();
            }

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string textma = txtMa.Text.Trim();
            string texthoten = txtHoTen.Text.Trim();
            if (String.IsNullOrEmpty(textma) || String.IsNullOrEmpty(textma))

            {

                MessageBox.Show("Vui lòng nhập thông tin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                var responce = await PosstID(textma, texthoten);
                //  dataGridView1 =responce;
                richTextBox1.Text = responce.ToString();
            }

        }

        private async void button4_Click(object sender, EventArgs e)
        {

            {
                string text = txtDeleteId.Text.Trim();
                if (String.IsNullOrEmpty(text))

                {

                    MessageBox.Show("Vui lòng nhập Mã bs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    var responce = await DeleteData(text);
                    //  dataGridView1 =responseData;
                    richTextBox1.Text = responce.ToString();


                }

            }
        }





    }
}
