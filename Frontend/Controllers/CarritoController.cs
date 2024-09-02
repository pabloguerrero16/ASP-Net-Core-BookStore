using Azure.Core;
using Frontend.Models;
using Frontend.Models.Paypal_Order;
using Frontend.Models.Paypal_Transaction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Frontend.Controllers
{
    public class CarritoController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public CarritoController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IActionResult> Carrito()
        {

            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }
            if (user == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            long usuarioId = user.Id;

            var response = await _httpClient.GetAsync($"https://localhost:7281/api/Carrito/ConsultarCarrito?q={usuarioId}");
            var carritoData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<CarritoViewModel>>(carritoData);

            if (data == null || !data.Any())
            {
                ViewData["EmptyCartMessage"] = "Tu carrito está vacío.";
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult EliminarProductoCarrito(long carritoId)
        {
            var response = _httpClient.DeleteAsync($"https://localhost:7281/api/Carrito?q={carritoId}").Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Carrito");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> registrarCarrito(int productoId, int cantidad, DateTime fechaCarrito)
        {
            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }

            if (user == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var carrito = new Carrito
            {
                UsuarioId = user.Id,
                ProductoId = productoId,
                Cantidad = cantidad,
                FechaCarrito = fechaCarrito
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7281/api/Carrito/RegistrarCarrito", carrito);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar el producto al carrito.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> Paypal([FromBody] PaypalRequest request)
        {
            bool status = false;
            string respuesta = string.Empty;

            using (var client = new HttpClient())
            {
                var userName = _config["PayPal:userName"];
                var passwd = _config["PayPal:passwd"];

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var orden = new PaypalOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<Models.Paypal_Order.PurchaseUnit>()
            {
                new Models.Paypal_Order.PurchaseUnit()
                {
                    amount = new Models.Paypal_Order.Amount(){currency_code="USD", value=request.Precio},
                    descripction = "Compra en Libreria Antigua"
                }
            },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "Libreria Antigua",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://localhost:7105/Carrito/PageSuccess",
                        cancel_url = "https://localhost:7105/"
                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }

            }

            return Json(new { status = status, respuesta = respuesta });
        }


        [HttpGet]
        public async Task<IActionResult> PageSuccess()
        {
            string token = Request.Query["token"];
            bool status = false;

            using (var client = new HttpClient())
            {
                var userName = _config["PayPal:userName"];
                var passwd = _config["PayPal:passwd"];

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);


                status = response.IsSuccessStatusCode;
                ViewData["Status"] = status;

                if (status)
                {
                    var jsonRespuesta = response.Content.ReadAsStringAsync().Result;
                    PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonRespuesta);

                    ViewData["IdTransaccion"] = objeto.purchase_units[0].payments.captures[0].id;
                }

            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> PagarCarrito(long usuarioId)
        {
            var userBytes = HttpContext.Session.Get("User");
            Usuario user = null;
            if (userBytes != null)
            {
                var userJson = Encoding.UTF8.GetString(userBytes);
                user = JsonConvert.DeserializeObject<Usuario>(userJson);
            }
            try
            {
                var carrito = new Carrito { UsuarioId = user.Id };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7281/api/Carrito/PagarCarrito", carrito);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo eliminar el carrito.");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error: {ex.Message}");
                return View("Error");
            }
        }
    }
}
