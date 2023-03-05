using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Domain;
using portal.IO.Common;
using portal.IO.Provider;
using portal.Security.Identity;
using portal.WebAPI.DataTransferObjects;
using System.Data;

namespace portal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditionsController : PortalBaseController
    {
        private readonly PortalDbContext _context;
        private readonly IStorageProvider _storageProvider;
        private ILogger<EditionsController> _logger;

        public EditionsController(PortalDbContext context, IStorageProvider storageProvider, ILogger<EditionsController> logger)
        {
            _context = context;
            _storageProvider = storageProvider;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of editions for a journal.
        /// </summary>
        /// <param name="journalId">The ID of the journal to get the editions for.</param>
        /// <returns>A list of editions for the specified journal.</returns>
        /// <response code="200">Returns the list of editions for the specified journal.</response>
        /// <response code="400">If the specified journal is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize]
        [HttpGet("{journalId}")]
        public async Task<ActionResult<EditionList>> GetEditions(Guid journalId)
        {
            var journal = await _context.Journals.AsNoTracking().Where(x => x.Id == journalId && x.Status == Domain.Entities.RecordStatus.Active)
                            .Include(x => x.Editions.Where(x => x.Status == Domain.Entities.RecordStatus.Active)).FirstOrDefaultAsync();

            if (journal == null)
            {
                _logger.LogWarning($"Invalid journal {journalId} requested by {UserId}.");
                return BadRequest("Invalid Journal.");
            }

            var result = journal.Editions.Select(x => new EditionList
            {
                Id = x.Id.Value,
                JournalId = x.JournalId,
                JournalName = journal.Name,
                CreatedOn = x.CreatedOn
            }).OrderByDescending(x => x.CreatedOn).ToList();

            _logger.LogInformation($"Successfully retrieved {result.Count} active editions of journal {journalId} for {UserId}.");
            return Ok(result);
        }

        /// <summary>
        /// Downloads the file associated with the given edition ID.
        /// </summary>
        /// <param name="editionId">The ID of the edition to download.</param>
        /// <returns>The file associated with the edition ID.</returns>
        [Authorize]
        [HttpGet("{editionId}/download")]
        public async Task<ActionResult<byte[]>> Download(Guid editionId)
        {
            try
            {
                using (var fs = _storageProvider.Download(editionId))
                {
                    using (var ms = new MemoryStream())
                    {
                        await fs.CopyToAsync(ms);
                        return Ok(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading file for edition ID {editionId}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Uploads an edition for a journal.
        /// </summary>
        /// <remarks>
        /// The file size limit is 5MB and only PDF format is supported.
        /// </remarks>
        /// <param name="editionUploadDTO">The edition upload information.</param>
        /// <returns>The uploaded edition information.</returns>
        /// <response code="200">Returns the uploaded edition information.</response>
        /// <response code="400">Returns errors with the request.</response>
        [Authorize(Roles = nameof(Constants.Roles.Publisher))]
        [RequestSizeLimit(5242880)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Domain.Entities.Edition>> PostEdition([FromForm] EditionUpload editionUploadDTO)
        {
            if (_context.Journals.Any(x => x.Id == editionUploadDTO.JournalId && x.Status == Domain.Entities.RecordStatus.Active) == false)
            {
                _logger.LogWarning("Invalid journal with ID {journalId} for edition upload by user {userId}.", editionUploadDTO.JournalId, UserId);

                return BadRequest($"Invalid journal with ID {editionUploadDTO.JournalId} for edition upload by user {UserId}.");
            }

            var readStream = editionUploadDTO.File.OpenReadStream();

            if (readStream.Length > 5242880)
            {
                _logger.LogWarning("Invalid file size for edition upload by user {userId}.", UserId);
                return BadRequest($"Invalid file size for edition upload by user {UserId}.");
            }

            if (FileHelper.VerifyFormat.PDF(readStream) == false)
            {
                _logger.LogWarning("Invalid file format for edition upload by user {userId}.", UserId);
                return BadRequest($"Invalid file format for edition upload by user {UserId}.");
            }

            var edition = new Domain.Entities.Edition()
            {
                JournalId = editionUploadDTO.JournalId,
                CreatedBy = UserId,
                CreatedOn = DateTime.Now
            };

            _context.Editions.Add(edition);
            await _context.SaveChangesAsync();

            try
            {
                _storageProvider.Upload(edition.Id.Value, readStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading edition with ID {editionId} for user {userId}.", edition.Id, UserId);

                edition.Status = Domain.Entities.RecordStatus.Deleted;
                edition.ModifiedBy = "System";
                edition.ModifiedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                return BadRequest($"Error while uploading edition with ID {edition.Id} for user {UserId}.");
            }

            return Ok(edition);
        }
    }
}