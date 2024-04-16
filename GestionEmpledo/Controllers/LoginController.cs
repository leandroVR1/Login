using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmpledo.Models;
using GestionEmpledo.Data;
using System.Linq;

namespace GestionEmpledo.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
      public IActionResult Login(Empleado model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    var empleado = _context.Empleados.FirstOrDefault(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);

    if (empleado != null)
    {
        // Autenticación exitosa
        // Guarda información del usuario en la sesión, por ejemplo el Id del usuario
        HttpContext.Session.SetInt32("EmpleadoId", empleado.Id);
        return RedirectToAction("EntradaSalida"); // Cambio aquí
    }
    else
    {
        // Credenciales inválidas
        ModelState.AddModelError("", "Correo o contraseña incorrectos");
        return View(model);
    }
}


        // GET: /Login/EntradaSalida
        public IActionResult EntradaSalida()
        {
            var model = new RegistrosEntrada_Salida();
            return View(model);
        }


        // POST: /Login/EntradaSalida
        [HttpPost]
        public IActionResult EntradaSalida(RegistrosEntrada_Salida model)
        {
            if (ModelState.IsValid)
            {
                // Guarda el registro de entrada y salida en la base de datos
                _context.RegistrosEntrada_Salidas.Add(model);
                _context.SaveChanges();

                // Redirecciona a alguna página de éxito o acción
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si el modelo no es válido, vuelve a mostrar el formulario con errores
                return View(model);
            }
        }
    }
}
