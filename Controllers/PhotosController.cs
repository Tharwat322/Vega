using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostEnvironment _host;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVehicleRepository _repository;
        private readonly PhotoSettings photoSettings;
        private readonly IPhotoRepository _photoRepository;

        public PhotosController(IHostEnvironment host, IMapper mapper,
                                IUnitOfWork unitOfWork,
                                IVehicleRepository repository,
                                IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository )
        {
            photoSettings = options.Value;
            _host = host;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _photoRepository = photoRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos  = await _photoRepository.GetPhotos(vehicleId);
            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }


        [HttpPost]
        public async Task<IActionResult> Upload( int vehicleId, IFormFile file )
        {

            var vehicle = await _repository.GetVehicle(vehicleId, includeRelated:false);

            if(vehicle == null)
               return NotFound();

            if(file == null)  return BadRequest("Null file");
            if(file.Length == 0) return BadRequest("Empty file");
            if(file.Length > photoSettings.MaxBytes) return BadRequest("Max file size is exeeded");
            if(!photoSettings.IsSupported(file.FileName))
                 return BadRequest("Invalid file type");
                  
              // Get path folder
            var uploadsFolderPath= Path.Combine(_host.ContentRootPath, "uploads");
              // check if is exist
            if(!Directory.Exists(uploadsFolderPath))
              // if not create one
               Directory.CreateDirectory(uploadsFolderPath);
               //  get file name with its extension 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
              // create new path for this folder 
            var filePath = Path.Combine(uploadsFolderPath, fileName);
               // save in memory
             using( var stream = new FileStream(filePath, FileMode.Create))
             {       
                  await file.CopyToAsync(stream);
             }

             var photo = new Photo
             {
               FileName = fileName
             };

              vehicle.Photos.Add(photo);
              await _unitOfWork.CompleteAsync();


           return Ok(_mapper.Map<Photo, PhotoResource>(photo));
          
        }
    }
}