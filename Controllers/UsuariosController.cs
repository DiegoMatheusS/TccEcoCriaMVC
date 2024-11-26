using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EcoCriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcoCriaMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient; // Cliente HTTP injetado
        private string uriBase = "http://ecocria.somee.com/Usuarios/";

        public UsuariosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Exibir a página de cadastro de usuário
        [HttpGet]
        public IActionResult Index()
        {
            
            return View("CadastrarUsuario");
        }

        // Registrar um novo usuário
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

        // Logout do usuário
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensagem"] = "Usuário desconectado";
            return RedirectToAction("Index", "Home");
        }

        // Exibir a página de login
        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View("AutenticarUsuario");
        }


        // Autenticar o usuário
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

            // Armazena o nome do usuário na TempData
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


        // Exibir a página de "Esqueci minha senha"
        [HttpGet]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        // Enviar código para o e-mail do usuário
        [HttpPost]
        public async Task<IActionResult> EnviarCodigoParaValidacao(string email)
        {
            var url = $"{uriBase}/EsqueciSenha";  // URL da API de envio do código
            var content = new StringContent(JsonConvert.SerializeObject(new { Email = email }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Código de recuperação enviado para o seu e-mail.";
                return RedirectToAction("ValidarCodigo"); // Redireciona para a página de validação do código
            }

            TempData["MensagemErro"] = "Erro ao enviar o código. Tente novamente.";
            return View();
        }

        // Exibir a página de "Validar código"
        [HttpGet]
        public IActionResult ValidarCodigo()
        {
            return View(); // Exibe a página para inserir o código de validação
        }

        // Validar o código de recuperação
        [HttpPost]
        public async Task<IActionResult> ValidarCodigo(string email, string codigo)
        {
            var url = $"{uriBase}/ValidarCodigo";  // URL da API de validação do código
            var content = new StringContent(JsonConvert.SerializeObject(new { Email = email, Codigo = codigo }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Código validado com sucesso. Agora, altere sua senha.";
                return RedirectToAction("MudandoSenha");
            }

            TempData["MensagemErro"] = "Código inválido ou expirado. Tente novamente.";
            return View();
        }

        // Exibir a página para alterar a senha
        [HttpGet]
        public IActionResult MudandoSenha()
        {
            return View();
        }

        // Alterar a senha do usuário
        [HttpPost]
        public async Task<IActionResult> MudandoSenha(string codigo, string novaSenha, string confirmarSenha)
        {
            var url = $"{uriBase}/MudandoSenha";  // URL da API de mudança de senha
            var content = new StringContent(JsonConvert.SerializeObject(new { Codigo = codigo, NovaSenha = novaSenha, ConfirmarSenha = confirmarSenha }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Senha alterada com sucesso.";
                return RedirectToAction("Login");
            }

            TempData["MensagemErro"] = "Erro ao alterar a senha. Tente novamente.";
            return View();
        }


        
    }
}
