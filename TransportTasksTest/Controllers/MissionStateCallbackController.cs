using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TransportTasksTest.Controllers
{
    public class MissionStateCallbackController : Controller
    {
        // GET: MissionStateCallbackController
        public string Index()
        {
            return "test";
        }

        // GET: MissionStateCallbackController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MissionStateCallbackController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MissionStateCallbackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MissionStateCallbackController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MissionStateCallbackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MissionStateCallbackController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MissionStateCallbackController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
