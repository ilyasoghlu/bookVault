
using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using System.Security.Cryptography.X509Certificates;

namespace BookVault.Application.Test
{
	public class UnitTest1
	{
		//todo: inmemorydatabase for testing
		//todo: use constructor injection for the repositories and service
		//private readonly Mock<IBookRepository> _bookRepository;
		//private readonly Mock<IAuthorRepository> _authorRepository;
		//private readonly Mock<IPublisherRepository> _publisherRepository;
		//private readonly Mock<IShelfRepository> _shelfRepository;
		//private readonly BookService _service;

		//public UnitTest1()
		//{
		//	_bookRepository = new Mock<IBookRepository>();
		//	_authorRepository = new Mock<IAuthorRepository>();
		//	_publisherRepository = new Mock<IPublisherRepository>();
		//	_shelfRepository = new Mock<IShelfRepository>();

		//	_service = new BookService(
		//		_bookRepository.Object,
		//		_authorRepository.Object,
		//		_publisherRepository.Object,
		//		_shelfRepository.Object);
		//}
		[Fact]
		public async Task GetByIdAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
		{
			// Arrange
			var bookRepository = new Mock<IBookRepository>();
			var bookAuthor = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();


			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);

			var service = new BookService(
				bookRepository.Object,
				bookAuthor.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			//Act & Assert

			await Assert.ThrowsAsync<
				BookVault.Application.Exceptions.NotFoundException>(
					() => service.GetByIdAsync(bookId));
		}

		[Fact]
		public async Task CreateAsync_WhenIsbnAlreadyExists_ThrewConflictExpection()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var isbn = "978-3-16-148410-0";

			var existingBook = new BookVault.Domain.Entities.Book
			{
				Id = Guid.NewGuid(),
				Isbn = isbn,

			};

			bookRepository
				.Setup(repo => repo.GetByIsbnAsync(isbn))
				.ReturnsAsync(existingBook);
			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			var dto = new BookVault.Application.DTOs.Books.Requests.CreateBookDto
			{
				Isbn = isbn,
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = Guid.NewGuid(),
				Publisherid = Guid.NewGuid(),
				Shelfid = 1
			};

			//Act & Assert

			await Assert.ThrowsAsync<
				BookVault.Application.Exceptions.ConflictException>(
					() => service.CreateAsync(dto));

			// Verify added or not new book with the same ISBN
			bookRepository.Verify(
				repo => repo.AddAsync
				(It.IsAny<BookVault.Domain.Entities.Book>()),
				Times.Never);

			// Verify SaveAsync is not called
			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Never);

		}

		[Fact]
		public async Task CreateAsync_WhenDataIsValid_CreateBookAndreturnId()
		{
			//Arrange

			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var authorId = Guid.NewGuid();
			var publisherId = Guid.NewGuid();
			var shelfId = 1;

			bookRepository
				.Setup(repo => repo.GetByIsbnAsync("978-3-16-148410-0"))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);

			authorRepository
				.Setup(repo => repo.GetByIdAsync(authorId))
				.ReturnsAsync(new BookVault.Domain.Entities.Author
				{
					Id = authorId,
					Firstname = "Test",
					Lastname = "Test",
					Birthdate = new DateOnly(1990, 1, 1),
					Nationality = "Test",
					Biography = "Test",
				});
			publisherRepository
				.Setup(repo => repo.GetByIdAsync(publisherId))
				.ReturnsAsync(new BookVault.Domain.Entities.Publisher
				{
					Id = publisherId,
					Publishername = "Test Publisher",
					Email = "test@example.com",
					Address = "Test Address",
					City = "Test City",
					Website = "https://testpublisher.com",


				});
			shelfRepository
				.Setup(repo => repo.GetByIdAsync(shelfId))
				.ReturnsAsync(new BookVault.Domain.Entities.Shelf
				{
					Id = shelfId,
					Shelvecode = 101,
					Capacity = 150,
					Caseid = 1,

				});

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);


			var dto = new BookVault.Application.DTOs.Books.Requests.CreateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId,
				Publisherid = publisherId,
				Shelfid = shelfId
			};


			//Act

			var result = await service.CreateAsync(dto);

			//Assert

			Assert.NotEqual(Guid.Empty, result);

			bookRepository.Verify(
				repo => repo.AddAsync(
					It.Is<BookVault.Domain.Entities.Book>(book =>
					book.Id == result &&
					book.Isbn == dto.Isbn &&
					book.Title == dto.Title &&
					book.Authorid == authorId &&
					book.Publisherid == publisherId &&
					book.Shelfid == shelfId)
					),
				Times.Once);
			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}

		[Fact]
		public async Task CreateAsync_WhenAuthorDoesNotExist_ThrowNotFoundExpection()
		{
			//Arrange

			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var authorId = Guid.NewGuid();
			var publisherId = Guid.NewGuid();
			var shelfId = 1;

			bookRepository
				.Setup(repo => repo.GetByIsbnAsync("978-3-16-148410-0"))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);

			authorRepository
				.Setup(repo => repo.GetByIdAsync(authorId))
				.ReturnsAsync((BookVault.Domain.Entities.Author?)null);


			publisherRepository
				.Setup(repo => repo.GetByIdAsync(publisherId))
				.ReturnsAsync(new BookVault.Domain.Entities.Publisher
				{
					Id = publisherId,
					Publishername = "Test Publisher",
					Email = "test@example.com",
					Address = "Test Address",
					City = "Test City",
					Website = "https://testpublisher.com",


				});
			shelfRepository
				.Setup(repo => repo.GetByIdAsync(shelfId))
				.ReturnsAsync(new BookVault.Domain.Entities.Shelf
				{
					Id = shelfId,
					Shelvecode = 101,
					Capacity = 150,
					Caseid = 1,

				});

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			var dto = new BookVault.Application.DTOs.Books.Requests.CreateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId, // Non-existing author
				Publisherid = publisherId,
				Shelfid = shelfId
			};


			//Act & Assert

			var result = await Assert.ThrowsAsync<BookVault.Application.Exceptions.NotFoundException>(
				() => service.CreateAsync(dto));
		}

		[Fact]

		public async Task CreateAsync_WhenPublilsherDoesNotExist_ThrowNotFoundExpection()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();


			var authorId = Guid.NewGuid();
			var publisherId = Guid.NewGuid();
			var shelfId = 1;

			bookRepository
				.Setup(repo => repo.GetByIsbnAsync("978-3-16-148410-0"))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);

			authorRepository
				.Setup(repo => repo.GetByIdAsync(authorId))
				.ReturnsAsync(new BookVault.Domain.Entities.Author
				{
					Id = authorId,
					Firstname = "Test",
					Lastname = "Test",
					Birthdate = new DateOnly(1990, 1, 1),
					Nationality = "Test",
					Biography = "Test",
				});

			publisherRepository
				.Setup(repo => repo.GetByIdAsync(publisherId))
				.ReturnsAsync((BookVault.Domain.Entities.Publisher?)null);

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);


			var dto = new BookVault.Application.DTOs.Books.Requests.CreateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId,
				Publisherid = publisherId, // Non-existing publisher
				Shelfid = shelfId
			};


			//Act & Assert

			await Assert.ThrowsAsync<BookVault.Application.Exceptions.NotFoundException>(
				() => service.CreateAsync(dto));
		}

		[Fact]

		public async Task CreateAsync_WhenShelfDoesNotExist_ThrowsNotFoundException()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var authorId = Guid.NewGuid();
			var publisherId = Guid.NewGuid();
			var shelfId = 1;

			bookRepository
				.Setup(repo => repo.GetByIsbnAsync("978-3-16-148410-0"))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);

			authorRepository
				.Setup(repo => repo.GetByIdAsync(authorId))
				.ReturnsAsync(new BookVault.Domain.Entities.Author
				{
					Id = authorId,
					Firstname = "Test",
					Lastname = "Test",
					Birthdate = new DateOnly(1990, 1, 1),
					Nationality = "Test",
					Biography = "Test",
				});

			publisherRepository
				.Setup(repo => repo.GetByIdAsync(publisherId))
				.ReturnsAsync(new BookVault.Domain.Entities.Publisher
				{
					Id = publisherId,
					Publishername = "Test Publisher",
					Email = "test@example.com"
				});

			shelfRepository
				.Setup(repo => repo.GetByIdAsync(shelfId))
				.ReturnsAsync((BookVault.Domain.Entities.Shelf?)null);

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			var dto = new BookVault.Application.DTOs.Books.Requests.CreateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId,
				Publisherid = publisherId,
				Shelfid = shelfId // Non-existing shelf
			};


			//Act & Assert		

			await Assert.ThrowsAsync<BookVault.Application.Exceptions.NotFoundException>(
				() => service.CreateAsync(dto));

		}

		[Fact]

		public async Task GetAllAsync_WhenBookExist_ReturnBooks()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();


			bookRepository
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(new List<BookVault.Domain.Entities.Book>
				{
					new BookVault.Domain.Entities.Book
					{
						Id = Guid.NewGuid(),
						Isbn = "978-3-16-148410-0",
						Title = "Test Book 1",
						Category = "Fiction",
						Genre = "Novel",
						Publicationdate = new DateOnly(2023, 1, 1),
						Pagecount = 150,
						Edition = "1st",
						Totalcopies = 10,
						Language = "English",
						Authorid = Guid.NewGuid(),
						Publisherid = Guid.NewGuid(),
						Shelfid = 1
					},
					new BookVault.Domain.Entities.Book
					{
						Id = Guid.NewGuid(),
						Isbn = "978-3-16-148410-1",
						Title = "Test Book 2",
						Category = "Non-Fiction",
						Genre = "Biography",
						Publicationdate = new DateOnly(2022, 5, 15),
						Pagecount = 200,
						Edition = "2nd",
						Totalcopies = 5,
						Language = "English",
						Authorid = Guid.NewGuid(),
						Publisherid = Guid.NewGuid(),
						Shelfid = 2
					}
				});



			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);
			//Act
			var result = await service.GetAllAsync();
			//Assert
			Assert.NotNull(result);

			Assert.Equal(2, result.Count());

			Assert.Equal("Test Book 1", result.First().Title);

			Assert.Equal("Test Book 2", result.Last().Title);


			// Verify 
			bookRepository.Verify(
				repo => repo.GetAllAsync(),
				Times.Once);
		}

		[Fact]
		public async Task GetAllAsync_WhenNoBooksExist_ReturnsEmptyList()

		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var books = new List<BookVault.Domain.Entities.Book>();

			bookRepository
					.Setup(repo => repo.GetAllAsync())
					.ReturnsAsync(books);

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			// Act & Assert
			var result = await service.GetAllAsync();
			Assert.Empty(result);
			
			// Verify

			bookRepository.Verify(
				repo => repo.GetAllAsync(),
				Times.Once);


		}

		[Fact]
		public async Task GetByIdAsync_WhenBookExists_ReturnsBook()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();

			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new BookVault.Domain.Entities.Book
				{
					Id = bookId,
					Isbn = "978-3-16-148410-0",
					Title = "Test Book",
					Category = "Fiction",
					Genre = "Novel",
					Publicationdate = new DateOnly(2023, 1, 1),
					Pagecount = 150,
					Edition = "1st",
					Totalcopies = 10,
					Language = "English",
					Authorid = Guid.NewGuid(),
					Publisherid = Guid.NewGuid(),
					Shelfid = 1
				});

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);


			// Act & Assert
			var result = await service.GetByIdAsync(bookId);
			Assert.NotNull(result);
			Assert.Equal(bookId, result.Id);

			// Verify

			bookRepository.Verify(
				repo => repo.GetByIdAsync(bookId),
				Times.Once);

		}


		[Fact]
		public async Task UpdateAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();

			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);


			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			//Act & Assert
			var dto = new BookVault.Application.DTOs.Books.Requests.UpdateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Updated Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = Guid.NewGuid(),
				Publisherid = Guid.NewGuid(),
				Shelfid = 1
			};
			await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(bookId, dto));

			//Verify
			bookRepository.Verify(
				repo => repo.Update(It.IsAny<BookVault.Domain.Entities.Book>()),
				Times.Never);

			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Never);

		}

		[Fact]
		public async Task UpdateAsync_WhenDataIsValid_UpdatesBook()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();
			var authorId = Guid.NewGuid();
			var publisherId = Guid.NewGuid();
			var shelfId = 1;

			var existingBook = new BookVault.Domain.Entities.Book
			{
				Id = bookId,
				Isbn = "978-3-16-148410-0",
				Title = "Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId,
				Publisherid = publisherId,
				Shelfid = shelfId		
			};


			var dto = new BookVault.Application.DTOs.Books.Requests.UpdateBookDto
			{
				Isbn = "978-3-16-148410-0",
				Title = "Updated Test Book",
				Category = "Fiction",
				Genre = "Novel",
				Publicationdate = new DateOnly(2023, 1, 1),
				Pagecount = 150,
				Edition = "1st",
				Totalcopies = 10,
				Language = "English",
				Authorid = authorId,
				Publisherid = publisherId,
				Shelfid = shelfId	
			};

			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(existingBook);

			authorRepository
				.Setup(repo => repo.GetByIdAsync(authorId))
				.ReturnsAsync(new BookVault.Domain.Entities.Author
				{
					Id = authorId,
					Firstname = "Test",
					Lastname = "Author",
					Birthdate = new DateOnly(1990, 1, 1),
					Nationality = "Test",
					Biography = "Test"
				});

			publisherRepository
				.Setup(repo => repo.GetByIdAsync(publisherId))
				.ReturnsAsync(new BookVault.Domain.Entities.Publisher
				{
					Id = publisherId,
					Publishername = "Test Publisher",
					Email = "test@example.com",
					Address = "Test Address",
					City = "Test City",
					Website = "https://testpublisher.com"
				});

			shelfRepository
				.Setup(repo => repo.GetByIdAsync(shelfId))
				.ReturnsAsync(new BookVault.Domain.Entities.Shelf
				{
					Id = shelfId,
					Shelvecode = 101,
					Capacity = 150,
					Caseid = 1
				});


			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);


			// Act
			await service.UpdateAsync(bookId, dto);
			
			
			// Assert

			Assert.Equal("Updated Test Book", existingBook.Title);
			Assert.Equal(dto.Publisherid, existingBook.Publisherid);
			Assert.Equal(dto.Authorid, existingBook.Authorid);
			Assert.Equal(dto.Shelfid, existingBook.Shelfid);
			
			
			// Verify
			
			bookRepository.Verify(
				repo => repo.GetByIdAsync(bookId),
				Times.Once);


			bookRepository.Verify(
				repo => repo.Update(It.IsAny<BookVault.Domain.Entities.Book>()),
				Times.Once);

			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_WhenBookDoesNotExist_ThrowsNotFoundException()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();

			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync((BookVault.Domain.Entities.Book?)null);


			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);

			//Act & Assert

			await Assert.ThrowsAsync<BookVault.Application.Exceptions.NotFoundException>(
				() => service.DeleteAsync(bookId));

			// Verify

			bookRepository.Verify(
				repo => repo.Delete(It.IsAny<BookVault.Domain.Entities.Book>()),
				Times.Never);

			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Never);
		}



		[Fact]
		public async Task DeleteAsync_WhenBookExists_DeletesBook()
		{
			//Arrange
			var bookRepository = new Mock<IBookRepository>();
			var authorRepository = new Mock<IAuthorRepository>();
			var publisherRepository = new Mock<IPublisherRepository>();
			var shelfRepository = new Mock<IShelfRepository>();

			var bookId = Guid.NewGuid();

			bookRepository
				.Setup(repo => repo.GetByIdAsync(bookId))
				.ReturnsAsync(new BookVault.Domain.Entities.Book
				{
					Id = bookId,
					Isbn = "978-3-16-148410-0",
					Title = "Test Book",
					Category = "Fiction",
					Genre = "Novel",
					Publicationdate = new DateOnly(2023, 1, 1),
					Pagecount = 150,
					Edition = "1st",
					Totalcopies = 10,
					Language = "English",
					Authorid = Guid.NewGuid(),
					Publisherid = Guid.NewGuid(),
					Shelfid = 1
				});

			var service = new BookService(
				bookRepository.Object,
				authorRepository.Object,
				publisherRepository.Object,
				shelfRepository.Object);


			//Act
			await service.DeleteAsync(bookId);

			//Assert
			bookRepository.Verify(
				repo => repo.GetByIdAsync(bookId),
				Times.Once);

			bookRepository.Verify(
				repo => repo.Delete(It.IsAny<BookVault.Domain.Entities.Book>()),
				Times.Once);

			bookRepository.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}

	}
}