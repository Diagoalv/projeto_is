using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; //Stream
using System.Linq;
using System.Net; //HttpWebRequest
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using RestSharp;

//JavaScriptSerializer --> necessário criar referencia para System.Web.Extensions caso pretendam usar para serializar objetos em JSON

namespace ClientProductsApp
{
    public partial class FormMain : Form
    {

        string baseURI = @"http://localhost:61859/"; //TODO: needs to be updated!
        RestClient client = null;


        public FormMain()
        {
            InitializeComponent();
            client = new RestClient(baseURI);
        }

        private void buttonGetAll_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/products", Method.Get);
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");

            var response = client.Execute<List<Product>>(request).Data;

            richTextBoxShowProducts.Clear();
            foreach (var item in response)
            {
                richTextBoxShowProducts.AppendText($"Id: {item.Id} - {item.Name} \t {item.Category}\n");
            }
            MessageBox.Show("end");
        }

        private void buttonGetProductById_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/products/{id}", Method.Get);
            request.AddUrlSegment("id", textBoxFilterById.Text);
            request.AddHeader("Accept", "application/json");

            //var response = client.Execute<Product>(request).Data;

            //if (response != null)
            //{
            //    textBoxOutput.Text = $"Id: {response.Id} - {response.Name} \t {response.Category} {response.Price}\n";
            //}
            //else
            //    textBoxOutput.Text = "";

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var prod = serializer.Deserialize<Product>(response.Content);
                if (prod != null)
                {
                    textBoxOutput.Text = $"Id: {prod.Id} - {prod.Name} \t {prod.Category} {prod.Price}\n";
                }
                else
                    textBoxOutput.Text = "";
            }


            MessageBox.Show("end");
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            //Product prod = new Product();
            //prod.Id = 0; //atribuido pela BD
            //prod.Name = textBoxName.Text;

            Product prod = new Product { 
                Id = 0,
                Name = textBoxName.Text,
                Category = textBoxCategory.Text,
                Price = Convert.ToDecimal(textBoxPrice.Text)
            };

            RestRequest request = new RestRequest("api/products", Method.Post);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-Type", "application/json");
            request.AddObject(prod);

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                MessageBox.Show("Product created!");
            else
                MessageBox.Show($"Unable to create Product! {response.StatusDescription}");
        }

        private void buttonPut_Click(object sender, EventArgs e)
        {
            Product prod = new Product
            {
                Id = int.Parse(textBoxID.Text),
                Name = textBoxName.Text,
                Category = textBoxCategory.Text,
                Price = Convert.ToDecimal(textBoxPrice.Text)
            };

            RestRequest request = new RestRequest("api/products/{id}", Method.Put);
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("id", textBoxID.Text);
            //request.AddHeader("Content-Type", "application/json");
            request.AddObject(prod);

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                MessageBox.Show("Product updated!");
            else
                MessageBox.Show($"Unable to update Product! {response.StatusDescription}");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/products/{id}", Method.Delete);
            request.AddUrlSegment("id", textBoxID.Text);

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                MessageBox.Show("Product deleted!");
            else
                MessageBox.Show($"Unable to delete Product! {response.StatusDescription}");
        }
    }
}
