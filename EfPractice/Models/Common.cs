namespace EfPractice.Models
{
    public static class Common
    {
        public static string GetUserRoleById(int userRole)
        {
            return userRole switch
            {
                1 => "Admin",
                2 => "Manager",
                3 => "Staff",
                _ => "No"
            };
        }
    }
}
