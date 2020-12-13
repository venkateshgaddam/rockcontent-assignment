using System;
using enumCurrentState = RockContent.Common.Models.CurrentState;

namespace RockContent.Common.Models
{
    /// <summary>
    ///     This is the Like Table where eveytime we Like/Dislike a Article, an entry will be Inserted/Updated in this TABLE
    ///     It Contains the in-depth details like who liked the article, timeStamp, the Liked User's UserID and the timestamp as well
    /// </summary>
    public class LikeDbModel
    {

        /// <summary>
        ///     Each Row in the Table is defined by this Column (This is Unique and IDENTITY COLUMN)
        /// </summary>
        public Guid LikeId { get; set; }

        /// <summary>
        ///     The ArticleId of the Article (This is the Foreign Key)
        ///     which is being Liked/Disliked by the Audience
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        ///     The UserId of the User who likes/Dislikes the Particluar Article
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     The Time at which the particular post is liked/Disliked
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        ///     This is a Toggle property Where TRUE means the User Liked the Post and 
        ///     FALSE means the User Diliked the Post. Therefore Incase If a user likes a post again in future,
        ///     The DB will just update the below Column to TRUE instaed of INSERTING an Entire row
        /// </summary>
        public string CurrentState { get; set; } = "UnLiked";


        public LikeDbModel SetArticleId(Guid guid) { ArticleId = guid; return this; }
        
        public LikeDbModel SetUserId(Guid guid) { UserId = guid; return this; }

        public LikeDbModel SetTimeStamp(DateTimeOffset dateTimeOffset) { TimeStamp = dateTimeOffset; return this; }

        public LikeDbModel SetCurrentState(bool CurrentState) 
        {
            if (CurrentState)
            {
                this.CurrentState = enumCurrentState.Liked.ToString();
            }
            else
            {
                this.CurrentState = enumCurrentState.UnLiked.ToString();
            } 
            return this;
        }
    }

    public enum CurrentState
    {
        Liked,
        UnLiked
    }
}
