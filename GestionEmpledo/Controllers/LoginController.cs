using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmpledo.Models;
using GestionEmpledo.Data;
using System.Linq;
using Microsoft.AspNetCore.Http; // Agrega este using para acceder a HttpContext.Session

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
        public IActionResult Login(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ModelState.AddModelError("", "Por favor, ingrese el correo y la contraseña.");
                return View("Index");
            }

            var empleado = _context.Empleados.FirstOrDefault(u => u.Correo == correo && u.Contraseña == contraseña);

            if (empleado != null)
            {
                // Autenticación exitosa
                // Guarda información del usuario en la sesión, por ejemplo el Id del usuario
                HttpContext.Session.SetInt32("EmpleadoId", empleado.Id);

                // Agrega un registro de depuración para verificar si el ID del empleado se guardó correctamente
                Console.WriteLine($"Empleado ID guardado en la sesión: {empleado.Id}");

                return RedirectToAction("EntradaSalida");
            }
            else
            {
                // Credenciales inválidas
                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View("Index");
            }
        }

        // GET: /Login/EntradaSalida
          public IActionResult EntradaSalida()
    {
        var model = new RegistrosEntrada_Salida();
        return View(model);
    }

    // POST: /Login/GuardarEntradaSalida
    [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult GuardarEntradaSalida(RegistrosEntrada_Salida model)
{
    if (ModelState.IsValid)
    {
        // Obtener el Id del empleado de la sesión
        var empleadoId = HttpContext.Session.GetInt32("EmpleadoId");

        if (empleadoId.HasValue)
        {
            // Asignar el Id del empleado al campo IdEmpleado del modelo
            model.IdEmpleado = empleadoId.Value;

            // Guardar en la base de datos
            _context.RegistrosEntrada_Salida.Add(model);
            _context.SaveChanges();
            
            // Redireccionar a alguna página de éxito o acción
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // No se pudo obtener el Id del empleado de la sesión
            ModelState.AddModelError("", "No se pudo obtener el Id del empleado.");
        }
    }

    // Si el modelo no es válido, vuelve a mostrar el formulario con errores
    return View(model);
}


// GET: /Login/Historial
public IActionResult Historial()
{
    // Obtener todos los registros de entrada y salida de la base de datos
    var registros = _context.RegistrosEntrada_Salida
        .Include(r => r.Empleado) // Incluir información del empleado asociado
        .ToList();

    return View(registros);
}



    }
}
