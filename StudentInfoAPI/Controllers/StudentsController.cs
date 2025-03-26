using Microsoft.AspNetCore.Mvc;
using StudentInfoAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private static List<Student> _students = new List<Student>(); // Veritabanı yerine geçici liste

        // POST: api/students - Yeni öğrenci ekle
        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Öğrenci bilgileri eksik.");

            // Öğrenci bilgilerini doğrula
            if (!IsValidStudent(student, out string validationError))
                return BadRequest(validationError);

            // Aynı öğrenci numarasına sahip başka bir öğrenci olup olmadığını kontrol et
            if (_students.Any(s => s.StudentNumber == student.StudentNumber))
                return BadRequest("Bu öğrenci numarası zaten kullanılıyor.");

            student.Id = _students.Count + 1; // Basit ID atama
            _students.Add(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // GET: api/students - Tüm öğrencileri listele
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return Ok(_students);
        }

        // PUT: api/students/5 - Öğrenciyi güncelle
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Öğrenci bilgileri eksik.");

            var existingStudent = _students.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
                return NotFound("Öğrenci bulunamadı.");

            // Öğrenci bilgilerini doğrula
            if (!IsValidStudent(student, out string validationError))
                return BadRequest(validationError);

            // Aynı öğrenci numarasına sahip başka bir öğrenci olup olmadığını kontrol et
            if (_students.Any(s => s.StudentNumber == student.StudentNumber && s.Id != id))
                return BadRequest("Bu öğrenci numarası başka bir öğrenci tarafından kullanılıyor.");

            // Öğrenci bilgilerini güncelle
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.StudentNumber = student.StudentNumber;
            existingStudent.ClassName = student.ClassName;

            return NoContent();
        }

        // DELETE: api/students/5 - Öğrenciyi sil
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound("Öğrenci bulunamadı.");

            _students.Remove(student);
            return NoContent();
        }

        // GET: api/students/5 - Belirli bir öğrenciyi getir
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound("Öğrenci bulunamadı.");
            return Ok(student);
        }

        // Öğrenci bilgilerini doğrulayan yardımcı metot
        private bool IsValidStudent(Student student, out string validationError)
        {
            validationError = string.Empty;

            // Ad alanı zorunlu ve sadece harflerden oluşmalı
            if (string.IsNullOrEmpty(student.FirstName) || !Regex.IsMatch(student.FirstName, @"^[a-zA-Z]+$"))
            {
                validationError = "Ad alanı zorunludur ve sadece harflerden oluşmalıdır.";
                return false;
            }

            // Soyad alanı zorunlu ve sadece harflerden oluşmalı
            if (string.IsNullOrEmpty(student.LastName) || !Regex.IsMatch(student.LastName, @"^[a-zA-Z]+$"))
            {
                validationError = "Soyad alanı zorunludur ve sadece harflerden oluşmalıdır.";
                return false;
            }

            // Öğrenci numarası zorunlu ve en fazla 5 haneli olmalı
            if (string.IsNullOrEmpty(student.StudentNumber) || !Regex.IsMatch(student.StudentNumber, @"^\d{1,5}$"))
            {
                validationError = "Öğrenci numarası zorunludur ve en fazla 5 haneli olmalıdır.";
                return false;
            }

            // Sınıf alanı tek haneli bir sayı olmalı
            if (!Regex.IsMatch(student.ClassName, @"^\d$"))
            {
                validationError = "Sınıf alanı tek haneli bir sayı olmalıdır.";
                return false;
            }

            return true;
        }
    }
}
