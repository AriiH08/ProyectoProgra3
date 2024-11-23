using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using ClinicaMedica.Data.Models;
using System.Data;

public class AccesoController : Controller
{
    private const string connectionString = "Data Source=DESKTOP-EI0DHR3\\SQLEXPRESS;Initial Catalog=ClinicaMedica;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";

    // Página de Login
    public IActionResult Login()
    {
        // Verificar si ya hay un usuario logueado
        if (HttpContext.Session.GetString("USUARIO") != null)
        {
            var rolId = HttpContext.Session.GetInt32("RolId");
            if (rolId == 1)
                return RedirectToAction("Index", "Admin");
            else if (rolId == 2)
                return RedirectToAction("Index", "Citas");

            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // Página de Registro
    public IActionResult Registrar()
    {
        return View();
    }

    // Procesar Login
    [HttpPost]
    public IActionResult Login(Usuario usuario)
    {
        usuario.Clave = ConvertirSha256(usuario.Clave);

        try
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("Clave", usuario.Clave);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario.UsuarioId = Convert.ToInt32(reader["UsuarioId"]);
                        usuario.RolId = Convert.ToInt32(reader["RolId"]);
                    }
                }
            }

            if (usuario.UsuarioId != 0)
            {
                // Guardar datos en la sesión
                HttpContext.Session.SetString("USUARIO", usuario.Correo);
                HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
                HttpContext.Session.SetInt32("RolId", usuario.RolId);

                // Depuración
                Console.WriteLine($"Login exitoso: UsuarioId={usuario.UsuarioId}, RolId={usuario.RolId}");

                // Redirigir según el rol
                if (usuario.RolId == 1) // SuperUsuario
                    return RedirectToAction("Index", "Admin");
                else if (usuario.RolId == 2) // Cliente
                    return RedirectToAction("Index", "Citas");

                return RedirectToAction("Index", "Home"); // Redirección genérica
            }
            else
            {
                ViewData["Mensaje"] = "Credenciales incorrectas.";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewData["Mensaje"] = $"Error: {ex.Message}";
            return View();
        }
    }

    // Procesar Registro
    [HttpPost]
    public IActionResult Registrar(Usuario oUsuario)
    {
        bool registrado = false;
        string mensaje = "";

        // Validar que las contraseñas coincidan
        if (oUsuario.Clave != oUsuario.ConfirmarClave)
        {
            ViewData["Mensaje"] = "Las contraseñas no coinciden.";
            return View();
        }

        oUsuario.Clave = ConvertirSha256(oUsuario.Clave); // Hash de la contraseña

        try
        {
            // Asignar el rol automáticamente si no hay un superusuario logueado
            var usuarioLogueadoRol = HttpContext.Session.GetInt32("RolId");
            if (usuarioLogueadoRol == null || usuarioLogueadoRol != 1)
            {
                oUsuario.RolId = 2; // Cliente por defecto
            }

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.Parameters.AddWithValue("RolId", oUsuario.RolId); // Registrar con el rol asignado
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewData["Mensaje"] = $"Error: {ex.Message}";
            return View();
        }
    }

    // Cerrar Sesión
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Limpiar la sesión
        return RedirectToAction("Login", "Acceso");
    }

    // Método para convertir texto a SHA256
    private static string ConvertirSha256(string texto)
    {
        StringBuilder sb = new StringBuilder();
        using (SHA256 hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(texto));

            foreach (byte b in result)
                sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
