using System;

namespace RockContent.Common.Models
{
    public class CreateLikeModel
    {

        ///     The ArticleId of the Article (This is the Foreign Key)
        ///     which is being Liked/Disliked by the Audience
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        ///     The UserId of the User who likes/Dislikes the Particluar Article
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     This is a Toggle property Where TRUE means the User Liked the Post and 
        ///     FALSE means the User Diliked the Post. Therefore Incase If a user likes a post again in future,
        ///     The DB will just update the below Column to TRUE instaed of INSERTING an Entire row
        /// </summary>
        public bool CurrentState { get; set; }
    }


    public class UpdateLikeModel
    {
        /// <summary>
        ///     Each Row in the Table is defined by this Column (This is Unique and IDENTITY COLUMN)
        /// </summary>
        public Guid LikeId { get; set; }


        ///     The ArticleId of the Article (This is the Foreign Key)
        ///     which is being Liked/Disliked by the Audience
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        ///     The UserId of the User who likes/Dislikes the Particluar Article
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     This is a Toggle property Where TRUE means the User Liked the Post and 
        ///     FALSE means the User Diliked the Post. Therefore Incase If a user likes a post again in future,
        ///     The DB will just update the below Column to TRUE instaed of INSERTING an Entire row
        /// </summary>
        public bool CurrentState { get; set; }
    }
}
