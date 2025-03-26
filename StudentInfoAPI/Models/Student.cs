namespace StudentInfoAPI.Models
{
    public class Student
    {
        // Öğrencinin benzersiz kimliği
        public int Id { get; set; }

        // Öğrencinin adı
        public string FirstName { get; set; }

        // Öğrencinin soyadı
        public string LastName { get; set; }

        // Öğrencinin numarası (en fazla 5 haneli)
        public string StudentNumber { get; set; }

        // Öğrencinin sınıfı (tek haneli sayı)
        public string ClassName { get; set; }
    }
}
