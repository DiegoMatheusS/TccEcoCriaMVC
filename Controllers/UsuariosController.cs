using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EcoCriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;


namespace EcoCriaMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        private string uriBase = "http://ecocria.somee.com/Usuarios/";

        public UsuariosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


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
                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(uriBase + "Registrar", content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Usuário {0} registrado com sucesso! Faça o login para acessar.", u.NomeUsuario);
                    return View("AutenticarUsuario");
                }
                else
                {
                    throw new Exception(serialized);
                }
            }
            catch (Exception ex)
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
                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(uriBase + "Autenticar", content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Usuario uLogado = JsonConvert.DeserializeObject<Usuario>(serialized);
                    HttpContext.Session.SetString("SessionTokenUsuario", uLogado.Token);
                    TempData["EmailUsuario"] = uLogado.EmailUsuario;


                    TempData["NomeUsuario"] = uLogado.NomeUsuario;

                    TempData["Mensagem"] = string.Format("Bem-vindo {0}!!", uLogado.NomeUsuario);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw new Exception(serialized);
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return IndexLogin();
            }
        }


           /* [HttpGet]
            public IActionResult RecuperarSenha()
            {
                return View();
            }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(Usuario modelo)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(modelo));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(uriBase + "RecuperarSenha", content);

                Console.WriteLine($"Resposta da API: {response.StatusCode}");
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Mensagem: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensagem"] = responseContent; 
                    return RedirectToAction("IndexLogin"); 
                }
                else
                {
                    TempData["MensagemErro"] = responseContent;
                    return View(modelo); 
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro: {ex.Message}";
                return View(modelo); // Retorna à view com erro
            }
        }*/


    }
}

