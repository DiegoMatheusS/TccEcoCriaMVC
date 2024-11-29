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
using EcoCriaMVC.Models;

namespace EcocriaMVC.Controllers
{
    public class DescarteController : Controller
    {
        public string uriBase = "http://www.ecocria.somee.com/ColetaItens";

        [HttpPost]
        public async Task<IActionResult> RealizarDescarte(ColetaItens coletaItens)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string token = HttpContext.Session.GetString("SessionTokenUsuario");
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var content = new StringContent(JsonConvert.SerializeObject(coletaItens));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await client.PostAsync(uriBase, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Mensagem"] = "Descarte realizado com sucesso!";
                    }
                    else
                    {
                        TempData["Mensagem"] = "Erro ao realizar descarte.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro na requisição: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    
    }
}