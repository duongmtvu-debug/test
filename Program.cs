using Microsoft.Data.Sqlite;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAll");

string connStr = "Data Source=qlnv.db";

// ===== CRUD =====

// GET ALL
app.MapGet("/nhanvien", () =>
{
    using var conn = new SqliteConnection(connStr);
    return conn.Query<NhanVien>("SELECT * FROM nhanvien");
});

// GET BY ID
app.MapGet("/nhanvien/{id}", (string id) =>
{
    using var conn = new SqliteConnection(connStr);
    return conn.QueryFirstOrDefault<NhanVien>(
        "SELECT * FROM nhanvien WHERE manv=@id",
        new { id });
});

// ADD
app.MapPost("/nhanvien", (NhanVien nv) =>
{
    using var conn = new SqliteConnection(connStr);
    conn.Execute(
        "INSERT INTO nhanvien (manv, hoten) VALUES (@manv, @hoten)", nv);
    return Results.Ok("Added");
});

// UPDATE
app.MapPut("/nhanvien/{id}", (string id, NhanVien nv) =>
{
    using var conn = new SqliteConnection(connStr);
    conn.Execute(
        "UPDATE nhanvien SET hoten=@hoten WHERE manv=@manv", nv);
    return Results.Ok("Updated");
});

// DELETE
app.MapDelete("/nhanvien/{id}", (string id) =>
{
    using var conn = new SqliteConnection(connStr);
    conn.Execute(
        "DELETE FROM nhanvien WHERE manv=@id", new { id });
    return Results.Ok("Deleted");
});

app.Run();