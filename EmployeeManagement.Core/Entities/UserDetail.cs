namespace StockManagement.Core.Entities
{
    public class UserDetail : BaseEntity<Guid>
    {
        public string Nom{ get; set; }
        public string Prenom{ get; set; }
        public string Email{ get; set; }        

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
