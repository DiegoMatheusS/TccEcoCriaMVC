using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EcoCriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcoCriaMVC.Controllers
{
    public class UsuariosController : Controller
    {
        public string uriBase = "http://ecocria.somee.com/Usuarios/";

        [HttpGet]
        public IActionResult Index()
        {
            return View("CadastrarUsuario");
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarAsync(Usuario u)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Registrar";

                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] =
                        string.Format("Usuário {0} Registrado com sucesso! Faça o login para acessar.", u.NomeUsuario);
                    return View("AutenticarUsuario");
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
        public IActionResult Logout()
    {
    
        HttpContext.Session.Clear();
        TempData["Mensagem"] = "Usuário desconectado";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
        public ActionResult IndexLogin()
        {
            return View("AutenticarUsuario");
        }

        [HttpPost]
        public async Task<ActionResult> AutenticarAsync(Usuario u)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Autenticar";

                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Usuario uLogado = JsonConvert.DeserializeObject<Usuario>(serialized);
                    HttpContext.Session.SetString("SessionTokenUsuario", uLogado.Token);
                    TempData["Mensagem"] = string.Format("Bem-Vindo {0}!!", uLogado.NomeUsuario);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return IndexLogin();
            }
        }









    }
    
}