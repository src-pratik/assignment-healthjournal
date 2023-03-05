using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Domain;
using portal.Security.Identity;
using portal.WebAPI.DataTransferObjects;
using System.Data;

namespace portal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : PortalBaseController
    {
        private readonly PortalDbContext _context;
        private readonly SecurityDbContext _contextSecurity;

        private ILogger<JournalsController> _logger;

        public JournalsController(PortalDbContext context, SecurityDbContext securityDbContext, ILogger<JournalsController> logger)
        {
            _context = context;
            _contextSecurity = securityDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new journal.
        /// </summary>
        /// <remarks>
        /// Only users with the 'Publisher' role are authorized to use this endpoint.
        /// </remarks>
        /// <param name="journalDTO">The journal data to create.</param>
        /// <returns>The newly created journal.</returns>
        /// <response code="200">Returns the newly created journal.</response>
        /// <response code="400">If a journal with the same name already exists.</response>
        [Authorize(Roles = nameof(Constants.Roles.Publisher))]
        [HttpPost]
        public async Task<IActionResult> PostJournal(DataTransferObjects.JournalDto journalDTO)
        {
            var userId = UserId;

            _logger.LogDebug($"Request received from {userId}. Request {journalDTO}", userId, journalDTO);

            var journal = journalDTO.FromDTO();

            journal.Name = journal.Name.Trim();
            journal.CreatedBy = userId;

            if (_context.Journals.Any(x => x.Name == journal.Name && x.CreatedBy == journal.CreatedBy))
            {
                _logger.LogInformation($"Journal already exists for {userId} {journal.Name}", userId, journal.Name);
                return BadRequest($"Journal with name {journal.Name} already exists.");
            }

            journal.Id = null;
            journal.CreatedOn = DateTime.Now;
            journal.ModifiedBy = null;
            journal.ModifiedOn = null;

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Journal saved for {userId} {journal.Name}", userId, journal.Name);

            return Ok(journal.ToDTO());
        }

        /// <summary>
        /// Retrieves a list of active journals.
        /// </summary>
        /// <remarks>
        /// Requires authorized user to access this endpoint.
        /// </remarks>
        /// <returns>A list of active journals.</returns>
        /// <response code="200">Returns a list of active journals.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalDetails>>> GetJournals()
        {
            _logger.LogDebug($"Request received from {UserId}.");
            var results = await _context.Journals
                        .AsNoTracking()
                        .Where(x => x.Status == Domain.Entities.RecordStatus.Active)
                        .Select(x => new JournalDetails()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CreatedBy = x.CreatedBy ?? string.Empty,
                            CreatedOn = x.CreatedOn
                        })
                        .ToListAsync();

            var createdByList = results.Select(x => x.CreatedBy).ToList();

            var users = _contextSecurity.Users.AsNoTracking().Where(x => createdByList.Contains(x.Id)).ToDictionary(x => x.Id);

            foreach (var item in results)
            {
                if (users.ContainsKey(item.CreatedBy))
                    item.CreatedBy = users[item.CreatedBy].UserName;
            }

            _logger.LogInformation($"Request processed for {UserId}.");

            return results;
        }

        /// <summary>
        /// Retrieves a list of journals that the user has subscribed to.
        /// </summary>
        /// <remarks>
        /// Only users with the 'User' role are authorized to use this endpoint.
        /// </remarks>
        /// <returns>A list of subscribed journals.</returns>
        /// <response code="200">Returns a list of subscribed journals.</response>
        [Authorize(Roles = nameof(Constants.Roles.User))]
        [HttpGet("subscribed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JournalDetails>>> GetSubscribedJournals()
        {
            _logger.LogDebug($"Request received from {UserId}.");

            var results = await _context.Subcriptions.AsNoTracking()
                                .Where(x => x.UserId == UserId && x.Status == Domain.Entities.RecordStatus.Active && x.Journal.Status == Domain.Entities.RecordStatus.Active)
                                .Include(x => x.Journal).Select(x => new JournalDetails()
                                {
                                    Id = x.Journal.Id,
                                    Name = x.Journal.Name,
                                    CreatedBy = x.Journal.CreatedBy ?? string.Empty,
                                    CreatedOn = x.Journal.CreatedOn
                                }).ToListAsync();

            var createdByList = results.Select(x => x.CreatedBy).ToList();

            var users = _contextSecurity.Users.AsNoTracking().Where(x => createdByList.Contains(x.Id)).ToDictionary(x => x.Id);

            foreach (var item in results)
            {
                if (users.ContainsKey(item.CreatedBy))
                    item.CreatedBy = users[item.CreatedBy].UserName;
            }
            _logger.LogInformation($"Request processed for {UserId}.");
            return Ok(results);
        }

        /// <summary>
        /// Retrieves a list of Journals published by the authenticated user.
        /// </summary>
        /// <returns>A list of published Journals</returns>
        /// <response code="200">Returns the list of published Journals</response>
        /// <response code="401">If the user is not authenticated or does not have the 'Publisher' role</response>
        [Authorize(Roles = nameof(Constants.Roles.Publisher))]
        [HttpGet("published")]
        public async Task<ActionResult<IEnumerable<JournalDetails>>> GetPublishedJournals()
        {
            _logger.LogDebug($"Request received from {UserId} to get published Journals.");

            var results = await _context.Journals.AsNoTracking()
                                .Where(x => x.CreatedBy == UserId && x.Status == Domain.Entities.RecordStatus.Active)
                                .Select(x => new JournalDetails()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    CreatedBy = x.CreatedBy ?? string.Empty,
                                    CreatedOn = x.CreatedOn
                                })
                        .ToListAsync();

            var createdByList = results.Select(x => x.CreatedBy).ToList();

            var users = _contextSecurity.Users.AsNoTracking().Where(x => createdByList.Contains(x.Id)).ToDictionary(x => x.Id);

            foreach (var item in results)
            {
                if (users.ContainsKey(item.CreatedBy))
                    item.CreatedBy = users[item.CreatedBy].UserName;
            }

            _logger.LogInformation($"Request processed for {UserId} to get published Journals.");
            return Ok(results);
        }

        /// <summary>
        /// Retrieves the details of a Journal by ID.
        /// </summary>
        /// <param name="id">The ID of the Journal to retrieve</param>
        /// <returns>The details of the requested Journal</returns>
        /// <response code="200">Returns the details of the requested Journal</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="404">If a Journal with the requested ID is not found</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DataTransferObjects.JournalDto>> GetJournal(Guid? id)
        {
            var journal = await _context.Journals.FindAsync(id);

            if (journal == null)
            {
                _logger.LogWarning($"Journal with ID {id} not found for {UserId}.");
                return NotFound();
            }
            _logger.LogInformation($"Request processed for {UserId} to get Journal with ID {id}.");

            return Ok(journal.ToDTO());
        }
    }
}