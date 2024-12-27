using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppTest.Data;
using AppTest.Model;

namespace AppTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
          if (_context.PaymentDetails == null)
          {
              return NotFound();
          }
            return await _context.PaymentDetails.ToListAsync();
        }

        // GET: api/PaymentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
          if (_context.PaymentDetails == null)
          {
              return NotFound();
          }
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            return paymentDetail;
        }

		// PUT: api/PaymentDetails/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
		{
			if (id != paymentDetail.PaymentDetailId)
			{
				return BadRequest();
			}

			var existingEntity = await _context.PaymentDetails.FindAsync(id);
			if (existingEntity == null)
			{
				return NotFound();
			}

			// Mettre à jour les champs manuellement pour éviter les conflits
			existingEntity.CardOwnerName = paymentDetail.CardOwnerName;
			existingEntity.CardNumber = paymentDetail.CardNumber;
			existingEntity.ExpirationDate = paymentDetail.ExpirationDate;
			existingEntity.SecurityCode = paymentDetail.SecurityCode;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// POST: api/PaymentDetails
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
		{
			if (paymentDetail == null) // Validation ajoutée ici
			{
				return BadRequest();
			}

			if (_context.PaymentDetails == null)
			{
				return Problem("Entity set 'AppDbContext.PaymentDetails' is null.");
			}

			_context.PaymentDetails.Add(paymentDetail);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PaymentDetailId }, paymentDetail);
		}


		// DELETE: api/PaymentDetails/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            if (_context.PaymentDetails == null)
            {
                return NotFound();
            }
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            _context.PaymentDetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentDetailExists(int id)
        {
            return (_context.PaymentDetails?.Any(e => e.PaymentDetailId == id)).GetValueOrDefault();
        }
    }
}
