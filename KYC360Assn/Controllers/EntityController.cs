using System.Linq;
using KYC360Assn.DataAccessLayer;
using KYC360Assn.Models.DBEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KYC360Assn.Models.Request;
using System.Drawing.Printing;
using System.Globalization;
using Serilog;
using KYC360Assn.Utils;

namespace KYC360Assn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly EntityDbContext _context;

        public EntityController(EntityDbContext context)
        {
            _context = context;
        }

        //Sorting, Searching, Pagination, Filtering
        [HttpGet]
        public IActionResult GetEntities(string search=null, string gender=null, DateTime? startDate=null, DateTime? endDate = null, string countries = null, int pageNumber = 1, int pageSize = 10, string sortBy = "Id", string sortOrder = "asc")
        {
            IQueryable<Entity> query = _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Names.Any(n => (n.FirstName + " " + n.MiddleName + " " + n.Surname).Contains(search))
                    || e.Addresses.Any(a => a.Country.Contains(search) || a.AddressLine.Contains(search)));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(e => e.Gender == gender);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.Dates.Any(d => d.DateValue >= startDate && d.DateValue <= endDate));
            }

            if (!string.IsNullOrEmpty(countries))
            {
                List<string> countryList = countries.Split(',').ToList();
                query = query.Where(e => e.Addresses.Any(a => countryList.Contains(a.Country)));
            }

            // Sorting
            switch (sortBy.ToLower())
            {
                case "gender":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Gender) : query.OrderBy(e => e.Gender);
                    break;
                case "startdate":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Dates.Min(d => d.DateValue)) : query.OrderBy(e => e.Dates.Min(d => d.DateValue));
                    break;
                case "enddate":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Dates.Max(d => d.DateValue)) : query.OrderBy(e => e.Dates.Max(d => d.DateValue));
                    break;
                case "country":
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Addresses.Min(a => a.Country)) : query.OrderBy(e => e.Addresses.Min(a => a.Country));
                    break;
                default:
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id);
                    break;
            }

            // Pagination
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var entities = query.ToList();

            return Ok(new
            {
                Entities = entities
            });
        }

        //get by Id
        [HttpGet("{id}")]
        public IActionResult GetEntityById(int id)
        {
            var entity = _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        //Create an entity
        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody] NewEntityRequestBody request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Func<Task<bool>> operation = async () =>
            {
                Entity entity = new Entity { Names = new List<Name>(), Addresses = new List<Address>(), Dates = new List<Date>(), Deceased = request.Deceased, Gender = request.Gender, };
                _context.Entities.Add(entity);

                foreach (NewNamesRequest name in request.Names)
                {
                    _context.Names.Add(new Name { FirstName = name.FirstName, MiddleName = name.MiddleName, Surname = name.Surname, Entity = entity, EntityId = entity.Id});
                }

                foreach (NewAddressesRequest address in request.Addresses)
                {
                    _context.Addresses.Add(new Address { AddressLine = address.AddressLine, City = address.City, Country = address.Country, Entity = entity, EntityId = entity.Id });
                }

                foreach (NewDateRequest date in request.Dates)
                {
                    _context.Dates.Add(new Date { DateType = date.DateType, DateValue = date.DateValue, Entity = entity, EntityId = entity.Id });
                }

                await _context.SaveChangesAsync();
                return true;
            };

            var retryAndBackoff = new RetryAndBackoff();

            bool success = await retryAndBackoff.RetryWriteOperation(operation);

            if (success)
            {
                return Ok("Entity created successfully");
            }
            else
            {
                return StatusCode(500, "Failed to create entity after multiple attempts."); // Return an appropriate error response
            }
        }

        //update an entity
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity(int id, [FromBody] NewEntityRequestBody request)
        {
            if (id != request.Id)
            {
                return BadRequest("Entity ID in the request body does not match the ID in the route.");
            }

            Func<Task<bool>> operation = async () =>
            {
                var entityToUpdate = await _context.Entities
                    .Include(e => e.Addresses)
                    .Include(e => e.Dates)
                    .Include(e => e.Names)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (entityToUpdate == null)
                {
                    return false;
                }

            
                entityToUpdate.Deceased = request.Deceased;
                entityToUpdate.Gender = request.Gender;

            
                _context.Names.RemoveRange(entityToUpdate.Names); 
                entityToUpdate.Names.Clear(); 

                foreach (var nameRequest in request.Names)
                {
                    entityToUpdate.Names.Add(new Name
                    {
                        FirstName = nameRequest.FirstName,
                        MiddleName = nameRequest.MiddleName,
                        Surname = nameRequest.Surname
                    });
                }

            
                _context.Addresses.RemoveRange(entityToUpdate.Addresses);
                entityToUpdate.Addresses.Clear();

                foreach (var addressRequest in request.Addresses)
                {
                    entityToUpdate.Addresses.Add(new Address
                    {
                        AddressLine = addressRequest.AddressLine,
                        City = addressRequest.City,
                        Country = addressRequest.Country
                    });
                }

            
                _context.Dates.RemoveRange(entityToUpdate.Dates);
                entityToUpdate.Dates.Clear();

                foreach (var dateRequest in request.Dates)
                {
                    entityToUpdate.Dates.Add(new Date
                    {
                        DateType = dateRequest.DateType,
                        DateValue = dateRequest.DateValue
                    });
                }

                _context.Entities.Update(entityToUpdate);
                await _context.SaveChangesAsync();

                return true;
            };

            var retryAndBackoff = new RetryAndBackoff();

            bool success = await retryAndBackoff.RetryWriteOperation(operation);

            if (success)
            {
                return Ok("Entity updated successfully");
            }
            else
            {
                return StatusCode(500, "Failed to update entity after multiple attempts."); 
            }
        }

        //remove an entity
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            Func<Task<bool>> operation = async () =>
            {
                var entityToDelete = await _context.Entities.FindAsync(id);

                if (entityToDelete == null)
                {
                    return false;
                }

                _context.Entities.Remove(entityToDelete);
                await _context.SaveChangesAsync();

                return true;
            };

            var retryAndBackoff = new RetryAndBackoff();

            bool success = await retryAndBackoff.RetryWriteOperation(operation);

            if (success)
            {
                return Ok("Entity deleted successfully");
            }
            else
            {
                return StatusCode(500, "Failed to delete entity after multiple attempts.");
            }
        }
    }
}
