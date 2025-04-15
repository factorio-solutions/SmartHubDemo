using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportTasksTest.OPCUAModule;

namespace TransportTasksTest.Controllers
{
    public class MissionStateCallbackController : Controller
    {


        public MissionStateCallbackController()
        {

        }


        // GET: MissionStateCallbackController
        public async Task<IActionResult> Index()
        {
            try
            {
                // Read the request body
                var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

                // Log the received payload
                Console.WriteLine($"Received callback: {requestBody}");

                OPCMonitor.ProcessCallback(requestBody);

                // Pass the enabled state to the view
                ViewBag.IsButtonEnabled = OPCMonitor.BoxCreated;

                // Respond with a success status
                Response.StatusCode = StatusCodes.Status200OK;

                return View();
            }
            catch (Exception ex)
            {
                // Handle any errors
                ViewBag.IsButtonEnabled = false; // Default to disabled in case of error
                return View();
            }
        }

        public ActionResult Go()
        {
            AMRModule.AMRCommands.FillUpBox();         
            AMRModule.AMRCommands.StartMission();
            return View("Go");
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
