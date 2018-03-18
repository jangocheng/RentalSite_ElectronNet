namespace ElectronNet.ApiSettings
{
    /// <summary>
    /// v1版本Url
    /// </summary>
    public class FirstVersion
    {
        /// <summary>
        /// HomeController Url
        /// </summary>
        public HomeSettings Home { get; set; }

        /// <summary>
        /// PermissionController Url
        /// </summary>
        public PermissionSettings Permission { get; set; }

        /// <summary>
        /// RoleController Url
        /// </summary>
        public RoleSettings Role { get; set; }

        /// <summary>
        /// AdminUserController Url
        /// </summary>
        public AdminUserSettings AdminUser { get; set; }

        /// <summary>
        /// CityController Url
        /// </summary>
        public CitySettings City { get; set; }

        /// <summary>
        /// HouseController Url
        /// </summary>
        public HouseSettings House { get; set; }

        /// <summary>
        /// CommunityController Url
        /// </summary>
        public CommunitySettings Community { get; set; }
    }
}
