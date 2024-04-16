using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmpledo.Models;
using GestionEmpledo.Data;
using System.Linq;

namespace GestionEmpledo.Controllers
{
    public class LoginController : Controller
    {
        private readonly BaseContext _context;

        public LoginController(BaseContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Empleado model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Empleados.FirstOrDefault(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);
            if (user != null)
            {
                // Autenticación exitosa
                // Guarda información del usuario en la sesión, por ejemplo el Id del usuario
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home"); // Redirecciona a la página de inicio
            }
            else
            {
                // Credenciales inválidas
                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View(model);
            }
        }
    }
}
