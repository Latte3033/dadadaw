using System;

namespace WebStock.Models
{
    public class Document
    {
        // กำหนดสถานะเริ่มต้นเป็น "Read"
        public string Status { get; set; } = "Read";

        // Constructor ปกติ
        public Document() { }

        // Constructor ที่กำหนดสถานะเริ่มต้น
        public Document(string initialStatus)
        {
            Status = initialStatus;
        }

        // เมธอดสำหรับอัพเดตสถานะ
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }

        // เมธอดแสดงสถานะ
        public void DisplayStatus()
        {
            Console.WriteLine($"Current Status: {Status}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // สร้าง object ของ Document โดยใช้ค่าเริ่มต้น
            Document doc = new Document();
            Console.WriteLine($"Initial Status: {doc.Status}"); // ควรแสดง "Read"

            // อัพเดตสถานะ
            doc.UpdateStatus("Unread");
            Console.WriteLine($"Updated Status: {doc.Status}"); // ควรแสดง "Unread"

            // ใช้เมธอด UpdateStatus ในการอัพเดต
            doc.UpdateStatus("Read");
            Console.WriteLine($"Final Status: {doc.Status}"); // ควรแสดง "Read"
        }
    }
}
