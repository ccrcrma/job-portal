namespace job_portal.Interfaces
{
    public interface ISoftDelete
    {
        public bool IsSoftDeleted { get; set; }
        public void Trash()
        {
            IsSoftDeleted = true;
        }
        public void Restore()
        {
            IsSoftDeleted = false;
        }
    }
}