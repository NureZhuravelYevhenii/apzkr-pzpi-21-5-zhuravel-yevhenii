namespace VetAutoMobile.Entities.Guards
{
    public readonly record struct Guard(string Name)
    {
        public static Guard LoginRequired { get; } = new Guard("login_required");
        public static Guard OnlyIfLogout { get; } = new Guard("only_logout");
    }
}
