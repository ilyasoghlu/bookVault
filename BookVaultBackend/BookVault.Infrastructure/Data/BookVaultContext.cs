using BookVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Data;

public partial class BookVaultContext : DbContext
{
	public BookVaultContext()
	{
	}

	public BookVaultContext(DbContextOptions<BookVaultContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Author> Authors { get; set; }

	public virtual DbSet<Book> Books { get; set; }

	public virtual DbSet<BookCase> BookCases { get; set; }

	public virtual DbSet<Borrowing> Borrowings { get; set; }

	public virtual DbSet<Employee> Employees { get; set; }

	public virtual DbSet<Publisher> Publishers { get; set; }

	public virtual DbSet<Reader> Readers { get; set; }

	public virtual DbSet<Room> Rooms { get; set; }

	public virtual DbSet<Section> Sections { get; set; }

	public virtual DbSet<Shelf> Shelves { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<Rule> Rules { get; set; }

	public virtual DbSet<UserRule> UserRules { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasPostgresEnum(
			"employeestatus",
			new[] { "Active", "Inactive", "Terminated" });

		// =========================================================
		// AUTHOR
		// =========================================================

		modelBuilder.Entity<Author>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Authors_pkey");

			entity.Property(e => e.Id)
				.HasDefaultValueSql("gen_random_uuid()")
				.HasColumnName("id");

			entity.Property(e => e.Firstname)
				.HasMaxLength(30)
				.HasColumnName("firstname");

			entity.Property(e => e.Lastname)
				.HasMaxLength(30)
				.HasColumnName("lastname");

			entity.Property(e => e.Birthdate)
				.HasColumnName("birthdate");

			entity.Property(e => e.Deathdate)
				.HasColumnName("deathdate");

			entity.Property(e => e.Nationality)
				.HasMaxLength(30)
				.HasColumnName("nationality");

			entity.Property(e => e.Biography)
				.HasColumnName("biography");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Publisherid)
				.HasColumnName("publisherid");

			entity.HasOne(e => e.Publisher)
				.WithMany(e => e.Authors)
				.HasForeignKey(e => e.Publisherid)
				.HasConstraintName("fk_author_publisher");
		});

		// =========================================================
		// BOOK
		// =========================================================

		modelBuilder.Entity<Book>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Books_pkey");

			entity.Property(e => e.Id)
				.HasDefaultValueSql("gen_random_uuid()")
				.HasColumnName("id");

			entity.Property(e => e.Isbn)
				.HasMaxLength(36)
				.HasColumnType("character varying(36)")
				.HasColumnName("isbn");

			entity.Property(e => e.Title)
				.HasMaxLength(150)
				.HasColumnName("title");

			entity.Property(e => e.Category)
				.HasMaxLength(50)
				.HasColumnName("category");

			entity.Property(e => e.Genre)
				.HasMaxLength(50)
				.HasColumnName("genre");

			entity.Property(e => e.Publicationdate)
				.HasColumnName("publicationdate");

			entity.Property(e => e.Pagecount)
				.HasColumnName("pagecount");

			entity.Property(e => e.Edition)
				.HasMaxLength(50)
				.HasColumnName("edition");

			entity.Property(e => e.Totalcopies)
				.HasColumnName("totalcopies");

			entity.Property(e => e.Language)
				.HasMaxLength(50)
				.HasColumnName("language");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Authorid)
				.HasColumnName("authorid");

			entity.Property(e => e.Publisherid)
				.HasColumnName("publisherid");

			entity.Property(e => e.Shelfid)
				.HasColumnName("shelfid");

			entity.HasOne(e => e.Author)
				.WithMany(e => e.Books)
				.HasForeignKey(e => e.Authorid)
				.HasConstraintName("fk_book_author");

			entity.HasOne(e => e.Publisher)
				.WithMany(e => e.Books)
				.HasForeignKey(e => e.Publisherid)
				.HasConstraintName("fk_book_publisher_id");

			entity.HasOne(e => e.Shelf)
				.WithMany(e => e.Books)
				.HasForeignKey(e => e.Shelfid)
				.HasConstraintName("fk_book_shelf");


		});

		// =========================================================
		// BOOK CASE
		// =========================================================

		modelBuilder.Entity<BookCase>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("BookCases_pkey");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Casenumber)
				.HasColumnName("casenumber");

			entity.Property(e => e.Capacity)
				.HasColumnName("capacity");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Sectionid)
				.HasColumnName("sectionid");

			entity.HasOne(e => e.Section)
				.WithMany(e => e.BookCases)
				.HasForeignKey(e => e.Sectionid)
				.HasConstraintName("fk_case_section");
		});

		// =========================================================
		// BORROWING
		// =========================================================

		modelBuilder.Entity<Borrowing>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Borrowings_pkey");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Duedate)
				.HasColumnName("duedate");

			entity.Property(e => e.Returndate)
				.HasColumnName("returndate");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Readerid)
				.HasColumnName("readerid");

			entity.Property(e => e.Bookid)
				.HasColumnName("bookid");

			entity.HasOne(e => e.Book)
				.WithMany(e => e.Borrowings)
				.HasForeignKey(e => e.Bookid)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("fk_borrowing_book");

			entity.HasOne(e => e.Reader)
				.WithMany(e => e.Borrowings)
				.HasForeignKey(e => e.Readerid)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("fk_borrowing_reader");
		});

		// =========================================================
		// EMPLOYEE
		// =========================================================

		modelBuilder.Entity<Employee>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("employees_pkey");

            object value = entity.Property(e => e.Id)
				.HasColumnName("id")
				.HasColumnType("integer")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Firstname)
				.HasMaxLength(30)
				.HasColumnName("firstname");

			entity.Property(e => e.Lastname)
				.HasMaxLength(30)
				.HasColumnName("lastname");

			entity.Property(e => e.Email)
				.HasMaxLength(50)
				.HasColumnName("email");

			entity.Property(e => e.Phone)
				.HasMaxLength(20)
				.HasColumnName("phone");

			entity.Property(e => e.Employeeposition)
				.HasMaxLength(30)
				.HasColumnName("employeeposition");

			entity.Property(e => e.Hiredate)
				.HasColumnName("hiredate");

			entity.Property(e => e.Enddate)
				.HasColumnName("enddate");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");


			// for user access
			entity.Property(e => e.UserId)
				.HasColumnName("userId");

			entity.HasIndex(e => e.UserId)
				.IsUnique();

			entity.HasOne(e => e.User)
				.WithOne()
				.HasForeignKey<Employee>(e => e.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("fk_employee_user");

		});

		// =========================================================
		// PUBLISHER
		// =========================================================

		modelBuilder.Entity<Publisher>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Publishers_pkey");

			entity.HasIndex(e => e.Email)
				.HasDatabaseName("Publishers_email_key")
				.IsUnique();

			entity.HasIndex(e => e.Website)
				.HasDatabaseName("Publishers_website_key")
				.IsUnique();

			entity.Property(e => e.Id)
				.HasDefaultValueSql("gen_random_uuid()")
				.HasColumnName("id");

			entity.Property(e => e.Publishername)
				.HasMaxLength(30)
				.HasColumnName("publishername");

			entity.Property(e => e.Address)
				.HasMaxLength(50)
				.HasColumnName("address");

			entity.Property(e => e.City)
				.HasMaxLength(30)
				.HasColumnName("city");

			entity.Property(e => e.Website)
				.HasMaxLength(30)
				.HasColumnName("website");

			entity.Property(e => e.Email)
				.HasMaxLength(50)
				.HasColumnName("email");

			entity.Property(e => e.Phone)
				.HasMaxLength(20)
				.HasColumnName("phone");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");
		});

		// =========================================================
		// READER
		// =========================================================

		modelBuilder.Entity<Reader>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Readers_pkey");

			entity.HasIndex(e => e.Email)
				.HasDatabaseName("Readers_email_key")
				.IsUnique();

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Firstname)
				.HasMaxLength(30)
				.HasColumnName("firstname");

			entity.Property(e => e.Lastname)
				.HasMaxLength(30)
				.HasColumnName("lastname");

			entity.Property(e => e.Email)
				.HasMaxLength(30)
				.HasColumnName("email");

			entity.Property(e => e.Phone)
				.HasMaxLength(20)
				.HasColumnName("phone");

			entity.Property(e => e.Dateofbirth)
				.HasColumnName("dateofbirth");

			entity.Property(e => e.Expirationdate)
				.HasColumnName("expirationdate");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Createdby)
				.HasMaxLength(50)
				.HasColumnName("createdby");

			entity.Property(e => e.Updatedby)
				.HasMaxLength(50)
				.HasColumnName("updatedby");

			// for user access 
			entity.Property(e => e.UserId)
				.HasColumnName("userId");

			entity.HasIndex(e => e.UserId)
				.IsUnique();

			entity.HasOne(e => e.User)
				.WithOne()
				.HasForeignKey<Reader>(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("fk_reader_user");

		});

		// =========================================================
		// ROOM
		// =========================================================

		modelBuilder.Entity<Room>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Rooms_pkey");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Title)
				.HasMaxLength(30)
				.HasColumnName("title");

			entity.Property(e => e.Roomnumber)
				.HasColumnName("roomnumber");

			entity.Property(e => e.Capacity)
				.HasColumnName("capacity");

			entity.Property(e => e.Roomfloor)
				.HasColumnName("roomfloor");

			entity.Property(e => e.Description)
				.HasColumnName("description");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Employeeid)
				.HasColumnName("employeeid");

			entity.HasOne(e => e.Employee)
				.WithMany(e => e.Rooms)
				.HasForeignKey(e => e.Employeeid)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("fk_room_employee");

			entity.HasMany(e => e.Sections)
				.WithOne(e => e.Room)
				.HasForeignKey(e => e.Roomid)
				.HasConstraintName("fk_section_room");
		});

		// =========================================================
		// SECTION
		// =========================================================

		modelBuilder.Entity<Section>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Sections_pkey");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Title)
				.HasMaxLength(30)
				.HasColumnName("title");

			entity.Property(e => e.Sectionnumber)
				.HasColumnName("sectionnumber");

			entity.Property(e => e.Capacity)
				.HasColumnName("capacity");

			entity.Property(e => e.Sectionfloor)
				.HasColumnName("sectionfloor");

			entity.Property(e => e.Description)
				.HasColumnName("description");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Employeeid)
				.HasColumnName("employeeid");

			entity.Property(e => e.Roomid)
				.HasColumnName("roomid");

			entity.HasOne(e => e.Employee)
				.WithMany(e => e.Sections)
				.HasForeignKey(e => e.Employeeid)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("fk_section_employee");

			entity.HasOne(e => e.Room)
				.WithMany(e => e.Sections)
				.HasForeignKey(e => e.Roomid)
				.HasConstraintName("fk_section_room");
		});

		// =========================================================
		// SHELF
		// =========================================================

		modelBuilder.Entity<Shelf>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Shelves_pkey");

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.ValueGeneratedOnAdd()
				.UseIdentityByDefaultColumn();

			entity.Property(e => e.Shelvecode)
				.HasColumnName("shelvecode");

			entity.Property(e => e.Capacity)
				.HasColumnName("capacity");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("createdat");

			entity.Property(e => e.Updatedat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp with time zone")
				.HasColumnName("updatedat");

			entity.Property(e => e.Caseid)
				.HasColumnName("caseid");

			entity.HasOne(e => e.Case)
				.WithMany(e => e.Shelves)
				.HasForeignKey(e => e.Caseid)
				.HasConstraintName("fk_shelve_case");
		});

		// =========================================================
		// USER
		// =========================================================

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Users_pkey");

			entity.HasIndex(e => e.Email)
				.HasDatabaseName("uq_users_email")
				.IsUnique();

			entity.HasIndex(e => e.Username)
				.HasDatabaseName("uq_users_username")
				.IsUnique();

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.HasDefaultValueSql("gen_random_uuid()");

			entity.Property(e => e.Firstname)
				.HasMaxLength(30)
				.HasColumnName("firstName");

			entity.Property(e => e.Lastname)
				.HasMaxLength(30)
				.HasColumnName("lastName");

			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.HasColumnName("email");

			entity.Property(e => e.Username)
				.HasMaxLength(30)
				.HasColumnName("userName");

			entity.Property(e => e.Passwordhash)
				.HasMaxLength(255)
				.HasColumnName("passwordHash");

			entity.Property(e => e.Isactive)
				.HasDefaultValue(true)
				.HasColumnName("isActive");

			entity.Property(e => e.Isdeleted)
				.HasDefaultValue(false)
				.HasColumnName("isDeleted");

			entity.Property(e => e.Createdat)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp without time zone")
				.HasColumnName("createdAt");

			entity.Property(e => e.Updatedat)
				.HasColumnType("timestamp without time zone")
				.HasColumnName("updatedAt");
		});

		// =========================================================
		// RULE
		// =========================================================

		modelBuilder.Entity<Rule>(entity =>
		{
			entity.HasKey(e => e.Id)
				.HasName("Rules_pkey");

			entity.HasIndex(e => e.Code)
				.HasDatabaseName("uq_rules_code")
				.IsUnique();

			entity.Property(e => e.Id)
				.HasColumnName("id")
				.HasDefaultValueSql("gen_random_uuid()");

			entity.Property(e => e.Code)
				.HasMaxLength(100)
				.HasColumnName("code");

			entity.Property(e => e.Rulename)
				.HasMaxLength(150)
				.HasColumnName("ruleName");

			entity.Property(e => e.Description)
				.HasColumnName("description");

			entity.Property(e => e.Isactive)
				.HasDefaultValue(true)
				.HasColumnName("isActive");

			entity.Property(e => e.Createdat)
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnName("createdAt");

			entity.Property(e => e.Updatedat)
				.HasColumnType("timestamp with time zone")
				.HasColumnName("updatedAt");
		});

		// =========================================================
		// USER RULE
		// =========================================================

		modelBuilder.Entity<UserRule>(entity =>
		{
			entity.HasKey(e => new { e.Userid, e.Ruleid })
				.HasName("UserRules_pkey");

			entity.Property(e => e.Userid)
				.HasColumnName("userId");

			entity.Property(e => e.Ruleid)
				.HasColumnName("ruleId");

			entity.Property(e => e.Isallowed)
				.HasDefaultValue(true)
				.HasColumnName("isAllowed");

			entity.Property(e => e.Assignedat)
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnName("assignedAt");

			entity.Property(e => e.Assignedby)
				.HasColumnName("assignedBy");

			entity.Property(e => e.Expiresat)
				.HasColumnType("timestamp with time zone")
				.HasColumnName("expiresAt");

			entity.HasOne(e => e.User)
				.WithMany(e => e.UserRules)
				.HasForeignKey(e => e.Userid)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("fk_userrules_user");

			entity.HasOne(e => e.Rule)
				.WithMany(e => e.UserRules)
				.HasForeignKey(e => e.Ruleid)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("fk_userrules_rule");

			entity.HasOne(e => e.AssignedByUser)
				.WithMany(e => e.AssignedUserRules)
				.HasForeignKey(e => e.Assignedby)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("fk_userrules_assignedby");

			entity.ToTable("UserRules", t =>
			{
				t.HasCheckConstraint(
					"chk_userrules_expiration",
					"\"expiresAt\" IS NULL OR \"expiresAt\" > \"assignedAt\"");
			});
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
