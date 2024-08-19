using API.Extensions;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName {get; set;}

    public byte[] PasswordHash {get; set; }

    public byte[] PasswordSalt {get; set; }

    public DateOnly DateOfBirth { get; set; }
    public string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string Gender { get; set; }
    public string Introduction { get; set; }
    public string LookingFor { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public List<Photo> Photos { get; set; } = new();
    public List<UserLike> LikedByUsers { get; set; }
    public List<UserLike> LikedUsers { get; set; }
    
    public List<Message> MessagesSent { get; set;}
    public List<Message> MessagesReceived { get; set;}



    // public int GetAge(){
    //     return DateOfBirth.CalcuateAge();
    // }

}

// i can add it to Entities folder 
// public class Photo
// {
//     public int Id { get; set; }
//     public string Url { get; set; }
//     public bool IsMain { get; set; }
//     public string PublicId { get; set; }
//     public int AppUserId { get; set; }
//     public AppUser AppUser { get; set; }
// }