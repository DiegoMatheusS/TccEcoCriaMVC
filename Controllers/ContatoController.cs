using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EcocriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EcocriaMVC.Controllers
{
    public class ContatoController : Controller
    {
        public string uriBase = "http://www.ecocria.somee.com/Contato/";

        [HttpPost]
        public async Task<IActionResult> EnviarAsync(Contato contato)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    var content = new StringContent(JsonConvert.SerializeObject(contato));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);

                    string serialized = await response.Content.ReadAsStringAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        // Usar TempData para armazenar a mensagem de sucesso
                        TempData["Mensagem"] = "Enviado com sucesso";
                    }
                else
                    {
                        TempData["Mensagem"] = "Erro: " + serialized;
                    }
                }
            }
            catch (System.Exception ex)
            {
                TempData["Mensagem"] = "Erro na requisição: " + ex.Message;
                return RedirectToAction("Index");
            }

             return RedirectToAction("Sobre", "Home");
        }
    }
}