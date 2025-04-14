using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AMRApi.Endpoints
{
    public static class MissionStateCallbackEndpoint
    {
        //public static void MapMissionStateCallback(this WebApplication app)
        //{
        //    app.MapPost("/missionStateCallback", async (HttpContext context) =>
        //    {
        //        try
        //        {
        //            // Read the request body
        //            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

        //            // Log the received payload
        //            Console.WriteLine($"Received callback: {requestBody}");

        //            // Respond with a success status
        //            context.Response.StatusCode = StatusCodes.Status200OK;
        //            await context.Response.WriteAsync("Callback received successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle any errors
        //            Console.WriteLine($"Error processing callback: {ex.Message}");
        //            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //            await context.Response.WriteAsync("Error processing callback.");
        //        }
        //    });
        //}
    }
}
