using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcocriaMVC.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using EcocriaMVC.Models.Enum;
using EcoCriaMVC.Models;

namespace EcocriaMVC.Controllers
{
    public class PontosController : Controller
    {
        public string uriBase = "http://www.ecocria.somee.com/Pontos/";

        [HttpGet]
        public async Task<IActionResult> ListarPontos()
        {
            try
            {
                string uriComplementar = "GetAll";
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Pontos> listaPontos = await Task.Run(() => JsonConvert.DeserializeObject<List<Pontos>>(serialized));
                    return View(listaPontos);
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> Descarte(int idPonto)
        {
            try
            {
                string uriComplementar = $"{idPonto}"; 
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Pontos ponto = JsonConvert.DeserializeObject<Pontos>(serialized); 
                    return View(ponto);
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message; 
                return RedirectToAction("Index"); 
            }
        }


    }
}