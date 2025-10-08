using Microsoft.AspNetCore.Mvc;
using AppWebCP.Data; // Seu using do DbContext
using Microsoft.EntityFrameworkCore;

namespace AppWebCP.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Login/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login/Entrar
        [HttpPost]
        public async Task<IActionResult> Entrar(string email, string senha)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Email == email && f.Senha == senha);

            if (funcionario == null)
            {
                ViewBag.Error = "Email ou senha inválidos!";
                return View("Index");
            }

            // "Loga" o usuário guardando uma informação na sessão
            HttpContext.Session.SetString("UsuarioLogado", funcionario.Nome);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Login/Sair
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}