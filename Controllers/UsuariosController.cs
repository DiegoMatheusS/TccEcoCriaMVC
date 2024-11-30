using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EcoCriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TccEcoCriaMVC.Message;

namespace EcoCriaMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient; // Cliente HTTP injetado

        private readonly EmailHelper _emailHelper;
        private string uriBase = "http://Testemvc.somee.com/Usuarios/";

        public UsuariosController(HttpClient httpClient, EmailHelper emailHelper)
        {
            _httpClient = httpClient;
            _emailHelper = emailHelper;
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

        [HttpPost]
        public async Task<IActionResult> EnviarCodigoParaValidacao(string email)
        {
            try
            {
                // Criar um objeto Usuario apenas com o e-mail
                var usuario = new Usuario { EmailUsuario = email };

                // Serializar o objeto para enviar para a API
                var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");

                // Enviar a requisição para a API
                var response = await _httpClient.PostAsync(uriBase + "SalvarCodigo", content); // API para salvar o código

                // Verificar se a resposta foi bem-sucedida
                if (!response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["MensagemErro"] = $"Erro ao salvar o código de recuperação: {responseContent}";
                    return View("EsqueciSenha");
                }

                // Gerar o código de recuperação e enviar e-mail
                var codigoRecuperacao = GerarCodigoRecuperacao();
                var mensagem = $"Seu código de recuperação de senha é: {codigoRecuperacao}. Ele expirará em 30 minutos.";

                // Enviar e-mail
                var emailModel = new TccEcoCriaMVC.Models.Email
                {
                    Remetente = "ecocria2024@gmail.com",
                    Destinatario = email,
                    Assunto = "Recuperação de Senha - EcoCria",
                    Mensagem = mensagem
                };

                await _emailHelper.EnviarEmail(emailModel);

                TempData["Mensagem"] = "Código de recuperação enviado para o seu e-mail.";
                return RedirectToAction("ValidarCodigo");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao enviar código de recuperação: {ex.Message}";
                return View("EsqueciSenha");
            }
        }





        // Método para gerar o código de recuperação
        private string GerarCodigoRecuperacao()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Gera um código numérico aleatório de 6 dígitos
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
