var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigin = "_mySpecificOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigin, policy =>
    {
        policy.WithOrigins("http://localhost:5227")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigin);

var Books = new List<BookRecord> {
    new BookRecord(1, "The Johnsons", "Mike Johnson", "9780068120084", "Biography", "/images/books/TheJohnsons.jpg"),
    new BookRecord(2, "To Kill a Mockingbird", "Harper Lee", "9780061120084", "Fiction", "/images/books/ToKillAMockingbird.jpg"),
    new BookRecord(3, "1984", "George Orwell", "9780451524935", "Dystopian", "/images/books/1984.jpg"),
    new BookRecord(4, "The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", "Classics", "/images/books/TheGreatGatsby.jpg"),
    new BookRecord(5, "Harry Potter and the Sorcerer's Stone", "J.K. Rowling", "9780590353427", "Fantasy", "/images/books/HarryPotter.jpg"),
    new BookRecord(6, "The Hobbit", "J.R.R. Tolkien", "9780547928227", "Fantasy", "/images/books/TheHobbit.jpg"),
};

// Get all books
app.MapGet("/books", () =>
{
    return Books;
})
.WithName("GetBooks")
.WithOpenApi();

// Get one book
app.MapGet("/books/{id}", (int id) =>
{
    return Books.SingleOrDefault(b => b.Id == id);
})
.WithName("GetBookById")
.WithOpenApi();

app.MapPost("/books", (HttpContext context, BookRecord book) =>
{
    int newId = Books.Count > 0 ? Books.Max(b => b.Id) + 1 : 1;

    book = book with { Id = newId };
    Books.Add(book);

    return Results.Ok(Books);
})
.WithName("AddBooks")
.WithOpenApi();


app.Run();

record BookRecord(int Id, string Title, string Author, string ISBN, string Genre, string ImageUrl);