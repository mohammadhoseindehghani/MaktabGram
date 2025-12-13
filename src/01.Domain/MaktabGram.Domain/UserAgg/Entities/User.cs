using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.CommentAgg.Entities;
using MaktabGram.Domain.Core.FollowerAgg.Entities;
using MaktabGram.Domain.Core.PostAgg.Entities;
using MaktabGram.Domain.Core.UserAgg.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace MaktabGram.Domain.Core.UserAgg.Entities;
public class User : BaseEntity
{
    #region Ctor
    public User() 
    {
        IsActive = true;
        IsAdmin = false;
    }
    #endregion

    #region properties

    public int IdentityUserId { get; set; }
    public IdentityUser<int> IdentityUser { get; set; }

    public string? Username { get; set; }
    public string PasswordHash { get; set; }
    public Mobile Mobile { get; set; }
    public bool IsActive { get; set; }
    public bool VerifiedBadge { get; private set; }
    public bool IsAdmin { get; set; }

    #endregion

    #region NavigationProperties

    public UserProfile Profile { get; set; }

    public List<Follower> Followers { get; set; }
    public List<Follower> Followings { get; set; }

    public List<Post> Posts { get; set; }
    public List<Comment> Comments { get; set; }
    public List<PostLike> PostLikes { get; set; }
    public List<CommentLike> CommentLikes { get; set; }
    public List<PostSave> SavedPosts { get; set; }
    public List<PostTag> TaggedPosts { get; set; }


    #endregion

    #region Behaviars
    public void Activate() => IsActive = true;
    public void DeActivate() => IsActive = false;
    public void SetVerifiedBadge()
    {
        if (CreatedAt > DateTime.Now.AddYears(-2)
            && IsActive == true
            && Followers.Count > 100000)
        {
            VerifiedBadge = true;
        }
        else
            throw new Exception("شما شرایط کافی برای گرفتن تیک آبی رو ندارید.");
    }
    #endregion
}
