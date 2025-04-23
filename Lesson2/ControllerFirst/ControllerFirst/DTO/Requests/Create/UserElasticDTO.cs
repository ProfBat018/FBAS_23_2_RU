namespace ControllerFirst.DTO.Requests;

public class UserElasticDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public List<string> Roles { get; set; }
}