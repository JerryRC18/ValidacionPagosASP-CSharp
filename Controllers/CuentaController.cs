
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using SSP2.Models;
using SSP2.Data;
using AspNetCore.ReCaptcha;

namespace SSP2.Controllers
{

    public class CuentaController : Controller
    {
        private readonly Context _contexto;
       
        public CuentaController(Context contexto)
        {
            _contexto = contexto;
        }

        
        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home", "Folios");
            }
            return View();
        }

        [ValidateReCaptcha]
        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("sp_validar_usuario", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = u.UserName;
                        cmd.Parameters.Add("@Clave", System.Data.SqlDbType.VarChar).Value = u.Clave;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr["UserName"] != null && u.UserName != null && ModelState.IsValid)
                            {
                                List<Claim> c = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, u.UserName)
                                };
                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();

                                p.AllowRefresh = true;
                                p.IsPersistent = u.MantenerActivo;

                                if (!u.MantenerActivo)
                                {
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5);
                                }
                                else
                                {
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
                                }
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                return RedirectToAction("Index", "Home", "Folios");
                            }
                            else
                            {
                                ViewBag.Error = "Credenciales incorrectas o cuenta no registrada";
                            }
                        }
                        con.Close();
                    }
                    return View();
                }
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

