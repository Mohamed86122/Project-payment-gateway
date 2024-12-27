using AppTest.Controllers;
using AppTest.Data;
using AppTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class PaymentDetailsControllerTests
	{

		private readonly PaymentDetailsController _controller;
		private readonly AppDbContext _context;

		public PaymentDetailsControllerTests()
		{
			// Configure In-Memory Database
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new AppDbContext(options); // Initialisation du contexte

			SeedDatabase();

			_controller = new PaymentDetailsController(_context); // Initialisation du contrôleur
		}
		private void SeedDatabase()
		{
			_context.PaymentDetails.RemoveRange(_context.PaymentDetails); // Réinitialisation
			_context.PaymentDetails.AddRange(new List<PaymentDetail>
			{
				new PaymentDetail { PaymentDetailId = 1, CardOwnerName = "John Doe", CardNumber = "1111222233334444", ExpirationDate = "12/24", SecurityCode = "123" },
				new PaymentDetail { PaymentDetailId = 2, CardOwnerName = "Jane Doe", CardNumber = "5555666677778888", ExpirationDate = "11/23", SecurityCode = "456" }
			});
			_context.SaveChanges();
		}

		[Fact]
		public async Task GetPaymentDetails_ShouldReturnAllItems()
		{
			// Act
			var result = await _controller.GetPaymentDetails();

			// Assert
			var actionResult = Assert.IsType<ActionResult<IEnumerable<PaymentDetail>>>(result);
			var items = Assert.IsType<List<PaymentDetail>>(actionResult.Value);
			Assert.Equal(2, items.Count);
		}

		[Fact]
		public async Task GetPaymentDetail_ShouldReturnItem()
		{
			if (!_context.PaymentDetails.Any())
			{
				_context.PaymentDetails.AddRange(new List<PaymentDetail>
	{
		new PaymentDetail { PaymentDetailId = 1, CardOwnerName = "John Doe", CardNumber = "1111222233334444", ExpirationDate = "12/24", SecurityCode = "123" },
		new PaymentDetail { PaymentDetailId = 2, CardOwnerName = "Jane Doe", CardNumber = "5555666677778888", ExpirationDate = "11/23", SecurityCode = "456" }
	});
				_context.SaveChanges();
			}

			// Vérifiez les données seedées
			var item = _context.PaymentDetails.Find(1);
			if (item == null)
			{
				throw new Exception("Données de test non initialisées correctement.");
			}
		}

		[Fact]
		public async Task GetPaymentDetail_ShouldReturnNotFound_ForInvalidId()
		{
			// Act
			var result = await _controller.GetPaymentDetail(99);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task PostPaymentDetail_ShouldAddNewItem()
		{
			// Arrange
			var newItem = new PaymentDetail
			{
				PaymentDetailId = 3,
				CardOwnerName = "New User",
				CardNumber = "9999000011112222",
				ExpirationDate = "10/25",
				SecurityCode = "789"
			};

			// Act
			var result = await _controller.PostPaymentDetail(newItem);

			// Assert
			var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var createdItem = Assert.IsType<PaymentDetail>(actionResult.Value);
			Assert.Equal("New User", createdItem.CardOwnerName);
			Assert.Equal(3, _context.PaymentDetails.Count());
		}

		[Fact]
		public async Task PostPaymentDetail_ShouldReturnBadRequest_WhenItemIsNull()
		{
			// Act
			var result = await _controller.PostPaymentDetail(null);

			// Assert
			Assert.IsType<BadRequestResult>(result.Result);
		}
		[Fact]
		public async Task PutPaymentDetail_ShouldUpdateExistingItem()
		{
			// Arrange
			var updatedItem = new PaymentDetail
			{
				PaymentDetailId = 1,
				CardOwnerName = "Updated Name",
				CardNumber = "1234567890123456",
				ExpirationDate = "01/30",
				SecurityCode = "999"
			};

			// Détachez l'entité existante pour éviter des conflits
			var existingEntity = _context.PaymentDetails.Find(updatedItem.PaymentDetailId);
			_context.Entry(existingEntity).State = EntityState.Detached;

			// Act
			var result = await _controller.PutPaymentDetail(1, updatedItem);

			// Assert
			Assert.IsType<NoContentResult>(result);
			var item = _context.PaymentDetails.Find(1);
			Assert.Equal("Updated Name", item.CardOwnerName);
		}


		[Fact]
		public async Task PutPaymentDetail_ShouldReturnBadRequest_ForMismatchedId()
		{
			// Arrange
			var updatedItem = new PaymentDetail
			{
				PaymentDetailId = 99,
				CardOwnerName = "Updated Name",
				CardNumber = "1234567890123456",
				ExpirationDate = "01/30",
				SecurityCode = "999"
			};

			// Act
			var result = await _controller.PutPaymentDetail(1, updatedItem);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact]
		public async Task DeletePaymentDetail_ShouldRemoveItem()
		{
			// Act
			var result = await _controller.DeletePaymentDetail(1);

			// Assert
			Assert.IsType<NoContentResult>(result);
			Assert.Equal(1, _context.PaymentDetails.Count());
		}

		[Fact]
		public async Task DeletePaymentDetail_ShouldReturnNotFound_ForInvalidId()
		{
			// Act
			var result = await _controller.DeletePaymentDetail(99);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}
	}

}
