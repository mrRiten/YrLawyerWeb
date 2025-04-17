namespace YrLawyerWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class Client
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<ClientService> ClientServices { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }

    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }

        public ICollection<ClientService> ClientServices { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }

    public class Feedback
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }

    public class ClientService
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public DateTime DateRequested { get; set; }
    }
}
