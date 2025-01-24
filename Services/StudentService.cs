using Microsoft.EntityFrameworkCore;
using StudentsMinimalApi.Data;
using StudentsMinimalApi.Models;

namespace StudentsMinimalApi.Services;

public class StudentService {
    public static async Task<IResult> GetAllStudents(SchoolDBContext db) {
        return TypedResults.Ok(await db.Students.ToListAsync());
    }

    public static async Task<IResult> GetStudentsBySchool(string school, SchoolDBContext db) {
        var students = await db.Students.Where(t => t.School!.ToLower() == school.ToLower()).ToListAsync();
        return TypedResults.Ok(students);
    }

    public static async Task<IResult> GetStudent(int id, SchoolDBContext db) {
        return await db.Students.FindAsync(id)
        is Student student
            ? Results.Ok(student)
            : Results.NotFound();
    }

    public static async Task<IResult> CreateSttudent(Student student, SchoolDBContext db) {
        db.Students.Add(student);
        await db.SaveChangesAsync();

        return Results.Created($"/students/{student.StudentId}", student);
    }

    public static async Task<IResult> UpdateStudent(int id, Student inputStudent, SchoolDBContext db) {
        var student = await db.Students.FindAsync(id);

        if (student is null) return Results.NotFound();

        student.FirstName = inputStudent.FirstName;
        student.LastName = inputStudent.LastName;
        student.School = inputStudent.School;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteStudent(int id, SchoolDBContext db) {
        if (await db.Students.FindAsync(id) is Student student)
        {
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return TypedResults.Ok(student);
        }

        return TypedResults.NotFound();
    }
}

