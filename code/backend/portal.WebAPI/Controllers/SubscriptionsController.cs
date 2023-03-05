using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Domain;
using portal.Domain.Entities;

namespace portal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : PortalBaseController
    {
        private readonly PortalDbContext _context;

        private ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(PortalDbContext context, ILogger<SubscriptionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new subscription for the specified journal for the authenticated user.
        /// </summary>
        /// <param name="journalId">The unique identifier of the journal to subscribe to.</param>
        /// <returns>The ID of the newly created subscription.</returns>
        /// <response code="200">Returns the ID of the newly created subscription.</response>
        /// <response code="400">If the specified journal ID is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize(Roles = nameof(portal.Security.Identity.Constants.Roles.User))]
        [HttpPost("{journalId}")]
        public async Task<ActionResult<Subscription>> PostSubscription(Guid journalId)
        {
            var journalExists = _context.Journals.AsNoTracking().Any(x => x.Id == journalId);

            if (journalExists == false)
            {
                _logger.LogWarning("Invalid journal ID {JournalId} provided for subscription", journalId);
                return BadRequest($"Invalid journal ID {journalId} provided for subscription");
            }

            var subscriptionExists = await _context.Subcriptions.AsNoTracking()
                                    .Where(x => x.UserId == UserId && x.JournalId == journalId).FirstOrDefaultAsync();

            if (subscriptionExists != null)
            {
                _logger.LogInformation("Subscription already exists for user {UserId} and journal {JournalId}", UserId, journalId);
                return Ok(subscriptionExists.Id);
            }

            var subscription = new Subscription()
            {
                Id = null,
                JournalId = journalId,
                UserId = UserId,
                CreatedBy = "System",
                CreatedOn = DateTime.Now,
                Status = RecordStatus.Active
            };

            _context.Subcriptions.Add(subscription);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {UserId} subscribed to journal {journalId}");

            return Ok(subscription.Id);
        }

        /// <summary>
        /// Deletes a user's subscription to a journal.
        /// </summary>
        /// <param name="journalId">The ID of the journal to unsubscribe from.</param>
        /// <returns>No content if the subscription was successfully deleted, otherwise NotFound.</returns>
        /// <response code="204">No content if the subscription was successfully deleted.</response>
        /// <response code="404">NotFound if no subscription was found for the specified journal and user.</response>
        [Authorize(Roles = nameof(Security.Identity.Constants.Roles.User))]
        [HttpDelete("{journalId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubscription(Guid journalId)
        {
            _logger.LogInformation("Deleting subscription for user {UserId} and journal {JournalId}", UserId, journalId);

            var subscription = await _context.Subcriptions.Where(x => x.UserId == UserId && x.JournalId == journalId).FirstOrDefaultAsync();

            if (subscription == null)
            {
                _logger.LogInformation("Subscription not found for user {UserId} and journal {JournalId}", UserId, journalId);
                return NotFound();
            }

            subscription.Status = RecordStatus.Deleted;

            _context.Subcriptions.Update(subscription);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Subscription deleted for user {UserId} and journal {JournalId}", UserId, journalId);
            return NoContent();
        }
    }
}