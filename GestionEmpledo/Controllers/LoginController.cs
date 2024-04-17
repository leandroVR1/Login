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
  public IActionResult GuardarEntradaSalida(RegistrosEntrada_Salida model)
{
    if (ModelState.IsValid)
    {
        // Obtener el Id del empleado de la sesión
        var empleadoId = HttpContext.Session.GetInt32("EmpleadoId");

        if (empleadoId.HasValue)
{
    // Agregar registro de depuración para verificar el Id del empleado
    Console.WriteLine($"Id del Empleado: {empleadoId}");

    // Asignar el Id del empleado al campo IdEmpleado del modelo
    model.IdEmpleado = empleadoId.Value;

    // Guardar en la base de datos
    _context.RegistrosEntrada_Salida.Add(model);
    _context.SaveChanges();
    
    // Redireccionar a la vista "Historial"
    return RedirectToAction("Historial");
}
        else
        {
            // No se pudo obtener el Id del empleado de la sesión
            ModelState.AddModelError("", "No se pudo obtener el Id del empleado.");
        }
    }

    // Si el modelo no es válido o si falla la obtención del Id del empleado, vuelve a mostrar el formulario con errores
    return View("EntradaSalida", model);
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

// GeT: /Login/Empleados
public IActionResult Empleados()
{
    // Obtener todos los empleados de la base de datos
    var empleados = _context.Empleados
       .ToList();

    return View(empleados);
}


//Post: /Login/Create


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction("Empleados");
            }
            return View(empleado);
        }


    }
}
