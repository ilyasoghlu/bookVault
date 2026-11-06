using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using BookVault.Application.DTOs.Borrowings.Requests;
using BookVault.Domain.Entities;

namespace BookVault.Application.Test
{
	public class BorrowingServiceTest
	{
		private readonly Mock<IBorrowingRepository> _borrowingRepository;
		private readonly Mock<IReaderRepository> _readerRepository;
		private readonly Mock<IBookRepository> _bookRepository;

		private readonly BorrowingService _borrowingService;


		public BorrowingServiceTest()
		{
			_borrowingRepository = new Mock<IBorrowingRepository>();
			_readerRepository = new Mock<IReaderRepository>();
			_bookRepository = new Mock<IBookRepository>();

			_borrowingService = new BorrowingService(
				_borrowingRepository.Object,
				_readerRepository.Object,
				_bookRepository.Object);

		}



		[Fact]
		public async Task GetAllAsync_WhenCalled_ReturnsAllBorrowings()
		{
			// Arrange
			var borrowingId = 1;
			var bookId = Guid.NewGuid();
			var readerId = 1;


			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow),
				Returndate = null,

				Bookid = bookId,
				Readerid = readerId
			};

			var borrowings = new List<BookVault.Domain.Entities.Borrowing> { borrowing };

			_borrowingRepository
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(borrowings);

			// Act

			var result = (await _borrowingService.GetAllAsync()).ToList();

			// Assert
			Assert.Single(result);
			Assert.Equal(borrowingId, result[0].Id);
			Assert.Equal(readerId, result[0].Readerid);
			Assert.Equal(bookId, result[0].Bookid);

			// Verfy 

			_borrowingRepository.Verify(
				repo => repo.GetAllAsync(),
				Times.Once);
		}

		[Fact]

		public async Task GetAllAsync_WhenBorrowingDoesNotExist_ReturnsEmptyList()
		{
			// Arrange
			var borrowingId = 1;
			var bookId = Guid.NewGuid();
			var readerId = 1;

			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow),
				Returndate = null,

				Bookid = bookId,
				Readerid = readerId
			};

			var borrowings = new List<BookVault.Domain.Entities.Borrowing> { borrowing };

			_borrowingRepository
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(new List<BookVault.Domain.Entities.Borrowing>());
			// Act
			var result = await _borrowingService.GetAllAsync();
			// Assert

			Assert.Empty(result);
		}



		[Fact]
		public async Task GetByIdAsync_WhenBorrowingExists_ReturnsBorrowingDetailsDto()
		{
			// Arrange
			var borrowingId = 1;
			var bookId = Guid.NewGuid();
			var readerId = 1;

			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow),
				Returndate = null,
				Bookid = bookId,
				Readerid = readerId,
				Book = new BookVault.Domain.Entities.Book { Id = bookId, Title = "Test Book" },
				Reader = new BookVault.Domain.Entities.Reader { Id = readerId, Firstname = "John" }

			};

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync(borrowing);

			// Act
			var result = await _borrowingService.GetByIdAsync(borrowingId);

			// Assert
			Assert.Equal(borrowingId, result.Id);
		}


		[Fact]
		public async Task GetByIdAsync_WhenBorrowingDoesNotExists_ReturnsNull()
		{
			// Arrange
			var borrowingId = 1;

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync((BookVault.Domain.Entities.Borrowing?)null);

			// Act
			var result = await _borrowingService.GetByIdAsync(borrowingId);

			// Assert
			Assert.Null(result);

			_borrowingRepository.Verify(
				repo => repo.GetByIdAsync(borrowingId),
				Times.Once);
		}


		// Create Async Tests

		[Fact]
		public async Task CreateAsync_WhenBorrowAlreadyExist_ThrowsConflictException()
		{
			// Arrange
			var readerId = 1;
			var bookId = Guid.NewGuid();
			var dueDate = DateOnly.FromDateTime(DateTime.UtcNow);

			_borrowingRepository
				.Setup(repo => repo.ExistsActiveBorrowingAsync(readerId, bookId))
				.ReturnsAsync(true);

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(new Reader
				{
					Id = readerId,
					Firstname = "John",
					Lastname = "Doe"
				});

			_bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new Book
				{
					Id = bookId,
					Title = "Test Book"

				});

			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = dueDate
			};
			// Act & Assert
			await Assert.ThrowsAsync<ConflictException>(
				() => _borrowingService.CreateAsync(dto));

			//Verify
			_borrowingRepository.Verify(
				repo => repo.ExistsActiveBorrowingAsync(readerId, bookId),
				Times.Once);

		}

		[Fact]
		public async Task CreateAsync_WhenReaderDoesNotExist_ThrowsNotFoundException()
		{
			// Arrange
			var readerId = 1;
			var bookId = Guid.NewGuid();

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync((Reader?)null);



			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(
				() => _borrowingService.CreateAsync(dto));

			//Verify
			_borrowingRepository.Verify(
				repo => repo.ExistsActiveBorrowingAsync(readerId, bookId),
				Times.Never);

		}



		[Fact]
		public async Task CreateAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
		{
			// Arrange
			var readerId = 1;
			var bookId = Guid.NewGuid();

			_bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync((Book?)null);

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(new Reader
				{
					Id = readerId,
					Firstname = "John",
					Lastname = "Doe"
				});

			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(
				() => _borrowingService.CreateAsync(dto));

			//Verify
			_borrowingRepository.Verify(
				repo => repo.ExistsActiveBorrowingAsync(readerId, bookId),
				Times.Never);
		}

		[Fact]

		public async Task CreateAsync__WhenDueDateIsEarlierThanToday_ThrowsBadRequestException()
		{
			// Arrange

			var dueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1); // Return date earlier than due date
			var readerId = 1;
			var bookId = Guid.NewGuid();

			_bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new Book
				{
					Id = bookId,
					Title = "Test Book"
				});

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(new Reader
				{
					Id = readerId,
					Firstname = "John",
					Lastname = "Doe"
				});

			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = dueDate
			};

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _borrowingService.CreateAsync(dto));

		}


		[Fact]

		public async Task CreateAsync_WhenThisReaderAlredadyHasAnActiveBorrowingForThisBook_ThrowsConflictException()
		{
			// Arrange
			var readerId = 1;
			var bookId = Guid.NewGuid();

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(new Reader
				{
					Id = readerId,
					Firstname = "John",
					Lastname = "Doe"
				});
			_bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new Book
				{
					Id = bookId,
					Title = "Test Book"
				});

			_borrowingRepository
				.Setup(repo => repo.ExistsActiveBorrowingAsync(readerId, bookId))
				.ReturnsAsync(true);

			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			// Act & Assert
			await Assert.ThrowsAsync<ConflictException>(
				() => _borrowingService.CreateAsync(dto));


			// Verify

			_borrowingRepository.Verify(
				repo => repo.ExistsActiveBorrowingAsync(readerId, bookId),
				Times.Once);

		}


		[Fact]

		public async Task CreateAsync_WhenNoAvailableCopies_ThrowsConflictExpectation()

		{
			// Arrange
			var bookId = Guid.NewGuid();
			var readerId = 1;

			_borrowingRepository
				.Setup(repo => repo.ExistsActiveBorrowingAsync(readerId, bookId))
				.ReturnsAsync(false);



			_bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new Book
				{
					Id = bookId,
					Title = "Test Book",
					Totalcopies = 0,
					//Borrowings = new BorrowingCollection
					//{
					//	ActiveCount = 0
					//}
				});

			_readerRepository
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(new Reader
				{
					Id = readerId,
					Firstname = "John",
					Lastname = "Doe"
				});

			var dto = new CreateBorrowingDto
			{
				Readerid = readerId,
				Bookid = bookId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			// Act & Assert
			await Assert.ThrowsAsync<ConflictException>(
				() => _borrowingService.CreateAsync(dto));

		}

		// Update Async Tests



		[Fact]

		public async Task UpdateAsync_WhenBorrowingDoesNotExist_ThrowsNotFoundException()
		{
			// Arrange
			var borrowingId = 1;

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync((BookVault.Domain.Entities.Borrowing?)null);

			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(
				() => _borrowingService.UpdateAsync(borrowingId, new UpdateBorrowingDto
				{
					Returndate = DateOnly.FromDateTime(DateTime.UtcNow)
				}));

			// Verify

			_borrowingRepository
				.Verify(repo => repo.GetByIdAsync(borrowingId),
				Times.Once);
		}

		[Fact]

		public async Task UpdateAsync_WhenDueDateEarlierThanBorrowingCreateDate_ThrowsBadRequestException()
		{

			// Arrange
			var borrowingId = 1;
			var borrowingCreateDate = DateTime.SpecifyKind(
				DateTime.UtcNow, DateTimeKind.Unspecified);

			var dueDate = borrowingCreateDate.AddDays(-1); // Due date earlier than borrowing create date
			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow),
				Createdat = borrowingCreateDate
			};

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync(borrowing);

			// Act & Assert

			await Assert.ThrowsAsync<BadRequestException>(
				() => _borrowingService.UpdateAsync(borrowingId, new UpdateBorrowingDto
				{
					Duedate = DateOnly.FromDateTime(dueDate)
				}));

			// Verify

			_borrowingRepository
				.Verify(repo => repo.GetByIdAsync(borrowingId),
				Times.Once);

		}

		[Fact]

		public async Task UpdateAsync_WhenReturnDateEarlierThanCreateDate_ThrowsBadRequestException()
		{
			//Arrange
			var borrowingId = 1;
			var borrowingCreateDate = DateTime.SpecifyKind(
				DateTime.UtcNow, DateTimeKind.Unspecified);
			var returnDate = borrowingCreateDate.AddDays(-1); // Return date earlier than borrowing create date

			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Duedate = DateOnly.FromDateTime(DateTime.UtcNow),
				Createdat = borrowingCreateDate
			};

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync(borrowing);

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _borrowingService.UpdateAsync(borrowingId, new UpdateBorrowingDto
				{
					Returndate = DateOnly.FromDateTime(returnDate)
				}));

			// Verify
			_borrowingRepository
				.Verify(repo => repo.GetByIdAsync(borrowingId),
				Times.Once);
		}


		[Fact]

		public async Task UpdateAsync_WhenBorrowingAlreadyReturned_ThrowsConflictException()
		{
			// Arrange
			var borrowingId = 1;
			var borrowingCreateDate = DateTime.SpecifyKind(
				DateTime.UtcNow.AddDays(-10),
				DateTimeKind.Unspecified);

			var borrowing = new BookVault.Domain.Entities.Borrowing
			{
				Id = borrowingId,
				Createdat = borrowingCreateDate,
				Duedate = DateOnly.FromDateTime(borrowingCreateDate).AddDays(5),
				Returndate = DateOnly.FromDateTime(borrowingCreateDate).AddDays(3)
			};

			_borrowingRepository
				.Setup(repo => repo.GetByIdAsync(borrowingId))
				.ReturnsAsync(borrowing);

			// Act & Assert
			await Assert.ThrowsAsync<ConflictException>(
				() => _borrowingService.UpdateAsync(
					borrowingId,
					new UpdateBorrowingDto
					{
						Duedate = borrowing.Duedate,
						Returndate = DateOnly.FromDateTime(DateTime.UtcNow)
					}));

		}
	}
}
