using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleProviderService _vehicleProviderService;

        public VehicleController(
            IVehicleService vehicleService,
            IVehicleProviderService vehicleProviderService)
        {
            _vehicleService = vehicleService;
            _vehicleProviderService = vehicleProviderService;
        }

        // ==========================================
        // DISPLAY ALL VEHICLES
        // ==========================================

        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();

            return View(vehicles);
        }


        // ==========================================
        // DISPLAY CREATE VEHICLE PAGE
        // ==========================================

        [Authorize(Roles = "VehicleProvider")]
        public IActionResult Create()
        {
            return View();
        }


        // ==========================================
        // SAVE VEHICLE
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "VehicleProvider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Vehicle vehicle,
            IFormFile? vehicleImage)
        {
            var userId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return RedirectToAction("Create", "VehicleProvider");
            }

            // Automatically assign the logged-in provider
            vehicle.ProviderId = provider.ProviderId;

            // Save uploaded vehicle image
            if (vehicleImage != null && vehicleImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "vehicles"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension =
                    Path.GetExtension(vehicleImage.FileName);

                var fileName =
                    Guid.NewGuid().ToString() + extension;

                var filePath =
                    Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await vehicleImage.CopyToAsync(stream);
                }

                vehicle.ImageUrl =
                    "/images/vehicles/" + fileName;
            }

            if (ModelState.IsValid)
            {
                await _vehicleService.AddVehicleAsync(vehicle);

                return RedirectToAction(nameof(Index));
            }

            return View(vehicle);
        }


        // ==========================================
        // DISPLAY EDIT VEHICLE PAGE
        // ==========================================

        [Authorize(Roles = "VehicleProvider")]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle =
                await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var userId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null ||
                vehicle.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            return View(vehicle);
        }


        // ==========================================
        // UPDATE VEHICLE
        // ==========================================

        [HttpPost]
        [Authorize(Roles = "VehicleProvider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            var userId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return Forbid();
            }

            // Get the existing vehicle from the database
            var existingVehicle =
                await _vehicleService.GetVehicleByIdAsync(id);

            if (existingVehicle == null)
            {
                return NotFound();
            }

            // Make sure the vehicle belongs to the logged-in provider
            if (existingVehicle.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            // Keep the original provider ownership
            vehicle.ProviderId = existingVehicle.ProviderId;

            // Keep the existing image if no new image value is provided
            if (string.IsNullOrEmpty(vehicle.ImageUrl))
            {
                vehicle.ImageUrl = existingVehicle.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateVehicleAsync(vehicle);

                return RedirectToAction(nameof(Index));
            }

            return View(vehicle);
        }


        // ==========================================
        // DISPLAY DELETE CONFIRMATION PAGE
        // ==========================================

        [Authorize(Roles = "VehicleProvider")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle =
                await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var userId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null ||
                vehicle.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            return View(vehicle);
        }


        // ==========================================
        // DELETE VEHICLE
        // ==========================================

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "VehicleProvider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return Forbid();
            }

            // Get the vehicle before deleting it
            var vehicle =
                await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Make sure the vehicle belongs to the logged-in provider
            if (vehicle.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            await _vehicleService.DeleteVehicleAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}