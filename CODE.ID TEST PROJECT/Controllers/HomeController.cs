using CODE.ID_TEST_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CODE.ID_TEST_PROJECT.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/comments");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<Comment>>(content);

                var totalItems = comments.Count;
                var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

                var paginatedComments = comments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.Page = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;

                return View(paginatedComments);
            }

            return View();
        }
    }
}