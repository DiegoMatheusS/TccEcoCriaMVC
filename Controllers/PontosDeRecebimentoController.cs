using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EcoCriaMVC.Controllers
{
    public class PontosDeRecebimentoController : Controller
    {
         public IActionResult Pontos()
        {
            return View();
        }
    }
}