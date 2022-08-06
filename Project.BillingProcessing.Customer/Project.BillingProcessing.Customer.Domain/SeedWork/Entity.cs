namespace Project.BillingProcessing.Customer.Domain.SeedWork
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateCreation { get; set; }
        public virtual DateTime DateModified { get; set; }
    }
}
