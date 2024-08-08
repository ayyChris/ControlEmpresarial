using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace ControlEmpresarial.Vistas.Colaborador
{
    public partial class EnviarMensajeColaboradores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializa el DropDownList aquí, si es necesario
            }
        }
        protected async void submit_Click(object sender, EventArgs e) 
        {
            
        }
        protected async void generarPrompt_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Replace with your OpenAI API key
                    var apiKey = "";

                    // Replace with your assistant ID
                    var assistantId = "";

                    // Add the authorization header
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiKey);

                    // Set up the data to send (customize as needed)
                    var data = new
                    {
                        prompt = "Using uploaded documentation, what is the meaning of life?",
                        max_tokens = 50,
                        temperature = 0.5,
                        frequency_penalty = 0.2,
                        presence_penalty = 0.2
                    };

                    // Serialize the data object to JSON
                    var jsonContent = JsonConvert.SerializeObject(data);

                    // Create StringContent with JSON
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Post the content to the assistant endpoint
                    var response = await httpClient.PostAsync($"https://api.openai.com/v1/assistants/{assistantId}/completions", content);

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Display the translation result
                    txtMensaje.Text = responseContent;
                }
            }
            catch (Exception ex)
            {
                txtMensaje.Text = "Error: " + ex.Message;
            }
        }
    }
}

