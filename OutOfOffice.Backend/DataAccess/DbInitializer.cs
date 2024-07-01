namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(OutOfOfficeDbContext context)
        {
            context.Database.EnsureCreated();            
        }
    }

}
