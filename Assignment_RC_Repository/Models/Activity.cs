using System;

namespace RockContent.Common.Models
{
    /// <summary>
    ///     This is the Acticity Table where we will store the activities (i.e. Likes, Comments) Count.
    ///     Not only in this DB but also we will store the data of this activity in the REDIS with the KEY "RockContent|Article|{ArticleID}|TotalLikes/Comments" 
    ///     which makes our Fetch more faster
    ///     How Data inserted into the TABLE :
    ///             Whenever an entry is INSERTED Or UPDATED we will put two DB Triggers for both the DML Operations.
    ///             INSERT Trigger:- Once this is triggered, We will increase the Activity table Data by 1 (or we will insert the Data with the Count as 1 (If it's a First Like))
    ///             Update Trigger:- Update the Activity Table by Reducing the Count by 1 Once the Like Table gets Updated
    /// </summary>
    public class Activity
    {
        /// <summary>
        ///     The row in the Table is defined by this ID
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        ///     Either LIKE or COMMENT
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        ///     The ArticleId of the particular Article.
        ///     Also, this is the PrimaryKey and we will create a CLustered-INDEX
        /// </summary>
        public Guid ArticleId { get; set; }

        /// <summary>
        ///     This is the Total Count of the Likes/Comments
        /// </summary>
        public long Count { get; set; }
    }
}
